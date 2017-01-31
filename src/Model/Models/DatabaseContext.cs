using Microsoft.EntityFrameworkCore;

namespace Model.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().Property(x => x.Salt).HasColumnType("varbinary");
        }
    }
}
