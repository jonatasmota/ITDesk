using DBProgrammingClass_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace DBProgrammingClass_3.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        [HttpGet]
        public ActionResult Index()
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            List<Customer> customer = context.Customers.OrderByDescending(x => x.CustomerID)
                .Where(x => x.Inactive == 0)
                .ToList();

            ViewBag.States = context.States.ToList();

            return View(customer);
        }

        public ActionResult Delete(int id)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            Customer customer = context.Customers.Find(id);

            if (customer != null)
            {
                try
                {
                    customer.Inactive = 1;
                    context.Customers.AddOrUpdate(customer);
                    context.SaveChanges();
                    TempData["SuccessMessage"] = $"{customer.Name} successfully deleted!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"{customer.Name} not deleted, please try again! + {ex.Message}";
                }
            }

            return Redirect("/Customer/Index");
        }

        public ActionResult AddCustomer()
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            Customer customer = new Customer();

            ViewBag.States = context.States.ToList();

            return View("EditCustomer", customer);
        }

        [HttpGet]
        public ActionResult EditCustomer(int id)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            Customer customer = context.Customers.FirstOrDefault(c => c.CustomerID == id);

            ViewBag.States = context.States.ToList();

            return View(customer);
        }

        [HttpPost]
        public ActionResult EditCustomer(Customer customer)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                context.Customers.AddOrUpdate(customer);
                context.SaveChanges();
                TempData["SuccessMessage"] = $"{customer.Name} successfully added!";

                return Redirect("/Customer/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"{customer.Name} not added, please try again! {ex.Message}";
                return Redirect("/Customer/Index");
            }
        }
    }
}