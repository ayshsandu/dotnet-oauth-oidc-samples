namespace MyServiceApp;
using MyServiceApp.services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Invoke a service to call an API endpoint
        MyService service = new MyService(_logger);
        string response = await service.InvokeApiAsync("/posts");
        _logger.LogInformation("Response recieved to worker: {response}", response);

        //invoke get token
        try
        {
            string token = await service.GetAccessTokenAsync();
            _logger.LogInformation("Token recieved to worker: {token}", token);
        }
        catch (System.Exception ex)
        {
            // Break the execution if the token is not recieved
            throw;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
