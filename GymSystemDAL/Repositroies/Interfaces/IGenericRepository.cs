using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        IEnumerable<TEntity> GetAll(Func<TEntity, bool>?condition=null);
        TEntity? GetById(int id);
        int Add(TEntity entity);
        int Delete(int id);
        int Update(TEntity entity);
    }
}
