namespace MyServiceApp.services;
using System.Text.Json;
using System.Text.Json.Serialization;

public class MyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    //base url for the API
    private readonly string resourceAPIBaseURL;

    // KeyManager configurations
    string tokenEndpoint;
    string clientId;
    string clientSecret;

    public MyService(ILogger logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();

        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        resourceAPIBaseURL = _configuration["ResourceAPI:BaseURL"];

        clientId = _configuration["IdentityProvider:ClientId"];
        clientSecret = _configuration["IdentityProvider:ClientSecret"];
        tokenEndpoint = _configuration["IdentityProvider:TokenEndpoint"];
        
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

    /**
    * Get the access token using client credentials grant
    * This implements the OAuth 2.0 client credentials grant protocol,
        making a standard HTTP request to the token endpoint using the HttpClient class.
    */
    public async Task<string> GetAccessTokenAsync()
    {
        // Create a new HTTP request message
        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
        // Add the required parameters to the request message body
        tokenRequest.Content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", clientSecret }
            }
        );

        try
        {
            // Send the token request and parse the response
            var tokenResponse = await _httpClient.SendAsync(tokenRequest);

            if (!tokenResponse.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Token request failed: {tokenResponse.StatusCode} : {tokenResponse.ReasonPhrase}"
                );
            }

            // Read the response, parse it to json and extract the access token
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            _logger.LogInformation("Response: {response}", tokenContent);
            var tokenObject = JsonSerializer.Deserialize<JsonDocument>(tokenContent);
            var accessToken = tokenObject.RootElement.GetProperty("access_token").GetString();
            return accessToken;
        }
        catch (System.Exception ex)
        {
            throw new Exception($"Something went wrong when obtaining the token: {ex.Message}");
        }
    }
}
