using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Model.Models.Database;

namespace Model.Models
{
    public static class DatabaseContextDataSeed
    {
        public static void SeedData(this IServiceScopeFactory scopeFactory)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
                if (!context.Roles.Any())
                {
                    var roles = new List<Role>
                    {
                        new Role
                        {
                            Name = "User",
                            AccessLevel = 100
                        }
                    };
                    context.AddRange(roles);
                    context.SaveChanges();
                }

                if (!context.Languages.Any())
                {
                    var languages = new List<Language>
                    {
                        new Language { Name = "Polski", Code="pl-PL" },
                        new Language { Name = "Angielski", Code="en-US" },
                        new Language { Name = "Niemiecki", Code="de-DE" },
                        new Language { Name = "Francuski", Code="fd-FR" },
                        new Language { Name = "Włoski", Code="it-IT" },
                        new Language { Name = "Szwedzki", Code="sv-SE" },
                        new Language { Name = "Czeski", Code="cs-CZ" },
                        new Language { Name = "Rosyjski", Code="ru-RU" },
                        new Language { Name = "Hiszpański", Code="es-ES" }
                    };
                    context.AddRange(languages);
                    context.SaveChanges();
                }

                if (!context.Games.Any())
                {
                    var languages = new List<Game>
                    {
                        new Game { Name = "Fiszki" },
                        new Game { Name = "Test" },
                        new Game { Name = "Memo" },
                        new Game { Name = "Wisielec" }
                    };
                    context.AddRange(languages);
                    context.SaveChanges();
                }
            }
        }
    }
}
