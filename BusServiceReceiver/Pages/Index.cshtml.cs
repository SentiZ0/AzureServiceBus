using Azure.Messaging.ServiceBus;
using BusServiceReceiver.Models;
using BusServiceReceiver.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BusServiceReceiver.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly string _serviceConnectionString;
        private readonly BusDBContext _context;

        public IndexModel(BusDBContext context, ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _serviceConnectionString = configuration.GetConnectionString("BusConnectionString");
        }

        public string AlertMessage { get; set; }

        public async Task<IActionResult> OnPost()
        {
            return Page();
        }

        public async void OnGet()
        {
        }
    }
}