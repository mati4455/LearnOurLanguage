using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class UserDictionariesRepository : RepositoryBase<UserDictionary>, IUserDictionariesRepository
    {
        public UserDictionariesRepository(DatabaseContext db) : base(db)
        {

        }
    }
}
