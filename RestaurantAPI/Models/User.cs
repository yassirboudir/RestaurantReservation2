using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
