using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IceCat.Assets.Interfaces {
    public interface IDataAccessLayer {

        ArrayList getProducts();
        ArrayList getBrands();
        ArrayList getCategories();

        ArrayList getProduct(int id);

        ArrayList getBrand(int id);
        ArrayList getCategory(int id);

        void insertProduct(IProduct product);
        void deleteProduct(int id);
        ArrayList getProductIds();
        void updateProductBulk(ArrayList products);
        void insertBrandBulk(ArrayList brands);
        void insertCategoryBulk(ArrayList categories);
        void insertProductBulk(ArrayList products);
        void initDatabase();
        void teardownDatabase();
    }
}
