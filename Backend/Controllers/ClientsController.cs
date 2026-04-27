using Backend.Models;
using Backend.Services;
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
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _clientService.GetByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            var result = await _clientService.CreateAsync(client);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            if (id != client.Id) return BadRequest("Mismatched ID");
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
