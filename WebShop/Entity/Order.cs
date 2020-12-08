using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Entity
{
    public class Order
    {
        public Order()
        {
            OrderLines = new List<OrderLine>();
        }

        public Order(int orderNo, Customer customer, DateTime createdate, string pdfpath)
        {
            OrderNo = orderNo;
            Customer = customer;
            Createdate = createdate;
            OrderLines = new List<OrderLine>();
            PDF = pdfpath;
        }

        public Order(Customer customer, string shipping, List<OrderLine> orderLines)
        {
            Customer = customer;
            Shipping = shipping;
            OrderLines = orderLines;
        }

        //Properties

        public int OrderNo { get; set; }
        public Customer Customer { get; set; }

        public string Shipping { get; set; }
        public string PDF { get; set; }

        public DateTime Createdate { get; set; }

        public List<OrderLine> OrderLines { get; set; }

        public void addOrderLine(OrderLine line)
        {
            this.OrderLines.Add(line);
        }

        public double getTotalPrice()
        {
            double price = 0;
            foreach (var line in OrderLines)
            {
                price += line.Amount * line.Price;
            }
            return price;
        }


    }

}