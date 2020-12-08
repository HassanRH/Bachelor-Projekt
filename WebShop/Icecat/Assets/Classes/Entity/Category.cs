using IceCat.Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCat.Assets.Classes
{
    class Category : ICategory
    {

    public Category() { }

        public Category(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        private int id;
        private string name;

        public int getID() {
            return id;
        }

        public string getName() {
            return name;
        }

        public void setID(int id) {
            this.id = id;
        }

        public void setName(string name) {
            this.name = name;
        }
    }
}
