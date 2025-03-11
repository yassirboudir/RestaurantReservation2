using RestaurantAPI;
using RestaurantAPI.Data;
using RestaurantAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services using Startup.cs
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    
    if (!db.Restaurants.Any())
    {
        // Create sample restaurants with a featured image (ImageUrl)
        var tacos = new Restaurant
        {
            Name = "Tacos square",
            Description = "Authentic Mexican tacos.",
            Address = "123 Taco Blvd",
            Phone = "123456789",
            Website = "https://www.tacossquare.com",
            ImageUrl = "tacos1.jpg"  // Featured image for index page
        };
        var maitre = new Restaurant
        {
            Name = "Le maitre chicken",
            Description = "Delicious roasted chicken.",
            Address = "456 Chicken Road",
            Phone = "987654321",
            Website = "https://www.lemaitrechicken.com",
            ImageUrl = "maitre1.jpg"
        };
        var pizza = new Restaurant
        {
            Name = "Pizza hut",
            Description = "Famous pizza chain.",
            Address = "789 Pizza Street",
            Phone = "5555555555",
            Website = "https://www.pizzahut.com",
            ImageUrl = "pizza1.jpg"
        };
        db.Restaurants.AddRange(tacos, maitre, pizza);
        db.SaveChanges();
        
        // Seed additional images for each restaurant in wwwroot/images/restaurants/
        db.RestaurantImages.AddRange(
            new RestaurantImage { RestaurantId = tacos.Id, FileName = "tacos1.jpg" },
            new RestaurantImage { RestaurantId = tacos.Id, FileName = "tacos2.jpg" },
            new RestaurantImage { RestaurantId = maitre.Id, FileName = "maitre1.jpg" },
            new RestaurantImage { RestaurantId = maitre.Id, FileName = "maitre2.jpg" },
            new RestaurantImage { RestaurantId = pizza.Id, FileName = "pizza1.jpg" },
            new RestaurantImage { RestaurantId = pizza.Id, FileName = "pizza2.jpg" }
        );
        db.SaveChanges();
    }
}

startup.Configure(app, app.Environment);
app.Run();
