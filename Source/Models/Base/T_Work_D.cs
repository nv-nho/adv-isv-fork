using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    public class T_Work_D : M_Base<T_Work_D>
    {
        #region Variant

        /// <summary>
        /// hid
        /// </summary>
        private int hid { get; set; }

        /// <summary>
        /// hid
        /// </summary>
        private int id { get; set; }

        /// <summary>
        /// date
        /// </summary>
        private DateTime date { get; set; }

        /// <summary>
        /// lineno
        /// </summary>
        private int lineno { get; set; }

        /// <summary>
        /// pid
        /// </summary>
        private int pid { get; set; }

        /// <summary>
        /// workplace
        /// </summary>
        private string workplace { get; set; }

        /// <summary>
        /// starttime
        /// </summary>
        private int starttime { get; set; }

        /// <summary>
        /// endtime
        /// </summary>
        private int endtime { get; set; }

        /// <summary>
        /// worktime
        /// </summary>
        private int worktime { get; set; }

        /// <summary>
        /// Memo
        /// </summary>
        private string memo { get; set; }


        /// <summary>
        /// projectName
        /// </summary>
        private string projectCd { get; set; }

        /// <summary>
        /// projectName
        /// </summary>
        private string projectName { get; set; }
        
        #endregion
        #region Property
        /// <summary>
        /// Get or set HID
        /// </summary>
        public int HID
        {
            get { return hid; }
            set
            {
                if (value != hid)
                {
                    hid = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Date
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value != date)
                {
                    date = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set LineNo
        /// </summary>
        public int LineNo
        {
            get { return lineno; }
            set
            {
                if (value != lineno)
                {
                    lineno = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set PID
        /// </summary>
        public int PID
        {
            get { return pid; }
            set
            {
                if (value != pid)
                {
                    pid = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set WorkPlace
        /// </summary>
        public string WorkPlace
        {
            get { return workplace; }
            set
            {
                if (value != workplace)
                {
                    workplace = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }
        
        /// <summary>
        /// Get or set StartTime
        /// </summary>
        public int StartTime
        {
            get { return starttime; }
            set
            {
                if (value != starttime)
                {
                    starttime = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set EndTime
        /// </summary>
        public int EndTime
        {
            get { return endtime; }
            set
            {
                if (value != endtime)
                {
                    endtime = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set WorkTime
        /// </summary>
        public int WorkTime
        {
            get { return worktime; }
            set
            {
                if (value != worktime)
                {
                    worktime = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Memo
        /// </summary>
        public string Memo
        {
            get { return memo; }
            set
            {
                if (value != memo)
                {
                    memo = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set ProjectName
        /// </summary>
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                if (value != projectName)
                {
                    projectName = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set ProjectName
        /// </summary>
        public string ProjectCD
        {
            get { return projectCd; }
            set
            {
                if (value != projectCd)
                {
                    projectCd = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Delete Flag
        /// Use for delete row
        /// </summary>
        public bool DelFlag { get; set; }

        /// <summary>
        /// Checked
        /// </summary>
        public bool Checked { get; set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Work_D
        /// </summary>
        public T_Work_D()
            : base()
        {
            this.lineno = 0;
            this.workplace = string.Empty;
            this.starttime = 0;
            this.endtime = 0;
            this.worktime = 0;
            this.memo = string.Empty;
            this.projectName = string.Empty;
        }
        /// <summary>
        /// Contructor T_Work_H
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Work_D(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;
            this.hid = (int)dr["HID"];
            this.date = (DateTime)dr["Date"];
            this.lineno = (int)dr["LineNo"];
            this.pid = (int)dr["PID"];
            this.workplace = (string)dr["WorkPlace"];
            this.starttime = (int)dr["StartTime"];
            this.endtime = (int)dr["EndTime"];
            this.worktime = (int)dr["WorkTime"];
            this.memo = string.Format("{0}", dr["Memo"]);
        }
        #endregion
    }
}
