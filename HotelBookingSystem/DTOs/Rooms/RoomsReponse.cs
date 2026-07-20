namespace HotelBookingSystem.API.DTOs.Rooms
{
    public class RoomsReponse
    {
        public int Id {  get; set; }
        public string Number { get; set;}
        public string Type {  get; set; }
        public decimal PricePerNight {  get; set; }
        public int Capacity {  get; set; }

    }

}
