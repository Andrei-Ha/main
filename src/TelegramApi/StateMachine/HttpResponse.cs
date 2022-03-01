using System.Net;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public class HttpResponse<T>
    {
        public T? Model { get; set; } = default;
        public HttpStatusCode StatusCode { get; set; }
    }
}
