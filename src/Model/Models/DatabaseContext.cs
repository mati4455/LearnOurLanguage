using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Model.Models.Database;

namespace Model.Models
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Dictionary> Dictionaries { get; set; }
        //public virtual DbSet<UserDictionary> UserDictionaries { get; set; }
        public virtual DbSet<Translation> Translations { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameSession> GameSessions { get; set; }
        public virtual DbSet<GameSessionTranslation> GameSessionTranslations { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DatabaseContext()
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
