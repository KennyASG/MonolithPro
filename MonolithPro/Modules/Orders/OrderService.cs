using MonolithPro.Shared;
using MonolithPro.Modules.Users;
using MonolithPro.Modules.Inventory;

namespace MonolithPro.Modules.Orders;

public class OrderService
{
    private readonly List<Order> _orders;
    private readonly DatabaseContext _dbContext;
    private readonly LoggerService _logger;
    private readonly EmailService _emailService;
    private readonly UserService _userService;
    private readonly InventoryService _inventoryService;
    private int _nextId = 1;

    public OrderService(
        DatabaseContext dbContext, 
        LoggerService logger, 
        EmailService emailService,
        UserService userService,
        InventoryService inventoryService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _emailService = emailService;
        _userService = userService;
        _inventoryService = inventoryService;
        _orders = new List<Order>();
        
        _logger.Log("OrderService initialized");
    }

    public List<Order> GetAllOrders()
    {
        _logger.Log("Getting all orders");
        return _orders;
    }

    public Order GetOrderById(int id)
    {
        _logger.Log($"Getting order by ID: {id}");
        return _orders.FirstOrDefault(o => o.Id == id);
    }

    public Order CreateOrder(int userId, int productId, int quantity)
    {
        _logger.Log($"Creating order - User: {userId}, Product: {productId}, Quantity: {quantity}");
        
        // ACOPLAMIENTO DIRECTO: Dependencia con UserService
        var user = _userService.GetUserById(userId);
        if (user == null)
        {
            _logger.LogError($"Cannot create order: User {userId} not found");
            throw new InvalidOperationException($"User with ID {userId} does not exist");
        }

        // ACOPLAMIENTO DIRECTO: Dependencia con InventoryService
        var product = _inventoryService.GetProductById(productId);
        if (product == null)
        {
            _logger.LogError($"Cannot create order: Product {productId} not found");
            throw new InvalidOperationException($"Product with ID {productId} does not exist");
        }

        // ACOPLAMIENTO DIRECTO: Verificar y reservar stock
        bool stockReserved = _inventoryService.CheckAndReserveStock(productId, quantity);
        if (!stockReserved)
        {
            _logger.LogError($"Cannot create order: Insufficient stock for product {productId}");
            throw new InvalidOperationException($"Insufficient stock for product {productId}");
        }

        // Calcular total usando el precio del inventario
        decimal total = _inventoryService.GetProductPrice(productId) * quantity;

        var order = new Order(_nextId++, userId, productId, quantity, total);
        order.Status = "Confirmed";
        _orders.Add(order);
        
        _dbContext.SaveChanges();
        
        // ACOPLAMIENTO DIRECTO: Enviar email de confirmación al usuario
        _emailService.SendOrderConfirmation(user.Email, order.Id);
        
        _logger.Log($"Order created successfully. Order ID: {order.Id}, Total: ${order.Total}");
        return order;
    }

    public List<Order> GetOrdersByUserId(int userId)
    {
        _logger.Log($"Getting orders for user: {userId}");
        
        // ACOPLAMIENTO DIRECTO: Verificar que el usuario existe
        if (!_userService.UserExists(userId))
        {
            _logger.LogWarning($"User {userId} not found while fetching orders");
            return new List<Order>();
        }
        
        return _orders.Where(o => o.UserId == userId).ToList();
    }
}