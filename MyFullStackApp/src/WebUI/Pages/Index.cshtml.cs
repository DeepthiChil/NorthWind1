using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFullStackApp.WebUI.Services;

namespace MyFullStackApp.WebUI.Pages;
public class IndexModel : PageModel
{
    private readonly ApiService _api;
    public List<Customer> Customers { get; set; } = new();

    public IndexModel(ApiService api) => _api = api;

    public async Task OnGetAsync()
    {
        Customers = await _api.GetCustomersAsync();
    }
}
