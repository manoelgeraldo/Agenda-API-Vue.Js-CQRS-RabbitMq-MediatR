using Agenda.Api.Configurations;
using Agenda.Infra.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextConfiguration(builder.Configuration);
builder.Services.AddMediatRConfiguration();
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddDependencyInjectionConfiguration();
builder.Services.AddRabbitMqConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AgendaDbContext>();
	db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
