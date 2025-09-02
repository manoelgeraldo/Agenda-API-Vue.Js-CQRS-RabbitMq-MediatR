using Agenda.Domain.Contracts;
using Agenda.Application.DTOs;
using Agenda.Application.Queries;
using AutoMapper;
using MediatR;

namespace Agenda.Application.Handlers
{
	public class SearchContactsHandler(IContactRepository repo, IMapper mapper) : IRequestHandler<SearchContactsQuery, PagedResult<ContactDto>>
	{
		public async Task<PagedResult<ContactDto>> Handle(SearchContactsQuery request, CancellationToken cancellationToken)
		{
			var (items, total) = await repo.SearchAsync(request.Q, request.Page, request.PageSize);
			var dtos = items.Select(mapper.Map<ContactDto>).ToList();
			return new PagedResult<ContactDto>(dtos, request.Page, request.PageSize, total);
		}
	}
}
