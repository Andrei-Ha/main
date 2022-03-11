using System;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Api.DTO.BookingDto;
using Exadel.OfficeBooking.Api.DTO.WorkplaceDto;

namespace Exadel.OfficeBooking.Api.Interfaces
{
    public interface IBookingService
    {
        Task<ServiceResponse<GetBookingDto[]>> GetAllBookings();
        Task<ServiceResponse<GetBookingDto>> GetBookingById(Guid id);
        Task<WorkplaceGetDto?> GetFirstFreeWorkplaceInOfficeForBooking(GetFirstFreeWorkplaceForBookingDto bookingDto);
        Task<WorkplaceGetDto?> GetFirstFreeWorkplaceInOfficeForRecuringBooking(GetFirstFreeWorkplaceForRecuringBookingDto bookingDto);

        Task<ServiceResponse<GetOneDayBookingDto>> CreateBooking(AddBookingDto bookingDto);

        Task<ServiceResponse<GetOneDayBookingDto>> UpdateBooking(UpdateBookingDto bookingDto);

        Task<ServiceResponse<GetRecurringBookingDto>> CreateRecurringBooking(AddRecurringBookingDto bookingDto);
        
        Task<ServiceResponse<GetBookingDto>> UpdateRecurringBooking(UpdateRecurringBookingDto bookingDto);

        Task<ServiceResponse<GetBookingDto[]>> DeleteBooking(Guid id);
    }
}
