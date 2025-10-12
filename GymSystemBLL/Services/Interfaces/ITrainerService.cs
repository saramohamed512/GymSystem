using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymSystemBLL.ViewModels;

namespace GymSystemBLL.Services.Interfaces
{
    internal interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainers(CreateTrainerViewModel CreateTrainerViewModel);

        TrainerViewModel? GetTrainerDetails(int id);

    

        //GetTrainerId To Update view
        TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId);
        //apply update
        bool UpdateTrainerDetails(int id, TrainerToUpdateViewModel updateTrainer);

        bool RemoveTrainer(int id);
    }
}
