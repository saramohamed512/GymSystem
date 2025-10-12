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
    internal class TrainerService : ITrainerService
    {
        #region Fields
        private readonly IGenericRepository<Trainer> _trainerRepository;
    

        #endregion
        public TrainerService
            (
                IGenericRepository<Trainer> trainerRepository

            )
        {
           _trainerRepository = trainerRepository;
           
        }
        public bool CreateTrainers(CreateTrainerViewModel createTrainer)
        {
            //Check if Email or Phone are unique
            try
            {
                if (IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone))
                    return false;
                var trainer = new Trainer
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    Gender = createTrainer.Gender,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Address = new Address
                    {
                        BuildingNumber = createTrainer.BuildingNumber,
                        Street = createTrainer.Street,
                        City = createTrainer.City,
                    },
                    Specialites = createTrainer.Specialites

                };
                return _trainerRepository.Add(trainer) > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {

            var trainers = _trainerRepository.GetAll();
            if (trainers is null || !trainers.Any()) return [];
            var trainerViewModels = trainers.Select(trainer => new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialites= trainer.Specialites,
            });
            return trainerViewModels;
        }

        public TrainerViewModel? GetTrainerDetails(int id)
        {
            
            var trainer = _trainerRepository.GetById(id);
            if (trainer is null) return null;
            var trainerViewModel = new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = $"{trainer.Address?.BuildingNumber} . {trainer.Address?.Street} . {trainer.Address?.City}",
                Specialites = trainer.Specialites,

            };
            
            return trainerViewModel;
        }


        public TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var trainer = _trainerRepository.GetById(TrainerId);
            if (trainer is null) return null;
            var trainerToUpdate = new TrainerToUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address?.BuildingNumber ?? 0,
                Street = trainer.Address?.Street ?? string.Empty,
                City = trainer.Address?.City ?? string.Empty,
                Specialites = trainer.Specialites
            };
            return trainerToUpdate;
        }

        public bool RemoveTrainer(int id)
        {
            var trainer = _trainerRepository.GetById(id);
            if (trainer is null) return false;

            // Prevent deletion if trainer has future sessions
            if (trainer.TrainerSessions != null && trainer.TrainerSessions.Any(s => s.StartDate > DateTime.Now))
                return false;

            return _trainerRepository.Delete(trainer.Id) > 0;
        }

        public bool UpdateTrainerDetails(int id, TrainerToUpdateViewModel updateTrainer)
        {
            
            try
            {
                var trainer = _trainerRepository.GetById(id);
                if (trainer is null) return false;
                // Check if the updated email or phone already exists for another trainer
                if ((trainer.Email != updateTrainer.Email && IsEmailExist(updateTrainer.Email)) ||
                    (trainer.Phone != updateTrainer.Phone && IsPhoneExist(updateTrainer.Phone)))
                {
                    return false;
                }
                trainer.Email = updateTrainer.Email;
                trainer.Phone = updateTrainer.Phone;
                trainer.Address.BuildingNumber = updateTrainer.BuildingNumber;
                trainer.Address.Street = updateTrainer.Street;
                trainer.Address.City = updateTrainer.City;
                trainer.Specialites = updateTrainer.Specialites;
                return _trainerRepository.Update(trainer) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExist(string email)
        {
            return _trainerRepository.GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _trainerRepository.GetAll(m => m.Phone == phone).Any();
        }


        #endregion
    }
}