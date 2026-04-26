using Backend.Common;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public interface IProduitService
    {
        Task<Result<IEnumerable<Produit>>> GetAllAsync();
        Task<Result<Produit>> GetByIdAsync(int id);
        Task<Result<Produit>> CreateAsync(Produit produit);
        Task<Result> UpdateAsync(Produit produit);
        Task<Result> SoftDeleteAsync(int id);
    }

    public class ProduitService : IProduitService
    {
        private readonly ApplicationDbContext _context;

        public ProduitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Produit>>> GetAllAsync()
        {
            var produits = await _context.Produits.ToListAsync();
            return Result<IEnumerable<Produit>>.Success(produits);
        }

        public async Task<Result<Produit>> GetByIdAsync(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) return Result<Produit>.Failure("Product not found.");
            return Result<Produit>.Success(produit);
        }

        public async Task<Result<Produit>> CreateAsync(Produit produit)
        {
            try
            {
                _context.Produits.Add(produit);
                await _context.SaveChangesAsync();
                return Result<Produit>.Success(produit);
            }
            catch (Exception ex)
            {
                return Result<Produit>.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateAsync(Produit produit)
        {
            try
            {
                _context.Entry(produit).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> SoftDeleteAsync(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) return Result.Failure("Product not found.");

            produit.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
