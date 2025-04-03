<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmDepartmentList.aspx.cs" Inherits="OMS.Master.FrmDepartmentList" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        //**********************
        // Init
        //**********************
        $(function () {
            setFocus();
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
            getCtrlById("txtDepartmentCD").focus().select();
           // var inValidDefault = getCtrlById("hdInValideDefault").val();
           // getCtrlById("cmbInvalidData").val(inValidDefault);
           // getCtrlById("txtWorkingSystemCD").focus().select();
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <%-- Search Condition--%>
    <div class="well well-sm">
        <!--Collapse Button-->
        <div class="row">
            <div class="col-md-12">
                <button id="viewDetailPress" type="button" class="btn btn-default btn-sm" data-toggle="collapse"
                    data-target="#viewdetails">
                    <span class="glyphicon glyphicon-align-justify"></span>
                </button>
            </div>
        </div>
        <div class="collapse <%= this.Collapse %>" id="viewdetails">
            <div class="row">
             <%--DepartmentCD--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtDepartmentCD.ClientID %>">
                            部門コード
                        </label>
                        <cc1:ICodeTextBox ID="txtDepartmentCD" runat="server" CssClass="form-control input-sm" CodeType="AlphaNumeric" AllowChars="-"
                            AjaxUrlMethod="GetDepartmentCD" LabelNames="txtDepartmentCD"  />
                    </div>
                </div>
            </div>
            <div class="row">             
                <%--DepartmentName--%>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtDepartmentName.ClientID %>">
                            部門名</label>
                        <cc1:ITextBox ID="txtDepartmentName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
                <%--txtDepartmentName2--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtDepartmentName2.ClientID %>">
                            略称</label>
                        <cc1:ITextBox ID="txtDepartmentName2" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
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
                    <%-- New button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmDepartmentEntry.aspx" OnCommand="btnNew_Click"
                            CssClass="btn btn-primary btn-sm loading">
                            <span class="glyphicon glyphicon-plus"></span>&nbsp;新規
                        </asp:LinkButton>
                    </div>
                    <%-- Back button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" PostBackUrl="../Menu/FrmMasterMenu.aspx"
                            CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:PagingHeaderControl ID="PagingHeader" runat="server" />
      <%-- List User--%>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto">
        <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
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
                        <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") %>'
                            PostBackUrl="FrmDepartmentEntry.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading"> 
                            <span class="glyphicon glyphicon-pencil"></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <%# Eval("DepartmentCD")%>
                    </td>
                    <td>
                        <%# Eval("DepartmentName")%>
                    </td>
                     <td>
                        <%# Eval("DepartmentName2")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
    <uc1:PagingFooterControl ID="PagingFooter" runat="server" />
    </div>
</asp:Content>
