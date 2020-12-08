using System.Collections.Generic;
using WebShop.Entity;

namespace WebShop.Models
{
    public class CheckOutForm
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

        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Apartsuite { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public List<product> products { get; set; }
        public string Shipping { get; set; }
        public string Country { get; set; }
        public string Message { get; set; }


        public CheckOutForm()
        {
            products = new List<product>();
        }

        public double getTotalPrice()
        {
            double totalAmount = 0;
            foreach (var product in products)
            {
                totalAmount += product.getTotalPrice();
            }
            return totalAmount;
        }
    }
}