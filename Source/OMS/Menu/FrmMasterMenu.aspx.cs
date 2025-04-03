using System;
using OMS.Models;
using OMS.Utilities;
using OMS.DAC;
using System.Collections.Generic;

namespace OMS.Menu
{
    public partial class FrmMasterMenu : FrmBase
    {
        #region Event

        /// <summary>
        /// Init page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "マスターメニュー";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string enableClass = "btn btn-default btn-lg btn-block";
            string disableClass = "btn btn-default btn-lg btn-block disabled";

            //User            
            base.SetAuthority(FormId.User);
            this.btnFrmUserList.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);

            //Department
            base.SetAuthority(FormId.Department);
            this.btnFrmDepartmentList.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);

            //GroupUser
            base.SetAuthority(FormId.GroupUser);
            this.btnFrmGroupUserList.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);

            //WorkingSystem
            base.SetAuthority(FormId.WorkingSystem);
            this.btnFrmWorkingSystem.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);

            //Information
            base.SetAuthority(FormId.Information);
            this.btnFrmInformation.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);

            //CompanyInfo
            base.SetAuthority(FormId.CompanyInfo);
            this.btnFrmCompanyInfo.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);
            
            //Setting
            base.SetAuthority(FormId.Setting);
            this.btnFrmSetting.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);
            
            this.btnFrmConfigList.Attributes.Add("class", base.IsAdmin() ? enableClass : disableClass);

            //Cost
            base.SetAuthority(FormId.Cost);
            this.btnFrmCostList.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);
        }

        #endregion

    }
}