using Model.Base;
using Model.Models.Database;
using System.Collections.Generic;

namespace Model.Repositories.Interfaces
{
    public interface ILanguagesRepository : IRepositoryBase<Language>
    {
        IEnumerable<Language> GetSortedLanguages();
    }
}
