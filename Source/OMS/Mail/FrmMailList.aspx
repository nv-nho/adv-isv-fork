<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmMailList.aspx.cs" Inherits="OMS.Mail.FrmMailList" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>
<asp:Content ID="ctHeader" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
    .popover-title{
        background: #c82333;
        color: #fff;
    }
    </style>
    <script language="javascript" type="text/javascript">

        //**********************
        // Init
        //**********************
        $(function () {
            setFocus();
            
            var elements = document.getElementsByClassName("UnReplyUserList");
            for (var i = 0; i < elements.length; i++) {
                elements[i].addEventListener('mouseenter', ShowUnReplyUser, false);
            }

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
            getCtrlById("txtID").focus();
        }
        function ShowUnReplyUser() {
            var sender = this;
            var params = { 'HID': $(sender).attr("data-id") };
            ajax("GetUnReplyUser", params, function (response) {
                if (response.d) {
                    var content = "";
                    $.each(response.d, function (index, value) {
                        content += "<span>" + value + "</span><br/>";
                    });
                    if (response.d.length > 0) {

                        $(sender).popover({ title: "未返信", content: content, placement: "left", trigger: "hover", html: "true" });
                        $(sender).popover("show");
                    }
                }
            });
            sender.removeEventListener("mouseenter", ShowUnReplyUser, false); 
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Search Condition--%>
    <div class="well well-sm">
        <!--Collapse Button-->
        <button type="button" class="btn btn-default btn-sm" data-toggle="collapse" data-target="#viewdetails">
            <span class="glyphicon glyphicon-align-justify"></span>
        </button>
        <div class="collapse <%= this.Collapse %>" id="viewdetails">
            <div class="row">
                <%--MailID--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtID.ClientID %>">
                            送信ID</label>
                        <cc1:ICodeTextBox ID="txtID" runat="server" CodeType="Numeric" CssClass="form-control input-sm"></cc1:ICodeTextBox>
                    </div>
                </div>
                 <%--subject--%>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtSubject.ClientID %>">
                            件名</label>
                        <cc1:ITextBox ID="txtSubject" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
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
                        <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmMailEntry.aspx" OnCommand="btnNew_Click"
                            CssClass="btn btn-primary btn-sm loading">
                            <span class="glyphicon glyphicon-plus"></span>&nbsp;新規
                        </asp:LinkButton>
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
    <%-- List Mail--%>
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <asp:Repeater ID="rptMailList" runat="server">
        <HeaderTemplate>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("RowNumber") %>
                    <asp:HiddenField ID="ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                </td>
                <td>
                    <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") %>'
                        PostBackUrl="FrmMailEntry.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading">
                        <span class="glyphicon glyphicon-pencil"></span>
                    </asp:LinkButton>
                </td>

                <td>
                    <%# Server.HtmlEncode(Eval("ID", "{0}"))%>
                </td>

                <td>
                    <%# Server.HtmlEncode(Eval("StrUpdateDate", "{0}"))%>
                </td>
                 <td>
                    <%# Server.HtmlEncode(Eval("StrReplyDueDate", "{0}"))%>
                </td>
                 <td>
                    <div data-id="<%# Eval("ID") %>" class="UnReplyUserList">
                        <%# Server.HtmlEncode(Eval("Subject", "{0}"))%>
                    </div>
                </td>
                <td>
                    <div data-id="<%# Eval("ID") %>" class="UnReplyUserList">
                        <%# Server.HtmlEncode(Eval("CountRep", "{0}"))%>
                    </div>
                </td>
                <td>
                  <span class="<%# Server.HtmlEncode(Eval("Class", "{0}"))%>" style="width:60px; display: inline-block"><%# Server.HtmlEncode(Eval("Content", "{0}"))%></span>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <uc1:PagingFooterControl ID="PagingFooter" runat="server" />
</asp:Content>
