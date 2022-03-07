using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.DTO.OfficeDto
{
    public class ParkingPlaceGetDto : ParkingPlaceSetDto
    {
        public Guid Id { get; set; }
    }
}
