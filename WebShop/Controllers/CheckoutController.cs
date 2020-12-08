using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebShop.Entity;
using WebShop.Models;
using System.Security.Cryptography;
using WebShop.Services.Interfaces;
using System.Text;


namespace WebShop.Controllers
{
    public class CheckoutController : SurfaceController
    {
        IOrderService<Order, CheckOutForm> _orderService;
        public CheckoutController(IOrderService<Order, CheckOutForm> orderService)
        {
            _orderService = orderService;
        }

        public CheckoutController()
        {
        }

        public PartialViewResult GetCheckoutForm()
        {
            //Is null on load
            CheckOutForm form;

            if (Session["CheckoutForm"] == null)
            {
                form = new CheckOutForm();
            }
            else
            {
                form = Session["CheckoutForm"] as CheckOutForm;
            }
            var products = Session["productsInCart"] as List<product>;
            form.products = products;
            return PartialView("CheckoutForm", form);
        }

        private string Sign(Dictionary<string, string> parameters, string api_key)
        {
            var result = String.Join(" ", parameters.OrderBy(c => c.Key).Select(c => c.Value).ToArray());
            var e = Encoding.UTF8;
            var hmac = new HMACSHA256(e.GetBytes(api_key));
            byte[] b = hmac.ComputeHash(e.GetBytes(result));
            var s = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                s.Append(b[i].ToString("x2"));
            }
            return s.ToString();
        }

        [HttpPost]
        [ActionName("submitForm")]
        public ActionResult submitForm(CheckOutForm model)
        {
            Session["CheckoutForm"] = model;
            if (model.Email == null || model.First_Name == null || model.Last_Name == null || model.Address == null || model.City == null || model.Phone == null || model.ZipCode == null || model.Country == null || model.Shipping == null)
            {
                model.Message = "Please remeber to fulfill all required fields";
                return CurrentUmbracoPage();
            }

            model.products = Session["productsInCart"] as List<product>;
            if (model.getTotalPrice() < 1)
            {
                return CurrentUmbracoPage();
            }

            var orderNo = _orderService.insertOrder(_orderService.ConvertModelToOrderTypeT(model));

            if(orderNo > 0)
            {
                var order = _orderService.GetOrder(orderNo);

                Session["productsInCart"] = null;
                Session["CheckoutForm"] = null;

                var amount = (int)Math.Round( model.getTotalPrice()*100);

                var parameter = new Dictionary<string, string>();

                const int merchantId = 124024;
                const int agreemendId = 458932;

                const string apiKey = "7634f30ee8aa77c3df1bb706afb66c84e77ed7391ca5d25f2f4f8de58275ab38";

                const string callbackUrl = "http://mh.alpha-solutions.dk/callback";
                const string cancelUrl = "http://mh.alpha-solutions.dk/cancel";
                const string continueUrl = "http://mh.alpha-solutions.dk//continue";
                const string language = "da";
                const string currency = "DKK";
                const string version = "v10";

                var @params = new Dictionary<string, string>();

                @params.Add("merchant_id", merchantId.ToString());
                @params.Add("agreement_id", agreemendId.ToString());
                @params.Add("order_id", String.Format("{0:0000}", orderNo));
                @params.Add("amount", amount.ToString());
                @params.Add("currency", currency);
                @params.Add("continueurl", continueUrl);
                @params.Add("cancelurl", cancelUrl);
                @params.Add("callbackurl", callbackUrl);
                @params.Add("language", language);
                @params.Add("autocapture", "1");
                @params.Add("version", version);

                model.AgreementId = agreemendId;
                model.Amount = amount;
                model.OrderId = String.Format("{0:0000}", orderNo);
                model.Autocapture = "1";
                model.CallbackUrl = callbackUrl;
                model.CancelUrl = cancelUrl;
                model.Cheksum = Sign(@params, apiKey);
                model.ContinueUrl = continueUrl;
                model.Currency = currency;
                model.Language = language;
                model.MerchantID = merchantId;
                model.Version = version;

                return PartialView("Payment", model);              
            }
            else
            {
                return CurrentUmbracoPage();
            }     
        }
    }
}