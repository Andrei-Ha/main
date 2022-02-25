namespace Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto
{
    public class OfficeSetDto
    {
        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsFreeParkingAvailable { get; set; }
    }
}
