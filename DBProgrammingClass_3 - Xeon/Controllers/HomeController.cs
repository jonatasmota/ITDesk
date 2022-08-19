using DBProgrammingClass_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;


namespace DBProgrammingClass_3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new TechSupportEntities();

            if (Models.User.IsGuest()) return RedirectToAction("Login", "Login");

            ViewBag.Customers = context.Customers.OrderBy(x => x.Name).Count();
            ViewBag.Incidents = context.Incidents.OrderBy(x => x.IncidentID).Count();
            ViewBag.Products = context.Products.OrderBy(x => x.ProductCode).Count();
            ViewBag.Users = context.Users.OrderBy(x => x.UserId).Count();
            ViewBag.Technicians = context.Technicians.OrderBy(x => x.TechID).Count();
            ViewBag.Registrations = context.Registrations.OrderBy(x => x.RegistrationDate).Count();

            ViewBag.ClosedIncidents = context.Incidents.Where(x => x.DateClosed != null).Count();
            ViewBag.OpenIncidents = context.Incidents.Where(x => x.DateClosed == null).Count();
            ViewBag.IncidentsMonth = context.Incidents.Where(x => x.DateOpened.Month == DateTime.Now.Month).Count();
            ViewBag.RegistrationsMonth = context.Registrations.Where(x => x.RegistrationDate.Month == DateTime.Now.Month).Count();
            ViewBag.DateTime = DateTime.Now.ToString();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("sigilous@gmail.com", "Jonatas");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "owst2322";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
    }
}