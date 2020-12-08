using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebShop.Entity;
using WebShop.Models.Product;
using WebShop.Services.Interfaces;

namespace WebShop.Controllers
{
    public class ProductController : SurfaceController
    {
        private readonly IProductService<product, BinaryClass> _productService;

        public ProductController()
        {
        }

        public ProductController(IProductService<product, BinaryClass> productService)
        {
            _productService = productService;
        }

        // GET: Product
        public ActionResult GetSingleProduct(int id)
        {
            ProductModel model = new ProductModel
            {
                Product = _productService.GetSingleProduct(id)
            };

            return PartialView("SingleProductView", model);
        }
    }
}