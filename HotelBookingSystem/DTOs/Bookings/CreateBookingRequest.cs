namespace HotelBookingSystem.API.DTOs.Bookings
{
    public class CreateBookingRequest
    {
        public int RoomId { get; set; }

        public DateTime CheckIn {  get; set; }
        public DateTime CheckOut { get; set; }  


    }
}
