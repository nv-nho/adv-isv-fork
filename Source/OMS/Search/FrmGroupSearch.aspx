<%@ Page Title="" Language="C#" MasterPageFile="~/SiteSearch.Master" AutoEventWireup="true"
    CodeBehind="FrmGroupSearch.aspx.cs" Inherits="OMS.Search.FrmGroupSearch" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="pagingFooter" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>
<%--Title Web--%>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleWeb" runat="server">
    Group Search
</asp:Content>
<%--Script--%>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">

        $(function () {
            setFocus();
        });

        /*
        * set focus
        */
        function setFocus() {
            getCtrlById("txtGroupCD").focus().select();
        }

        /*
        * Select row on grid
        */
        function selectRow(index) {

            var groupNm = getCtrlById("GroupName", index).val();
            var groupCd = getCtrlById("GroupCD", index).val();

            window.opener.$('#' + getCtrlById("Out1").val()).val(groupCd);
            window.opener.$('#' + getCtrlById("Out2").val()).val(groupNm);
            window.opener.findBack();
            closeWindow();
        }

        /*
        * Clear form
        */
        function clearForm() {
            $(":text").val("");
            getCtrlById("txtGroupCD").focus();
        }     

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="server">
    権限グループ検索
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="Out1" runat="server" />
    <asp:HiddenField ID="Out2" runat="server" />
    <div class="well well-md">
        <!--Collapse Button-->
        <button id="viewDetailPress" type="button" class="btn btn-default btn-xs" data-toggle="collapse"
            data-target="#viewdetails">
            <span class="glyphicon glyphicon-align-justify"></span>
        </button>
        <div class="collapse <%= this.Collapse %>" id="viewdetails">
            <div class="row">
                <%--group code--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label">
                            コード</label>
                        <cc1:ICodeTextBox ID="txtGroupCD" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm"
                            LabelNames="txtGroupCD" AjaxUrlMethod="FormatGroupCD"></cc1:ICodeTextBox>
                    </div>
                </div>
                <%--Group Name--%>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label">
                            権限グループ名</label>
                        <cc1:ITextBox ID="txtGroupName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
            </div>
            <%--Search and Clear Button--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="btn-group">
                        <button class="btn btn-default btn-sm loading" type="button" id="btnSearch" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;検索
                        </button>
                        <button id="btnClear" type="button" class="btn btn-default btn-sm" onclick="clearForm();"
                            runat="server">
                            <span class="glyphicon glyphicon-refresh"></span>&nbsp;クリア
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:PagingHeaderControl ID="PagingHeader" runat="server" />
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <%-- List User--%>
    <asp:Repeater ID="rptGroupUserList" runat="server">
        <HeaderTemplate>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("RowNumber") %>
                </td>
                <td>
                    <button type="button" class="btn btn-info btn-xs" onclick="selectRow('<%#Container.ItemIndex%>');">
                        <span class="glyphicon glyphicon-ok"></span>
                    </button>
                </td>
                <td>
                    <asp:HiddenField ID="GroupCD" runat="server" Value='<%# Eval("GroupCD") %>' />
                    <%# Eval("GroupCD")%>
                </td>
                <td>
                    <asp:HiddenField ID="GroupName" runat="server" Value='<%# Eval("GroupName") %>' />                    
                    <%# Server.HtmlEncode(Eval("GroupName", "{0}"))%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <pagingFooter:PagingFooterControl ID="PagingFooter" runat="server" />
</asp:Content>
