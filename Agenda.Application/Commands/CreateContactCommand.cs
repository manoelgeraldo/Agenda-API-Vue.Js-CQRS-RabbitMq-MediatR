using Agenda.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Application.Commands
{
	public record CreateContactCommand(string Name, string Email, string Phone) : IRequest<ContactDto>;
}
