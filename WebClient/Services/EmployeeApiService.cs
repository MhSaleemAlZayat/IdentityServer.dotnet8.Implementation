using Duende.IdentityModel.Client;
using System.Text.Json;

namespace WebClient.Services;

public class EmployeeApiService : IEmployeeApiService
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _employeeHttpClient;

    public EmployeeApiService(HttpClient httpClient, HttpClient employeeHttpClient)
    {
        _httpClient = httpClient;
        _employeeHttpClient = employeeHttpClient;
    }
    public async Task<List<EmployeeApiModel>> GetEmployees()
    {
        List<EmployeeApiModel> employees = new List<EmployeeApiModel>();
        try
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                throw new Exception(disco.Error);
            }

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "employee_api"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.WriteLine(tokenResponse.ErrorDescription);
                return employees;
            }

            _employeeHttpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await _employeeHttpClient.GetAsync("https://localhost:5002/employees");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                return employees;
            }
            else
            {
                var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
                Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));

                var json = await response.Content.ReadAsStringAsync();
                employees = JsonSerializer.Deserialize<List<EmployeeApiModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<EmployeeApiModel>();
            }

            return employees;
        }
        catch (Exception exp)
        {

            throw;
        }
    }
}
