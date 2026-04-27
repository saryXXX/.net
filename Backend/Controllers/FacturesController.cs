using Backend.Models;
using Backend.Services;
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
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _factureService.GetByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Facture facture)
        {
            var result = await _factureService.CreateFactureAsync(facture);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
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
