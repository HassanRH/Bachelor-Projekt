using System.Collections.Generic;
using WebShop.Entity;
using WebShop.Models;

namespace Umbraco.Web.PublishedModels
{
    public partial class CheckOut
    {
        private List<product> ProductsToCheckout;
        public CheckOutForm form = new CheckOutForm();

        public void setProducts(List<product> products)
        {
            this.ProductsToCheckout = products;
        }

        public List<product> getProducts()
        {
            return this.ProductsToCheckout;
        }

        public double getTotalPriceForCart()
        {
            double price = 0;
            foreach (product item in ProductsToCheckout)
            {
                price += item.getTotalPrice();
            }
            return price;
        }

        public double getTotalPrice()
        {
            double price = 0;
            foreach (product item in ProductsToCheckout)
            {
                price += item.getTotalPrice();
            }
            return price;
        }
    }
}