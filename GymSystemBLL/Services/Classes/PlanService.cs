using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.PlanViewModel;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
           var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];
            return plans.Select(plan => new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive,
            });
        }

        public PlanViewModel? GetPlanById(int id)
        {
            
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null) return null;
            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive,
            };

        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {

            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            //Check Active Memberships with this Plan
            if (plan is null || plan.IsActive == false || HasActiveMembership(PlanId))  return null;
            return new UpdatePlanViewModel
            {
                PlanName = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
            };
        }

        public bool ToggleStatus(int PlanId)
        {
            

            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            //Check Active Memberships with this Plan
            if (plan is null || HasActiveMembership(PlanId))  return false;
          
            try
            {
                if (plan.IsActive)
                {
                    //If Active Check Active Memberships
                    plan.IsActive= false;
                }
                plan.IsActive = !plan.IsActive;
                plan.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatedPlan)
        {
           

            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            //Check Active Memberships with this Plan
            if (plan is null || plan.IsActive == false || HasActiveMembership(PlanId)) return false;
            try
            {
               //Tuples [C# New Feature]
                (plan.Name, plan.Description, plan.Price, plan.DurationDays) =
                     (updatedPlan.PlanName, updatedPlan.Description, updatedPlan.Price, updatedPlan.DurationDays);
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #region Helper Methods
        private bool HasActiveMembership(int planId)
        {
            var ActiveMembership = _unitOfWork.GetRepository<Membership>()
                .GetAll(m => m.PlanId == planId && m.Status=="Active");
            return ActiveMembership.Any();

        }
        #endregion
    }
}
