using System;
using System.Collections.Generic;
using System.Linq;

namespace EC226.Utility.Extensions;

public static class DateTimeExtend
{
   public static bool IsEmpty(this DateTime Self)
   {
      return Self == default;
   }

   public static bool Before(this DateTime Self, DateTime Target)
   {
      return Self.CompareTo(Target) < 0;
   }

   public static bool Between(this DateTime Self, DateTime Begin, DateTime End)
   {
      return Self >= Begin && Self <= End;
   }

   public static bool After(this DateTime Self, DateTime Target)
   {
      return Self.CompareTo(Target) > 0;
   }

   public static DateTime StartOfMonth(this DateTime Self)
   {
      return new DateTime(Self.Year, Self.Month, 1);
   }

   public static DateTime EndOfMonth(this DateTime Self)
   {
      return Self.StartOfMonth().AddMonths(1).AddTicks(-1);
   }

   public static DateTime StartOfDay(this DateTime Self)
   {
      return Self.Date;
   }

   public static DateTime EndOfDay(this DateTime Self)
   {
      return Self.Date.Next().AddSeconds(-1); //return Self.Date.AddDays(1).AddTicks(-1);
   }

   public static bool IsAfternoon(this DateTime Self)
   {
      return Self.IsAfternoon(12);
   }

   public static bool IsAfternoon(this DateTime Self, int AfternoonFrom)
   {
      return Self.Hour >= AfternoonFrom;
   }

   public static bool IsWeekEnd(this DateTime Self)
   {
      var day = Self.DayOfWeek;
      return day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;
   }

   public static DateTime BusinessDay(this DateTime Self, int Days, string Format, List<string> Holiday, bool CheckAfternoon = true)
   {
      if (CheckAfternoon && Self.IsAfternoon())
      {
         Days++;
      }

      return Self._BusinessDay(Days, Format, Holiday);
   }

   private static DateTime _BusinessDay(this DateTime Self, int Days, string FormatString, List<string> Holiday)
   {
      if ((Self.IsWeekEnd() && Days == 0) || Holiday.Contains(Self.ToString(FormatString)))
      {
         Days++;
      }

      for (int i = 0; i < Days; i++)
      {
         Self = Self.Next();
         if (Self.IsWeekEnd() || Holiday.Contains(Self.ToString(FormatString)))
         {
            i--;
         }
      }

      return Self;
   }

   public static DateTime BusinessDay(this DateTime Self, int Days, string Format, List<string> Holiday, int AfternoonFrom)
   {
      if (Self.IsAfternoon(AfternoonFrom))
      {
         Days++;
      }
      return Self._BusinessDay(Days, Format, Holiday);
   }

   public static string BusinessDayString(this DateTime Self, int Days, string Format, List<string> Holiday, bool CheckAfternoon = true)
   {
      return Self.BusinessDay(Days, Format, Holiday, CheckAfternoon).ToString(Format);
   }

   public static decimal ElapsedDays(this DateTime Self, DateTime EndDate)
   {
      return (decimal)(EndDate - Self).TotalDays;
   }

   public static decimal ElapsedHours(this DateTime Self, DateTime EndDate)
   {
      return (decimal)(EndDate - Self).TotalHours;
   }

   public static decimal ElapsedMinutes(this DateTime Self, DateTime EndDate)
   {
      return (decimal)(EndDate - Self).TotalMinutes;
   }

   public static DateTime Next(this DateTime Self)
   {
      return Self.AddDays(1);
   }

   public static DateTime Previous(this DateTime Self)
   {
      return Self.AddDays(-1);
   }

   public static DateTime Next(this DateTime Self, DayOfWeek DesiredDay)
   {
      return Self.AddDays(Count(Self.DayOfWeek, DesiredDay) * -1);
   }

   public static DateTime NextWorkDay(this DateTime Self, string dateFormate = "dd/MM/yyyy", params DateTime[] Holiday)
   {
      var holiday = Holiday.Select(h => h.ToString(dateFormate)).ToArray();

      while (Self.IsWeekEnd() || holiday.Contains(Self.ToString(dateFormate)))
      {
         Self = Self.Next();
         Self = DateTime.ParseExact(Self.ToString(dateFormate), dateFormate, System.Threading.Thread.CurrentThread.CurrentCulture).AddHours(1);
      }

      return Self;
   }

   private static int Count(DayOfWeek BaseDay, DayOfWeek DesiredDay)
   {
      int different = BaseDay - DesiredDay;
      if (different > 0)
      {
         return different - 7;
      }
      return different == 0 ? -7 : different;
   }

   public static DateTime Previous(this DateTime Self, DayOfWeek DesiredDay)
   {
      return Self.AddDays(Count(DesiredDay, Self.DayOfWeek));
   }

   public static DateTime ZeroSecond(this DateTime Self)
   {
      return Self.AddSeconds(-Self.Second);
   }

   public static DateTime SqlEndOfDay(this DateTime Self)
   {
      return Self.Date.Next().AddSeconds(-59);
   }

   public static DateTime SqlDateEnd(this DateTime Self)
   {
      return Self.SqlEndOfDay();
   }

   private static IEnumerable<DateTime> YieldDates(this DateTime Self, DateTime End, int Days)
   {
      while (Self <= End)
      {
         yield return Self;
         Self = Self.AddDays(Days);
      }
   }
}