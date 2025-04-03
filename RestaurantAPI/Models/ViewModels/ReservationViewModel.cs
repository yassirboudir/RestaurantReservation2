using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models.ViewModels
{
    public class ReservationViewModel
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        [Required]
        public DateTime? ReservationDate { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
    }
}
