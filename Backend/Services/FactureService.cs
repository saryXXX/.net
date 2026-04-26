using Backend.Common;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public interface IFactureService
    {
        Task<Result<IEnumerable<Facture>>> GetAllAsync();
        Task<Result<Facture>> GetByIdAsync(int id);
        Task<Result<Facture>> CreateFactureAsync(Facture facture);
        Task<Result> ValidateFactureAsync(int id);
        Task<Result> SoftDeleteAsync(int id);
    }

    public class FactureService : IFactureService
    {
        private readonly ApplicationDbContext _context;

        public FactureService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Facture>>> GetAllAsync()
        {
            var factures = await _context.Factures
                .Include(f => f.Client)
                .Include(f => f.Lignes)
                    .ThenInclude(l => l.Produit)
                .ToListAsync();
            return Result<IEnumerable<Facture>>.Success(factures);
        }

        public async Task<Result<Facture>> GetByIdAsync(int id)
        {
            var facture = await _context.Factures
                .Include(f => f.Client)
                .Include(f => f.Lignes)
                    .ThenInclude(l => l.Produit)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (facture == null) return Result<Facture>.Failure("Invoice not found.");
            return Result<Facture>.Success(facture);
        }

        public async Task<Result<Facture>> CreateFactureAsync(Facture facture)
        {
            try
            {
                // 1. Get Fiscal Parameters (Current Timbre)
                var fiscalParams = await _context.ParametresFiscaux
                    .Where(p => p.DateDebut <= DateTime.UtcNow && (p.DateFin == null || p.DateFin >= DateTime.UtcNow))
                    .OrderByDescending(p => p.DateDebut)
                    .FirstOrDefaultAsync();

                facture.TimbreFiscal = fiscalParams?.TimbreFiscal ?? 1.000m;

                // 2. Calculate Totals
                facture.TotalHT = 0;
                facture.TotalTVA = 0;

                foreach (var ligne in facture.Lignes)
                {
                    // Ensure we have latest product info if needed, or assume UI sends correct snapshot
                    ligne.MontantHT = ligne.Quantite * ligne.PrixUnitaireHT;
                    ligne.MontantTVA = ligne.MontantHT * (ligne.TauxTVA / 100);
                    ligne.MontantTTC = ligne.MontantHT + ligne.MontantTVA;

                    facture.TotalHT += ligne.MontantHT;
                    facture.TotalTVA += ligne.MontantTVA;
                }

                facture.TotalTTC = facture.TotalHT + facture.TotalTVA + facture.TimbreFiscal;

                // 3. Generate Invoice Number if empty (Format: FACT-YYYY-MM-XXXX)
                if (string.IsNullOrWhiteSpace(facture.Numero))
                {
                    var count = await _context.Factures.CountAsync(f => f.DateFacture.Year == DateTime.UtcNow.Year) + 1;
                    facture.Numero = $"FACT-{DateTime.UtcNow:yyyy-MM}-{count:D4}";
                }

                _context.Factures.Add(facture);
                await _context.SaveChangesAsync();

                return Result<Facture>.Success(facture);
            }
            catch (Exception ex)
            {
                return Result<Facture>.Failure(ex.Message);
            }
        }

        public async Task<Result> ValidateFactureAsync(int id)
        {
            var facture = await _context.Factures.Include(f => f.Lignes).FirstOrDefaultAsync(f => f.Id == id);
            if (facture == null) return Result.Failure("Invoice not found.");

            if (facture.Statut == "Validée") return Result.Failure("Invoice already validated.");

            facture.Statut = "Validée";
            
            // Phase 3: Stock Decrement will be hooked here
            
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> SoftDeleteAsync(int id)
        {
            var facture = await _context.Factures.FindAsync(id);
            if (facture == null) return Result.Failure("Invoice not found.");

            if (facture.Statut == "Validée" || facture.Statut == "Payée")
                return Result.Failure("Cannot delete a validated or paid invoice.");

            facture.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
