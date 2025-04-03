using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OMS.Utilities
{
    public class CheckDataUtil
    {
        /// <summary>
        /// Check is integer
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>TRUE: Integer, FLASE: not integer</returns>
        public static bool IsInteger(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            int temp;
            return int.TryParse(value, NumberStyles.AllowThousands, new CultureInfo(Constants.DFT_CULTURE_NAME), out temp);
        }

        /// <summary>
        /// Check is Number
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>TRUE: number, FLASE: not number</returns>
        public static bool IsNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            double temp;
            return double.TryParse(value, NumberStyles.Number, new CultureInfo(Constants.DFT_CULTURE_NAME), out temp);
        }

        /// <summary>
        /// Check is data
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>TRUE: date, FLASE: not date</returns>
        public static bool IsDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != Constants.FMT_DATE.Length)
            {
                return false;
            }

            DateTime temp;
            return DateTime.TryParseExact(value, Constants.FMT_DATE, new CultureInfo(Constants.DFT_CULTURE_NAME), DateTimeStyles.None, out temp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool isNumeric(System.Object Expression)
        {

            if (Expression == null || Expression is DateTime)
            {
                return false;
            }

            if (Expression is Int16 || Expression is Int32 || Expression is Int64
                || Expression is Decimal || Expression is Single || Expression is Double
                || Expression is Boolean)
            {
                return true;
            }
            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Decimal Check
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="outVaule">output value</param>
        /// <returns></returns>
        public static bool IsDecimal(object value, ref decimal outVaule)
        {
            if (value == null)
            {
                return false;
            }
            else
            {
                return decimal.TryParse(value.ToString(), out outVaule);
            }

        }

        /// <summary>
        /// Check Nummeric
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return CheckRegex(value, "^[0-9]*$");
        }

        /// <summary>
        /// Check AlphaNumeric
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsAnphaNumeric(string value)
        {
            return CheckRegex(value, "^[a-zA-Z0-9]*$");
        }

        /// <summary>
        /// check regex
        /// </summary>
        /// <param name="val">string value</param>
        /// <param name="pattern">pattern</param>
        /// <returns></returns>
        private static bool CheckRegex(string val, string pattern)
        {
            Match match = Regex.Match(val, pattern, RegexOptions.Compiled);
            return match.Success;
        }

        /// <summary>
        /// Check Email Format Correct.
        /// </summary>
        /// <param name="value">input value</param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            string regular = "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@"
                + "((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\."
                + "([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + "([a-zA-Z]+[\\w-]+\\.)+[a-zA-Z]{2,4})$";            
            return CheckRegex(value, regular);
        }

        /// <summary>
        /// Check Format Correct
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool IsCheckFormat(string value, string format)
        {
            return CheckRegex(value, format);
        }
    }
}