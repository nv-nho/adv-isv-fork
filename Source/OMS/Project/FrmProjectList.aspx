<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmProjectList.aspx.cs" Inherits="OMS.Project.FrmProjectList" %>

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

            <%if (this.IsOutFile == true)
            { %> 
                setTimeout(function () {
                    getCtrlById("btnDownload").click();
                    hideLoading();
                }, 0);
            <%} %>

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
            getCtrlById("<%= cmbInvalidData.ClientID %>").val("-1");
            getCtrlById("<%= cmbAcceptanceFlag.ClientID %>").val("-1");
            getCtrlById("<%= cmbDepartment.ClientID %>").val("-1");
            getCtrlById("<%= cmbUser.ClientID %>").val("-1");
            getCtrlById("<%= txtProjectCD.ClientID %>").focus().select();
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
                <%--ProjectCD--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtProjectCD.ClientID %>">
                            プロジェクトコード
                        </label>
                        <cc1:ICodeTextBox ID="txtProjectCD" runat="server" CssClass="form-control input-sm" CodeType="AlphaNumeric" AllowChars="-"
                            AjaxUrlMethod="GetProjectCD" LabelNames="txtProjectCD" />
                    </div>
                </div>
                <%--ProjectName--%>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtProjectName.ClientID %>">
                            プロジェクト名</label>
                        <cc1:ITextBox ID="txtProjectName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
                <%--WorkPlace--%>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtWorkPlace.ClientID %>">
                            作業場所</label>
                        <cc1:ITextBox ID="txtWorkPlace" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <%--Start date 1--%>
                <div class="col-md-2">
                    <div class='form-group <%=GetClassError("dtStartDate1")%>'>
                        <label class="control-label" for="<%= dtStartDate1.ClientID %>">
                            開始日</label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtStartDate1" runat="server" CssClass="form-control input-sm"
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
                <%--Start date 2--%>
                <div class="col-md-2">
                    <div class='form-group <%=GetClassError("dtStartDate2")%>'>
                        <label class="control-label">
                            <strong class="text-danger">&nbsp</strong></label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtStartDate2" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                </div>
                <%--End date 1--%>
                <div class="col-md-2">
                    <div class='form-group <%=GetClassError("dtEndDate1")%>'>
                        <label class="control-label" for="<%= dtEndDate1.ClientID %>">
                            終了日</label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtEndDate1" runat="server" CssClass="form-control input-sm"
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
                <%--End date 2--%>
                <div class="col-md-2">
                    <div class='form-group <%=GetClassError("dtEndDate2")%>'>
                        <label class="control-label">
                            <strong class="text-danger">&nbsp</strong></label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtEndDate2" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
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
                <%--AcceptanceFlag--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbAcceptanceFlag.ClientID %>">
                            状況</label>
                        <asp:DropDownList ID="cmbAcceptanceFlag" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2">
                </div>
                <div class="col-md-5">
                    <div class="row">
                        <%--Department--%>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label" for="<%= cmbDepartment.ClientID %>">
                                    部門</label>
                                <asp:DropDownList ID="cmbDepartment" runat="server" CssClass="form-control input-sm">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <%--User--%>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label" for="<%= cmbUser.ClientID %>">
                                    管理者</label>
                                <asp:DropDownList ID="cmbUser" runat="server" CssClass="form-control input-sm">
                                </asp:DropDownList>
                            </div>
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
            <div class="col-md-6">
                <div class="btn-group btn-group-justified">
                    <%-- New button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmProjectEntry.aspx" OnCommand="btnNew_Click"
                            CssClass="btn btn-primary btn-sm loading">
                            <span class="glyphicon glyphicon-plus"></span>&nbsp;新規
                        </asp:LinkButton>
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
            <div class="col-md-4 col-sm-4"></div>
            <div class="col-md-2 col-sm-12">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnExcel" runat="server" CssClass="btn btn-sm btn-default loading"
                            OnCommand="btnExcel_Click">
                                     <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;オーダー取得
                        </asp:LinkButton>
                        <button type="button" id="btnDownload" class="btn btn-default btn-sm hide" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;Download
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:PagingHeaderControl ID="PagingHeader" runat="server" />
    <%-- List User--%>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto">
        <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
        <asp:Repeater ID="rptProjectList" runat="server">
            <HeaderTemplate>
                <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class='<%# GetColorClass(Convert.ToInt32(Eval("Color")))%>'>
                    <td>
                        <%# Eval("RowNumber")%>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") %>'
                            PostBackUrl="FrmProjectEntry.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading"> 
                            <span class="glyphicon glyphicon-pencil"></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <%# Eval("ProjectCD")%>
                    </td>
                    <td>
                        <%# Eval("ProjectName")%>
                    </td>
                    <td>
                        <%# Eval("DepartmentName")%><br />
                        <%# Eval("UserName")%>
                    </td>
                    <td class="text-right">
                        <%# Eval("OrderAmount", OMS.Utilities.Constants.NUMBER_FORMAT_INT)%>
                    </td>
                    <td>
                        <%# Server.HtmlEncode(Eval("StartDateStr", "{0}"))%><br />
                        <%# Server.HtmlEncode(Eval("EndDateStr", "{0}"))%>
                    </td>
                    <td>
                        <%# Eval("AcceptanceFlag").Equals(1) ? "検収" : "仕掛"%>
                    </td>
                    <td>
                        <%# Eval("Memo")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
    <uc1:PagingFooterControl ID="PagingFooter" runat="server" />
</asp:Content>
