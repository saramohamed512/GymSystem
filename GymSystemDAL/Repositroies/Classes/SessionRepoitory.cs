using GymSystemDAL.Data.Context;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositroies.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Classes
{
    public class SessionRepoitory : GenericRepository<Session>, ISessionRepoitory
    {

        private readonly GymSystemDBContext _dBContext;
        //Chain CTOR Parent
        public SessionRepoitory(GymSystemDBContext dBContext):base(dBContext)
        {
            _dBContext= dBContext;
        }
        public IEnumerable<Session> GetAllSessionWithTrainerAndCategory()
        {
            return _dBContext.Sessions.Include(s => s.SessionTrainer)
                .Include(s => s.SessionCategory)
                .ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dBContext.MemberSessions.Count(b => b.SessionId == sessionId);

        }
    }
}
