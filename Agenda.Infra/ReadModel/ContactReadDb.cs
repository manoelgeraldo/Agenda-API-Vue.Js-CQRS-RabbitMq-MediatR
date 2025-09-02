using Agenda.Domain.Contracts;
using Agenda.Domain.Entities;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Agenda.Infra.ReadModel
{
	public class ContactReadDb : IContactReadDb
	{
		private readonly string _connString;
		public ContactReadDb(IConfiguration cfg) => _connString = cfg.GetConnectionString("Default")!;


		public async Task<Contact?> GetByEmailAsync(string email)
		{
			using var con = new SqliteConnection(_connString);
			var sql = @"SELECT Id, 
							         Name, 
							         Email, 
							         NormalizedEmail, 
							         Phone, 
							         NormalizedPhone, 
							         CreatedAt, 
							         UpdatedAt, 
							         IsDeleted 
							   FROM Contacts
							   WHERE NormalizedEmail = @e
								 AND IsDeleted = 0 LIMIT 1";
			return await con.QueryFirstOrDefaultAsync<Contact>(sql, new { e = email.Trim().ToLowerInvariant() });
		}
	}
}