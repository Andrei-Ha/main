namespace Exadel.OfficeBooking.TelegramApi
{
    public static class StringHtmlExtensions
    {
        public static string Bold(this string str)
        {
            return "<b>" + str + "</b>";
        }
    }
}
