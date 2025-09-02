using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Domain.Entities
{
	public class Contact
	{
		public Guid Id { get; private set; } = Guid.NewGuid();
		public string Name { get; private set; } = string.Empty;
		public string Email { get; private set; } = string.Empty;
		public string NormalizedEmail { get; private set; } = string.Empty;
		public string Phone { get; private set; } = string.Empty; // mantem formato original
		public string NormalizedPhone { get; private set; } = string.Empty; // apenas dígitos
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
		public bool IsDeleted { get; private set; }


		public static Contact Create(string name, string email, string phone)
		{
			var c = new Contact();
			c.Update(name, email, phone, isCreate: true);
			return c;
		}


		public void Update(string name, string email, string phone, bool isCreate = false)
		{
			Name = name.Trim();
			Email = email.Trim();
			NormalizedEmail = Email.ToLowerInvariant();
			Phone = phone.Trim();
			NormalizedPhone = new string(Phone.Where(char.IsDigit).ToArray());
			if (isCreate) CreatedAt = DateTime.UtcNow;
			if (!isCreate) UpdatedAt = DateTime.UtcNow;
		}


		public void SoftDelete()
		{
			IsDeleted = true;
			UpdatedAt = DateTime.UtcNow;
		}
	}
}
