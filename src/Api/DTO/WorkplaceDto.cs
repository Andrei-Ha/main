namespace Exadel.OfficeBooking.Api.DTO
{
    public class WorkplaceDto
    {
        public Guid Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public TypesDto Type { get; set; }

        public bool IsBooked { get; set; }

        public bool IsNextToWindow { get; set; }

        public bool HasPC { get; set; }

        public bool HasMonitor { get; set; }

        public bool HasKeyboard { get; set; }

        public bool HasMouse { get; set; }

        public bool HasHeadset { get; set; }
    }

    public enum TypesDto
    {
        Regular,
        Administrative,
        Non_bookable
    }
}
