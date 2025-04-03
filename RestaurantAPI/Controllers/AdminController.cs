using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard: list all pending reservations
        public async Task<IActionResult> Dashboard()
        {
            var pendingReservations = await _context.Reservations
                .Include(r => r.Restaurant)
                .Include(r => r.User)
                .Where(r => r.Status == "Pending")
                .ToListAsync();
            return View(pendingReservations);
        }

        // Accept a reservation (set status to "Accepted")
        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                reservation.Status = "Accepted";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        // Decline a reservation (set status to "Declined")
        [HttpPost]
        public async Task<IActionResult> Decline(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                reservation.Status = "Declined";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }
    }
}
