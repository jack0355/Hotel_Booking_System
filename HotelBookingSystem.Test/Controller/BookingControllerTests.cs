using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelBookingSystem.API.Controllers;
using HotelBookingSystem.API.Data;
using HotelBookingSystem.API.DTOs.Bookings;
using HotelBookingSystem.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace HotelBookingSystem.Tests.Controller
{
    public class BookingControllerTests
    {
        [Fact]
        public async Task Create_WithValidRequest_Should_ReturnOk()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;


            using (var db = new AppDbContext(options))
            {
                var room = new Room
                {
                    Id = 1,
                    Number = "101",
                    Type = "Single",
                    PricePerNight = 50m,
                    IsActive = true,
                };
                db.Rooms.Add(room);

                var user = new User
                {
                    Id = 1,
                    username = "testuser",
                    Role = "Guest"
                };
                db.Users.Add(user);

                await db.SaveChangesAsync();

                var bookingService = new BookingService();


                var controller = new BookingsController(db, bookingService);


                var claims = new List<Claim>
                { 
                new Claim(ClaimTypes.NameIdentifier, "1")
                };
                var identity = new ClaimsIdentity(claims, "Test");
                var principal = new ClaimsPrincipal(identity);


                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = principal
                    }
                };

                var request = new CreateBookingRequest
                {
                    RoomId = 1,
                    CheckIn = DateTime.UtcNow.AddDays(1),
                    CheckOut = DateTime.UtcNow.AddDays(3)
                };

                var result = await controller.Create(request);

                var okResult = Assert.IsType<OkObjectResult>(result);

                var bookingResponse = Assert.IsType<BookingReponse>(okResult.Value);

                Assert.Equal(1, bookingResponse.Id);

                Assert.Equal("101", bookingResponse.RoomNumber);

                Assert.Equal(100m, bookingResponse.TotalPrice);
            }
        }


        [Fact]
        public async Task Create_WithInvalidDates_Should_ReturnBadRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var db = new AppDbContext(options))
            {
               
                var room = new Room
                {
                    Id = 1,
                    Number = "101",
                    Type = "Single",
                    PricePerNight = 50m,
                    IsActive = true
                };
                db.Rooms.Add(room);
                await db.SaveChangesAsync();

                var bookingService = new BookingService();
                var controller = new BookingsController(db, bookingService);

              
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "1")
                };
                var identity = new ClaimsIdentity(claims, "Test");
                var principal = new ClaimsPrincipal(identity);

                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = principal
                    }
                };

            
                var request = new CreateBookingRequest
                {
                    RoomId = 1,
                    CheckIn = DateTime.UtcNow.AddDays(3),
                    CheckOut = DateTime.UtcNow.AddDays(1) 
                };

               
                var result = await controller.Create(request);

               
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Check-out Must be after Check-in", badRequestResult.Value);
            }
        }


        [Fact]
        public async Task Create_WithOverlappingBooking_Should_ReturnConflict()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var db = new AppDbContext(options))
            {
              
                var room = new Room
                {
                    Id = 1,
                    Number = "101",
                    Type = "Single",
                    PricePerNight = 50m,
                    IsActive = true
                };
                db.Rooms.Add(room);

             
                var user = new User
                {
                    Id = 1,
                    username = "testuser",
                    Role = "Guest"
                };
                db.Users.Add(user);

            
                var guest = new Guest
                {
                    Id = 1,
                    FullName = "Test User",
                    Email = "testuser",
                    Phone = "N/A"
                };
                db.Guests.Add(guest);

             
                var existingBooking = new Booking
                {
                    RoomId = 1,
                    GuestID = 1,
                    CheckIn = DateTime.UtcNow.AddDays(1),
                    CheckOut = DateTime.UtcNow.AddDays(3), 
                    Status = "Confirmed",
                    TotalPrice = 100m,
                    CreatedAt = DateTime.UtcNow
                };
                db.Bookings.Add(existingBooking);
                await db.SaveChangesAsync();

                var bookingService = new BookingService();
                var controller = new BookingsController(db, bookingService);

             
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "1")
                };
                var identity = new ClaimsIdentity(claims, "Test");
                var principal = new ClaimsPrincipal(identity);

                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = principal
                    }
                };

                var request = new CreateBookingRequest
                {
                    RoomId = 1,
                    CheckIn = DateTime.UtcNow.AddDays(2), 
                    CheckOut = DateTime.UtcNow.AddDays(4) 
                };

               
                var result = await controller.Create(request);

                
                var conflictResult = Assert.IsType<ConflictObjectResult>(result);
                Assert.Equal("Room is already Booked for the selected Dates", conflictResult.Value);
            }
        }
    }
}
    


