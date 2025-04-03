<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmAttendanceList.aspx.cs" Inherits="OMS.Attendance.FrmAttendanceList" %>

<%@ Import Namespace="OMS.Utilities" %>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .content-time-total {
            width: 73px;
        }

        .content-time-work {
            width: 85px;
        }

        .content-time {
            width: 50px;
        }

        .v-algin {
            vertical-align: middle !important;
        }

        .wrapper1, .wrapper2 {
            overflow-x: scroll;
            overflow-y: hidden;
        }

        .wrapper1 {
            height: 20px;
        }

        .wrapper2 {
        }

        .div1 {
            height: 20px;
        }

        .div2 {
            overflow: none;
        }
        .bg-yellow{
            background-color:yellow;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var _hasData = false;

        <%if (this.IsShowQuestion == true)
        { %>
            $('#modalQuestion').modal('show');

            $('#modalQuestion').on('shown.bs.modal', function (e) {
                $('<%=this.DefaultButton%>').focus();
            });
        <%} %>

        <%  else if (this.IsShowInfo)
            { 
        %>
            $('#modalInfo').modal('show');

            $('#modalInfo').on('shown.bs.modal', function (e) {
                $('<%=this.DefaultButton%>').focus();
            });
        <%} %>
        <%if (this.Success == true)
        { %>
            showSuccess();
            setTimeout(function () {
                hideSuccess();
            }, 1000);
        <%} %>

        <%if (this.IsOutFile == true)
        { %> 
            setTimeout(function () {
                getCtrlById("btnDownload").click();
                hideLoading();
            }, 0);
        <%} %>


        <%if (this.hasData)
        { %> 
            _hasData = true;
        <%} %>

            if (getCtrlById("ddlDateOfService").val() == -1 || getCtrlById("ddlDepartment").val() == -1 || getCtrlById("ddlUser").val() == -1 || !_hasData) {
                getCtrlById("ddlWorkingCalendar").focus();
                $('#tblRouter').hide();
                $('.scrollmenu2').hide();
                getCtrlById("btnRegisterDefault").hide();
                getCtrlById("btnSubmission").hide();
                getCtrlById("btnSubmissionCancel").hide();
                $(".Verification").hide();
            }
            else {
                $('#ListMenu2').show();
                $('#tblRouter').show();
                $('#tabledetail').show();
                $('.scrollmenu2').show();
                $('.wrapper1').show();
                getCtrlById("btnRegisterDefault").show();
                getCtrlById("btnSubmission").show();
                getCtrlById("btnSubmissionCancel").show();
                $(".Verification").show();
                if(getCtrlById("hidDetailClientID").val() != "")
                {
                    var offset = $('#' + getCtrlById("hidDetailClientID").val()).offset();
                    offset.left -= offset.left;
                    offset.top -= 200;
                    $('html, body').animate({
                        scrollTop: offset.top,
                        scrollLeft: offset.left
                    });
                    $('#' + getCtrlById("hidDetailClientID").val()).focus();
                }  

            }

            $(".btn-group").each(function () {
                if ($(this).children().hasClass("hidden")) {
                    $(this).addClass("hidden");
                }
            });

            if (__isTouchDevice) {
                $(".wrapper1").remove();
            }

            $(".div1").width($(".tbl-header").width() + 2);
            $(".wrapper1").scroll(function () {
                $(".scrollmenu2").scrollLeft($(".wrapper1").scrollLeft());

            });

            $(".scrollmenu2").scroll(function () {
                $(".wrapper1").scrollLeft($(".scrollmenu2").scrollLeft());

            });

            $(document).ready(function () {
                var hasVerticalScrollbar = $(".scrollmenu2")[0].scrollWidth > $(".scrollmenu2")[0].clientWidth;

                if (hasVerticalScrollbar) {
                    $('.sticky').show();
                }
                else {
                    $('.sticky').hide();
                }
            });

            // when resize window
            $(window).resize(function () {
                var hasVerticalScrollbar = $(".scrollmenu2")[0].scrollWidth > $(".scrollmenu2")[0].clientWidth;

                if (hasVerticalScrollbar) {
                    $('.sticky').show();
                }
                else {
                    $('.sticky').hide();
                }
            });
        });

    function showmodalVerification() {

        $('#modalVerification').modal('show');
    }

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <%--Condition Search--%>
    <asp:HiddenField ID="hidDetailClientID" runat="server"/>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-3">
                <div class='form-group <% =GetClassError("ddlWorkingCalendar")%>'>
                    <label class="control-label" for="<%= ddlWorkingCalendar.ClientID %>">
                        勤務カレンダー</label>
                    <asp:DropDownList ID="ddlWorkingCalendar" runat="server" CssClass="form-control input-sm"
                        OnSelectedIndexChanged="ddlWorkingCalendar_SelectedIndexChanged" AutoPostBack="True"
                        autocomplete="off">
                    </asp:DropDownList>
                </div>
            </div>
            <%--Date Of Service--%>
            <div class="col-md-2">
                <div class='form-group <% =GetClassError("ddlDateOfService")%>'>
                    <label class="control-label" for="<%= ddlDateOfService.ClientID %>">
                        勤務年月</label>
                    <asp:DropDownList ID="ddlDateOfService" runat="server" CssClass="form-control input-sm"
                        OnSelectedIndexChanged="ddlDateOfService_SelectedIndexChanged" AutoPostBack="True"
                        autocomplete="off">
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
            <div class="col-md-4">
            </div>
        </div>
    </div>
    <%--Create RegisterDefault and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-4 col-sm-4">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnExcel" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnExcel_Click">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</asp:LinkButton>
                        <button type="button" id="btnDownload" class="btn btn-default btn-sm hide" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;Download
                        </button>
                    </div>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack1" runat="server" OnClick="btnBack_Click"
                            CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <div class="col-md-4 col-sm-4">
                <div class="col-md-3 col-sm-6 Verification">
                    <label class="control-label" style="margin-top: 5px;">有給残数</label>
                </div>
                <div class="col-md-3 col-sm-6 Verification">
                    <cc1:ITextBox ID="txtVacationDay" runat="server" ReadOnly="true" CssClass="form-control input-sm text-center"></cc1:ITextBox>
                </div>

                <%--Verification--%>
                <div class="col-md-6 col-sm-12 Verification">
                    <button id="btnVerification" type="button" style="text-align: center; vertical-align: middle; line-height: 26px; width: 75px;" class="btn btn-default btn-xs" onclick="showmodalVerification();">確認</button>
                </div>
            </div>
            <%if (this.isApprovalForm == false)
                { %>
            <div class="col-md-4 col-sm-4">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnRegisterDefault" runat="server" CssClass="btn btn-primary btn-sm loading"
                            OnClick="btnRegisterDefault_Click" Style="display: none;">
                                     <span class="glyphicon glyphicon-plus-sign"></span>&nbsp;日報一括登録
                        </asp:LinkButton>
                    </div>
                    <%if (this.ShowSubmitCance == false && isShowbtnSubmitORbtnSubmitCancel == true)
                        { %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnSubmission" runat="server" CssClass="btn btn-primary btn-sm loading" OnCommand="btnSubmission_Click">
                                    <span class="glyphicon glyphicon-ok"></span>&nbsp;勤務表提出</asp:LinkButton>
                    </div>
                    <%} %>
                    <%if (this.ShowSubmitCance == true && isShowbtnSubmitORbtnSubmitCancel == true)
                        { %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnSubmissionCancel" runat="server" CssClass="btn btn-success btn-sm loading" OnCommand="btnSubmissionCancel_Click">
                                    <span class="glyphicon glyphicon-ok"></span>&nbsp;<%=nameButtonApproval%></asp:LinkButton>
                    </div>
                    <%} %>
                </div>
            </div>
            <%} %>
        </div>
    </div>
    <%--Header2 --%>
    <div class="form-group scrollmenu" style="overflow: auto; white-space: nowrap;">
        <table id="tblRouter" class="table table-bordered" style="width: 1140px; margin-bottom: 0px; display: none;">
            <tr class="">
                <th rowspan="4" class="text-center v-algin active" style="width: 10px;">当<br />
                    月<br />
                    合<br />
                    計
                </th>
                <th colspan="5" class="v-algin text-center active">出勤
                </th>
                <th class="text-center active" colspan="<%= NumVacation %>">休暇
                </th>
                <th class="text-center active" colspan="<%= NumOvertime %>">残業
                </th>
                <th rowspan="2" class="v-algin text-center active">総残業時間
                </th>
                <th rowspan="2" class="v-algin text-center active">総労働時間
                </th>
            </tr>
            <tr>
                <th class="v-algin text-center active  content-time-work">出勤
                </th>
                <th class="v-algin text-center active  content-time-work">遅刻
                </th>
                <th class="v-algin text-center active  content-time-work">早退
                </th>
                <th class="v-algin text-center active  content-time-work">所定休日
                </th>
                <th class="v-algin text-center active  content-time-work">法定休日
                </th>
                <asp:Repeater ID="rptVacationH1" runat="server">
                    <ItemTemplate>
                        <th class="v-algin text-center active content-time">
                            <%# Eval("Value3")%>
                        </th>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rptOverTimeH1" runat="server">
                    <ItemTemplate>
                        <th class="v-algin text-center active content-time-work">
                            <%# Eval("Value2")%>
                        </th>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
            <tr>
                <td class="text-center">
                    <asp:Label ID="numWorkingDays" runat="server" Text=""></asp:Label>
                </td>
                <td class="text-center">
                    <asp:Label ID="numLateDays" runat="server" Text=""></asp:Label>
                </td>
                <td class="text-center">
                    <asp:Label ID="numEarlyDays" runat="server" Text=""></asp:Label>
                </td>
                <td class="text-center">
                    <asp:Label ID="numSH_Days" runat="server" Text=""></asp:Label>
                </td>
                <td class="text-center">
                    <asp:Label ID="numLH_Days" runat="server" Text=""></asp:Label>
                </td>
                <asp:Repeater ID="rptVacationD1" runat="server">
                    <ItemTemplate>
                        <td rowspan="2" class="v-algin text-center" runat="server">
                            <%# Eval("Value4")%>
                        </td>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rptOverTimeD1" runat="server">
                    <ItemTemplate>
                        <td rowspan="2" class="v-algin text-center" runat="server">
                            <%# Eval("Value4")%>
                        </td>
                    </ItemTemplate>
                </asp:Repeater>
                <td rowspan="2" class="v-algin text-center" style="width: 94px;">
                    <asp:Label ID="totalOverTimeHours" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="2" class="v-algin text-center" style="width: 94px;">
                    <asp:Label ID="totalWorkingHours" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="v-algin text-center">
                    <asp:Label ID="timeWorkingHours" runat="server" Text=""></asp:Label>
                </td>
                <td class="v-algin text-center">
                    <asp:Label ID="timeLateHours" runat="server" Text=""></asp:Label>
                </td>
                <td class="v-algin text-center">
                    <asp:Label ID="timeEarlyHours" runat="server" Text=""></asp:Label>
                </td>
                <td class="v-algin text-center">
                    <asp:Label ID="timeSH_Hours" runat="server" Text=""></asp:Label>
                </td>
                <td class="v-algin text-center">
                    <asp:Label ID="timeLH_Hours" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <%-- Attendance --%>
    <div class="sticky" style="display: none;">
        <div class="wrapper1">
            <div class="div1">
            </div>
        </div>
    </div>
    <div class="form-group scrollmenu2" style="display: none; overflow: auto;">
        <asp:Repeater ID="rptAttendanceList" runat="server" OnItemDataBound="rptAttendanceList_ItemDataBound">
            <HeaderTemplate>

                <table class="table table-bordered tbl-header" style="margin-bottom: 0px;">
                    <thead id="headertemplate">
                        <%--Header row 1--%>
                        <tr class="active iColH-AB">
                            <th rowspan="2" colspan="2" class=" v-algin text-center">
                                <div class="iColH-AB">
                                    日付
                                </div>
                            </th>
                            <th colspan="3" class="v-algin text-center">
                                <div>
                                    勤務体系・実績時刻
                                </div>
                            </th>
                            <th colspan="5" class="v-algin text-center">
                                <div>
                                    実績時間
                                </div>
                            </th>
                            <th colspan="<%= NumOvertime %>" class="v-algin text-center">残業実績
                            </th>
                        </tr>
                        <%--Header row 2--%>
                        <tr class="active" style="white-space: nowrap;">
                            <th class="v-algin text-center">
                                <div class="iColH-1">
                                    勤務体系
                                </div>
                            </th>
                            <th class="v-algin text-center">
                                <div class="iColH-2">
                                    出勤時刻
                                </div>
                            </th>
                            <th class="v-algin text-center">
                                <div class="iColH-3">
                                    退出時刻
                                </div>
                            </th>
                            <th class="v-algin text-center">
                                <div class="iColH-4">
                                    出勤
                                </div>
                            </th>
                            <th class="v-algin text-center">
                                <div class="iColH-5">
                                    遅刻
                                </div>
                            </th>
                            <th class="v-algin text-center">
                                <div class="iColH-6">
                                    早退
                                </div>
                            </th>
                            <th class="v-algin text-center">
                                <div class="iColH-6">
                                    所定休日
                                </div>
                            </th>
                            <th class="v-algin text-center">
                                <div class="iColH-6">
                                    法定休日
                                </div>
                            </th>
                            <asp:Repeater ID="rptOverTimeH2" runat="server">
                                <ItemTemplate>
                                    <th class="v-algin text-center content-time-work iColH-OverTime">
                                        <%# Eval("Value2")%>
                                    </th>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <div id="divHeader" runat="server">
                    <%--Header row 1--%>
                    <tr id="TrHeaderHiden1" class="active iColH-AB">
                        <th rowspan="2" colspan="2" class=" v-algin text-center">
                            <div class="iColH-AB">
                                日付
                            </div>
                        </th>
                        <th colspan="3" class="v-algin text-center">
                            <div>
                                勤務体系・実績時刻
                            </div>
                        </th>
                        <th colspan="5" class="v-algin text-center">
                            <div>
                                実績時間
                            </div>
                        </th>
                        <th colspan="<%= NumOvertime %>" class="v-algin text-center">残業実績
                        </th>
                    </tr>
                    <%--Header row 2--%>
                    <tr id="TrHeaderHiden2" class="active">
                        <th class="v-algin text-center">
                            <div class="iColH-1">
                                勤務体系
                            </div>
                        </th>
                        <th class="v-algin text-center">
                            <div class="iColH-2">
                                出勤時刻
                            </div>
                        </th>
                        <th class="v-algin text-center">
                            <div class="iColH-3">
                                退出時刻
                            </div>
                        </th>
                        <th class="v-algin text-center">
                            <div class="iColH-4">
                                出勤
                            </div>
                        </th>
                        <th class="v-algin text-center">
                            <div class="iColH-5">
                                遅刻
                            </div>
                        </th>
                        <th class="v-algin text-center">
                            <div class="iColH-6">
                                早退
                            </div>
                        </th>
                        <th class="v-algin text-center">
                            <div class="iColH-6">
                                所定休日
                            </div>
                        </th>
                        <th class="v-algin text-center">
                            <div class="iColH-6">
                                法定休日
                            </div>
                        </th>
                        <asp:Repeater ID="rptOverTimeH2Hiden" runat="server">
                            <ItemTemplate>
                                <th class="v-algin text-center content-time-work iColH-OverTime">
                                    <%# Eval("Value2")%>
                                </th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                </div>
                <tr class="<%# Eval("TextColorCurrentDate") %>">
                    <td rowspan="2" class="iColB-A v-algin text-center <%# Eval("TextColorClass") %>">
                        <div class="" style="width: 90px;">
                            <%# Eval("StringDate")%>
                        </div>
                        <%# Eval("ApprovalStatus")%>
                        <asp:HiddenField ID="Date" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Date") %>' />
                        <asp:HiddenField ID="UserID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                    </td>
                    <td rowspan="2" class="iColB-B v-algin text-center text-color-normalday">
                        <div>
                            <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID")+"," + Eval("UID")+","+Eval("Date")+ "," + Eval("StartDate")  +"," + Eval("EndDate") + ",FrmAttendanceList"%>'
                                PostBackUrl="FrmAttendanceEntry.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading">
                                    <span class="glyphicon glyphicon-pencil"></span>
                            </asp:LinkButton>
                        </div>
                        <div>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("ID")+"," + Eval("UID")+","+Eval("Date")+ "," + Eval("StartDate")  +"," + Eval("EndDate") + ",FrmAttendanceList_Request"%>'
                                PostBackUrl="FrmAttendanceEntry.aspx" OnCommand="btnDetail_Click" Font-Underline="false">
                                    <span class="label label-primary" style="<%# Eval("ClassApprovalStatus")%>">申請</span>
                            </asp:LinkButton>
                        </div>
                    </td>
                    <td class="v-algin text-center iColB-1 text-color-normalday" style="width: 190px;">
                        <%# Eval("WorkingSystemName") %>
                    </td>
                    <td class="v-algin text-center iColB-2 text-color-normalday  " style="background-color: <%# Eval("BackColorInterValTime") %>">
                        <div class="content-time">
                            <%# Eval("EntryTime") %>
                        </div>
                    </td>
                    <td class="v-algin text-center iColB-3 text-color-normalday">
                        <div class="content-time">
                            <%# Eval("ExitTime")%>
                        </div>
                    </td>
                    <td class="v-algin text-center iColB-4 text-color-normalday">
                        <div class="content-time">
                            <%# Eval("WorkingHours")%>
                        </div>
                    </td>
                    <td class="v-algin text-center iColB-5 text-color-normalday">
                        <div class="content-time">
                            <%# Eval("LateHours")%>
                        </div>
                    </td>
                    <td class="v-algin text-center iColB-6 text-color-normalday">
                        <div class="content-time">
                            <%# Eval("EarlyHours")%>
                        </div>
                    </td>
                    <td class="v-algin text-center iColB-6 text-color-normalday">
                        <div class="content-time">
                            <%# Eval("SH_Hours")%>
                        </div>
                    </td>
                    <td class="v-algin text-center iColB-6 text-color-normalday">
                        <div class="content-time">
                            <%# Eval("LH_Hours")%>
                        </div>
                    </td>
                    <asp:Repeater ID="rptOverTimeD2" runat="server">
                        <ItemTemplate>
                            <td class="v-algin content-time-work text-center iColB-OverTime text-color-normalday" runat="server">
                                <div class="content-time">
                                    <%# Eval("Value4")%>
                                </div>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr class="<%# Eval("TextColorCurrentDate") %>">
                    <td colspan="<%= NumOvertime + 8 %>" class="text-left text-color-normalday">
                        <div style="min-height:20px;">
                            <%-- <%# Eval("Remark")%>--%>
                            <table class="" style="width: 100%;color:Red;">
                                <tbody>
                                    <tr>
                                        <td><%# Eval("ExchangeStatus")%></td>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:Repeater ID="rptRested" runat="server">
                                <HeaderTemplate>
                                    <table class="rptRested" style="width: 100%;">
                                        <thead>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="text-left">
                                            <%# Eval("ContentVacation")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody> </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="rptContent" runat="server">
                                <HeaderTemplate>
                                    <table class="rptContent" style="width: 100%;">
                                        <thead>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="text-left">
                                            <%# Eval("Content")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody> </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <table class="" style="width: 100%;">
                                <tbody>
                                    <tr>
                                        <td><%# Eval("Memo")%></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <%--Create New and Back Button--%>
    <div id="ListMenu2" class="well well-sm" style="display: none;">
        <div class="row">
            <div class="col-md-4 col-sm-4">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnExcelButtom" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnExcel_Click">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</asp:LinkButton>
                    </div>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" OnClick="btnBack_Click"
                            CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
            </div>
            <%if (this.isApprovalForm == false)
                { %>
            <div class="col-md-4 col-sm-4">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnRegisterDefaultBottom" runat="server" CssClass="btn btn-primary btn-sm loading"
                            OnClick="btnRegisterDefault_Click">
                                     <span class="glyphicon glyphicon-plus-sign"></span>&nbsp;日報一括登録
                        </asp:LinkButton>
                    </div>
                    <%if (this.ShowSubmitCance == false)
                        { %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnSubmitSsionButtom" runat="server" CssClass="btn btn-primary btn-sm loading" OnCommand="btnSubmission_Click">
                                    <span class="glyphicon glyphicon-ok"> </span>&nbsp;勤務表提出</asp:LinkButton>
                    </div>
                    <%} %>
                    <%if (this.ShowSubmitCance == true)
                        { %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnSubmitCancelButtom" runat="server" CssClass="btn btn-success btn-sm loading" OnCommand="btnSubmissionCancel_Click">
                                    <span class="glyphicon glyphicon-ok"> </span>&nbsp;<%=nameButtonApproval%></asp:LinkButton>
                    </div>

                    <%} %>
                </div>
            </div>
            <%} %>
        </div>
    </div>

    <div class="modalAlert modal fade" id="modalInfo" tabindex="-1" role="dialog" aria-labelledby="questionTitle" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
        <div class="modal-content">
        <div class="modal-header">
            <h4 class="modal-title" id="infoModalLabel"><span class='glyphicon glyphicon-info-sign'></span> 情報</h4>
            </div>

            <div class="modal-body" id="infoMessage" runat="server">
            </div>
            <div class="modal-footer">
            <asp:Button ID="btnInfoOk" runat="server" Text="OK" class="btn btn-default" data-dismiss="modal"/>                    
            </div>
        </div>
        </div>
    </div>

    <%--(Verification)--%>
    <div class="modalDetailAttendance modal fade" id="modalVerification" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-lg">
            <div class="modal-content" style="height: 310px; min-height: unset;">
                <div class="modal-header">
                    <h4 class="modal-title">有給休暇取得状況</h4>
                </div>
                <div class="modal-body">

                    <div class="row">
                        <div class="col-md-12">
                        <table class="table table-bordered" style="margin-bottom:0px;">
                            <thead class="table-header">
                                <asp:Repeater ID="rptVacationHeader" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <th class="text-center"></th>
                                            <th class="text-center"><%# Eval("StartMonth") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth1") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth2") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth3") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth4") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth5") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth6") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth7") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth8") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth9") %>月</th>
                                            <th class="text-center"><%# Eval("NextMonth10") %>月</th>
                                            <th class="text-center"><%# Eval("EndMonth") %>月</th>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptVacationList" runat="server">
                                    <ItemTemplate>
                                        <tr style="text-align: center">
                                            <td class="col-md-2.5">
                                                <label class="control-label"><%# Eval("Year") %></label>
                                            </td>
                                            <td class="col-md-0.5">
                                                <%# Eval("SumDayOff1") %>
                                            </td>
                                            <td class="col-md-0.5">
                                                <%# Eval("SumDayOff2") %>
                                            </td>
                                            <td class="col-md-0.5">
                                                <%# Eval("SumDayOff3") %>
                                            </td>
                                            <td class="col-md-0.5">
                                                <%# Eval("SumDayOff4") %>
                                            </td>
                                            <td class="col-md-0.5">
                                                <%# Eval("SumDayOff5") %>
                                            </td>
                                            <td class="col-md-0.5">
                                                <%# Eval("SumDayOff6") %>
                                            </td>
                                            <td class="col-md-0.5">
                                                <%# Eval("SumDayOff7") %>
                                            </td>
                                            <td class="col-md-0.5">
                                                <%# Eval("SumDayOff8") %>
                                            </td>
                                            <td class="col-md-0.5">
                                                <%# Eval("SumDayOff9") %>
                                            </td>
                                            <td class="col-md-1">
                                                <%# Eval("SumDayOff10") %>
                                            </td>
                                            <td class="col-md-1">
                                                <%# Eval("SumDayOff11") %>
                                            </td>
                                            <td class="col-md-1">
                                                <%# Eval("SumDayOff12") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <div class="col-md-12 col-sm-12 col-xs-12 text-right">
                        <button class="btn btn-default btn-sm loading" data-dismiss="modal">OK</button>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
