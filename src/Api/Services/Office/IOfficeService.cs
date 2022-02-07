using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.Models.Office;
using Exadel.OfficeBooking.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.OfficeBooking.Api.Services
{
    public interface IOfficeService
    {
        Task<List<OfficeViewModel>> GetOffices(OfficeFilterModel filterModel);

        Task<OfficeViewModel> GetOfficeById(Guid id);

        void  DeleteOffice(Guid id);

        Task<Guid> SaveOffice(OfficeViewModel office);
    }
}

