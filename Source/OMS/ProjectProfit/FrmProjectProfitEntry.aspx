<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmProjectProfitEntry.aspx.cs" Inherits="OMS.ProjectProfit.FrmProjectProfitEntry" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .MaxheightDetail {
            max-height: 430px;
        }
    </style>
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Search Condition--%>
    <div class="well well-sm">
        <div class="row">
            <%--projectCode--%>
            <div class="col-md-2">
                <div class="form-group">
                    <label class="control-label" for="<%= txtProjectCode.ClientID %>">
                        プロジェクトコード</label>
                    <cc1:ICodeTextBox ID="txtProjectCode" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm" SearchButtonID="btnProjectCode" AllowChars="-" />
                </div>
            </div>
            <%--projectName--%>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="control-label" for="<%= txtProjectName.ClientID %>">
                        プロジェクト名</label>
                    <cc1:ITextBox ID="txtProjectName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                </div>
            </div>
            <%--Department--%>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="control-label" for="<%= txtDepartment.ClientID %>">
                        部門</label>
                    <cc1:ITextBox ID="txtDepartment" runat="server" CssClass="form-control input-sm">
                    </cc1:ITextBox>
                </div>
            </div>
            <%--Admin--%>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="control-label" for="<%= txtUser.ClientID %>">
                        管理者</label>
                    <cc1:ITextBox ID="txtUser" runat="server" CssClass="form-control input-sm">
                    </cc1:ITextBox>
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

            <div class="col-md-2">
                <div class='form-group'>
                    <label class="control-label" for="<%= txtStatus.ClientID %>">
                        状況</label>
                    <cc1:ITextBox ID="txtStatus" runat="server" CssClass="form-control input-sm">
                    </cc1:ITextBox>
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
                        <asp:LinkButton ID="btnExcel1" runat="server" CssClass="btn btn-default btn-sm loading" OnCommand="btnExcel_Command">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel
                        </asp:LinkButton>
                        <button type="button" id="btnDownload" class="btn btn-default btn-sm hide" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;Download
                        </button>
                    </div>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" PostBackUrl="../ProjectProfit/FrmProjectProfitList.aspx" CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="projectList" class="overflow-auto">
        <table class="table table-striped table-bordered table-condensed">
            <thead>
                <tr>
                    <th class="text-right">受注金額</th>
                    <th class="text-right">利益率</th>
                    <th class="text-right">原価計</th>
                    <th class="text-right">直接費計</th>
                    <th class="text-right">間接費計</th>
                    <th class="text-right">経費計</th>
                </tr>
            </thead>
            <tbody>
                <tr class="text-right">
                    <td><%= String.Format(OMS.Utilities.Constants.NUMBER_FORMAT_INT, this.ProjectProfitInfo.OrderAmount) %></td>
                    <td><%= String.Format("{0:P2}", this.ProjectProfitInfo.ProfitRate) %></td>
                    <td><%= String.Format(OMS.Utilities.Constants.NUMBER_FORMAT_INT, this.ProjectProfitInfo.CostTotal) %></td>
                    <td><%= String.Format(OMS.Utilities.Constants.NUMBER_FORMAT_INT, this.ProjectProfitInfo.DirectCost) %></td>
                    <td><%= String.Format(OMS.Utilities.Constants.NUMBER_FORMAT_INT, this.ProjectProfitInfo.IndirectCosts) %></td>
                    <td><%= String.Format(OMS.Utilities.Constants.NUMBER_FORMAT_INT, this.ProjectProfitInfo.Expense) %></td>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="table MaxheightDetail">
        <asp:Repeater ID="rptDetailList1" runat="server">
            <HeaderTemplate>
                <table class="table table-striped table-bordered table-condensed">
                    <caption class="text-left text-dark"><strong>直接費・間接費一覧</strong></caption>
                    <thead>
                        <tr>
                            <td>社員コード</td>
                            <td>社員名</td>
                            <td class="text-right">直接費</td>
                            <td class="text-right">直接費（時間外）</td>
                            <td class="text-right">間接費</td>
                            <td class="text-right">計</td>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="text-right">
                    <td class="text-left col-md-1">
                        <%# Eval("UserCD")%>
                    </td>
                    <td class="text-left col-md-3">
                        <%# Eval("UserName")%>
                    </td>
                    <td class="col-md-2">
                        <%# Eval("DirectCost",OMS.Utilities.Constants.NUMBER_FORMAT_INT) %>
                    </td>
                    <td class="col-md-2">
                        <%# Eval("DirectCostAfter",OMS.Utilities.Constants.NUMBER_FORMAT_INT) %>
                    </td>
                    <td class="col-md-2">
                        <%# Eval("IndirectCosts",OMS.Utilities.Constants.NUMBER_FORMAT_INT) %>
                    </td>
                    <td class="col-md-2">
                        <%# Eval("Total",OMS.Utilities.Constants.NUMBER_FORMAT_INT) %>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" CssClass="table MaxheightDetail">
        <asp:Repeater ID="rptDetailList2" runat="server">
            <HeaderTemplate>
                <table class="table table-striped table-bordered table-condensed">
                    <caption class="text-left text-dark"><strong>経費一覧</strong></caption>
                    <thead>
                        <tr>
                            <td>利用日</td>
                            <td>支払先（社員）</td>
                            <td class="text-center">経費種目</td>
                            <td>支払先</td>
                            <td class="text-right">金額</td>
                            <td>目的・経路</td>
                            <td class="text-center">承認状況</td>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("DateStr")%>
                    </td>
                    <td>
                        <%# Eval("RegisterPersonName")%>
                    </td>
                    <td class="text-center">
                        <%# Eval("Type") %>
                    </td>
                    <td>
                        <%# Eval("DestinationName") %>
                    </td>
                    <td class="text-right">
                        <%# Eval("ExpenceAmount",OMS.Utilities.Constants.NUMBER_FORMAT_INT) %>
                    </td>
                    <td>
                        <%# Eval("Memo") %>
                    </td>
                    <td class="text-center">
                        <%# Eval("ApprovedStatus") %>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
    <%--Excel and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-6">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnExcel2" runat="server" CssClass="btn btn-default btn-sm loading" OnCommand="btnExcel_Command">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel
                        </asp:LinkButton>
                    </div>
                    <div class="btn-group">
                        <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="../ProjectProfit/FrmProjectProfitList.aspx" CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
