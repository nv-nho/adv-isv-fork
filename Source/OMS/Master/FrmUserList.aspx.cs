using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using OMS.Models;
using OMS.DAC;
using System.Collections;
using OMS.Utilities;

namespace OMS.Master
{
    /// <summary>
    /// Class Form User List
    /// ISV-TRUC
    /// </summary>
    public partial class FrmUserList : FrmBaseList
    {
        private const string CONST_DANGER_TEXT = "無効";
        #region Property

        /// <summary>
        /// Get or set Collapse
        /// </summary>
        public string Collapse
        {
            get { return (string)ViewState["Collapse"]; }
            set { ViewState["Collapse"] = value; }
        }

        #endregion

        #region Event

        /// <summary>
        /// Event Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "社員一覧";
            base.FormSubTitle = "List";

            // header grid sort
            this.HeaderGrid.OnSortClick += Sort_Click;

            // paging footer
            this.PagingFooter.OnClick += PagingFooter_Click;

            // paging header
            this.PagingHeader.OnClick += PagingHeader_Click;
            this.PagingHeader.OnPagingClick += PagingFooter_Click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;
            this.PagingHeader.IsShowColor = true;
            this.PagingHeader.DangerText = CONST_DANGER_TEXT;

            //Search button
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Init Max Length
            this.txtUserName1.MaxLength = M_User.USER_NAME_1_MAX_LENGTH;
            //this.txtUserName2.MaxLength = M_User.USER_NAME_2_MAX_LENGTH;
            this.txtGroupCode.MaxLength = M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH;
            this.txtGroupName.MaxLength = M_GroupUser_H.GROUP_NAME_MAX_LENGTH;

            this.txtDepartmentCode.MaxLength = M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH;
            this.txtDepartmentName.MaxLength = M_Department.DEPARTMENT_NAME_MAX_LENGTH;
        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetAuthority(FormId.User);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            }

            if (!this.IsPostBack)
            {
                //Init data
                this.InitData();

                //Show condition
                if (this.PreviousPage != null)
                {
                    if (this.PreviousPageViewState["Condition"] != null)
                    {
                        Hashtable data = (Hashtable)PreviousPageViewState["Condition"];

                        this.ShowCondition(data);
                    }
                }

                //Show data on grid
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);

                this.Collapse = string.Empty;
            }
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Refresh sort header
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "3";

            // Refresh load grid
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);

            this.Collapse = "in";
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, CommandEventArgs e)
        {
            //Save condition
            this.SaveCondition();
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, CommandEventArgs e)
        {
            //UserID
            this.ViewState["ID"] = e.CommandArgument;

            //Save condition
            this.SaveCondition();
        }

        /// <summary>
        /// Click PagingFooter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PagingFooter_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                int curPage = int.Parse((sender as LinkButton).CommandArgument);
                this.PagingFooter.CurrentPage = curPage;
                this.PagingHeader.CurrentPage = curPage;
                this.LoadDataGrid(curPage, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Click PagingHeader
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PagingHeader_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Click Sort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Click(object sender, EventArgs e)
        {
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        #endregion

        #region Method

        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondition()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.txtUserName1.ID, this.txtUserName1.Value);
            //hash.Add(this.txtUserName2.ID, this.txtUserName2.Value);
            hash.Add(this.txtGroupCode.ID, this.txtGroupCode.Value);
            hash.Add(this.txtGroupName.ID, this.txtGroupName.Value);
            hash.Add(this.txtDepartmentCode.ID, this.txtDepartmentCode.Value);
            hash.Add(this.txtDepartmentName.ID, this.txtDepartmentName.Value);
            hash.Add(this.cmbInvalidData.ID, this.cmbInvalidData.SelectedValue);
            hash.Add("NumRowOnPage", this.PagingHeader.NumRowOnPage);
            hash.Add("CurrentPage", this.PagingHeader.CurrentPage);

            hash.Add("SortField", this.HeaderGrid.SortField);
            hash.Add("SortDirec", this.HeaderGrid.SortDirec);

            this.ViewState["Condition"] = hash;
        }

        /// <summary>
        /// Show Condition
        /// </summary>
        private void ShowCondition(Hashtable data)
        {
            this.txtUserName1.Value = data[this.txtUserName1.ID].ToString();
            //this.txtUserName2.Value = data[this.txtUserName2.ID].ToString();
            this.txtGroupCode.Value = data[this.txtGroupCode.ID].ToString();
            this.txtGroupName.Value = data[this.txtGroupName.ID].ToString();
            this.txtDepartmentCode.Value = data[this.txtDepartmentCode.ID].ToString();
            this.txtDepartmentName.Value = data[this.txtDepartmentName.ID].ToString();
            this.cmbInvalidData.SelectedValue = data[this.cmbInvalidData.ID].ToString();

            int curPage = int.Parse(data["CurrentPage"].ToString());
            this.PagingHeader.CurrentPage = curPage;
            this.PagingFooter.CurrentPage = curPage;

            int rowOfPage = int.Parse(data["NumRowOnPage"].ToString());
            this.PagingHeader.NumRowOnPage = rowOfPage;

            this.HeaderGrid.SortField = data["SortField"].ToString();
            this.HeaderGrid.SortDirec = data["SortDirec"].ToString();
        }

        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            // Default data valide
            this.hdInValideDefault.Value = this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_INVALID_TYPE);

            //Invalid
            this.InitCombobox(this.cmbInvalidData);

            // header grid
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "3";

            //this.btnNew.Attributes.Add("class", this.CheckAuthorityMaster(FormId.User, AuthorTypeMaster.New) ? Constants.CSS_BTN_NEW : Constants.CSS_BTN_NEW_DISABLED);
            //this.btnNew.Attributes.Add("class", this._authority.IsMasterNew ? Constants.CSS_BTN_NEW : Constants.CSS_BTN_NEW_DISABLED);
            base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
        }


        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl)
        {
            // init combox 
            ddl.DataSource = this.GetDataForDropdownList(M_Config_H.CONFIG_CD_INVALID_TYPE);
            ddl.DataValueField = "Value";
            ddl.DataTextField = "DisplayName";
            ddl.DataBind();
            ddl.SelectedValue = this.hdInValideDefault.Value;
        }

        /// <summary>
        /// GetDataForDropdownList
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private IList<DropDownModel> GetDataForDropdownList(string configCD)
        {
            using (DB db = new DB())
            {
                Config_HService configSer = new Config_HService(db);
                return configSer.GetDataForDropDownList(configCD);

            }
        }

        /// <summary>
        /// GetDefaultValueForDropdownList
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private string GetDefaultValueForDropdownList(string configCD)
        {
            using (DB db = new DB())
            {
                Config_HService configSer = new Config_HService(db);
                return configSer.GetDefaultValueDrop(configCD);
            }
        }


        /// <summary>
        /// load data grid
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            int totalRow = 0;

            IList<UserInfo> listUserInfo;

            //Get data
            using (DB db = new DB())
            {
                UserService userService = new UserService(db);
                totalRow = userService.getTotalRow(txtUserName1.Value, this.txtGroupCode.Value, this.txtGroupName.Value,this.txtDepartmentCode.Value,this.txtDepartmentName.Value, cmbInvalidData.SelectedItem.Value);

                listUserInfo = userService.GetListByCond(txtUserName1.Value, this.txtGroupCode.Value, this.txtGroupName.Value, this.txtDepartmentCode.Value, this.txtDepartmentName.Value, cmbInvalidData.SelectedItem.Value,
                                                         pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
            }

            //Show data
            if (listUserInfo.Count == 0)
            {
                this.rptUserList.DataSource = null;
            }
            else
            {
                // paging header
                this.PagingHeader.RowNumFrom = int.Parse(listUserInfo[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(listUserInfo[listUserInfo.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = totalRow;
                this.PagingHeader.CurrentPage = pageIndex;

                // paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = totalRow;

                // header
                this.HeaderGrid.TotalRow = totalRow;
                //this.HeaderGrid.AddColumms(new string[] { "#", "", "CD", "Login-ID", "User Name 1", "User Name 2", "Group", "Valid" });
                this.HeaderGrid.AddColumms(new string[] { "#", "", "CD", "ログインID", "社員名", "部門", "カレンダー", "権限グループ", "N#有給休暇の残日数" });
                //HeaderGrid.Columns[6].Sorting = false;

                // detail
                this.rptUserList.DataSource = listUserInfo;
            }

            this.rptUserList.DataBind();
        }

        #endregion

        #region Web Methods

        /// <summary>
        /// Get Group Name By Group Code
        /// </summary>
        /// <param name="groupCd">Group Code</param>
        /// <returns>Group Name</returns>
        [System.Web.Services.WebMethod]
        public static string GetGroupName(string groupCD)
        {
            var groupCd = groupCD;
            var groupCdShow = groupCD;
            groupCd = OMS.Utilities.EditDataUtil.ToFixCodeDB(groupCd, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH);
            groupCdShow = EditDataUtil.ToFixCodeShow(groupCd, M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH);

            try
            {
                using (DB db = new DB())
                {
                    GroupUserService grpSer = new GroupUserService(db);
                    M_GroupUser_H model = grpSer.GetByGroupCD(groupCd);
                    if (model != null)
                    {
                        var result = new
                        {
                            groupCD = groupCdShow,
                            groupNm = model.GroupName
                        };
                        return OMS.Utilities.EditDataUtil.JsonSerializer<object>(result);
                    }
                    var onlyCD = new
                    {
                        groupCD = groupCdShow,
                        groupNm = string.Empty
                    };
                    return OMS.Utilities.EditDataUtil.JsonSerializer<object>(onlyCD);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Group Name By Group Code
        /// </summary>
        /// <param name="groupCd">Group Code</param>
        /// <returns>Group Name</returns>
        [System.Web.Services.WebMethod]
        public static string GetDepartmentName(string departmentCD)
        {
            var departmentCd = departmentCD;
            var departmentCdShow = departmentCD;

            try
            {
                using (DB db = new DB())
                {
                    DepartmentService grpSer = new DepartmentService(db);
                    M_Department model = grpSer.GetByDepartmentCd(departmentCd);
                    if (model != null)
                    {
                        var result = new
                        {
                            departmentCD = departmentCdShow,
                            departmentNm = model.DepartmentName
                        };
                        return OMS.Utilities.EditDataUtil.JsonSerializer<object>(result);
                    }
                    var onlyCD = new
                    {
                        departmentCD = departmentCdShow,
                        departmentNm = string.Empty
                    };
                    return OMS.Utilities.EditDataUtil.JsonSerializer<object>(onlyCD);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}