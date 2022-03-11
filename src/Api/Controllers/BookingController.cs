using System;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO.BookingDto;
using Exadel.OfficeBooking.Api.DTO.WorkplaceDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Exadel.OfficeBooking.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.OfficeBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[AllowAnonymous]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        var response = await _bookingService.GetAllBookings();
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(Guid id)
    {
        var response = await _bookingService.GetBookingById(id);
        if (response.StatusCode == 404)
            return NotFound(response);
        return Ok(response);
    }

    [HttpPost("get/firstfree")]
    public async Task<ActionResult<WorkplaceGetDto>> GetFirstFreeWorkplaceInOfficeForBooking(GetFirstFreeWorkplaceForBookingDto bookingDto)
    {
        var workplace = await _bookingService.GetFirstFreeWorkplaceInOfficeForBooking(bookingDto);

        if (workplace == null)
            return NotFound(new { message = "Requested workplace not found" });

        return Ok(workplace);
    }

    [HttpPost("get/recuringfirstfree")]
    public async Task<ActionResult<WorkplaceGetDto>> GetFirstFreeWorkplaceInOfficeForRecuringBooking(GetFirstFreeWorkplaceForRecuringBookingDto bookingDto)
    {
        var workplace = await _bookingService.GetFirstFreeWorkplaceInOfficeForRecuringBooking(bookingDto);

        if (workplace == null)
            return NotFound(new { message = "Requested workplace not found" });

        return Ok(workplace);
    }

    [HttpPost("add/one-day")]
    public async Task<IActionResult> CreateBooking(AddBookingDto bookingDto)
    {
        var response = await _bookingService.CreateBooking(bookingDto);
        if (response.StatusCode == 409)
            return Conflict(response);
        
        return Ok(response);
    }

    [HttpPost("add/recurring")]
    public async Task<IActionResult> AddRecurringBooking(AddRecurringBookingDto bookingDto)
    {
        var response = await _bookingService.CreateRecurringBooking(bookingDto);
        if (response.StatusCode == 409)
            return Conflict(response);
        
        return Ok(response);
    }

    [HttpPut("update/one-day")]
    public async Task<IActionResult> UpdateBooking(UpdateBookingDto bookingDto)
    {
        var response = await _bookingService.UpdateBooking(bookingDto);
        if (response.StatusCode == 409)
            return Conflict(response);
        if (response.StatusCode == 404)
            return NotFound(response);
        
        return Ok(response);
    }
    
    [HttpPut("update/recurring")]
    public async Task<IActionResult> UpdateRecurringBooking(UpdateRecurringBookingDto bookingDto)
    {
        var response = await _bookingService.UpdateRecurringBooking(bookingDto);
        if (response.StatusCode == 409)
            return Conflict(response);
        if (response.StatusCode == 404)
            return NotFound(response);
        return Ok(response);
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
        var response = await _bookingService.DeleteBooking(id);
        if (response.StatusCode == 404)
            return NotFound(response);
        return Ok(response);
    }
}
