using Agenda.Application.Commands;
using Agenda.Domain.Contracts;
using Agenda.Application.DTOs;
using AutoMapper;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Agenda.Application.Handlers
{
	public class UpdateContactHandler(IContactRepository repo, IMapper mapper, IPublishEndpoint bus)
		: IRequestHandler<UpdateContactCommand, ContactDto>
	{
		public async Task<ContactDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
		{
			var existing = await repo.GetByIdAsync(request.Id) 
				?? throw new KeyNotFoundException("Contato não encontrado.");
			existing.Update(request.Name, request.Email, request.Phone);

			if (await repo.EmailExistsAsync(existing.NormalizedEmail, existing.Id))
				throw new ValidationException("E-mail já cadastrado.");
			if (await repo.PhoneExistsAsync(existing.NormalizedPhone, existing.Id))
				throw new ValidationException("Telefone já cadastrado.");

			await repo.UpdateAsync(existing);
			await bus.Publish(new ContactUpdated(existing.Id, existing.Email), cancellationToken);
			return mapper.Map<ContactDto>(existing);
		}
	}
}
