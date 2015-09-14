using System;
using System.Globalization;
using System.Linq;

namespace SolarSystemWeb.Models.Helpers
{
    public static class NumberHelper
    {
        /// <summary>
        /// Возвращает число в виде строки с проблеами, с проблеами, 
        /// разделяющими группы по три цифры
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToStringWithDelimiters(this double num)
        {
            var nfi = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalDigits = 0 };            
            nfi.NumberGroupSeparator = " ";

            return num.ToString("n", nfi);
        }

        /// <summary>
        /// Возвращает Tuple из двух строк, где первая строка - мантисса, 
        /// а вторая - экспонента аргумента при основании степени, равном 10        
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static Tuple<string, string> ExtractExponentInfo(this double num)
        {
            if(num < 1000)
                return new Tuple<string, string>(num.ToString("E3"), "");

            string raw = num.ToString("E3");
            var arr = raw.Split(new [] { "E+" }, StringSplitOptions.RemoveEmptyEntries);
            return new Tuple<string, string>(arr.First(), arr.Last().TrimStart('0'));
        }
    }
}