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
    internal class MemberRepository : IMemberRepository
    {
        //Connect to DB
        private readonly GymSystemDBContext _dbContext = new GymSystemDBContext();
        public int Add(Member member)
        {
            _dbContext.Members.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var member = _dbContext.Members.Find(id);
            if (member is null) return 0;
         
            _dbContext.Members.Remove(member);
            return _dbContext.SaveChanges();

        }

        public IEnumerable<Member> GetAll()  => _dbContext.Members.ToList();
        

        public Member? GetById(int id) => _dbContext.Members.Find(id);

        
       

        public int Update(Member member)
        {
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();
        }
    }
}
