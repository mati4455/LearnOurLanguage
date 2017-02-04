using Model.Base;
using Model.Models;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace Model.Repositories
{
    public class RolesRepository : RepositoryBase<Role>, IRolesRepository
    {
        public RolesRepository(DatabaseContext db) : base(db)
        {

        }
    }
}
