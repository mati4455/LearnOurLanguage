using Model.Models.Account;

namespace Model.Services.Interfaces
{
    public interface IAuthService
    {
        AppUserVo CheckAuthorization(AppUserAuthVo user);
    }
}
