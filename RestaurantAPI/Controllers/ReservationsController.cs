using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;
using RestaurantAPI.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace RestaurantAPI.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Reservations/MakeReservation?restaurantId=1
        [HttpGet]
        public async Task<IActionResult> MakeReservation(int restaurantId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }
            var vm = new ReservationViewModel
            {
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                NumberOfPeople = 1
            };
            return View(vm);
        }

        // POST: /Reservations/MakeReservation
        [HttpPost]
        public async Task<IActionResult> MakeReservation(ReservationViewModel vm)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid || vm.ReservationDate == null)
            {
                return View(vm);
            }
            var reservation = new Reservation
            {
                RestaurantId = vm.RestaurantId,
                UserId = userId.Value,
                ReservationDate = vm.ReservationDate.Value,
                NumberOfPeople = vm.NumberOfPeople
            };
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Redirect to new Details action
            return RedirectToAction("Details", new { id = reservation.Id });
        }

        // GET: /Reservations/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Restaurant)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: /Reservations/Cancel
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || reservation.UserId != userId)
            {
                return Unauthorized();
            }
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserReservations");
        }

        // GET: /Reservations/UserReservations
        public async Task<IActionResult> UserReservations()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var reservations = await _context.Reservations
                .Include(r => r.Restaurant)
                .Where(r => r.UserId == userId)
                .ToListAsync();
            return View(reservations);
        }
    }
}
