using System;
using Xunit;


public class BookingTests
{
    [Fact]
    public void CheckOut_Must_Be_After_CheckIn()
    {
        var checkIn = new DateTime(2026, 8, 10);
        var checkOut = new DateTime(2026, 8, 9);

        bool isValid = checkOut > checkIn;

        Assert.False(isValid);
    }
}