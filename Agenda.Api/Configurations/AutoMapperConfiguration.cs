using Agenda.Application.Mappings;

namespace Agenda.Api.Configurations
{
	public static class AutoMapperConfiguration
	{
		public static void AddAutoMapperConfiguration(this IServiceCollection services)
		{
			services.AddAutoMapper(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
		}
	}
}
