using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO.officeDto;
using Exadel.OfficeBooking.Api.DTO.OfficeDto;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.OfficeBooking.Api.Interfaces

{
    public interface IOfficeService
    {
        Task<List<CreateOfficeDto>> GetOffices(OfficeFilterDto filterModel);

        Task<CreateOfficeDto> GetOfficeById(Guid id);

        void  DeleteOffice(Guid id);

        Task<Guid> SaveOffice(CreateOfficeDto office);
    }
}

