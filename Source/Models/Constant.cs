
namespace OMS.Models
{
    public class Constant
    {
        #region PK,FK,UN Key

        //M_Company
        public const string M_COMPANY_PK = "M_Company_PK";

        //M_Config_D
        public const string M_CONFIG_D_PK = "M_Config_D_PK";

        //M_Config_H
        public const string M_CONFIG_H_PK = "M_Config_H_PK";
        public const string M_CONFIG_H_UN = "M_Config_H_UN";
        
        //M_Department
        public const string M_DEPARTMENT_PK = "M_Department_PK";
        public const string M_DEPARTMENT_UN = "M_Department_UN";

        //M_GroupUser_D
        public const string M_GroupUser_D_PK = "M_GroupUser_D_PK";

        //M_GroupUser_H
        public const string M_GROUPUSER_DEFAULT_CODE = "0000";
        public const string M_GROUPUSER_PK = "M_GroupUser_PK";
        public const string M_GROUPUSER_UN = "M_GroupUser_UN";

        //M_Information
        public const string M_INFORMATION_PK = "M_Information_PK";

        //M_Message
        public const string M_MESSAGE_PK = "M_Message_PK";

        //M_Project
        public const string M_PROJECT_PK = "M_Project_PK";
        public const string M_PROJECT_UN = "M_Project_UN";
        public const string M_PROJECT_FK_DEPARTMENTID = "M_Project_FK_DepartmentID";

        //M_Setting
        public const string M_SETTING_PK = "M_Setting_PK";

        //M_User
        public const string M_USER_DEFAULT_CODE = "0000";
        public const string M_USER_PK = "M_User_PK";
        public const string M_USER_FK_DEPARTMENTID = "M_User_FK_DepartmentID";
        public const string M_USER_FK_GROUPID = "M_User_FK_GroupID";
        public const string M_USER_UN_CODE = "M_User_UN1";
        public const string M_USER_UN_LOGINID = "M_User_UN2";

        //M_WorkingSystem
        public const string M_WORKINGSYSTEM_PK = "M_WorkingSystem_PK";
        public const string M_WORKINGSYSTEM_UN = "M_WorkingSystem_UN";

        //T_Attached
        public const string T_ATTACHED_PK = "T_Attached_PK";

        //T_Attendance
        public const string T_ATTENDANCE_PK = "T_Attendance_PK";
        public const string T_ATTENDANCE_FK_WORKINGCALENDAR_H = "T_Attendance_FK_WorkingCalendarHID";
        public const string T_ATTENDANCE_FK_WORKINGSYSTEM = "T_Attendance_FK_WorkingSystemID";
        public const string T_ATTENDANCE_UN = "T_Attendance_UN";

        //T_Work_D
        public const string T_WORK_D_PK = "T_Work_D_PK";
        public const string T_WORK_D_FK_PROJECT = "T_Work_D_FK_ProjectID";

        //T_Work_H
        public const string T_WORK_H_PK = "T_Work_H_PK";
        public const string T_WORK_H_UN = "T_Work_H_UN";

        //T_WorkingCalendar_D
        public const string T_WORKINGCALENDAR_D_PK = "T_WorkingCalendar_D_PK";
        public const string T_WORKINGCALENDAR_D_FK_WORKINGSYSTEM = "T_WorkingCalendar_D_FK_WorkingSystemID";

        //T_WorkingCalendar_H
        public const string T_WORKINGCALENDAR_H_PK = "T_WorkingCalendar_H_PK";
        public const string T_WORKINGCALENDAR_H_UN = "T_WorkingCalendar_H_UN";
        public const string T_ATTENDANCE_FK_WORKINGCALENDARHID = "T_Attendance_FK_WorkingCalendarHID";
        public const string T_ATTENDANCERESULT_FK_CALLENDARID = "T_AttendanceResult_FK_CallendarID";

        // T_Expence_H
        public const string T_EXPENCE_H_FK_PROJECTID = "T_Expence_H_FK_ProjectID";
        public const string T_EXPENCE_H_FK_DEPARTMENTID = "T_Expence_H_FK_DepartmentID";

        #endregion

        /// <summary>
        /// Authorized Signature / Seal
        /// </summary>
        public const string DEFAULT_POSITION = "Authorized Signature / Seal";
        /// <summary>
        /// Ký tên và đóng dấu
        /// </summary>
        public const string DEFAULT_REPRESENT = "";

        /// <summary>
        /// Default id
        /// </summary>
        public const int DEFAULT_ID = 1;

        /// <summary>
        /// Money code (USD)
        /// </summary>
        public const string MONEY_CD_USD = "USD";

        //Currency Master defaul row
        public const int DEFAULT_NUMBER_ROW = 50;

        #region Money

        //Max profit
        public const decimal MAX_PROFIT = 100.00m;

        //Vat Ration
        public const int MAX_VATRATIO = 99;

        //Quantity
        public const decimal MAX_QUANTITY_DECIMAL = 999999.99M;
        public const decimal MAX_QUANTITY_NOT_DECIMAL = 999999M;

        //Unit Price
        public const decimal MAX_UNIT_PRICE_DECIMAL = 9999999999.99M;
        public const decimal MAX_UNIT_PRICE_NOT_DECIMAL = 9999999999M;

        //Sub Total
        public const decimal MAX_SUB_TOTAL_DECIMAL = 9999999999999.99M;
        public const decimal MAX_SUB_TOTAL_NOT_DECIMAL = 9999999999999M;
        public const decimal MIN_SUB_TOTAL_DECIMAL = -9999999999999.99M;
        public const decimal MIN_SUB_TOTAL_NOT_DECIMAL = -9999999999999M;

        //Sub Vat
        public const decimal MAX_SUB_VAT_DECIMAL = 999999999999.99M;
        public const decimal MAX_SUB_VAT_NOT_DECIMAL = 999999999999M;
        public const decimal MIN_SUB_VAT_DECIMAL = -999999999999.99M;
        public const decimal MIN_SUB_VAT_NOT_DECIMAL = -999999999999M;

        //Total
        public const decimal MAX_SUM_TOTAL_DECIMAL = 99999999999999.99M;
        public const decimal MAX_SUM_TOTAL_NOT_DECIMAL = 99999999999999;
        public const decimal MIN_SUM_TOTAL_DECIMAL = -99999999999999.99M;
        public const decimal MIN_SUM_TOTAL_NOT_DECIMAL = -99999999999999;

        //VAT
        public const decimal MAX_SUM_VAT_DECIMAL = 9999999999999.99M;
        public const decimal MAX_SUM_VAT_NOT_DECIMAL = 9999999999999;
        public const decimal MIN_SUM_VAT_DECIMAL = -9999999999999.99M;
        public const decimal MIN_SUM_VAT_NOT_DECIMAL = -9999999999999;

        //SUB AMOUNT
        public const decimal MAX_SUB_AMOUNT_DECIMAL = 99999999999999.99M;
        public const decimal MAX_SUB_AMOUNT_NOT_DECIMAL = 99999999999999;
        public const decimal MIN_SUB_AMOUNT_DECIMAL = -99999999999999.99M;
        public const decimal MIN_SUB_AMOUNT_NOT_DECIMAL = -99999999999999;

        //SUM AMOUNT
        public const decimal MAX_SUM_AMOUNT_DECIMAL = 99999999999999.99M;
        public const decimal MAX_SUM_AMOUNT_NOT_DECIMAL = 99999999999999;
        public const decimal MIN_SUM_AMOUNT_DECIMAL = -99999999999999.99M;
        public const decimal MIN_SUM_AMOUNT_NOT_DECIMAL = -99999999999999;

        //BALANCE
        public const decimal MAX_BALANCE_DECIMAL = 99999999999999.99M;
        public const decimal MAX_BALANCE_NOT_DECIMAL = 99999999999999;
        public const decimal MIN_BALANCE_DECIMAL = -99999999999999.99M;
        public const decimal MIN_BALANCE_NOT_DECIMAL = -99999999999999;
        
        #endregion

        #region Width Column
        //Sell and Cost
        public const string GRID_WIDTH = "1138px;";
        public const string PRODUCT_COLUMN_WIDTH = "468px;";
        public const string UNITPRICE_PROFIT_COLUMN_WIDTH = "170px;";
        public const string QTY_UNIT_COLUMN_WIDTH = "140px;";
        public const string SUBTOTAL_REMARK_COLUMN_WIDTH = "180px;";
        public const string VAT_RATIO_COLUMN_WIDTH = "180px;";

        //Payment and Deposit
        public const string DATE_WIDTH = "220px;";
        public const string METHOD_WIDTH = "220px;";
        public const string PERSONAL_WIDTH = "165px;";
        public const string AMOUNT_WIDTH = "313px;";
        public const string REMARK_WIDTH = "172px;";
        #endregion

        #region Gird Sort

        public const string SORT_FIELD_DEPT = "D";
        public const string SORT_FIELD_USER = "U";

        #endregion
    }
}
