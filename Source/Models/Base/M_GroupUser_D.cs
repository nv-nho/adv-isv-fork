using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class M_GroupUser_D
    /// Create  :isv.thuy
    /// Date    :30/07/2014
    /// </summary>
    [Serializable]
    public class M_GroupUser_D
    {
        #region Variable

        public int GroupID { get; set; }        
        public int FormID { get; set; }
        public string FormName { get; set; }

        private short authorityFlag1;
        private short authorityFlag2;
        private short authorityFlag3;
        private short authorityFlag4;
        private short authorityFlag5;
        private short authorityFlag6;
        private short authorityFlag7;
        private short authorityFlag8;
        private short authorityFlag9;
        private short authorityFlag10;
        private short authorityFlag11;
        private short authorityFlag12;

        /// <summary>
        /// Status
        /// </summary>
        private DataStatus _status;

        #endregion

        #region Property

        /// <summary>
        /// Status
        /// </summary>
        public DataStatus Status
        {
            get
            {
                return this._status;
            }
        }

        public short AuthorityFlag1
        {
            get { return authorityFlag1; }
            set
            {
                if (value != authorityFlag1)
                {
                    authorityFlag1 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        public short AuthorityFlag2
        {
            get { return authorityFlag2; }
            set
            {
                if (value != authorityFlag2)
                {
                    authorityFlag2 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        public short AuthorityFlag3
        {
            get { return authorityFlag3; }
            set
            {
                if (value != authorityFlag3)
                {
                    authorityFlag3 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        public short AuthorityFlag4
        {
            get { return authorityFlag4; }
            set
            {
                if (value != authorityFlag4)
                {
                    authorityFlag4 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        public short AuthorityFlag5
        {
            get { return authorityFlag5; }
            set
            {
                if (value != authorityFlag5)
                {
                    authorityFlag5 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        public short AuthorityFlag6
        {
            get { return authorityFlag6; }
            set
            {
                if (value != authorityFlag6)
                {
                    authorityFlag6 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        public short AuthorityFlag7
        {
            get { return authorityFlag7; }
            set
            {
                if (value != authorityFlag7)
                {
                    authorityFlag7 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        public short AuthorityFlag8
        {
            get { return authorityFlag8; }
            set
            {
                if (value != authorityFlag8)
                {
                    authorityFlag8 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        public short AuthorityFlag9
        {
            get { return authorityFlag9; }
            set
            {
                if (value != authorityFlag9)
                {
                    authorityFlag9 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        public short AuthorityFlag10
        {
            get { return authorityFlag10; }
            set
            {
                if (value != authorityFlag10)
                {
                    authorityFlag10 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }
        public short AuthorityFlag11
        {
            get { return authorityFlag11; }
            set
            {
                if (value != authorityFlag11)
                {
                    authorityFlag11 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }
        public short AuthorityFlag12
        {
            get { return authorityFlag12; }
            set
            {
                if (value != authorityFlag12)
                {
                    authorityFlag12 = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor M_GroupUser_M
        /// </summary>
        public M_GroupUser_D()
            : base()
        {
            this.authorityFlag1 = 0;
            this.authorityFlag2 = 0;
            this.authorityFlag3 = 0;
            this.authorityFlag4 = 0;
            this.authorityFlag5 = 0;
            this.authorityFlag6 = 0;
            this.authorityFlag7 = 0;
            this.authorityFlag8 = 0;
            this.authorityFlag9 = 0;
            this.authorityFlag10 = 0;
            this.authorityFlag11 = 0;
            this.authorityFlag12 = 0;
        }
        
        public M_GroupUser_D(DbDataReader dr)
        {
            this.GroupID = int.Parse(string.Format("{0}", dr["GroupID"]));            
            this.FormID = int.Parse(string.Format("{0}", dr["FormID"]));
            this.FormName = string.Format("{0}", dr["FormName"]);

            this.authorityFlag1 = short.Parse(string.Format("{0}", dr["AuthorityFlag1"]));
            this.authorityFlag2 = short.Parse(string.Format("{0}", dr["AuthorityFlag2"]));
            this.authorityFlag3 = short.Parse(string.Format("{0}", dr["AuthorityFlag3"]));
            this.authorityFlag4 = short.Parse(string.Format("{0}", dr["AuthorityFlag4"]));
            this.authorityFlag5 = short.Parse(string.Format("{0}", dr["AuthorityFlag5"]));
            this.authorityFlag6 = short.Parse(string.Format("{0}", dr["AuthorityFlag6"]));
            this.authorityFlag7 = short.Parse(string.Format("{0}", dr["AuthorityFlag7"]));
            this.authorityFlag8 = short.Parse(string.Format("{0}", dr["AuthorityFlag8"]));
            this.authorityFlag9 = short.Parse(string.Format("{0}", dr["AuthorityFlag9"]));
            this.authorityFlag10 = short.Parse(string.Format("{0}", dr["AuthorityFlag10"]));
            this.authorityFlag11 = short.Parse(string.Format("{0}", dr["AuthorityFlag11"]));
            this.authorityFlag12 = short.Parse(string.Format("{0}", dr["AuthorityFlag12"]));
        }

        #endregion

    }
}
