using DBProgrammingClass_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgramming3.Views
{
    public class IncidentController : Controller
    {
        // GET: Incident
        public ActionResult Index()
        {
            var context = new TechSupportEntities();

            if (DBProgrammingClass_3.Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            List<Incident> incidents = context.Incidents.OrderBy(x => x.IncidentID).ToList();

            ViewBag.Customers = context.Customers.ToList();
            ViewBag.Technicians = context.Technicians.ToList();
            ViewBag.Products = context.Products.ToList();
            
            return View(incidents);
        }

        public ActionResult AddIncident(int id = 0)
        {
            var context = new TechSupportEntities();

            if (DBProgrammingClass_3.Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            ViewBag.Customers = context.Customers.Where(x => x.Inactive == 0).ToList();
            ViewBag.Technicians = context.Technicians.ToList();
            ViewBag.Products = context.Products.ToList();

            Incident incident = context.Incidents.FirstOrDefault(i => i.IncidentID == id);

            if (id == 0)
            {
                incident = new Incident();
            }

            return View(incident);
        }

        [HttpPost]
        public ActionResult AddIncident(Incident incident)
        {
            var context = new TechSupportEntities();

            if (DBProgrammingClass_3.Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                if (ModelState.IsValid)
                {
                    context.Incidents.AddOrUpdate(incident);
                    context.SaveChanges();
                    TempData["SuccessMessage"] = $"Incident #{incident.IncidentID} successfully added!";

                    return Redirect("/Incident/Index");
                }

                return View(incident);
                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Incident not added, please try again! {ex.Message}";
                return Redirect("/Incident/Index");
            }            
        }

        public ActionResult Delete(int id)
        {
            var context = new TechSupportEntities();

            if (DBProgrammingClass_3.Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                Incident incidentToRemove = context.Incidents.FirstOrDefault(x => x.IncidentID == id);

                context.Incidents.Remove(incidentToRemove);
                context.SaveChanges();

                TempData["SuccessMessage"] = $"Incident #{incidentToRemove.IncidentID} successfully deleted!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Incident not deleted, please try again! {ex.Message}";
            }

            return Redirect("/Incident/Index");
        }
    }
}