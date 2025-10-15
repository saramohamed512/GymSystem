using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.SessionsViewModel;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositroies.Classes;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork= unitOfWork;
        }
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            //var Sessions = _unitOfWork.GetRepository<Session>().GetAll();
            var Sessions = _unitOfWork.SessionRepoitory.GetAllSessionWithTrainerAndCategory();
            if (!Sessions.Any()) return [];

            return Sessions.Select(s => new SessionViewModel
            {
                Id = s.Id,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Capacity = s.Capacity,
                TrainerName = s.SessionTrainer.Name, //Related Entity
                CategoryName = s.SessionCategory.CategoryName, //Related Entity
                AvailableSlot = s.Capacity - _unitOfWork.SessionRepoitory.GetCountOfBookedSlots(s.Id)



            }); 
        }
    }
}
