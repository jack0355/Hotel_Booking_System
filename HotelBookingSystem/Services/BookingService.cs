using System;



namespace HotelBookingSystem.API.Services
{
    public class BookingService
    {
        public int CalculateNights(DateTime checkIn , DateTime checkOut)
        {
            return (checkOut - checkIn).Days;
        }

        public decimal CalculateTotalPrice(DateTime checkIn , DateTime checkOut , decimal pricepernight)
        {
            var nights = CalculateNights(checkIn , checkOut);
            return nights * pricepernight;
        }
    }
}
