using System.Collections;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace IceCat.Assets.Interfaces
{
    public interface Iicecat
    {
        Task UpdateProductsFromDaily();
        Task PopulateDB();

        void ValidationCallBack(object sender, ValidationEventArgs e);

        void BulkUpdateProducts(ArrayList products);

        void BulkInsert(ArrayList brands, ArrayList categories, ArrayList products);

        ArrayList CreateTagsFromXmlList(ArrayList xmlList);
        ArrayList CreateCategoriesFromXmlList(ArrayList xmlList);
        ArrayList CreateBrandsFromXmlList(ArrayList xmlList);

        ArrayList CreateProductsFromXmlList(ArrayList xmlList);

        Task<XmlDocument> FetchSingleProductXmlFromIcecat(int id);

        Task<ArrayList> FetchAllProductsIceCatByIDList(ArrayList list);
    }
}
