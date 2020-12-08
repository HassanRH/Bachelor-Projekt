using Hangfire;
using Hangfire.Server;
using System;
using System.Net;
using System.Net.Mail;
using WebShop.Entity;
using WebShop.Icecat.Assets.Interfaces;
using WebShop.Models;
using WebShop.Services.Implementation;
using WebShop.Services.Interfaces;

namespace WebShop
{
    public class HangfireService : IHangfire
    {

        private readonly IFacade _ifc;
        private readonly ISolrService<SolrResponse> _solr;
        private readonly IOrderService<Order, CheckOutForm> _orderService;

        public HangfireService(IFacade ifc, ISolrService<SolrResponse> solr, IOrderService<Order, CheckOutForm> orderService)
        {
            _ifc = ifc;
            _solr = solr;
            _orderService = orderService;
        }

        public void ScheduleRecurringProductUpdate()
        {
            _ifc.updateProducts();
            _solr.deleteAllDocuments();
            _solr.Populatesolr();
            _ifc.initDatabaseWithProducts();
        }

        public void InitDatabase()
        {
            //_ifc.initDatabaseWithProducts();
            _solr.Populatesolr();
        }

        public void sendMailByGmail(ContactFormModel model)
        {
            var fromAddress = new MailAddress("hassanrh1996@gmail.com", "To MHShop");
            var toAddress = new MailAddress("hassanrh1996@gmail.com", "To MHShop");
            string fromPassword = "Hemmeligt2303";
            string subject = "Shop Request";
            string body = model.Name + "\n" + model.Email + "\n" + model.Message;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject + " / " + model.Email,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public void SendOrderConfirmation(int orderno)
        {
            _orderService.SendOrderConfirmationMailByGmail(orderno);
        }

        public void AddToSubscriptions(string email)
        {
            _orderService.AddSubscriber(email);
        }

        public void DoSomething()
        {
            throw new NotImplementedException();
        }

        public void DoIt(PerformContext context)
        {
            throw new NotImplementedException();
        }
    }
}