using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Entity
{
    public class SolrResponse
    {
        public SolrResponse()
        {

        }

        public SolrResponse(int totalproducts, List<productinfo> data, Dictionary<string, List<string>> facetdictionary, Dictionary<string,int> facetQuery)
        {
            TotalProducts = totalproducts;
            this.Data = data;
            this.FacetDictionary = facetdictionary;
            this.FacetQuery = facetQuery;
        }

        public int TotalProducts { get; set; }

        public Dictionary<string,int> FacetQuery { get; set; }
        public List<productinfo> Data { get; set; }
        public Dictionary<string, List<string>> FacetDictionary { get; set; }
        
    }
}