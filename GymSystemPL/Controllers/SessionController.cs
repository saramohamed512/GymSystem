using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Get All Sessions
        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }
        #endregion
        #region Get Session Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction("Index");
            }
            var session = _sessionService.GetSessionById(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            return View(session);
        }
        #endregion
        #region Create Session
        public ActionResult Create()
        {
            LoadDropdownsForTrainers();
            LoadDropdownsForCategories();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateSessionViewModel CreatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdownsForTrainers();
                return View(CreatedSession);
            }
            var Result = _sessionService.CreateSession(CreatedSession);
            if (!Result)
            {
                TempData["ErrorMessage"] = "Failed to create session.";
                LoadDropdownsForTrainers();
                return View(CreatedSession);
            }
            else
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                LoadDropdownsForTrainers();
                return RedirectToAction("Index");


            }
        }
        #endregion
        #region Edit Session
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction("Index");
            }
            var Session = _sessionService.GetSessionToUpdate(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            LoadDropdownsForTrainers();
            return View(Session);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id,UpdateSessionViewModel UpdatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdownsForTrainers();
                return View(UpdatedSession);
            }
            var Result = _sessionService.UpdateSession(UpdatedSession, id);

            if (!Result)
            {
                TempData["ErrorMessage"] = "Failed to update session.";
                LoadDropdownsForTrainers();
                return View(UpdatedSession);
            }
            else
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Delete Session
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction("Index");
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            ViewBag.SessionId = id;
            return View(Session);
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _sessionService.RemoveSession(id);
            if (!Result)
            {
                TempData["ErrorMessage"] = "Failed to delete session.";
            }
            else
            {
                TempData["SuccessMessage"] = "Session deleted successfully.";
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Helper Methods
        private void LoadDropdownsForTrainers()
        {
            var Trainers = _sessionService.GetTrainerForSessions();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void LoadDropdownsForCategories()
        {

            var Categories = _sessionService.GetCategoryForSessions();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }
        #endregion

    }
}
