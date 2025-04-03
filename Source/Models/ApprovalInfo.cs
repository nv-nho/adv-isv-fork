using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    [Serializable]
    public class ApprovalInfo
    {
        public long RowNumber { get; set; }
        public int ID { get; set; }
        public int UID { get; set; }

        public int ApprovalStatus { get; set; }
        public string ApprovalStatusName { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string ApplyContent { get; set; }
        public long UpdateDate { get; set; }

        public ApprovalInfo(){ }

        /// <summary>
        /// Constructor class PayslipInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ApprovalInfo(DbDataReader dr)
        {
            if (dr["RowNumber"].ToString().Length > 0)
            {
                this.RowNumber = (long)dr["RowNumber"];
            }
            this.ID = int.Parse(dr["ID"].ToString());
            this.UID = int.Parse(dr["UID"].ToString());
            this.UserName = dr["UserName"].ToString();
            this.ApprovalStatus = int.Parse(dr["ApprovalStatus"].ToString());
            this.ApprovalStatusName = dr["ApprovalStatusName"].ToString();
            this.Date = (DBNull.Value.Equals(dr["Date"]) ? "" : ((string)dr["Date"]));
            this.UpdateDate = (DBNull.Value.Equals(dr["UpdateDate"]) ? DateTime.Now.Ticks : ((DateTime)dr["UpdateDate"]).Ticks);

            List<string> lstApplyContent = new List<string>();
            //休暇
            if (dr["VacationFull"].ToString() != string.Empty)
            {
                lstApplyContent.Add("全休（" + dr["VacationFull"].ToString() + "）"); 
            }

            //休暇
            if (dr["VacationMorning"].ToString() != string.Empty)
            {
                lstApplyContent.Add("午前半休（" + dr["VacationMorning"].ToString() + "）"); 
            }

            //休暇
            if (dr["VacationAfternoon"].ToString() != string.Empty)
            {
                lstApplyContent.Add("午後半休（" + dr["VacationAfternoon"].ToString() + "）");
            }

            if (dr["IsUseTimeApproval"].ToString() == "1")
            {
                //遅刻
                if (dr["LateHours"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("遅刻");
                }

                //早退
                if (dr["EarlyHours"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("早退");
                }

                //所定休日
                if (dr["SH"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("所定休日");
                }

                //法定休日
                if (dr["LH"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("法定休日");
                }

                //早出残業
                if (dr["OT"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("残業");
                }
            }

            ApplyContent = string.Join("<br/>", lstApplyContent);
        }
    }

    [Serializable]
    public class ApprovalMailInfo
    {
        public string MailSubject { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string ExchangeDate { get; set; }
        public string ApplyContent { get; set; }
        public string RequestNote { get; set; }
        public string EntryTime { get; set; }
        public string ExitTime { get; set; }
        public string ApprovalUserName { get; set; }
        public string ApprovalNote { get; set; }

        public ApprovalMailInfo() { }

        /// <summary>
        /// Constructor class PayslipInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ApprovalMailInfo(DbDataReader dr)
        {
            this.DepartmentName = dr["DepartmentName"].ToString();
            this.UserName = dr["UserName"].ToString();
            this.Date = (DBNull.Value.Equals(dr["Date"]) ? "" : ((string)dr["Date"]));
            this.ExchangeDate = (DBNull.Value.Equals(dr["ExchangeDate"]) ? "" : ("（休出日：" + (string)dr["ExchangeDate"] + "）"));
            this.RequestNote = dr["RequestNote"].ToString().Trim();
            this.ApprovalUserName = dr["ApprovalUserName"].ToString();
            this.ApprovalNote = dr["ApprovalNote"].ToString().Trim();

            List<string> lstMailSubject = new List<string>();
            List<string> lstApplyContent = new List<string>();
            string strSubVal;
            //休暇
            if (dr["VacationFull"].ToString() != string.Empty)
            {
                strSubVal = dr["VacationFull"].ToString();
                lstApplyContent.Add("全休（" + strSubVal + "）");
                if (!lstMailSubject.Contains(strSubVal))
                {
                    lstMailSubject.Add(strSubVal);
                }
            }

            //休暇
            if (dr["VacationMorning"].ToString() != string.Empty)
            {
                strSubVal = dr["VacationMorning"].ToString();
                lstApplyContent.Add("午前半休（" + strSubVal + "）");
                if (!lstMailSubject.Contains(strSubVal))
                {
                    lstMailSubject.Add(strSubVal);
                }
            }

            //休暇
            if (dr["VacationAfternoon"].ToString() != string.Empty)
            {
                strSubVal = dr["VacationAfternoon"].ToString();
                lstApplyContent.Add("午後半休（" + strSubVal + "）");
                if (!lstMailSubject.Contains(strSubVal))
                {
                    lstMailSubject.Add(strSubVal);
                }
            }

            if (dr["IsUseTimeApproval"].ToString() == "1")
            {
                //遅刻
                if (dr["LateHours"].ToString() != string.Empty
                    || dr["EarlyHours"].ToString() != string.Empty
                    || dr["SH"].ToString() != string.Empty
                    || dr["LH"].ToString() != string.Empty
                    || dr["OT"].ToString() != string.Empty
                   )
                {
                    //出勤時刻
                    if (dr["EntryTime"].ToString() != string.Empty)
                    {
                        EntryTime = Utilities.CommonUtil.IntToTime(int.Parse(dr["EntryTime"].ToString()), true);
                    }

                    //退出時刻
                    if (dr["ExitTime"].ToString() != string.Empty)
                    {
                        ExitTime = Utilities.CommonUtil.IntToTime(int.Parse(dr["ExitTime"].ToString()), true);
                    }
                }

                //遅刻
                if (dr["LateHours"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("遅刻");
                    lstMailSubject.Add("遅刻");
                }

                //早退
                if (dr["EarlyHours"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("早退");
                    lstMailSubject.Add("早退");
                }

                //所定休日
                if (dr["SH"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("所定休日");
                    lstMailSubject.Add("所定休日");
                }

                //法定休日
                if (dr["LH"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("法定休日");
                    lstMailSubject.Add("法定休日");
                }

                //早出残業
                if (dr["OT"].ToString() != string.Empty)
                {
                    lstApplyContent.Add("残業");
                    lstMailSubject.Add("残業");
                }
            }

            MailSubject = string.Join("・", lstMailSubject);
            ApplyContent = string.Join("、", lstApplyContent);
        }
    }
}
