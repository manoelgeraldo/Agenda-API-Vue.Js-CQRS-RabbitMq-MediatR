using Agenda.Domain.Contracts;
using Agenda.Infra.ReadModel;
using Agenda.Infra.Repositories;

namespace Agenda.Api.Configurations
{
	public static class DependencyInjectionConfiguration
	{
		public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
		{
			services.AddScoped<IContactRepository, ContactRepository>();
			services.AddScoped<IContactReadDb, ContactReadDb>();
		}
	}
}
