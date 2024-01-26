using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Linq;

namespace _shop_serialization_
{

    [Serializable]
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }

        public Product() { }
        public Product(int id, string name, string type, int price, int amount)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"Id: {Id} Name: {Name, -20} Type: {Type,-20} Price: {Price,-10} Amount: {Amount}\n";
        }
    }


    [Serializable]
    class Shop
    {
        protected Dictionary<int, Product> dict = new Dictionary<int, Product>();
        public Dictionary<int, Product> Dict { get => dict; set => dict = value; }
        public Shop() { }
        public Shop(params Product[] prodacts)
        {
            foreach (var prodact in prodacts)
            {
                dict.Add(prodact.Id, prodact);
            }
        }

        public static void SaveToFileBin(Shop s, string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using(FileStream fs = new FileStream(filename, FileMode.Create))
            {
                bf.Serialize(fs, s);
            }
        }

        public static Shop ReadFromFileBin(string filename)
        {
            Shop s = new Shop();
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                s = (Shop)bf.Deserialize(fs);
            }
            return s;
        }
        public static void SaveToFileJson(Shop s, string filename)
        {
            string json = JsonSerializer.Serialize<Shop>(s);
            File.WriteAllText(filename, json);
        }

        public static Shop ReadFromFileJson(string filename)
        {
            Shop s = JsonSerializer.Deserialize<Shop>(File.ReadAllText(filename)) ?? throw new ArgumentNullException($"ReadFromFileJson { filename }");
            return s;
        }



        public  override string ToString()
        {
            return String.Join("", Dict.Values);
        }
    }







    internal class Program
    {
        static void Main(string[] args)
        {
            
            Shop s = new Shop(
                new Product(1, "Laptop", "Electronics", 1200, 10),
                new Product(2, "Coffee Maker", "Appliances", 50, 25),
                new Product(3, "Running Shoes", "Apparel", 80, 30),
                new Product(4, "Smartphone", "Electronics", 800, 15),
                new Product(5, "Toaster", "Appliances", 30, 20));

            Console.WriteLine(s);
            //Shop.SaveToFileBin(s, "shop.dat");

            //Shop shopNew = Shop.ReadFromFileBin("shop.dat");
            //Console.WriteLine();
            //Console.WriteLine(shopNew);

            Shop.SaveToFileJson(s, "Shop.json");

            Shop shopNewJson = Shop.ReadFromFileJson("shop.json");
            Console.WriteLine();
            Console.WriteLine(shopNewJson);
        }
    }
}