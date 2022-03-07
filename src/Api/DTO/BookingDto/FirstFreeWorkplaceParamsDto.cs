using System;

namespace Exadel.OfficeBooking.Api.DTO.BookingDto
{
    public class FirstFreeWorkplaceParamsDto
    {
        public Guid OfficeId { get; set; }

        public DateTime BookingDate { get; set; }

        public BookingTypeEnum BookingType { get; set; }

    }
}
