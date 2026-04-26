using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/test-seed")]
    public class TestController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IProduitService _produitService;
        private readonly IFactureService _factureService;
        private readonly IAnalyticsService _analyticsService;

        public TestController(
            IClientService clientService, 
            IProduitService produitService, 
            IFactureService factureService,
            IAnalyticsService analyticsService)
        {
            _clientService = clientService;
            _produitService = produitService;
            _factureService = factureService;
            _analyticsService = analyticsService;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var clients = await _clientService.GetAllAsync();
            var produits = await _produitService.GetAllAsync();
            var factures = await _factureService.GetAllAsync();
            var stats = await _analyticsService.GetGeneralStatsAsync();

            return Ok(new
            {
                ClientsCount = clients.Value?.Count() ?? 0,
                ProductsCount = produits.Value?.Count() ?? 0,
                InvoicesCount = factures.Value?.Count() ?? 0,
                DashboardStats = stats.Value
            });
        }
    }
}
