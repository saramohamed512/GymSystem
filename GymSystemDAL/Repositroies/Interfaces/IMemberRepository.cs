using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Interfaces
{
    internal interface IMemberRepository
    {

        IEnumerable<Entities.Member> GetAll();
        Entities.Member? GetById(int id);

        int Add(Entities.Member member);

        int Delete(int id);

        int Update(Entities.Member member);

    }
}
