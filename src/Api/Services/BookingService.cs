using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Api.DTO.BookingDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Exadel.OfficeBooking.Domain.Bookings;
using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.Domain.Person;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Exadel.OfficeBooking.Api.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;

    public BookingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<GetBookingDto[]>> GetAllBookings()
    {
        ServiceResponse<GetBookingDto[]> response = new()
        {
            Data = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Workplace)
                .AsNoTracking()
                .Select(b => b.Adapt<GetBookingDto>())
                .ToArrayAsync()
        };

        return response;
    }

    public async Task<ServiceResponse<GetBookingDto>> GetBookingById(Guid id)
    {
        Booking? booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Workplace)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null) return NotFoundResponse<GetBookingDto>("Requested booking doesn’t exist");

        ServiceResponse<GetBookingDto> response = new()
        {
            Data = booking.Adapt<GetBookingDto>()
        };
        return response;
    }
    
    public async Task<ServiceResponse<GetOneDayBookingDto>> CreateBooking(AddBookingDto bookingDto)
    {
        ServiceResponse<GetOneDayBookingDto> response = new();

        Workplace? workplace = await _context.Workplaces
            .Include(w => w.Bookings)
            .FirstOrDefaultAsync(w => w.Id == bookingDto.WorkplaceId);
        User? user = await _context.Users
            .Include(u => u.Vacations)
            .FirstOrDefaultAsync(u => u.Id == bookingDto.UserId);
        
        //check if StartDate is not in the period of vacation days of user
        if(HasOneDayBookingVacationConflict(user, bookingDto.StartDate))
            return ConflictResponse<GetOneDayBookingDto>("Cannot select date on vacation days.");

        //check if workplace is available at the StartDate
        if(!IsWorkplaceAvailableForOneDayBooking(workplace, bookingDto.StartDate))
            return ConflictResponse<GetOneDayBookingDto>("The selected workplace has been booked by another user");
        
        //workplace is not booked, create newBooking
        Booking newBooking = new Booking
        {
            Id = new Guid(),
            User = user,
            Workplace = workplace,
            StartDate = bookingDto.StartDate
        };
        
        await _context.Bookings.AddAsync(newBooking);
        await _context.SaveChangesAsync();

        response.StatusCode = 201;
        var responseBooking = bookingDto.Adapt<GetOneDayBookingDto>();
        responseBooking.Id = newBooking.Id;
        response.Data = responseBooking;
        
        return response;
    }

    public async Task<ServiceResponse<GetOneDayBookingDto>> UpdateBooking(UpdateBookingDto bookingDto)
    {
        ServiceResponse<GetOneDayBookingDto> response = new();
        
        Booking? booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Workplace)
            .FirstOrDefaultAsync(b => b.Id == bookingDto.Id);
        if (booking == null) return NotFoundResponse<GetOneDayBookingDto>("Requested booking doesn’t exist");

        Workplace? workplace = await _context.Workplaces
            .Include(w => w.Bookings)
            .FirstOrDefaultAsync(w => w.Id == bookingDto.WorkplaceId);
        User? user = await _context.Users
            .Include(u => u.Vacations)
            .FirstOrDefaultAsync(u => u == booking.User);
        
        //check vacation and workplace conflicts
        if(HasOneDayBookingVacationConflict(user, bookingDto.StartDate))
            return ConflictResponse<GetOneDayBookingDto>("Cannot select date on vacation days.");
        
        if(!IsWorkplaceAvailableForOneDayBooking(workplace, bookingDto.StartDate))
            return ConflictResponse<GetOneDayBookingDto>("The selected workplace has been booked by another user");
        
        //update properties and save changes
        booking.StartDate = bookingDto.StartDate;
        booking.Workplace = workplace;

        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();

        response.Data = booking.Adapt<GetOneDayBookingDto>();
        return response;
    }

    public async Task<ServiceResponse<GetRecurringBookingDto>> CreateRecurringBooking(AddRecurringBookingDto bookingDto)
    {
        ServiceResponse<GetRecurringBookingDto> response = new();

        Workplace? workplace = await _context.Workplaces
            .Include(w => w.Bookings)
            .FirstOrDefaultAsync(w => w.Id == bookingDto.WorkplaceId);
        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == bookingDto.UserId);

        if (bookingDto.EndDate <= bookingDto.StartDate)
            return ConflictResponse<GetRecurringBookingDto>("Verify end date");

        List<DateTime> recurringDates = GetRecurringBookingDates(bookingDto.Adapt<RecurrencePattern>());

        //check if workplace is not booked in the period of vacation days of user
        if(await HasRecurringBookingVacationConflict(user, recurringDates))
            return ConflictResponse<GetRecurringBookingDto>("Cannot select date on vacation days.");
        
        //check if workplace is available at given dates
        if (!IsWorkplaceAvailableForRecurringBooking(workplace, recurringDates))
            return ConflictResponse<GetRecurringBookingDto>("The selected workplace has been booked by another user");

        //Add new recurring booking
        Booking newBooking = bookingDto.Adapt<Booking>();
        newBooking.Id = new Guid();
        newBooking.User = user;
        newBooking.Workplace = workplace;
        newBooking.IsRecurring = true;
        
        await _context.Bookings.AddAsync(newBooking);
        await _context.SaveChangesAsync();

        response.Data = newBooking.Adapt<GetRecurringBookingDto>();
        response.StatusCode = 209;
        return response;
    }

    public async Task<ServiceResponse<GetBookingDto>> UpdateRecurringBooking(UpdateRecurringBookingDto bookingDto)
    {
        ServiceResponse<GetBookingDto> response = new();
        
        Booking? booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingDto.Id);
        if (booking == null) return NotFoundResponse<GetBookingDto>("Requested booking doesn’t exist");

        Workplace? workplace = await _context.Workplaces
            .Include(w => w.Bookings)
            .FirstOrDefaultAsync(w => w == booking.Workplace);
        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u == booking.User);
        
        List<DateTime> recurringDates = GetRecurringBookingDates(bookingDto.Adapt<RecurrencePattern>());

        //check vacation and workplace conflicts
        if(await HasRecurringBookingVacationConflict(user, recurringDates))
            return ConflictResponse<GetBookingDto>("Cannot select date on vacation days.");
        
        if(!IsWorkplaceAvailableForRecurringBooking(workplace, recurringDates))
            return ConflictResponse<GetBookingDto>("The selected workplace has been booked by another user");
        
        //update properties and save changes
        booking = bookingDto.Adapt<Booking>();
        booking.Workplace = workplace;
        booking.User = user;

        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();

        response.Data = booking.Adapt<GetBookingDto>();
        return response;
    }

    public async Task<ServiceResponse<GetBookingDto[]>> DeleteBooking(Guid id)
    {
        Booking? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null) return NotFoundResponse<GetBookingDto[]>("Requested booking doesn’t exist");

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        return await GetAllBookings();
    }

    private async Task<bool> HasRecurringBookingVacationConflict(User user, List<DateTime> recurringDates)
    {
        List<DateTime> vacationDates = await GetVacationDates(user);
        return recurringDates.Intersect(vacationDates).Any();
    }

    private bool IsWorkplaceAvailableForRecurringBooking(Workplace workplace, List<DateTime> recurringDates)
    {
        foreach (var booking in workplace.Bookings)
        {
            if (booking.IsRecurring)
            {
                List<DateTime> curRecurringDates = GetRecurringBookingDates(booking.Adapt<RecurrencePattern>());
                if (recurringDates.Intersect(curRecurringDates).Any()) return false;
            }
            else
            {
                if (recurringDates.BinarySearch(booking.StartDate) >= 0) return false;
            }
        }

        return true;
    }
    
    private bool HasOneDayBookingVacationConflict(User user, DateTime bookingDate)
    {
        foreach (var vacation in user.Vacations)
        {
            for (var curDate = vacation.VacationStart; curDate <= vacation.VacationEnd; curDate = curDate.AddDays(1))
                if (curDate == bookingDate) return true;
        }
        return false;
    }

    private bool IsWorkplaceAvailableForOneDayBooking(Workplace workplace, DateTime bookingDate)
    {
        foreach (var booking in workplace.Bookings)
        {
            if (booking.IsRecurring) {
                List<DateTime> curRecurringDates = GetRecurringBookingDates(booking.Adapt<RecurrencePattern>());
                if (curRecurringDates.BinarySearch(bookingDate) > 0) return false;
            }
            else {
                if (bookingDate == booking.StartDate) return false;
            }
        }

        return true;
    }

    private List<DateTime> GetRecurringBookingDates(RecurrencePattern booking)
    {
        List<DateTime> recurringDates = new();

        if (booking.Interval < 1) booking.Interval = 1;
        
        if (booking.EndDate != null)
        {
            var curDate = booking.StartDate;
            while (curDate <= booking.EndDate)
            {
                if (booking.Frequency == RecurringFrequency.Daily)
                {
                    recurringDates.Add(curDate);
                    curDate = curDate.AddDays(booking.Interval);
                }

                if (booking.Frequency == RecurringFrequency.Weekly)
                {
                    var curDayOfWeek = (int) curDate.DayOfWeek;
                    if (booking.RecurringWeekDays[curDayOfWeek] == '1')
                        recurringDates.Add(curDate);
                    curDate = curDate.AddDays(1);

                    //this is for interval to skip weeks if necessary
                    if (curDate.DayOfWeek == DayOfWeek.Monday && booking.Interval > 1)
                        curDate = curDate.AddDays(7 * (booking.Interval - 1));
                }

                if (booking.Frequency == RecurringFrequency.Monthly)
                {
                    recurringDates.Add(curDate);
                    curDate = curDate.AddMonths(booking.Interval);
                }

                if (booking.Frequency == RecurringFrequency.Yearly)
                {
                    recurringDates.Add(curDate);
                    curDate = curDate.AddYears(booking.Interval);
                }
            }
        }

        if (booking.Count != null)
        {
            var curDate = booking.StartDate;
            var initDayOfWeek = (int) curDate.DayOfWeek;

            //when count is weekly we have to add count*7 days per count
            var countTimes = 1;
            if (booking.Frequency == RecurringFrequency.Weekly)
                countTimes = 7;

            for (var i = 0; i < booking.Count * countTimes; i++)
            {
                if (booking.Frequency == RecurringFrequency.Daily)
                {
                    recurringDates.Add(curDate);
                    curDate = curDate.AddDays(booking.Interval);
                }

                if (booking.Frequency == RecurringFrequency.Weekly)
                {
                    var curDayOfWeek = (int) curDate.DayOfWeek;
                    if (booking.RecurringWeekDays[curDayOfWeek] == '1')
                        recurringDates.Add(curDate);
                    curDate = curDate.AddDays(1);

                    if (curDate.DayOfWeek == DayOfWeek.Monday && booking.Interval > 1)
                        curDate = curDate.AddDays(7 * (booking.Interval - 1));
                }

                if (booking.Frequency == RecurringFrequency.Monthly)
                {
                    recurringDates.Add(curDate);
                    curDate = curDate.AddMonths(booking.Interval);
                }

                if (booking.Frequency == RecurringFrequency.Yearly)
                {
                    recurringDates.Add(curDate);
                    curDate = curDate.AddYears(booking.Interval);
                }
            }
        }

        return recurringDates;
    }

    private async Task<List<DateTime>> GetVacationDates(User user)
    {
        List<DateTime> vacationDates = new();
        
        List<Vacation> vacations = await _context.Vacations
            .AsNoTracking()
            .Where(v => v.User == user)
            .ToListAsync();
        foreach (var vacation in vacations)
        {
            for (var current = vacation.VacationStart;
                 current <= vacation.VacationEnd;
                 current = current.AddDays(1))
            {
                vacationDates.Add(current);
            }
        }
        return vacationDates;
    }

    private ServiceResponse<T> ConflictResponse<T>(string message)
    {
        return new ServiceResponse<T>
        {
            Success = false,
            StatusCode = 409,
            Message = message
        };
    }
    private ServiceResponse<T> NotFoundResponse<T>(string message)
    {
        return new ServiceResponse<T>
        {
            Success = false,
            StatusCode = 404,
            Message = message
        };
    }
    
}
