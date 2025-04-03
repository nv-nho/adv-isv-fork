using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class ContentInfo
    /// ISV-THAN
    /// </summary>
    [Serializable]
    public class ContentInfo
    {
        public string WorkTime { get; set; }
        public string ProjectName { get; set; }
        public string Content { get; set; }
        public string VacationFlag { get; set; }
        public string ContentVacation { get; set; }
        public string Value3 { get; set; }
        public int RowNumber { get; set; }

        /// <summary>
        /// Constructor class ContentInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ContentInfo(DbDataReader dr)
        {
            this.WorkTime = Utilities.CommonUtil.IntToTime(int.Parse(dr["WorkTime"].ToString()), true);
            this.ProjectName = dr["ProjectName"].ToString();
            this.Content = "&nbsp&nbsp&nbsp[" + this.WorkTime + "] &nbsp &nbsp &nbsp " + this.ProjectName;
            this.VacationFlag = dr["VacationFlag"].ToString();
            this.Value3 = dr["Value3"].ToString();
            switch (int.Parse(dr["VacationFlag"].ToString()))
            {
                case 0:
                    this.ContentVacation = "&nbsp;&nbsp;&nbsp;全休 : " + "<span style='color:red'>" + dr["Value3"].ToString() + "</span>";
                    break;
                case 1:
                    this.ContentVacation = "&nbsp;&nbsp;&nbsp;前半休 : " + "<span style='color:red'>" + dr["Value3"].ToString() + "</span>";
                    break;
                case 2:
                    this.ContentVacation = "&nbsp;&nbsp;&nbsp;後半休 : " + "<span style='color:red'>" + dr["Value3"].ToString() + "</span>";
                    break;
                case 3:
                    if (int.Parse(dr["RowNumber"].ToString()) == 1)
                    {
                        this.ContentVacation = "&nbsp;&nbsp;&nbsp;前半休 : " + "<span style='color:red'>" + dr["Value3"].ToString() + "</span>";
                    }
                    else if (int.Parse(dr["RowNumber"].ToString()) == 2)
                    {
                        this.ContentVacation = "&nbsp;&nbsp;&nbsp;後半休 : " + "<span style='color:red'>" + dr["Value3"].ToString() + "</span>";
                    }
                    break;
            }

        }

        /// <summary>
        /// Constructor class ContentInfo
        /// </summary>
        public ContentInfo()
        {
            this.WorkTime = string.Empty;
            this.ProjectName = string.Empty;
            this.Content = string.Empty;
        }
    }
}
