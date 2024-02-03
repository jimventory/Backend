using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Backend1.Types;

public class BusinessRegistrationForm
{
    [FromBody]
    public string Name { get; set; }
    [FromBody]
    public string Location { get; set; }
    
}