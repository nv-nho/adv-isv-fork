using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OMS.Utilities;

namespace OMS.Utilities
{
    public class CommonUtil
    {
        /// <summary>
        /// Get default date
        /// VN-Nho
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDefaultDate()
        {
            return new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// Parse Integer
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>integer</returns>
        public static int ParseInteger(string value)
        {
            return int.Parse(value, NumberStyles.Number, new CultureInfo(Constants.DFT_CULTURE_NAME));
        }

        /// <summary>
        /// Try parse integer
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="result">out param int</param>
        /// <returns>TRUE: is Integer, FALSE: is not integer</returns>
        public static bool TryParseInt(string value, ref int result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (int.TryParse(value, NumberStyles.Number, new CultureInfo(Constants.DFT_CULTURE_NAME), out result))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Try parse number
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="result">out param number</param>
        /// <returns>TRUE: is number, FALSE: is not number</returns>
        public static bool TryParseNumber(string value, ref double result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (double.TryParse(value, NumberStyles.Number, new CultureInfo(Constants.DFT_CULTURE_NAME), out result))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Try parse decimal
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="result">out param decimal</param>
        /// <returns>TRUE: is decimal, FALSE: is not decimal</returns>
        public static bool TryParseDecimal(string value, ref decimal result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (decimal.TryParse(value, NumberStyles.Number, new CultureInfo(Constants.DFT_CULTURE_NAME), out result))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parse Decimal
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>decimal</returns>
        public static decimal ParseDecimal(string value)
        {
            return decimal.Parse(value, NumberStyles.Number, new CultureInfo(Constants.DFT_CULTURE_NAME));
        }

        /// <summary>
        /// Try parse Date
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="result">out param Date</param>
        /// <returns>TRUE: is Date, FALSE: is not Date</returns>
        public static bool TryParseDate(string value, ref DateTime result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (DateTime.TryParseExact(value, Constants.FMT_DATE, new CultureInfo(Constants.DFT_CULTURE_NAME), DateTimeStyles.None, out result))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check Scale decimal number 
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">decimal number </param>
        /// <param name="scale">scale</param>
        public static bool CheckScale(decimal value, int scale)
        {
            decimal temp = decimal.Parse(value.ToString("G29"));
            System.Data.SqlTypes.SqlDecimal x = new System.Data.SqlTypes.SqlDecimal(temp);
            return x.Scale <= scale;
        }

        /// <summary>
        /// Parse Date
        /// Author: ISV-Vinh
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>DateTime</returns>
        public static DateTime ParseDate(string value)
        {
            return DateTime.ParseExact(value, Constants.FMT_DATE, new CultureInfo(Constants.DFT_CULTURE_NAME), DateTimeStyles.None);
        }

        /// <summary>
        /// Parse Date
        /// Author: ISV-Nho
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="fromFormat">format</param>
        /// <returns>DateTime</returns>
        public static DateTime ParseDate(string value, string fromFormat)
        {
            return DateTime.ParseExact(value, fromFormat, new CultureInfo(Constants.DFT_CULTURE_NAME), DateTimeStyles.None);
        }

        /// <summary>
        /// Parse Date
        /// Author: ISV-Nho
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="fromFormat">from format</param>
        /// <param name="toFormat">to format</param>
        /// <returns>string date</returns>
        public static string ParseDate(string value, string fromFormat, string toFormat)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            else
            {
                return DateTime.ParseExact(value, fromFormat, new CultureInfo(Constants.DFT_CULTURE_NAME), DateTimeStyles.None).ToString(toFormat);
            }

        }

        /// <summary>
        /// Check null value
        /// </summary>
        /// <typeparam name="TypeValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullVal(object value, bool defaultValueIsNull)
        {
            if (defaultValueIsNull)
            {
                if (value == null)
                {
                    return true;
                }

                var type = value.GetType();
                if (type.Equals(typeof(string)))
                {
                    return string.IsNullOrEmpty((string)value);
                }
                if (type.Equals(typeof(bool)))
                {
                    return (bool)value == default(bool);
                }
                if (type.Equals(typeof(DateTime)))
                {
                    return (DateTime)value == default(DateTime);
                }
                //number 
                var number = 0m;
                if (decimal.TryParse(value.ToString(), out number))
                {
                    return number == default(decimal);
                }
            }
            else
            {
                if (value == null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Customs function Left VB
        /// </summary>
        /// <param name="Original"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static string Left(string Original, int Count)
        {
            // Can't remember if the Left function throws an exception in this case,but for
            // this method, we will just return the original string.
            if (Original == null || Original == string.Empty
                || Original.Length < Count)
            {
                return Original;
            }
            else
            {
                // Return a sub-string of the original string, starting at index 0.
                return Original.Substring(0, Count);
            }
        }

        /// <summary>
        /// Customs function Right VB
        /// </summary>
        /// <param name="Original"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static string Right(string Original, int Count)
        {
            // same thing as above.
            if (Original == null || Original == string.Empty
                || Original.Length < Count)
            {
                return Original;
            }
            else
            {
                // blah blah blah
                return Original.Substring(Original.Length - (Count));
            }
        }

        /// <summary>
        /// Customs function Mid VB
        /// </summary>
        /// <param name="param"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Mid(string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            string result = param.Substring(startIndex - 1, length);
            //return the result of the operation
            return result;
        }

        /// <summary>
        /// Customs function Mid VB
        /// </summary>
        /// <param name="param"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string Mid(string param, int startIndex)
        {
            //start at the specified index and return all characters after it
            //and assign it to a variable
            string result = param.Substring(startIndex - 1);
            //return the result of the operation
            return result;
        }

        /// <summary>
        /// TimeToInt
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public static int TimeToInt(string pValue)
        {
            if (pValue == "")
                return 0;
            string[] ary = pValue.Split(':');
            int nHours = 0;
            int nMinutes = 0;

            if (ary.Length != 2)
            {
                return 0;
            }

            nHours = int.Parse(ary[0]);
            nMinutes = int.Parse(ary[1]);
            if (nMinutes < 0)
            {

                return 0;
            }

            return (nHours * 600 + nMinutes * 10);
        }

        /// <summary>
        /// IntToTime
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="pShowZero"></param>
        /// <returns></returns>
        public static string IntToTime(int pValue, bool pShowZero)
        {

            string sResult = "";
            int nHours = 0;
            int nMinutes = 0;


            //ゼロ値表示無効
            if (pValue == 0 && !pShowZero)
            {
                return "";
            }

            //負数対応
            if (pValue < 0)
            {
                pValue *= -1;
                sResult = "-";
            }

            //単位変換(600 = 1時間)
            nHours = pValue / 600;
            nMinutes = (pValue % 600) / 10;

            sResult += nHours.ToString().PadLeft(2, '0');
            sResult += ":";
            sResult += nMinutes.ToString().PadLeft(2, '0');

            return sResult;
        }

        /// <summary>
        /// IntToWorkingTime
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="pShowZero"></param>
        /// <returns></returns>
        public static decimal IntToHours(int pValue)
        {

            decimal nHours = 0;
            decimal nMinutes = 0;


            //ゼロ値表示無効
            if (pValue == 0)
            {
                return 0;
            }

            //負数対応
            if (pValue < 0)
            {
                pValue *= -1;
            }

            //単位変換(600 = 1時間)
            nHours = pValue / 600;
            nMinutes = (pValue % 600) / 10;

            return nHours + nMinutes / 60;
        }

        /// <summary>
        /// TimeToMinute
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public static int TimeToMinute(string pValue)
        {
            string[] ary = pValue.Split(':');
            int nHours = 0;
            int nMinutes = 0;

            if (ary.Length != 2)
            {
                return 0;
            }

            nHours = int.Parse(ary[0]);
            nMinutes = int.Parse(ary[1]);
            if (nMinutes < 0)
            {

                return 0;
            }

            return (nHours * 60 + nMinutes);
        }
    }
}
