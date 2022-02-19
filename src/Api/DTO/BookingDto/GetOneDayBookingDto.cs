using System;

namespace Exadel.OfficeBooking.Api.DTO.BookingDto;

public class GetOneDayBookingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid WorkplaceId { get; set; }
    public Guid? ParkingPlaceId { get; set; }

    public DateTime Date { get; set; }
}
