using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.TelegramApi.Calendar
{
    public static class Bold
    {
        public static string StringToBold(string str)
        { 
            string[] temp = new string[str.Length];
            string messageBold = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '0') temp[i] = "𝟎";
                else if (str[i] == '1') temp[i] = "𝟏";
                else if (str[i] == '2') temp[i] = "𝟐";
                else if (str[i] == '3') temp[i] = "𝟑";
                else if (str[i] == '4') temp[i] = "𝟒";
                else if (str[i] == '5') temp[i] = "𝟓";
                else if (str[i] == '6') temp[i] = "𝟔";
                else if (str[i] == '7') temp[i] = "𝟕";
                else if (str[i] == '8') temp[i] = "𝟖";
                else if (str[i] == '9') temp[i] = "𝟗";
                else continue;
            }

            for (int i = 0; i < temp.Length; i++)
            {
                messageBold += temp[i];
            }
            return messageBold;
        }
    }
}
