using Microsoft.AspNetCore.Mvc;

namespace MonolithPro.Modules.Orders;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public ActionResult<List<Order>> GetAllOrders()
    {
        var orders = _orderService.GetAllOrders();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public ActionResult<Order> GetOrderById(int id)
    {
        var order = _orderService.GetOrderById(id);
        
        if (order == null)
        {
            return NotFound(new { message = $"Order with ID {id} not found" });
        }
        
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public ActionResult<List<Order>> GetOrdersByUser(int userId)
    {
        var orders = _orderService.GetOrdersByUserId(userId);
        return Ok(orders);
    }

    [HttpPost]
    public ActionResult<Order> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            var order = _orderService.CreateOrder(
                request.UserId, 
                request.ProductId, 
                request.Quantity
            );
            
            return CreatedAtAction(
                nameof(GetOrderById), 
                new { id = order.Id }, 
                order
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public class CreateOrderRequest
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}