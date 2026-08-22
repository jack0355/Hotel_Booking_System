using System;
using Xunit;
using HotelBookingSystem.API.Services;


public class BookingTests
{
    private readonly BookingService _bookingService = new();


    [Fact]
    public void CalculateTotaPrice_Should_Multiply_Nights_By_Rates()
    {
        var checkIn = DateTime.UtcNow;
        var checkOut = checkIn.AddDays(3);


        var rate = 100m;
        var expected = 300m;

        var acutal = _bookingService.CalculateTotalPrice(checkIn, checkOut, rate);


        Assert.Equal(expected, acutal);
    }


    [Fact]
    public void CalculateTotalPrice_WithDifferentRate_Should_BeCorrect()
    {
        var checkIn = DateTime.UtcNow;
        var checKOut = checkIn.AddDays(5);

        var rate = 150m;
        var expected = 750;

        var actual = _bookingService.CalculateTotalPrice(checkIn, checKOut, rate);

        Assert.Equal(expected, actual);
    }





    [Fact]
    public void CaculateTotalPrice_WithZeroNights_should_BeZero()
    {
        var checkIn = DateTime.UtcNow;
        var checkOut = checkIn;
        var rate = 100m;
        var expected = 0m;

        var actual = _bookingService.CalculateTotalPrice(checkIn, checkOut, rate);

        Assert.Equal(expected, actual);
    }

    [Fact] 
    
    public void CalculateTotaPrice_WithNegativeNights_Should_Zero()
    {
        var checkIn = DateTime.UtcNow.AddDays(3);
        var checkOut = DateTime.UtcNow;

        var rate = 100m;
        var expected = 0m;

        var actual = _bookingService.CalculateTotalPrice(checkIn, checkOut, rate);
        Assert.Equal(expected, actual);
    }
}

