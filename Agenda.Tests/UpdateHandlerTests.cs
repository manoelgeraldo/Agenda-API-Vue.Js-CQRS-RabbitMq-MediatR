using Agenda.Application.Commands;
using Agenda.Domain.Contracts;
using Agenda.Application.Handlers;
using Agenda.Application.Mappings;
using Agenda.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Agenda.Tests
{
	public class UpdateContactHandlerTests
	{
		[Fact]
		public async Task Update_Should_Throw_When_Email_Exists()
		{
			var repo = new Mock<IContactRepository>();
			var bus = new Mock<IPublishEndpoint>();
			var loggerFactory = NullLoggerFactory.Instance;

			var config = new MapperConfiguration(cfg =>
				{
					cfg.AddProfile<MappingProfile>();
				}, loggerFactory);
			config.AssertConfigurationIsValid();
			IMapper mapper = config.CreateMapper();

			var contact = Contact.Create("A", "a@a.com", "11999999999");
			repo.Setup(r => r.GetByIdAsync(contact.Id)).ReturnsAsync(contact);
			repo.Setup(r => r.EmailExistsAsync(contact.NormalizedEmail, contact.Id)).ReturnsAsync(true);

			var h = new UpdateContactHandler(repo.Object, mapper, bus.Object);

			await FluentActions.Invoking(() => h.Handle(new UpdateContactCommand(contact.Id, "A", "a@a.com", "11999999999"), default))
			.Should().ThrowAsync<ValidationException>();
		}
	}
}
