using System.Web.Mvc;
using System.Web.Security;
using PureSurveyProjectTracker.Models.ViewModels;
using PureSurveyProjectTracker.Models.EntityManager;

namespace PureSurveyProjectTracker.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserSignUpView USV)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                if (!UM.IsLoginNameExist(USV.LoginName))
                {
                    UM.AddUserAccount(USV);
                    FormsAuthentication.SetAuthCookie(USV.FirstName, false);
                    return RedirectToAction("LogIn", "Account");

                }
                else
                    ModelState.AddModelError("", "Login Name already taken.");
            }
            return View();
        }
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserLoginView ULV, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                var user = UM.GetUserFromDatabase(ULV.LoginName);

                string password = UM.GetUserPassword(ULV.LoginName);

                if (string.IsNullOrEmpty(password))
                    ModelState.AddModelError("", "The user login or password provided is incorrect.");
                else
                {
                    if (ULV.Password.Equals(password))
                    {
                        FormsAuthentication.SetAuthCookie(ULV.LoginName, false);
                        
                        return RedirectToAction("Index", "Home", new { Id = user.SYSUserID });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The password provided is incorrect.");
                    }
                }
            }
            return View(ULV);
        }

        //public ActionResult Register()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult Register()
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("SignUp", "Account");
            }
            return View();
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("LogIn", "Account");
        }
    }
}