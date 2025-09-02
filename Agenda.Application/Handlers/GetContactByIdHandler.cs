using Agenda.Domain.Contracts;
using Agenda.Application.DTOs;
using Agenda.Application.Queries;
using AutoMapper;
using MediatR;

namespace Agenda.Application.Handlers
{
	public class GetContactByIdHandler(IContactRepository repo, IMapper mapper) : IRequestHandler<GetContactByIdQuery, ContactDto?>
	{
		public async Task<ContactDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
		{
			var c = await repo.GetByIdAsync(request.Id);
			return c is null ? null : mapper.Map<ContactDto>(c);
		}
	}
}
