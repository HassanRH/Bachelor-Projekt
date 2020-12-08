using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using WebShop.Entity;

namespace WebShop.Models.Brands
{
    public class BrandModel 
    {
        public BrandModel()
        {
        }

        public List<Brand> Brands { get; set; }
        public void addBrand(Brand brand)
        {
            this.Brands.Add(brand);
        }

    }
}