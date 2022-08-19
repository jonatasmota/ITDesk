using DBProgrammingClass_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBProgrammingClass_3.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            List<Registration> registrations = context.Registrations.OrderBy(x => x.RegistrationDate).ToList();

            ViewBag.Customers = context.Customers.OrderBy(x => x.Name).ToList();
            ViewBag.Products = context.Products.OrderBy(x => x.Name).ToList();

            return View(registrations);
        }

        public ActionResult AddRegistration(int id = 0)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            ViewBag.Customers = context.Customers.Where(x => x.Inactive == 0).ToList();
            ViewBag.Products = context.Products.ToList();

            Registration registration = context.Registrations.FirstOrDefault(x => x.CustomerID == id);

            if (id == 0)
            {
                registration = new Registration();
            }

            return View(registration);
        }

        [HttpPost]
        public ActionResult AddRegistration(Registration registration)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            ViewBag.Customers = context.Customers.Where(x => x.Inactive == 0).ToList();
            ViewBag.Customers = context.Customers.ToList();

            try
            {
                context.Registrations.AddOrUpdate(registration);
                context.SaveChanges();

                TempData["SuccessMessage"] = $"Registration successfully added!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Registration not added, please try again! {ex.Message}";
                return Redirect("/Registration/Index");
            }

            return Redirect("/Registration/Index");
        }

        public ActionResult EditRegistration(Registration registration)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            registration.Customer = context.Customers.FirstOrDefault(x => x.CustomerID == registration.CustomerID);
            registration.Product = context.Products.FirstOrDefault(x => x.ProductCode == registration.ProductCode);

            ViewBag.Customers = context.Customers.Where(x => x.Inactive == 0).ToList();
            ViewBag.Products = context.Products.ToList();

            return View(registration);
        }

        [HttpPost]
        public ActionResult EditRegistration(int? originalCust, string originalProd, DateTime originalDate, Registration registration)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            registration.Customer = context.Customers.FirstOrDefault(x => x.CustomerID == registration.CustomerID);
            registration.Product = context.Products.FirstOrDefault(x => x.ProductCode == registration.ProductCode);

            ViewBag.Customers = context.Customers.Where(x => x.Inactive == 0).ToList();
            ViewBag.Customers = context.Customers.ToList();

            Registration oldRegistration = context.Registrations.FirstOrDefault(x => x.ProductCode == originalProd && x.CustomerID == originalCust && x.RegistrationDate == originalDate);

            try
            {
                context.Registrations.Remove(oldRegistration);
                context.Registrations.AddOrUpdate(registration);
                context.SaveChanges();

                TempData["SuccessMessage"] = $"Registration successfully updated!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Registration not updated, please try again! {ex.Message}";
                return Redirect("/Registration/Index");
            }

            return Redirect("/Registration/Index");
        }

        public ActionResult Delete(string productCode, int customerID)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                Registration registrationToRemove = context.Registrations.FirstOrDefault(x => x.ProductCode == productCode && x.CustomerID == customerID);

                context.Registrations.Remove(registrationToRemove);
                context.SaveChanges();

                TempData["SuccessMessage"] = $"Registration successfully deleted!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Registration not deleted, please try again! {ex.Message}";
            }

            return Redirect("/Registration/Index");
        }
    }
}