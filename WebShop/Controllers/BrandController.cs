using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebShop.Entity;
using WebShop.Models.Brands;
using WebShop.Models.Products;
using WebShop.Services.Interfaces;

namespace WebShop.Controllers
{
    public class BrandController : SurfaceController
    {
        private readonly IDatabase _db;
        public BrandController()
        {

        }

        public BrandController(IDatabase db)
        {
            this._db = db;
        }

        public ActionResult GetBrands()
        {
            var model = new BrandModel
            {
                Brands = GetBrandsForModel()
            };
            return PartialView("brandBanner", model);
        }

        [HttpPost]
        public ActionResult GetProductsFromBrand(string brand)
        {
            Session["products"] = null;
            Session["FilterQuery"] = null;
            Dictionary<string, List<string>> filterquery = new Dictionary<string, List<string>>();

            filterquery.Add("Brand", new List<string>());
            filterquery.Add("Category", new List<string>());
            filterquery.Add("Tags", new List<string>());
            filterquery.Add("Search", new List<string>());
            filterquery.Add("Price", new List<string>());
            filterquery["Brand"].Add(brand);
            Session["FilterQuery"] = filterquery;
            return this.RedirectToUmbracoPage(1335);
        }
        public List<Brand> GetBrandsForModel()
        {
            List<Brand> list = new List<Brand>();
            using (SqlConnection conn = new SqlConnection())
            {
                var script = "SELECT Brand_id, Brand_name, Brand_logo FROM Brand";
                SqlCommand command = new SqlCommand(script, conn);
                conn.ConnectionString = _db.getConnectionString();
                conn.Open();
                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Brand brand = new Brand
                        {
                            ID = int.Parse(oReader["Brand_id"].ToString()),
                            Name = oReader["Brand_name"].ToString()
                        };
                        if (oReader["Brand_logo"].GetType() != typeof(DBNull))
                        {
                            brand.Image = (byte[])oReader["Brand_logo"];
                        }

                        list.Add(brand);
                    }
                }
                return list;
            }
        }
    }
}