using System.Collections.Generic;
using Model.Base;
using Model.Models.Database;

namespace Model.Repositories.Interfaces
{
    public interface IDictionariesRepository : IRepositoryBase<Dictionary>
    {
        IList<Dictionary> GetForUser(int userId);
        IList<Dictionary> GetAllPublic();
    }
}
