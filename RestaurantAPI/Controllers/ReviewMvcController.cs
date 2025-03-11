using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RestaurantAPI.Controllers
{
    public class ReviewMvcController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReviewMvcController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Create(int restaurantId, int rating, string comment)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var review = new Review
            {
                RestaurantId = restaurantId,
                UserId = userId.Value,
                Rating = rating,
                Comment = comment
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }
    }
}
