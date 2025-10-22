using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;

using GymSystemDAL.Entities;

using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Trainer>();

                if (IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone)) return false;
                var Trainer = new Trainer()
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Specialites = createTrainer.Specialites,
                    Gender = createTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createTrainer.BuildingNumber,
                        City = createTrainer.City,
                        Street = createTrainer.Street,
                    }
                };


                Repo.Add(Trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers is null || !Trainers.Any()) return [];

            return Trainers.Select(X => new TrainerViewModel()
            {
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Specialites = X.Specialites
            });
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (Trainer is null) return null;

            return new TrainerViewModel
            {
                Email = Trainer.Email,
                Name = Trainer.Name,
                Phone = Trainer.Phone,
                Specialites = Trainer.Specialites
            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null) return null;

            return new TrainerToUpdateViewModel()
            {
                Name = Trainer.Name, // Display
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Street = Trainer.Address.Street,
                BuildingNumber = Trainer.Address.BuildingNumber,
                City = Trainer.Address.City,
                Specialites = Trainer.Specialites
            };
        }

        public bool RemoveTrainer(int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(trainerId);
            if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== UpdateTrainerDetails START ===");
                System.Diagnostics.Debug.WriteLine($"ID: {trainerId}");
                System.Diagnostics.Debug.WriteLine($"Email: {updatedTrainer?.Email}");
                System.Diagnostics.Debug.WriteLine($"Phone: {updatedTrainer?.Phone}");
                System.Diagnostics.Debug.WriteLine($"BuildingNumber: {updatedTrainer?.BuildingNumber}");
                System.Diagnostics.Debug.WriteLine($"Street: {updatedTrainer?.Street}");
                System.Diagnostics.Debug.WriteLine($"City: {updatedTrainer?.City}");
                System.Diagnostics.Debug.WriteLine($"Specialites: {updatedTrainer?.Specialites}");
                
                var Repo = _unitOfWork.GetRepository<Trainer>();
                var TrainerToUpdate = Repo.GetById(trainerId);

                if (TrainerToUpdate is null) 
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Trainer not found!");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"Found trainer: {TrainerToUpdate.Name}");
                System.Diagnostics.Debug.WriteLine($"Current email: {TrainerToUpdate.Email}");
                System.Diagnostics.Debug.WriteLine($"Current phone: {TrainerToUpdate.Phone}");

                // TEMPORARY: Skip validation for testing
                System.Diagnostics.Debug.WriteLine("SKIPPING VALIDATION FOR TESTING...");

                // Update trainer information
                System.Diagnostics.Debug.WriteLine("Updating trainer fields...");
                TrainerToUpdate.Email = updatedTrainer.Email;
                TrainerToUpdate.Phone = updatedTrainer.Phone;
                TrainerToUpdate.Address.BuildingNumber = updatedTrainer.BuildingNumber;
                TrainerToUpdate.Address.Street = updatedTrainer.Street;
                TrainerToUpdate.Address.City = updatedTrainer.City;
                TrainerToUpdate.Specialites = updatedTrainer.Specialites;
                TrainerToUpdate.UpdatedAt = DateTime.Now;

                System.Diagnostics.Debug.WriteLine("Calling Repo.Update...");
                Repo.Update(TrainerToUpdate);
                
                System.Diagnostics.Debug.WriteLine("Calling SaveChanges...");
                var changesCount = _unitOfWork.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"SaveChanges returned: {changesCount}");
                
                var saveResult = changesCount > 0;
                System.Diagnostics.Debug.WriteLine($"Final result: {saveResult}");
                System.Diagnostics.Debug.WriteLine($"=== UpdateTrainerDetails END ===");
                
                return saveResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION in UpdateTrainerDetails: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }




        #region Helper Methods

        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Phone == phone).Any();
        }
        private bool HasActiveSessions(int id)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(S => S.TrainerId == id && S.StartDate > DateTime.Now).Any();
            return activeSessions;
        }

        #endregion
    }


}