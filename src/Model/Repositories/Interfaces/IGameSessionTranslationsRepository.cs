using Model.Base;
using Model.Models.Database;

namespace Model.Repositories.Interfaces
{
    public interface IGameSessionTranslationsRepository : IRepositoryBase<GameSessionTranslation>
    {
        void DeleteByDictionaryId(int dictionaryId);
    }
}
