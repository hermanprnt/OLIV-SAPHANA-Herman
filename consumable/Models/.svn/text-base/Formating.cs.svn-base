using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public static class Formating
    {

        public const string VIEW_DATE = "dd.MM.yyyy";
        public const string VIEW_TIME = "HH:mm:ss";
        public const string SQL_DATE = "yyyy-MM-dd";
        public const string COUNTER_DATE = "MMM yyyy";
        public const string PERIODE = "MM-yyyy";
        public const string FULL_DATE = "dddd, dd MMM yyyy HH:mm:ss";
        public const string HALF_DATE = "dd MMM yyyy HH:mm:ss";

        public static string FormatPeriodeDate(this DateTime datum, string dateformat = PERIODE)
        {
            return datum.ToString(dateformat);
        }

        public static string FormatPeriodeDate(this string datum, string dateformat = PERIODE)
        {
            if ((datum != null) && (datum != ""))
            {
                string[] ndate = datum.Split('-');
                DateTime d = Convert.ToDateTime(ndate[1] + '/' + ndate[0] + '/' + ndate[2]);
                return d.ToString(dateformat);
            }
            else
            {
                return "";
            }
        }

        public static string FormatViewDate(this DateTime datum, string dateformat = VIEW_DATE)
        {
            return datum.ToString(dateformat);
        }

        public static string FormatViewDate(this string datum, string dateformat = VIEW_DATE)
        {
            if ((datum != null) && (datum != ""))
            {
                string[] ndate = datum.Split('-');
                DateTime d = Convert.ToDateTime(ndate[1] + '/' + ndate[0] + '/' + ndate[2]);
                return d.ToString(dateformat);
            }
            else
            {
                return "";
            }
        }

        public static string FormatViewTime(this DateTime datum, string dateformat = VIEW_TIME)
        {
            return datum.ToString(dateformat);
        }

        public static string FormatSQLDate(this DateTime datum, string dateformat = SQL_DATE)
        {
            return datum.ToString(dateformat);
        }

        public static string FormatSQLDate(this string datum, string dateformat = SQL_DATE)
        {
            DateTime d = new DateTime(1900, 1, 1);
            string v = "";

            if (DateTime.TryParseExact(datum,
                                       VIEW_DATE,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out d))
            { v = d.ToString(dateformat); }
            else
            {
                v = "";
            }

            return v;
        }

        public static string FormatFullDate(this DateTime datum, string dateformat = FULL_DATE)
        {
            return datum.ToString(dateformat);
        }

        public static string FormatHalfDate(this DateTime datum, string dateformat = HALF_DATE)
        {
            return datum.ToString(dateformat);
        }

        public static string FormatCounterDate(this string datum, string dateformat = COUNTER_DATE)
        {
            DateTime d = Convert.ToDateTime(datum);
            return d.ToString(dateformat);
        }

        public static string FormatRpCurrency(this double nominal)
        {
            return string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "Rp. {0:N}", nominal);
        }

        public static string FormatRpCurrency(this long nominal)
        {
            return string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "Rp. {0:N}", nominal);
        }

        public static string FormatWithoutRpCurrency(this double nominal)
        {
            return string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "{0:N}", nominal);
        }

        public static string FormatDotSeparator(this double nominal)
        {
            return string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "{0:N0}", nominal);
        }

        public static string FormatDotSeparatorWithDecimal(this double nominal)
        {
            return string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "{0:N}", nominal);
        }       

        public static string FormatCommaSeparator(this double nominal)
        {
            return string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), "{0:N0}", nominal);
        }

        public static string FormatCommaSeparatorWithDecimal(this double nominal)
        {
            return string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), "{0:N}", nominal);
        }      

        public static string FormatUSDCurrency(this double nominal)
        {
            return string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), "$. {0:N}", nominal);
        }

        public static string FormatNumber(this double number)
        {
            return String.Format("{0:###,###,###,###,###,###,###,###,###.##}", number);
        }

        public static string FormatNumber(this int number)
        {
            return String.Format("{0:###,###,###,###,###,###,###,###,###.##}", number);
        }

        public static string FormatPercentage(this double number)
        {
            return string.Format("{0:0.00} %", number);
        }
    }
}