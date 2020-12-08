using IceCat.Assets.Interfaces;
using System;
using System.Collections;

namespace IceCat.Assets.Classes
{
    class Product : IProduct
    {
        private int id;
        private string sku;
        private string title;
        private string description;
        private string longsummary;
        private string shortsummary;
        private Brand brand;
        private Category category;
        private byte[] thumbpic;
        private byte[] manual;
        private ArrayList pictures = new ArrayList();
        private double price;
        private ArrayList bulletpoints = new ArrayList();
        private string eancode;
        private ArrayList features = new ArrayList();

        public Product() {
            price = GenerateRandomPrice();
        }
        public Product(int Id, string Sku, string Ean, string Title, Brand Brand, Category Category, string Description, string Longsummary, string Shortsummary,  byte[] ThumbPic, byte[] Manual)
        {
            id = Id;
            sku = Sku;
            eancode = Ean;
            title = Title;
            description = Description;
            longsummary = Longsummary;
            shortsummary = Shortsummary;
            brand = Brand;
            category = Category;
            thumbpic = ThumbPic;
            manual = Manual;
            price = GenerateRandomPrice();
        }

        public ArrayList getPictures() {
            return pictures;
        }

        public Brand getBrand() {
            return this.brand;
        }
        public void setBrand(Brand brand) {
            this.brand = brand;
        }

        public Category getCategory() {
            return this.category;
        }

        public void setCategory(Category cat) {
            this.category = cat;
        }

        public string getLongSummary() {
            return longsummary;
        }
        public void setLongSummary(string longsummary) {
            this.longsummary = longsummary;
        }

        public string getShortSummary() {
            return shortsummary;
        }

        public void setShortSummary(string shortsummary) {
            this.shortsummary = shortsummary;
        }

        public string getTitle() {
            return title;
        }

        public void setTitle(string title) {
            this.title = title;
        }

        public void addPicture(byte[] picture) {
            pictures.Add(picture);
        }

        public string getDescription() {
            return description;
        }

        public int getID() {
            return id;
        }

        public double getPrice() {
            return price;
        }

        public string getSKU() {
            return sku;
        }


        public void setDescription(string desc) {
            this.description = desc;
        }

        public void setID(int id) {
            this.id = id;
        }


        public void setPrice(double price) {
            this.price = price;
        }

        public void setSKU(string sku) {
            this.sku = sku;
        }

        public byte[] getThumbpic() {
            return thumbpic;
        }

        public void setThumbpic(byte[] picture) {
            this.thumbpic = picture;
        }
        public byte[] getManual() {
            return manual;
        }

        public void setManual(byte[] manual) {
            this.manual = manual;
        }

        public ArrayList getBulletPoints() {
            return bulletpoints;
        }

        public void addBulletPoint(string bulletp) {
            this.bulletpoints.Add(bulletp);
        }
        public string getEANCode() {
            return eancode;
        }
        public void setEANCode(string ean) {
            this.eancode = ean;
        }
        public ArrayList getFeatures() {
            return features;
        }

        public void addFeautre(string feature) {
            this.features.Add(feature);
        }



        private double GenerateRandomPrice() {
            int minimum = 225;
            int maximum = 1499;
            Random random = new Random();
            double[] afterDot = { 0.25, 0.5, 0.75, 0.99 };
            int randomDot = random.Next(0, 4);
            return random.Next(minimum, maximum) + afterDot[randomDot];
        }

        public override string ToString() {
            return "{\nProduct - ID:" + id + "\nProductCode - " + sku + "\nShort Desc: " + shortsummary + "\nBrand - " + brand + "\nCategoryID - " + category + "\n}";
        }

        public string ToStringShort() {
            return "{ Product - ID:" + id + "\tProductCode - " + sku + " }\n";
        }
    }
}
