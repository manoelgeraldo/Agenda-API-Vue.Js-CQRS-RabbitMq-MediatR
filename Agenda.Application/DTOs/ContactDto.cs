using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Application.DTOs
{
	public record ContactDto(Guid Id, string Name, string Email, string Phone, DateTime CreatedAt, DateTime UpdatedAt);

	public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int Total);	
}
