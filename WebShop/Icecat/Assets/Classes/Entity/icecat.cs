using System;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using System.Collections;
using IceCat.Assets.Classes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IceCat.Assets.Interfaces;

namespace IceCat.Entity {
    public class icecat : Iicecat{

        private Assets.Interfaces.ICredentials _icecat_cred;
        private IDataAccessLayer _dal;
        static readonly HttpClient httpclient = new HttpClient();

        public icecat(IceCat.Assets.Interfaces.ICredentials icecatcred, IDataAccessLayer data_access_layer) {
            _icecat_cred = icecatcred;
            _dal = data_access_layer;
        }

        public async Task PopulateDB() {
            ArrayList productIds = ReadProductsFromXMLIndexAndReturnIDList();
            ArrayList xmldocuments = await FetchAllProductsIceCatByIDList(productIds);
            ArrayList brands = CreateBrandsFromXmlList(xmldocuments);
            ArrayList categories = CreateCategoriesFromXmlList(xmldocuments);
            ArrayList products = CreateProductsFromXmlList(xmldocuments);
            ArrayList updatedBrands = ReadSupplierDataAndUpdateLogo(brands);
            Console.Write("\nBrands: " + updatedBrands.Count + "\tCategories: " + categories.Count + "\tProducts : " + products.Count +  "\t");
            BulkInsert(updatedBrands, categories, products);
        }

        public async Task UpdateProductsFromDaily() {
            ArrayList list = await ReadDailyXml();
            ArrayList products = CreateProductsFromXmlList(list);
            if (products.Count > 0) {
                BulkUpdateProducts(products);
            } else {
                Console.WriteLine("No products to update today! :)");
            }
        }

        public ArrayList ReadProductsFromXMLIndexAndReturnIDList() {
            try {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Ignore;
                //settings.ValidationType = ValidationType.Schema;
                //settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.XmlResolver = new XmlUrlResolver();
                XmlReader xmlReader = XmlReader.Create("..\\..\\..\\assets\\files\\files.index.xml", settings);

                ArrayList prodids = new ArrayList();
                int counter = 0;
                while (xmlReader.Read()) {
                    if (xmlReader.NodeType == XmlNodeType.Element) {
                        if (xmlReader.Name == "file" && xmlReader.GetAttribute("On_Market") == "1") {
                            var date = xmlReader.GetAttribute("Date_Added");
                            string supplierID = xmlReader.GetAttribute("Supplier_id");
                            //if (date.StartsWith("2020") || date.StartsWith("2019") || date.StartsWith("2018")) {
                                if (supplierID == "23499") {
                                    int prod_id = int.Parse(xmlReader.GetAttribute("Product_ID"));
                                    prodids.Add(prod_id);
                                    counter++;
                                } else if (supplierID == "4612") {
                                    string catidStr = xmlReader.GetAttribute("Catid");
                                    if (catidStr == "3840" || catidStr == "2130" || catidStr == "2133") {
                                        int prod_id = int.Parse(xmlReader.GetAttribute("Product_ID"));
                                        prodids.Add(prod_id);
                                        counter++;
                                    }
                                } else if (supplierID == "12984") {
                                    int prod_id = int.Parse(xmlReader.GetAttribute("Product_ID"));
                                    prodids.Add(prod_id);
                                    counter++;
                                }
                            //}
                        }
                    }
                }
                Console.WriteLine("Fetched product ids from indexfile: " + prodids.Count);
                return prodids;
            } catch (Exception e) {
                Console.WriteLine(e);
                Console.WriteLine("\n\nError occured while reading from index and trying to populate db");
                throw e;
            }
        }

        public byte[] GetBinaryFromURL(string url) {
            using (var webClient = new WebClient()) {
                byte[] imageBytes = webClient.DownloadData(url);
                return imageBytes;
            }
        }

        public async Task<ArrayList> ReadDailyXml() {
            try {
                Console.WriteLine("Starting to look for updates on products.");
                var request = WebRequest.Create("https://data.icecat.biz/export/freexml/daily.index.xml");
                request.Credentials = new NetworkCredential(_icecat_cred.getUsername(), _icecat_cred.getPassword());

                ArrayList productIDs = _dal.getProductIds();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Ignore;
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.XmlResolver = new XmlUrlResolver();

                ArrayList xmldocuments = new ArrayList();
                int counter = 1;
                using (WebResponse response = request.GetResponse()) {
                    using (XmlReader xmlReader = XmlReader.Create(response.GetResponseStream(), settings)) {
                        while (xmlReader.Read()) {
                            if (xmlReader.NodeType == XmlNodeType.Element) {
                                string name = xmlReader.Name;
                                if (name == "file") {
                                    int id = int.Parse(xmlReader.GetAttribute("Product_ID"));
                                    if (productIDs.Contains(id)) {
                                        Console.Write("\r{0}   ", "Found " + counter + " products to update");
                                        counter++;
                                        string quality = xmlReader.GetAttribute("Quality");
                                        if (quality == "REMOVED") {
                                            _dal.deleteProduct(id);
                                        } else if (quality == "SUPPLIER" || quality == "ICECAT") {
                                            XmlDocument xmldoc = await FetchSingleProductXmlFromIcecat(id);
                                            xmldocuments.Add(xmldoc);
                                        } else {
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return xmldocuments;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw e;
            }
        }

        public ArrayList ReadSupplierDataAndUpdateLogo(ArrayList brands) {
            Console.WriteLine("\nFetching logo's for suppliers..");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.XmlResolver = new XmlUrlResolver();
            XmlReader xmlReader = XmlReader.Create("..\\..\\..\\assets\\files\\SuppliersList.xml", settings);
            int counter = 1;
            while (xmlReader.Read()) {
                if (xmlReader.NodeType == XmlNodeType.Element) {
                    string name = xmlReader.Name;
                    if (name == "Supplier") {
                        int id = int.Parse(xmlReader.GetAttribute("ID"));
                        foreach (Brand brand in brands) {
                            if (brand.getID() == id) {
                                if(xmlReader.GetAttribute("LogoPic") != "") {
                                    byte[] logo = GetBinaryFromURL(xmlReader.GetAttribute("LogoPic"));
                                    brand.setLogo(logo);
                                    Console.Write("\r{0}   ", "Fetched logo's " + counter + "/" + brands.Count);
                                    counter++;
                                }
                            }
                        }
                    }
                }
            }
            return brands;
        }

        public async Task<ArrayList> FetchAllProductsIceCatByIDList(ArrayList list) {
            Console.WriteLine("Starting to fetch from icecat api");
            int count = 1;
            ArrayList productsXmlList = new ArrayList();
            foreach (int id in list) {
                try {
                    XmlDocument xmldoc = await FetchSingleProductXmlFromIcecat(id);
                    productsXmlList.Add(xmldoc);
                    Console.Write("\r{0}    ", "Collected " + count + "/" + list.Count + " products xmlschema from icecat");
                    count++;
                } catch (Exception e) {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
            return productsXmlList;
        }

        public async Task<XmlDocument> FetchSingleProductXmlFromIcecat(int id) {
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{_icecat_cred.getUsername()}:{_icecat_cred.getPassword()}")));
            var response = await httpclient.GetStringAsync("https://data.Icecat.biz/export/freexml/EN/" + id + ".xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);
            return xmlDoc;
        }

        public ArrayList CreateProductsFromXmlList(ArrayList xmlList) {
            try {
                ArrayList productsList = new ArrayList();

                if (xmlList.Count > 0) {
                    Console.WriteLine("\nStarting to create Product Entities from XML data");
                }

                foreach (XmlDocument document in xmlList) {

                    Product product = new Product();

                    XmlNodeList productXml = document.GetElementsByTagName("Product");
                    for (int i = 0; i < 1; i++) {
                        product.setID(int.Parse(productXml[i].Attributes["ID"].Value));
                        product.setSKU(productXml[i].Attributes["Prod_id"].Value);
                        product.setTitle(productXml[i].Attributes["Title"].Value);
                        if(productXml[i].Attributes["Pic500x500"].Value != "") {
                            product.setThumbpic(GetBinaryFromURL(productXml[i].Attributes["Pic500x500"].Value));
                        }else
                        {
                            product.setThumbpic(GetBinaryFromURL(productXml[i].Attributes["LowPic"].Value));
                        }
                    }

                    XmlNodeList descriptionXml = document.GetElementsByTagName("ProductDescription");
                    for (int i = 0; i < 1; i++) {
                        if (descriptionXml[i].Attributes != null && descriptionXml[i].Attributes["ShortDesc"] != null) {
                            product.setDescription(descriptionXml[i].Attributes["ShortDesc"].Value);
                        }
                    }

                    XmlNodeList multimediaXml = document.GetElementsByTagName("MultimediaObject");
                    if (multimediaXml.Count > 0) {
                        for (int i = 0; i < 1; i++) {
                            if (multimediaXml[i].Attributes != null) {
                                string url = multimediaXml[i].Attributes["URL"].Value;
                                if (url != "") {
                                    product.setManual(GetBinaryFromURL(url));
                                }
                            }
                        }
                    }

                    XmlNodeList categoryXml = document.GetElementsByTagName("Category");
                    for (int i = 0; i < 1; i++) {
                        int catid = int.Parse(categoryXml[i].Attributes["ID"].Value);
                        var catname = categoryXml[i].Attributes["Value"].Value;
                        product.setCategory(new Category(catid,catname));
                    }

                    XmlNodeList shortsumXml = document.GetElementsByTagName("ShortSummaryDescription");
                    for (int i = 0; i < 1; i++) {
                        string shortsum = shortsumXml[i].InnerText;
                        product.setShortSummary(shortsum);
                    }

                    XmlNodeList longsumXml = document.GetElementsByTagName("LongSummaryDescription");
                    for (int i = 0; i < 1; i++) {
                        string longsum = longsumXml[i].InnerText;
                        product.setLongSummary(longsum);
                        if (product.getDescription() == "" || product.getDescription() == null)
                            product.setDescription(longsum);
                    }

                    XmlNodeList galleryXml = document.GetElementsByTagName("ProductPicture");
                    var count = 5;
                    var size = 0;
                    if (count > galleryXml.Count)
                    {
                        size = galleryXml.Count;
                    }
                    else
                    {
                        size = count;
                    }
                        
                    for (int i = 0; i < count; i++) {
                        string url = galleryXml[i].Attributes["Pic500x500"].Value;
                        if (url != "") {
                            product.addPicture(GetBinaryFromURL(url));
                        }
                    }

                    XmlNodeList bulletsXml = document.GetElementsByTagName("BulletPoint");
                    for (int i = 0; i < bulletsXml.Count; i++) {
                        string bulletp = bulletsXml[i].Attributes["Value"].Value;
                        product.addBulletPoint(bulletp);
                    }

                    if(document.GetElementsByTagName("EANCode") != null) {
                        XmlNodeList eancodeXml = document.GetElementsByTagName("EANCode");
                            for (int i = 0; i < 1; i++) {
                                if(eancodeXml[i].Attributes["EAN"] != null) {
                                    string ean = eancodeXml[i].Attributes["EAN"].Value;
                                    product.setEANCode(ean);
                                }                            }
                    }

                    XmlNodeList productfeatureXml = document.GetElementsByTagName("ProductFeature");
                    for (int i = 0; i < productfeatureXml.Count; i++)
                    {
                        string feature = productfeatureXml[i].Attributes["Presentation_Value"].Value.ToLower();
                        if (feature.Length > 1 && feature.Length < 25)
                        {
                            if (feature.Contains(","))
                            {
                                string[] features = feature.Split(',');
                                foreach (var feat in features)
                                {
                                    var trimmedFeature = feat.Trim();
                                    if (!product.getFeatures().Contains(trimmedFeature))
                                    {
                                        product.addFeautre(trimmedFeature);
                                    }
                                }

                            }
                            else if (feature.Contains("/"))
                            {
                                string[] features = feature.Split('/');
                                foreach (var feat in features)
                                {
                                    var trimmedFeature = feat.Trim();
                                    if (!product.getFeatures().Contains(trimmedFeature))
                                    {
                                        product.addFeautre(trimmedFeature);
                                    }
                                }
                            }
                            else
                            {
                                var trimmedFeature = feature.Trim();
                                if (!product.getFeatures().Contains(trimmedFeature))
                                {
                                    product.addFeautre(trimmedFeature);
                                }
                            }

                        }

                    }

                    XmlNodeList supplierXml = document.GetElementsByTagName("Supplier");
                    for (int i = 0; i < 1; i++) {
                        int id = int.Parse(supplierXml[i].Attributes["ID"].Value);
                        var name = supplierXml[i].Attributes["Name"].Value;
                        product.setBrand(new Brand(id,name));
                    }

                    productsList.Add(product);
                    Console.Write("\r{0}    ", "Creating product entity " + productsList.Count + "/" + xmlList.Count + " - Fetching Images...");
                }
                return productsList;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw e;
            }
        }

    public ArrayList CreateBrandsFromXmlList(ArrayList xmlList) {

        ArrayList brandsList = new ArrayList();
        ArrayList brandids = new ArrayList();


        foreach (XmlDocument document in xmlList) {

            Brand brand = new Brand();

            XmlNodeList supplierXml = document.GetElementsByTagName("Supplier");
            for (int i = 0; i < 1; i++) {
                string name = supplierXml[i].Attributes["Name"].Value;
                int id = int.Parse(supplierXml[i].Attributes["ID"].Value);
                brand.setID(id);
                brand.setName(name);
            }

            if (!brandids.Contains(brand.getID())) {
                brandids.Add(brand.getID());
                brandsList.Add(brand);
            }
        }

        return brandsList;
    }

    public ArrayList CreateCategoriesFromXmlList(ArrayList xmlList) {

        ArrayList categorysList = new ArrayList();
        ArrayList catids = new ArrayList();

        foreach (XmlDocument document in xmlList) {

            Category category = new Category();

            XmlNodeList categoryXml = document.GetElementsByTagName("Category");
            for (int i = 0; i < 1; i++) {
                int catid = int.Parse(categoryXml[i].Attributes["ID"].Value);
                string name = categoryXml[i].FirstChild.Attributes["Value"].Value;
                category.setID(catid);
                category.setName(name);
            }

            if (!catids.Contains(category.getID())) {
                catids.Add(category.getID());
                categorysList.Add(category);
            }
        }
        return categorysList;
    }

    public ArrayList CreateTagsFromXmlList(ArrayList xmlList) {

        ArrayList tagList = new ArrayList();

        foreach (XmlDocument document in xmlList) {

            XmlNodeList tagXml = document.GetElementsByTagName("ProductFeature");
            for (int i = 0; i < tagXml.Count; i++) {
                string tag = tagXml[i].Attributes["Presentation_Value"].Value.ToLower();
                if (!tagList.Contains(tag)) {
                    tagList.Add(tag);
                    Console.WriteLine(tag);
                }
            }
        }
        return tagList;
    }

    public void BulkInsert(ArrayList brands, ArrayList categories, ArrayList products) {
        Console.WriteLine("\nStarting to bulk insert into DB");
        _dal.insertBrandBulk(brands);
        _dal.insertCategoryBulk(categories);
        //dal.insertTagsBulk(tags);
        _dal.insertProductBulk(products);
        Console.WriteLine("Database populated with data! Have FUN! :-D");
    }
    public void BulkUpdateProducts(ArrayList products) {
        Console.WriteLine("\nStarting to bulk update into DB");
        _dal.updateProductBulk(products);
    }

    public void ValidationCallBack(object sender, ValidationEventArgs e) {
        if (e.Severity == XmlSeverityType.Warning)
            Console.WriteLine("Warning: Matching schema not found.  No validation occurred." + e.Message);
        else // Error
            Console.WriteLine("Validation error: " + e.Exception);
    }

    }
}
