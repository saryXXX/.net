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
    public class ProduitsController : ControllerBase
    {
        private readonly IProduitService _produitService;

        public ProduitsController(IProduitService produitService)
        {
            _produitService = produitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _produitService.GetAllAsync();
            if (!result.IsSuccess || result.Value == null) return BadRequest(result.Error);

            var dtos = result.Value.Select(p => new ProduitDto
            {
                Id = p.Id,
                Nom = p.Nom,
                PrixUnitaireHT = p.PrixUnitaireHT,
                TauxTVA = p.TauxTVA,
                StockActuel = p.StockActuel,
                SeuilAlerte = p.SeuilAlerte,
                Actif = p.Actif
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _produitService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Value == null) return NotFound(result.Error);

            var p = result.Value;
            var dto = new ProduitDto
            {
                Id = p.Id,
                Nom = p.Nom,
                PrixUnitaireHT = p.PrixUnitaireHT,
                TauxTVA = p.TauxTVA,
                StockActuel = p.StockActuel,
                SeuilAlerte = p.SeuilAlerte,
                Actif = p.Actif
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProduitDto dto)
        {
            var produit = new Produit
            {
                Nom = dto.Nom,
                PrixUnitaireHT = dto.PrixUnitaireHT,
                TauxTVA = dto.TauxTVA,
                StockActuel = dto.StockActuel,
                SeuilAlerte = dto.SeuilAlerte,
                Actif = dto.Actif
            };

            var result = await _produitService.CreateAsync(produit);
            if (!result.IsSuccess || result.Value == null) return BadRequest(result.Error);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProduitDto dto)
        {
            if (id != dto.Id) return BadRequest("Mismatched ID");

            var produit = new Produit
            {
                Id = dto.Id,
                Nom = dto.Nom,
                PrixUnitaireHT = dto.PrixUnitaireHT,
                TauxTVA = dto.TauxTVA,
                StockActuel = dto.StockActuel,
                SeuilAlerte = dto.SeuilAlerte,
                Actif = dto.Actif
            };

            var result = await _produitService.UpdateAsync(produit);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _produitService.SoftDeleteAsync(id);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return NoContent();
        }
    }
}
