using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyFullStackApp.Backend.Models;

namespace MyFullStackApp.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ApiDbContext _db;
    private readonly IHubContext<NotifyHub> _hub;
    private readonly IFirebasePushService _push;

    public OrdersController(ApiDbContext db, IHubContext<NotifyHub> hub, IFirebasePushService push)
    {
        _db = db;
        _hub = hub;
        _push = push;
    }

    [HttpGet("byCustomer/{customerId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCustomer(string customerId)
    {
        var orders = await _db.Orders
            .Where(o => o.CustomerID == customerId)
            .ToListAsync();
        return Ok(orders);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] Order o)
    {
        _db.Orders.Add(o);
        await _db.SaveChangesAsync();

        await NotifyClients("order");
        return CreatedAtAction(nameof(GetByCustomer), new { customerId = o.CustomerID }, o);
    }

    private async Task NotifyClients(string type)
    {
        await _hub.Clients.All.SendAsync("DataChanged", type);
        await _push.SendNotificationAsync(type);
    }
}
