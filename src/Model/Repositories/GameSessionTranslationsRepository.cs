using System.Linq;
using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class GameSessionTranslationsRepository : 
        RepositoryBase<GameSessionTranslation>, IGameSessionTranslationsRepository
    {
        public GameSessionTranslationsRepository(DatabaseContext db) : base(db)
        {
        }
        public void DeleteByDictionaryId(int dictionaryId)
        {
            var gst = GetAll().Where(x => x.GameSession.DictionaryId == dictionaryId).ToList();
            Delete(gst);
        }
    }
}
