using Exadel.OfficeBooking.Domain.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.EF.DbTestData
{
    public class UserTestData : IEntityTypeConfiguration<User>
    {
     
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(

                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName ="Anvar",
                    LastName ="Majidov",
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
                    FirstName ="Vasili",
                    LastName ="Molkov",
                    Email = "VasiliMolkov@fake.com",
                    EmploymentStart = DateTime.Now,
                    TelegramId = 778899
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Andrei",
                    LastName ="Harahliad",
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
