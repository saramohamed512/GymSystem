using GymSystemDAL.Data.Context;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymSystemDBContext _dBContext;

        public PlanRepository(GymSystemDBContext dBContext)
        {
          _dBContext = dBContext;
        }
        public IEnumerable<Plan> GetAll()
        {
            return _dBContext.Plans.ToList();

        }

        public Plan? GetById(int id)
        {
            return _dBContext.Plans.Find(id);
        }

        public int Update(Plan plan)
        {
            _dBContext.Plans.Update(plan);
            return _dBContext.SaveChanges();
        }
    }
}
