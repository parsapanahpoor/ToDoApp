using System.Globalization;

namespace ToDoApp.Application.Helpers;

public static class DateHelper
{
    private static readonly PersianCalendar PersianCal = new();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی
    /// </summary>
    public static string ToPersianDate(DateTime dateTime, bool includeTime = false)
    {
        var year = PersianCal.GetYear(dateTime);
        var month = PersianCal.GetMonth(dateTime);
        var day = PersianCal.GetDayOfMonth(dateTime);

        var result = $"{year}/{month:00}/{day:00}";
        
        if (includeTime)
        {
            result += $" {dateTime.Hour:00}:{dateTime.Minute:00}";
        }

        return result;
    }

    /// <summary>
    /// تبدیل تاریخ شمسی به میلادی
    /// </summary>
    public static DateTime FromPersianDate(int year, int month, int day, int hour = 0, int minute = 0)
    {
        return PersianCal.ToDateTime(year, month, day, hour, minute, 0, 0);
    }

    /// <summary>
    /// تبدیل رشته تاریخ شمسی به میلادی (فرمت: yyyy/mm/dd)
    /// </summary>
    public static DateTime? ParsePersianDate(string persianDate, int hour = 0, int minute = 0)
    {
        if (string.IsNullOrWhiteSpace(persianDate))
            return null;

        try
        {
            var parts = persianDate.Split('/');
            if (parts.Length != 3)
                return null;

            var year = int.Parse(parts[0]);
            var month = int.Parse(parts[1]);
            var day = int.Parse(parts[2]);

            return FromPersianDate(year, month, day, hour, minute);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// گرفتن نام روز هفته به فارسی
    /// </summary>
    public static string GetPersianDayName(DateTime dateTime)
    {
        var dayOfWeek = PersianCal.GetDayOfWeek(dateTime);
        return dayOfWeek switch
        {
            DayOfWeek.Saturday => "شنبه",
            DayOfWeek.Sunday => "یکشنبه",
            DayOfWeek.Monday => "دوشنبه",
            DayOfWeek.Tuesday => "سه‌شنبه",
            DayOfWeek.Wednesday => "چهارشنبه",
            DayOfWeek.Thursday => "پنج‌شنبه",
            DayOfWeek.Friday => "جمعه",
            _ => ""
        };
    }

    /// <summary>
    /// گرفتن نام ماه به فارسی
    /// </summary>
    public static string GetPersianMonthName(int month)
    {
        return month switch
        {
            1 => "فروردین",
            2 => "اردیبهشت",
            3 => "خرداد",
            4 => "تیر",
            5 => "مرداد",
            6 => "شهریور",
            7 => "مهر",
            8 => "آبان",
            9 => "آذر",
            10 => "دی",
            11 => "بهمن",
            12 => "اسفند",
            _ => ""
        };
    }
}

