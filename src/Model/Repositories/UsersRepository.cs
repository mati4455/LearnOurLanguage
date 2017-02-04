using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.Base;
using Model.Helpers;
using Model.Models;
using Model.Models.Account;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class UsersRepository : RepositoryBase<User>, IUsersRepository
    {
        public UsersRepository(DatabaseContext db) : base(db)
        {

        }

        public User PrepareData(User user)
        {
            if (user.Password.Length > 0)
            {
                var salt = HashHelper.GenerateSalt();
                var password = HashHelper.ComputeHash(user.Password.Trim(), salt);
                user.Salt = salt;
                user.Password = password;
            }
            else
            {
                var u = GetById(user.Id);
                user.Salt = u.Salt;
                user.Password = u.Password;
                Context.Entry(u).State = EntityState.Detached;
            }
            return user;
        }
    }
}
