using Microsoft.EntityFrameworkCore;
using backend.Models;


namespace backend.Data
{
	public class CharacterDb : DbContext
	{
		public CharacterDb(DbContextOptions<CharacterDb> options) : base(options) { }
		public DbSet<Character> Characters { get; set; } = null!;

	}

	

}
