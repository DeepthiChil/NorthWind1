using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFullStackApp.WebUI.Services;

namespace MyFullStackApp.WebUI.Pages;
public class LoginModel : PageModel
{
    private readonly ApiService _api;
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Error { get; set; }

    public LoginModel(ApiService api) => _api = api;

    public async Task<IActionResult> OnPostAsync()
    {
        if (await _api.LoginAsync(Username, Password))
            return Redirect("/Index");
        Error = true;
        return Page();
    }
}
