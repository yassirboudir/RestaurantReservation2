using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Dashboard(int restaurantId)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.Reservations)
                    .ThenInclude(res => res.User)
                .Include(r => r.Reviews)
                    .ThenInclude(rv => rv.User)
                .FirstOrDefaultAsync(r => r.Id == restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }
    }
}
