using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.OfficeBooking.Api.Interfaces

{
    public interface IOfficeService
    {
        Task<List<OfficeDto>> GetOffices(OfficeFilterDto filterModel);

        Task<OfficeDto?> GetOfficeById(Guid id);

        Task<Guid?>  DeleteOffice(Guid id);

        Task<Guid?> SaveOffice(OfficeDto office);

        Task<OfficeDto?> UpdateOffice(OfficeDto office);
    }
}

