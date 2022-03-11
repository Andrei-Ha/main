﻿using System;

namespace Exadel.OfficeBooking.Api.DTO.WorkplaceDto
{
    public class WorkplaceFilterDto
    {
        public string? Name { get; set; }

        public Guid? MapId { get; set; }

        public WorkplaceTypesDto? Type { get; set; }

        public bool? IsBooked { get; set; }

        public bool? IsNextToWindow { get; set; }

        public bool? IsVIP { get; set; }

        public bool? HasPC { get; set; }

        public bool? HasMonitor { get; set; }

        public bool? HasKeyboard { get; set; }

        public bool? HasMouse { get; set; }

        public bool? HasHeadset { get; set; }
    }
}
