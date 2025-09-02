using Agenda.Application.Commands;
using Agenda.Infra.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Api.Configurations
{
	public static class RabbitMqConfiguration
	{
		public static void AddRabbitMqConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var useRabbit = configuration.GetValue("Messaging:UseRabbitMQ", false);
			
			services.AddMassTransit(x =>
			{
				if (useRabbit)
				{
					x.UsingRabbitMq((ctx, cfg) =>
					{
						var host = configuration.GetValue<string>("RabbitMQ:Host") ?? "rabbitmq";
						cfg.Host(host, "/", h =>
						{
							h.Username(configuration.GetValue<string>("RabbitMQ:User") ?? "admin");
							h.Password(configuration.GetValue<string>("RabbitMQ:Pass") ?? "admin");
						});
					});
				}
				else
				{
					x.UsingInMemory((ctx, cfg) => { });
				}
			});
		}
	}
}
