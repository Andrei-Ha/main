using Exadel.OfficeBooking.Domain.Bookings;
using Exadel.OfficeBooking.Domain.OfficePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.TelegramApi.DTO.ParkingPlaceDto
{
    public class ParkingPlaceSetDto
    {
        public bool IsBooked { get; set; }

        public int PlaceNumber { get; set; }
        public Guid OfficeId { get; set; }

        public virtual Office Office { get; set; } = new();

        public Booking? Booking { get; set; }
    }
}
