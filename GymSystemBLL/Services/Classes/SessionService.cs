using AutoMapper;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.SessionsViewModel;
using GymSystemBLL.ViewModels.SessionViewModels;
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
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork ,IMapper mapper) 
        {
            _unitOfWork= unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {
                //check if trainer exists
                //check if category exists
                //check if start date < end date
                if (!IsTrainerExists(createdSession.TrainerId) || !IsCategoryExists(createdSession.CategoryId) || !IsDateTimeValid(createdSession.StartDate, createdSession.EndDate))
                    return false;
                if(createdSession.Capacity>25 || createdSession.Capacity<0) return false;

                var SessionEnttity=_mapper.Map< Session>(createdSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEnttity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
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

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = _unitOfWork.SessionRepoitory.GetSessionWithTrainerAndCategory(sessionId);
            if (session == null) return null;
            //return new SessionViewModel
            //{
            //    Id = session.Id,
            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    Capacity = session.Capacity,
            //    TrainerName = session.SessionTrainer.Name, //Related Entity
            //    CategoryName = session.SessionCategory.CategoryName, //Related Entity
            //    AvailableSlot = session.Capacity - _unitOfWork.SessionRepoitory.GetCountOfBookedSlots(session.Id)
            //};

            //Auto Mapper
            var MappedSession = _mapper.Map<Session, SessionViewModel>(session);
            MappedSession.AvailableSlot = MappedSession.Capacity - _unitOfWork.SessionRepoitory.GetCountOfBookedSlots(session.Id);
            return MappedSession;
        }
        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepoitory.GetById(sessionId);
            if (!IsSessionAvilableForUpdate(session!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(session);
        }

        public bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepoitory.GetById(sessionId);
                if (!IsSessionAvilableForUpdate(session!)) return false;
                //check if trainer exists
                //check if category exists
                //check if start date < end date
                if (!IsTrainerExists(updatedSession.TrainerId) || !IsDateTimeValid(updatedSession.StartDate, updatedSession.EndDate))
                    return false;
                _mapper.Map(updatedSession, session);
                session!.UpdatedAt = DateTime.Now;
                _unitOfWork.SessionRepoitory.Update(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveSession(int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepoitory.GetById(sessionId);
                if (!IsSessionAvilableForDelete(session!)) return false;
                _unitOfWork.SessionRepoitory.Delete(session!);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {

                return false;
            }
        }
        #region Helper Methods
        private bool IsTrainerExists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
           
        }
        private bool IsCategoryExists(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }
        private bool IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate;
        }

        private bool IsSessionAvilableForUpdate(Session session)
        {
            if(session is null) return false;
            //if session completed => cannot update
            if(session.EndDate < DateTime.Now) return false;
            //if session has started => cannot update
            if(session.StartDate < DateTime.Now) return false;
            //if session has members booked => cannot update
            var ActiveBookings = _unitOfWork.SessionRepoitory.GetCountOfBookedSlots(session.Id)>0;
            if(ActiveBookings) return false;
            return true;
        }

        private bool IsSessionAvilableForDelete(Session session)
        {
            if (session is null) return false;
            //if session completed => cannot update
            if (session.EndDate < DateTime.Now) return false;
            //if session upcoming => can delete
            if (session.StartDate > DateTime.Now) return false;
            //is session ongoing => can delete
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            //if session has started => cannot update
            //if (session.StartDate < DateTime.Now) return false;
            //if session has members booked => cannot update
            var ActiveBookings = _unitOfWork.SessionRepoitory.GetCountOfBookedSlots(session.Id) > 0;
            if (ActiveBookings) return false;
            return true;
        }


        #endregion
    }
}
