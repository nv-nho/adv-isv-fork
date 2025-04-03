using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using OMS.Models;
using OMS.DAC;

namespace OMS.Search
{
    /// <summary>
    /// UserSearch
    /// TRAM
    /// </summary>
    public partial class FrmUserSearch : FrmBaseList
    {
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
        public IList<UserSearchInfo> listUserInfo;

        #region Event

        /// <summary>
        /// Event Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            // header grid sort
            this.HeaderGrid.OnSortClick += Sort_Click;

            // paging header
            this.PagingHeader.OnClick += PagingHeader_Click;
            this.PagingHeader.OnPagingClick += Paging_Click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;

            // Paging footer
            this.PagingFooter.OnClick += Paging_Click;

            //Search button
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Init Max Length
            this.txtUserCD.MaxLength = M_User.MAX_USER_CODE_SHOW;
            this.txtLoginID.MaxLength = M_User.LOGIN_ID_MAX_LENGTH;
            this.txtUserName1.MaxLength = M_User.USER_NAME_1_MAX_LENGTH;
            //this.txtUserName2.MaxLength = M_User.USER_NAME_2_MAX_LENGTH;
            this.txtGroupCD.MaxLength = M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH;
            this.txtDepartmentCD.MaxLength = M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH;
        }

        /// <summary>
        /// Load Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // header grid
                this.HeaderGrid.SortDirec = "1";
                this.HeaderGrid.SortField = "1";
                // set default paging header
                this.PagingHeader.IsCloseForm = true;
                //this.PagingHeader.AddClass = "btn-success";

                //Set data into control
                this.InitData();

                //Load data into grid
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);

            }
        }

        /// <summary>
        /// Process the button Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Refresh sort header
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "1";

            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
            this.Collapse = "in";
        }

        /// <summary>
        /// Paging Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Paging_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this.LoadDataGrid(int.Parse((sender as LinkButton).CommandArgument), this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Click on the paging header
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
        /// Sorting on the repeater header
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
        /// Init Data
        /// </summary>
        private void InitData()
        {
            #region IN
            //Set UserCD
            if (Request.QueryString["in1"] != null)
            {
                this.txtUserCD.Value = Request.QueryString["in1"];
            }

            //Set UserName1
            if (Request.QueryString["in2"] != null)
            {
                this.txtUserName1.Value = Request.QueryString["in2"];
            }

            //Set UserName2
            if (Request.QueryString["in3"] != null)
            {
                //this.txtUserName2.Value = Request.QueryString["in3"];
                this.txtUserName1.Value = Request.QueryString["in3"];
            }

            //Set GroupCd
            if (Request.QueryString["in4"] != null)
            {
                this.txtGroupCD.Value = Request.QueryString["in4"];

                //Get GroupName
                if (!this.txtGroupCD.IsEmpty)
                {
                    M_GroupUser_H groupUser;
                    GroupUserService groupService;
                    using (DB db = new DB())
                    {
                        groupService = new GroupUserService(db);
                        groupUser = groupService.GetByGroupCD(Utilities.EditDataUtil.ToFixCodeDB(this.txtGroupCD.Value, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH));
                        if (groupUser != null)
                        {
                            this.txtGroupName.Value = groupUser.GroupName;
                        }
                    }
                }
                else
                {
                    this.txtGroupName.Value = string.Empty;
                }

            }

            //Set DepartmentCd
            if (Request.QueryString["in5"] != null)
            {
                this.txtDepartmentCD.Value = Request.QueryString["in5"];

                //Get GroupName
                if (!this.txtDepartmentCD.IsEmpty)
                {
                    M_Department departmentUser;
                    DepartmentService departmentService;
                    using (DB db = new DB())
                    {
                        departmentService = new DepartmentService(db);
                        departmentUser = departmentService.GetByDepartmentCd(this.txtDepartmentCD.Value);
                        if (departmentUser != null)
                        {
                            this.txtDepartmentName.Value = departmentUser.DepartmentName;
                        }
                    }
                }
                else
                {
                    this.txtDepartmentName.Value = string.Empty;
                }

            }
            #endregion

            #region OUT

            ////Set UserCdCtrl
            if (Request.QueryString["out1"] != null)
            {
                this.Out1.Value = Request.QueryString["out1"];
            }

            //Set UserName1Ctrl
            if (Request.QueryString["out2"] != null)
            {
                this.Out2.Value = Request.QueryString["out2"];
            }

            //Set UserName2Ctrl
            if (Request.QueryString["out3"] != null)
            {
                this.Out3.Value = Request.QueryString["out3"];
            }

            //Set GroupCdCtrl
            if (Request.QueryString["out4"] != null)
            {
                this.Out4.Value = Request.QueryString["out4"];
            }

            //Set DepartmentCdCtrl
            if (Request.QueryString["out5"] != null)
            {
                this.Out5.Value = Request.QueryString["out5"];
            }
            #endregion
        }
        /// <summary>
        /// Load data into grid
        /// </summary>
        /// <param name="pageIndex"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            string userCd = null;
            string loginID = null;
            string userName1 = null;
            //string userName2 = null;
            string groupCd = null;
            string departmentCd = null;
            int totalRow = 0;
            if (!this.txtUserCD.IsEmpty)
            {
                userCd = Utilities.EditDataUtil.ToFixCodeDB(this.txtUserCD.Value, M_User.USER_CODE_MAX_LENGTH);
            }

            if (!this.txtLoginID.IsEmpty)
            {
                loginID = this.txtLoginID.Value;
            }

            if (!this.txtUserName1.IsEmpty)
            {
                userName1 = this.txtUserName1.Value;
            }

            //if (!this.txtUserName2.IsEmpty)
            //{
            //    userName2 = this.txtUserName2.Value;
            //}

            if (!this.txtGroupCD.IsEmpty)
            {
                groupCd = Utilities.EditDataUtil.ToFixCodeDB(this.txtGroupCD.Value, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH);
            }

            if (!this.txtDepartmentCD.IsEmpty)
            {
                departmentCd = this.txtDepartmentCD.Value;
            }

            using (DB db = new DB())
            {
                UserService userService = new UserService(db);
                totalRow = userService.GetCountByConditionForSearch(userCd, loginID, userName1, groupCd, departmentCd);

                this.listUserInfo = userService.GetListByConditionForSearch(userCd, loginID, userName1, groupCd,departmentCd, pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
            }

            if (this.listUserInfo != null && this.listUserInfo.Count != 0)
            {
                this.PagingHeader.RowNumFrom = int.Parse(this.listUserInfo[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(this.listUserInfo[listUserInfo.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = totalRow;
                this.PagingHeader.CurrentPage = pageIndex;

                // paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = totalRow;

                // header
                this.HeaderGrid.TotalRow = totalRow;
                this.HeaderGrid.AddColumms(new string[] { "#", "", "CD", "Login-ID", "User Name 1", "User Name 2", "Group Name","Department Name" });
            }

            this.Collapse = this.listUserInfo.Count > 0 ? string.Empty : "in";
            this.rptUserList.DataSource = this.listUserInfo;
            this.rptUserList.DataBind();
        }

        #endregion

        #region Web Methods

        /// <summary>
        /// Show Group Name
        /// </summary>
        /// <param name="in1">GroupCD</param>
        /// <returns>GroupCD</returns>
        [System.Web.Services.WebMethod]
        public static string ShowGroupName(string in1)
        {
            try
            {
                var dbGroupCD = in1;
                dbGroupCD = OMS.Utilities.EditDataUtil.ToFixCodeDB(dbGroupCD, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH);
                in1 = OMS.Utilities.EditDataUtil.ToFixCodeShow(dbGroupCD, M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH);
                using (DB db = new DB())
                {
                    GroupUserService service = new GroupUserService(db);

                    var groupUser = service.GetByGroupCD(dbGroupCD);
                    if (groupUser != null)
                    {
                        var result = new
                        {
                            groupCD = in1,
                            groupName = groupUser.GroupName
                        };
                        return OMS.Utilities.EditDataUtil.JsonSerializer<object>(result);
                    }
                    var onlyCd = new
                    {
                        groupCD = in1
                    };
                    return OMS.Utilities.EditDataUtil.JsonSerializer<object>(onlyCd);
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Show Department Name
        /// </summary>
        /// <param name="in1">DepartmentCD</param>
        /// <returns>DepartmentNm</returns>
        [System.Web.Services.WebMethod]
        public static string ShowDepartmentName(string in1)
        {
            try
            {
                var dbDepartmentCD = in1;   
                using (DB db = new DB())
                {
                    DepartmentService service = new DepartmentService(db);

                    var departmentUser = service.GetByDepartmentCd(dbDepartmentCD);
                    if (departmentUser != null)
                    {
                        var result = new
                        {
                            departmentCD = in1,
                            departmentName = departmentUser.DepartmentName
                        };
                        return OMS.Utilities.EditDataUtil.JsonSerializer<object>(result);
                    }
                    var onlyCd = new
                    {
                        departmentCD = in1
                    };
                    return OMS.Utilities.EditDataUtil.JsonSerializer<object>(onlyCd);
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