using Agenda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Domain.Contracts
{
	public interface IContactReadDb
	{
		Task<Contact?> GetByEmailAsync(string email);
	}
}
