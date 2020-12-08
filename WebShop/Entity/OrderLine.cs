using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Entity
{
    public class OrderLine
    {
        public OrderLine()
        {
        }

        public OrderLine(string productSKU, int productID, string name, int amount, int price)
        {
            ProductSKU = productSKU;
            Name = name;
            Amount = amount;
            Price = price;
            ProductID = productID;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string ProductSKU { get; set; }

        public int ProductID { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }

    }
}