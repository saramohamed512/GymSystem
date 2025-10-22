using GymSystemBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
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
                return RedirectToAction(nameof(Index));
            var memberDetails = _memberService.GetMemberDetails(id);
            if(memberDetails == null)
                return RedirectToAction(nameof(Index));
            return View(memberDetails);
        }
        #endregion
        #region Get Member Health record
        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(Index));
            var memberHealthRecord = _memberService.GetMemberHealthRecordDetails(id);
            Console.WriteLine("Member Health Record Not Found");
            Console.WriteLine(memberHealthRecord);
            if (memberHealthRecord == null)
                
            return RedirectToAction(nameof(Index));


            return View(memberHealthRecord);
        }
        #endregion
    }
}
