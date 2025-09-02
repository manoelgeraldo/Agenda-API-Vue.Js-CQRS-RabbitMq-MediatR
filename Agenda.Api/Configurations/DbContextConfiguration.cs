using Agenda.Application.Commands;
using Agenda.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Api.Configurations
{
	public static class DbContextConfiguration
	{
		public static void AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AgendaDbContext>(o => o.UseSqlite(configuration.GetConnectionString("Default")));
		}
	}
}
