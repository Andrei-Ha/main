namespace Exadel.OfficeBooking.Api.DTO.OfficeDto
{
    public class OfficeSetDto 
    {
        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Adress { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsCityCenter { get; set; }

        public bool IsParkingAvailable { get; set; }
    }
} 
