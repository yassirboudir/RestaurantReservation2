using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
        public string Address { get; set; }
        public string TableType { get; set; }
        // NEW FIELD: tracks reservation status (e.g., Pending, Accepted, Rejected)
        public string Status { get; set; } = "Pending";
    }
}
