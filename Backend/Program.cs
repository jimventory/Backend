using System.Security.Claims;
using Backend1;
using Backend1.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Try to get env. var.
var dom = Environment.GetEnvironmentVariable("AUTH0_DOMAIN");
if (dom is null)
    throw new Exception("Couldn't find AUTH0 domain.");

Console.WriteLine(dom);

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";

builder.Services.AddDbContext<InventoryContext>();
builder.Services.AddControllers();

// TODO: I think allowing any origin in prod is bad; for dev purposes, I added this.
builder.Services.AddCors(options =>
{
    
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// TODO: I think allowing any origin in prod is bad; for dev purposes, I added this.
app.UseCors("AllowAnyOrigin");

app.MapControllers();

app.Run();