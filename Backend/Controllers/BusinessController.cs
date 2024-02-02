using Backend1.Factories;
using Backend1.Data;
using Backend1.Types;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend1.Controllers;

[ApiController]
[Route("api/business")]
public class BusinessController : Controller
{
    private readonly BusinessFactory _factory = new();
    private BusinessContext _db;
    
    public BusinessController(BusinessContext db)
    {
        _db = db;
    }

    [HttpGet("helloWorld")]
    [EnableCors]
    public ActionResult<string> HelloWorld([FromQuery] string name = "world")
    {
        string s = $"Hello, {name}!\n";
        return Ok(s);
    }

    [HttpPut("register")]
    [EnableCors]
    public ActionResult<string> Register([FromBody] BusinessRegistrationForm form)
    {
        var business = _factory.MakeBusiness(form);
        
        _db.Businesses.Add(business);

        try
        {
            _db.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
        
        return Ok($"This in development, but we've got you down on our list!");
    }
}