using System.Collections.Generic;
using WebShop.Entity;

namespace WebShop.Models.Cart
{
    public class ShoppingCartModel
    {

        public ShoppingCartModel()
        {
        }

        public List<product> ProductsInCart { get; set; }
        public void AddProduct(product product)
        {
            this.ProductsInCart.Add(product);
        }
        public void ExtendProductList(List<product> newproducts)
        {
            foreach (product product in newproducts)
            {
                this.ProductsInCart.Add(product);
            }
        }

        public double getCartTotal()
        {
            double price = 0;
            foreach(var item in ProductsInCart)
            {
                price +=  item.Amount * item.Price;
            }
            return price;
        }

    }
}