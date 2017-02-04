using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class GamesRepository : RepositoryBase<Game>, IGamesRepository
    {
        public GamesRepository(DatabaseContext db) : base(db)
        {
            
        }
    }
}
