using System;
namespace Tokio.Services
{
    public static class Validator
    {
        public static bool SameDateWith(this DateTime date, DateTime target)
        {
            return date.Day == target.Day && date.Month == target.Month && date.Year == target.Year;
        }

        public static bool BetweenDate(this DateTime date, DateTime start , DateTime end)
        {
            return date >= start && date <= end;
        }

        public static bool IsEmpty(this string text)
        {
            return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
        }
    }
}
