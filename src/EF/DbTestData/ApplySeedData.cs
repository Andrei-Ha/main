using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.Domain.Person;
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
            Guid mapTb = Guid.NewGuid();

            Guid tk = Guid.NewGuid();
            Guid mapTk = Guid.NewGuid();

            Guid min = Guid.NewGuid();
            Guid mapMin = Guid.NewGuid();

            Guid ode = Guid.NewGuid();
            Guid mapOde = Guid.NewGuid();

            Office tbilisi = new Office ()
            {

                Country = "Georgia",
                City = "Tbilisi",
                IsFreeParkingAvailable = false,
                Id = tb,
                Address = "Ilo Mosashvili str #24",
                Name = "IM24"
            };

            Office tashkent = new Office
            {
                Id = tk,
                City = "Tashkent",
                Country = "Uzbekistan",
                Address = "Mirzo Ulugbek Avenue # 73",
                IsFreeParkingAvailable = true,
                Name = "MU73",
            };

            Office minsk = new Office
            {
                Id = min,
                Country = "Belrus",
                City = "Minsk",
                IsFreeParkingAvailable = true,
                Address = "Naturalistov str # 3",
                Name = "N3"
            };

            Office odessa = new Office
            {
                Id = ode,
                Country = "Ukraine",
                City = "Odessa",
                IsFreeParkingAvailable = true,
                Address = "Yaroslaviv Val # 15",
                Name = "YV15"
            };

            modelBuilder.Entity<Office>().HasData(tbilisi, tashkent, minsk, odessa);
            
            modelBuilder.Entity<Map>().HasData(

                new
                {
                    FloorNumber = 12,
                    Id = mapTb,
                    IsKitchenPresent = true,
                    IsMeetingRoomPresent = true,

                    OfficeId = tb
                },
                new
                {
                    FloorNumber = 7,
                    Id = mapTk,
                    IsKitchenPresent = true,
                    IsMeetingRoomPresent = true,

                    OfficeId = tk
                },
                new
                {
                    FloorNumber = 5,
                    Id = mapMin,
                    IsKitchenPresent = true,
                    IsMeetingRoomPresent = true,

                    OfficeId = min
                },
                new
                {
                    FloorNumber = 9,
                    Id = mapOde,
                    IsKitchenPresent = true,
                    IsMeetingRoomPresent = true,

                    OfficeId = ode
                }
                );




            modelBuilder.Entity<ParkingPlace>().HasData(

                new
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 15,
                    IsBooked = false,
                    OfficeId = tk
                },
                new
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 48,
                    IsBooked = false,
                    OfficeId = min
                },
                new
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 90,
                    IsBooked = false,
                    OfficeId = ode
                }
                );

            modelBuilder.Entity<Workplace>().HasData(

                new
                {
                    HasHeadset = true,
                    HasKeyboard = true,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    IsBooked = false,
                    Name = "Super Workplace",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapTb
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = true,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    IsBooked = false,
                    Name = "Best Workplace",
                    Type = WorkplaceTypes.Administrative,
                    MapId = mapTk
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = true,
                    HasMonitor = true,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    IsBooked = true,
                    Name = "Super Workplace",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = true,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    IsBooked = false,
                    Name = "Slava Ukraina",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapOde
                }
             );

            modelBuilder.Entity<User>().HasData(
                  new User
                  {
                      Id = Guid.NewGuid(),
                      FirstName = "Anvar",
                      LastName = "Majidov",
                      Email = "AnvarMajidov@fake.com",
                      EmploymentStart = DateTime.Now,
                      TelegramId = 112233

                  },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Khamza",
                    LastName = "Davletov",
                    Email = "KhamzaDavletov@fake.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 223344
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Anvarkhon",
                    LastName = "Khamzaev",
                    Email = "AnvarkhonKhamzaev@fake.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 445566
                },

                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Palina",
                    LastName = "Fomchanka",
                    Email = "PalinaFomchanka@fake.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 667788
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Vasili",
                    LastName = "Molkov",
                    Email = "VasiliMolkov@fake.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 778899
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Andrei",
                    LastName = "Harahliad",
                    Email = "AndreiHarahliad@fake.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 889900
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Sandro",
                    LastName = "Sherazadishvili",
                    Email = "SandroSherazadishvili@fake.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 990011
                }
                );
        }
    }
}
