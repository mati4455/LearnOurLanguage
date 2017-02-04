using Model.Base;
using Model.Models.Database;

namespace Model.Repositories.Interfaces
{
    public interface IUsersRepository : IRepositoryBase<User>
    {
        User PrepareData(User user);
    }
}
