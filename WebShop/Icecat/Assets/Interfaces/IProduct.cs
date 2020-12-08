using System;
using System.Collections.Generic;
using System.Text;

namespace IceCat.Assets.Interfaces {
    public interface IProduct {

        int getID();

        void setID(int id);

        string getSKU();
        void setSKU(string sku);

        double getPrice();
        void setPrice(double price);

        string getDescription();
        void setDescription(string desc);

    }
}
