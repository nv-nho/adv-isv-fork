<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmInformationList.aspx.cs"
    Inherits="OMS.Master.FrmInformationList" MasterPageFile="~/Site.Master" Title="" %>

<%--Import Using--%>
<%------------------------------------------------------------------------------------------------------------------------------%>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="oms" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="pagingFooter" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="pagingHeader" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="headerGrid" %>
<%------------------------------------------------------------------------------------------------------------------------------%>
<%--Java Script--%>
<%------------------------------------------------------------------------------------------------------------------------------%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            setFocus();
        });

        /*
        * set focus
        */
        function setFocus() {
            if ($("#viewdetails").hasClass("in")) {
                getCtrlById("txtInformationName").focus().select();
            }
        }

        /*
        * Clear form
        */
        function clearForm() {
            $(":text").val("");
            getCtrlById("txtInformationName").focus().select();
        }

        function back() {
            location.href = "../Menu/FrmMasterMenu.aspx";
        }

    </script>
</asp:Content>
<%------------------------------------------------------------------------------------------------------------------------------%>
<%--Page Content--%>
<%------------------------------------------------------------------------------------------------------------------------------%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
   
    <%-- Search Condition--%>
    <div class="well well-sm">
        <!--Collapse Button-->
        <button type="button" class="btn btn-default btn-sm" data-toggle="collapse" data-target="#viewdetails">
            <span class="glyphicon glyphicon-align-justify"></span>
        </button>
        <div class="collapse <%= this.Collapse %>" id="viewdetails">
            <div class="row">
               
                <%--Information Name--%>
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtInformationName.ClientID %>">
                            お知らせ情報名</label>
                        <oms:ITextBox ID="txtInformationName" runat="server" CssClass="form-control input-sm"></oms:ITextBox>
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
                        <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmInformationDetail.aspx" OnCommand="btnNew_Click"
                            CssClass="btn btn-primary btn-sm">
                            <span class="glyphicon glyphicon-plus"></span>&nbsp;新規
                        </asp:LinkButton>
                    </div>
                    <div class="btn-group">
                        <button type="button" class="btn btn-default btn-sm" onclick="back();">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <pagingHeader:PagingHeaderControl ID="PagingHeader" runat="server" />
    <%-- List Event--%>
    <headerGrid:HeaderGridControl ID="HeaderGrid" runat="server" />
    <asp:Repeater ID="rptEventList" runat="server">
        <HeaderTemplate>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("RowNumber") %>
                    <asp:HiddenField ID="InformationID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                </td>
                <td>
                    <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") %>'
                        PostBackUrl="FrmInformationDetail.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading">
                        <span class="glyphicon glyphicon-pencil"></span>
                    </asp:LinkButton>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("InformationName", "{0}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("BeginDateStr", "{0}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("EndDateStr", "{0}"))%>
                </td>
               
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <pagingFooter:PagingFooterControl ID="PagingFooter" runat="server" />
</asp:Content>
<%------------------------------------------------------------------------------------------------------------------------------%>