using Backend1.Abstractions;
using Backend1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Backend1.Controllers;

[ApiController]
[Route("api/inventory")]
[Authorize]
public class InventoryController : Controller
{
    private readonly IItemService _itemService;
    private readonly ILogger _logger;

    public InventoryController(IItemService itemService, ILogger<InventoryController> logger)
    {
        _itemService = itemService;
        _logger = logger;
    }
    
    [HttpGet]
    [Route("private")]
    public ActionResult PrivateEndpointPlaceholder()
    {
        return Ok("This is a private endpoint in the inventory controller.\n");
    }

    [HttpPost]
    [Route("add")]
    public ActionResult AddItem([FromBody] Item newItem)
    {
        var rv = _itemService.Add(newItem, User);

        // Failed to add.
        if (rv == false)
            return StatusCode(500);
        
        return Ok(newItem);
    }

    [HttpPut]
    [Route("update")]
    public ActionResult UpdateItem([FromBody] Item updateItem)
    {
        var rv = _itemService.Update(updateItem, User);

        if (rv == false)
            return StatusCode(500);

        return Ok($"Updated item with ID {updateItem.Id}.");
    }
    
    [HttpDelete]
    [Route("remove/{id}")]
    public ActionResult DeleteItem(uint id)
    {
        var rv = _itemService.Delete(id, User);

        if (rv == false)
            return StatusCode(500);

        return Ok($"Deleted item with ID {id}.");
    }

    [HttpGet]
    [Route("getInventory")]
    public ActionResult GetInventory()
    {
        var rv = _itemService.GetBusinessInventoryItems(User);

        // Previously, we returned StatusCode 500 if we couldn't find any items.
        // That is not necessarily an error.  They could just have no items.
        // So, don't treat it like an error.

        return Ok(rv);
    }
}
