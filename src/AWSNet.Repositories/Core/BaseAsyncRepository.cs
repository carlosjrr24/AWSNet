using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AWSNet.Repositories.Core
{
    public abstract class BaseAsyncRepository<TEntity> where TEntity : class
    {
        public virtual IUnitOfWork UnitOfWork { get; set; }

        public virtual DbSet<TEntity> Set { get; set; }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            try
            {
                TEntity savedEntity = Set.Add(entity);
                await UnitOfWork.Context.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                //TODO: logging
                throw e;
            }
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await Set.FindAsync(id);
        }

        public virtual async Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>> where)
        {
            return await Task.FromResult(Set.Where(where));
        }

        public virtual async Task<IQueryable<TEntity>> GetAll()
        {
            return await Task.FromResult(Set);
        }

        public virtual async Task Delete(TEntity entity)
        {
            try
            {
                Set.Remove(entity);
                await UnitOfWork.Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //TODO: logging
                throw e;
            }
        }

        public virtual async Task Update(TEntity entity)
        {
            try
            {
                UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
                await UnitOfWork.Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //TODO: logging
                throw e;
            }
        }

        public virtual async Task<int> Count(Expression<Func<TEntity, bool>> where = null)
        {
            return where == null ? await Set.CountAsync() : await Set.CountAsync(where);
        }
    }
}
