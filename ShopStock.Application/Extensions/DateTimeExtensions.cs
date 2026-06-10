using System.Globalization;

namespace ShopStock.Application.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly PersianCalendar Pc = new();

        /// <summary>
        /// تبدیل تاریخ میلادی به رشته شمسی با فرمت YYYY/MM/DD
        /// </summary>
        public static string ToShamsi(this DateTime date)
        {
            int year = Pc.GetYear(date);
            int month = Pc.GetMonth(date);
            int day = Pc.GetDayOfMonth(date);

            // استفاده از format string برای قرار دادن صفر پشت اعداد تک رقمی (مثلا 03 به جای 3)
            return $"{year}/{month:00}/{day:00}";
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به رشته شمسی همراه با ساعت و دقیقه
        /// </summary>
        public static string ToShamsiWithTime(this DateTime date)
        {
            int year = Pc.GetYear(date);
            int month = Pc.GetMonth(date);
            int day = Pc.GetDayOfMonth(date);

            int hour = Pc.GetHour(date);
            int minute = Pc.GetMinute(date);

            return $"{year}/{month:00}/{day:00} - {hour:00}:{minute:00}";
        }
    }
}
