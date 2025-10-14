using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Interfaces
{
    public interface IUnitOfWork
    {
        //Call IUnitOfWork For Asking Specific Repository
        //Method To Get Repository
        //Method To Save Changes

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity , new();
        int SaveChanges();
    }
}
