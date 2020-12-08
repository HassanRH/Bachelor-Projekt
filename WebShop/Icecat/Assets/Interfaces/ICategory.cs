using System;
using System.Collections.Generic;
using System.Text;

namespace IceCat.Assets.Interfaces {
    public interface ICategory {

        int getID();
        void setID(int id);

        string getName();
        void setName(string name);

    }
}
