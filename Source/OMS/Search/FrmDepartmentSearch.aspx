<%@ Page Title="" Language="C#" MasterPageFile="~/SiteSearch.Master" AutoEventWireup="true" CodeBehind="FrmDepartmentSearch.aspx.cs" Inherits="OMS.Search.FrmDepartmentSearch" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="pagingFooter" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>
<%--Title Web--%>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleWeb" runat="server">
    Project Search
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
            getCtrlById("txtDepartmentCD").focus().select();
        }

        /*
        * Select row on grid
        */
        function selectRow(index) {

            var departmentCd = getCtrlById("DepartmentCD", index).val();
            var departmentNm = getCtrlById("DepartmentName", index).val();

            window.opener.$('#' + getCtrlById("Out1").val()).val(departmentCd);
            window.opener.$('#' + getCtrlById("Out2").val()).val(departmentNm);
            window.opener.findBack();
            closeWindow();
        }

        /*
        * Clear form
        */
        function clearForm() {
            $(":text").val("");
            getCtrlById("txtProjectCD").focus();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="server">
    部門検索
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
                <%--Department code--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label">
                            コード</label>
                        <cc1:ICodeTextBox ID="txtDepartmentCD" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm"
                            LabelNames="txtDepartmentCD" AjaxUrlMethod="FormatDepartmentCD"></cc1:ICodeTextBox>
                    </div>
                </div>
                <%--Department Name--%>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label">
                            部門名</label>
                        <cc1:ITextBox ID="txtDepartmentName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
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
    <asp:Repeater ID="rptDepartmentList" runat="server">
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
                    <asp:HiddenField ID="DepartmentCD" runat="server" Value='<%# Eval("DepartmentCD") %>' />
                    <%# Eval("DepartmentCD")%>
                </td>
                <td>
                    <asp:HiddenField ID="DepartmentName" runat="server" Value='<%# Eval("DepartmentName") %>' />                    
                    <%# Server.HtmlEncode(Eval("DepartmentName", "{0}"))%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <pagingFooter:PagingFooterControl ID="PagingFooter" runat="server" />
</asp:Content>



