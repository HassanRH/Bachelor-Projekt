using IceCat.Assets.Interfaces;


using System;
using WebShop.Entity;
using WebShop.Icecat.Assets.Interfaces;
using WebShop.Services.Interfaces;

namespace IceCat.Facade {
    public class IcecatFacade : IFacade
    {
        private readonly Iicecat _icecat;
        private readonly IDataAccessLayer _dal;
        private readonly ISolrService<SolrResponse> _solrService;

        public IcecatFacade(Iicecat icecat, IDataAccessLayer dal, ISolrService<SolrResponse> solrService)
        {
            this._icecat = icecat;
            this._dal = dal;
            this._solrService = solrService;
        }

        public void initDatabaseWithProducts()
        {
            Console.WriteLine("Starting...");
            //UNCOMMENT THIS FOR RECREATING DATABASE AND REPOPULATE WITH DATA 
            try
            {
                _dal.teardownDatabase();
                _dal.initDatabase();
                _icecat.PopulateDB();
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public void updateProducts()
        {
            Console.WriteLine("Updating Products...");
            _icecat.UpdateProductsFromDaily();
            Console.WriteLine("Removing products from Solr");
            _solrService.deleteAllDocuments();
            Console.WriteLine("Populating Solr with products");
            _solrService.Populatesolr();
        }
    }
}
