using GymSystemDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    internal class Trainer: GymUser
    {
        //CreatedAt Column Exists in BaseEntity
        //I will use it to store the Employment Start Date
        public Specialites Specialites { get; set; }

        #region 1:M RS Between SessionTrainer

        public ICollection<Session> TrainerSessions{ get; set; }//One
        #endregion
    }
}
