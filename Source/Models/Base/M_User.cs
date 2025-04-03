using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class M_User Model
    /// </summary>
    [Serializable]
    public class M_User : M_Base<M_User>
    {
        #region Contanst

        /// <summary>
        /// Max length of UserCD
        /// </summary>
        public const int USER_CODE_MAX_LENGTH = 10;
        /// <summary>
        /// Max length of UserName1
        /// </summary>
        public const int USER_NAME_1_MAX_LENGTH = 50;
        /// <summary>
        /// Max length of UserName2
        /// </summary>
        public const int USER_NAME_2_MAX_LENGTH = 15;
        /// <summary>
        /// Max length of Position1
        /// </summary>
        public const int POSITION_1_MAX_LENGTH = 60;
        /// <summary>
        /// Max length of Position2
        /// </summary>
        public const int POSITION_2_MAX_LENGTH = 60;
        /// <summary>
        /// Max length of LoginID
        /// </summary>
        public const int LOGIN_ID_MAX_LENGTH = 30;
        /// <summary>
        /// Max length of Password
        /// </summary>
        public const int PASSWORD_MAX_LENGTH = 15;
        /// <summary>
        /// Max length of UserCD to show
        /// </summary>
        public const int MAX_USER_CODE_SHOW = 4;
        /// <summary>
        /// ADMIN ISV CODE
        /// </summary>
        public const string ISV_ADMIN_CODE = "0000000000";

        /// <summary>
        /// Max length of EmailAddress
        /// </summary>
        public const int MAIL_ADDRESS_MAX_LENGTH = 100;

        #endregion

        #region Variant

        /// <summary>
        /// UserCD
        /// </summary>
        private string userCD;

        /// <summary>
        /// loginID
        /// </summary>
        private string loginID;

        /// <summary>
        /// userName1
        /// </summary>
        private string userName1;

        /// <summary>
        /// userName2
        /// </summary>
        private string userName2;

        /// <summary>
        /// position1
        /// </summary>
        private string position1;

        /// <summary>
        /// position2
        /// </summary>
        private string position2;

        /// <summary>
        /// password
        /// </summary>
        private string password;

        /// <summary>
        /// department ID
        /// </summary>
        private int departmentID;

        /// <summary>
        /// group ID
        /// </summary>
        private int groupID;

        /// <summary>
        /// MailAddress
        /// </summary>
        public string mailAddress;

        /// <summary>
        /// status Flag
        /// </summary>
        private short statusFlag;

        #endregion

        #region Property

        /// <summary>
        /// Get or set userCD
        /// </summary>
        public string UserCD
        {
            get { return userCD; }
            set
            {
                if (value != userCD)
                {
                    userCD = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set LoginID
        /// </summary>
        public string LoginID
        {
            get { return loginID; }
            set
            {
                if (value != loginID)
                {
                    loginID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set UserName1
        /// </summary>
        public string UserName1
        {
            get { return userName1; }
            set
            {
                if (value != userName1)
                {
                    userName1 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set UserName2
        /// </summary>
        public string UserName2
        {
            get { return userName2; }
            set
            {
                if (value != userName2)
                {
                    userName2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Position1
        /// </summary>
        public string Position1
        {
            get { return position1; }
            set
            {
                if (value != position1)
                {
                    position1 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Position2
        /// </summary>
        public string Position2
        {
            get { return position2; }
            set
            {
                if (value != position2)
                {
                    position2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Password
        /// </summary>
        public string Password
        {
            get { return password; }
            set
            {
                if (value != password)
                {
                    password = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Group ID
        /// </summary>
        public int GroupID
        {
            get { return groupID; }
            set
            {
                if (value != groupID)
                {
                    groupID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Department ID
        /// </summary>
        public int DepartmentID
        {
            get { return departmentID; }
            set
            {
                if (value != departmentID)
                {
                    departmentID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set EmailAddress
        /// </summary>
        public string MailAddress
        {
            get { return mailAddress; }
            set
            {
                if (value != mailAddress)
                {
                    mailAddress = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Status Flag
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
        /// Contructor M_User
        /// </summary>
        public M_User()
            : base()
        {

        }

        /// <summary>
        /// Contructor M_User
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public M_User(DbDataReader dr)
            : base(dr)
        {
            ISecurity sec = Security.Instance;

            this.UserCD =(string)dr["UserCD"];
            this.loginID = (string)dr["LoginID"];
            this.userName1 = (string)dr["UserName1"];
            this.userName2 = (string)dr["UserName2"];
            this.position1 = (string)dr["Position1"];
            this.position2 = (string)dr["Position2"];
            this.password =sec.Decrypt((string)dr["Password"]);
            this.groupID = (int)dr["GroupID"];
            this.departmentID = (int)dr["DepartmentID"];
            this.mailAddress = (string)dr["MailAddress"];
            this.statusFlag = short.Parse(string.Format("{0}", dr["StatusFlag"]));
        }

        #endregion
    }
}
