using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    ///Class T_WorkingCalendar_H Model
    /// </summary>
    [Serializable]
    public class T_WorkingCalendar_H : M_Base<T_WorkingCalendar_H>
    {
        #region Contanst

        /// <summary>
        /// Max length of CalendarCD
        /// </summary>
        public const int CALENDAR_CD_MAX_LENGTH = 10;
        /// <summary>
        /// Max length of CalendarName
        /// </summary>
        public const int CALENDAR_NAME_MAX_LENGTH = 100;

        #endregion

        #region Variant

        /// <summary>
        /// calendarCD
        /// </summary>
        private string calendarCD;

        /// <summary>
        /// calendarName
        /// </summary>
        private string calendarName;

        /// <summary>
        /// InitialDate
        /// </summary>
        private DateTime initialDate;

        /// <summary>
        /// AnnualWorkingDays
        /// </summary>
        private int annualWorkingDays;

        /// <summary>
        /// AnnualWorkingHours
        /// </summary>
        private int annualWorkingHours;

        /// <summary>
        /// AgreementFlag1
        /// </summary>
        private short agreementFlag1;

        /// <summary>
        /// agreementFlag2
        /// </summary>
        private short agreementFlag2;

        /// <summary>
        /// agreementFlag3
        /// </summary>
        private short agreementFlag3;

        /// <summary>
        /// agreementFlag4
        /// </summary>
        private short agreementFlag4;

        /// <summary>
        /// agreementFlag5
        /// </summary>
        private short agreementFlag5;

        /// <summary>
        /// agreementFlag6
        /// </summary>
        private short agreementFlag6;

        /// <summary>
        /// agreementFlag7
        /// </summary>
        private short agreementFlag7;

        /// <summary>
        /// agreementFlag8
        /// </summary>
        private short agreementFlag8;

        /// <summary>
        /// agreementFlag9
        /// </summary>
        private short agreementFlag9;

        /// <summary>
        /// agreementFlag10
        /// </summary>
        private short agreementFlag10;

        /// <summary>
        /// agreementFlag11
        /// </summary>
        private short agreementFlag11;

        /// <summary>
        /// agreementFlag12
        /// </summary>
        private short agreementFlag12;

        /// <summary>
        /// StatusFlag
        /// </summary>
        private short statusFlag;

        #endregion

        #region Property

        /// <summary>
        /// Get or set CalendarCD
        /// </summary>
        public string CalendarCD
        {
            get { return calendarCD; }
            set
            {
                if (value != calendarCD)
                {
                    calendarCD = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CalendarName
        /// </summary>
        public string CalendarName
        {
            get { return calendarName; }
            set
            {
                if (value != calendarName)
                {
                    calendarName = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set InitialDate
        /// </summary>
        public DateTime InitialDate
        {
            get { return initialDate; }
            set
            {
                if (value != initialDate)
                {
                    initialDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AnnualWorkingDays
        /// </summary>
        public int AnnualWorkingDays
        {
            get { return annualWorkingDays; }
            set
            {
                if (value != annualWorkingDays)
                {
                    annualWorkingDays = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AnnualWorkingHours
        /// </summary>
        public int AnnualWorkingHours
        {
            get { return annualWorkingHours; }
            set
            {
                if (value != annualWorkingHours)
                {
                    annualWorkingHours = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag1
        /// </summary>
        public short AgreementFlag1
        {
            get { return agreementFlag1; }
            set
            {
                if (value != agreementFlag1)
                {
                    agreementFlag1 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag2
        /// </summary>
        public short AgreementFlag2
        {
            get { return agreementFlag2; }
            set
            {
                if (value != agreementFlag2)
                {
                    agreementFlag2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag3
        /// </summary>
        public short AgreementFlag3
        {
            get { return agreementFlag3; }
            set
            {
                if (value != agreementFlag3)
                {
                    agreementFlag3 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag4
        /// </summary>
        public short AgreementFlag4
        {
            get { return agreementFlag4; }
            set
            {
                if (value != agreementFlag4)
                {
                    agreementFlag4 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag5
        /// </summary>
        public short AgreementFlag5
        {
            get { return agreementFlag5; }
            set
            {
                if (value != agreementFlag5)
                {
                    agreementFlag5 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag6
        /// </summary>
        public short AgreementFlag6
        {
            get { return agreementFlag6; }
            set
            {
                if (value != agreementFlag6)
                {
                    agreementFlag6 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag7
        /// </summary>
        public short AgreementFlag7
        {
            get { return agreementFlag7; }
            set
            {
                if (value != agreementFlag7)
                {
                    agreementFlag7 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag8
        /// </summary>
        public short AgreementFlag8
        {
            get { return agreementFlag8; }
            set
            {
                if (value != agreementFlag8)
                {
                    agreementFlag8 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag9
        /// </summary>
        public short AgreementFlag9
        {
            get { return agreementFlag9; }
            set
            {
                if (value != agreementFlag9)
                {
                    agreementFlag9 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag10
        /// </summary>
        public short AgreementFlag10
        {
            get { return agreementFlag10; }
            set
            {
                if (value != agreementFlag10)
                {
                    agreementFlag10 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag11
        /// </summary>
        public short AgreementFlag11
        {
            get { return agreementFlag11; }
            set
            {
                if (value != agreementFlag11)
                {
                    agreementFlag11 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AgreementFlag12
        /// </summary>
        public short AgreementFlag12
        {
            get { return agreementFlag12; }
            set
            {
                if (value != agreementFlag12)
                {
                    agreementFlag12 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set StatusFlag
        /// </summary>
        public short StatusFlag
        {
            get { return statusFlag; }
            set
            {
                if (value != statusFlag)
                {
                    statusFlag = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_WorkingCalendar_H
        /// </summary>
        public T_WorkingCalendar_H()
            : base()
        {
            
        }

        /// <summary>
        /// Contructor T_WorkingCalendar_H
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_WorkingCalendar_H(DbDataReader dr)
            : base(dr)
        {
            ISecurity sec = Security.Instance;
            this.CalendarCD = (string)dr["CalendarCD"];
            this.CalendarName = (string)dr["CalendarName"];
            this.InitialDate = (DateTime)dr["InitialDate"];
            this.AnnualWorkingDays = (int)dr["AnnualWorkingDays"];
            this.AnnualWorkingHours = (int)dr["AnnualWorkingHours"];

            this.AgreementFlag1 = short.Parse(string.Format("{0}", dr["AgreementFlag1"]));
            this.AgreementFlag2 = short.Parse(string.Format("{0}", dr["AgreementFlag2"]));
            this.AgreementFlag3 = short.Parse(string.Format("{0}", dr["AgreementFlag3"]));
            this.AgreementFlag4 = short.Parse(string.Format("{0}", dr["AgreementFlag4"]));
            this.AgreementFlag5 = short.Parse(string.Format("{0}", dr["AgreementFlag5"]));
            this.AgreementFlag6 = short.Parse(string.Format("{0}", dr["AgreementFlag6"]));
            this.AgreementFlag7 = short.Parse(string.Format("{0}", dr["AgreementFlag7"]));
            this.AgreementFlag8 = short.Parse(string.Format("{0}", dr["AgreementFlag8"]));
            this.AgreementFlag9 = short.Parse(string.Format("{0}", dr["AgreementFlag9"]));
            this.AgreementFlag10 = short.Parse(string.Format("{0}", dr["AgreementFlag10"]));
            this.AgreementFlag11 = short.Parse(string.Format("{0}", dr["AgreementFlag11"]));
            this.AgreementFlag12 = short.Parse(string.Format("{0}", dr["AgreementFlag12"]));
            this.StatusFlag = short.Parse(string.Format("{0}", dr["StatusFlag"]));

        }

        #endregion
    }
}
