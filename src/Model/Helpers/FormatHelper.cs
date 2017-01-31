using System;

namespace Model.Helpers
{
    public static class FormatHelper
    {
        public static string DateFormat(DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }

        public static string DateTimeFormat(DateTime datetime)
        {
            return datetime.ToString("dd.MM.yyyy HH:mm");
        }
    }
}