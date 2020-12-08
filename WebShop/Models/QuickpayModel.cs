using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class QuickpayModel
    {
        public string Language { get; set; }
        public string Autocapture { get; set; }
        public string OrderId { get; set; }
        public int MerchantID { get; set; }
        public int AgreementId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string ContinueUrl { get; set; }
        public string CancelUrl { get; set; }
        public string CallbackUrl { get; set; }
        public string Cheksum { get; set; }
        public string Version { get; set; }

    }
}