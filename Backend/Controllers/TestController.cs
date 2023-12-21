using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;

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