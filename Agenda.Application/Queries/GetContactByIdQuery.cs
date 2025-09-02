using Agenda.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Application.Queries
{
	public record GetContactByIdQuery(Guid Id) : IRequest<ContactDto?>;
}
