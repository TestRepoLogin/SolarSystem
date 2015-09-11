using System;

namespace SolarSystemWeb.Models.Helpers
{
    public static class TimespanHelper
    {
        public static string ToReadableString(this TimeSpan span)
        {
            int years = span.Days / 365;
            span = span.Subtract(TimeSpan.FromDays(365 * years));

            string formatted = string.Format("{0}{1}{2}{3}{4}",
                years > 0 ? string.Format("{0:0} {1}, ", years, GetShortYearString(years)) : string.Empty,
                span.Duration().Days > 0 ? string.Format("{0:0} {1} ", span.Days, span.Days == 1 ? string.Empty : "д.") : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} ч. ", span.Hours) : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} мин. ", span.Minutes) : string.Empty,
                span.Duration().Seconds > 0 ? string.Format("{0:0} сек.", span.Seconds) : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted))
                formatted = "0 сек.";

            return formatted;
        }

        private static string GetShortYearString(int years)
        {
            int reminder = years%10;
            return (reminder > 4 || reminder == 0) || (years > 10 && years < 15) ? "лет" : "г.";
        }
    }
}