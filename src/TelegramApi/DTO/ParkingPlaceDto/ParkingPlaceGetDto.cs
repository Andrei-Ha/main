using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.TelegramApi.DTO.ParkingPlaceDto
{
    public class ParkingPlaceGetDto : ParkingPlaceSetDto
    {
        public Guid Id { get; set; }
    }
}
