using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using MyWebApp.services;
using Microsoft.Extensions.Logging;

namespace MyWebApp.Controllers
{
    [Route("api/backend")]
    [ApiController]
    public class BackendController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BackendController> _logger;
        private readonly IConfiguration _configuration;

        public BackendController(HttpClient httpClient, ILogger<BackendController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        [HttpGet("data")]
        public IActionResult GetData()
        {
            // Replace this with your actual data retrieval logic
            var data = new { Message = "Data from backend API" };

            return Ok(data);
        }

        [HttpGet("secured-data")]
        public async Task<IActionResult>  GetDataInSecurely()
        {
            // Invoke a service to call an API endpoint without a token
            string apiEndpoint = _configuration["ResourceAPI:ChoreoAPIEndpoint"] + "/posts";
            MyService service = new MyService(_logger, _configuration);
            string response = await service.InvokeApiAsync(apiEndpoint);
            return Ok(response);
        }

        [HttpGet("secured-data-with-token")]
        public async Task<IActionResult>  GetDataSecurely()
        {
            // Invoke a service to call an API endpoint without a token
            string apiEndpoint = _configuration["ResourceAPI:ChoreoAPIEndpoint"] + "/posts";
            MyService service = new MyService(_logger, _configuration);
            string response = await service.InvokeSecuredApiAsync(apiEndpoint);
            return Ok(response);
        }
    }
}
