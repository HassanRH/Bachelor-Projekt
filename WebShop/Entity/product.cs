using System.Collections;
using System.Collections.Generic;

namespace WebShop.Entity
{
    public class product
    {
        //Constructors
        public product()
        {
            this.pictures = new List<int>();
            this.bulletpoints = new List<string>();
            this.tags = new List<string>();
        }
        public product(int id, string title, string sku, string description, string longsummary, string shortsummary, string brand, string category, List<int> pictures, List<string> bulletpoints, List<string> tags, double price)
        {
            Id = id;
            Title = title;
            Sku = sku;
            Description = description;
            Longsummary = longsummary;
            Shortsummary = shortsummary;
            this.Brand = brand;
            this.Category = category;
            this.pictures = pictures;
            this.bulletpoints = bulletpoints;
            this.tags = tags;
            Price = price;
        }

        //Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Sku { get; set; }
        public string Ean { get; set; }
        public string Description { get; set; }
        public string Longsummary { get; set; }
        public string Shortsummary { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }

        public int Amount { get; set; }
        public List<int> pictures { get; set; }
        public List<string> bulletpoints { get; set; }
        public List<string> tags { get; set; }
        public double Price { get; set; }

        public double getTotalPrice()
        {
            return this.Amount * this.Price;
        }

        public void addPicture(int pictureId)
        {
            this.pictures.Add(pictureId);
        }
        public void addBulletPoint(string bulletp)
        {
            this.bulletpoints.Add(bulletp);
        }

        public void addTag(string feature)
        {
            this.tags.Add(feature);
        }
    }
}