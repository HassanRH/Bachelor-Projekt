using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebShop.Entity
{

    public class productinfo
    {
        public productinfo()
        {

        }

        public productinfo(int id, string brand, string title, double price)
        {
            Id = id;
            Brand = brand;
            Title = title;
            Price = price;
        }

        public int Id { get; set; }

        public string Brand { get; set; }

        public string Title { get; set; }

        public byte[] Image { get; set; }

        public double Price { get; set; }

    }
}