using System;
using System.Collections;

namespace IceCat.Assets.Classes.DataAccessLayer.DatabaseEntity {
    public class dbProduct {
        public int id { get; set; }
        public string sku { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string longsummary { get; set; }
        public string shortsummary { get; set; }
        public int brand { get; set; }
        public int category { get; set; }
        public byte[] thumbpic { get; set; }
        public byte[] manual { get; set; }
        public ArrayList pictures { get; set; }
        public double price { get; set; }
        public ArrayList bulletpoints { get; set; }
        public string eancode { get; set; }
        public ArrayList features { get; set; }

    }
}
