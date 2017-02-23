using System.Collections.Generic;
using Model.Base;
using Model.Models.Database;

namespace Model.Repositories.Interfaces
{
    public interface ITranslationsRepository : IRepositoryBase<Translation>
    {
        IEnumerable<Translation> GetTranslationsForDictionary(int dictionaryId);
        void DeleteByDictionaryId(int dictionaryId);
    }
}
