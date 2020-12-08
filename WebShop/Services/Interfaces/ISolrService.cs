using System.Net;

namespace WebShop.Services.Interfaces
{
    public interface ISolrService<T>
    {
        HttpStatusCode Populatesolr();
        HttpStatusCode deleteAllDocuments();
        T GetProductsFromQuery(string url);
        T SearchByQuery(string url);
    }
}
