﻿using Exadel.OfficeBooking.Domain.OfficePlan;
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
            Guid mapTb1 = Guid.NewGuid();
            Guid mapTb2 = Guid.NewGuid();

            Guid tk = Guid.NewGuid();
            Guid mapTk = Guid.NewGuid();
            Guid mapTk1 = Guid.NewGuid();

            Guid min = Guid.NewGuid();
            Guid min2 = Guid.NewGuid();

            Guid mapMin3 = Guid.NewGuid();
            Guid mapMin4 = Guid.NewGuid();
            Guid mapMin5 = Guid.NewGuid();
            Guid mapMin6 = Guid.NewGuid();
            Guid mapMin21 = Guid.NewGuid();

            Guid ode = Guid.NewGuid();
            Guid mapOde = Guid.NewGuid();

            Office tbilisi = new Office()
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
                Country = "Belarus",
                City = "Minsk",
                IsFreeParkingAvailable = true,
                Address = "Naturalistov str # 3",
                Name = "N3"
            };

            Office minsk2 = new Office
            {
                Id = min2,
                Country = "Belarus",
                City = "Minsk",
                IsFreeParkingAvailable = false,
                Address = "Solnechnaya str # 59",
                Name = "S59"
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

            modelBuilder.Entity<Office>().HasData(tbilisi, tashkent, minsk, minsk2, odessa);

            modelBuilder.Entity<Map>().HasData(

                new
                {
                    FloorNumber = 1,
                    Id = mapTb,
                    IsKitchenPresent = true,
                    IsMeetingRoomPresent = true,

                    OfficeId = tb
                },
                new
                {
                    FloorNumber = 2,
                    Id = mapTb1,
                    IsKitchenPresent = false,
                    IsMeetingRoomPresent = true,

                    OfficeId = tb
                },
                new
                {
                    FloorNumber = 3,
                    Id = mapTb2,
                    IsKitchenPresent = true,
                    IsMeetingRoomPresent = false,

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
                    FloorNumber = 8,
                    Id = mapTk1,
                    IsKitchenPresent = false,
                    IsMeetingRoomPresent = false,

                    OfficeId = tk

                },
                new
                {
                    FloorNumber = 3,
                    Id = mapMin3,
                    IsKitchenPresent = true,
                    IsMeetingRoomPresent = true,

                    OfficeId = min
                },
                new
                {
                    FloorNumber = 4,
                    Id = mapMin4,
                    IsKitchenPresent = true,
                    IsMeetingRoomPresent = false,

                    OfficeId = min
                },
                new
                {
                    FloorNumber = 5,
                    Id = mapMin5,
                    IsKitchenPresent = false,
                    IsMeetingRoomPresent = false,

                    OfficeId = min
                },
                new
                {
                    FloorNumber = 6,
                    Id = mapMin6,
                    IsKitchenPresent = false,
                    IsMeetingRoomPresent = true,

                    OfficeId = min
                },
                new
                {
                    FloorNumber = 1,
                    Id = mapMin21,
                    IsKitchenPresent = false,
                    IsMeetingRoomPresent = true,

                    OfficeId = min2
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
                    PlaceNumber = 16,
                    IsBooked = false,
                    OfficeId = tk
                },
                new
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 17,
                    IsBooked = false,
                    OfficeId = tk
                },
                new
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 1,
                    IsBooked = false,
                    OfficeId = min
                },
                new
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 2,
                    IsBooked = false,
                    OfficeId = min
                },
                new
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 3,
                    IsBooked = false,
                    OfficeId = min

                },
                new
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 4,
                    IsBooked = false,
                    OfficeId = min
                },
                new
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = 5,
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
                    Name = "1",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapTb
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = true,
                    HasMonitor = true,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    Name = "2",
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
                    IsNextToWindow = false,
                    Name = "3",
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
                    Name = "77",
                    Type = WorkplaceTypes.Administrative,
                    MapId = mapTk
                },
        // Minsk office
                new
                {
                    HasHeadset = false,
                    HasKeyboard = false,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    Name = "301",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin3
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = false,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "302",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin3
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = true,
                    HasMonitor = true,
                    HasMouse = false,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "303",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin3
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = false,
                    HasMonitor = true,
                    HasMouse = false,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    Name = "304",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin3
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = false,
                    HasMonitor = true,
                    HasMouse = true,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "305",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin3
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = false,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "306",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin3
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = false,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "307",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin3
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = true,
                    HasMonitor = true,
                    HasMouse = true,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    Name = "333",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin3
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = false,
                    HasMonitor = true,
                    HasMouse = false,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "401",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin4
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = true,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "402",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin4
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
                    Name = "403",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin4
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = true,
                    HasMonitor = true,
                    HasMouse = true,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    Name = "404",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin4
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = false,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "405",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin4
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = true,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "501",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin5
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = true,
                    HasMonitor = true,
                    HasMouse = true,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    Name = "502",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin5
                },
                               new
                {
                    HasHeadset = false,
                    HasKeyboard = true,
                    HasMonitor = true,
                    HasMouse = true,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "601",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin6
                },
                new
                {
                    HasHeadset = false,
                    HasKeyboard = false,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = false,
                    Name = "602",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin6
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = true,
                    HasMonitor = false,
                    HasMouse = false,
                    HasPC = true,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    Name = "603",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin6
                },
                new
                {
                    HasHeadset = true,
                    HasKeyboard = false,
                    HasMonitor = false,
                    HasMouse = true,
                    HasPC = false,
                    Id = Guid.NewGuid(),
                    IsNextToWindow = true,
                    Name = "101",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapMin21
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
                    Name = "SU",
                    Type = WorkplaceTypes.Regular,
                    MapId = mapOde
                }
             );

            modelBuilder.Entity<User>().HasData(
                  new User
                  {
                      Id = Guid.NewGuid(),
                      FirstName = "Anvarjon",
                      LastName = "Majidov",
                      Email = "majidovanvar30.11.2002@gmail.com",
                      EmploymentStart = DateTime.Now,
                      TelegramId = 534430877,
                      Role = UserRole.Manager

                  },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Khamza",
                    LastName = "Davletov",
                    Email = "kdavletov@exadel.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 194740354,
                    Role = UserRole.Manager
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Anvarkhon",
                    LastName = "Khamzaev",
                    Email = "anvarkhon.khamzaev@gmail.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 112046934,
                    Role = UserRole.Manager
                },

                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Polina",
                    LastName = "",
                    Email = "apfpo2001@gmail.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 635524939,
                    Role = UserRole.Admin
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Vasili",
                    LastName = "",
                    Email = "malkou.vasili@gmail.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 957365793,
                    Role = UserRole.Admin
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Andrei",
                    LastName = "Harahliad",
                    Email = "asgoreglyad@gmail.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 1651658270,
                    Role = UserRole.Manager
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Sandro",
                    LastName = "Sherazadishvili",
                    Email = "sherashera777@gmail.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 5046701077,
                    Role = UserRole.Manager
                }
                );
        }
    }
}
