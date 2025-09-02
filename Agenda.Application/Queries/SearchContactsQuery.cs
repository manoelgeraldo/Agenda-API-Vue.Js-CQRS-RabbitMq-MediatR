using Agenda.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Application.Queries
{
	public record SearchContactsQuery(string? Q, int Page = 1, int PageSize = 10) : IRequest<PagedResult<ContactDto>>;
}
