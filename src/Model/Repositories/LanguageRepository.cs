using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class LanguageRepository : RepositoryBase<Language>, ILanguagesRepository
    {
        public LanguageRepository(DatabaseContext db) : base(db)
        {
            
        }
    }
}
