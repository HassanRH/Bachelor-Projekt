using Hangfire;
using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebShop.Models;
using WebShop.Services.Interfaces;

namespace WebShop.Controllers
{
    public class ContactController : SurfaceController
    {

        public ContactController() { }

        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        //Gets called from form in ContactForm.cshtml
        public ActionResult submitForm(string name, string email, string message)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            ContactFormModel model = new ContactFormModel { Name = name, Email = email, Message = message };

            BackgroundJob.Enqueue<IHangfire>( x => x.sendMailByGmail(model) );

            return RedirectToCurrentUmbracoPage();
        }


        public PartialViewResult getForm()
        {
            ContactFormModel model = new ContactFormModel();
            return PartialView("ContactForm", model);
        }
    }
}