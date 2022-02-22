using Exadel.OfficeBooking.Domain.OfficePlan;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.EF.DbTestData
{
    public static class ApplySeedData
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            Guid tb = Guid.NewGuid();
            Guid map = Guid.NewGuid();

            Office tbilisi = new Office ()
            {

                Country = "Georgia",
                City = "Tbilisi",
                IsFreeParkingAvailable = false,
                Id = tb,
                Address = "Ilo Mosashvili str #24",
                Name = "IM24"
            };

            modelBuilder.Entity<Office>().HasData(tbilisi);

            modelBuilder.Entity<Map>().HasData(
            new 
            {
                FloorNumber = 12,
                Id = map,
                IsKitchenPresent = true,
                IsMeetingRoomPresent = true,  
                
                OfficeId = tb
            });

            modelBuilder.Entity<ParkingPlace>().HasData(

                new 
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 15,
                    IsBooked = false,
                    OfficeId = tb
                }
                );

            modelBuilder.Entity<Workplace>().HasData(

                new 
                {
                    HasHeadset = true,
                    HasKeyboard =true,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    IsBooked = false,
                    Name = "Super Workplace",
                    Type = WorkplaceTypes.Regular,
                    MapId = map
                }
                );
        }
    }
}
