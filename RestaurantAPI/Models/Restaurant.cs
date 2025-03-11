using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        // Now this filename will be found in wwwroot/images
        public string ImageUrl { get; set; }
        
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<RestaurantImage> RestaurantImages { get; set; }
    }
}
