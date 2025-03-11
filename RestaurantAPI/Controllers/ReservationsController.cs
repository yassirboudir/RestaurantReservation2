using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Models;
using RestaurantAPI.Models.ViewModels; // <-- Make sure you have this
using System;
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

        // Show all restaurants with a "Make a Reserve" button
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var restaurants = await _context.Restaurants.ToListAsync();
            return View(restaurants);
        }

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

            return RedirectToAction("Confirmation", new { id = reservation.Id });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }
    }
}
