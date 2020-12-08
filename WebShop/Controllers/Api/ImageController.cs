using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Umbraco.Web.WebApi;
using WebShop.Entity;
using WebShop.Services.Interfaces;

namespace WebShop.Controllers.Api
{
    public class ImageController : UmbracoApiController
    {
        private readonly IProductService<product, BinaryClass> _productService;

        public ImageController()
        {
        }

        public ImageController(IProductService<product, BinaryClass> ips)
        {
            _productService = ips;
        }
        [HttpGet]
        public HttpResponseMessage GetThumbnailFromBrand([FromUri]Int64 id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            BinaryClass image = _productService.GetThumbnailFromBrandId(id);
            if (image != null)
            {
                response.Content = new ByteArrayContent(image.Binary);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return response;
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSingleImageFromId([FromUri]int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            BinaryClass image = (BinaryClass)_productService.GetBinaryFileById(id);
            if (image != null)
            {
                response.Content = new ByteArrayContent(image.Binary);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(image.Contenttype);
                return response;
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            
        }
        [HttpGet]
        public HttpResponseMessage GetThumbnailFromProduct([FromUri]int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            byte[] image = _productService.GetThumbnailFromProductId(id);
            if(image != null)
            {
                response.Content = new ByteArrayContent(image);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return response;
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
        }

        public HttpResponseMessage GetThumbnailFromProductSKU([FromUri]string Sku)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            byte[] image = _productService.GetThumbnailFromProductSku(Sku);
            if(image != null)
            {
                response.Content = new ByteArrayContent(image);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return response;
            }
            else{
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

        }

    }
}