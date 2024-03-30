using Backend1.Models;
using System.Diagnostics.CodeAnalysis;
namespace TestHelpers;

[ExcludeFromCodeCoverage]
public static class BusinessHelper
{
    public static Business GetBoilerplateBusiness(string? name = null, uint? id = null, string? location = null)
    {
        return new Business
        {
            Name = name ?? "FakeBusiness",
            Id = id ?? 0,
            Location = location ?? "Knoxville"
        };
    }
}
