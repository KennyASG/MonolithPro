namespace MonolithPro.Modules.Orders;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }

    public Order(int id, int userId, int productId, int quantity, decimal total)
    {
        Id = id;
        UserId = userId;
        ProductId = productId;
        Quantity = quantity;
        Total = total;
        CreatedAt = DateTime.Now;
        Status = "Pending";
    }
}