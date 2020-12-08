using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Services.Interfaces
{
    public interface IHangfire
    {
        void ScheduleRecurringProductUpdate();

        void DoSomething();

        void DoIt(PerformContext context);

        void sendMailByGmail(ContactFormModel model);

        void AddToSubscriptions(string email);

        void SendOrderConfirmation(int orderno);

    }
}
