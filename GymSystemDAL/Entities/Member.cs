using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    internal class Member : GymUser
    {
        //CreatedAt Column Exists in BaseEntity
        //I will use it to store the Membership Start Date

        public string? Photo { get; set; }
    }
}
