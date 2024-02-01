using Backend1.Business;
using Backend1.Types;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend1.Controllers;

[ApiController]
[Route("api/business")]
public class BusinessController : Controller
{
    private BusinessRegistrar _registrar = new();
    
    public BusinessController()
    {
        
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
    public ActionResult<string> Register([FromBody] BusinessRegistrationForm business)
    {
        _registrar.Register(business);
        return Ok($"Registration is in development, {business.Name} was not registered.");
    }
}