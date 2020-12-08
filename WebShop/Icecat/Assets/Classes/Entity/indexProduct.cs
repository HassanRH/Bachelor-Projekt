using System;
using System.Collections.Generic;
using System.Text;

namespace IceCat.Assets.Classes
{
    class indexProduct
    {
        // Constructor that takes no arguments:
        public indexProduct(int id, String prod_code, String prodpath, int Category_id, int supplier_id)
        {
            ID = id;
            Product_code = prod_code;
            Path = prodpath;
            CatID = Category_id;
            Supplier_ID = supplier_id;
        }

        // Auto-implemented readonly property:
        public int ID { get; }
        public string Path { get; }
        public int CatID { get; }
        public int Supplier_ID { get; }
        public string Product_code { get; }
        

        public override string ToString()
        {
            return "Product: ID:" + ID + " - ProductCode: " + Product_code + " - Path: " + Path + " - CatID: " + CatID + " - SupplierID: " + Supplier_ID;
        }
    }
}
