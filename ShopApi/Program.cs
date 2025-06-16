using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContexts
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<SecurityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SecurityConnection")));

// Add Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<SecurityDbContext>()
    .AddDefaultTokenProviders();

// Configure authentication
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ShopDbContext>();
    context.Database.EnsureCreated();
    
    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { Description = "Electronics" },
            new Category { Description = "Clothing" },
            new Category { Description = "Books" }
        );
        context.SaveChanges();
    }

    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product { Name = "Laptop", Description = "High performance laptop", Price = 999.99m, CategoryId = 1 },
            new Product { Name = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99m, CategoryId = 2 },
            new Product { Name = "Novel", Description = "Bestselling novel", Price = 12.99m, CategoryId = 3 }
        );
        context.SaveChanges();
    }
}

app.Run();