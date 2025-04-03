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
        public Restaurant Restaurant { get; set; } = null!;
        
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        
        [Required]
        public DateTime ReservationDate { get; set; }
        
        [Required]
        public int NumberOfPeople { get; set; }
        
        [Required]
        public string Address { get; set; } = string.Empty;
        
        [Required]
        public string TableType { get; set; } = string.Empty;
        
        // New property for reservation status
        [Required]
        public string Status { get; set; } = "Pending";
    }
}
