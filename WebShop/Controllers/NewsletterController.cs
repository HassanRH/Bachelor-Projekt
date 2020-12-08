using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebShop.Entity;
using WebShop.Models;
using WebShop.Services.Implementation;
using WebShop.Services.Interfaces;

namespace WebShop.Controllers
{
    public class NewsletterController : SurfaceController
    {

        public NewsletterController() {}
        

        public ActionResult subscribeForm(NewsletterFormModel model)
        {
            if (!ModelState.IsValid || model.Email == "" || model.Email == null)
                return CurrentUmbracoPage();

            var mail = model.Email;

            BackgroundJob.Enqueue<IHangfire>(x => x.AddToSubscriptions(mail));

            return RedirectToCurrentUmbracoPage();
        }

        public PartialViewResult getForm()
        {
            NewsletterFormModel model = new NewsletterFormModel();
            return PartialView("NewsletterForm", model);
        }
    }
}