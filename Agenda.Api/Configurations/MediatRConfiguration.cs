using Agenda.Application.Commands;

namespace Agenda.Api.Configurations
{
	public static class MediatRConfiguration
	{
		public static void AddMediatRConfiguration(this IServiceCollection services)
		{
			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssemblyContaining<CreateContactCommand>();
				cfg.RegisterServicesFromAssemblyContaining<DeleteContactCommand>();
				cfg.RegisterServicesFromAssemblyContaining<UpdateContactCommand>();
			});
		}
	}
}
