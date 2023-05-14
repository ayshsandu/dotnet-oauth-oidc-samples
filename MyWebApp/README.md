# MyWebApp

This project is a web application built using the ASP.NET Core Web App Boilerplate. It provides a simple UI with two buttons to load data from a backend API. One button invokes the API without any security headers, while the other button obtains a token using the client credentials grant and attaches the token to the API request.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) installed on your machine.

## Getting Started

1. Clone the repository:

```bash
git clone <repository-url>
```

2. Navigate to the project directory:

```bash
cd MyWebApp
```

3. Restore the project dependencies:

```bash
dotnet restore
```

4. Build the project:

```bash
dotnet build
```

5. Run the project:

```bash
dotnet run
```
6. Update the configurations in the `appsettings.json`. 

7. Open a web browser to access the web application.

## Usage

The web application UI contains two buttons:

1. **Load Data Unsecure**: Clicking this button will invoke the backend API without any security headers. It will display the response from the API in the UI.

2. **Load Data Securely**: Clicking this button will obtain a token using the client credentials grant and attach the token as a bearer token to the backend API request. It will display the response from the API in the UI.

## Configuration

The web application uses the `appsettings.json` file for configuration. Ensure that you have the necessary configurations set up before running the application. Open the `appsettings.json` file and update the following values:

```json
{
  ...,
  "ResourceAPI": {
    "BaseURL": "https://jsonplaceholder.typicode.com",
    "ChoreoAPIEndpoint": "<Choreo API Endpoint>"
  },
  "IdentityProvider": {
    "ClientId": "<your client id>",
    "ClientSecret": "<your client secret",
    "TokenEndpoint": "<your token endpoint>"
  }
}
```

Replace `<Choreo API Endpoint>` with the endpoint of your backend API, `<client-id>` with your client ID, `<client-secret>` with your client secret, and `<token-endpoint>` with the token endpoint URL.

## Troubleshooting

- If you encounter any issues related to dependencies or missing packages, try running `dotnet restore` to restore the project dependencies.

- Ensure that the backend API is accessible and running. Verify the API endpoint and make sure it is functioning correctly.

- Double-check the configuration values in the `appsettings.json` file. Ensure that the values for the backend API endpoint, client ID, client secret, and token endpoint are correct.

## Contributing

Contributions to this project are welcome. If you find any issues or have suggestions for improvement, feel free to open a pull request or submit an issue.

