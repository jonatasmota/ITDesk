using DBProgrammingClass_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DBProgrammingClass_3.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            List<Product> products = context.Products
                    .OrderByDescending(x =>
                        x.ProductCode)
                    .ToList();

            return View(products);
        }

        public ActionResult ProductDetails(string id)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            Product product = context.Products.FirstOrDefault(x => x.ProductCode == id);

            return PartialView(product);
        }

        public ActionResult Delete(string id)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                Product productToRemove = context.Products.FirstOrDefault(x => x.ProductCode == id);

                context.Products.Remove(productToRemove);
                context.SaveChanges();
                TempData["SuccessMessage"] = $"{productToRemove.Name} successfully deleted!";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Product not deleted, please try again! + {ex.Message}";
            }

            return Redirect("/Product/Index");
        }

        public ActionResult AddProduct()
        {
            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            Product product = new Product();

            return View("EditProduct", product);
        }

        [HttpGet]
        public ActionResult EditProduct(string id)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            Product product = context.Products.FirstOrDefault(x => x.ProductCode == id);

            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(Product product)
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            try
            {
                context.Products.AddOrUpdate(product);
                context.SaveChanges();
                TempData["SuccessMessage"] = "Product successfully added!";

                return Redirect("/Product/Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = $"Product not added, please try again! {ex.Message}";
                return Redirect("/Product/Index");
            }
        }
    }
}