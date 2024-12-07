using Microsoft.EntityFrameworkCore;

namespace PruebasConNAudio
{

    public class BlackItem
    {
        public int Id { get; set; }
        public string windowTitle { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<BlackItem> blackList { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Program.sqldbname);
        }
    }
}
