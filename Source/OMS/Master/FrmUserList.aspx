<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmUserList.aspx.cs" Inherits="OMS.Master.FrmUserList" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">

        var findBackCtr = "txtUserName1";
        //**********************
        // Init
        //**********************
        $(function () {
            setFocus();
            txtGroupCDOnChanged();
            txtDepartmentCDOnChanged();
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
        // Call Search Group User
        //**********************
        function callSearchGroupUser() {
            findBackCtr = "cmbInvalidData";
            //2018.01.18 nv-nho search all
            //var groupCD = getCtrlById("txtGroupCode").val();
            //var groupName = getCtrlById("txtGroupName").val();
            var groupCD = "";
            var groupName = "";
            showSearchGroupUser(groupCD, groupName, getCtrlById("txtGroupCode").attr("id"), getCtrlById("txtGroupName").attr("id"));
        };

        //**********************
        // Call Search Department User
        //**********************
        function callSearchDepartment() {
            findBackCtr = "txtGroupCode";
            //2018.01.18 nv-nho search all
            //var departmentCD = getCtrlById("txtDepartmentCode").val();
            //var departmentName = getCtrlById("txtDepartmentName").val();
            var departmentCD = "";
            var departmentName = "";
            showSearchDepartment(departmentCD, departmentName, getCtrlById("txtDepartmentCode").attr("id"), getCtrlById("txtDepartmentName").attr("id"));
        };

        //**********************
        // Clear Form
        //**********************
        function clearForm() {

            $(":text").val("");
            getCtrlById("txtUserName1").focus();

            var inValidDefault = getCtrlById("hdInValideDefault").val();
            getCtrlById("cmbInvalidData").val(inValidDefault);
        }

        //**********************
        // Group CD On Changed
        //**********************
        function txtGroupCDOnChanged() {
            getCtrlById("txtGroupCode").bind("change", function () {

                if ($.trim(this.value) != "") {
                    var params = { 'groupCD': $.trim(this.value) };
                    ajax('GetGroupName', params, function (response) {
                        if (response.d) {
                            var result = eval('(' + response.d + ')');
                            getCtrlById("txtGroupCode").val(result['groupCD']);
                            getCtrlById("txtGroupName").val(result['groupNm']);
                        }
                    });
                } else {
                    getCtrlById("txtGroupName").val('');
                }
            });
        }

        //**********************
        // department CD On Changed
        //**********************
        function txtDepartmentCDOnChanged() {
            getCtrlById("txtDepartmentCode").bind("change", function () {

                if ($.trim(this.value) != "") {
                    var params = { 'departmentCD': $.trim(this.value) };
                    ajax('GetDepartmentName', params, function (response) {
                        if (response.d) {
                            var result = eval('(' + response.d + ')');
                            getCtrlById("txtDepartmentCode").val(result['departmentCD']);
                            getCtrlById("txtDepartmentName").val(result['departmentNm']);
                        }
                    });
                } else {
                    getCtrlById("txtDepartmentName").val('');
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- hidden field default--%>
    <asp:HiddenField ID="hdInValideDefault" runat="server" />
    <%-- Search Condition--%>
    <div class="well well-sm">
        <!--Collapse Button-->
        <button type="button" class="btn btn-default btn-sm" data-toggle="collapse" data-target="#viewdetails">
            <span class="glyphicon glyphicon-align-justify"></span>
        </button>
        <div class="collapse <%= this.Collapse %>" id="viewdetails">
            <div class="row">
                <%--UserName1--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtUserName1.ClientID %>">
                            社員名</label>
                        <cc1:ITextBox ID="txtUserName1" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
                <%--UserName2--%>
                <%-- <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtUserName2.ClientID %>">
                            User Name 2</label>
                        <cc1:ITextBox ID="txtUserName2" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>--%>
            </div>
           <div class="row">
                 <%--department--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtDepartmentCode.ClientID %>">
                            部門</label>
                        <div class="input-group">
                            <span class="input-group-btn">
                                <button id="btnDepartmentCode" class="btn btn-default btn-sm loading" type="button" onclick="callSearchDepartment();return false;">
                                    <span class="glyphicon glyphicon-search"></span>
                                </button>
                            </span>
                            <cc1:ICodeTextBox ID="txtDepartmentCode" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm" SearchButtonID="btnDepartmentCode" AllowChars="-"/>
                        </div>
                    </div>
                </div>
                <%--txtDepartmentName--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtDepartmentName.ClientID %>">
                            部門名</label>
                        <cc1:ITextBox ID="txtDepartmentName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
                 <%--Group--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtGroupCode.ClientID %>">
                            権限グループ</label>
                        <div class="input-group">
                            <span class="input-group-btn">
                                <button id="btnGroupCode" class="btn btn-default btn-sm loading" type="button" onclick="callSearchGroupUser();return false;">
                                    <span class="glyphicon glyphicon-search"></span>
                                </button>
                            </span>
                            <cc1:ICodeTextBox ID="txtGroupCode" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm" SearchButtonID="btnGroupCode"/>
                        </div>
                    </div>
                </div>
                <%--txtGroupName--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtGroupName.ClientID %>">
                            権限グループ名</label>
                        <cc1:ITextBox ID="txtGroupName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <%--Invalid Data--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbInvalidData.ClientID %>">
                            無効データ</label>
                        <asp:DropDownList ID="cmbInvalidData" runat="server" CssClass="form-control input-sm">
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
    <%--Create New and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-6">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmUserDetail.aspx" OnCommand="btnNew_Click"
                            CssClass="btn btn-primary btn-sm loading">
                            <span class="glyphicon glyphicon-plus"></span>&nbsp;新規
                        </asp:LinkButton>
                    </div>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" PostBackUrl="../Menu/FrmMasterMenu.aspx" CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:PagingHeaderControl ID="PagingHeader" runat="server" />
    <%-- List User--%>
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <asp:Repeater ID="rptUserList" runat="server">
        <HeaderTemplate>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class='<%# GetColorClass(Convert.ToInt32(Eval("Color")))%>'>
                <td>
                    <%# Eval("RowNumber") %>
                    <asp:HiddenField ID="UserID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                </td>
                <td>
                    <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") %>'
                        PostBackUrl="FrmUserDetail.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading">
                        <span class="glyphicon glyphicon-pencil"></span>
                    </asp:LinkButton>
                </td>
                <td>
                    <%# Eval("UserCD")%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("LoginID", "{0}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("UserName1", "{0}"))%>
                </td>
                 <td>
                    <%# Server.HtmlEncode(Eval("DepartmentName", "{0}"))%>
                </td>
                 <td>
                    <%# Server.HtmlEncode(Eval("CalendarName", "{0}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("GroupName", "{0}"))%>
                </td>
                <td class="text-right">
                    <%# Server.HtmlEncode(Eval("TotalVacationDays", "{0}"))%>
                </td>
                <%--<td>
                    <span class='<%# Convert.ToBoolean(Eval("StatusFlag")) ? "text-danger glyphicon glyphicon-remove" : "text-success glyphicon glyphicon-ok" %>'>
                    </span>
                </td>--%>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <uc1:PagingFooterControl ID="PagingFooter" runat="server" />
</asp:Content>
