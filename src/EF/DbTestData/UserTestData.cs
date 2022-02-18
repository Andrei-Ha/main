using Exadel.OfficeBooking.Domain.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

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
                    ChatId = 112233,
                    FirstName ="Anvar",
                    LastName ="Majidov",
                    Email = "AnvarMajidov@fake.com",
                    Role = UserRole.CommonUser,
                    EmploymentStart = DateTime.Now
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    ChatId = 223344,
                    FirstName = "Khamza",
                    LastName = "Davletov",
                    Email = "KhamzaDavletov@fake.com",
                    Role = UserRole.CommonUser,
                    EmploymentStart = DateTime.Now
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    ChatId = 445566,
                    FirstName = "Anvarkhon",
                    LastName = "Khamzaev",
                    Email = "AnvarkhonKhamzaev@fake.com",
                    Role = UserRole.CommonUser,
                    EmploymentStart = DateTime.Now
                },

                new User
                {
                    Id = Guid.NewGuid(),
                    ChatId = 667788,
                    FirstName = "Palina",
                    LastName = "Fomchanka",
                    Email = "PalinaFomchanka@fake.com",
                    Role = UserRole.CommonUser,
                    EmploymentStart = DateTime.Now
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    ChatId = 778899,
                    FirstName ="Vasili",
                    LastName ="Molkov",
                    Email = "VasiliMolkov@fake.com",
                    Role = UserRole.CommonUser,
                    EmploymentStart = DateTime.Now
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    ChatId = 889900,
                    FirstName = "Andrei",
                    LastName ="Harahliad",
                    Email = "AndreiHarahliad@fake.com",
                    Role = UserRole.CommonUser,
                    EmploymentStart = DateTime.Now
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    ChatId = 990011,
                    FirstName = "Sandro",
                    LastName = "Sherazadishvili",
                    Email = "SandroSherazadishvili@fake.com",
                    Role = UserRole.CommonUser,
                    EmploymentStart = DateTime.Now
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    ChatId = 123465,
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    Email = "iivanov@gmail.com",
                    Role = UserRole.CommonUser,
                    EmploymentStart = DateTime.Now
                });
        }
    }
}
