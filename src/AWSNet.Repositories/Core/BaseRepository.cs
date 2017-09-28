﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace AWSNet.Repositories.Core
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        public virtual IUnitOfWork UnitOfWork { get; set; }

        public virtual DbSet<TEntity> Set { get; set; }

        public virtual TEntity Add(TEntity entity)
        {
            try
            {
                TEntity savedEntity = Set.Add(entity);
                UnitOfWork.Context.SaveChanges();
                return entity;
            }
            catch (Exception e)
            {
                //TODO: logging
                throw e;
            }
        }

        public virtual TEntity GetById(int id)
        {
            return Set.Find(id);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return Set.FirstOrDefault(where);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Set;
        }

        public virtual void Delete(TEntity entity)
        {
            Set.Remove(entity);
            UnitOfWork.Context.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            try
            {
                UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
                UnitOfWork.Context.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: logging
                throw e;
            }
        }

        public int Count(Expression<Func<TEntity, bool>> where = null)
        {
            return where == null ? Set.Count() : Set.Count(where);
        }
    }
}
