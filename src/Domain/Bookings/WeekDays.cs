﻿using System;

namespace Exadel.OfficeBooking.Domain.Bookings
{
    [Flags]
    public enum WeekDays
    {
        None = 0,
        Sunday = 1,
        Monday = 1 << 1,
        Tuesday = 1 << 2,
        Wednesday = 1 << 3,
        Thursday = 1 << 4,
        Friday = 1 << 5,
        Saturday = 1 << 6
    }
}
