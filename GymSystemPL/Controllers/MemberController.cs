using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class MemberController : Controller
    {
        //ASK CLR to Inject object from meber service layer
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }//Register for Service in Program.cs


        #region Get All Members
        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        #endregion
        #region Get Member Details
        public ActionResult MemberDetails(int id)
        {
            if(id <= 0) 
            {
                TempData["ErrorMessage"] = "Id Can't be 0 or Negative Value";
                return RedirectToAction(nameof(Index));

            }
            var memberDetails = _memberService.GetMemberDetails(id);
            if(memberDetails == null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));

            }
            return View(memberDetails);
        }
        #endregion
        #region Get Member Health record
        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't be 0 or Negative Value";
                return RedirectToAction(nameof(Index));

            }
            var memberHealthRecord = _memberService.GetMemberHealthRecordDetails(id);
            
            if (memberHealthRecord == null)

            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));

            }


            return View(memberHealthRecord);
        }
        #endregion

        #region Create Member

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreatedMember)
        {
            if(!ModelState.IsValid)
            {
               ModelState.AddModelError("Invalid Data","Check Data And Missing Fields");
                return View("Create",CreatedMember);
            }
            bool Result = _memberService.CreateMembers(CreatedMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Member";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion
        #region Edit Member
        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't be 0 or Negative Value";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberToUpdate(id);
            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id,MemberToUpdateViewModel MemberToUpdate) 
        { 
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("Invalid Data", "Check Data And Missing Fields");
                return View( MemberToUpdate);
            }
            bool Result = _memberService.UpdateMemberDetails(id, MemberToUpdate);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Member";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't be 0 or Negative Value";
                return RedirectToAction(nameof(Index));
            }
            var Result = _memberService.GetMemberDetails(id);
            if (Result==null)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = Result.Name;
            return View();
        }
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
           var Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Member";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
