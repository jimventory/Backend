using Microsoft.AspNetCore.Mvc;

namespace Backend1.Types;

public class BusinessRegistrationForm
{
    [FromBody]
    public string Name { get; set; }
    [FromBody]
    public string Location { get; set; }

    public override int GetHashCode()
    {
        return (Name + Location).GetHashCode();
    }
}