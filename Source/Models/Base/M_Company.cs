using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class M_Company Model
    /// </summary>
    [Serializable]
    public class M_Company : M_Base<M_Company>
    {
        #region Contanst

        /// <summary>
        /// Max length of CompanyName1
        /// </summary>
        public const int COMPANY_NAME1_MAX_LENGTH = 100;

        /// <summary>
        /// Max length of CompanyName2
        /// </summary>
        public const int COMPANY_NAME2_MAX_LENGTH = 100;

        /// <summary>
        /// Max length of CompanyAddress1
        /// </summary>
        public const int COMPANY_ADDRESS1_MAX_LENGTH = 150;

        /// <summary>
        /// Max length of CompanyAddress2
        /// </summary>
        public const int COMPANY_ADDRESS2_MAX_LENGTH = 150;

        /// <summary>
        /// Max length of CompanyAddress3
        /// </summary>
        public const int COMPANY_ADDRESS3_MAX_LENGTH = 150;

        /// <summary>
        /// Max length of CompanyAddress4
        /// </summary>
        public const int COMPANY_ADDRESS4_MAX_LENGTH = 150;

        /// <summary>
        /// Max length of CompanyAddress5
        /// </summary>
        public const int COMPANY_ADDRESS5_MAX_LENGTH = 150;

        /// <summary>
        /// Max length of CompanyAddress6
        /// </summary>
        public const int COMPANY_ADDRESS6_MAX_LENGTH = 150;

        /// <summary>
        /// Max length of Tel
        /// </summary>
        public const int TEL_MAX_LENGTH = 20;

        /// <summary>
        /// Max length of Tel2
        /// </summary>
        public const int TEL2_MAX_LENGTH = 20;

        /// <summary>
        /// Max length of FAX
        /// </summary>
        public const int FAX_MAX_LENGTH = 20;

        /// <summary>
        /// Max length of EmailAddress
        /// </summary>
        public const int EMAIL_ADDRESS_MAX_LENGTH = 50;

        /// <summary>
        /// Max length of TAXCode
        /// </summary>
        public const int TAX_CODE_MAX_LENGTH = 20;

        /// <summary>
        /// Max length of CompanyBank
        /// </summary>
        public const int COMPANY_BANK_MAX_LENGTH = 100;

        /// <summary>
        /// Max length of AccountCode
        /// </summary>
        public const int ACCOUNT_CODE_MAX_LENGTH = 20;

        /// <summary>
        /// Max length of Represent
        /// </summary>
        public const int REPRESENT_MAX_LENGTH = 50;

        /// <summary>
        /// Max length of Position
        /// </summary>
        public const int POSITION_MAX_LENGTH = 50;

        /// <summary>
        /// Max length of Position2
        /// </summary>
        public const int POSITION2_MAX_LENGTH = 50;
        #endregion

        #region Variable

        /// <summary>
        /// CompanyName1
        /// </summary>
        public string companyName1;

        /// <summary>
        /// CompanyName2
        /// </summary>
        public string companyName2;

        /// <summary>
        /// CompanyAddress1
        /// </summary>
        public string companyAddress1;

        /// <summary>
        /// CompanyAddress2
        /// </summary>
        public string companyAddress2;

        /// <summary>
        /// CompanyAddress3
        /// </summary>
        public string companyAddress3;

        /// <summary>
        /// CompanyAddress4
        /// </summary>
        public string companyAddress4;

        /// <summary>
        /// CompanyAddress5
        /// </summary>
        public string companyAddress5;

        /// <summary>
        /// CompanyAddress6
        /// </summary>
        public string companyAddress6;

        /// <summary>
        /// Tel
        /// </summary>
        public string tel;

        /// <summary>
        /// Tel2
        /// </summary>
        public string tel2;

        /// <summary>
        /// FAX
        /// </summary>
        public string fax;

        /// <summary>
        /// EmailAddress
        /// </summary>
        public string emailAddress;

        /// <summary>
        /// TAX Code
        /// </summary>
        public string taxCode;

        /// <summary>
        /// CompanyBank
        /// </summary>
        public string companyBank;

        /// <summary>
        /// AccountCode
        /// </summary>
        public string accountCode;

        /// <summary>
        /// Represent
        /// </summary>
        public string represent;

        /// <summary>
        /// Position
        /// </summary>
        public string position;

        /// <summary>
        /// Position2
        /// </summary>
        public string position2;

        #endregion

        #region Property

        /// <summary>
        /// Get or set companyName1
        /// </summary>
        public string CompanyName1
        {
            get { return companyName1; }
            set
            {
                if (value != companyName1)
                {
                    companyName1 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set companyName2
        /// </summary>
        public string CompanyName2
        {
            get { return companyName2; }
            set
            {
                if (value != companyName2)
                {
                    companyName2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CompanyAddress1
        /// </summary>
        public string CompanyAddress1
        {
            get { return companyAddress1; }
            set
            {
                if (value != companyAddress1)
                {
                    companyAddress1 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CompanyAddress2
        /// </summary>
        public string CompanyAddress2
        {
            get { return companyAddress2; }
            set
            {
                if (value != companyAddress2)
                {
                    companyAddress2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CompanyAddress3
        /// </summary>
        public string CompanyAddress3
        {
            get { return companyAddress3; }
            set
            {
                if (value != companyAddress3)
                {
                    companyAddress3 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CompanyAddress4
        /// </summary>
        public string CompanyAddress4
        {
            get { return companyAddress4; }
            set
            {
                if (value != companyAddress4)
                {
                    companyAddress4 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CompanyAddress5
        /// </summary>
        public string CompanyAddress5
        {
            get { return companyAddress5; }
            set
            {
                if (value != companyAddress5)
                {
                    companyAddress5 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CompanyAddress6
        /// </summary>
        public string CompanyAddress6
        {
            get { return companyAddress6; }
            set
            {
                if (value != companyAddress6)
                {
                    companyAddress6 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Tel
        /// </summary>
        public string Tel
        {
            get { return tel; }
            set
            {
                if (value != tel)
                {
                    tel = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Tel2
        /// </summary>
        public string Tel2
        {
            get { return tel2; }
            set
            {
                if (value != tel2)
                {
                    tel2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set FAX
        /// </summary>
        public string FAX
        {
            get { return fax; }
            set
            {
                if (value != fax)
                {
                    fax = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set EmailAddress
        /// </summary>
        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                if (value != emailAddress)
                {
                    emailAddress = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set TaxCode
        /// </summary>
        public string TAXCode
        {
            get { return taxCode; }
            set
            {
                if (value != taxCode)
                {
                    taxCode = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CompanyBank
        /// </summary>
        public string CompanyBank
        {
            get { return companyBank; }
            set
            {
                if (value != companyBank)
                {
                    companyBank = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AccountCode
        /// </summary>
        public string AccountCode
        {
            get { return accountCode; }
            set
            {
                if (value != accountCode)
                {
                    accountCode = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Represent
        /// </summary>
        public string Represent
        {
            get { return represent; }
            set
            {
                if (value != represent)
                {
                    represent = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Position
        /// </summary>
        public string Position
        {
            get { return position; }
            set
            {
                if (value != position)
                {
                    position = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Position
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
        
        #endregion

        #region Contructor
            /// <summary>
        /// Contructor M_Company
        /// </summary>
        public M_Company()
            : base()
        {

        }

        /// <summary>
        /// Contructor M_Company
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public M_Company(DbDataReader dr)
            : base(dr)
        {
            this.companyName1 = (string)dr["CompanyName1"];
            this.companyName2 = (string)dr["CompanyName2"];
            this.companyAddress1 = (string)dr["CompanyAddress1"];
            this.companyAddress2 = (string)dr["CompanyAddress2"];
            this.companyAddress3 = (string)dr["CompanyAddress3"];
            this.companyAddress4 = (string)dr["CompanyAddress4"];
            this.companyAddress5 = (string)dr["CompanyAddress5"];
            this.companyAddress6 = (string)dr["CompanyAddress6"];
            this.tel = (string)dr["Tel"];
            this.tel2 = (string)dr["Tel2"];
            this.fax = (string)dr["FAX"];
            this.emailAddress = (string)dr["EmailAddress"];
            this.taxCode = (string)dr["TAXCode"];
            this.companyBank = (string)dr["CompanyBank"];
            this.accountCode = (string)dr["AccountCode"];
            this.represent = (string)dr["Represent"];
            this.position = (string)dr["Position"];
            this.position2 = (string)dr["Position2"];
        }
        #endregion

    }
}
