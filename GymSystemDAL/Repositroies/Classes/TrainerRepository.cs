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
    internal class TrainerRepository : ITrainerRepository
    {
        private readonly GymSystemDBContext _dBContext;

        public TrainerRepository(GymSystemDBContext dBContext) {
            _dBContext = dBContext;
        }

        public int Add(Trainer trainer)
        {
            _dBContext.Trainers.Add(trainer);
            return _dBContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var trainer = _dBContext.Trainers.Find(id);
            if (trainer != null)
            {
                _dBContext.Trainers.Remove(trainer);
                return _dBContext.SaveChanges();
            }
            return 0;
        }

        public IEnumerable<Trainer> GetAll()
        {
            return _dBContext.Trainers.ToList();
        }

        public Trainer? GetById(int id)
        {
            return _dBContext.Trainers.Find(id);
        }

        public int Update(Trainer trainer)
        {

            _dBContext.Trainers.Update(trainer);
            return _dBContext.SaveChanges();
        }
    }
}
