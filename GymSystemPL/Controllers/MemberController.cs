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

        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
    }
}
