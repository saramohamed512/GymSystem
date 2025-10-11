using GymSystemDAL.Data.Context;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Classes
{
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entities.BaseEntity, new()
    {
       public  readonly GymSystemDBContext _dbContext;
        public GenericRepository(GymSystemDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);
            if (entity != null) {
                _dbContext.Set<TEntity>().Remove(entity);
                return _dbContext.SaveChanges();
            }
            return 0;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }

        public TEntity? GetById(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public int Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return _dbContext.SaveChanges();
        }
    }
}
