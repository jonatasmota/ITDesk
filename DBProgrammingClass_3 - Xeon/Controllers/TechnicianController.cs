using DBProgrammingClass_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgrammingClass_3.Controllers
{
    public class TechnicianController : Controller
    {
        // GET: Technician
        public ActionResult Index()
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            List<Technician> technicians = context.Technicians
                    .OrderByDescending(t =>
                        t.TechID)
                    .ToList();


            return View(technicians);
        }

        public ActionResult Delete(int id)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                Technician techToRemove = context.Technicians.FirstOrDefault(x => x.TechID == id);

                context.Technicians.Remove(techToRemove);
                context.SaveChanges();
                TempData["SuccessMessage"] = $"{techToRemove.Name} successfully deleted!";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Technician not deleted, please try again! + {ex.Message}";
            }

            return Redirect("/Technician/Index");
        }


        public ActionResult AddTechnician()
        {
            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            Technician technician = new Technician();

            return View("EditTechnician", technician);
        }

        [HttpGet]
        public ActionResult EditTechnician(int id)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            Technician technician = context.Technicians.FirstOrDefault(t => t.TechID == id);

            return View(technician);
        }

        [HttpPost]
        public ActionResult EditTechnician(Technician technician)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                if (ModelState.IsValid)
                {
                    context.Technicians.AddOrUpdate(technician);
                    context.SaveChanges();
                    TempData["SuccessMessage"] = "Technician successfully updated!";

                    return Redirect("/Technician/Index");
                }

                return View(technician);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Technician not added, please try again! {ex.Message}";
                return Redirect("/Technician/Index");
            }

        }
    }
}