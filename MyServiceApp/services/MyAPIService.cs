namespace MyServiceApp.services;

public class MyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    //base url for the API
    private readonly string resourceAPIBaseURL;

    // = "https://jsonplaceholder.typicode.com";

    public MyService(ILogger logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();

        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        resourceAPIBaseURL = _configuration["ResourceAPI:BaseURL"];
    }

    /*
    * Method to invoke a API endpoint
    */
    public async Task<string> InvokeApiAsync(string endpoint)
    {
        //Calling the API endpoint

        try
        {
            string apiUrl = resourceAPIBaseURL + endpoint;
            // + endpoint;
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                // Log the response content from the API endpoint
                _logger.LogError("Error: {response}", response.Content.ReadAsStringAsync());
                throw new Exception(
                    $"Error while calling the API endpoint: {response.StatusCode} : {response.ReasonPhrase}"
                );
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            // Log the response from the API endpoint
            _logger.LogInformation("Response: {response}", responseContent);
            return responseContent;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {error}", ex.Message);
            return ex.Message;
        }
    }
}
