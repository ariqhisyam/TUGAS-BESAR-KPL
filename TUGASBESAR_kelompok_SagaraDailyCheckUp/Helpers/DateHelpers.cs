using System;
using System.Globalization;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Helpers
{
    public static class DateHelper
    {
        public static string FormatIndo(DateTime date)
        {
            return date.ToString("dd MMMM yyyy", new CultureInfo("id-ID"));
        }
    }
}