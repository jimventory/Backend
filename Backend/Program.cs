using System.Security.Claims;
using Backend1;
using Backend1.Abstractions;
using Backend1.Data;
using Backend1.Repositories;
using Backend1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var domain = $"https://{EnvVarHelper.GetVariable("AUTH0_DOMAIN")}/";

builder.Services.AddDbContext<InventoryContext>();
builder.Services.AddDbContext<BusinessContext>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddControllers();

// TODO: I think allowing any origin in prod is bad; for dev purposes, I added this.
builder.Services.AddCors(options =>
{
    
    options.AddPolicy("AllowAnyOrigin", bd =>
    {
        bd.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = EnvVarHelper.GetVariable("AUTH0_AUDIENCE");
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