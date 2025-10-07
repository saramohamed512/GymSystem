using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    internal class MemberSession:BaseEntity
    {
        public int MemberId { get; set; }// FK from Member
        public int SessionId { get; set; }// FK from Session
        public Member Member { get; set; }
        public Session Session { get; set; }
        //BookingDate Column Exists in BaseEntity
        public bool IsAttend { get; set; }
       
    }
}
