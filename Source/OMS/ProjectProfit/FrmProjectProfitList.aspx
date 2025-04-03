<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmProjectProfitList.aspx.cs" Inherits="OMS.ProjectProfit.FrmProjectProfitList" %>

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
            <%if (this.IsOutFile == true)
            { %> 
                setTimeout(function () {
                    getCtrlById("btnDownload").click();
                    hideLoading();
            }, 0);
            <%} %>
        })
        //**********************
        // Init
        //**********************
        $(function () {
            setFocus();
            ShowProjectNm();
        });

        //**********************
        // Set Focus
        //**********************
        function setFocus() {

        }

        //**********************
        // Clear Form
        //**********************
        function clearForm() {
            $(":text").val("");
            var departmentDefault = getCtrlById("hdDepartmentDefault").val();
            getCtrlById("cmbDepartment").val(departmentDefault);
            //getCtrlById("cmbDepartment").trigger('change');
            getCtrlById("viewdetails").attr("collapse in");
            getCtrlById("cmbUser").val("-1");
            getCtrlById("cmbStatus").val("-1");
            getCtrlById("txtProjectCode").focus().select();
        }

        //**********************
        // Call Search Group User
        //**********************
        function callSearchProject() {
            var projectCD = "";
            var projectNm = "";
            showSearchProject(projectCD, projectNm, '1900/01/01', getCtrlById("txtProjectCode").attr("id"), getCtrlById("txtProjectName").attr("id"));

        };
        //**********************
        // Find Back
        //**********************    
        function findBack() {
            findBackCtr = "cmbDepartment";
            hideLoading();
            getCtrlById(findBackCtr).focus();
        }

        //**********************
        // Project CD On Changed
        //**********************
        function ShowProjectNm() {
            var projectVal = getCtrlById("txtProjectCode").val()

            if (projectVal != "") {
                var params = { 'in1': projectVal };
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
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- hidden field default--%>
    <asp:HiddenField ID="hdDepartmentDefault" runat="server" />

    <%-- Search Condition--%>
    <div class="well well-sm">
        <!--Collapse Button-->
        <button type="button" class="btn btn-default btn-sm" data-toggle="collapse" data-target="#viewdetails">
            <span class="glyphicon glyphicon-align-justify"></span>
        </button>
        <div class="collapse <%= this.Collapse %>" id="viewdetails">
            <div class="row">
                <%--projectCode--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtProjectCode.ClientID %>">
                            プロジェクト</label>
                        <div class="input-group">
                            <span class="input-group-btn">
                                <button id="btnProjectCode" class="btn btn-default btn-sm loading" type="button" onclick="callSearchProject();return false;">
                                    <span class="glyphicon glyphicon-search"></span>
                                </button>
                            </span>
                            <cc1:ICodeTextBox ID="txtProjectCode" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm"
                                LabelNames="projectCD:txtProjectCode,projectNm:txtProjectName" AjaxUrlMethod="GetProjectName" SearchButtonID="btnProjectCode" AllowChars="-" />
                        </div>
                    </div>
                </div>
                <%--projectName--%>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtProjectCode.ClientID %>">
                            &nbsp;</label>
                        <cc1:ITextBox ID="txtProjectName" ReadOnly="true" runat="server" TabIndex="-1" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
                <%--Department--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbDepartment.ClientID %>">
                            部門</label>
                        <asp:DropDownList ID="cmbDepartment" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--Admin--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbUser.ClientID %>">
                            管理者</label>
                        <asp:DropDownList ID="cmbUser" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <%--Start date 1--%>
                <div class="col-md-2">
                    <div class='form-group <%=GetClassError("dtStartDate")%>'>
                        <label class="control-label" for="<%= dtStartDate.ClientID %>">
                            集計対象期間</label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtStartDate" runat="server" CssClass="form-control input-sm"
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
                <%--End date 1--%>
                <div class="col-md-2">
                    <div class='form-group <%=GetClassError("dtEndDate")%>'>
                        <label class="control-label">
                            <strong class="text-danger">&nbsp</strong></label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtEndDate" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                </div>

                <div class="col-md-2">
                    <div class='form-group'>
                        <label class="control-label" for="<%= cmbStatus.ClientID %>">
                            状況</label>
                        <asp:DropDownList ID="cmbStatus" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
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
    <%--Excel and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-6">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnExcel" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnExcel_Command">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</asp:LinkButton>
                        <button type="button" id="btnDownload"  class="btn btn-default btn-sm hide" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;Download
                        </button>
                    </div>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" PostBackUrl="../Menu/FrmMainMenu.aspx" CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:PagingHeaderControl ID="PagingHeader" runat="server" />
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <%-- List --%>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto">
        <asp:Repeater ID="rptProjectList" runat="server">
            <HeaderTemplate>
                <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="text-right">
                    <td class="text-center">
                        <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") +"," + Eval("SC_StartDate")  +"," + Eval("SC_EndDate") %>'
                            PostBackUrl="FrmProjectProfitEntry.aspx" OnCommand="btnDetail_Command" CssClass="btn btn-info btn-sm loading"> 
                            <span class="glyphicon glyphicon-pencil"></span>
                        </asp:LinkButton>
                    </td>
                    <td class="text-left">
                        <%# Eval("ProjectCD")%> <br />
                        <%# Eval("ProjectName")%>
                    </td>
                    <td class="text-left">
                        <%# Eval("DepartmentName")%> <br />
                        <%# Eval("UserName")%>
                    </td>
                    <td class="text-left">
                        <%# Eval("StartDate", "{0:yyyy/MM/dd}")%><br />
                        <%# Eval("EndDate", "{0:yyyy/MM/dd}")%>
                    </td>
                    <td class="text-left">
                        <%# Eval("AcceptanceFlag").Equals(0) ? "仕掛" : "検収"%>
                    </td>
                    <td>
                        <%#   Eval("OrderAmount", "{0:#,##0}")%>
                    </td>
                    <td>
                        <%# Eval("ProfitRate", "{0:P2}")%>
                    </td>
                    <td>
                        <%# Eval("CostTotal", "{0:#,##0}")%>
                    </td>
                    <td>
                        <%# Eval("DirectCost", "{0:#,##0}")%>
                    </td>
                    <td>
                        <%# Eval("IndirectCosts", "{0:#,##0}")%>
                    </td>
                    <td>
                        <%# Eval("Expense", "{0:#,##0}")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <uc1:PagingFooterControl ID="PagingFooter" runat="server" />
    </asp:Panel>

</asp:Content>
