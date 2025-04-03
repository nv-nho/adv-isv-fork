<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmPayslipList.aspx.cs" Inherits="OMS.Payslip.FrmPayslipList" %>

<%@ Import Namespace="OMS.Utilities" %>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .temp 
        {
        	background-color:"#b5b5b5"
        }
        .bg-danger {
            background-color: #f2dede;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            <%if(this.Downloading == true){ %>
                getCtrlById("btnDownloading").click();
                hideLoading();
            <%} %>

        });

        //download file
        function download(id) {
            getCtrlById("hdnFileName").val(id);
            getCtrlById("btnDownload").click();
            hideLoading();
        }
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <asp:HiddenField ID="hdnFileName" runat="server" />
    <button type="button" id="btnDownload" class="hide" runat="server">
        <span class="glyphicon glyphicon-search"></span>&nbsp;Download
    </button>

    <button type="button" id="btnDownloading" class="hide" runat="server">
    </button>   

    <%--Condition Search--%>
    <div class="well well-sm">
        <div class="row">
            <%--Department--%>
            <div class="col-md-2">
                <div class="form-group">
                    <label class="control-label" for="<%= ddlYear.ClientID %>">
                        対象年度</label>
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control input-sm"
                        AutoPostBack="True" autocomplete="off" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>

            <%--Department--%>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="control-label" for="<%= ddlDepartment.ClientID %>">
                        部門</label>
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control input-sm"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"
                        autocomplete="off">
                    </asp:DropDownList>
                </div>
            </div>

            <%--EmployeeName--%>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="control-label" for="<%= ddlUser.ClientID %>">
                        社員名</label>
                    <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged"
                        AutoPostBack="True" autocomplete="off">
                    </asp:DropDownList>
                </div>
            </div>


            <div class="col-md-2">
                <div class="form-group">
                    <div>
                        <label class="control-label">&nbsp;</label>
                    </div>
                    <asp:LinkButton ID="btnUpload" runat="server" PostBackUrl="FrmPayslipUpload.aspx" OnCommand="btnUploadItem_Click"
                        CssClass="btn btn-primary btn-sm loading">
                        &nbsp;明細書アップロード
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <%--Create New and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-3">
                <div class="btn-group btn-group-justified">
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
    <%-- List Config--%>
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <asp:Repeater ID="rptList" runat="server">
        <HeaderTemplate>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr <%# (Eval("DownloadDate_fm") != "") ? "class='light-gray'" : "" %>>
                <td>
                    <%# Eval("RowNumber") %>
                </td>
                <td>
                    <button class="btn btn-info btn-sm" id="btnDownload<%# Eval("ID") %>" type="button"
                            onclick="download('<%# Eval("ID") %>');return false;">
                            <span class="glyphicon glyphicon-cloud-download"></span>
                    </button>
                </td>
                <td>
                    <%# Eval("Type")%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("Year", "{0}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("UploadDate_fm", "{0}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("DownloadDate_fm", "{0}"))%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <uc1:PagingFooterControl ID="PagingFooter" runat="server" />

</asp:Content>