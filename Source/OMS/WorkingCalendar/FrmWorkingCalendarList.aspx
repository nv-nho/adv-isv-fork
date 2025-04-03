<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmWorkingCalendarList.aspx.cs" Inherits="OMS.Master.FrmWorkingCalendarList" %>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">

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
            getCtrlById("cmbInvalidData").focus().select();
        }

        //**********************
        // Clear Form
        //**********************
        function clearForm() {

            $(":text").val("");
            getCtrlById("txtUserName1").focus();

            var inValidDefault = getCtrlById("hdInValideDefault").val();
            getCtrlById("cmbInvalidData").val(inValidDefault);
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
                <%--Product Id--%>
                
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtCalendarCD.ClientID %>">
                            コード</label>
                        <cc1:ICodeTextBox ID="txtCalendarCD" CodeType="Numeric" AllowChars="-" runat="server" CssClass="form-control input-sm"></cc1:ICodeTextBox>
                    </div>
                </div>

                <%--Product Name--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtCalendarName.ClientID %>">
                            名称</label>
                        <cc1:ITextBox ID="txtCalendarName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
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
                    <%if (!this.isOnlyPaidLeave) {%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmWorkingCalendarEntry.aspx" OnCommand="btnNew_Click"
                            CssClass="btn btn-primary btn-sm loading">
                            <span class="glyphicon glyphicon-plus"></span>&nbsp;新規
                        </asp:LinkButton>
                    </div>
                    <%} %>
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
    <%-- List WorkingCalendar--%>
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <asp:Repeater ID="rptWorkingCalendarList" runat="server">
        <HeaderTemplate>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr >
                <td>
                    <%# Eval("RowNumber") %>
                    <asp:HiddenField ID="UserID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                </td>
                <td>
                    <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") %>'
                        PostBackUrl="FrmWorkingCalendarEntry.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading">
                        <span class="glyphicon glyphicon-pencil"></span>
                    </asp:LinkButton>
                </td>
                <td>
                    <%# Eval("CalendarCD")%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("CalendarName", "{0}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("InitialDate", "{0}"))%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>

    <uc1:PagingFooterControl ID="PagingFooter" runat="server" />
</asp:Content>
