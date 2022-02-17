using Exadel.OfficeBooking.Domain.OfficePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Exadel.OfficeBooking.EF.DbTestData
{
    public class WorkplaceTestData : IEntityTypeConfiguration<Workplace>
    {
        public void Configure(EntityTypeBuilder<Workplace> builder)
        {
            builder.HasData(

                new Workplace
                {
                    Id = Guid.NewGuid(),
                    Name = "11",
                    Type = WorkplaceTypes.Regular,

                },
                new Office
                {
                    Id = Guid.NewGuid(),
                    Country = "Ukraine",
                    City = "Odessa",
                    IsFreeParkingAvailable = false,
                    Address = "Yaroslaviv Val # 15",
                    Name = "YV15"
                });
        }
    }
}
