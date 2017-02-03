using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PresentationAspNet.MVC
{
    public static class ExtendMethods
    {
        public static String ToNullFromString(this string dString)
        {
            return string.IsNullOrEmpty(dString) ? null : dString;
        }

        public static String ToStrFromDiscount(this bool? dBool)
        {
            return dBool.Value ? "%" : "đ";
        }
        public static String ToStrFromDiscount(this bool dBool)
        {
            return dBool ? "%" : "đ";
        }

        public static String ToStrFromInt(this int dDecimal)
        {
            return Decimal.Parse(dDecimal.ToString(CultureInfo.InvariantCulture)).ToString("###,##0");
        }
        public static String ToStrFromInt(this int? dDecimal)
        {
            if (dDecimal == null || dDecimal == 0)
            {
                return "0";
            }
            return ((decimal)dDecimal).ToString("###,##0");
        }
        public static String ToStrFromDec(this decimal dDecimal)
        {
            return dDecimal.ToString("###,##0");
        }
        public static String ToStrFromDec(this decimal? dDecimal)
        {
            if (dDecimal == null || dDecimal == 0)
            {
                return "0";
            }
            return ((decimal)dDecimal).ToString("###,##0");
        }
        public static String ToStrCommasFromDec(this decimal? dDecimal)
        {
            if (dDecimal == null || dDecimal == 0)
            {
                return "0";
            }
            return ((decimal)dDecimal).ToString("#,###.##", CultureInfo.InvariantCulture);
        }
        public static String ToStrCommasFromDec(this decimal dDecimal)
        {
            return dDecimal == 0 ? "0" : dDecimal.ToString("#,###", CultureInfo.InvariantCulture);
        }

        public static String ToStrFromDecNoDot(this decimal? dDecimal)
        {
            if (dDecimal == null || dDecimal == 0)
            {
                return "0";
            }
            return ((decimal)dDecimal).ToString("###,##0");
        }
        public static String ToStrFromDecNoDotAndComma(this decimal? dDecimal)
        {
            if (dDecimal == null || dDecimal == 0)
            {
                return "0";
            }
            return ((decimal)dDecimal).ToString("##0");
        }
        public static String ToStrFromDecHex(this decimal? dDecimal)
        {
            if (dDecimal == null || dDecimal == 0)
            {
                return "0";
            }
            return ((decimal)dDecimal).ToString("###,##0.#####");
        }
        public static String ToStrFromDouble(this double? dDouble)
        {
            if (dDouble == null || dDouble == 0)
            {
                return "0";
            }
            return ((double)dDouble).ToString("###,##0");
        }

        public static String ToStrFromDoubleHasPoint(this double? dDouble)
        {
            if (dDouble == null || dDouble == 0)
            {
                return "0";
            }
            var str = ((double)dDouble).ToString("###,###.##", CultureInfo.CreateSpecificCulture("en-US"));
            return str;
        }
        public static String ToStrFromDouble(this double dDouble)
        {
            return dDouble.ToString("###,##0");
        }
        public static String ToStrFromDoubleNoSeparate(this double? dDouble)
        {
            if (dDouble == null || dDouble == 0)
            {
                return "0";
            }
            return ((double)dDouble).ToString("##0");
        }
        public static String ToStrFromDoubleNoSeparate(this double dDouble)
        {
            return dDouble.ToString("##0");
        }
        public static String FormatDecFromStr(this string Sstring)
        {
            if (Sstring == null || Sstring == "0")
            {
                return "0";
            }
            return Sstring.Replace(".", "");
        }
        public static String ToStrFromDate(this DateTime dDate)
        {
            return dDate.ToString("dd/MM/yyyy") == "01/01/0001" ? DateTime.Now.ToString("dd/MM/yyyy") : dDate.ToString("dd/MM/yyyy");
        }
        public static String ToStrFromDateNoSeparate(this DateTime dDate)
        {
            return dDate.ToString("yyyyMMdd");
        }
        public static String ToStrFromDateNoSeparate(this DateTime? dDate)
        {
            return ((DateTime)dDate).ToString("yyyyMMdd");
        }
        public static String removeSignDate(this string dDate)
        {
            return dDate.Replace("-", "/");
        }
        public static String ToStrFromDate(this DateTime? dDate)
        {
            if (dDate == null)
            {
                return DateTime.Now.ToString("dd/MM/yyyy");
            }
            return ((DateTime)dDate).ToString("dd/MM/yyyy");
        }
        public static String ToStrFromDateFull(this DateTime? dDate)
        {
            if (dDate == null)
            {
                return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            return ((DateTime)dDate).ToString("dd/MM/yyyy HH:mm:ss");
        }
        public static String ToStrFromDateFull(this DateTime dDate)
        {
            return dDate.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public static String ToTimeFromDate(this DateTime? dDate)
        {
            if (dDate == null)
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
            return ((DateTime)dDate).ToString("HH:mm:ss");
        }
        public static String ToTimeFromDate(this DateTime dDate)
        {
            return dDate.ToString("HH:mm:ss");
        }
    }

    public class DateUtilities
    {
        public static void FormatDate(string tungay, out DateTime tNgay, string denngay, out DateTime dNgay)
        {
            ConvertToDateTime(tungay, out tNgay);
            ConvertToDateTime(denngay, out dNgay);
            tNgay = GetDateTimeExactlyFirst(tNgay);
            dNgay = GetDateTimeExactlyLast(dNgay);
        }

        public static void GetDateTimeExactlyFull(string tungay, out DateTime tNgay, string denngay, out DateTime dNgay)
        {
            ConvertToDateTime(tungay, out tNgay);
            ConvertToDateTime(denngay, out dNgay);
            tNgay = GetDateTimeFull(tNgay);
            dNgay = GetDateTimeFull(dNgay);
        }

        public static DateTime FirstDayOfMonthFromDateTime(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
        {
            var firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public static bool ConvertToDateTime(string dateValue, out DateTime dateObject)
        {
            dateValue = string.IsNullOrEmpty(dateValue) ? DateTime.Now.ToStrFromDate() : dateValue;
            var regEx = new Regex(@"\d{1,2}/\d{1,2}/\d{2,4}");
            bool flag = true;
            if (!regEx.IsMatch(dateValue))
            {
                dateObject = DateTime.Now;
                return false;
            }

            var dtfi = new DateTimeFormatInfo { ShortDatePattern = "dd/MM/yyyy", DateSeparator = "/" };
            try
            {
                dateObject = GetDateTimeFull(Convert.ToDateTime(dateValue, dtfi));
            }
            catch
            {
                dateObject = DateTime.Now;
                flag = false;
            }
            return flag;
        }
        public static bool ConvertToDateTime(string dateValue, out DateTime? dateObject)
        {
            dateValue = string.IsNullOrEmpty(dateValue) ? DateTime.Now.ToStrFromDate() : dateValue;
            var regEx = new Regex(@"\d{1,2}/\d{1,2}/\d{2,4}");
            bool flag = true;
            if (!regEx.IsMatch(dateValue))
            {
                dateObject = DateTime.Now;
                return false;
            }

            var dtfi = new DateTimeFormatInfo { ShortDatePattern = "dd/MM/yyyy", DateSeparator = "/" };
            try
            {
                dateObject = Convert.ToDateTime(dateValue, dtfi);
            }
            catch
            {
                dateObject = DateTime.Now;
                flag = false;
            }
            return flag;
        }

        public static DateTime GetDateTimeFull(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, 0);
        }

        public static DateTime GetDateTimeExactlyFirst(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime GetDateTimeExactlyLast(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }
    }
}