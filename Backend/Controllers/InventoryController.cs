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

    [HttpPost]
    [Route("uploadInventory")]
    public ActionResult UploadInventory()
    {
        if (Request.Form.Files.Count == 0)
            return BadRequest("No file uploaded.");
        if (Request.Form.Files.Count > 1)
            return BadRequest("To many files uploaded.");

        const int maxFileSize = 5 * 1024 * 1024; // 5 MB in bytes
        string[] permittedExtensions = { ".csv" };
        var file = Request.Form.Files[0];
        var filename = Path.GetFileName(file.FileName);

        // Doing some file checks
        if (file == null || file.Length == 0)
            return BadRequest("File is empty.");
        if (file.Length > maxFileSize)
            return BadRequest("File is to large.");
        if (!permittedExtensions.Contains(Path.GetExtension(filename)))
            return BadRequest("File type not allowed.");

        try
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                int itemCount = 0;
                while (!reader.EndOfStream)
                {
                    Console.Write(itemCount);
                    var line = reader.ReadLine();
                    var values = line.Split(",");

                    // Checking that their are not more attributes than possible
                    if (values.Length > 7)
                    {
                        _logger.LogError($"To many values on line: {line}");
                        continue;
                    }

                    // Want at least something for the name
                    if (string.IsNullOrWhiteSpace(values[0]))
                    {
                        _logger.LogError($"The first value is empty on line: {line}");
                        continue;
                    }

                    // Adding elements to an item if they are valid
                    var item = new Item
                    {
                        Name = values[0],
                        Quantity = values.Length > 1 && uint.TryParse(values[1], out uint quantity) ? quantity : 0,
                        Price = values.Length > 2 && double.TryParse(values[2], out double price) ? price : 0.0,
                        About = values.Length > 3 ? values[3] : string.Empty,
                        ImageUrl = values.Length > 4 ? values[4] : string.Empty,
                        Sales = values.Length > 5 && uint.TryParse(values[5], out uint sales) ? sales : 0,
                        LowStockNotification = values.Length > 6 && uint.TryParse(values[6], out uint stock) ? stock : 0,
                    };

                    _itemService.Add(item, User);
                    itemCount++;
                }
                if (itemCount > 0)
                    return Ok($"Added {itemCount} new items.");
                else return BadRequest("No valid items in file.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing CSV File: {ex.Message}");
            return StatusCode(500);
        }
    }
}
