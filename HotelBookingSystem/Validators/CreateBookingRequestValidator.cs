using System;
using FluentValidation;
using FluentValidation.Validators;
using HotelBookingSystem.API.DTOs.Bookings;



namespace HotelBookingSystem.API.Validators
{

    public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
    { 
        public CreateBookingRequestValidator() 
        {
            RuleFor(x => x.RoomId).GreaterThan(0).WithMessage("Room ID must be a valid room");

            RuleFor(x => x.CheckIn).GreaterThan(DateTime.UtcNow).WithMessage("Check-in date must be in the future");

            RuleFor(x => x.CheckOut).GreaterThan(x => x.CheckIn).WithMessage("Check-out must be afer Check-in");

            RuleFor(x => (x.CheckOut - x.CheckIn).Days).GreaterThanOrEqualTo(1).WithMessage("Minmum Stay is 1 night . ");

                
        }
    }
}
