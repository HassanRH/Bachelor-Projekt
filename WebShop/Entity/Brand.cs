namespace WebShop.Entity
{
    public class Brand
    {
        public Brand() { }
        public Brand(int iD, string name, byte[] image)
        {
            ID = iD;
            Name = name;
            Image = image;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }


}