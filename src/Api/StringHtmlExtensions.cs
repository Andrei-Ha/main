namespace Exadel.OfficeBooking.TelegramApi
{
    public static class StringHtmlExtensions
    {
        public static string Bold(this string str)
        {
            return "<b>" + str + "</b>";
        }

        public static string DelBoldTags(this string str) => str.Replace("<b>", "").Replace("</b>", "").Replace("<i>", "").Replace("</i>", "");
    }
}
