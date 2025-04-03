
using OMS.Models;
using OMS.Utilities;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace OMS
{
    /// <summary>
    /// Class Form Detail
    /// </summary>
    public class FrmBaseDetail : FrmBase
    {
        #region Property

        /// <summary>
        /// Get or set Mode
        /// </summary>
        public Mode Mode
        {
            get { return base.GetValueViewState<Mode>("Mode"); }
            set
            {
                ViewState["Mode"] = value;
                if (typeof(SiteMaster) == this.Master.GetType().BaseType)
                {
                    SiteMaster master = (SiteMaster)this.Master;

                    switch (this.Mode)
                    {
                        case Mode.Insert:
                            master.LabelMode = "<label id='lblMode' runat='server' mode='insert' class='label label-primary'>新規</label>"; break;
                        case Mode.Revise:
                            master.LabelMode = "<label id='lblMode' runat='server' mode='revise' class='label label-primary'>REVISE</label>"; break;
                        case Mode.Update:
                            master.LabelMode = "<label id='lblMode' runat='server' mode='update' class='label label-warning'>編集</label>"; break;
                        case Mode.Copy:
                            master.LabelMode = "<label id='lblMode' runat='server' mode='copy' class='label label-success'>コピー</label>"; break;
                        case Mode.Delete:
                            master.LabelMode = "<label id='lblMode' runat='server' mode='delete' class='label label-danger'>削除</label>"; break;
                        case Mode.Check:
                            master.LabelMode = "<label id='lblMode' runat='server' mode='check' class='label label-warning'>チェック</label>"; break;
                        case Mode.Approve:
                            master.LabelMode = "<label id='lblMode' runat='server' mode='accept' class='label label-success'>承認</ label>"; break;
                        case Mode.NotApprove:
                            master.LabelMode = "<label id='lblMode' runat='server' mode='accept' class='label label-danger'>承認削除</ label>"; break;
                        default:
                            master.LabelMode = "<label id='lblMode' runat='server' mode='view' class='label label-view'>表示</label>"; break;
                    }
                }
            }
        }

        /// <summary>
        /// Get or ser IsShowQuestion
        /// </summary>
        public bool IsShowQuestion { get; set; }

        /// <summary>
        /// Get or ser DefaultButton
        /// </summary>
        public string DefaultButton { get; set; }

        /// <summary>
        /// Get or set Success
        /// </summary>
        public bool Success { get; set; }

        #endregion

        #region Method

        /// <summary>
        /// Show message question
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="defaultButton">Default Button</param>
        protected void ShowQuestionMessage(string messageID, DefaultButton defaultButton, bool isDelete = false, params string[] args)
        {
            this.Mode = this.Mode;

            //Get Message
            M_Message mess = (M_Message)this.Messages[messageID];
            HtmlGenericControl questionMessage = (HtmlGenericControl)this.Master.FindControl("questionMessage");
            questionMessage.InnerHtml = "<p>" + " " + string.Format(mess.Message3, args) + "</p>";
            this.IsShowQuestion = true;

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            Button btnNo1 = (Button)this.Master.FindControl("btnNo1");
            btnNo.Visible = isDelete;
            btnNo1.Visible = !isDelete;

            if (defaultButton == Models.DefaultButton.Yes)
            {
                this.DefaultButton = "#btnYes";
            }
            else
            {
                if (isDelete)
                {
                    this.DefaultButton = "#btnNo";
                }
                else
                {
                    this.DefaultButton = "#btnNo1";
                }
            }
        }

        #endregion
    }
}