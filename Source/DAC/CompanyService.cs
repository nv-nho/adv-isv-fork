using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OMS.Models;

namespace OMS.DAC
{
    /// <summary>
    /// Company service
    /// </summary>
    public class CompanyService:BaseService
    {

         #region Constructor
       
        private CompanyService():base()
        {

        }

        public CompanyService(DB db):base(db)
        {
        }

        #endregion

        #region Get Data

        /// <summary>
        /// Get data
        /// </summary>
        /// <returns>M_User</returns>
        public M_Company GetData()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
           
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("   ID");
            cmdText.AppendLine("  ,CompanyName1");
            cmdText.AppendLine("  ,CompanyName2");
            cmdText.AppendLine("  ,CompanyAddress1");
            cmdText.AppendLine("  ,CompanyAddress2");
            cmdText.AppendLine("  ,CompanyAddress3");
            cmdText.AppendLine("  ,CompanyAddress4");
            cmdText.AppendLine("  ,CompanyAddress5");
            cmdText.AppendLine("  ,CompanyAddress6");
            cmdText.AppendLine("  ,Tel");
            cmdText.AppendLine("  ,Tel2");
            cmdText.AppendLine("  ,FAX");
            cmdText.AppendLine("  ,EmailAddress");
            cmdText.AppendLine("  ,TAXCode");
            cmdText.AppendLine("  ,CompanyBank");
            cmdText.AppendLine("  ,AccountCode");
            cmdText.AppendLine("  ,Represent");
            cmdText.AppendLine("  ,Position");
            cmdText.AppendLine("  ,Position2");
            cmdText.AppendLine("  ,CreateDate");
            cmdText.AppendLine("  ,CreateUID");
            cmdText.AppendLine("  ,UpdateDate");
            cmdText.AppendLine("  ,UpdateUID");

            cmdText.AppendLine(" FROM dbo.M_Company");

            //Para
            Hashtable paras = new Hashtable();
            return this.db.Find1<M_Company>(cmdText.ToString(), paras);
        }

        #endregion

        #region Insert

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="company">M_Company</param>
        /// <returns></returns>
        public int Insert(M_Company company)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO dbo.M_Company");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  CompanyName1");
            cmdText.AppendLine(" ,CompanyName2");
            cmdText.AppendLine(" ,CompanyAddress1");
            cmdText.AppendLine(" ,CompanyAddress2");
            cmdText.AppendLine(" ,CompanyAddress3");
            cmdText.AppendLine(" ,CompanyAddress4");
            cmdText.AppendLine(" ,CompanyAddress5");
            cmdText.AppendLine(" ,CompanyAddress6");
            cmdText.AppendLine(" ,Tel");
            cmdText.AppendLine(" ,Tel2");
            cmdText.AppendLine(" ,FAX");
            cmdText.AppendLine(" ,EmailAddress");
            cmdText.AppendLine(" ,TAXCode");
            cmdText.AppendLine(" ,CompanyBank");
            cmdText.AppendLine(" ,AccountCode");
            cmdText.AppendLine(" ,Represent");
            cmdText.AppendLine(" ,Position");
            cmdText.AppendLine(" ,Position2");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_CompanyName1");
            cmdText.AppendLine(" ,@IN_CompanyName2");
            cmdText.AppendLine(" ,@IN_CompanyAddress1");
            cmdText.AppendLine(" ,@IN_CompanyAddress2");
            cmdText.AppendLine(" ,@IN_CompanyAddress3");
            cmdText.AppendLine(" ,@IN_CompanyAddress4");
            cmdText.AppendLine(" ,@IN_CompanyAddress5");
            cmdText.AppendLine(" ,@IN_CompanyAddress6");
            cmdText.AppendLine(" ,@IN_Tel");
            cmdText.AppendLine(" ,@IN_Tel2");
            cmdText.AppendLine(" ,@IN_FAX");
            cmdText.AppendLine(" ,@IN_EmailAddress");
            cmdText.AppendLine(" ,@IN_TAXCode");
            cmdText.AppendLine(" ,@IN_CompanyBank");
            cmdText.AppendLine(" ,@IN_AccountCode");
            cmdText.AppendLine(" ,@IN_Represent");
            cmdText.AppendLine(" ,@IN_Position");
            cmdText.AppendLine(" ,@IN_Position2");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras,"IN_CompanyName1", company.CompanyName1);
            base.AddParam(paras,"IN_CompanyName2", company.CompanyName2);
            base.AddParam(paras,"IN_CompanyAddress1", company.CompanyAddress1);
            base.AddParam(paras,"IN_CompanyAddress2", company.CompanyAddress2);
            base.AddParam(paras,"IN_CompanyAddress3", company.CompanyAddress3);
            base.AddParam(paras,"IN_CompanyAddress4", company.CompanyAddress4);
            base.AddParam(paras,"IN_CompanyAddress5", company.CompanyAddress5);
            base.AddParam(paras,"IN_CompanyAddress6", company.CompanyAddress6);
            base.AddParam(paras,"IN_Tel", company.Tel);
            base.AddParam(paras,"IN_Tel2", company.Tel2);
            base.AddParam(paras,"IN_FAX", company.FAX);
            base.AddParam(paras,"IN_EmailAddress", company.EmailAddress);
            base.AddParam(paras,"IN_TAXCode", company.TAXCode);
            base.AddParam(paras,"IN_CompanyBank", company.CompanyBank);
            base.AddParam(paras,"IN_AccountCode", company.AccountCode);
            base.AddParam(paras,"IN_Represent", company.Represent);
            base.AddParam(paras,"IN_Position", company.Position);
            base.AddParam(paras,"IN_Position2", company.Position2);
            base.AddParam(paras,"IN_CreateUID", company.CreateUID);
            base.AddParam(paras,"IN_UpdateUID", company.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="user">M_Company</param>
        /// <returns></returns>
        public int Update(M_Company company)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE dbo.M_Company");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  CompanyName1	  = @IN_CompanyName1");
            cmdText.AppendLine(" ,CompanyName2	  = @IN_CompanyName2");
            cmdText.AppendLine(" ,CompanyAddress1 = @IN_CompanyAddress1");
            cmdText.AppendLine(" ,CompanyAddress2 = @IN_CompanyAddress2");
            cmdText.AppendLine(" ,CompanyAddress3 = @IN_CompanyAddress3");
            cmdText.AppendLine(" ,CompanyAddress4 = @IN_CompanyAddress4");
            cmdText.AppendLine(" ,CompanyAddress5 = @IN_CompanyAddress5");
            cmdText.AppendLine(" ,CompanyAddress6 = @IN_CompanyAddress6");
            cmdText.AppendLine(" ,Tel		      = @IN_Tel");
            cmdText.AppendLine(" ,Tel2		      = @IN_Tel2");
            cmdText.AppendLine(" ,FAX	          = @IN_FAX");
            cmdText.AppendLine(" ,EmailAddress    = @IN_EmailAddress");
            cmdText.AppendLine(" ,TAXCode	      = @IN_TAXCode");
            cmdText.AppendLine(" ,CompanyBank	  = @IN_CompanyBank");
            cmdText.AppendLine(" ,AccountCode	  = @IN_AccountCode");
            cmdText.AppendLine(" ,Represent	      = @IN_Represent");
            cmdText.AppendLine(" ,Position	      = @IN_Position");
            cmdText.AppendLine(" ,Position2	      = @IN_Position2");
            cmdText.AppendLine(" ,UpdateDate	  = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID	      = @IN_UpdateUID");


            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras,"IN_CompanyName1", company.CompanyName1);
            base.AddParam(paras,"IN_CompanyName2", company.CompanyName2);
            base.AddParam(paras,"IN_CompanyAddress1", company.CompanyAddress1);
            base.AddParam(paras,"IN_CompanyAddress2", company.CompanyAddress2);
            base.AddParam(paras,"IN_CompanyAddress3", company.CompanyAddress3);
            base.AddParam(paras,"IN_CompanyAddress4", company.CompanyAddress4);
            base.AddParam(paras,"IN_CompanyAddress5", company.CompanyAddress5);
            base.AddParam(paras,"IN_CompanyAddress6", company.CompanyAddress6);
            base.AddParam(paras,"IN_Tel", company.Tel);
            base.AddParam(paras,"IN_Tel2", company.Tel2);
            base.AddParam(paras,"IN_FAX", company.FAX);
            base.AddParam(paras,"IN_EmailAddress", company.EmailAddress);
            base.AddParam(paras,"IN_TAXCode", company.TAXCode);
            base.AddParam(paras,"IN_CompanyBank", company.CompanyBank);
            base.AddParam(paras,"IN_AccountCode", company.AccountCode);
            base.AddParam(paras,"IN_Represent", company.Represent);
            base.AddParam(paras,"IN_Position", company.Position);
            base.AddParam(paras,"IN_Position2", company.Position2);
            base.AddParam(paras,"IN_UpdateDate", company.UpdateDate, true);
            base.AddParam(paras,"IN_UpdateUID", company.UpdateUID);


            cmdWhere.AppendLine(" UpdateDate      = @IN_UpdateDate ");

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

    }
}

