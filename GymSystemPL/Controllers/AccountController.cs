using GymSystemBLL.Services;
using GymSystemBLL.Services.Classes;
using GymSystemBLL.ViewModels.AccountViewModel;
using GymSystemDAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager) 
        {
            _accountService= accountService;
            _signInManager= signInManager;
        }
        #region Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("InvalidModel","Invalid Email or Password");
                return View(model);

            }
            var User = _accountService.ValidateUser(model);
            if (User == null)
            {
                ModelState.AddModelError("InvalidUser", "Invalid Email or Password");
                return View(model);
            }
            var Result = _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe,  false).Result;
            if(Result.IsNotAllowed)
             {
                ModelState.AddModelError("NotAllowed", "You are not allowed to login");
                
            }
            if(Result.IsLockedOut)
            {
                ModelState.AddModelError("LockedOut", "Your account is locked out");

            }
            if(Result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);


        }
        #endregion
        #region Logout

        #endregion
        #region Access Denied

        #endregion
    }
}
