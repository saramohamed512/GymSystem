using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class TrainerController : Controller
    {
        //ASK CLR to Inject object from trainer service layer
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }//Register for Service in Program.cs


        #region Get All Trainers
        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }
        #endregion

        #region Get Trainer Details
        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't be 0 or Negative Value";
                return RedirectToAction(nameof(Index));

            }
            var trainerDetails = _trainerService.GetTrainerDetails(id);
            if (trainerDetails == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found!";
                return RedirectToAction(nameof(Index));

            }
            return View(trainerDetails);
        }
        #endregion

        #region Create Trainer

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateTrainerViewModel CreatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Invalid Data", "Check Data And Missing Fields");
                return View("Create", CreatedTrainer);
            }
            bool Result = _trainerService.CreateTrainer(CreatedTrainer);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Trainer";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Edit Trainer
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't be 0 or Negative Value";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerToUpdate(id);
            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found!";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, TrainerToUpdateViewModel TrainerToUpdate)
        {
            // Debug: Log the received data
            System.Diagnostics.Debug.WriteLine($"Edit POST - ID: {id}");
            System.Diagnostics.Debug.WriteLine($"Email: {TrainerToUpdate?.Email}");
            System.Diagnostics.Debug.WriteLine($"Phone: {TrainerToUpdate?.Phone}");
            System.Diagnostics.Debug.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            
            // Add a simple debug message to TempData to see if the method is being called
            TempData["DebugMessage"] = $"Edit POST called with ID: {id}, Email: {TrainerToUpdate?.Email}";
            
            // Test if we can get the trainer from the service
            var testTrainer = _trainerService.GetTrainerDetails(id);
            TempData["DebugMessage"] += $" | Test trainer: {testTrainer?.Name}";
            
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                ModelState.AddModelError("Invalid Data", "Check Data And Missing Fields");
                return View(TrainerToUpdate);
            }
            
            System.Diagnostics.Debug.WriteLine("About to call UpdateTrainerDetails...");
            bool Result = _trainerService.UpdateTrainerDetails(TrainerToUpdate, id);
            System.Diagnostics.Debug.WriteLine($"Update Result: {Result}");
            
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Trainer. Email or Phone may already exist for another trainer.";
                TempData["DebugMessage"] = $"Update failed. Result: {Result}";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Trainer
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't be 0 or Negative Value";
                return RedirectToAction(nameof(Index));
            }
            var Result = _trainerService.GetTrainerDetails(id);
            if (Result == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = Result.Name;
            return View();
        }
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = _trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Trainer";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
