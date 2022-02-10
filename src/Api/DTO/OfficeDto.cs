
using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Domain.OfficePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.DTO

{
    public class OfficeDto 
    {
        public Guid Id { get; set; }
        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Adress { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsCityCenter { get; set; }

        public bool IsParkingAvailable { get; set; }

        //public static implicit operator OfficeDto (Office source)
        //{
        //    if (source == null) return null;
           
        //    return new OfficeDto
        //    {
                
        //        Adress = source.Adress,
        //        City = source.City,
        //        Country = source.Country,
        //        Id = source.Id,
        //        IsCityCenter = source.IsCityCenter,
        //        IsParkingAvailable = source.IsParkingAvailable,
        //        Name = source.Name

        //    };
        //}
    }
} 
