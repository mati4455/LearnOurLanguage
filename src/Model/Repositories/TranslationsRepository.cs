using System.Collections.Generic;
using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;
using System.Linq;

namespace Model.Repositories
{
    public class TranslationsRepository : RepositoryBase<Translation>, ITranslationsRepository
    {
        public TranslationsRepository(DatabaseContext db) : base(db)
        {

        }

        public IEnumerable<Translation> GetTranslationsForDictionary(int dictionaryId)
        {
            return GetAll()
                //.Include(x => x.Dictionary.FirstLanguage)
                //.Include(x => x.Dictionary.SecondLanguage)
                .Where(x => x.DictionaryId == dictionaryId);
        }
    }
}
