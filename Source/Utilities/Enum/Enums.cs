using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OMS.Utilities
{
    /// <summary>
    /// Mode
    /// </summary>
    public enum Mode
    {
        View = 0,
        Insert,
        Update,
        Delete,
        Copy,
        Revise,
        Check,
        Approve,
        NotApprove,

        [Description("承認")]
        Approval,

        [Description("申請")]
        Request,

        [Description("差戻")]
        Cancel
    }

    /// <summary>
    /// Enum Color List
    /// </summary>
    public enum ColorList
    {
        Danger = 0,
        Warning,
        Info,
        Success,
        Finish
    }

    /// <summary>
    /// Form ID Master
    /// </summary>
    public enum FormId
    {
        [Description("社員マスタ")]
        User = 1,
        
        [Description("部門マスタ")]
        Department = 2,
        
        [Description("権限マスタ")]
        GroupUser = 3,

        [Description("勤務体系マスタ")]
        WorkingSystem = 4,

        [Description("プロジェクトマスタ")]
        Project = 5,
        
        [Description("お知らせ")]
        Information = 6,

        [Description("会社情報")]
        CompanyInfo = 7,

        [Description("基本設定")]
        Setting = 8,

        [Description("メール送信")]
        SendMail = 9,

        [Description("勤務カレンダー")]
        WorkCalendar = 10,

        [Description("勤務表")]
        Attendance = 11,

        [Description("勤務表承認")]
        AttendanceApproval = 12,

        [Description("勤務表集計")]
        AttendanceSummary = 13,

        [Description("給与賞与明細")]
        AttendancePayslip = 14,

        [Description("原価マスタ")]
        Cost = 15,

        //[Description("経費登録")]
        //Expence = 16,

        [Description("採算管理")]
        ProjectProfit = 17,

        [Description("経費申請承認")]
        ExpenceGroup = 18,

        [Description("申請")]
        Approval = 19,
    }

    /// <summary>
    /// Authority Master
    /// </summary>
    public enum AuthorTypeMaster
    {
        [Description("表示")]
        View = 1,
        [Description("新規")]
        New,
        [Description("編集")]
        Edit,
        [Description("複写")]
        Copy,
        [Description("削除")]
        Delete
    }

    /// <summary>
    /// Authority WorkCalendar
    /// </summary>
    public enum AuthorTypeWorkCalendar
    {
        [Description("表示")]
        View = 1,
        [Description("新規")]
        New,
        [Description("編集")]
        Edit,
        [Description("削除")]
        Delete,
        [Description("協定設定")]
        AgreementSetting,
        [Description("出力")]
        ExportExcel
    }

    /// <summary>
    /// Authority WorkSchedule
    /// </summary>
    public enum AuthorTypeWorkSchedule
    {
        [Description("表示")]
        View = 1,

        [Description("新規")]
        New,

        [Description("編集")]
        Edit,

        [Description("削除")]
        Delete,

        [Description("他部門")]
        OtherDepartments,

        [Description("他社員")]
        OtherEmployees,

        [Description("他更新")]
        OtherUpdates,

        [Description("一括登録")]       
        ShelfRegistration,

        [Description("出力")]
        ExportExcel
    }

    /// <summary>
    /// Authority AttendanceApproval
    /// </summary>
    public enum AuthorTypeAttendanceApproval
    {
        [Description("表示")]
        View = 1,

        [Description("承認")]
        Approval,

        [Description("解除")]
        reject
    }

    /// <summary>
    /// Authority AttendanceSummary
    /// </summary>
    public enum AuthorTypeAttendanceSummary
    {
        [Description("表示")]
        View = 1,

        [Description("出力")]
        ExportExcel

    }

    /// <summary>
    /// Authority AttendanceSummary
    /// </summary>
    public enum AuthorTypeAttendancePayslip
    {
        [Description("表示")]
        View = 1,

        [Description("他部門")]
        OtherDepartments,

        [Description("他社員")]
        OtherEmployees,

        [Description("更新（アップロード）")]
        Upload

    }
    /// <summary>
    ///権限マスタ（経費申請・承認）
    /// </summary>
    public enum AuthorTypeExpenceGroup
    {
        [Description("表示")]
        View = 1,

        [Description("新規")]
        New,

        [Description("編集")]
        Edit,

        [Description("複写")]
        Copy,

        [Description("削除")]
        Delete,

        [Description("他申請")]
        Accept,

        [Description("承認・解除")]
        AcceptOrDenied,

        [Description("一括承認")]
        AcceptMutil,

        [Description("管理者")]
        Fullrecognition,

        [Description("Excel")]
        ExportExcel1,

        [Description("清算書")]
        ExportExcel2,

        [Description("メール")]
        Email

    }

    /// <summary>
    /// Authority Approval
    /// </summary>
    public enum AuthorTypeApproval
    {
        [Description("承認")]
        Approval = 1,

        [Description("他部門承認")]
        ApprovalAll = 2,

        [Description("承認依頼メール受信（GL）")]
        ApprovalMail = 3,

        [Description("承認結果メール受信（総務）")]
        ConfirmMail = 4
    }

    /// <summary>
    /// Language 
    /// </summary>
    /// <remarks></remarks>
    public enum Language
    {
        /// <summary>
        /// Read number with english
        /// </summary>
        /// <remarks></remarks>
        English,

        /// <summary>
        /// Read number with vietnam
        /// </summary>
        /// <remarks></remarks>
        Vietnam,

        /// <summary>
        /// Read number with japan
        /// </summary>
        /// <remarks></remarks>
        Japan

    }

    /// <summary>
    /// Vacation Flag
    /// </summary>
    public enum VacationFlag
    {
        AllHolidays = 0,
        Morning,
        Afternoon,
        MorningAndAfternoon
    }

    /// <summary>
    /// Working Type
    /// </summary>s
    public enum WorkingType
    {
        WorkFullTime = 0,
        WorkHoliDay,
        LegalHoliDay
    }

    /// <summary>
    /// Vacation
    /// </summary>s
    public enum Vacation
    {
        AnnualPaid = 0,
        Transfer,
        AlternativeLeave,
        SpecialHoliday,
        Absence
    }

    /// <summary>
    /// 状況フラグ
    /// </summary>s
    public enum AttendanceStatusFlag
    {
        NotSubmitted = 0,
        Submitted,
        Approved
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ConfigSort
    {
        value1 = 0,
        value2,
        value3,
        value4,
    }

    public enum AttendanceApprovalStatus
    {
        [Description("")]
        None = 0,

        [Description("申請必要")]
        NeedApproval,

        [Description("申請中")]
        Request,

        [Description("承認済み")]
        Approved,

        [Description("差戻")]
        Cancel
    }

    public enum AttendanceApprovalShinseiKubun
    {
        [Description("遅刻/早退")]
        Late_Early = 1,

        [Description("所定休日")]
        SH_Hours,

        [Description("法定休日")]
        LH_Hours,

        [Description("残業")]
        SH_OverTimeHours,

        [Description("休暇")]
        Vacation
    }
}