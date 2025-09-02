using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Agenda.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController(IConfiguration cfg) : ControllerBase
	{
		public record LoginRequest(string Email, string Password);

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginRequest req)
		{
			// usuário fixo
			if (!req.Email.Equals("admin@email.com", StringComparison.OrdinalIgnoreCase) || req.Password != "123456")
				return Unauthorized();

			var secret = cfg["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key ausente.");
			var keyBytes = Encoding.UTF8.GetBytes(secret);

			var key = new SymmetricSecurityKey(keyBytes);
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				claims: [new Claim(ClaimTypes.Name, req.Email)],
				notBefore: DateTime.UtcNow,
				expires: DateTime.UtcNow.AddHours(8),
				signingCredentials: creds
			);

			return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
		}
	}
}
