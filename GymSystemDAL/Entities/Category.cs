using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    internal class Category: BaseEntity
    {
        public string CategoryName { get; set; } = null!;

        #region 1:M RS Between SessionCategory
        public  ICollection<Session> Sessions { get; set; }//Many
        #endregion
    }
}
