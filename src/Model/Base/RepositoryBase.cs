using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace Model.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseModel, new()
    {
        private DbSet<T> _dbSet;
        //protected ILogger<RepositoryBase<T>> Logger => App.LoggerFactory.CreateLogger<RepositoryBase<T>>();

        public RepositoryBase()
        {
            //Logger.LogInformation($"init repository ({typeof(T).Name})");
        }

        public RepositoryBase(DatabaseContext db) : this()
        {
            Context = db;
        }

        protected DatabaseContext Context { get; set; }

        private DbSet<T> Collection => _dbSet ?? (_dbSet = Context.Set<T>());

        public virtual IQueryable<T> GetAll()
        {
            return Collection;
        }

        public virtual T GetById(int id)
        {
            return Collection.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = Collection.Where(predicate);
            return query;
        }

        public virtual bool Insert(params T[] entities)
        {
            try
            {
                foreach (var entity in entities)
                    Collection.Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return false;
            }
        }

        public virtual bool Insert(IEnumerable<T> entities)
        {
            try
            {
                foreach (var entity in entities)
                    Collection.Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return false;
            }
        }

        public virtual bool Delete(params T[] entities)
        {
            try
            {
                foreach (var entity in entities)
                    Collection.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return false;
            }
        }

        public virtual bool Delete(params int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var e = new T {Id = id};
                    Context.Attach(e);
                    Context.Remove(e);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return false;
            }
        }

        public virtual bool Delete(IEnumerable<int> ids)
        {
            return Delete(ids.ToArray());
        }

        public virtual bool Delete(IEnumerable<T> ids)
        {
            return Delete(ids.ToArray());
        }

        public virtual bool Update(params T[] entities)
        {
            try
            {
                foreach (var entity in entities)
                    Context.Entry(entity).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return false;
            }
        }

        public virtual bool Update(IEnumerable<T> entities)
        {
            return Update(entities.ToArray());
        }

        public virtual bool Save()
        {
            try
            {
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return false;
            }
        }

        private void LogException(Exception ex)
        {
            //Logger.LogError($"repository db error: {ex.Message}");
        }
    }
}