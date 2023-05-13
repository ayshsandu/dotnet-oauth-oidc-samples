namespace MyServiceApp;
using MyServiceApp.services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    //base urls of the APIs
    private readonly string choreoAPIBaseURL;
    private readonly string apiEndpoint;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        choreoAPIBaseURL = _configuration["ResourceAPI:ChoreoAPIEndpoint"] ?? "";
        apiEndpoint = choreoAPIBaseURL + "/posts";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Invoke a service to call an API endpoint without a token
        MyService service = new MyService(_logger, _configuration);
        string response = await service.InvokeApiAsync(apiEndpoint);
        _logger.LogInformation("Response recieved without token: {response}", response);

        //invoke get token
        string token = await service.GetAccessTokenAsync();
        _logger.LogInformation("Token recieved to worker: {token}", token);

        // Invoke a service to call an API endpoint with a token
        string sucessResponse = await service.InvokeSecuredApiAsync(apiEndpoint);
        _logger.LogInformation("Response recieved with token: {response}", response);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(2000, stoppingToken);
        }
    }
}
