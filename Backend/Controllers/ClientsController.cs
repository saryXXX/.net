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
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _clientService.GetAllAsync();
            if (!result.IsSuccess || result.Value == null) return BadRequest(result.Error);

            var dtos = result.Value.Select(c => new ClientDto
            {
                Id = c.Id,
                Nom = c.Nom,
                Prenom = c.Prenom,
                Email = c.Email,
                Telephone = c.Telephone,
                Adresse = c.Adresse,
                MatriculeFiscal = c.MatriculeFiscal,
                DateCreation = c.DateCreation
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _clientService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Value == null) return NotFound(result.Error);

            var c = result.Value;
            var dto = new ClientDto
            {
                Id = c.Id,
                Nom = c.Nom,
                Prenom = c.Prenom,
                Email = c.Email,
                Telephone = c.Telephone,
                Adresse = c.Adresse,
                MatriculeFiscal = c.MatriculeFiscal,
                DateCreation = c.DateCreation
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientDto dto)
        {
            var client = new Client
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Adresse = dto.Adresse,
                MatriculeFiscal = dto.MatriculeFiscal
            };

            var result = await _clientService.CreateAsync(client);
            if (!result.IsSuccess || result.Value == null) return BadRequest(result.Error);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientDto dto)
        {
            if (id != dto.Id) return BadRequest("Mismatched ID");

            var client = new Client
            {
                Id = dto.Id,
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Adresse = dto.Adresse,
                MatriculeFiscal = dto.MatriculeFiscal,
                DateCreation = dto.DateCreation
            };

            var result = await _clientService.UpdateAsync(client);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clientService.SoftDeleteAsync(id);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return NoContent();
        }
    }
}
