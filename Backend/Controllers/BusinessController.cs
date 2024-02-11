using Backend1.Abstractions;
using Backend1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend1.Controllers;

[ApiController]
[Route("api/business")]
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
}