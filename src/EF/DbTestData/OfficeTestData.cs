using Exadel.OfficeBooking.Domain.OfficePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

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
                    Country = "Uzbekistan",
                    City = "Tashkent",
                    Address = "Mirzo Ulugbek Avenue # 73",
                    Name = "MU73",
                    IsFreeParkingAvailable = true,
                    //Maps = new List<Map>
                    //{
                    //    new Map
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        FloorNumber = 1,
                    //        IsKitchenPresent = true,
                    //        IsMeetingRoomPresent = true,
                    //        Workspaces = new List<Workplace>
                    //        {
                    //            new Workplace
                    //            {
                    //                Name = "11",
                    //                Type = WorkplaceTypes.Regular,
                    //                IsBooked = true,
                    //                IsNextToWindow = true,
                    //                HasPC = true,
                    //                HasMonitor = true,
                    //                HasKeyboard = true,
                    //                HasMouse = true,
                    //                HasHeadset = true
                    //            }
                    //        }
                            
                    //    }
                    //}
                },
                new Office
                {
                    Id = Guid.NewGuid(),
                    Country = "Belrus",
                    City = "Minsk",
                    Address = "Naturalistov str # 3",
                    Name = "N3",
                    IsFreeParkingAvailable = true,
                },
                new Office
                {
                    Id = Guid.NewGuid(),
                    Country = "Georgia",
                    City = "Tbilisi",
                    Address = "Ilo Mosashvili str #24",
                    Name = "IM24",
                    IsFreeParkingAvailable = false,
                },
                new Office
                {
                    Id = Guid.NewGuid(),
                    Country = "Lithuania",
                    City = "Klaipėda",
                    Address = "Danes str # 6-401",
                    Name = "D6-401",
                    IsFreeParkingAvailable = true,
                },
                new Office
                {
                    Id = Guid.NewGuid(),
                    Country = "Ukraine",
                    City = "Odessa",
                    Address = "Yaroslaviv Val # 15",
                    Name = "YV15",
                    IsFreeParkingAvailable = false,
                }); 
        }
    }
}
