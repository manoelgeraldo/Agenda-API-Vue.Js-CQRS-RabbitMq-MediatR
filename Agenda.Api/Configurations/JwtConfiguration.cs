using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Agenda.Api.Configurations
{
	public static class JwtConfiguration
	{
		public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var secret = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key ausente.");
			var keyBytes = Encoding.UTF8.GetBytes(secret);

			var key = new SymmetricSecurityKey(keyBytes);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(o =>
			{
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = key,
					ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
					ClockSkew = TimeSpan.Zero
				};

				o.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = ctx =>
					{
						Console.WriteLine($"JWT fail: {ctx.Exception.Message}");
						return Task.CompletedTask;
					}
				};
			});
		}
	}
}
