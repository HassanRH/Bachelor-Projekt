using System;
using System.Collections.Generic;
using WebShop.Entity;

namespace WebShop.Models.Products
{
    public class ProductInfoModel
    {
        public ProductInfoModel() 
        {
            Products = new List<productinfo>();
        }
        public int SelectedProductID { get; set; }

        public Boolean Start { get; set; }

        public int TotalProducts { get; set; }

        public Dictionary<string,List<string>> Query { get; set; }
        public Dictionary<string, List<string>> FilterQuery { get; set; }
        public Dictionary<string, List<string>> Facets { get; set; }
        public Dictionary<string, List<string>> Filters { get; set; }

        public Dictionary<string, int> FacetQuery { get; set; }
        public string Sort { get; set; }

        public List<productinfo> Products { get; set; }
        public void AddProduct(productinfo product)
        {
            this.Products.Add(product);
        } 

        public void AddFilterParamToQuery(string name, List<string> list)
        {
            this.Query.Add(name, list);
        }

        public void appendProductList(List<productinfo> list)
        {
            this.Products.AddRange(list);
        }
    }
}