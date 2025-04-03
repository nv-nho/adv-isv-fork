using System;
using System.Data.Common;

namespace OMS.Models
{
    [Serializable]
    public class M_Message
    {
        #region Const MessageID

        /// <summary>
        /// Please input {0}.
        /// </summary>
        public const string MSG_0001 = "0001";

        /// <summary>
        /// Please input {0}.
        /// </summary>
        public const string MSG_REQUIRE = "0001";

        /// <summary>
        /// {0} has already existed.
        /// </summary>
        public const string MSG_EXIST_CODE = "0002";

        /// <summary>
        /// Inputed {0} has not existed.
        /// </summary>
        public const string MSG_NOT_EXIST_CODE = "0003";

        /// <summary>
        /// An unexpected error has occurred. {0} data is failed.
        /// </summary>
        public const string MSG_UPDATE_FAILE = "0004";

        /// <summary>
        /// This data has been already updated by another user. Please press the Back button for trying again.
        /// </summary>
        public const string MSG_DATA_CHANGED = "0005";

        /// <summary>
        /// {0} is incorrect.
        /// </summary>
        public const string MSG_INCORRECT_FORMAT = "0006";

        /// <summary>
        /// {0} has already used in system. Can't delete.
        /// </summary>
        public const string MSG_EXIST_CANT_DELETE = "0008";

        /// <summary>
        /// Data will be created. Are you OK?
        /// </summary>
        public const string MSG_QUESTION_INSERT = "0009";

        /// <summary>
        /// Data will be updated. Are you OK?
        /// </summary>
        public const string MSG_QUESTION_UPDATE = "0010";

        /// <summary>
        /// Data will be deleted.  Are you OK?
        /// </summary>
        public const string MSG_QUESTION_DELETE = "0011";

        /// <summary>
        /// {0} is duplicated.
        /// </summary>
        public const string MSG_DUPLICATE = "0012";

        /// <summary>
        /// Old password is invalid.
        /// </summary>
        public const string MSG_PASS_INVALID = "0013";

        /// <summary>
        /// Confirm password is incorrect.
        /// </summary>
        public const string MSG_PASS_NOT_MATCH = "0014";

        /// <summary>
        /// Password must have at least 8 characters and contain both the alpha and the number.
        /// </summary>
        public const string MSG_PASSWORD_RULE = "0015";

        /// <summary>
        /// Please select {0}.
        /// </summary>
        public const string MSG_PLEASE_SELECT = "0016";

        /// <summary>
        /// {0} must be less than or equal to {1}.
        /// </summary>
        public const string MSG_LESS_THAN_EQUAL = "0017";

        /// <summary>
        /// Please input {0}. 【Row {1}】
        /// </summary>
        public const string MSG_REQUIRE_GRID = "0018";

        /// <summary>
        /// {0} is duplicated. 【Row {1}】
        /// </summary>
        public const string MSG_DUPLICATE_GRID = "0019";

        /// <summary>
        /// {0} must be less than or equal to {1}. 【Row {2}】
        /// </summary>
        public const string MSG_LESS_THAN_EQUAL_GRID = "0020";

        /// <summary>
        /// {0} is invalid. 【Row {1}】
        /// </summary>
        public const string MSG_INVALID_GRID = "0021";

        /// <summary>
        /// {0} must be greater than or equal to {1}. 【Row {2}】 
        /// </summary>
        public const string MSG_GREATER_THAN_EQUAL_GRID = "0022";

        /// <summary>
        /// {0} doesn't exist.
        /// </summary>
        public const string MSG_VALUE_NOT_EXIST = "0023";

        /// <summary>
        /// {0} was disabled. Please check the relevant master. 
        /// </summary>
        public const string MSG_CODE_DISABLE = "0024";

        /// <summary>
        /// Login information is incorrect.
        /// </summary>
        public const string MSG_LOGIN_INFO_INCORRECT = "0025";

        /// <summary>
        /// Can't create {0}. It made up to 9,999 in a month.
        /// </summary>
        public const string MSG_SIZE_MAX_NO = "0026";

        /// <summary>
        /// {0} must be greater than or equal to {1}.
        /// </summary>
        public const string MSG_GREATER_THAN_EQUAL = "0027";

        /// <summary>
        /// Inputed {0} has not existed. 【Row {1}】
        /// </summary>
        public const string MSG_NOT_EXIST_CODE_GRID = "0028";

        /// <summary>
        /// {0} must be less than {1}.
        /// </summary>
        public const string MSG_LESS_THAN = "0030";

        /// <summary>
        /// {0} must be greater than {1}. 【Row {2}】
        /// </summary>
        public const string MSG_GREATER_THAN_GRID = "0032";

        /// <summary>
        /// {0} must be less than {1}. 【Row {2}】
        /// </summary>
        public const string MSG_LESS_THAN_GRID = "0033";

        /// <summary>
        /// 勤務表を提出します。宜しいですか？
        /// </summary>
        public const string MSG_SUBMIT = "0034";

        /// <summary>
        ///勤務表の提出を取消します。宜しいですか？
        /// </summary>
        public const string MSG_SUBMIT_CANCEL = "0035";

        /// <summary>
        /// 勤務日報が登録されていない出勤日が存在するため提出できません。</br>再度、出勤日の日報登録をご確認下さい。
        /// </summary>
        public const string MSG_SUBMIT_ERROR = "0036";

        /// <summary>
        /// Download {0}. Are you OK?
        /// </summary>
        public const string MSG_QUESTION_OUTPUT_FILE = "0037";

        /// <summary>
        /// This data will be rose to "Sales" stage. Are you OK?
        /// </summary>
        public const string MSG_QUESTION_MOVED_TO_SALES = "0038";

        /// <summary>
        /// {0} process has been completed. {1}<button class="btn btn-link" onclick="callRef('{2}');return false;">{3}</button>.
        /// </summary>
        public const string MSG_PROCESS_COMPLETED = "0039";

        /// <summary>
        /// Selected row will be deleted. Are you OK?
        /// </summary>
        public const string MSG_QUESTION_DELETE_SELECTED_ROW = "0042";

        /// <summary>
        /// Please select row data.
        /// </summary>
        public const string MSG_SELECT_ROW_DATA = "0043";

        /// <summary>
        /// Please select file to upload
        /// </summary>
        public const string MSG_SELECT_FILE_UPLOAD = "0044";

        /// <summary>
        /// {0} disabled. 【Row {1}】
        /// </summary>
        public const string MSG_CODE_DISABLE_GRID = "0047";

        /// <summary>
        /// If you change the Currency, "Unit price"  "Subtotal"  "TAX"  will be cleared.  Are you OK?
        /// </summary>
        public const string MSG_CODE_CURRENCY_CHANGED = "0048";

        /// <summary>
        /// If you change the Method of Taxation, "TAX amount" will be changed. Are you OK?
        /// </summary>
        public const string MSG_CODE_METHOD_CHANGED = "0049";

        ///// <summary>
        ///// This data has been already processed P.O. </br>Can't continue this process.
        ///// </summary>
        //public const string MSG_ALREADY_PROCESSED_PO = "0050";

        /// <summary>
        /// Please input an excel file path.
        /// </summary>
        public const string MSG_INPUT_EXCEL = "0050";

        /// <summary>
        /// File extension must be in {0}.
        /// </summary>
        public const string MSG_CODE_FILE_EXTENSION = "0051";

        /// <summary>
        /// Please input an image file path.
        /// </summary>
        public const string MSG_INPUT_IMAGE = "0053";

        /// <summary>
        /// This data has been already updated by another user. Please try again.
        /// </summary>
        public const string MSG_CODE_DELETED_UPDATED = "0054";

        /// <summary>
        /// Can't create data. {0} is different.
        /// </summary>
        public const string MSG_SELECT_ROW_SAME_VALUE = "0055";

        /// <summary>
        ///  {0} data will be created. Are you OK ?
        /// </summary>
        public const string MSG_QUESTION_CREATE = "0059";

        /// <summary>
        /// Can't delete. Selected detail row has been created {0} data.【Row {1}】
        /// </summary>
        public const string MSG_CAN_NOT_DELETE_SELL_INVOICE_DELIVERY_GRID = "0061";

        /// <summary>
        /// Please Select {0}. 【Row {1}】
        /// </summary>
        public const string MSG_REQUIRE_SELECT_GRID = "0063";

        /// <summary>
        /// Size of {0} must be less than or equal to {1}.
        /// </summary>
        public const string MSG_SIZE_FILE_UPLOAD_LESS_THAN_EQUAL = "0068";

        /// <summary>
        /// Make a Copy. Are you OK?
        /// </summary>
        public const string MSG_MAKE_COPY = "0069";

        /// <summary>
        /// Make a Revision. Are you OK?
        /// </summary>
        public const string MSG_MAKE_REVISE = "0070";

        /// <summary>
        /// {0} must be different {1}.
        /// </summary>
        public const string MSG_MUST_BE_DIFFERENT = "0071";

        /// <summary>
        /// Make copy data from cost. Are you OK?
        /// </summary>
        public const string MSG_COPY_FROM_COST = "0072";

        /// <summary>
        /// Make copy data from sell. Are you OK?
        /// </summary>
        public const string MSG_COPY_FROM_SELL = "0073";

        /// <summary>
        /// {0} must be different {1}.【Row {2}】
        /// </summary>
        public const string MSG_MUST_BE_DIFFERENT_GRID = "0074";

        /// <summary>
        /// 設定済のカレンダーが初期化されます。宜しいですか？
        /// </summary>
        public const string MSG_CLEAR = "0075";

        /// <summary>
        /// 勤務日報を定時で一括登録します。よろしいですか？既に日報が存在する日付は更新されません。
        /// </summary>
        public const string MSG_REGISTER_DEFAULT = "0076";

        /// <summary>
        /// Update table data {0} before executing this function.
        /// </summary>
        public const string MSG_UPDATE_TABLE_DATA = "0078";

        /// <summary>
        /// 以下のユーザーは既に他の勤務カレンダーに設定されているため登録できません。
        /// </summary>
        public const string MSG_VALUE_EXIST_TREEVIEW = "0079";

        /// <summary>
        /// 今月の勤務カレンダーが設定されていないため勤務表が登録できません。</br>詳しくはシステム管理者にお問い合わせ下さい。
        /// </summary>
        public const string MSG_NOT_EXIST_CALENDAR = "0080";

        /// <summary>
        /// 選択データに承認済データがあります。
        /// </summary>
        public const string MSG_APPROVAL_FAILE = "0040";

        /// <summary>
        /// 選択データに未承認データがあります。
        /// </summary>
        public const string MSG_RELEASE_FAILE = "0041";

        /// <summary>
        /// Data will be approval. Are you OK?
        /// </summary>
        public const string MSG_QUESTION_APPROVAL = "0069";

        /// <summary>
        /// Data will be Release. Are you OK?
        /// </summary>
        public const string MSG_QUESTION_RELEASE = "0070";

        /// <summary>
        /// 出力するCSV対象期間は起算日に基づく１か月間となります。</br>正しい日付を入力して下さい。
        /// </summary>
        public const string MSG_CSV_RANGE_DATE = "0077";

        /// <summary>
        /// 出力するCSVに規定を超える数値が含まれています。</br>抽出条件を再確認してください。
        /// </summary>
        public const string MSG_CREATE_CSV_ERROR = "0081";

        /// <summary>
        /// Value of {0} must be equal to {1}.
        /// </summary>
        public const string MSG_VALUE_MUST_EQUAL = "0082";

        /// <summary>
        /// File extension must be in {0}.【Row {1}】.
        /// </summary>
        public const string MSG_EXTENSION_GRID = "0083";

        /// <summary>
        /// {0} doesn't exist.【Row {1}】.
        /// </summary>
        public const string MSG_FILE_EXIST_GRID = "0084";

        /// <summary>
        /// User {0} has error when sending email. Please try again.
        /// </summary>
        public const string MSG_USER_SEND_ERMAIL_ERROR = "0085";

        /// <summary>
        /// Can't connect to Server Email. Please try again.
        /// </summary>
        public const string MSG_CONNECT_ERMAIL_ERROR = "0086";

        /// <summary>
        /// Do you want to resend email?
        /// </summary>
        public const string MSG_CONFIRM_RESEND_EMAIL = "0087";

        /// <summary>
        /// Make a draft. Are you OK?
        /// </summary>
        public const string MSG_MAKE_DRAFT = "0088";

        /// <summary>
        /// {0} must be greater than or equal to {1}.
        /// </summary>
        public const string MSG_DATE_GREATER_THAN_EQUAL = "0089";

        /// <summary>
        /// 経費申請を行います。
        /// プロジェクト管理者に承認依頼メールが送信されます。
        /// よろしいですか？
        /// </summary>
        public const string MSG_CONFIRM_SEND_MAIL_APPLY = "0094";

        /// <summary>
        /// 経費承認を行います。
        /// 総務部（経理担当者）へ経費申請（承認済）のメールが送信されます。
        /// よろしいですか？
        /// </summary>
        public const string MSG_CONFIRM_SEND_MAIL_APPROVE = "0095";

        /// <summary>
        /// 承認済経費の解除を行います。
        /// 総務部（経理担当者）へ承認済経費（解除）のメールが送信されます。
        /// よろしいですか？
        /// </summary>
        public const string MSG_CONFIRM_SEND_MAIL_REJECT = "0096";

        /// <summary>
        /// not enough vacation days
        /// </summary>
        public const string MSG_NOT_ENOUGH_VACATION_DAYS = "0097";

        /// <summary>
        /// 申請します。宜しいですか。
        /// </summary>
        public const string MSG_QUESTION_ACCEPT_REQUEST = "0098";

        /// <summary>
        /// 承認します。宜しいですか。
        /// </summary>
        public const string MSG_QUESTION_ACCEPT_APPROVAL = "0099";

        /// <summary>
        /// 差戻します。宜しいでしょうか。
        /// </summary>
        public const string MSG_QUESTION_CANCEL_APPROVAL = "0100";

        /// <summary>
        /// このデータを{0}しました。{1}ほうしいですか。
        /// </summary>
        public const string MSG_CONFIRM_ATTENDENCE_UPDATE = "0101";

        /// <summary>
        /// 申請必要データがあります。申請して下さい。
        /// </summary>
        public const string MSG_REQUEST_INFO = "0102";

        /// <summary>
        /// 未承認出勤日が存在するため提出できません。
        /// </summary>
        public const string MSG_SUBMIT_APPROVAL_ERROR = "0103";

        /// <summary>
        /// この振休対象日が使用済ため、他の振休対象日を選択してください。
        /// </summary>
        public const string MSG_EXCHANGE_DATE_IS_USED = "0104";

        /// <summary>
        /// この日付は振休取得済でした。変更・削除できません。
        /// </summary>
        public const string MSG_DATE_IS_EXCHANGED = "0105";

        /// <summary>
        /// この日付は振休予定じゃありません。
        /// </summary>
        public const string MSG_ISNOT_EXCHANGE_DATE = "0106";

        /// <summary>
        /// 対象データがありません。
        /// </summary>
        public const string MSG_HAS_NOT_DATA = "0107";

        /// <summary>
        /// ファイル拡張子が許可されていません。正しい拡張子のファイルを選択して下さい。
        /// </summary>
        public const string MSG_EXTENSION = "0108";

        #endregion

        public string MessageID { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public string Message3 { get; set; }
        public string Type { get; set; }

        public M_Message(DbDataReader dr)
        {
            this.MessageID = (string)dr["MessageID"];
            this.Message1 = (string)dr["Message1"];
            this.Message2 = (string)dr["Message2"];
            this.Message3 = (string)dr["Message3"];
            this.Type = (string)dr["Type"];
        }
    }
}
