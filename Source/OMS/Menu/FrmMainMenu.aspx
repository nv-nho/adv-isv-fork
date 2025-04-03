<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmMainMenu.aspx.cs" Inherits="OMS.Menu.FrmMainMenu" %>

<asp:Content ID="ctHeader" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
         <%if (this.IsShowQuestion == true)
        { %>
        $('#modalQuestion').modal('show');

        $('#modalQuestion').on('shown.bs.modal', function (e) {
            $('<%=this.DefaultButton%>').focus();
                    });

                <%} %>
    });
    </script>
</asp:Content>
<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Repeater ID="rptData" runat="server">
        <ItemTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <span class="glyphicon glyphicon-info-sign"></span>&nbsp;<%# Eval("InformationName")%>
                </div>
                <div class="panel-body">
                    <%# Eval("InformationContent")%>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <div class="row">
        <div class="col-md-4">
            <!--Working Calendar-->
            <div class="well well-sm" align="center">
                <img src="../Images/callendar.png" alt="callendar" />
                <p>
                    年間勤務カレンダーの表示、<br />
                    登録、編集を行います。
                </p>
                <asp:LinkButton ID="btnWorkingCalendar" runat="server" OnCommand="btnWorkingCalendar_Click"
                    class="btn btn-default btn-lg btn-block">
                    年間勤務カレンダー
                </asp:LinkButton>
            </div>
        </div>

        <div class="col-md-4">
            <!--AttenDance-->
            <div class="well well-sm" align="center">
                <img src="../Images/attendance.png" alt="attendance" />
                <p>
                    月間での勤務表を表示、<br />
                    登録、編集を行います。
                </p>
                <asp:LinkButton ID="btnTAttendance" runat="server" OnCommand="btnTAttendance_Click"
                    class="btn btn-default btn-lg btn-block">
                    勤務表登録
                </asp:LinkButton>
            </div>
        </div>

        <div class="col-md-4">
            <!--Master-->
            <div class="well well-sm" align="center">
                <img src="../Images/project.png" alt="project" />
                <p>
                    プロジェクトの表示、<br />
                    登録、編集を行います。
                </p>
                <asp:LinkButton ID="btnProject" runat="server" PostBackUrl="~/Project/FrmProjectList.aspx"
                    class="btn btn-default btn-lg btn-block">
                    プロジェクト登録
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4">
            <!--AttendanceApproval-->
            <div class="well well-sm" align="center">
                <img src="../Images/approval.png" alt="approval" />
                <p>
                    提出された勤務表の、<br />
                    承認を行います。
                </p>
                <asp:LinkButton ID="btnAttendanceApproval" runat="server" PostBackUrl="~/AttendanceApproval/FrmAttendanceApproval.aspx"
                    class="btn btn-default btn-lg btn-block">
                    勤務表承認
                </asp:LinkButton>
            </div>
        </div>

        <div class="col-md-4">
            <!--Master-->
            <div class="well well-sm" align="center">
                <img src="../Images/summary.png" alt="summary" />
                <p>
                    勤務表を各種条件で、<br />
                    集計、出力します。
                </p>
                <asp:LinkButton ID="btnAttendanceSummary" runat="server" PostBackUrl="~/AttendanceSummary/FrmAttendanceSummary.aspx"
                    class="btn btn-default btn-lg btn-block">
                    勤務表集計
                </asp:LinkButton>
            </div>
        </div>

        <div class="col-md-4">
            <!--Expense Registration-->
            <div class="well well-sm" align="center">
                <img src="../Images/expence.png" alt="summary" />
                <p>
                    宿泊・交通費などプロジェクトに利用した<br />
                    経費の申請・承認を行います。
                </p>
                <asp:LinkButton ID="btnExpenseRegistration" runat="server" PostBackUrl="~/Expence/FrmExpenceList.aspx"
                    class="btn btn-default btn-lg btn-block">
                    経費申請・承認
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4">
            <!--Pays-->
            <div class="well well-sm" align="center">
                <img src="../Images/payslip.png" alt="Master" />
                <p>
                    給与明細賞与明細を<br />
                    ダウンロードします。
                </p>
                <asp:LinkButton ID="btnAttendancePayslip" runat="server" PostBackUrl="~/Payslip/FrmPayslipList.aspx"
                    class="btn btn-default btn-lg btn-block">
                    給与賞与明細
                </asp:LinkButton>
            </div>
        </div>

        <div class="col-md-4">
            <!--Profit Management-->
            <div class="well well-sm" align="center">
                <img src="../Images/profit.png" alt="Master" />
                <p>
                    プロジェクト別の採算状況を<br />
                    一覧表示して管理します。
                </p>
                <asp:LinkButton ID="btnProfitManagement" runat="server" PostBackUrl="~/ProjectProfit/FrmProjectProfitList.aspx"
                    class="btn btn-default btn-lg btn-block">
                    採算管理
                </asp:LinkButton>
            </div>
        </div>

        <div class="col-md-4">
            <!--Sent Mail-->
            <div class="well well-sm" align="center">
                <img src="../Images/approval.png" alt="approval" />
                <p>
                    各種休暇申請状況の確認と<br />
                    休暇承認を行います。</p>
                <asp:LinkButton ID="btnApproval" runat="server" PostBackUrl="~/Approval/FrmApprovalList.aspx"
                    class="btn btn-default btn-lg btn-block">
                    各種休暇承認
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4">
            <!--Master-->
            <div class="well well-sm" align="center">
                <img src="../Images/master.png" alt="Master" />
                <p>
                    各種マスタの表示、<br />
                    登録、編集を行います。
                </p>
                <asp:LinkButton ID="btnMaster" runat="server" PostBackUrl="~/Menu/FrmMasterMenu.aspx"
                    class="btn btn-default btn-lg btn-block">
                    各種マスタ
                </asp:LinkButton>
            </div>
        </div>

        <div class="col-md-4">
            <!--Sent Mail-->
            <div class="well well-sm" align="center">
                <img src="../Images/mail.png" alt="Mail" />
                <p>
                    社内一斉メール送信と、<br />
                    返信状況の確認を行います。
                </p>
                <asp:LinkButton ID="btnSentMail" runat="server" PostBackUrl="~/Mail/FrmMailList.aspx"
                    class="btn btn-default btn-lg btn-block">
                    メール送信
                </asp:LinkButton>
            </div>
        </div>
    </div>


</asp:Content>
