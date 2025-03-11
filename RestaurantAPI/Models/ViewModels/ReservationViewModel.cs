namespace RestaurantAPI.Models.ViewModels
{
    public class ReservationViewModel
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? ReservationDate { get; set; }
        public int NumberOfPeople { get; set; }
    }
}
