<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmExpenceList.aspx.cs" Inherits="OMS.Expence.FrmExpenceList" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            <%
        if (this.IsShowQuestion == true)
        {
            %>
            $('#modalQuestion').modal('show');
            $('#modalQuestion').on('shown.bs.modal', function (e) {
                $('<%=this.DefaultButton%>').focus();
            });
            <%
        }

        if (this.Success == true)
        {
            %>
            showSuccess();
            setTimeout(function () {
                hideSuccess();
            }, 1000);
            <%
        }

        if (this.IsOutFile == true)
        {
            %>
            setTimeout(function () {
                getCtrlById("btnDownload").click();
                hideLoading();
            }, 0);
            <%
        }
            %>

            if ("<%= this.tabIndex%>" == "0") {
                $("#nav-apply").addClass("active");
                $("#tab-apply").addClass("active in");

                $("#nav-approve").removeClass("active");
                $("#tab-approve").removeClass("active in");
            } else {
                $("#nav-apply").removeClass("active");
                $("#tab-apply").removeClass("active in");

                $("#nav-approve").addClass("active");
                $("#tab-approve").addClass("active in");
            }
        })

        function showmodalDownLoad() {
            $('#modalDownLoad').modal('show');

            $(getCtrlById("radStatusSinsei")).prop('checked', true);
            $(getCtrlById("radAccountingDate")).prop('checked', true);
            $(getCtrlById("dtAccountingDateFrom")).val("");
            $(getCtrlById("dtAccountingDateTo")).val("");
            $(getCtrlById("dtUseDateFrom")).val("");
            $(getCtrlById("dtUseDateTo")).val("");

            setDisplayRadio(true);
        }
        var findBackCtr = "cmbDepartment";
        //**********************
        // Init
        //**********************
        $(function () {
            $(getCtrlById("chkSelectAll")).bind("change", function () {
                $(".selectFlg").prop("checked", this.checked);
            });

            selectFlg();
            setFocus();
            txtProjectCDOnChanged();
        });

        //**********************
        // Set Focus
        //**********************
        function setFocus() {

        }

        //**********************
        // Find Back
        //**********************
        function findBack() {
            hideLoading();

            getCtrlById(findBackCtr).focus().select();
        }

        //**********************
        // Clear Form
        //**********************
        function clearForm() {
            $(":text").val("");
            getCtrlById("cmbDepartment").val("-1");
            getCtrlById("cmbUser").val("-1");
            getCtrlById("cmbUser2").val("-1");
            getCtrlById("cmbExpenseType").val("-1");
            getCtrlById("txtDepartmentCode").focus();
        }

        //**********************
        // Call Search Project
        //**********************
        function callSearchProject() {
            findBackCtr = "cmbExpenseType";

            var projectCD = "";
            var projectName = "";
            var initDate = '1900/01/01';
            showSearchProject(projectCD, projectName, initDate, getCtrlById("txtProjectCode").attr("id"), getCtrlById("txtProjectName").attr("id"));
        };

        //**********************
        // Project CD On Changed
        //**********************
        function txtProjectCDOnChanged() {
            getCtrlById("txtProjectCode").bind("change", function () {

                if ($.trim(this.value) != "") {
                    var params = { 'projectCD': $.trim(this.value) };
                    ajax('GetProjectName', params, function (response) {
                        if (response.d) {
                            var result = eval('(' + response.d + ')');
                            getCtrlById("txtProjectCode").val(result['projectCD']);
                            getCtrlById("txtProjectName").val(result['projectNm']);
                        }
                    });
                } else {
                    getCtrlById("txtProjectName").val('');
                }
            });
        }
        //**********************
        // Init & Regist Event for selectFlg
        //**********************
        function selectFlg() {

            var checkAll = true;

            var minLength = 0;
            $(".selectFlg").each(function (i, ctr) {
                minLength = (i + 1);
                if (!$(ctr).is(":checked")) {
                    checkAll = false;
                    return;
                }
            }).bind("change", function () {
                var longId = $(this).attr("id");
                var shortId = "chkSelectlg";

                if ($(this).is(":checked")) {
                } else {
                    $(getCtrlById("chkSelectAll")).prop("checked", false);
                }

                if ($(".selectFlg:not(:checked)").length == 0) {
                    $(getCtrlById("chkSelectAll")).prop("checked", true);
                }
            });
            $(getCtrlById("chkSelectAll")).prop("checked", (checkAll && minLength > 0));
        }

        $(getCtrlById("Apply_tab")).click(function () {
            getCtrlById("BtnApply").click();
        });

        $(getCtrlById("Approved_tab")).click(function () {
            getCtrlById("BtnApproved").click();
        });

        $(getCtrlById("radAccountingDate")).on('change', function () {
            if ($(this).is(':checked')) {
                setDisplayRadio(true);
            }
        });

        $(getCtrlById("radUseDate")).on('change', function () {
            if ($(this).is(':checked')) {
                setDisplayRadio(false);
            }
        });

        function setDisplayRadio(isAccountingDate) {

            $(getCtrlById("dtAccountingDateFrom")).prop("disabled", !isAccountingDate);
            $(getCtrlById("dtAccountingDateTo")).prop("disabled", !isAccountingDate);

            $(getCtrlById("dtUseDateFrom")).prop("disabled", isAccountingDate);
            $(getCtrlById("dtUseDateTo")).prop("disabled", isAccountingDate);

            if (!isAccountingDate) {
                $(getCtrlById("dtAccountingDateFrom")).val("");
                $(getCtrlById("dtAccountingDateTo")).val("");
            } else {
                $(getCtrlById("dtUseDateFrom")).val("");
                $(getCtrlById("dtUseDateTo")).val("");
            }
        }

    </script>
    <style>
        div.text-ellipsis {
            white-space: pre-wrap; /* css-3 */
            white-space: -moz-pre-wrap; /* Mozilla, since 1999 */
            word-wrap: break-word; /* Internet Explorer 5.5+ */
        }

        .form-check-input label {
            font-weight: normal;
        }

        @media (max-width:767px) {
            .modalExport .modal-dialog {
                margin: auto 15%;
                width: 70%;
                height: 450px;
            }
        }

        @media (min-width: 768px) {
            .modalExport .modal-dialog {
                margin: auto 15%;
                width: 70%;
                height: 450px;
            }

            #modalDownLoad .modal-body {
                overflow-y: visible;
            }
        }

        @media (min-width: 992px) {
            .modalExport .modal-dialog {
                margin: auto 20%;
                width: 60%;
                height: 450px;
            }

            #modalDownLoad .modal-body {
                overflow-y: visible;
            }
        }

        @media (min-width: 1200px) {
            .modalExport .modal-dialog {
                margin: auto 27.5%;
                width: 45%;
                height: 450px;
            }

            #modalDownLoad .modal-body {
                overflow-y: visible;
            }
        }

        .modal-body
        {
            overflow-y: hidden;
        }

        @media (max-width:767px) {
            #modalQuestion .modal-dialog {
                margin: auto 15%;
                width: 70%;
            }
        }

        @media (min-width: 768px) {
            #modalQuestion .modal-dialog {
                margin: auto 15%;
                width: 70%;
            }
        }

        @media (min-width: 992px) {
            #modalQuestion .modal-dialog {
                margin: auto 25%;
                width: 50%;
            }
        }

        @media (min-width: 1290px) {
            #modalQuestion .modal-dialog {
                margin: auto 30%;
                width: 30%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <%-- Search Condition--%>
    <div class="well well-sm">
        <!--Collapse Button-->
        <div class="row">
            <div class="col-md-12">
                <button id="BtnApply" type="button" style="display: none" class="btn btn-default btn-sm" data-toggle="collapse"
                    runat="server">
                </button>
                <button id="BtnApproved" type="button" style="display: none" class="btn btn-default btn-sm" data-toggle="collapse"
                    runat="server">
                </button>
                <button id="viewDetailPress" type="button" class="btn btn-default btn-sm" data-toggle="collapse"
                    data-target="#viewdetails">
                    <span class="glyphicon glyphicon-align-justify"></span>
                </button>
            </div>
        </div>
        <div class="collapse in" id="viewdetails">
            <div class="row">
                <%--Department Code--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbDepartment.ClientID %>">
                            負担部門
                        </label>
                        <asp:DropDownList ID="cmbDepartment" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--Department Name--%>
                <div class="col-md-4">
                </div>
                <%--Project Code--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtProjectCode.ClientID %>">
                            負担プロジェクト
                        </label>
                        <div class="input-group">
                            <span class="input-group-btn">
                                <button id="btnProjectCode" class="btn btn-default btn-sm loading" type="button" onclick="callSearchProject(); return false;">
                                    <span class="glyphicon glyphicon-search"></span>
                                </button>
                            </span>
                            <cc1:ICodeTextBox ID="txtProjectCode" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm" SearchButtonID="btnProjectCode" AllowChars="-" />
                        </div>
                    </div>
                </div>
                <%--Project Name--%>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtProjectName.ClientID %>">
                            &nbsp;
                        </label>
                        <cc1:ITextBox ID="txtProjectName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <%--Expense type--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbExpenseType.ClientID %>">
                            経費種類</label>
                        <asp:DropDownList ID="cmbExpenseType" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--Space--%>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label">
                            &nbsp;
                        </label>
                    </div>
                </div>
                <%--User code--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbUser.ClientID %>">
                            支払先（社員）
                        </label>
                        <asp:DropDownList ID="cmbUser" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--Kara--%>
                <div class="col-md-1">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                    </div>
                </div>
                <%--User code--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbUser2.ClientID %>">
                            承認者
                        </label>
                        <asp:DropDownList ID="cmbUser2" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--User name --%>
                <div class="col-md-4">
                </div>
            </div>
            <div class="row">
                <%--Accounting date 1--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= dtAccountingDate1.ClientID %>">
                            計上日</label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtAccountingDate1" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <%--Kara--%>
                <div class="col-md-1">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>
                <%--Accounting date 2--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label">
                            <strong class="text-danger">&nbsp</strong></label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtAccountingDate2" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                </div>
                <%--The date of use 1--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= dtTheDateOfUse1.ClientID %>">
                            利用日</label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtTheDateOfUse1" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <%--Kara--%>
                <div class="col-md-1">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>
                <%--The date of use 2--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label">
                            <strong class="text-danger">&nbsp</strong></label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtTheDateOfUse2" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <%--Search and Clear Button--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="btn-group">
                        <button type="button" id="btnSearch" class="btn btn-default btn-sm loading" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;検索
                        </button>
                        <button type="button" class="btn btn-default btn-sm" onclick="clearForm();">
                            <span class="glyphicon glyphicon-refresh"></span>&nbsp;クリア
                        </button>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <%--Create New and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-7">
                <div class="btn-group btn-group-justified">
                    <%-- New button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmExpenceEntry.aspx" OnCommand="btnNew_Click"
                            CssClass="btn btn-primary btn-sm loading">
                            <span class="glyphicon glyphicon-plus"></span>&nbsp;新規
                        </asp:LinkButton>
                    </div>
                    <%-- Excel --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnExcel" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnExcel_Command">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</asp:LinkButton>
                        <button type="button" id="btnDownload" class="btn btn-default btn-sm hide" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;Download
                        </button>
                    </div>
                    <%-- Excel2 --%>
                    <div class="btn-group">
                        <%if (base._authority.IsExpenceExportExcel2)
                        {%>
                        <button id="BntExcelBottom" type="button" class="btn btn-sm btn-default text-left" onclick="showmodalDownLoad();">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;精算書出力</button>
                        <%}
                        else
                        { %>
                        <button id="Button1" disabled type="button" class="btn btn-sm btn-default text-left" onclick="showmodalDownLoad();">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;精算書出力</button>
                        <%} %>
                    </div>
                    <%-- Back button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" PostBackUrl="../Menu/FrmMainMenu.aspx"
                            CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
            </div>
            <div class="col-md-1">
                <div class="btn-group" style="float: right">
                    <asp:LinkButton ID="btnAccept" runat="server" Text="一括承認" Width="200" CssClass="btn btn-success btn-sm loading" OnClick="btnAccept_Click">
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <uc2:PagingHeaderControl ID="PagingHeader" runat="server" />
    <%-- List Detail--%>

    <%--Nav tabs--%>
    <div id="Tabs" role="tabpanel">
        <ul class="nav nav-tabs" id="myTab" role="tablist">
            <li id="nav-apply" class="nav-item">
                <a class="nav-link" id="Apply_tab" data-toggle="tab" href="#Apply" role="tab" aria-controls="Apply" aria-selected="true" runat="server">申請中</a>
            </li>
            <li id="nav-approve" class="nav-item">
                <a class="nav-link" id="Approved_tab" data-toggle="tab" href="#Approved" role="tab" aria-controls="Approved" aria-selected="false" runat="server">承認済</a>
            </li>
        </ul>
        <%--Tab panes --%>
        <div class="tab-content" id="myTabContent">
            <div class="tab-pane fade" id="tab-apply" role="tabpanel" aria-labelledby="Apply-tab">
                <div class="form-group scrollmenu2" style="overflow: auto; white-space: nowrap;">
                    <asp:Repeater ID="rptProjectList" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped table-condensed" style="margin-bottom: 0px;">
                                <thead id="headertemplate">
                                    <tr class="active">
                                        <th class="v-algin text-center">#
                                        </th>
                                        <th class="v-algin text-center">
                                            <input type="checkbox" id="chkSelectAll" runat="server" />
                                        </th>
                                        <th class="v-algin text-center">詳細
                                        </th>
                                        <th class="v-algin">
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='ExpenceNo'
                                                OnClick="Sort_Click" class="loading">
                                            <%  if (this.SortField == "ExpenceNo")
                                                {
                                                    if (this.SortDirec == "1")
                                                    { %>
                                                        経費番号 <span class='glyphicon glyphicon-arrow-up'></span>
                                            <%      }
                                                else
                                                { %>
                                                        経費番号 <span class='glyphicon glyphicon-arrow-down'></span>
                                            <%      }
                                                }
                                                else
                                                { %>
                                                    経費番号
                                            <%  }%>
                                            </asp:LinkButton>
                                        </th>
                                        <th class="v-algin">
                                            <asp:LinkButton ID="LinkButton4" Text="計上日" runat="server" CommandArgument='Date'
                                                OnClick="Sort_Click" class="loading">
                                            <%  if (this.SortField == "Date")
                                                {
                                                    if (this.SortDirec == "1")
                                                    { %>
                                                        計上日 <span class='glyphicon glyphicon-arrow-up'></span>
                                            <%      }
                                                else
                                                { %>
                                                        計上日 <span class='glyphicon glyphicon-arrow-down'></span>
                                            <%      }
                                                }
                                                else
                                                { %>
                                                    計上日
                                            <%  }%>
                                            </asp:LinkButton>
                                        </th>
                                        <th class="v-algin">負担部門</th>
                                        <th class="v-algin">負担プロジェクト</th>
                                        <th class="v-algin">経費種類</th>
                                        <th class="v-algin text-right">金額</th>
                                        <th class="v-algin">支払先（社員）</th>
                                        <th class="v-algin">承認予定</th>
                                        <th class="v-algin text-center">状況</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="white-space: nowrap;">
                                <asp:HiddenField ID="hiddenID" runat="server" Value='<%# Eval("ID")%>' />
                                <td>
                                    <%# Eval("RowNumber") %>
                                </td>

                                <td class="text-center">
                                    <input id="chkSelectlg" class="selectFlg" type="checkbox" runat="server" checked='<%# Eval("CheckFlag")%>' />
                                </td>
                                <td class="text-center">
                                    <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") %>'
                                        PostBackUrl="FrmExpenceEntry.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading">
                                    <span class="glyphicon glyphicon-pencil"></span>
                                    </asp:LinkButton>
                                </td>
                                <td><%# Server.HtmlEncode(Eval("ExpenceNo", "{0}"))%></td>
                                <td><%# Server.HtmlEncode(Eval("DateStr", "{0}"))%></td>
                                <td><%# Server.HtmlEncode(Eval("DepartmentName", "{0}"))%></td>
                                <td><div class="text-ellipsis"><%# Server.HtmlEncode(Eval("ProjectName", "{0}"))%></div></td>
                                <td><%# Server.HtmlEncode(Eval("Value2", "{0}"))%></td>
                                <td class="text-right">
                                    <%# Eval("ExpenceAmount", "{0:#,##0}")%>
                                </td>
                                <td>
                                    <%# Server.HtmlEncode(Eval("UserName1", "{0}"))%>
                                </td>
                                <td>
                                    <%# Server.HtmlEncode(Eval("UserName3", "{0}"))%>
                                </td>
                                <td class="text-center">
                                    <span class="label label-warning">申請中</span>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div class="tab-pane fade " id="tab-approve" role="tabpanel" aria-labelledby="Approved-tab">
                <div class="form-group scrollmenu2" style="overflow: auto; white-space: nowrap;">
                    <asp:Repeater ID="rptProject_ApprovedList" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped table-condensed" style="margin-bottom: 0px;">
                                <thead id="headertemplate">
                                    <tr class="active">
                                        <th class="v-algin text-center">#
                                        </th>
                                        <th class="v-algin text-center">詳細
                                        </th>
                                        <th class="v-algin">
                                            <asp:LinkButton ID="LinkButton1" Text='経費番号' runat="server" CommandArgument='ExpenceNo'
                                                OnClick="Sort_Click" class="loading">
                                            <%  if (this.SortField == "ExpenceNo")
                                                {
                                                    if (this.SortDirec == "1")
                                                    { %>
                                                        経費番号 <span class='glyphicon glyphicon-arrow-up'></span>
                                            <%      }
                                                else
                                                { %>
                                                        経費番号 <span class='glyphicon glyphicon-arrow-down'></span>
                                            <%      }
                                                }
                                                else
                                                { %>
                                                    経費番号
                                            <%  }%>
                                            </asp:LinkButton>
                                        </th>
                                        <th class="v-algin">
                                            <asp:LinkButton ID="LinkButton2" Text="計上日" runat="server" CommandArgument='Date'
                                                OnClick="Sort_Click" class="loading">
                                            <%  if (this.SortField == "Date")
                                                {
                                                    if (this.SortDirec == "1")
                                                    { %>
                                                        計上日 <span class='glyphicon glyphicon-arrow-up'></span>
                                            <%      }
                                                else
                                                { %>
                                                        計上日 <span class='glyphicon glyphicon-arrow-down'></span>
                                            <%      }
                                                }
                                                else
                                                { %>
                                                    計上日
                                            <%  }%>
                                            </asp:LinkButton>
                                        </th>
                                        <th class="v-algin">負担部門</th>
                                        <th class="v-algin">負担プロジェクト</th>
                                        <th class="v-algin">経費種類</th>
                                        <th class="v-algin text-right">金額</th>
                                        <th class="v-algin">支払先（社員）</th>
                                        <th class="v-algin">承認者</th>
                                        <th class="v-algin text-center">状況</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="white-space: nowrap;">
                                <asp:HiddenField ID="hinUID" runat="server" Value='<%# Eval("ID")%>' />
                                <td>
                                    <%# Eval("RowNumber") %>
                                </td>
                                 <td class="text-center">
                                    <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") %>'
                                        PostBackUrl="FrmExpenceEntry.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading">
                                    <span class="glyphicon glyphicon-pencil"></span>
                                    </asp:LinkButton>
                                </td>
                                <td><%# Server.HtmlEncode(Eval("ExpenceNo", "{0}"))%></td>
                                <td><%# Server.HtmlEncode(Eval("DateStr", "{0}"))%></td>
                                <td><%# Server.HtmlEncode(Eval("DepartmentName", "{0}"))%></td>
                                <td><div class="text-ellipsis"><%# Server.HtmlEncode(Eval("ProjectName", "{0}"))%></div></td>
                                <td><%# Server.HtmlEncode(Eval("Value2", "{0}"))%></td>
                                <td class="text-right"><%#  Eval("ExpenceAmount", "{0:#,##0}")%></td>
                                <td><%# Server.HtmlEncode(Eval("UserName1", "{0}"))%></td>
                                <td><%# Server.HtmlEncode(Eval("UserName2", "{0}"))%></td>
                                <td class="text-center">
                                    <span class="label label-primary">承認済</span>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
    <%--(Excel)--%>
    <div id="modalDownLoad" class="modalExport modal fade" role="dialog" data-backdrop="static">
        <div class="modal-dialog modal-lg modal-dialog-centered">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">交通・宿泊費清算書出力</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <div class="row form-group">
                            <div class="col-md-12">
                                <label>申請状況</label>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12">
                                <div class="radio-inline">
                                    <asp:RadioButton ID="radStatusSinsei" CssClass="form-check-input" GroupName="radStatus" Text="&nbsp;申請中" Value="1" runat="server" />
                                </div>
                                <div class="radio-inline">
                                    <asp:RadioButton ID="radStatusSyonin" CssClass="form-check-input" GroupName="radStatus" Text="&nbsp;承認済" Value="2" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12">
                                <label>集計期間</label>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3">
                                <div class="form-check">
                                    <asp:RadioButton ID="radAccountingDate" CssClass="form-check-input" GroupName="radDate" Text="&nbsp;計上日で集計" Value="1" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="input-group date">
                                    <cc1:IDateTextBox ID="dtAccountingDateFrom" runat="server" CssClass="form-control input-sm"
                                        PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD" />
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                            <div class="col-md-1">
                                ～
                            </div>
                            <div class="col-md-4">
                                <div class="input-group date">
                                    <cc1:IDateTextBox ID="dtAccountingDateTo" runat="server" CssClass="form-control input-sm"
                                        PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD" />
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3">
                                <div class="form-check">
                                    <asp:RadioButton ID="radUseDate" CssClass="form-check-input" GroupName="radDate" Text="&nbsp;利用日で集計" Value="2" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="input-group date">
                                    <cc1:IDateTextBox ID="dtUseDateFrom" runat="server" CssClass="form-control input-sm"
                                        PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD" />
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                            <div class="col-md-1">
                                ～
                            </div>
                            <div class="col-md-4">
                                <div class="input-group date">
                                    <cc1:IDateTextBox ID="dtUseDateTo" runat="server" CssClass="form-control input-sm"
                                        PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD" />
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3">
                                <label class="control-label">対象社員</label>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-4">
                                <asp:DropDownList ID="cmbUser3" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">キャンセル</button>
                    <asp:LinkButton ID="btnExel" runat="server" CssClass="btn btn-primary btn-sm loading" OnClientClick="$('#modalDownLoad').modal('hide');" OnCommand="btnExcel2_Click">
                        &nbsp;出力
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <uc1:PagingFooterControl ID="PagingFooter" runat="server" />
    <asp:HiddenField ID="InitDateHidden" runat="server" />
</asp:Content>
