using Backend1.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend1.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : Controller
{
    private InventoryContext _db;
    
    public InventoryController(InventoryContext db)
    {
        _db = db;
    }

    [HttpGet]
    [Route("size")]
    public ActionResult<string> GetSize()
    {
        var size = _db.Items.Count();
        var str = $"The global inventory currently contains {size} items.\n";

        return Ok(str);
    }
}