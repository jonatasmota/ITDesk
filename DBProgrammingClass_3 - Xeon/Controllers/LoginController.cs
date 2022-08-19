using DBProgrammingClass_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgrammingClass_3.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
                using (TechSupportEntities db = new TechSupportEntities())
                {
                    var obj = db.Users.Where(a => a.UserName.Equals(objUser.UserName) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session.Add("UserID", obj.UserId.ToString());
                        Session.Add("UserName", obj.UserName.ToString());
                        return RedirectToAction("Index", "Home");
                    }
                }
    
            return View(objUser);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            /*Session["UserName"] = null;*/
            return RedirectToAction("Login", "Login");
        }
    }
}