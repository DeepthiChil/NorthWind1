using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFullStackApp.Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyFullStackApp.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IFirebasePushService _pushService;

    public AuthController(
        UserManager<IdentityUser> userManager,
        IConfiguration config,
        IFirebasePushService pushService)
    {
        _userManager = userManager;
        _config = config;
        _pushService = pushService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDto dto)
    {
        var user = new IdentityUser { UserName = dto.Username, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded) return Ok();
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized();

        var token = GenerateJwtToken(user.UserName);
        return Ok(new { token });
    }

    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"],
            claims: new[] { new Claim(ClaimTypes.Name, username) },
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class AuthDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
