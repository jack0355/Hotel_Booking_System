namespace HotelBookingSystem.Blazor.Components.Pages.Admin.Rooms_Management
{
    public class RoomResponse
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
    }

    public class RoomRequest
    {
        public string Number { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
    }
}