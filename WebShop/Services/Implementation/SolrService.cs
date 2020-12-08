using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using WebShop.Entity;
using WebShop.Services.Interfaces;

namespace WebShop.Services.Implementation
{

    public class SolrService : ISolrService<SolrResponse>
    {
        private readonly string _Url = "http://localhost:8983/solr/shoproducts";
        private readonly string _searchUrl = "http://localhost:8983/solr/shoproducts/select?fl=id,Title,Price";
        private readonly IDatabase _db;

        public SolrService(IDatabase db)
        {
            this._db = db;
        }

        public HttpStatusCode deleteAllDocuments()
        {
            var deleteParam = "/update?commit=true&stream.body=<delete><query>*:*</query></delete>";
            string uri = _Url + deleteParam;
            using (var client = new HttpClient())
            {
                var httpContent = new StringContent("", Encoding.UTF8, "text/xml");
                var res = client.PostAsync(new Uri(uri), httpContent).Result;
                return res.StatusCode;
            }
        }

        public HttpStatusCode Populatesolr()
        {
            List<int> productids = new List<int>();
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();
                var script = "SELECT Products.ID FROM Products";
                SqlCommand command = new SqlCommand(script, conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productids.Add(int.Parse(reader["ID"].ToString()));
                    }
                }
                List<SolrModel> Solrproduct = new List<SolrModel>();

                foreach (int ID in productids)
                {
                    Solrproduct.Add(GetSingleProduct(ID));
                }
                Dictionary<string, List<SolrModel>> spm = new Dictionary<string, List<SolrModel>>();
                spm.Add("add", Solrproduct);
                var json = JsonConvert.SerializeObject(spm, Formatting.Indented);

                using (var client = new HttpClient())
                {
                    try
                    {
                        string uri = _Url + "/update/json?wt=json&commit=true";
                        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        var res = client.PostAsync(new Uri(uri), httpContent).Result;
                        return res.StatusCode;
                    }catch(Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public SolrModel GetSingleProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();
                var selectProductScript = "SELECT ID, Title, EANCode, Price, Description, Short_summary_description, Long_summary_description, Thumb_pic FROM Products WHERE Products.ID = @id";
                var selectProductPicturesScript = "SELECT ID FROM Products_Gallery WHERE Product_id = @id";
                var selectProductBulletpointsScript = "SELECT bulletpoint FROM BulletPoints_Products INNER JOIN BulletPoint ON BulletPoints_Products.BulletPointID = BulletPoint.ID WHERE BulletPoints_Products.ProductID = @id";
                var selectProductTagsScript = "SELECT tag FROM Products_Tags INNER JOIN Tags ON Products_Tags.TagID = Tags.ID WHERE Products_Tags.ProductID = @id";
                var selectProductCategoryAndBrandScript = "SELECT Category.Category_name, Brand.Brand_name FROM Category INNER JOIN Products ON Category.Category_id = Products.Category_id " +
                                                "INNER JOIN Brand ON Products.Brand_id = Brand.Brand_id " +
                                                "WHERE Products.ID = @id";
                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Connection = conn;
                command.Transaction = transaction;
                command.Parameters.AddWithValue("@id", id);

                SolrModel product = new SolrModel();
                try
                {
                    command.CommandText = selectProductScript;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            product.id = int.Parse(oReader["ID"].ToString());
                            product.Title = oReader["Title"].ToString();
                            product.Price = Math.Round(double.Parse(oReader["Price"].ToString()), 2, MidpointRounding.AwayFromZero);
                            product.Ean = oReader["EANCode"].ToString();
                            product.Description = oReader["Description"].ToString();
                            product.Shortsummary = oReader["Short_summary_description"].ToString();
                            product.Longsummary = oReader["Long_summary_description"].ToString();
                        }
                    }
                    command.Connection = conn;
                    command.Transaction = transaction;
                    command.CommandText = selectProductCategoryAndBrandScript;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            product.Category = oReader["Category_name"].ToString();
                            product.Brand = oReader["Brand_name"].ToString();

                        }
                    }
                    command.Connection = conn;
                    command.Transaction = transaction;
                    command.CommandText = selectProductPicturesScript;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            if (product.Pictures.Count < 5)
                            {
                                product.addPicture(int.Parse(oReader["ID"].ToString()));
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    command.Connection = conn;
                    command.Transaction = transaction;
                    command.CommandText = selectProductBulletpointsScript;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            product.addBulletPoint(oReader["bulletpoint"].ToString());
                        }
                    }
                    command.Connection = conn;
                    command.Transaction = transaction;
                    command.CommandText = selectProductTagsScript;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            product.addTag(oReader["tag"].ToString());
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    try
                    {
                        product = null;
                        transaction.Rollback();
                        throw e;
                    }
                    catch (SqlException ex)
                    {
                        if (transaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }
                }
                return product;
            }
        }
        public SolrResponse GetProductsFromQuery(string query)
        {
            return SearchByQuery(query);
        }

        public SolrResponse SearchByQuery(string query)
        {
            var Productlists = new List<productinfo>();
            using (var client = new HttpClient())
            {
                string queryStr = _searchUrl + query;
                //var finalUrl = Uri.EscapeUriString(queryStr);
                HttpResponseMessage response = client.GetAsync(queryStr).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseString);
                JObject data = JsonConvert.DeserializeObject<JObject>(responseString);
                var products = data.Values().Select(x => x.ToObject<productinfo>()).ToList();
                var list = (JArray)data["response"]["docs"];
                Dictionary<string, List<string>> facetDictionary = data["facet_counts"]["facet_fields"].ToObject<Dictionary<string, List<string>>>();

                Dictionary<string, int> FacetQueriesDictionaryBroken = data["facet_counts"]["facet_queries"].ToObject<Dictionary<string, int>>();
                Dictionary<string, int> FacetQueriesDictionaryFixed = new Dictionary<string, int>();

                foreach (KeyValuePair<string,int> index in FacetQueriesDictionaryBroken)
                {
                    var ses = index.Key;
                    var res = ses.Split('}')[1];
                    FacetQueriesDictionaryFixed[res] = index.Value;
                }


                //Dictionary<string, string> facetDictionary = new Dictionary<string, List<string>();
                var NumberTotal = (int)data["response"]["numFound"];
                
                foreach (var item in list)
                {
                    productinfo product = new productinfo();
                    product.Id = item.Value<int>("id");
                    product.Title = item.Value<string>("Title");
                    product.Price = item.Value<double>("Price");
                    Productlists.Add(product);
                }
                SolrResponse solrR = new SolrResponse(NumberTotal, Productlists, facetDictionary, FacetQueriesDictionaryFixed);
                return solrR;
            }
        }
    }
}