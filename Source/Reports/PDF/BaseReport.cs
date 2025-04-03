using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace OMS.Reports.PDF
{
    public class BaseReport : System.Web.UI.Page
    {
        #region Constant
        protected DateTime DATE_TIME_DEFAULT = new DateTime(1900, 1, 1);
        #endregion

        #region Get datsource

        /// <summary>
        /// get info company
        /// </summary>
        /// <returns></returns>
        protected M_Company GetCompany()
        {
            using (DB db = new DB())
            {
                CompanyService companySer = new CompanyService(db);

                //Get Company
                return companySer.GetData();
            }
        }

        /// <summary>
        /// GetGroupUserByID
        /// </summary>
        /// <param name="groupID">groupID</param>
        /// <returns>M_GroupUser_H</returns>
        protected M_GroupUser_H GetGroupUserByID(int groupID)
        {
            using (DB db = new DB())
            {
                GroupUserService groupUserSer = new GroupUserService(db);
                return groupUserSer.GetByGroupID(groupID);
            }
        }

        /// <summary>
        /// GetUserByUserCD
        /// </summary>
        /// <param name="userCd">userCd</param>
        /// <returns>M_User</returns>
        protected M_User GetUserByUserCD(string userCd)
        {
            using (DB db = new DB())
            {
                UserService userSer = new UserService(db);
                return userSer.GetByUserCD(userCd);
            }
        }

        #endregion

        #region Get display report

        #region Header page

        /// <summary>
        /// get path logo 1
        /// </summary>
        /// <returns></returns>
        protected string GetPathLogo1()
        {
            using (DB db = new DB())
            {
                SettingService settingSer = new SettingService(db);
                //var extension = Path.GetExtension(settingSer.GetData().Logo1);
                var logo = settingSer.GetData().Logo1;

                //Get Setting
                return new Uri(Server.MapPath("~/Logo/" + logo)).AbsoluteUri;
            }
        }

        /// <summary>
        /// get path logo
        /// </summary>
        /// <returns></returns>
        protected string GetPathLogo()
        {
            using (DB db = new DB())
            {
                SettingService settingSer = new SettingService(db);
                //var extension = Path.GetExtension(settingSer.GetData().Logo1);
                var logo = settingSer.GetData().Logo2;

                //Get Setting
                return new Uri(Server.MapPath("~/Logo/" + logo)).AbsoluteUri;
            }
        }

        #endregion

        #region Header report

        /// <summary>
        /// Get Format Date
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        protected string GetDispDate(int month, int day, int year)
        {
            string sDate = null;
            string sMonth = string.Empty;
            switch (month)
            {
                case 1:
                    sMonth = "Jan";
                    break;
                case 2:
                    sMonth = "Feb";
                    break;
                case 3:
                    sMonth = "Mar";
                    break;
                case 4:
                    sMonth = "Apr";
                    break;
                case 5:
                    sMonth = "May";
                    break;
                case 6:
                    sMonth = "Jun";
                    break;
                case 7:
                    sMonth = "Jul";
                    break;
                case 8:
                    sMonth = "Aug";
                    break;
                case 9:
                    sMonth = "Sep";
                    break;
                case 10:
                    sMonth = "Oct";
                    break;
                case 11:
                    sMonth = "Nov";
                    break;
                case 12:
                    sMonth = "Dec";
                    break;
            }

            sDate = day.ToString("00") + "/" + sMonth + "/" + year.ToString();
            return sDate;
        }

        /// <summary>
        /// Get Display Address
        /// </summary>
        /// <param name="add1"></param>
        /// <param name="add2"></param>
        /// <param name="add3"></param>
        /// <returns></returns>
        protected string GetDispAddressMultiLine(string add1, string add2, string add3)
        {
            //string temp = string.Empty;
            //if (string.IsNullOrEmpty(add1))
            //{
            //    return temp;
            //}

            //if (!string.IsNullOrEmpty(add2))
            //{
            //    temp = add1 + "\n" + add2;
            //}

            //if (!string.IsNullOrEmpty(add3))
            //{
            //    temp = add1 + "\n" + add2 + '\n' + add3;
            //}

            //return temp;

            //-------2014/12/08 ISV-HUNG Edit Start -----------//
            var fAdds = new string[] { add1, add2, add3 };
            return string.Join("\n", fAdds.Where(s => !string.IsNullOrEmpty(s)));
            //-------2014/12/08 ISV-HUNG Edit End -----------//
        }

        #endregion

        #region detail report

        #endregion

        #region footer report
        
        #endregion

        #endregion

        #region report

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DateValue"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        protected string GetDateString(DateTime DateValue, Language language)
        {
            if (language == Language.Vietnam)
            {
                dynamic StrDate = string.Format("ngày {0} tháng {1} năm {2}", DateValue.Day.ToString("00"), DateValue.Month.ToString("00"), DateValue.Year.ToString("0000"));
                return StrDate;
            }
            else if (language == Language.Japan)
            {
                dynamic StrDate = string.Format("{0}年月{1}日{2}", DateValue.Year.ToString("0000"), DateValue.Month.ToString("00"), DateValue.Day.ToString("00"));//DateValue.ToString("yyyy年MM月dd日");
                return StrDate;
            }
            else
            {
                dynamic SuffixDay = string.Empty;
                dynamic SuffixMoth = string.Empty;

                switch (DateValue.Day)
                {
                    case 1:
                    case 21:
                    case 31:
                        SuffixDay = "st";
                        break;
                    case 2:
                    case 22:
                        SuffixDay = "nd";
                        break;
                    case 3:
                    case 23:
                        SuffixDay = "rd";
                        break;
                    default:
                        SuffixDay = "th";
                        break;
                }
                switch (DateValue.Month)
                {
                    case 1:
                        SuffixMoth = "January";
                        break;
                    case 2:
                        SuffixMoth = "Feburary";
                        break;
                    case 3:
                        SuffixMoth = "March";
                        break;
                    case 4:
                        SuffixMoth = "April ";
                        break;
                    case 5:
                        SuffixMoth = "May";
                        break;
                    case 6:
                        SuffixMoth = "June ";
                        break;
                    case 7:
                        SuffixMoth = "July";
                        break;
                    case 8:
                        SuffixMoth = "August";
                        break;
                    case 9:
                        SuffixMoth = "September";
                        break;
                    case 10:
                        SuffixMoth = "October";
                        break;
                    case 11:
                        SuffixMoth = "November";
                        break;
                    case 12:
                        SuffixMoth = "December";
                        break;
                }
                return string.Format("{0} {1}{2}, {3}", SuffixMoth, DateValue.Day.ToString("00"), SuffixDay, DateValue.Year.ToString("0000"));
            }
        }

        /// <summary>
        /// get Number Line
        /// </summary>
        /// <param name="textString"></param>
        /// <param name="font"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public int getNumberLine(string textString, Font font, int width)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(0, 0, width, 1);
            SizeF sOne = TextRenderer.MeasureText("test", font, rectangle.Size, TextFormatFlags.WordBreak);
            SizeF s = TextRenderer.MeasureText(textString, font, rectangle.Size, TextFormatFlags.WordBreak);

            return int.Parse(Math.Ceiling(s.Height / sOne.Height).ToString());
        }

        /// <summary>
        /// Get Month String
        /// </summary>
        /// <param name="value">Date</param>
        /// <returns></returns>
        protected string GetMonthString(DateTime value)
        {
            switch (value.Month)
            {
                case 1:
                    return "January";
                case 2:
                    return "Feburary";
                case 3:
                    return "March";
                case 4:
                    return "April ";
                case 5:
                    return "May";
                case 6:
                    return "June ";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                default:
                    return "December";
            }
        }

        /// <summary>
        /// Get Day String
        /// </summary>
        /// <param name="value">Date</param>
        /// <returns></returns>
        protected string GetDayString(DateTime value)
        {
            switch (value.Day)
            {
                case 1:
                case 21:
                case 31:
                    return value.Day.ToString("00") + "st";
                case 2:
                case 22:
                    return value.Day.ToString("00") + "nd";
                case 3:
                case 23:
                    return value.Day.ToString("00") + "rd";
                default:
                    return value.Day.ToString("00") + "th";
            }
        }
        #endregion
    }
}
