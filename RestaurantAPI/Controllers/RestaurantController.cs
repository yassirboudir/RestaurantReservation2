using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RestaurantAPI.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Restaurants
        // Lists all restaurants
        public async Task<IActionResult> Index()
        {
            var restaurants = await _context.Restaurants.ToListAsync();
            return View(restaurants);
        }

        // GET: /Restaurants/Details/5
        // Shows details for a specific restaurant, including reviews, images, average rating, etc.
        public async Task<IActionResult> Details(int id)
        {
            // Load restaurant with reviews & user references
            var restaurant = await _context.Restaurants
                .Include(r => r.Reviews)
                    .ThenInclude(rv => rv.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            // Calculate average rating (out of 5) & total reviews
            double avgRating = 0.0;
            int totalReviews = 0;
            if (restaurant.Reviews != null && restaurant.Reviews.Any())
            {
                avgRating = restaurant.Reviews.Average(r => r.Rating); // average on a 1–5 scale
                totalReviews = restaurant.Reviews.Count;
            }
            // Scale to 1–10
            double avgOutOfTen = avgRating * 2.0;

            // Pass to ViewBag so your Razor view can display them
            ViewBag.AverageOutOfTen = avgOutOfTen;
            ViewBag.TotalReviews = totalReviews;

            // Load images if you have a RestaurantImage model/table
            var images = await _context.RestaurantImages
                .Where(img => img.RestaurantId == id)
                .ToListAsync();
            ViewBag.Images = images;

            // Check if the user is logged in & if it's a favorite
            var userId = HttpContext.Session.GetInt32("UserId");
            bool isFavorite = false;
            if (userId != null)
            {
                isFavorite = await _context.Favorites.AnyAsync(f => f.RestaurantId == id && f.UserId == userId.Value);
            }
            ViewBag.IsFavorite = isFavorite;

            return View(restaurant);
        }
    }
}
