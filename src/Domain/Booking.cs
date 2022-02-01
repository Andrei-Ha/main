namespace Exadel.OfficeBooking.Domain;

public class Booking
{
    public int Id { get; set; }
    //name of the workspace that user wants to book
    public string WorkspaceName { get; set; }
    //booking of office must be approved by Admin, otherwise by default it is false
    public bool IsApproved { get; set; } = false;
    //WaitingList is used to store users who want to book this Workspace
    //public List<User> WaitingList { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
