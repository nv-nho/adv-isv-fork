using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Models.Type;
using System.Collections;
using System.Data;
using OMS.Models;
using OMS.Utilities;

namespace OMS.DAC
{
    public class T_WorkingCalendar_HService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Contructor of T_WorkingCalendar_H service
        /// </summary>        
        private T_WorkingCalendar_HService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_WorkingCalendar_H service
        /// </summary>
        /// <param name="db">Class DB</param>
        public T_WorkingCalendar_HService(DB db)
            : base(db)
        {
        }

        #endregion

        #region Get Data
        /// <summary>
        /// Get the list for searching
        /// </summary>
        /// <param name="model">WorkingCalendarHeaderSearch</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="sortField">sortField</param>
        /// <param name="sortDirec">sortDirec</param>
        /// <returns></returns>
        /// 
        public IList<WorkingCalendarResult> GetListForSearch(WorkingCalendarHeaderSearch model, int onlyUserID,int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "UpdateDate", "", "CalendarCD", "CalendarName", "InitialDate" };
            string[] direc = new string[] { "ASC", "DESC" };

            string RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1];
            if (sortField == 1)
            {
                RowNumber = fields[sortField - 1] + " " + direc[1];
            }

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");
            cmdText.AppendLine(" ,CalendarCD");
            cmdText.AppendLine(" ,CalendarName");
            cmdText.AppendLine(" ,InitialDate");
            cmdText.AppendLine(" ,AnnualWorkingDays");
            cmdText.AppendLine(" ,AnnualWorkingHours");
            cmdText.AppendLine(" ,AgreementFlag1");
            cmdText.AppendLine(" ,AgreementFlag2");
            cmdText.AppendLine(" ,AgreementFlag3");
            cmdText.AppendLine(" ,AgreementFlag4");
            cmdText.AppendLine(" ,AgreementFlag5");
            cmdText.AppendLine(" ,AgreementFlag6");
            cmdText.AppendLine(" ,AgreementFlag7");
            cmdText.AppendLine(" ,AgreementFlag8");
            cmdText.AppendLine(" ,AgreementFlag9");
            cmdText.AppendLine(" ,AgreementFlag10");
            cmdText.AppendLine(" ,AgreementFlag11");
            cmdText.AppendLine(" ,AgreementFlag12");
            cmdText.AppendLine(" ,StatusFlag");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.T_WorkingCalendar_H AS T");

            //Parameter
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(model.CalendarCD))
            {
                if (model.CalendarCD.Length < 50)
                {
                    cmdWhere.AppendLine("AND UPPER(T.CalendarCD) LIKE '%' + UPPER(@IN_CalendarCD) + '%'");
                }
                else
                {
                    cmdWhere.AppendLine("AND UPPER(T.CalendarCD) = UPPER(@IN_CalendarCD)");
                }
                base.AddParam(paras, "IN_CalendarCD", model.CalendarCD, true);
            }

            if (!string.IsNullOrEmpty(model.CalendarName))
            {
                if (model.CalendarName.Length < 100)
                {
                    cmdWhere.AppendLine("AND UPPER(T.CalendarName) LIKE '%' + UPPER(@IN_CalendarName) + '%'");
                }
                else
                {
                    cmdWhere.AppendLine("AND UPPER(T.CalendarName) = UPPER(@IN_CalendarName)");
                }
                base.AddParam(paras, "IN_CalendarName", model.CalendarName, true);
            }

            if (onlyUserID > 0)
            {
                cmdWhere.AppendLine("AND EXISTS (SELECT 1 ");
                cmdWhere.AppendLine("	    FROM T_WorkingCalendar_U U");
                cmdWhere.AppendLine("	    WHERE U.HID = T.ID AND U.UID = @IN_UID)");
                base.AddParam(paras, "IN_UID", onlyUserID);
            }
            base.AddParam(paras, "IN_PageIndex", pageIndex);
            base.AddParam(paras, "IN_PageSize", pageSize);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }
            //SQL OUT
            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine(" DATA.*");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" )");
            cmdOutText.AppendLine(" AS DATA");
            cmdOutText.AppendLine(" WHERE");
            cmdOutText.AppendLine(" DATA.RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1");
            return this.db.FindList1<WorkingCalendarResult>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get total row
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public int GetTotalRow(WorkingCalendarHeaderSearch model, int onlyUserID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.T_WorkingCalendar_H AS T");

            StringBuilder cmdWhere = new StringBuilder();
            //Parameter
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(model.CalendarCD))
            {
                if (model.CalendarCD.Length < 50)
                {
                    cmdWhere.AppendLine("AND UPPER(T.CalendarCD) LIKE '%' + UPPER(@IN_CalendarCD) + '%'");
                }
                else
                {
                    cmdWhere.AppendLine("AND UPPER(T.CalendarCD) = UPPER(@IN_CalendarCD)");
                }
                base.AddParam(paras, "IN_CalendarCD", model.CalendarCD, true);
            }
            if (!string.IsNullOrEmpty(model.CalendarName))
            {
                if (model.CalendarCD.Length < 50)
                {
                    cmdWhere.AppendLine("AND UPPER(T.CalendarName) LIKE '%' + UPPER(@IN_CalendarName) + '%'");
                }
                else
                {
                    cmdWhere.AppendLine("AND UPPER(T.CalendarName) = UPPER(@IN_CalendarName)");
                }
                base.AddParam(paras, "IN_CalendarName", model.CalendarName, true);
            }
            if (onlyUserID > 0)
            {
                cmdWhere.AppendLine("AND EXISTS (SELECT 1 ");
                cmdWhere.AppendLine("	    FROM T_WorkingCalendar_U U");
                cmdWhere.AppendLine("	    WHERE U.HID = T.ID AND U.UID = @IN_UID)");
                base.AddParam(paras, "IN_UID", onlyUserID);
            }
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }


        /// <summary>
        /// Get by WorkingCalendar ID
        /// </summary>
        /// <param name="WorkingCalendarID">WorkingCalendarID</param>
        /// <returns></returns>
        public T_WorkingCalendar_H GetByID(int workingCalendarID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_WorkingCalendar_H AS T");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" T.ID = @IN_WorkingCalendarID ");
            base.AddParam(paras, "IN_WorkingCalendarID", workingCalendarID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<T_WorkingCalendar_H>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get by WorkingCalendar CD
        /// </summary>
        /// <param name="WorkingCalendarCD">WorkingCalendarCD</param>
        /// <returns></returns>
        public T_WorkingCalendar_H GetByCD(string workingCalendarCD)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_WorkingCalendar_H AS T");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" T.CalendarCD = @IN_WorkingCalendarCD ");
            base.AddParam(paras, "IN_WorkingCalendarCD", workingCalendarCD);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<T_WorkingCalendar_H>(cmdText.ToString(), paras);
        }


        /// <summary>
        /// Get by user id and date
        /// </summary>
        /// <param name="WorkingCalendarID">WorkingCalendarID</param>
        /// <returns></returns>
        public T_WorkingCalendar_H GetByByUserAndDate(int userID, DateTime date)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_WorkingCalendar_H AS T");
            cmdText.AppendLine(" INNER JOIN");
            cmdText.AppendLine("        dbo.T_WorkingCalendar_D AS T_D ON T.ID = T_D.HID");
            cmdText.AppendLine(" INNER JOIN");
            cmdText.AppendLine("        dbo.T_WorkingCalendar_U AS T_U");
            cmdText.AppendLine(" ON");
            cmdText.AppendLine("        T_U.HID = T_D.HID");
            cmdText.AppendLine(" WHERE");
            cmdText.AppendLine("        T_U.UID = @IN_UID");
            cmdText.AppendLine("    AND T_D.WorkingDate = @IN_Date");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UID", userID);
            base.AddParam(paras, "IN_Date", date, true);

            return this.db.Find1<T_WorkingCalendar_H>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Calendar header id
        /// </summary>
        /// <param name="userID">user id</param>
        /// <param name="date">date</param>
        /// <returns></returns>
        public int GetIDByUserAndDate(int userID, DateTime date)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("        T_D.HID");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine("        dbo.T_WorkingCalendar_D AS T_D");
            cmdText.AppendLine(" INNER JOIN");
            cmdText.AppendLine("        dbo.T_WorkingCalendar_U AS T_U");
            cmdText.AppendLine(" ON");
            cmdText.AppendLine("        T_U.HID = T_D.HID");
            cmdText.AppendLine(" WHERE");
            cmdText.AppendLine("        T_U.UID = @IN_UID");
            cmdText.AppendLine("    AND T_D.WorkingDate = @IN_Date");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UID", userID);
            base.AddParam(paras, "IN_Date", date, true);

            int ret = 0;
            int.TryParse(string.Format("{0}", this.db.ExecuteScalar1(cmdText.ToString(), paras)), out ret);

            return ret;

        }

       /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        public IList<DropDownModel> GetDateOfServiceComboboxByWorkingCalendarHID(int hID, ref string defaultVal)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            T_WorkingCalendar_H calendarH = new T_WorkingCalendar_H();
            IList<DropDownModel> lstRet = new List<DropDownModel>();
            //Parameter
            Hashtable paras = new Hashtable();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_WorkingCalendar_H AS T");
            cmdText.AppendLine(" WHERE T.ID = @IN_ID");
            cmdText.AppendLine(" ORDER BY T.InitialDate");

            //Parameter
            base.AddParam(paras, "IN_ID", hID);

            calendarH = this.db.Find1<T_WorkingCalendar_H>(cmdText.ToString(), paras);
            if (calendarH != null)
            {
                DateTime fromDate = calendarH.InitialDate;
                DateTime toDate = new DateTime();
                for (int i = 0; i < 12; i++)
                {
                    fromDate = calendarH.InitialDate.AddMonths(i);
                    toDate = calendarH.InitialDate.AddMonths(i + 1).AddDays(-1);

                    string stInitialDate = string.Empty;
                    if (fromDate.Year != toDate.Year)
                    {
                        stInitialDate = string.Format(Constants.FMR_DATE_YM, fromDate) + "〜" + string.Format(Constants.FMR_DATE_YM, toDate);
                    }
                    else
                    {

                        if (fromDate.Month != toDate.Month)
                        {
                            stInitialDate = string.Format(Constants.FMR_DATE_YM, fromDate) + "〜" + string.Format(Constants.FMR_DATE_M, toDate);
                        }
                        else
                        {
                            stInitialDate = string.Format(Constants.FMR_DATE_YM, fromDate);
                        }
                    }
                    DropDownModel model = new DropDownModel();
                    model.Value = string.Format(Constants.FMR_DATE_YMD, fromDate);
                    model.DisplayName = stInitialDate;
                    model.DataboundItem = calendarH.ID;

                    lstRet.Add(model);

                    if (DateTime.Now.Date >= fromDate && DateTime.Now.Date <= toDate)
                    {
                        defaultVal = string.Format(Constants.FMR_DATE_YMD, fromDate);
                    }

                    // add value for listCheckDefaltVal
                    if (i == 11 && defaultVal == string.Empty)
                    {
                        defaultVal = string.Format(Constants.FMR_DATE_YMD, fromDate);
                    }
                }
            }

            return lstRet;
        }

        /// <summary>
        /// GetListInitDate
        /// </summary>
        public DataTable GetDataInitDate()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  InitialDate");
            cmdText.AppendLine(" FROM dbo.T_WorkingCalendar_H AS T");
            return this.db.Find1<DataTable>(cmdText.ToString(), null);
        }

        /// <summary>
        /// Get Data Project
        /// </summary>
        /// <returns></returns>
        public IList<DropDownModel> GetWorkingCalendarCbbData(ref string defaultVal
                                                                , int userID = Constants.DEFAULT_VALUE_INT
                                                                , bool isConditionU = false
                                                                , int departmentID = Constants.DEFAULT_VALUE_INT)
        {
            //SQL String
            IList<DropDownModel> lstRet = new List<DropDownModel>();
            IList<T_WorkingCalendar_H> lstCalendarH = new List<T_WorkingCalendar_H>();

            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UID", userID);
            base.AddParam(paras, "IN_DepartmentID", departmentID);

            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT DISTINCT ");
            cmdText.AppendLine(" WH.* ");
            cmdText.AppendLine(" FROM dbo.T_WorkingCalendar_H AS WH ");
            cmdText.AppendLine(" WHERE EXISTS(SELECT* FROM T_WorkingCalendar_U WU WHERE WU.HID = WH.ID ");

            if (departmentID != Constants.DEFAULT_VALUE_INT)
            {
                cmdText.AppendLine(" AND WU.UID IN (SELECT ID FROM M_User WHERE DepartmentID = @IN_DepartmentID) ");
            }

            if (userID != Constants.DEFAULT_VALUE_INT && isConditionU)
            {
                cmdText.AppendLine(" AND WU.UID IN (@IN_UID) ");
            }

            cmdText.AppendLine(" ) ORDER BY WH.CalendarCD");

            lstCalendarH = this.db.FindList1<T_WorkingCalendar_H>(cmdText.ToString(), paras);

            List<int> lstHIDD = new List<int>();
            defaultVal = string.Empty;
            foreach (var item in lstCalendarH)
            {
                DateTime fromDate = item.InitialDate;
                DateTime toDate = item.InitialDate.AddMonths(12).AddDays(-1);
                DropDownModel model = new DropDownModel();
                model.DisplayName = item.CalendarName;
                model.Value = item.ID.ToString();

                if ((DateTime.Now.Date >= fromDate && DateTime.Now.Date <= toDate))
                {
                    if (string.IsNullOrEmpty(defaultVal))
                    {
                        defaultVal = item.ID.ToString();
                    }
                    lstHIDD.Add(item.ID);
                }

                lstRet.Add(model);
            }

            Hashtable parasU = new Hashtable();
            base.AddParam(parasU, "IN_UID", userID);

            StringBuilder cmdTextU = new StringBuilder();
            cmdTextU.AppendLine(" SELECT * ");
            cmdTextU.AppendLine(" FROM   dbo.T_WorkingCalendar_U ");
            cmdTextU.AppendLine(" WHERE  UID = @IN_UID ");
            IList<T_WorkingCalendar_U> lst_WorkingCalendar_U = this.db.FindList1<T_WorkingCalendar_U>(cmdTextU.ToString(), parasU);
            List<int> lstHID = new List<int>();
            foreach (var item in lst_WorkingCalendar_U)
            {
                if (lstHIDD.Contains(item.HID))
                {
                    defaultVal = item.HID.ToString();
                }
            }

            if (string.IsNullOrEmpty(defaultVal) && lstCalendarH.Count > 0)
            {
                defaultVal = lstCalendarH[0].ID.ToString();
            }

            return lstRet;
        }

        public bool IsExistsByUID(int UID, int calendarID)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("SELECT");
			cmdText.AppendLine("    COUNT(1) AS data_count");
		    cmdText.AppendLine("FROM T_WorkingCalendar_H H");
		    cmdText.AppendLine("WHERE EXISTS (SELECT 1 ");
			cmdText.AppendLine("	    FROM T_WorkingCalendar_U U");
            cmdText.AppendLine("	    WHERE U.HID = H.ID AND U.UID = @IN_UID)");
            
            Hashtable prms = new Hashtable();
            base.AddParam(prms, "IN_UID", UID);

            if (calendarID > 0)
            {
                cmdText.AppendLine("AND H.ID = @IN_ID");
                base.AddParam(prms, "IN_ID", calendarID);
            }

            return (int)this.db.ExecuteScalar1(cmdText.ToString(), prms) > 0;
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert T_WorkingCalendar_H
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int Insert(T_WorkingCalendar_H t_WorkingCalendar_H)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO [dbo].[T_WorkingCalendar_H]");
            cmdText.AppendLine(" (");
            cmdText.AppendLine(" CalendarCD");
            cmdText.AppendLine(" ,CalendarName");
            cmdText.AppendLine(" ,InitialDate");
            cmdText.AppendLine(" ,AnnualWorkingDays");
            cmdText.AppendLine(" ,AnnualWorkingHours");
            cmdText.AppendLine(" ,AgreementFlag1");
            cmdText.AppendLine(" ,AgreementFlag2");
            cmdText.AppendLine(" ,AgreementFlag3");
            cmdText.AppendLine(" ,AgreementFlag4");
            cmdText.AppendLine(" ,AgreementFlag5");
            cmdText.AppendLine(" ,AgreementFlag6");
            cmdText.AppendLine(" ,AgreementFlag7");
            cmdText.AppendLine(" ,AgreementFlag8");
            cmdText.AppendLine(" ,AgreementFlag9");
            cmdText.AppendLine(" ,AgreementFlag10");
            cmdText.AppendLine(" ,AgreementFlag11");
            cmdText.AppendLine(" ,AgreementFlag12");
            cmdText.AppendLine(" ,StatusFlag");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine(" @IN_CalendarCD");
            cmdText.AppendLine(" ,@IN_CalendarName");
            cmdText.AppendLine(" ,@IN_InitialDate");
            cmdText.AppendLine(" ,@IN_AnnualWorkingDays");
            cmdText.AppendLine(" ,@IN_AnnualWorkingHours");
            cmdText.AppendLine(" ,@IN_AgreementFlag1");
            cmdText.AppendLine(" ,@IN_AgreementFlag2");
            cmdText.AppendLine(" ,@IN_AgreementFlag3");
            cmdText.AppendLine(" ,@IN_AgreementFlag4");
            cmdText.AppendLine(" ,@IN_AgreementFlag5");
            cmdText.AppendLine(" ,@IN_AgreementFlag6");
            cmdText.AppendLine(" ,@IN_AgreementFlag7");
            cmdText.AppendLine(" ,@IN_AgreementFlag8");
            cmdText.AppendLine(" ,@IN_AgreementFlag9");
            cmdText.AppendLine(" ,@IN_AgreementFlag10");
            cmdText.AppendLine(" ,@IN_AgreementFlag11");
            cmdText.AppendLine(" ,@IN_AgreementFlag12");
            cmdText.AppendLine(" ,@IN_StatusFlag");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID)");

            //Add Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarCD", t_WorkingCalendar_H.CalendarCD);
            base.AddParam(paras, "IN_CalendarName", t_WorkingCalendar_H.CalendarName);
            base.AddParam(paras, "IN_InitialDate", t_WorkingCalendar_H.InitialDate, true);
            base.AddParam(paras, "IN_AnnualWorkingDays", t_WorkingCalendar_H.AnnualWorkingDays);
            base.AddParam(paras, "IN_AnnualWorkingHours", t_WorkingCalendar_H.AnnualWorkingHours);
            base.AddParam(paras, "IN_AgreementFlag1", t_WorkingCalendar_H.AgreementFlag1);
            base.AddParam(paras, "IN_AgreementFlag2", t_WorkingCalendar_H.AgreementFlag2);
            base.AddParam(paras, "IN_AgreementFlag3", t_WorkingCalendar_H.AgreementFlag3);
            base.AddParam(paras, "IN_AgreementFlag4", t_WorkingCalendar_H.AgreementFlag4);
            base.AddParam(paras, "IN_AgreementFlag5", t_WorkingCalendar_H.AgreementFlag5);
            base.AddParam(paras, "IN_AgreementFlag6", t_WorkingCalendar_H.AgreementFlag6);
            base.AddParam(paras, "IN_AgreementFlag7", t_WorkingCalendar_H.AgreementFlag7);
            base.AddParam(paras, "IN_AgreementFlag8", t_WorkingCalendar_H.AgreementFlag8);
            base.AddParam(paras, "IN_AgreementFlag9", t_WorkingCalendar_H.AgreementFlag9);
            base.AddParam(paras, "IN_AgreementFlag10", t_WorkingCalendar_H.AgreementFlag10);
            base.AddParam(paras, "IN_AgreementFlag11", t_WorkingCalendar_H.AgreementFlag11);
            base.AddParam(paras, "IN_AgreementFlag12", t_WorkingCalendar_H.AgreementFlag12);
            base.AddParam(paras, "IN_StatusFlag", t_WorkingCalendar_H.StatusFlag);

            base.AddParam(paras, "IN_CreateUID", t_WorkingCalendar_H.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", t_WorkingCalendar_H.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="m_WorkingSystem">m_WorkingSystem</param>
        /// <returns></returns>
        public int Update(T_WorkingCalendar_H t_WorkingCalendar_H)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE [dbo].[T_WorkingCalendar_H]");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  CalendarCD = @IN_CalendarCD");
            cmdText.AppendLine(" ,CalendarName = @IN_CalendarName");
            cmdText.AppendLine(" ,InitialDate = @IN_InitialDate");
            cmdText.AppendLine(" ,AnnualWorkingDays = @IN_AnnualWorkingDays");
            cmdText.AppendLine(" ,AnnualWorkingHours = @IN_AnnualWorkingHours");
            cmdText.AppendLine(" ,AgreementFlag1 = @IN_AgreementFlag1");
            cmdText.AppendLine(" ,AgreementFlag2 = @IN_AgreementFlag2");
            cmdText.AppendLine(" ,AgreementFlag3 = @IN_AgreementFlag3");
            cmdText.AppendLine(" ,AgreementFlag4 = @IN_AgreementFlag4");
            cmdText.AppendLine(" ,AgreementFlag5 = @IN_AgreementFlag5");
            cmdText.AppendLine(" ,AgreementFlag6 = @IN_AgreementFlag6");
            cmdText.AppendLine(" ,AgreementFlag7 = @IN_AgreementFlag7");
            cmdText.AppendLine(" ,AgreementFlag8 = @IN_AgreementFlag8");
            cmdText.AppendLine(" ,AgreementFlag9 = @IN_AgreementFlag9");
            cmdText.AppendLine(" ,AgreementFlag10 = @IN_AgreementFlag10");
            cmdText.AppendLine(" ,AgreementFlag11 = @IN_AgreementFlag11");
            cmdText.AppendLine(" ,AgreementFlag12 = @IN_AgreementFlag12");
            cmdText.AppendLine(" ,StatusFlag = @IN_StatusFlag");

            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID	 = @IN_UpdateUID");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_ID", t_WorkingCalendar_H.ID);
            base.AddParam(paras, "IN_CalendarCD", t_WorkingCalendar_H.CalendarCD);
            base.AddParam(paras, "IN_CalendarName", t_WorkingCalendar_H.CalendarName);
            base.AddParam(paras, "IN_InitialDate", t_WorkingCalendar_H.InitialDate, true);
            base.AddParam(paras, "IN_AnnualWorkingDays", t_WorkingCalendar_H.AnnualWorkingDays);
            base.AddParam(paras, "IN_AnnualWorkingHours", t_WorkingCalendar_H.AnnualWorkingHours);

            base.AddParam(paras, "IN_AgreementFlag1", t_WorkingCalendar_H.AgreementFlag1);
            base.AddParam(paras, "IN_AgreementFlag2", t_WorkingCalendar_H.AgreementFlag2);
            base.AddParam(paras, "IN_AgreementFlag3", t_WorkingCalendar_H.AgreementFlag3);
            base.AddParam(paras, "IN_AgreementFlag4", t_WorkingCalendar_H.AgreementFlag4);
            base.AddParam(paras, "IN_AgreementFlag5", t_WorkingCalendar_H.AgreementFlag5);
            base.AddParam(paras, "IN_AgreementFlag6", t_WorkingCalendar_H.AgreementFlag6);
            base.AddParam(paras, "IN_AgreementFlag7", t_WorkingCalendar_H.AgreementFlag7);
            base.AddParam(paras, "IN_AgreementFlag8", t_WorkingCalendar_H.AgreementFlag8);
            base.AddParam(paras, "IN_AgreementFlag9", t_WorkingCalendar_H.AgreementFlag9);
            base.AddParam(paras, "IN_AgreementFlag10", t_WorkingCalendar_H.AgreementFlag10);
            base.AddParam(paras, "IN_AgreementFlag11", t_WorkingCalendar_H.AgreementFlag11);
            base.AddParam(paras, "IN_AgreementFlag12", t_WorkingCalendar_H.AgreementFlag12);
            base.AddParam(paras, "IN_StatusFlag", t_WorkingCalendar_H.StatusFlag);

            base.AddParam(paras, "IN_UpdateDate", t_WorkingCalendar_H.UpdateDate, true);
            base.AddParam(paras, "IN_UpdateUID", t_WorkingCalendar_H.UpdateUID);


            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate = @IN_UpdateDate");

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Delete By Id
        /// <summary>
        /// Delete t_WorkingCalendar_H By Id 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int Delete(int workingDateID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_WorkingCalendar_H]");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("ID = @IN_ID");

            base.AddParam(paras, "IN_ID", workingDateID);
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
