using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public class Member : GymUser
    {
        //CreatedAt Column Exists in BaseEntity
        //I will use it to store the Membership Start Date

        public string? Photo { get; set; }

        #region 1:1 RS Between Member HealthRecord
        //Navigation Property
        public HealthRecord HealthRecord { get; set; }
        #endregion
        #region M:M Between MemberPlan
        public ICollection<Membership> Memberships { get; set; }
        #endregion
        #region M:M Between MemberSession
        public ICollection<MemberSession> MemberSessions { get; set; }
        #endregion
    }
}
