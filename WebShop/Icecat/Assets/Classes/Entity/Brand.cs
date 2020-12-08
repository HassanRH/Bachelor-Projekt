using IceCat.Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCat.Assets.Classes
{
    class Brand : IBrand
    {
        private int BrandID;
        private string Brand_name;
        private byte[] Brand_logo;

        public Brand(int brandID, string brand_name)
        {
            BrandID = brandID;
            Brand_name = brand_name;
        }

        public Brand(int id, string name, byte[] logo) {
            BrandID = id;
            Brand_name = name;
            Brand_logo = logo;
        }

        public Brand() { }

        public int getID() {
            return BrandID;
        }

        public string getName() {
            return Brand_name;
        }
        
        public byte[] getLogo() {
            return Brand_logo;
        }

        public void setLogo(byte[] picture) {
            this.Brand_logo = picture;
        }

        public void setID(int id) {
            BrandID = id;
        }

        public void setName(string name) {
            Brand_name = name;
        }

    }
}
