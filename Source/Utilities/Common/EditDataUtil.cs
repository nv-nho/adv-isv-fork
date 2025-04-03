using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace OMS.Utilities
{
    public class EditDataUtil
    {
        /// <summary>
        /// Fix code to show
        /// </summary>
        /// <param name="iCode"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string ToFixCodeShow(string iCode, int iLength)
        {
            if (string.IsNullOrEmpty(iCode))
            {
                return iCode;
            }
            Match match = Regex.Match(iCode, Constants.PATTERN_NUMERIC, RegexOptions.IgnoreCase);
            // Here we check the Match instance.
            if (match.Success)
            {
                return iCode.Substring(iCode.Length - iLength, iLength);
            }
            return iCode;
        }

        /// <summary>
        /// Fix code to db
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToFixCodeDB(string iCode, int length)
        {
            if (string.IsNullOrEmpty(iCode))
            {
                return iCode;
            }
            Match match = Regex.Match(iCode, Constants.PATTERN_NUMERIC, RegexOptions.IgnoreCase);
            // Here we check the Match instance.
            if (match.Success)
            {
                if (iCode.Length > length)
                {
                    return iCode.Substring(iCode.Length - length, length);
                }
                return iCode.PadLeft(length, '0');
            }
            return iCode;
        }

        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string jsonString = ser.Serialize(t);
            return jsonString;
        }
        
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return (T)ser.Deserialize(jsonString, typeof(T));
        }

        public static string Decode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            IDictionary<string, string> _specials_d = EditDataUtil.JsonDeserialize<Dictionary<string, string>>(Constants.SPECIALS_D);
            foreach (string key in _specials_d.Keys)
            {
                value = value.Replace(key, _specials_d[key]);
            }

            return value;
        }
    }
}