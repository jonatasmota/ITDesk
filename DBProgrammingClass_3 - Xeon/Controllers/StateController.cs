using DBProgrammingClass_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgrammingClass_3.Controllers
{
    public class StateController : Controller
    {
        // GET: State
        public ActionResult Index()
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            List<State> states = context.States
                    .OrderByDescending(x =>
                        x.StateCode)
                    .ToList();

            return View(states);
        }

        public ActionResult Delete(string id)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                State stateToRemove = context.States.FirstOrDefault(x => x.StateCode == id);

                context.States.Remove(stateToRemove);
                context.SaveChanges();
                TempData["SuccessMessage"] = $"{stateToRemove.StateName} successfully deleted!";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"State not deleted, please try again! + {ex.Message}";
            }

            return Redirect("/State/Index");
        }

        public ActionResult AddState()
        {
            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            State state = new State();

            return View("EditState", state);
        }

        [HttpGet]
        public ActionResult EditState(string id)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            State state = context.States.FirstOrDefault(x => x.StateCode == id);

            return View(state);
        }

        [HttpPost]
        public ActionResult EditState(State state)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                context.States.AddOrUpdate(state);
                context.SaveChanges();
                TempData["SuccessMessage"] = "State successfully updated!";

                return Redirect("/State/Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = $"State not updated, please try again! {ex.Message}";
                return Redirect("/State/Index");
            }
        }
    }
}