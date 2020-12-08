using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebShop.Entity
{
    public class SolrModel
     {
        //Constructors
        public SolrModel()
        {
            this.Pictures = new List<int>();
            this.Bulletpoints = new List<string>();
            this.Tags = new List<string>();
        }
        public SolrModel(int id, string title, string sku, string description, string longsummary, string shortsummary, string brand, string category, List<int> pictures, List<string> bulletpoints, List<string> tags, double price)
        {
            this.id = id;
            Title = title;
            Sku = sku;
            Description = description;
            Longsummary = longsummary;
            Shortsummary = shortsummary;
            this.Brand = brand;
            this.Category = category;
            this.Pictures = pictures;
            this.Bulletpoints = bulletpoints;
            this.Tags = tags;
            Price = price;
        }

        //Properties
        public int id { get; set; }

        public string Title { get; set; }

        public string Sku { get; set; }

        public string Ean { get; set; }
        public string Description { get; set; }

        public string Longsummary { get; set; }

        public string Shortsummary { get; set; }

        public string Brand { get; set; }
        public string Category { get; set; }
        public List<int> Pictures { get; set; }
        public List<string> Bulletpoints { get; set; }

        public List<string> Tags { get; set; }

        public double Price { get; set; }

        public void addPicture(int pictureId)
        {
            this.Pictures.Add(pictureId);
        }
        public void addBulletPoint(string bulletp)
        {
            this.Bulletpoints.Add(bulletp);
        }

        public void addTag(string feature)
        {
            this.Tags.Add(feature);
        }
    }
}