using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Interfaces
{
    public interface ISessionRepoitory: IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionWithTrainerAndCategory();

        int GetCountOfBookedSlots(int sessionId);
        Session? GetSessionWithTrainerAndCategory(int session);
    }
}
