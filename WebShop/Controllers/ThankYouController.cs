using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebShop.Entity;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class ThankYouController : SurfaceController
    {
        public ThankYouController() { }

        public ActionResult GetThanks()
        {
            Order order = Session["Order"] as Order;
            return PartialView("ThankYouView", order);
        }
    }
}