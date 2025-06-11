using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyFullStackApp.Backend.Models;

namespace MyFullStackApp.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ApiDbContext _db;
    private readonly IHubContext<NotifyHub> _hub;
    private readonly IFirebasePushService _push;

    public CustomersController(ApiDbContext db, IHubContext<NotifyHub> hub, IFirebasePushService push)
    {
        _db = db;
        _hub = hub;
        _push = push;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var data = await _db.Customers.Include(c => c.Orders).ToListAsync();
        return Ok(data);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] Customer c)
    {
        _db.Customers.Add(c);
        await _db.SaveChangesAsync();

        await NotifyClients("customer");
        return CreatedAtAction(nameof(GetAll), new { id = c.CustomerID }, c);
    }

    private async Task NotifyClients(string type)
    {
        await _hub.Clients.All.SendAsync("DataChanged", type);
        await _push.SendNotificationAsync(type);
    }
}
