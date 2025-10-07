using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public class Session:BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region 1:M RS Between SessionCategory
        public int CategoryId { get; set; }
        public Category SessionCategory { get; set; }//One
        #endregion

        #region 1:M RS Between SessionTrainer
        //FK
        public int TrainerId { get; set; }
        public Trainer SessionTrainer { get; set; }//One
        #endregion
        #region M:M Between MemberSession
        public ICollection<MemberSession> SessionMembers { get; set; }
        #endregion
    }
}
