using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class TranslationsRepository : RepositoryBase<Translation>, ITranslationsRepository
    {
        public TranslationsRepository(DatabaseContext db) : base(db)
        {
            
        }
    }
}
