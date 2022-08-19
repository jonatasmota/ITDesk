using DBProgrammingClass_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgrammingClass_3.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            var context = new TechSupportEntities();

            List<User> users = context.Users
                    .OrderByDescending(x =>
                        x.UserId)
                    .ToList();

            return View(users);
        }

        public ActionResult Delete(int id)
        {
            var context = new TechSupportEntities();
            
            try
            {
                User userToRemove = context.Users.FirstOrDefault(x => x.UserId == id);

                context.Users.Remove(userToRemove);
                context.SaveChanges();
                TempData["SuccessMessage"] = $"{userToRemove.UserName} successfully deleted!";
                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"User not deleted, please try again! + {ex.Message}";
            }

            return Redirect("/User/Index");
        }


        public ActionResult AddUser()
        {
            User user = new User();

            user.DateAdded = DateTime.Now;

            return View("EditUser", user);
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            var context = new TechSupportEntities();

            User user = context.Users.FirstOrDefault(x => x.UserId == id);

            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            var context = new TechSupportEntities();
            
            try
            {
                if (ModelState.IsValid)
                {
                    user.UpdateDate = DateTime.Now;
                    context.Users.AddOrUpdate(user);
                    context.SaveChanges();
                    TempData["SuccessMessage"] = "User successfully added!";

                    return Redirect("/User/Index");
                }

                return View(user);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = $"User not added, please try again! {ex.Message}";
                return Redirect("/User/Index");
            }

        }
    }
}