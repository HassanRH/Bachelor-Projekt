using System;
using System.Collections.Generic;

namespace WebShop.Services.Interfaces
{
    public interface IProductService<T,R>
    {
        T GetSingleProduct(Int64 id);

        List<T> GetAllProducts();

        T GetSingleProductInfo(Int64 id);

        object GetBinaryFileById(Int64 id);
        byte[] GetThumbnailFromProductId(Int64 id);
        byte[] GetThumbnailFromProductSku(string sku);
        R GetThumbnailFromBrandId(Int64 id);
    }
}
