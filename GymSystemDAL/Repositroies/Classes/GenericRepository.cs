﻿using GymSystemDAL.Data.Context;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entities.BaseEntity, new()
    {
       public  readonly GymSystemDBContext _dbContext;
        public GenericRepository(GymSystemDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
          
        }

        public void Delete(TEntity entity)
        {
           
                _dbContext.Set<TEntity>().Remove(entity);
               
           
          
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition= null)
        {
            if (condition == null)
                 return _dbContext.Set<TEntity>().ToList();
            else
                return _dbContext.Set<TEntity>().Where(condition).ToList();
        }

        public TEntity? GetById(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
          
        }
    }
}
