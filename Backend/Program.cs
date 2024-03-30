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
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IUserBusinessIdResolver, UserBusinessIdResolver>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowSpecificOrigins",
            policy =>
            {
                policy.WithOrigins("http://localhost:3000",
                        "https://frontend-brown-gamma-36.vercel.app/")
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
            ValidateAudience = true
        };
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowSpecificOrigins");

app.MapControllers();

app.Run();

public partial class Program { }
