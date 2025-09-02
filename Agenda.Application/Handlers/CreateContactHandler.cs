using Agenda.Application.Commands;
using Agenda.Domain.Contracts;
using Agenda.Application.DTOs;
using Agenda.Domain.Entities;
using AutoMapper;
using MassTransit;
using MediatR;

namespace Agenda.Application.Handlers
{
	public class CreateContactHandler(IContactRepository repo, IMapper mapper, IPublishEndpoint bus)
		: IRequestHandler<CreateContactCommand, ContactDto>
	{
		public async Task<ContactDto> Handle(CreateContactCommand req, CancellationToken cancellationToken)
		{
			var created = Contact.Create(req.Name, req.Email, req.Phone);
			await repo.AddAsync(created);
			await bus.Publish(new ContactUpdated(created.Id, created.Email), cancellationToken);
			return mapper.Map<ContactDto>(created);
		}
	}
}
