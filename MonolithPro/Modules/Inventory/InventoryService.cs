using MonolithPro.Shared;

namespace MonolithPro.Modules.Inventory;

public class InventoryService
{
    private readonly List<Product> _products;
    private readonly DatabaseContext _dbContext;
    private readonly LoggerService _logger;

    public InventoryService(DatabaseContext dbContext, LoggerService logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        
        // Datos iniciales en memoria
        _products = new List<Product>
        {
            new Product(1, "Laptop HP", 50, 899.99m),
            new Product(2, "Mouse Logitech", 150, 29.99m),
            new Product(3, "Teclado Mecánico", 80, 79.99m),
            new Product(4, "Monitor Samsung 24\"", 30, 199.99m),
            new Product(5, "Webcam HD", 100, 49.99m)
        };
        
        _logger.Log("InventoryService initialized with sample products");
    }

    public List<Product> GetAllProducts()
    {
        _logger.Log("Getting all products");
        return _products;
    }

    public Product GetProductById(int id)
    {
        _logger.Log($"Getting product by ID: {id}");
        var product = _products.FirstOrDefault(p => p.Id == id);
        
        if (product == null)
        {
            _logger.LogWarning($"Product with ID {id} not found");
        }
        
        return product;
    }

    public bool UpdateStock(int productId, int quantity)
    {
        _logger.Log($"Updating stock for product {productId}, quantity: {quantity}");
        var product = GetProductById(productId);
        
        if (product == null)
        {
            _logger.LogError($"Cannot update stock: Product {productId} not found");
            return false;
        }

        product.Stock += quantity;
        _dbContext.SaveChanges();
        _logger.Log($"Product {productId} stock updated. New stock: {product.Stock}");
        return true;
    }

    public bool CheckAndReserveStock(int productId, int quantity)
    {
        _logger.Log($"Checking and reserving stock for product {productId}, quantity: {quantity}");
        var product = GetProductById(productId);
        
        if (product == null)
        {
            _logger.LogError($"Cannot reserve stock: Product {productId} not found");
            return false;
        }

        if (product.Stock < quantity)
        {
            _logger.LogWarning($"Insufficient stock for product {productId}. Available: {product.Stock}, Requested: {quantity}");
            return false;
        }

        product.Stock -= quantity;
        _dbContext.SaveChanges();
        _logger.Log($"Stock reserved for product {productId}. Remaining stock: {product.Stock}");
        return true;
    }

    public decimal GetProductPrice(int productId)
    {
        var product = GetProductById(productId);
        return product?.Price ?? 0;
    }
}