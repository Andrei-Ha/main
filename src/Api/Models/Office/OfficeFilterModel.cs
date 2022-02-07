using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Models.Office
{
    public class OfficeFilterModel
    {

        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Adress { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool? IsCityCenter { get; set; }

        public bool? IsParkingAvailable { get; set; }
    }
}
