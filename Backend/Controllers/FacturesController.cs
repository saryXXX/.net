using Backend.Models;
using Backend.Services;
using Dashboard.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FacturesController : ControllerBase
    {
        private readonly IFactureService _factureService;

        public FacturesController(IFactureService factureService)
        {
            _factureService = factureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _factureService.GetAllAsync();
            if (!result.IsSuccess || result.Value == null) return BadRequest(result.Error);

            var dtos = result.Value.Select(f => new InvoiceDto
            {
                Id = f.Id,
                Numero = f.Numero,
                DateFacture = f.DateFacture,
                ClientId = f.ClientId,
                ClientName = f.Client != null ? $"{f.Client.Prenom} {f.Client.Nom}" : "Unknown",
                ClientMatricule = f.Client?.MatriculeFiscal,
                TotalHT = f.TotalHT,
                TotalTVA = f.TotalTVA,
                TotalTTC = f.TotalTTC,
                TimbreFiscal = f.TimbreFiscal,
                Statut = f.Statut
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _factureService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Value == null) return NotFound(result.Error);

            var f = result.Value;
            var dto = new InvoiceDto
            {
                Id = f.Id,
                Numero = f.Numero,
                DateFacture = f.DateFacture,
                ClientId = f.ClientId,
                ClientName = f.Client != null ? $"{f.Client.Prenom} {f.Client.Nom}" : "Unknown",
                ClientMatricule = f.Client?.MatriculeFiscal,
                TotalHT = f.TotalHT,
                TotalTVA = f.TotalTVA,
                TotalTTC = f.TotalTTC,
                TimbreFiscal = f.TimbreFiscal,
                Statut = f.Statut,
                Lignes = f.Lignes.Select(l => new InvoiceLineDto
                {
                    Id = l.Id,
                    ProduitId = l.ProduitId,
                    ProductName = l.Produit?.Nom,
                    Quantite = l.Quantite,
                    PrixUnitaireHT = l.PrixUnitaireHT,
                    TauxTVA = l.TauxTVA,
                    MontantHT = l.MontantHT,
                    MontantTVA = l.MontantTVA,
                    MontantTTC = l.MontantTTC
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvoiceDto dto)
        {
            var facture = new Facture
            {
                ClientId = dto.ClientId,
                DateFacture = dto.DateFacture == default ? DateTime.UtcNow : dto.DateFacture,
                Statut = "Brouillon",
                Lignes = dto.Lignes.Select(l => new LigneFacture
                {
                    ProduitId = l.ProduitId,
                    Quantite = l.Quantite,
                    PrixUnitaireHT = l.PrixUnitaireHT,
                    TauxTVA = l.TauxTVA
                }).ToList()
            };

            var result = await _factureService.CreateFactureAsync(facture);
            if (!result.IsSuccess || result.Value == null) return BadRequest(result.Error);
            
            // Map back to DTO for response to avoid circular references
            var responseDto = new InvoiceDto
            {
                Id = result.Value.Id,
                Numero = result.Value.Numero,
                DateFacture = result.Value.DateFacture,
                ClientId = result.Value.ClientId,
                TotalTTC = result.Value.TotalTTC,
                Statut = result.Value.Statut
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InvoiceDto dto)
        {
            var result = await _factureService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Value == null) return NotFound(result.Error);

            var facture = result.Value;
            if (facture.Statut != "Brouillon") return BadRequest("Cannot edit a validated invoice.");

            var oldStatut = facture.Statut;
            facture.ClientId = dto.ClientId;
            facture.DateFacture = dto.DateFacture;
            facture.Statut = dto.Statut;
            
            // Simple approach: clear lines and re-add (if not validated)
            facture.Lignes.Clear();
            foreach (var l in dto.Lignes)
            {
                facture.Lignes.Add(new LigneFacture
                {
                    ProduitId = l.ProduitId,
                    Quantite = l.Quantite,
                    PrixUnitaireHT = l.PrixUnitaireHT,
                    TauxTVA = l.TauxTVA
                });
            }

            var saveResult = await _factureService.CreateFactureAsync(facture); 
            if (!saveResult.IsSuccess) return BadRequest(saveResult.Error);

            // If status changed to Validée and it wasn't before, trigger stock adjustment
            if (oldStatut == "Brouillon" && dto.Statut == "Validée")
            {
                var validationResult = await _factureService.ValidateFactureAsync(id);
                if (!validationResult.IsSuccess) return BadRequest("Saved but failed to adjust stock: " + validationResult.Error);
            }

            return Ok();
        }

        [HttpPut("{id}/validate")]
        public async Task<IActionResult> Validate(int id)
        {
            var result = await _factureService.ValidateFactureAsync(id);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _factureService.SoftDeleteAsync(id);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return NoContent();
        }
    }
}
