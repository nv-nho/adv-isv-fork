<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmAttendanceApproval.aspx.cs" Inherits="OMS.AttendanceApproval.FrmAttendanceApproval" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .wrapper1
        {
            overflow-x: scroll;
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
    </style>
    <script type="text/javascript">

        //**********************
        // Init
        //**********************
        $(function () {
            setFocus();
            <%if(this.IsShowQuestion == true){ %>
                $('#modalQuestion').modal('show');

                $('#modalQuestion').on('shown.bs.modal', function (e) {
                    $('<%=this.DefaultButton%>').focus();
                });

             <%} %>
            <%if(this.Success == true){ %>
                showSuccess();
                 setTimeout(function() {
                     hideSuccess();
                }, 1000 );
            <%} %>

         $(getCtrlById("chkSelectAll")).bind("change", function () {
                $(".selectFlg").prop("checked", this.checked);
            });

         selectFlg();

         if(__isTouchDevice){
            $(".wrapper1").remove();
         }
         
            setTimeout(function() {
                $(".div1").width($(".tbl-header").width() + 2);
            }, 50 );

            $(".wrapper1").scroll(function () {            
                $(".scrollmenu2").scrollLeft($(".wrapper1").scrollLeft());     
            });

            $(".scrollmenu2").scroll(function () {
                $(".wrapper1").scrollLeft($(".scrollmenu2").scrollLeft());         
            });
            
        });

       //**********************
        // Init & Regist Event for selectFlg
        //**********************
        function selectFlg(){
            
             var checkAll = true;

            var minLength = 0;
            $(".selectFlg").each(function(i, ctr){
                minLength = ( i + 1);
                if(!$(ctr).is(":checked")){
                    checkAll = false;
                    return;
                }
            }).bind("change", function(){
                var longId = $(this).attr("id");
                var shortId = "chkSelectlg";
                
                if($(this).is(":checked")){
                    // Description: edit script 
                    // Author: ISV-PHUONG
                    // Date  : 2014/12/05
                    // ---------------------- Start ------------------------------
                    // ---------------------- End   ------------------------------
                }else{
                    $(getCtrlById("chkSelectAll")).prop("checked", false);
                }

                if($(".selectFlg:not(:checked)").length == 0){
                    $(getCtrlById("chkSelectAll")).prop("checked", true);
                }

            });
            $(getCtrlById("chkSelectAll")).prop("checked", (checkAll && minLength > 0));
        }

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
          
            getCtrlById("ddlDateOfServiceTo").val(-1);
            getCtrlById("ddlDateOfServiceFrom").val(-1);
            getCtrlById("ddlDepartment").val(-1);
            getCtrlById("ddlUser").val(-1);
      
            getCtrlById("cmbInvalidData").val(getCtrlById("hdInValideDefault").val());
            getCtrlById("ddlWorkingCalendar").val(getCtrlById("hdCalendarDefault").val());

           var ddl =  getCtrlById("ddlWorkingCalendar");
            __doPostBack(ddl,'');

          $('#viewdetails').attr("class", "collapse in");

        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <%-- hidden field default--%>
    <asp:HiddenField ID="hdInValideDefault" runat="server" />
    <asp:HiddenField ID="hdCalendarDefault" runat="server" />
    <%-- Search Condition--%>
    <div class="well well-sm">
        <!--Collapse Button-->
        <button type="button" class="btn btn-default btn-sm" data-toggle="collapse" data-target="#viewdetails">
            <span class="glyphicon glyphicon-align-justify"></span>
        </button>
        <div class="collapse <%= this.Collapse %>" id="viewdetails">
            <div class="row">
                <%--ddlWorkingCalendar--%>
                <div class="col-md-3">
                    <div class='form-group <% =GetClassError("ddlWorkingCalendar")%>'>
                        <label class="control-label" for="<%= ddlWorkingCalendar.ClientID %>">
                            勤務カレンダー<strong class="text-danger">*</strong></label>
                        <asp:DropDownList ID="ddlWorkingCalendar" runat="server" CssClass="form-control input-sm"
                            OnSelectedIndexChanged="ddlWorkingCalendar_SelectedIndexChanged" AutoPostBack="True"
                            autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--ddlDateOfServiceFrom--%>
                <div class="col-md-2">
                    <div class='form-group <% =GetClassError("ddlDateOfServiceFrom")%>'>
                        <label class="control-label" for="<%= ddlDateOfServiceFrom.ClientID %>">
                            勤務年月</label>
                        <asp:DropDownList ID="ddlDateOfServiceFrom" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>
                <%--ddlDateOfServiceTo--%>
                <div class="col-md-2">
                    <div class='form-group <% =GetClassError("ddlDateOfServiceTo")%>'>
                        <label class="control-label" for="<%= ddlDateOfServiceTo.ClientID %>">
                            &nbsp;</label>
                        <asp:DropDownList ID="ddlDateOfServiceTo" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <%--ddlDepartment--%>
                <div class="col-md-3">
                    <div class='form-group <% =GetClassError("ddlDepartment")%>'>
                        <label class="control-label" for="<%= ddlDepartment.ClientID %>">
                            部署
                        </label>
                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control input-sm"
                            OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="True"
                            autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--txtUserNm--%>
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlUser.ClientID %>">
                            社員名</label>
                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--Invalid Data--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbInvalidData.ClientID %>">
                            承認済</label>
                        <asp:DropDownList ID="cmbInvalidData" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
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
    <%--Create Approval,Release and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-3 col-sm-3">
                <div class="btn-group btn-group-justified">
                    <%-- Back button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" PostBackUrl="../Menu/FrmMainMenu.aspx"
                            CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-5 col-sm-5">
            </div>
            <%if (isShowButtonAction == true)
              { %>
            <div class="col-md-4 col-sm-4">
                <div class="btn-group btn-group-justified">
                    <%-- Approval button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnApproval" runat="server" OnCommand="btnApproval_Click" CssClass="btn btn-primary btn-sm loading">
                                   <span class="glyphicon glyphicon-ok"> </span>&nbsp;承認
                        </asp:LinkButton>
                    </div>
                    <%-- Release button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnRelease" runat="server" OnCommand="btnRelease_Click" CssClass="btn btn-warning btn-sm loading">
                                   <span class="glyphicon glyphicon-remove"> </span> &nbsp;解除
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <%} %>
        </div>
    </div>

    <uc2:PagingHeaderControl ID="PagingHeader" runat="server" />
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <div class= "row">
        <br />
    </div>

    <%-- List AttendanceApproval--%>
    <%if (isShowButtonAction == true)
      { %>
    <div class="sticky">
        <div class="wrapper1">
            <div class="div1">
            </div>
        </div>
    </div>
    <%} %>
    <div class="form-group scrollmenu2" style="overflow: auto; white-space: nowrap;">
        <asp:Repeater ID="rptAttendanceApprovalList" runat="server" OnItemDataBound="rptAttendanceApprovalList_ItemDataBound">
            <HeaderTemplate>
                <table class="table table-bordered tbl-header" style="margin-bottom: 0px;">
                    <thead id="headertemplate">
                        <tr class="text-center v-algin active">
                            <th class=" v-algin text-center">
                                <div class="iColH-AB">
                                </div>
                            </th>
                            <th class=" v-algin text-center">
                                <div class="iColH-AB">
                                    <input type="checkbox" id="chkSelectAll" runat="server" />
                                </div>
                            </th>
                            <th class=" v-algin text-center">
                                詳細
                            </th>
                            <th class=" v-algin text-center">
                                <div class="iColH-AB">
                                    社員名
                                </div>
                            </th>
                            <th class=" v-algin text-center">
                                <div class="iColH-AB">
                                    承認状態
                                </div>
                            </th>
                            <th class=" v-algin text-center">
                                <div class="iColH-AB">
                                    対象月
                                </div>
                            </th>
                            <th colspan="2" class="v-algin text-center active">
                                <div>
                                    実績時間
                                </div>
                            </th>
                            <th colspan="5" class="v-algin text-center active">
                                <div>
                                    出勤
                                </div>
                            </th>
                            <th colspan="<%= NumVacation %>" class="v-algin text-center active">
                                休暇
                            </th>
                            <th colspan="<%= NumOvertime %>" class="v-algin text-center activer">
                                残業
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="active" style="white-space: nowrap; vertical-align: middle; text-align: center">
                    <asp:HiddenField ID="hinUID" runat="server" Value='<%# Eval("UID")%>' />
                    <asp:HiddenField ID="hinStartDate" runat="server" Value='<%# Eval("StartDate")%>' />
                    <asp:HiddenField ID="hidEnddate" runat="server" Value='<%# Eval("EndDate") %>' />
                    <asp:HiddenField ID="hidCallendarID" runat="server" Value='<%# Eval("CallendarID") %>' />
                    <asp:HiddenField ID="hidStatusFlag" runat="server" Value='<%# Eval("StatusFlag") %>' />
                    <td rowspan="3" style="vertical-align: middle; background-color: white">
                        <%# Eval("RowNumber") %>
                    </td>
                    <td rowspan="3" style="vertical-align: middle; background-color: white">
                        <input id="chkSelectlg" class="selectFlg" type="checkbox" runat="server" checked='<%# Eval("CheckFlag")%>' />
                    </td>
                    <td rowspan="3" style="vertical-align: middle; background-color: white">
                        <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("UID")+","+Eval("UserNm")+","+Eval("DepartmentID")+","+Eval("DepartmentName")+"," + Eval("StartDate")  +"," + Eval("EndDate") + "," + Eval("CallendarID")%>'
                            PostBackUrl="/Attendance/FrmAttendanceList.aspx" OnCommand="btnDetail_Click"
                            CssClass="btn btn-info btn-sm loading"> 
                            <span class="glyphicon glyphicon-pencil"></span>
                        </asp:LinkButton>
                    </td>
                    <td rowspan="3" style="vertical-align: middle; text-align: left; background-color: white">
                        <%# Eval("UserCD")%><br />
                        <%# Eval("UserNm")%><br />
                        <%# Eval("DepartmentName")%><br />
                    </td>
                    <td rowspan="3" style="vertical-align: middle; background-color: white">
                        <span class="<%# Eval("CssNameStatus")%>">
                            <%# Eval("NameStatus")%></span>
                    </td>
                    <td rowspan="3" style="vertical-align: middle; background-color: white">
                        <%# Eval("RangeDate")%>
                    </td>
                    <td class="v-algin text-center">
                        <div class="iColH-1">
                            総残業時間
                        </div>
                    </td>
                    <td class="v-algin text-center">
                        <div class="iColH-2">
                            総労働時間
                        </div>
                    </td>
                    <td class="v-algin text-center">
                        <div class="iColH-2">
                            出勤
                        </div>
                    </td>
                    <td class="v-algin text-center">
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
                <tr>
                    <td rowspan="2" class="v-algin text-center" style="vertical-align: middle">
                        <div class="iColH-2">
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
                <tr>
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
    </div>
     <uc1:PagingFooterControl ID="PagingFooter" runat="server" />
    <div class= "row">
        <br />
    </div>

    <%if (isShowButtonAction)
      { %>
    <%--Create Approval,Release and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-3 col-sm-3">
                <div class="btn-group btn-group-justified">
                    <%-- Back button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="LinkBackButtom" runat="server" PostBackUrl="../Menu/FrmMainMenu.aspx"
                            CssClass="btn btn-default btn-sm loading">
                                    <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-5 col-sm-5">
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="btn-group btn-group-justified">
                    <%-- Approval button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnApprovalButtom" runat="server" OnCommand="btnApproval_Click"
                            CssClass="btn btn-primary btn-sm loading">
                                          <span class="glyphicon glyphicon-ok"> </span>&nbsp;承認
                        </asp:LinkButton>
                    </div>
                    <%-- Release button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnReleaseButtom" runat="server" OnCommand="btnRelease_Click"
                            CssClass="btn btn-warning btn-sm loading">
                                       <span class="glyphicon glyphicon-remove"> </span>&nbsp;解除
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%} %>
   
</asp:Content>
