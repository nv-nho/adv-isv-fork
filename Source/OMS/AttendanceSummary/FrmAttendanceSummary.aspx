<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmAttendanceSummary.aspx.cs" Inherits="OMS.Attendance.FrmAttendanceSummary" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .v-algin
        {
            vertical-align: middle !important;
        }
        .wrapper1
        {
            overflow-x: auto;
            overflow-y: hidden;
        }
        .wrapper1
        {
            height: 20px;
        }
        .div1
        {
            height: 20px;
        }
        .tr-normal
        {
        	background-color:White !important;
        }
</style>
 <script type="text/javascript">

    //**********************
    // Init
    //**********************
     $(function () {
         init();

         <%if(this.IsOutFile==true){ %> 
                 setTimeout(function()
            {
                getCtrlById("btnDownload").click();
                hideLoading();                  
            },0);
         <%} %>

         <%if(this.IsShowQuestion == true){ %>
            $('#modalQuestion').modal('show');

            $('#modalQuestion').on('shown.bs.modal', function (e) {
                $('<%=this.DefaultButton%>').focus();
            });

        <%} %>
         // focus When have error
         focusErrors();
         RadioCheck();

        if(__isTouchDevice){
            $(".wrapper1").remove();
        }

        setTimeout(function() {
            $(".div1").width($(".tbl-header").width() + 1);
        }, 50 );

        　$(".wrapper1").scroll(function () {            
            $(".scrollmenu2").scrollLeft($(".wrapper1").scrollLeft());     
        　});

        $(".scrollmenu2").scroll(function () {
            $(".wrapper1").scrollLeft($(".scrollmenu2").scrollLeft());         
        　});

        <%if (isShowButtonMenu){%>
            $(".sticky").show();
        <%}else{ %>
            $(".sticky").hide();
        <%} %>
     });

     getCtrlById("ddlOvertime_Vacation").bind("change", ddlOvertimeVacationSelectedIndexChanged);

     //****************************************
     // when ddlOvertime, Vacation change value
     //****************************************
     function ddlOvertimeVacationSelectedIndexChanged() {

        　getCtrlById("txtHour_Days").val("");
          getCtrlById("ddlCompare").val("0");
         if (getCtrlById("ddlOvertime_Vacation").val() != -1) {

             var Overtime_VacationValue = getCtrlById("ddlOvertime_Vacation").val()
             getCtrlById("txtHour_Days").removeAttr('readonly');
                 getCtrlById("ddlCompare").attr('disabled', false);
             if (Overtime_VacationValue.includes("Vacation") || Overtime_VacationValue.includes("Days")) {
                 getCtrlById("lblHour_day").text('日数');
                 getCtrlById("txtHour_Days").show();
                 getCtrlById("txtHour_Days").focus();
             }
             else {
                 getCtrlById("lblHour_day").text('時間');
                 getCtrlById("txtHour_Days").focus();
             }
         }
         else {
             getCtrlById("txtHour_Days").val('');
             getCtrlById("txtHour_Days").prop('readonly', true);
             getCtrlById("ddlCompare").attr('disabled', true);
         }
     }

     //**********************
     // Init
     //**********************
     function init() {
         if (getCtrlById("ddlOvertime_Vacation").val() != -1) {
             var Overtime_VacationValue = getCtrlById("ddlOvertime_Vacation").val()
             if (Overtime_VacationValue.includes("Vacation") || Overtime_VacationValue.includes("Days")) {
                 getCtrlById("lblHour_day").text('日数');
                 getCtrlById("txtHour_Days").removeAttr('readonly');
                 getCtrlById("ddlCompare").attr('disabled', false);
                 getCtrlById("txtHour_Days").show();
             }
             else {
                 getCtrlById("lblHour_day").text('時間');
             }
         }
         else {

             getCtrlById("txtHour_Days").prop('readonly', true);
             getCtrlById("ddlCompare").attr('disabled', true);
         }
     }

     //**********************
     // Clear Form
     //**********************
     function clearForm() {
         $(":text").val("");
         getCtrlById("ddlCompare").val("0");
         getCtrlById("rbTotal").prop('checked', true);
         getCtrlById("rbApprove").prop('checked', false);

         getCtrlById("ddlDateOfServiceTo").val(-1);
         getCtrlById("ddlDateOfServiceFrom").val(-1);
         getCtrlById("ddlDepartment1").val(-1);
         getCtrlById("ddlDepartment2").val(-1);
         getCtrlById("ddlOvertime_Vacation").val(-1);
         getCtrlById("ddlUser11").val(-1);
         getCtrlById("ddlUser12").val(-1);
         getCtrlById("ddlUser13").val(-1);
         getCtrlById("ddlUser14").val(-1);
         getCtrlById("ddlUser21").val(-1);
         getCtrlById("ddlUser22").val(-1);
         getCtrlById("ddlUser23").val(-1);
         getCtrlById("ddlUser24").val(-1);

         getCtrlById("ddlWorkingCalendar").focus().select();
         getCtrlById("ddlWorkingCalendar").val(getCtrlById("hdCalendarDefault").val());

         var ddl = getCtrlById("ddlWorkingCalendar");
         __doPostBack(ddl, '');
     }

     function RadioCheck() {
            var rdo = getCtrlById("rbTotal");
            if (rdo[0].checked == true) {
                $(".total_Condition").show();
                $(".approve_Condition").children('div').removeClass("has-error");
                $(".approve_Condition").hide();
                getCtrlById("ddlDateOfServiceTo").val(-1);
                getCtrlById("ddlDateOfServiceFrom").val(-1);
            }
            else 
            {
                $(".total_Condition").children('div').removeClass("has-error");
                $(".total_Condition").hide();
                $(".approve_Condition").show();

                getCtrlById("dtStartDate").val("");
                getCtrlById("dtEndDate").val("");
            }
    }

 </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <asp:HiddenField ID="hdSortField" runat="server" />
    <asp:HiddenField ID="hdSortUserDirec" runat="server" />
    <asp:HiddenField ID="hdSortDeptDirec" runat="server" />
    <asp:HiddenField ID="hdCalendarDefault" runat="server" />
    <%--Condition Search--%>
    <div class="well well-sm">
        <!--Collapse Button-->
        <button type="button" class="btn btn-default btn-sm" data-toggle="collapse" data-target="#viewdetails">
            <span class="glyphicon glyphicon-align-justify"></span>
        </button>
        <div class="collapse <%= this.Collapse %>" id="viewdetails">
            <div class="row">
                <%--Combobox WorkingCalendar--%>
                <div class="col-md-3">
                    <div class='form-group <%=GetClassError("ddlWorkingCalendar")%>'>
                        <label class="control-label" for="<%= ddlWorkingCalendar.ClientID %>">
                            勤務カレンダー<strong class="text-danger">*</strong></label>
                        <asp:DropDownList ID="ddlWorkingCalendar" runat="server" CssClass="form-control input-sm" 
                        AutoPostBack="True" OnSelectedIndexChanged="ddlWorkingCalendar_SelectedIndexChanged" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Work table status--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label">
                            勤務表状態</label>
                        <div class = "form-group">
                            <asp:RadioButton id="rbTotal"　Checked = "true" runat="server" GroupName="SearchApprove" onclick = "RadioCheck();"></asp:RadioButton>
                            <label class="control-label" for="<%= rbTotal.ClientID %>" style="text-align:left;font-weight: normal">全て</label>

                            <asp:RadioButton id="rbApprove" runat="server" GroupName="SearchApprove" onclick = "RadioCheck();"></asp:RadioButton>
                            <label class="control-label" for="<%= rbApprove.ClientID %>" style="text-align:left;font-weight: normal">承認済</label>
                        </div>
                    </div>
                </div>

                 <%--From Date--%>
                <div class="col-md-2 total_Condition">
                    <div class='form-group <%=GetClassError("dtStartDate")%>'>
                        <label class="control-label" for="<%= dtStartDate.ClientID %>">
                            勤務年月日</label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtStartDate" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <%--Kara--%>
                <div class="col-md-1 total_Condition">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>

                <%--End date--%>
                <div class="col-md-2 total_Condition">
                    <div class='form-group <%=GetClassError("dtEndDate")%>'>
                        <label class="control-label" for="<%= dtEndDate.ClientID %>">&nbsp;</label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtEndDate" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>

                <%--From Date--%>
                <div class="col-md-2 approve_Condition">
                    <div class='form-group <%=GetClassError("ddlDateOfServiceFrom")%>'>
                        <label class="control-label" for="<%= ddlDateOfServiceFrom.ClientID %>">
                            勤務年月</label>
                        <asp:DropDownList ID="ddlDateOfServiceFrom" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Kara--%>
                <div class="col-md-1 approve_Condition">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>

                <%--End date--%>
                <div class="col-md-2 approve_Condition">
                    <div class='form-group <%=GetClassError("ddlDateOfServiceTo")%>'>
                        <label class="control-label" for="<%= ddlDateOfServiceFrom.ClientID %>">&nbsp;</label>
                        <asp:DropDownList ID="ddlDateOfServiceTo" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="row">

                <%--Combobox Department--%>
                <div class="col-md-3">
                    <div class='form-group'>
                        <label class="control-label" for="<%= ddlDepartment1.ClientID %>">
                            部署</label>
                        <asp:DropDownList ID="ddlDepartment1" runat="server" CssClass="form-control input-sm" 
                        OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="True"
                            autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Combobox User--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlUser11.ClientID %>">
                            社員名</label>
                        <asp:DropDownList ID="ddlUser11" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Combobox User--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlUser12.ClientID %>">
                            社員名</label>
                        <asp:DropDownList ID="ddlUser12" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Combobox User--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlUser13.ClientID %>">
                            社員名</label>
                        <asp:DropDownList ID="ddlUser13" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Combobox User--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlUser14.ClientID %>">
                            社員名</label>
                        <asp:DropDownList ID="ddlUser14" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">

                <%--Combobox Department--%>
                <div class="col-md-3">
                    <div class='form-group '>
                        <label class="control-label" for="<%= ddlDepartment2.ClientID %>">
                            部署</label>
                        <asp:DropDownList ID="ddlDepartment2" runat="server" CssClass="form-control input-sm" 
                        OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="True"
                            autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Combobox User--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlUser21.ClientID %>">
                            社員名</label>
                        <asp:DropDownList ID="ddlUser21" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Combobox User--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlUser22.ClientID %>">
                            社員名</label>
                        <asp:DropDownList ID="ddlUser22" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Combobox User--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlUser23.ClientID %>">
                            社員名</label>
                        <asp:DropDownList ID="ddlUser23" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>

                <%--Combobox User--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlUser24.ClientID %>">
                            社員名</label>
                        <asp:DropDownList ID="ddlUser24" runat="server" CssClass="form-control input-sm" autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <%--Combobox Overtime--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlOvertime_Vacation.ClientID %>">
                            抽出条件</label>
                        <asp:DropDownList ID="ddlOvertime_Vacation" runat="server" CssClass="form-control input-sm"
                        autocomplete="off">
                        </asp:DropDownList>                    
                    </div>
                </div>
            
                <%--Combobox Compare--%>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label ">&nbsp;</label>
                        <div>
                            <label class="col-xs-2">が、</label>
                            <div class="col-xs-4">
                                <cc1:ICodeTextBox ID="txtHour_Days" runat="server" AllowChars="." CodeType="Numeric"
                                    CssClass="form-control input-sm" Text=""></cc1:ICodeTextBox>
                            </div>
                            <label id = "lblHour_day" class="col-xs-2">時間</label>
                            
                            <div class="col-xs-4">
                                <asp:DropDownList ID="ddlCompare" runat="server" CssClass="form-control input-sm" autocomplete="off">
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
                    <button type="button" id="btnClear" class="btn btn-default btn-sm" onclick="clearForm();">
                        <span class="glyphicon glyphicon-refresh"></span>&nbsp;クリア
                    </button>
                </div>
            </div>
        </div>
        </div>
    </div>

    <div class="well well-sm">
        <div class="row">
            <div class="col-md-4">
                <div class="btn-group btn-group-justified">      
                    <div class="btn-group">
                        <asp:LinkButton ID="btnExcelTop" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnExcel_Click">
                             <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</asp:LinkButton>
                        
                           <button type="button" id="btnDownload" class="btn btn-primary btn-sm hide" runat="server">
                                <span class="glyphicon glyphicon-search"></span>&nbsp;Download
                           </button>          
                    </div>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnCSVTop" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnCSV_Click">
                        <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;CSV</asp:LinkButton>  
                    </div>         
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack1" runat="server" PostBackUrl="../Menu/FrmMainMenu.aspx"
                            CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
            </div>
            <div class="col-md-2 col-sm-2">
                <div class="btn-group btn-group-justified">
                    <%-- Approval button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnExchangeExcelTop" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnExchangeExcel_Click">
                             振替休日一覧表 
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <uc2:PagingHeaderControl ID="PagingHeader" runat="server" />
    <%-- List WorkingCalendar--%>
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <div class= "row">
        <br />
    </div>


    <%-- List AttendanceApproval--%>

    <div class="sticky" style = "display: none;">
        <div class="wrapper1">
            <div class="div1">
            </div>
        </div>
    </div>

    <div class="form-group scrollmenu2" style="overflow: auto;">
    <asp:Panel ID="Panel1" runat="server"> 
        <asp:Repeater ID="rptAttendanceSummaryList" runat="server" OnItemDataBound="rptAttendanceSummaryList_ItemDataBound">
           <HeaderTemplate>         
                <table class="table table-bordered tbl-header" style="margin-bottom: 0px;font-size:12px;">
                    <thead id="headertemplate">
                        <%--Header row 1--%>
                        <tr class="active ">
                            <th  class=" v-algin text-center">
                                <div class="iColH-AB">
                                </div>
                            </th>
                            <th  class=" v-algin text-center">
                                <div class="iColH-AB">
                                    <asp:LinkButton ID="lnkSortUCode" runat="server" CommandArgument='U'
                                        OnClick="Sort_Click" class="loading"><%=GetHeadSortText("U")%></asp:LinkButton>
                                    /
                                    <asp:LinkButton ID="lnkSortDCode" runat="server" CommandArgument='D'
                                        OnClick="Sort_Click" class="loading"><%=GetHeadSortText("D")%></asp:LinkButton>
                                </div>
                            </th>
                            
                            <th colspan="2" class="v-algin text-center">
                                <div>
                                    実績時間
                                </div>
                            </th>
                            <th colspan="5" class="v-algin text-center">
                                <div>
                                   出勤
                                </div>
                            </th>
                             <th colspan="<%= NumVacation %>" class="v-algin text-center">
                               休暇
                            </th>
                             <th colspan="<%= NumOvertime %>" class="v-algin text-center">
                                残業
                            </th>
                        </tr>
                    </thead>
                   <tbody>
            </HeaderTemplate>
            <ItemTemplate>              
                <tr class="active" style="white-space: nowrap;vertical-align: middle">
                    <td rowspan="3" style="vertical-align: middle; background-color:White">
                        <%# Eval("RowNumber") %>
                    </td>
                    <td rowspan="3" style="vertical-align: middle; width:1000px" class="<%# Eval("tr-class") %>">
                        <%# Eval("UserCD")%><br />
                        <%# Eval("UserNm")%><br />
                        <%# Eval("DepartmentName")%><br />
                    </td>
                     <td class="v-algin text-center">
                        <div class="iColH-1">
                           総残業時間
                        </div>
                    </td>
                    <td  class="v-algin text-center">
                        <div class="iColH-2">
                            総労働時間
                        </div>
                    </td>
                    <td  class="v-algin text-center">
                        <div class="iColH-2">
                            出勤
                        </div>
                    </td>
                    <td  class="v-algin text-center">
                        <div class="iColH-2">
                           遅刻
                        </div>
                    </td>
                    <td class="v-algin text-center">
                        <div class="iColH-2">
                           早退
                        </div>
                    </td>
                    <td class="v-algin text-center">
                        <div class="iColH-2">
                            所定休日
                        </div>
                    </td>
                    <td class="v-algin text-center">
                        <div class="iColH-2">
                           法定休日
                        </div>
                    </td>
                    <asp:Repeater ID="rptVacationH" runat="server">
                        <ItemTemplate>
                           <td class="v-algin text-center" style="vertical-align: middle">
                                <div class="iColH-2">
                                    <%# Eval("Value3")%>
                                 </div>
                            </td>
                        </ItemTemplate>
                     </asp:Repeater>
                    <asp:Repeater ID="rptOverTimeH" runat="server">
                        <ItemTemplate>
                            <td class="v-algin text-center" style="vertical-align: middle">
                                <div class="iColH-2">
                                    <%# Eval("Value2")%>
                                 </div>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr class="<%# Eval("tr-class") %>">
                     <td rowspan="2" class="v-algin text-center" style="vertical-align: middle">
                        <div class="iColH-2" >
                            <%# Eval("TotalOverTimeHours")%>
                        </div>
                     </td>
                     <td rowspan="2" class="v-algin text-center" style="vertical-align: middle">
                        <div class="iColH-2 text-center ">
                            <%# Eval("TotalWorkingHours")%>
                        </div>
                     </td>
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                            <%# Eval("numWorkingDays")%>
                        </div>
                     </td>
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                            <%# Eval("numLateDays")%>
                        </div>
                     </td>
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                            <%# Eval("numEarlyDays")%>
                        </div>
                     </td>
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                            <%# Eval("numSH_Days")%>
                        </div>
                     </td>
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                              <%# Eval("numLH_Days")%>
                        </div>
                     </td>
                     <asp:Repeater ID="rptVacationD" runat="server">
                        <ItemTemplate>
                          <td rowspan="2" class="v-algin text-center" style="vertical-align: middle">
                                <div class="iColH-2">
                                     <%# Eval("VacationDate")%>
                                </div>
                          </td>
                      </ItemTemplate>
                     </asp:Repeater>
                    <asp:Repeater ID="rptOverTimeD" runat="server">
                        <ItemTemplate>
                            <td rowspan="2" class="v-algin text-center" style="vertical-align: middle">
                                <div class="iColH-2">
                                  <%# Eval("Value4")%>
                                </div>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr class="<%# Eval("tr-class") %>">
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                            <%# Eval("WorkingHours")%>
                        </div>
                     </td>
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                            <%# Eval("LateHours")%>
                        </div>
                     </td>
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                            <%# Eval("EarlyHours")%>
                        </div>
                     </td>
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                            <%# Eval("SH_Hours")%>
                        </div>
                     </td>
                     <td class="v-algin text-center">
                        <div class="iColH-2">
                            <%# Eval("LH_Hours")%>
                        </div>
                     </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
    </div>
    <uc1:PagingFooterControl ID="PagingFooter" runat="server" />
    <div class= "row">
        <br />
    </div>

    <%if (isShowButtonMenu)
      {%>
        <div class="well well-sm">
            <div class="row">
                <div class="col-md-4">
                    <div class="btn-group btn-group-justified">      
                        <div class="btn-group">
                            <asp:LinkButton ID="btnExcelBottom" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnExcel_Click">
                                 <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</asp:LinkButton>        
                        </div>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnCSVBottom" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnCSV_Click">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;CSV</asp:LinkButton>  
                        </div>      
                        <div class="btn-group">
                            <asp:LinkButton ID="btnBackBottom" runat="server" PostBackUrl="../Menu/FrmMainMenu.aspx"
                                CssClass="btn btn-default btn-sm loading">
                                <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>   
                <div class="col-md-6 col-sm-6">
                </div>
                <div class="col-md-2 col-sm-2">
                    <div class="btn-group btn-group-justified">
                        <%-- Approval button --%>
                        <div class="btn-group">
                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-sm btn-default text-left" OnCommand="btnExchangeExcel_Click">
                                 振替休日一覧表 
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>  
            </div>
        </div>
    <%} %>
</asp:Content>

