using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using WebShop.Entity;
using WebShop.Models.Products;
using WebShop.Services.Interfaces;

namespace WebShop.Controllers
{
    public class ShopController : SurfaceController
    {
        private readonly ISolrService<SolrResponse> _searchService;

        public ShopController(ISolrService<SolrResponse> searchService)
        {
            _searchService = searchService;
        }

        // Gets called from view as Html.Action()
        public ActionResult GetProductsviewOnLoad()
        {
            if (Session["FilterQuery"] == null)
            {
                Session["Products"] = null;
                Dictionary<string, List<string>> filterquery = new Dictionary<string, List<string>>();
                filterquery.Add("Brand", new List<string>());
                filterquery.Add("Category", new List<string>());
                filterquery.Add("Tags", new List<string>());
                filterquery.Add("Search", new List<string>());
                filterquery.Add("Price", new List<string>());

                Session["FilterQuery"] = filterquery;
                Session["Products"] = null;
                Session["Facets"] = null;
                Session["TotalProducts"] = null;
                Session["SortQuery"] = null;
                Session["FacetQuery"] = null;
            }

            Session["TotalProducts"] = null;
            return PartialView("productsview");
        }

        public List<string> GetAndTransformPublicContentPriceFilter()
        {
            IPublishedContent priceFilter = Umbraco.ContentAtRoot().FirstOrDefault(x => x.ContentType.Alias == "home").FirstChildOfType("shop").FirstChildOfType("filtersFolder").Descendants().Where(x => x.ContentType.Alias == "filterType" && x.IsPublished() && x.Name == "Price").FirstOrDefault();

            List<string> myList = new List<string>();
            foreach(var child in priceFilter.Children)
            {
                myList.Add(child.Value("queryValue").ToString());
            }

            return myList;
        }


        // Gets called from JQuery In main.js
        // Gets called from Html.Action
        public ActionResult RenderFilterRow()
        {
            ProductInfoModel model = new ProductInfoModel();

            var query = CreateQuery("0");
            //"&q=*:*&facet.field=Brand&facet.field=Category&facet.field=Tags&start=0&facet=on&rows=0&wt=json"
            SolrResponse solrResponse = _searchService.GetProductsFromQuery(query);
            
            Session["Facets"] = solrResponse.FacetDictionary;
            Session["TotalProducts"] = solrResponse.TotalProducts;
            Session["FacetQuery"] = solrResponse.FacetQuery;

            model.FacetQuery = solrResponse.FacetQuery;
            model.Facets = solrResponse.FacetDictionary;
            model.TotalProducts = solrResponse.TotalProducts;
            model.Filters = Session["FilterQuery"] as Dictionary<string, List<string>>;
            model.Sort = Session["SortQuery"] as string;
            return PartialView("FilterRow", model);
        }

        // Gets called from JQuery In main.js
        public int CheckProductFetch()
        {
            List<productinfo> products = Session["Products"] as List<productinfo>;
            if(products == null)
            {
                return 0;
            }
            var start = products.Count;
            var totalAmount = (int)Session["TotalProducts"];
            if (start < totalAmount)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        // Gets called from JQuery
        public ActionResult RenderPagenationProducts()
        {
            ProductInfoModel model = new ProductInfoModel();
            List<productinfo> products = Session["Products"] as List<productinfo>;
            var start = products.Count;
            var query = getPaginationQuery(start);
            SolrResponse solrResponse = _searchService.GetProductsFromQuery(query);
            updateSessionParamsWithSolrValues(solrResponse, false);
            model.Products = solrResponse.Data;
            return PartialView("ProductList", model);
        }

        // Gets called from JQuery
        public ActionResult RenderProducts()
        {
            ProductInfoModel model = new ProductInfoModel();
            if (Session["Products"] != null)
            {
                List<productinfo> products = Session["Products"] as List<productinfo>;
                model.Products = products;
                return PartialView("ProductList", model);  
            }
            else
            {
                var queryStr = CreateQuery("0");
                SolrResponse solrResponse = _searchService.GetProductsFromQuery(queryStr);
                model.Products = solrResponse.Data;
                model.Facets = solrResponse.FacetDictionary;
                model.TotalProducts = solrResponse.TotalProducts;
                model.FacetQuery = solrResponse.FacetQuery;
                updateSessionParamsWithSolrValues(solrResponse, true);
                return PartialView("ProductList", model);
            }
        }

        // Gets called from JQuery In main.js
        public ActionResult RemoveFilterParam(string key, string value)
        {
            ProductInfoModel model = new ProductInfoModel();

            Dictionary<string, List<string>> FilterQuery = Session["FilterQuery"] as Dictionary<string, List<string>>;

            if (FilterQuery.ContainsKey(key))
            {
                FilterQuery[key].Remove(value);
            }
            model.Filters = FilterQuery;

            Session["Products"] = null;

            //Check if Filter query is empty
            var empty = false;
            foreach (KeyValuePair<string, List<string>> index in FilterQuery)
            {
                foreach(var param in index.Value)
                {
                    empty = true;
                }
            }

            if (!empty)
            {
                Session["Facets"] = null;
            }

            return PartialView("FilterRow", model);
        }

        // Gets called from JQuery In main.js
        public void RemoveAllFilterParam()
        {
            Dictionary<string, List<string>> query = new Dictionary<string, List<string>>();
            foreach (KeyValuePair<string, List<string>> index in Session["FilterQuery"] as Dictionary<string, List<string>>)
            {
                query[index.Key] = new List<string>();
            }
            Session["FilterQuery"] = query;
            Session["SortQuery"] = new Dictionary <string, string>();
            Session["Facets"] = null;
            Session["Products"] = null;
        }

        // Gets called from view as JQuery
        public ActionResult SearchPrice(string price)
        {
            var model = new ProductInfoModel();
            updateSessionFilterQuery("Price", price, false);
            var queryStr = CreateQuery("0");
            SolrResponse solrResponse = _searchService.GetProductsFromQuery(queryStr);
            model.Products = solrResponse.Data;
            model.Facets = solrResponse.FacetDictionary;
            model.FacetQuery = solrResponse.FacetQuery;
            updateSessionParamsWithSolrValues(solrResponse,true);
            return PartialView("ProductList", model); 
        }
        // Gets called from view as JQuery
        public ActionResult SearchTags(string tag)
        {
                var model = new ProductInfoModel();
                updateSessionFilterQuery("Tags", tag, false);
                var queryStr = CreateQuery("0");
                SolrResponse solrResponse = _searchService.GetProductsFromQuery(queryStr);
                model.Products = solrResponse.Data;
                model.Facets = solrResponse.FacetDictionary;
                model.FacetQuery = solrResponse.FacetQuery;
                updateSessionParamsWithSolrValues(solrResponse, true);
                return PartialView("ProductList", model);
        }
        // Gets called from view as JQuery  
        public ActionResult SearchBrand(string brand)
        {
            var model = new ProductInfoModel();
            updateSessionFilterQuery("Brand", brand, false);
            var queryStr = CreateQuery("0");
            SolrResponse solrResponse = _searchService.GetProductsFromQuery(queryStr);
            model.Products = solrResponse.Data;
            model.Facets = solrResponse.FacetDictionary;
            model.FacetQuery = solrResponse.FacetQuery;
            updateSessionParamsWithSolrValues(solrResponse, true);
            return PartialView("ProductList", model);

        }
        // Gets called from view as JQuery
        public ActionResult SearchCategory(string category)
        {
            var model = new ProductInfoModel();
            updateSessionFilterQuery("Category", category, false);
            var queryStr = CreateQuery("0");
            SolrResponse solrResponse = _searchService.GetProductsFromQuery(queryStr);
            model.Products = solrResponse.Data;
            model.Facets = solrResponse.FacetDictionary;
            model.FacetQuery = solrResponse.FacetQuery;
            updateSessionParamsWithSolrValues(solrResponse,true);
            return PartialView("ProductList", model);
        }

        public ActionResult SearchSort(string value)
        {
            var model = new ProductInfoModel();
            updateSessionSortQuery(value);
            var queryStr = CreateQuery("0");
            SolrResponse solrResponse = _searchService.GetProductsFromQuery(queryStr);
            model.Products = solrResponse.Data;
            model.Facets = solrResponse.FacetDictionary;
            model.FacetQuery = solrResponse.FacetQuery;
            updateSessionParamsWithSolrValues(solrResponse,true);
            return PartialView("ProductList", model);
        }

        public ActionResult Search(string search)
        {
            {
                var model = new ProductInfoModel();
                updateSessionFilterQuery("Search", search, false);
                var query = CreateQuery("0");
                SolrResponse solrResponse = _searchService.GetProductsFromQuery(query);
                updateSessionParamsWithSolrValues(solrResponse,true);
                model.Products = solrResponse.Data;
                model.Facets = solrResponse.FacetDictionary;
                model.FacetQuery = solrResponse.FacetQuery;
                return PartialView("ProductList", model);
            }
        }

        public void updateSessionFilterQuery(string key, string value, bool reset)
        {
            if (reset)
            {
                Dictionary<string, List<string>> query = Session["FilterQuery"] as Dictionary<string, List<string>>;
                query[key] = new List<string>();
                Session["FilterQuery"] = query;
            }
            else
            {
                Dictionary<string, List<string>> query = Session["FilterQuery"] as Dictionary<string, List<string>>;
                List<string> mylist = query[key];
                if (!mylist.Contains(value))
                {
                    mylist.Add(value);
                    query[key] = mylist;
                }
                else
                {
                    mylist.Remove(value);
                    query[key] = mylist;
                }
                Session["FilterQuery"] = query;
            }
        }

        public void updateSessionSortQuery(string value)
        {
          Session["SortQuery"] = value;
        }

        public void updateSessionParamsWithSolrValues(SolrResponse solr, bool newsearch)
        {
            Session["Facets"] = solr.FacetDictionary;
            Session["TotalProducts"] = solr.TotalProducts;
            Session["FacetQuery"] = solr.FacetQuery;
            List<productinfo> products = Session["Products"] as List<productinfo>;

            if (newsearch)
            {
                Session["Products"] = solr.Data;
            }
            else
            {
                if (products == null)
                {
                    products = solr.Data;
                }
                else
                {
                    products.AddRange(solr.Data);
                }
            }
        }


        public string CreateQuery(string start)
        {
            var url = "&q=";
            var urlQuery = "";
            var urlFilterQuery = "";
            var facetFields = "";
            var sort = "";
            var facetQuery = "";
            //All fields query params
            Dictionary<string, List<string>> filterQuery = Session["FilterQuery"] as Dictionary<string, List<string>>;
            string sortQuery = Session["SortQuery"] as string;

            //Filter query params
            foreach (KeyValuePair<string, List<string>> index in filterQuery)
            {
                if(index.Key == "Search")
                {
                    if (index.Value.Count > 0)
                    {
                        foreach (var param in index.Value)
                        {
                            if (urlQuery != "") urlQuery += " AND ";

                            urlQuery += "\"" + param + "\"";
                        }
                    }
                    else
                    {
                        urlQuery += "*:*";
                    }
                }
                else
                {

                    if (index.Value.Count > 0)
                    {
                        //if (urlFilterQuery != "") urlFilterQuery += " AND ";

                        if (index.Key == "Price")
                        {
                                urlFilterQuery += "&fq={!tag=" + index.Key + "}" + index.Key + ":(";
                                foreach (string param in index.Value)
                                {
                                    urlFilterQuery += param + " ";
                                }
                                urlFilterQuery += ")";
                        }
                        else
                        {
                            //urlFilterQuery += "{!tag=" + index.Key + "}" +
                            urlFilterQuery += "&fq={!tag=" + index.Key + "}" + index.Key + ":(";

                            foreach (string param in index.Value)
                            {
                                var newparam = param.Replace("&", "%26");

                                if (newparam.Contains(" "))
                                {
                                    urlFilterQuery += '"' + newparam + '"';
                                }
                                else
                                {
                                    urlFilterQuery += newparam + " ";
                                }
                            }
                            urlFilterQuery += ")";
                        }
                    }
                }
            }

            if(sortQuery != "")
            {
                sort = "&" + sortQuery;
            }
            Session["QueryStr"] = urlQuery;

            //Generate facet for each filter query
            foreach (KeyValuePair<string, List<string>> index in filterQuery)
            {
                if(index.Key != "Search" && index.Key != "Price")
                {
                    facetFields += "facet.field=" + "{!ex=" + index.Key + "}" + index.Key + "&";
                }
            }

            //Generate facet.query
            List<string> list = GetAndTransformPublicContentPriceFilter();
            foreach(var value in list)
            {
                //
                facetQuery += "&facet.query={!ex=Price}" + value;
            }

            if (urlFilterQuery != "")
            {
                url += urlQuery + urlFilterQuery + "&" + facetFields + "start=" + start + "&rows=20&facet.mincount=1" + sort + facetQuery;
                Session["PaginationQuery"] = urlQuery + urlFilterQuery + "&" + facetFields + sort + facetQuery;
            }
            else
            {
                url += urlQuery + "&" + facetFields + "start=" + start + "&facet=on&rows=20&wt=json&facet.mincount=1" + sort + facetQuery;
                Session["PaginationQuery"] = urlQuery + "&" + facetFields + facetQuery + sort + facetQuery;
            }
            
            Session["FullQueryStr"] = url;
            
            return url;
    }

        public string getPaginationQuery(int start)
        {
            var query = Session["PaginationQuery"];
            return  "&q=" + query + "&start=" + start + "&rows=20";
        }

        public ActionResult FeaturedProducts(IEnumerable<string> list)
        {
            ProductInfoModel model = new ProductInfoModel();
            var counter = 0;
            string queryString = "&q=id:(";
            foreach(var id in list)
            {
                counter++;
                queryString += id + " ";
            }
            queryString += ")&facet.field={!ex=Brand}Brand&facet.field={!ex=Category}Category&facet.field={!ex=Tags}Tags&start=0&facet=on&rows="+ counter;
            SolrResponse solrResponse = _searchService.GetProductsFromQuery(queryString);
            model.TotalProducts = solrResponse.TotalProducts;
            model.Products = solrResponse.Data;
            return PartialView("ProductList", model);
        }
    }
}