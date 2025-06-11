using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFullStackApp.WebUI.Services;

namespace MyFullStackApp.WebUI.Pages;
public class OrdersModel : PageModel
{
    private readonly ApiService _api;
    public string CustomerID { get; set; }
    public string CustomerName { get; set; } = "";
    public List<Order> Orders { get; set; } = new();
    [BindProperty] public string ShipCity { get; set; }

    public OrdersModel(ApiService api) => _api = api;

    public async Task OnGetAsync(string id)
    {
        CustomerID = id;
        Orders = await _api.GetOrdersAsync(id);
        var customers = await _api.GetCustomersAsync();
        CustomerName = customers.FirstOrDefault(c => c.CustomerID == id)?.CompanyName ?? "";
    }

    public async Task<IActionResult> OnPostAsync(string CustomerID)
    {
        var o = new Order(0, CustomerID, DateTime.UtcNow, ShipCity);
        await _api.AddOrderAsync(o);
        return RedirectToPage(new { id = CustomerID });
    }
}
