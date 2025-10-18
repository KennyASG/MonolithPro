namespace MonolithPro.Modules.Inventory;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }

    public Product(int id, string name, int stock, decimal price)
    {
        Id = id;
        Name = name;
        Stock = stock;
        Price = price;
    }
}