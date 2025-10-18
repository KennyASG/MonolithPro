using Microsoft.AspNetCore.Mvc;

namespace MonolithPro.Modules.Inventory;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public InventoryController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public ActionResult<List<Product>> GetAllProducts()
    {
        var products = _inventoryService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProductById(int id)
    {
        var product = _inventoryService.GetProductById(id);
        
        if (product == null)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        return Ok(product);
    }

    [HttpPut("{id}/stock")]
    public ActionResult UpdateStock(int id, [FromBody] UpdateStockRequest request)
    {
        var success = _inventoryService.UpdateStock(id, request.Quantity);
        
        if (!success)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        var product = _inventoryService.GetProductById(id);
        return Ok(new { 
            message = "Stock updated successfully", 
            productId = id,
            newStock = product.Stock
        });
    }
}

public class UpdateStockRequest
{
    public int Quantity { get; set; }
}