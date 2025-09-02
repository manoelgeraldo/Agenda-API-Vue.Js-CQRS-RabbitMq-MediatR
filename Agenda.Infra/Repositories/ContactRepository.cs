using Agenda.Domain.Contracts;
using Agenda.Domain.Entities;
using Agenda.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infra.Repositories
{
	public class ContactRepository(AgendaDbContext db) : IContactRepository
	{
		public async Task AddAsync(Contact contact)
		{
			db.Contacts.Add(contact);
			await db.SaveChangesAsync();
		}
		public async Task UpdateAsync(Contact contact)
		{
			db.Contacts.Update(contact);
			await db.SaveChangesAsync();
		}

		public Task<Contact?> GetByIdAsync(Guid id) => db.Contacts.FirstOrDefaultAsync(x => x.Id == id);

		public async Task<(IReadOnlyList<Contact> Items, int Total)> SearchAsync(string? q, int page, int pageSize)
		{
			q ??= string.Empty;
			var qTrim = q.Trim();
			var qLower = qTrim.ToLowerInvariant();
			var qDigits = new string(qTrim.Where(char.IsDigit).ToArray());

			var query = db.Contacts.AsQueryable();

			if (!string.IsNullOrWhiteSpace(qTrim))
			{
				query = query.Where(c =>
				EF.Functions.Like(EF.Functions.Collate(c.Name, "NOCASE"), "%" + qTrim + "%") ||
				c.NormalizedEmail.Contains(qLower) ||
				(qDigits != "" && c.NormalizedPhone.Contains(qDigits))
				);
			}

			var total = await query.CountAsync();

			var items = await query.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
			return (items, total);
		}

		public Task<bool> EmailExistsAsync(string normalizedEmail, Guid? ignoreId = null)
		{
			var q = db.Contacts.Where(x => x.NormalizedEmail == normalizedEmail);
			if (ignoreId.HasValue) q = q.Where(x => x.Id != ignoreId);
			return q.AnyAsync();
		}

		public Task<bool> PhoneExistsAsync(string normalizedPhone, Guid? ignoreId = null)
		{
			var q = db.Contacts.Where(x => x.NormalizedPhone == normalizedPhone);
			if (ignoreId.HasValue) q = q.Where(x => x.Id != ignoreId);
			return q.AnyAsync();
		}
	}
}
