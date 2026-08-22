using HotelBookingSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace HotelBookingSystem.API.Services
{
    public class BookingExpiryService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        
        public BookingExpiryService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // wait 30 seconds on startup to give SQL Server time to initialize
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CancelExpiredBookings();
                }
                catch (Exception ex)
                {
                    // log the error but don't crash — just wait and retry
                    Console.WriteLine($"BookingExpiryService error: {ex.Message}");
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task CancelExpiredBookings()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


            var expiredBookings = await db.Bookings
                .Where(b => b.Status == "Pending" && b.CreatedAt < DateTime.UtcNow.AddMinutes(-15))
                .ToListAsync();


            foreach(var booking in expiredBookings)
            {
                booking.Status = "Cancelled";
            }

            if (expiredBookings.Any())
                await db.SaveChangesAsync();
        }

    }
}
