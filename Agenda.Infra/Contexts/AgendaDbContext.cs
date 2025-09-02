using Agenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infra.Contexts
{
	public class AgendaDbContext(DbContextOptions<AgendaDbContext> options) : DbContext(options)
	{
		public DbSet<Contact> Contacts => Set<Contact>();

		protected override void OnModelCreating(ModelBuilder b)
		{
			b.Entity<Contact>(e =>
			{
				e.ToTable("Contacts");
				e.HasKey(x => x.Id);
				e.Property(x => x.Name).HasMaxLength(100).IsRequired();
				e.Property(x => x.Email).HasMaxLength(200).IsRequired();
				e.Property(x => x.NormalizedEmail).HasMaxLength(200).IsRequired();
				e.Property(x => x.Phone).HasMaxLength(20).IsRequired();
				e.Property(x => x.NormalizedPhone).HasMaxLength(20).IsRequired();
				e.HasIndex(x => x.NormalizedEmail).IsUnique().HasFilter("IsDeleted = 0");
				e.HasIndex(x => x.NormalizedPhone).IsUnique().HasFilter("IsDeleted = 0");
				e.HasQueryFilter(x => !x.IsDeleted);
			});
		}
	}
}
