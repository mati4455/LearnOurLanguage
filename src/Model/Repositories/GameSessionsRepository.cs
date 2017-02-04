using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class GameSessionsRepository : RepositoryBase<GameSession>, IGameSessionsRepository
    {
        public GameSessionsRepository(DatabaseContext db) : base(db)
        {
            
        }
    }
}
