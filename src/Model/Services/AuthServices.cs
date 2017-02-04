using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model.Helpers;
using Model.Models.Account;
using Model.Repositories.Interfaces;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class AuthService : IAuthService
    {
        private IUsersRepository UsersRepository { get; }

        public AuthService(IUsersRepository usersRepo)
        {
            UsersRepository = usersRepo;
        }
        
        public AppUserVo CheckAuthorization(AppUserAuthVo u)
        {
            var user = UsersRepository
                .GetAll()
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Login.Equals(u.Login));

            var accepted = false;
            if (user != null)
            {
                var userPassword = HashHelper.ComputeHash(u.Password, user.Salt);
                accepted = userPassword.Equals(user.Password);
            }

            return accepted 
                ? Mapper.Map<AppUserVo>(user)
                : new AppUserVo { AccessLevel = -1 };
        }
    }
}
