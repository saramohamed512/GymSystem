using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GymSystemPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        public PlanController(IPlanService planService) 
        {
            _planService=planService;
        }//Regiister in Service  in Program.cs

        #region Get All Plans
        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }
        #endregion
        #region Get Plan Details
        public ActionResult Details(int id)
        {
           if(id <= 0)
           {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number !";
                return RedirectToAction("Index");
           }
           var Plan = _planService.GetPlanById(id);
           if(Plan == null)
           {
                 TempData["ErrorMessage"] = "Plan Not Found !";
                 return RedirectToAction("Index");
           }
           return View(Plan);

        }
        #endregion
        #region Edit Plan
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number !";
                return RedirectToAction("Index");
            }
            var Plan = _planService.GetPlanToUpdate(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found !";
                return RedirectToAction("Index");
            }
            return View(Plan);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel UpdatedPlan )
        {
            if (!ModelState.IsValid)
            {
               ModelState.AddModelError( "Wrong Data !" ,"Check Data Again !");
                return View(UpdatedPlan);
            }
            var isUpdated = _planService.UpdatePlan(id, UpdatedPlan);
            if (!isUpdated)
            {
                TempData["ErrorMessage"] = "Failed to Update Plan !";
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Plan Updated Successfully !";
            return RedirectToAction("Index");

        }
        #endregion

    }
}
