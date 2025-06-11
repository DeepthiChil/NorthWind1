using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MyFullStackApp.Mobile.Models;

namespace MyFullStackApp.Mobile.Services;
public class ApiService
{
    private readonly HttpClient _client = new();
    private readonly string _baseUrl = "https://your-backend-url/api/";

    public async Task<bool> LoginAsync(string user, string pass)
    {
        var dto = new { Username = user, Password = pass };
        var resp = await _client.PostAsync(_baseUrl + "auth/login",
            new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));

        if (!resp.IsSuccessStatusCode) return false;
        var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        var token = doc.RootElement.GetProperty("token").GetString();

        await SecureStorage.SetAsync("jwt", token!);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return true;
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        SetAuthHeader();
        var resp = await _client.GetStringAsync(_baseUrl + "customers");
        return JsonSerializer.Deserialize<List<Customer>>(resp)!;
    }

    public async Task<List<Order>> GetOrdersAsync(string customerId)
    {
        SetAuthHeader();
        var resp = await _client.GetStringAsync(_baseUrl + $"orders/byCustomer/{customerId}");
        return JsonSerializer.Deserialize<List<Order>>(resp)!;
    }

    public async Task<bool> AddOrderAsync(Order order)
    {
        SetAuthHeader();
        var content = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");
        var resp = await _client.PostAsync(_baseUrl + "orders", content);
        return resp.IsSuccessStatusCode;
    }

    private void SetAuthHeader()
    {
        if (_client.DefaultRequestHeaders.Authorization is null)
        {
            var token = SecureStorage.GetAsync("jwt").GetAwaiter().GetResult();
            if (token != null)
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
