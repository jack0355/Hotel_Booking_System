using HotelBookingSystem.API.Data;
using HotelBookingSystem.API.DTOs.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Security.Claims;

namespace HotelBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BookingsController(AppDbContext db)
        {
            _db = db;
        }


        [HttpPost]
        public async Task<IActionResult>Create(CreateBookingRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();

            var userId = int.Parse(userIdClaim);


            if (request.CheckOut <= request.CheckIn)
                return BadRequest("Check-out Must be after Check-in");

            var room = await _db.Rooms.FindAsync(request.RoomId);
            if (room == null || !room.IsActive)
                return NotFound("Room not Found Or unavailable");


            // --- CONCURRENCY CHECK --- 
            //  HERE I NEED TO  check if room is already booked for overlapping dates
            // this is the double-booking prevention logic
            var IsBooked = await _db.Bookings.AnyAsync(b=>
            b.RoomId == request.RoomId &&
            b.Status != "Cancelled" && 
            b.CheckIn < request.CheckOut && 
            b.CheckOut > request.CheckIn);

            if (IsBooked)
                return Conflict("Room is already Booked for the selected Dates");

            var nights = (request.CheckOut - request.CheckIn).Days;
            var totalPrice = nights * room.PricePerNight;

            var user = await _db.Users.FindAsync(userId);
            var guest = await _db.Guests.FirstOrDefaultAsync(g => g.Email == user.username);

            if(guest == null)
            {
                guest = new Guest
                {
                    FullName = user.username,
                Email = user.username,
                Phone=  "N/A"

                };
               _db.Guests.Add(guest);
               await _db.SaveChangesAsync();

            }

            var booking = new Booking
            {
                RoomId = request.RoomId,
                GuestID = guest.Id,
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                TotalPrice = totalPrice,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };


            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();


            return Ok(new BookingReponse
            {

                Id = booking.Id,
                RoomNumber = room.Number,
                RoomType = room.Type,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status
            });
        }





        [HttpGet("my")]
        public async Task<IActionResult>GetMyBookings()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();

            var userId = int.Parse(userIdClaim);

        var user = await _db.Users.FindAsync(userId);
            var guest = await _db.Guests.FirstOrDefaultAsync(g => g.Email
             == user.username);

            if (guest == null) return Ok(new List<BookingReponse>());


            var booking = await _db.Bookings.Where(b => b.GuestID == guest.Id).
                Include(b => b.Room)
                .Select(b => new BookingReponse
                {
                    Id = b.Id,
                    RoomNumber = b.Room.Number,
                    RoomType = b.Room.Type,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status
                }).ToListAsync();
            return Ok(booking);

        }





        [HttpPut("{id}/cancel")]
        public async Task<IActionResult>Cancel(int id)
        {
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaims == null) return Unauthorized();
            var userId = int.Parse(userIdClaims);

            var user = await _db.Users.FindAsync(userId);
            var guest = await _db.Guests.FirstOrDefaultAsync(g => g.Email == user.username);

            if (guest == null) return NotFound();

            var booking = await _db.Bookings
                .FirstOrDefaultAsync(b=>b.Id == id && b.GuestID == guest.Id);

            if(booking == null) return NotFound("Booking Not Found");
            if (booking.Status == "Cancelled") return BadRequest("Booking already  Cancelled");

            booking.Status = "Cancelled";
            await _db.SaveChangesAsync();
            return Ok("Booking Cancelled Successfully");
        }





        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>GetAll()
        {
            var bookings = await _db.Bookings
                .Include(b => b.RoomId)
                .Include(b => b.Guest)
                .Select(b => new BookingReponse
                {
                    Id = b.Id,
                    RoomNumber = b.Room.Number,
                    RoomType = b.Room.Type,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status
                }).ToListAsync();

            return Ok(bookings);
        }




        [HttpPut("{id}/confirm")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>Confirm(int id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null) return NotFound("booking not found ");
            if (booking.Status != "Pending")
                return BadRequest($"Cannot Confirm a booking with status : {booking.Status}");

            booking.Status = "Confirmed";
            await _db.SaveChangesAsync();
            return Ok("Booking Confirmed Succesfully");
        }



        [HttpPut("{id}/admin-cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AdminCancel(int id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null) return NotFound("Booking Not Found");
            if (booking.Status == "Cancelled")
                return BadRequest("Already Cancelled");


            booking.Status = "Cancelled";
            await _db.SaveChangesAsync();
            return Ok("Booking Cancelled By Admin");
        }
    }
}
