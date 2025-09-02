using Agenda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Domain.Contracts
{
	public interface IContactRepository
	{
		Task<bool> EmailExistsAsync(string normalizedEmail, Guid? ignoreId = null);
		Task<bool> PhoneExistsAsync(string normalizedPhone, Guid? ignoreId = null);
		Task AddAsync(Contact contact);
		Task UpdateAsync(Contact contact);
		Task<Contact?> GetByIdAsync(Guid id);
		Task<(IReadOnlyList<Contact> Items, int Total)> SearchAsync(string? q, int page, int pageSize);
	}
}
