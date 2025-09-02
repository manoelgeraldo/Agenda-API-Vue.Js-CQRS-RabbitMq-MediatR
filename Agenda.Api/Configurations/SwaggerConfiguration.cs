using Agenda.Application.Commands;

namespace Agenda.Api.Configurations
{
	public static class SwaggerConfiguration
	{
		public static void AddSwaggerConfiguration(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new() { Title = "Agenda API", Version = "v1" });
				var scheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
				{
					Name = "Authorization",
					In = Microsoft.OpenApi.Models.ParameterLocation.Header,
					Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Reference = new Microsoft.OpenApi.Models.OpenApiReference { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
				};
				c.AddSecurityDefinition("Bearer", scheme);
				c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
				{
					{ scheme, Array.Empty<string>() }
				});
			});
		}
	}
}
