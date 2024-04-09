using Backend1.Abstractions;
using Backend1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Backend1.Controllers;

[ApiController]
[Route("api/business")]
[Authorize]
public class BusinessController : Controller
{
    private readonly IBusinessService _businessService;
    
    public BusinessController(IBusinessService businessService)
    {
        _businessService = businessService;
    }

    [HttpPut("register")]
    public ActionResult Register([FromBody] Business business)
    {
        var rv = _businessService.Add(business);

        // Failed to add.
        if (rv == false)
            return StatusCode(500);
        
        return Ok(business);
    }

    // Maybe in the future just return the actual business object?
    // For now, just return a boolean.
    [HttpGet]
    public ActionResult IsRegistered()
    {
        var rv = _businessService.Find(User);

        // Business does not exist.
        if (rv == false)
            return StatusCode(500);

        return Ok(rv);
    }
}
