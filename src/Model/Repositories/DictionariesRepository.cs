using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class DictionariesRepository : RepositoryBase<Dictionary>, IDictionariesRepository
    {
        public DictionariesRepository(DatabaseContext db) : base(db)
        {
            
        }
    }
}
