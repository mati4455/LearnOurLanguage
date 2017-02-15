using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class DictionariesRepository : RepositoryBase<Dictionary>, IDictionariesRepository
    {
        public DictionariesRepository(DatabaseContext db) : base(db)
        {
            
        }

        public IList<Dictionary> GetForUser(int userId)
        {
            return GetAll()
                .Include(x => x.FirstLanguage)
                .Include(x => x.SecondLanguage)
                .Where(x => x.UserId == userId)
                .ToList();
        }
        public IList<Dictionary> GetAllPublic()
        {
            return GetAll()
                .Include(x => x.FirstLanguage)
                .Include(x => x.SecondLanguage)
                .Where(x => x.Public)
                .ToList();
        }
    }
}
