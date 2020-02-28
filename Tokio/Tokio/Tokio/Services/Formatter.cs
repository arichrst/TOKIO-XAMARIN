using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tokio.Services
{
    public static class Formatter
    {
        public static string ToRupiah(this double amount)
        {
            CultureInfo culture = new CultureInfo("id-ID");
            return String.Format(culture, "Rp. {0:N}", amount);
        }

        public static double ToDouble(this string rupiah)
        {
            return double.Parse(Regex.Replace(rupiah, @",.*|\D", ""));
        }

        public static string ToShortDate(this DateTime date)
        {
            return date.ToString("dd MMM yyyy");
        }

        public static string ToLongDate(this DateTime date)
        {
            return date.ToString("dd MMMM yyyy");
        }

        public static string ToTime(this DateTime date)
        {
            return date.ToString("hh:mm");
        }

        public static string ToCompleteDateTime(this DateTime date)
        {
            return date.ToString("dd MMM yyyy hh:mm");
        }
    }
}
