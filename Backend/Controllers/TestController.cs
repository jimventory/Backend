using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend1.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : Controller
{
    public TestController()
    {
        
    }

    [HttpGet("helloWorld")]
    [EnableCors]
    public ActionResult<string> HelloWorld([FromQuery] string name = "world")
    {
        string s = $"Hello, {name}!\n";
        return Ok(s);
    }
}