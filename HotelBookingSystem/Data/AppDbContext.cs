using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HotelBookingSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Guest> Guests {  get; set; }
        public DbSet<Booking> Bookings {  get; set; }
        public DbSet<User>Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().Property(r => r.PricePerNight)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Booking>().Property(b => b.TotalPrice).HasPrecision(10, 2);


        }
    }

   
}
