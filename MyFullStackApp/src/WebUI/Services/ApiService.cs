using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MyFullStackApp.WebUI.Services;

public class ApiService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _config;

    public ApiService(IHttpClientFactory factory, IConfiguration config)
    {
        _client = factory.CreateClient("api");
        _config = config;
    }

    public async Task<bool> LoginAsync(string user, string pass)
    {
        var dto = new { Username = user, Password = pass };
        var resp = await _client.PostAsync("auth/login",
            new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
        if (!resp.IsSuccessStatusCode) return false;

        var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        var token = doc.RootElement.GetProperty("token").GetString()!;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return true;
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _client.GetFromJsonAsync<List<Customer>>("customers")
               ?? new List<Customer>();
    }

    public async Task<List<Order>> GetOrdersAsync(string customerId)
    {
        return await _client.GetFromJsonAsync<List<Order>>($"orders/byCustomer/{customerId}")
               ?? new List<Order>();
    }

    public async Task<bool> AddOrderAsync(Order o)
    {
        var resp = await _client.PostAsJsonAsync("orders", o);
        return resp.IsSuccessStatusCode;
    }
}

public record Customer(string CustomerID, string CompanyName);
public record Order(int OrderID, string CustomerID, DateTime OrderDate, string ShipCity);
