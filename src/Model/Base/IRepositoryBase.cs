using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Model.Base
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        bool Insert(params T[] entities);
        bool Insert(IEnumerable<T> entities);

        bool Delete(params T[] entities);
        bool Delete(params int[] ids);
        bool Delete(IEnumerable<int> ids);
        bool Delete(IEnumerable<T> ids);

        bool Update(params T[] entities);
        bool Update(IEnumerable<T> entities);

        bool Save();
    }
}