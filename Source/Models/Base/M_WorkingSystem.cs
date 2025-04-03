using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class M_WorkingSystem
    /// Create  :isv.than
    /// Date    :20/09/2017
    /// </summary>
    [Serializable]
    public class M_WorkingSystem : M_Base<M_WorkingSystem>
    {
        #region Constant

        public const int WORKING_SYSTEM_CODE_SHOW_MAX_LENGTH = 2;
        public const int WORKING_SYSTEM_CODE_DB_MAX_LENGTH = 2;
        public const int WORKING_SYSTEM_NAME_MAX_LENGTH = 100;
        public const int WORKING_SYSTEM_NAME2_MAX_LENGTH = 50;
        public const string GROUP_ADMIN = "0001";

        #endregion

        #region Variable

        public string WorkingSystemCD { get; set; }

        private string workingSystemName;
        private string workingSystemName2;

        private int workingType;
        private int? working_Start;
        private int? working_End;
        private int? working_End_2;

        private int? overTime1_Start;
        private int? overTime1_End;
        private int? overTime2_Start;
        private int? overTime2_End;
        private int? overTime3_Start;
        private int? overTime3_End;
        private int? overTime4_Start;
        private int? overTime4_End;
        private int? overTime5_Start;
        private int? overTime5_End;

        private int breakType;
        private int? break1_Start;
        private int? break1_End;
        private int? break2_Start;
        private int? break2_End;
        private int? break3_Start;
        private int? break3_End;
        private int? break4_Start;
        private int? break4_End;

        private int? dateSwitchTime;
        private int? first_End;
        private int? latter_Start;
        private int? allOff_Hours;
        private int? firstOff_Hours;
        private int? latterOff_Hours;

        #endregion

        #region Property

        public string WorkingSystemName
        {
            get { return workingSystemName; }
            set
            {
                if (value != workingSystemName)
                {
                    workingSystemName = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public string WorkingSystemName2
        {
            get { return workingSystemName2; }
            set
            {
                if (value != workingSystemName2)
                {
                    workingSystemName2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int WorkingType
        {
            get { return workingType; }
            set
            {
                if (value != workingType)
                {
                    workingType = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Working_Start
        {
            get { return working_Start; }
            set
            {
                if (value != working_Start)
                {
                    working_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Working_End
        {
            get { return working_End; }
            set
            {
                if (value != working_End)
                {
                    working_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Working_End_2
        {
            get { return working_End_2; }
            set
            {
                if (value != working_End_2)
                {
                    working_End_2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime1_Start
        {
            get { return overTime1_Start; }
            set
            {
                if (value != overTime1_Start)
                {
                    overTime1_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime1_End
        {
            get { return overTime1_End; }
            set
            {
                if (value != overTime1_End)
                {
                    overTime1_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime2_Start
        {
            get { return overTime2_Start; }
            set
            {
                if (value != overTime2_Start)
                {
                    overTime2_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime2_End
        {
            get { return overTime2_End; }
            set
            {
                if (value != overTime2_End)
                {
                    overTime2_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime3_Start
        {
            get { return overTime3_Start; }
            set
            {
                if (value != overTime3_Start)
                {
                    overTime3_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime3_End
        {
            get { return overTime3_End; }
            set
            {
                if (value != overTime3_End)
                {
                    overTime3_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime4_Start
        {
            get { return overTime4_Start; }
            set
            {
                if (value != overTime4_Start)
                {
                    overTime4_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime4_End
        {
            get { return overTime4_End; }
            set
            {
                if (value != overTime4_End)
                {
                    overTime4_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime5_Start
        {
            get { return overTime5_Start; }
            set
            {
                if (value != overTime5_Start)
                {
                    overTime5_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTime5_End
        {
            get { return overTime5_End; }
            set
            {
                if (value != overTime5_End)
                {
                    overTime5_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int BreakType
        {
            get { return breakType; }
            set
            {
                if (value != breakType)
                {
                    breakType = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Break1_Start
        {
            get { return break1_Start; }
            set
            {
                if (value != break1_Start)
                {
                    break1_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Break1_End
        {
            get { return break1_End; }
            set
            {
                if (value != break1_End)
                {
                    break1_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Break2_Start
        {
            get { return break2_Start; }
            set
            {
                if (value != break2_Start)
                {
                    break2_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Break2_End
        {
            get { return break2_End; }
            set
            {
                if (value != break2_End)
                {
                    break2_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Break3_Start
        {
            get { return break3_Start; }
            set
            {
                if (value != break3_Start)
                {
                    break3_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Break3_End
        {
            get { return break3_End; }
            set
            {
                if (value != break3_End)
                {
                    break3_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Break4_Start
        {
            get { return break4_Start; }
            set
            {
                if (value != break4_Start)
                {
                    break4_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Break4_End
        {
            get { return break4_End; }
            set
            {
                if (value != break4_End)
                {
                    break4_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? DateSwitchTime
        {
            get { return dateSwitchTime; }
            set
            {
                if (value != dateSwitchTime)
                {
                    dateSwitchTime = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? First_End
        {
            get { return first_End; }
            set
            {
                if (value != first_End)
                {
                    first_End = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? Latter_Start
        {
            get { return latter_Start; }
            set
            {
                if (value != latter_Start)
                {
                    latter_Start = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? AllOff_Hours
        {
            get { return allOff_Hours; }
            set
            {
                if (value != allOff_Hours)
                {
                    allOff_Hours = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? FirstOff_Hours
        {
            get { return firstOff_Hours; }
            set
            {
                if (value != firstOff_Hours)
                {
                    firstOff_Hours = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? LatterOff_Hours
        {
            get { return latterOff_Hours; }
            set
            {
                if (value != latterOff_Hours)
                {
                    latterOff_Hours = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }



        #endregion

        #region Contructor

        /// <summary>
        /// Contructor M_WorkingSystem
        /// </summary>
        public M_WorkingSystem()
            : base()
        {

        }

        public M_WorkingSystem(DbDataReader dr)
            : base(dr)
        {
            //this.WorkingSystemCD = Utilities.EditDataUtil.ToFixCodeShow((string)dr["WorkingSystemCD"], M_WorkingSystem.WORKING_SYSTEM_CODE_SHOW_MAX_LENGTH); 
            this.WorkingSystemCD = (string)dr["WorkingSystemCD"];
            this.workingSystemName = (string)dr["WorkingSystemName"];
            this.workingSystemName2 = (string)dr["WorkingSystemName2"];

            this.workingType = int.Parse(dr["WorkingType"].ToString());

            if (dr["Working_Start"] != DBNull.Value)
            {
                this.Working_Start = Convert.ToInt32(dr["Working_Start"]);
            }
            else
            {
                this.Working_Start = null;
            }
            if (dr["Working_End"] != DBNull.Value)
            {
                this.Working_End = Convert.ToInt32(dr["Working_End"]);
            }
            else
            {
                this.Working_End = null;
            }

            if (dr["Working_End_2"] != DBNull.Value)
            {
                this.Working_End_2 = Convert.ToInt32(dr["Working_End_2"]);
            }
            else
            {
                this.Working_End_2 = null;
            }

            if (dr["OverTime1_Start"] != DBNull.Value)
            {
                this.OverTime1_Start = Convert.ToInt32(dr["OverTime1_Start"]);
            }
            else
            {
                this.OverTime1_Start = null;
            }
            if (dr["OverTime1_End"] != DBNull.Value)
            {
                this.OverTime1_End = Convert.ToInt32(dr["OverTime1_End"]);
            }
            else
            {
                this.OverTime1_End = null;
            }
            if (dr["OverTime2_Start"] != DBNull.Value)
            {
                this.OverTime2_Start = Convert.ToInt32(dr["OverTime2_Start"]);
            }
            else
            {
                this.OverTime2_Start = null;
            }
            if (dr["OverTime2_End"] != DBNull.Value)
            {
                this.OverTime2_End = Convert.ToInt32(dr["OverTime2_End"]);
            }
            else
            {
                this.OverTime2_End = null;
            }
            if (dr["OverTime3_Start"] != DBNull.Value)
            {
                this.OverTime3_Start = Convert.ToInt32(dr["OverTime3_Start"]);
            }
            else
            {
                this.OverTime3_Start = null;
            }
            if (dr["OverTime3_End"] != DBNull.Value)
            {
                this.OverTime3_End = Convert.ToInt32(dr["OverTime3_End"]);
            }
            else
            {
                this.OverTime3_End = null;
            }
            if (dr["OverTime4_Start"] != DBNull.Value)
            {
                this.OverTime4_Start = Convert.ToInt32(dr["OverTime4_Start"]);
            }
            else
            {
                this.OverTime4_Start = null;
            }
            if (dr["OverTime4_End"] != DBNull.Value)
            {
                this.OverTime4_End = Convert.ToInt32(dr["OverTime4_End"]);
            }
            else
            {
                this.OverTime4_End = null;
            }
            if (dr["OverTime5_Start"] != DBNull.Value)
            {
                this.OverTime5_Start = Convert.ToInt32(dr["OverTime5_Start"]);
            }
            else
            {
                this.OverTime5_Start = null;
            }
            if (dr["OverTime5_End"] != DBNull.Value)
            {
                this.OverTime5_End = Convert.ToInt32(dr["OverTime5_End"]);
            }
            else
            {
                this.OverTime5_End = null;
            }

            this.breakType = int.Parse(dr["BreakType"].ToString());

            if (dr["Break1_Start"] != DBNull.Value)
            {
                this.Break1_Start = Convert.ToInt32(dr["Break1_Start"]);
            }
            else
            {
                this.Break1_Start = null;
            }
            if (dr["Break1_End"] != DBNull.Value)
            {
                this.Break1_End = Convert.ToInt32(dr["Break1_End"]);
            }
            else
            {
                this.Break1_End = null;
            }
            if (dr["Break2_Start"] != DBNull.Value)
            {
                this.Break2_Start = Convert.ToInt32(dr["Break2_Start"]);
            }
            else
            {
                this.Break2_Start = null;
            }
            if (dr["Break2_End"] != DBNull.Value)
            {
                this.Break2_End = Convert.ToInt32(dr["Break2_End"]);
            }
            else
            {
                this.Break2_End = null;
            }
            if (dr["Break3_Start"] != DBNull.Value)
            {
                this.Break3_Start = Convert.ToInt32(dr["Break3_Start"]);
            }
            else
            {
                this.Break3_Start = null;
            }
            if (dr["Break3_End"] != DBNull.Value)
            {
                this.Break3_End = Convert.ToInt32(dr["Break3_End"]);
            }
            else
            {
                this.Break3_End = null;
            }
            if (dr["Break4_Start"] != DBNull.Value)
            {
                this.Break4_Start = Convert.ToInt32(dr["Break4_Start"]);
            }
            else
            {
                this.Break4_Start = null;
            }
            if (dr["Break4_End"] != DBNull.Value)
            {
                this.Break4_End = Convert.ToInt32(dr["Break4_End"]);
            }
            else
            {
                this.Break4_End = null;
            }

            if(dr["DateSwitchTime"].ToString()!=string.Empty){
                this.dateSwitchTime = int.Parse(dr["DateSwitchTime"].ToString());
            }

            if (dr["First_End"].ToString() != string.Empty)
            {
                this.first_End = int.Parse(dr["First_End"].ToString());
            }

            if (dr["Latter_Start"].ToString() != string.Empty)
            {
                this.latter_Start = int.Parse(dr["Latter_Start"].ToString());
            }

            if (dr["AllOff_Hours"].ToString() != string.Empty)
            {
                this.allOff_Hours = int.Parse(dr["AllOff_Hours"].ToString());
            }

            if (dr["FirstOff_Hours"].ToString() != string.Empty)
            {
                this.firstOff_Hours = int.Parse(dr["FirstOff_Hours"].ToString());
            }
         
            if (dr["LatterOff_Hours"].ToString() != string.Empty)
            {
                this.latterOff_Hours = int.Parse(dr["LatterOff_Hours"].ToString());
            }

        //    this.dateSwitchTime = dr["DateSwitchTime"].ToString()!=string.Empty? int.Parse(dr["DateSwitchTime"].ToString());
            //this.first_End = dr["DateSwitchTime"]!=null? int.Parse(dr["First_End"].ToString()):0;
            //this.latter_Start = dr["DateSwitchTime"]!=null? int.Parse(dr["Latter_Start"].ToString()):0;
            //this.allOff_Hours = dr["DateSwitchTime"]!=null? int.Parse(dr["AllOff_Hours"].ToString()):0;
            //this.firstOff_Hours = dr["DateSwitchTime"]!=null? int.Parse(dr["FirstOff_Hours"].ToString()):0;
            //this.latterOff_Hours = dr["DateSwitchTime"] != null ? int.Parse(dr["LatterOff_Hours"].ToString()) : 0;

        }
        #endregion
    }
}
