using Backend1.Data;
using Backend1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    
    [HttpGet("private")]
    [EnableCors]
    [Authorize]
    public ActionResult<string> PrivateEndpointPlaceholder()
    {
        return Ok("This is a private endpoint in the inventory controller.\n");
    }

    [HttpPost]
    [Route("add")]
    public ActionResult AddItem([FromBody] Item newItem)
    {
        _db.Items.Add(newItem);
        _db.SaveChanges();

        return Ok(newItem);
    }

    [HttpDelete]
    [Route("remove/{id}")]
    public ActionResult DeleteItem(uint id) {
        var item = _db.Items.Find(id);
        
        if (item == null) {
            return NotFound($"Item {id} not found");
        }
        Console.WriteLine(item.Name);
        _db.Items.Remove(item);
        _db.SaveChanges();
        return Ok("item removed");
    }

}