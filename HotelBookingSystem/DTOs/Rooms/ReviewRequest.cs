namespace HotelBookingSystem.API.DTOs.Rooms
{
    public class ReviewRequest
    {
        public int Rating {  get; set; }
        public string Comment { set; get; } = string.Empty;


    }
}
