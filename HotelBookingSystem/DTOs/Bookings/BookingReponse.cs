namespace HotelBookingSystem.API.DTOs.Bookings
{
    public class BookingReponse
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

        public DateTime? CheckedInAt {get; set; }
        public DateTime? CheckedOutAt {get;set; }
    }
}
