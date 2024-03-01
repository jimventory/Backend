using Backend1.Abstractions;
using Backend1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend1.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : Controller
{
    private readonly IItemService _itemService;
    
    public InventoryController(IItemService itemService)
    {
        _itemService = itemService;
    }
    
    [HttpGet("private")]
    [EnableCors]
    [Authorize]
    public ActionResult PrivateEndpointPlaceholder()
    {
        return Ok("This is a private endpoint in the inventory controller.\n");
    }

    [HttpPost]
    [Route("add")]
    public ActionResult AddItem([FromBody] Item newItem)
    {
        var rv = _itemService.Add(newItem);

        // Failed to add.
        if (rv == false)
            return StatusCode(500);
        
        return Ok(newItem);
    }

    [HttpPut]
    [Route("update")]
    public ActionResult UpdateItem([FromBody] Item updateItem)
    {
        var rv = _itemService.Update(updateItem);

        if (rv == false)
            return StatusCode(500);

        return Ok($"Updated item with ID {updateItem.Id}.");
    }
    
    [HttpDelete]
    [Route("remove/{id}")]
    public ActionResult DeleteItem(uint id)
    {
        var rv = _itemService.Delete(id);

        if (rv == false)
            return StatusCode(500);

        return Ok($"Deleted item with ID {id}.");
    }

    [HttpGet]
    [Route("getInventory/{businessId}")]
    public ActionResult GetInventory(uint businessId)
    {
        var rv = _itemService.GetBusinessInventoryItems(businessId);

        if (rv.IsNullOrEmpty())
            return StatusCode(500);

        return Ok(rv);
    }
}
