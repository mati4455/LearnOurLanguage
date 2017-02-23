using System.Linq;
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

        public void DeleteByDictionaryId(int dictionaryId)
        {
            var gs = GetAll().Where(x => x.DictionaryId == dictionaryId).ToList();
            Delete(gs);
        }
    }
}
