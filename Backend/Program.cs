using Backend1;
using Backend1.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

// TODO: I think allowing any origin in prod is bad; for dev purposes, I added this.
app.UseCors("AllowAnyOrigin");

app.MapControllers();

app.Run();