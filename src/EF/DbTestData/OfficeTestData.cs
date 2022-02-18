using Exadel.OfficeBooking.Domain.OfficePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.EF.DbTestData
{
    public class OfficeTestData : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasData(

                new Office
                {
                    Id = Guid.NewGuid(),
                    City = "Tashkent",
                    Country = "Uzbekistan",
                    Address = "Mirzo Ulugbek Avenue # 73",
                    IsFreeParkingAvailable = true,
                    Name = "MU73",
                },
                new Office
                {
                    Id = Guid.NewGuid(),
                    Country = "Belrus",
                    City = "Minsk",
                    IsFreeParkingAvailable = true,
                    Address = "Naturalistov str # 3",
                    Name = "N3"
                },
                new Office
                {
                    Id = Guid.NewGuid(),
                    Country = "Georgia",
                    City = "Tbilisi",
                    IsFreeParkingAvailable = false,
                    Address = "Ilo Mosashvili str #24",
                    Name = "IM24"
                },
                new Office
                {
                    Id = Guid.NewGuid(),
                    Country = "Lithuania",
                    City = "Klaipėda",
                    IsFreeParkingAvailable = true,
                    Address = "Danes str # 6-401",
                    Name = "D6-401"
                },
                new Office
                {
                    Id = Guid.NewGuid(),
                    Country = "Ukraine",
                    City = "Odessa",
                    IsFreeParkingAvailable = false,
                    Address = "Yaroslaviv Val # 15",
                    Name = "YV15"
                }
                ) ; 
        }
    }
}
