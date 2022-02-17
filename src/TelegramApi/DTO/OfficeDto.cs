﻿using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO
{
    public class OfficeDto
    {
        public Guid Id { get; set; }

        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsFreeParkingAvailable { get; set; }
    }
}
