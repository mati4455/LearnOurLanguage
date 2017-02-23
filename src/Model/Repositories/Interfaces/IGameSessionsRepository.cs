using Model.Base;
using Model.Models.Database;

namespace Model.Repositories.Interfaces
{
    public interface IGameSessionsRepository : IRepositoryBase<GameSession>
    {
        void DeleteByDictionaryId(int dictionaryId);
    }
}
