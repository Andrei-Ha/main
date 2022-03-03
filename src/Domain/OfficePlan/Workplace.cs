using Exadel.OfficeBooking.Domain.Bookings;
using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Workplace : BaseModel
    {
        public string Name { get; set; } = string.Empty;

        public WorkplaceTypes Type { get; set; }

        public bool IsBooked { get; set; }

        public bool IsNextToWindow { get; set; }

        public bool HasPC { get; set; }

        public bool HasMonitor { get; set; }

        public bool HasKeyboard { get; set; }

        public bool HasMouse { get; set; }

        public bool HasHeadset { get; set; }

        public Guid MapId { get; set; }

        public virtual Map Map { get; set; } = new();

        public List<Booking>? Bookings { get; set; }
    }

    public enum WorkplaceTypes
    {
        Regular,
        Administrative,
        Non_bookable
    }
}
