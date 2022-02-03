using Exadel.OfficeBooking.Domain.OfficePlan;

namespace Exadel.OfficeBooking.Domain;

public class Booking
{
    public Guid Id { get; set; }
    public WorkSpace WorkSpace { get; set; } = new();
    public User User { get; set; } = new();
    
    public string RecurringDays { get; set; } = string.Empty;
    public bool IsRecurringMonthly { get; set; }
    public bool IsRecurringYearly { get; set; }
    
    public bool IsApproved { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
