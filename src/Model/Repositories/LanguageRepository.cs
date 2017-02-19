using System;
using System.Collections.Generic;
using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;
using System.Linq;

namespace Model.Repositories
{
    public class LanguageRepository : RepositoryBase<Language>, ILanguagesRepository
    {
        public LanguageRepository(DatabaseContext db) : base(db)
        {
            
        }

        public IEnumerable<Language> GetSortedLanguages()
        {

            return GetAll().OrderBy(x=>x.Name);
                
        }
    }
}
