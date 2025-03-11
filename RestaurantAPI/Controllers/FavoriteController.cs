using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RestaurantAPI.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FavoriteController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var favorites = await _context.Favorites.Include(f => f.Restaurant).Where(f => f.UserId == userId).ToListAsync();
            return View(favorites);
        }
        [HttpPost]
        public async Task<IActionResult> Add(int restaurantId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var exists = await _context.Favorites.AnyAsync(f => f.RestaurantId == restaurantId && f.UserId == userId);
            if(!exists)
            {
                var favorite = new Favorite { RestaurantId = restaurantId, UserId = userId.Value };
                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }
        [HttpPost]
        public async Task<IActionResult> Remove(int restaurantId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.RestaurantId == restaurantId && f.UserId == userId);
            if(favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Restaurants", new { id = restaurantId });
        }
    }
}
