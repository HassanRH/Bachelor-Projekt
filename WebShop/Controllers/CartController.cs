using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebShop.Entity;
using WebShop.Models.Cart;
using WebShop.Services.Interfaces;

namespace WebShop.Controllers
{
    public class CartController : SurfaceController
    {
        private readonly IProductService<product, BinaryClass> _productService;
        private ShoppingCartModel shoppingCart;

        public CartController()
        {
        }

        public CartController(IProductService<product, BinaryClass> productService)
        {
            _productService = productService;
            shoppingCart = new ShoppingCartModel();
        }

        public PartialViewResult AdjustProductAmount(int id, int amount)
        {
            var products = Session["productsInCart"] as List<product>;
            foreach (var product in products)
            {
                if (product.Id == id)
                {
                    product.Amount = amount;
                }
            }
            shoppingCart.ProductsInCart = products;
            Session["productsInCart"] = products;
            return PartialView("shopCart", shoppingCart);
        }

        public PartialViewResult GetFullShoppingCart()
        {
            var products = Session["productsInCart"] as List<product>;
            shoppingCart.ProductsInCart = products;
            Session["productsInCart"] = products;
            return PartialView("shopCart", shoppingCart);
        }
        public ActionResult GetShoppingCartPage()
        {
            return View("ShoppingCartPage");
        }

        public ActionResult GetCheckoutPage()
        {
            return View("../checkout");
        }

        public PartialViewResult GetSideShoppingCart()
        {
            if(Session["productsInCart"] != null){
                var products = Session["productsInCart"] as List<product>;
                shoppingCart.ProductsInCart = products;
                return PartialView("ShoppingCartSide", shoppingCart);
            }
            else
            {
                shoppingCart.ProductsInCart = new List<product>();
                return PartialView("ShoppingCartSide", shoppingCart);
            }
        }

        public PartialViewResult AddProductToCart(int id, int amount)
        {
            if (Session["productsInCart"] != null)
            {
                List<product> list = Session["productsInCart"] as List<product>;
                foreach(var product in list)
                {
                    if(product.Id == id)
                    {
                        product.Amount += amount;
                        shoppingCart.ProductsInCart = list;
                        return PartialView("ShoppingCartSide", shoppingCart);
                    }
                }
                product Product = _productService.GetSingleProduct(id);
                Product.Amount = amount;
                list.Add(Product);
                shoppingCart.ProductsInCart = list;
                Session["productsInCartCount"] = list.Count;
                return PartialView("ShoppingCartSide", shoppingCart);
            }
            else
            {
                List<product> list = new List<product>();
                product Product = _productService.GetSingleProduct(id);
                Product.Amount = amount;
                list.Add(Product);
                shoppingCart.ProductsInCart = list;
                Session["productsInCart"] = list;
                Session["productsInCartCount"] = list.Count;
                return PartialView("ShoppingCartSide", shoppingCart);
            }
        }

        public PartialViewResult RemoveProductFromCart(int id)
        {
                List<product> list = Session["productsInCart"] as List<product>;
                List<product> newList = new List<product>();
                foreach(var item in list)
                {
                    if(item.Id == id)
                    {
                        continue;
                    }
                    else
                    {
                        newList.Add(item);
                    }
                }
                Session["productsInCart"] = newList;
                Session["productsInCartCount"] = newList.Count;
                shoppingCart.ProductsInCart = newList;
                return PartialView("ShoppingCartSide", shoppingCart);
        }

        public PartialViewResult RemoveProductFromCartPage(int id)
        {
            List<product> list = Session["productsInCart"] as List<product>;
            List<product> newList = new List<product>();
            foreach (var item in list)
            {
                if (item.Id == id)
                {
                    continue;
                }
                else
                {
                    newList.Add(item);
                }
            }
            Session["productsInCart"] = newList;
            Session["productsInCartCount"] = newList.Count;
            shoppingCart.ProductsInCart = newList;
            return PartialView("shopCart", shoppingCart);
        }

        public PartialViewResult UpdateCartLogo()
        {
            List<product> list = Session["productsInCart"] as List<product>;
            if (list == null)
            {
                shoppingCart.ProductsInCart = new List<product>();
            }
            else
            {
                shoppingCart.ProductsInCart = list;
            }
            Session["productsInCartCount"] = shoppingCart.ProductsInCart.Count;
            Session["productsInCart"] = shoppingCart.ProductsInCart;

            return PartialView("Navigation/CartLogoView", shoppingCart);
        }
    }
}