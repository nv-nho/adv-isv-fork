<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmGroupUserDetail.aspx.cs" Inherits="OMS.Master.FrmGroupUserDetail" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Import Namespace="OMS.Utilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        //**********************
        // Init
        //**********************
        $(function () {            
            $("[type=checkbox]").bootstrapSwitch();
            setFocus();

            // show message success
            <%if(this.Success == true){ %>
                showSuccess();
                 setTimeout(function() {
                     hideSuccess();
                }, 1000 );
            <%} %>

            <%if(this.IsShowQuestion == true){ %>
                $('#modalQuestion').modal('show');

                $('#modalQuestion').on('shown.bs.modal', function (e) {
                    $('<%=this.DefaultButton%>').focus();
                });

            <%} %>   

            $(".rowHeader").each(function(index) {
                if(index != 0){
                    displayAll(index , true);
                }
            });   

            $(".chkAuthAll").on('switchChange.bootstrapSwitch', chkAuthAllChange);
            
            <%if(this.Mode==Mode.View){ %>
                $(".chkAuthAll").bootstrapSwitch('readonly', true, true);
            <%} %>             

            $(".chkAuth").on('switchChange.bootstrapSwitch', chkAuthChange);
            
            <%if(this.Mode==Mode.View){ %>
                $(".chkAuth").bootstrapSwitch('readonly', true, true);
            <%} %>      

             $(".chkAuthWorkCalendar").on('switchChange.bootstrapSwitch', chkAuthWorkCalendarChange);    
            
            <%if(this.Mode==Mode.View){ %>
                $(".chkAuthWorkCalendar").bootstrapSwitch('readonly', true, true);
            <%} %>
            
             $(".chkAuthAttendance").on('switchChange.bootstrapSwitch', chkAuthAcceptChange);    
            
            <%if(this.Mode==Mode.View){ %>
                $(".chkAuthAttendance").bootstrapSwitch('readonly', true, true);
            <%} %>

            $(".chkAuthAttendanceApproval").on('switchChange.bootstrapSwitch', chkAuthAttendanceApprovalChange);    
            
            <%if(this.Mode==Mode.View){ %>
                $(".chkAuthAttendanceApproval").bootstrapSwitch('readonly', true, true);
            <%} %>

            $(".chkAuthAttendanceSummary").on('switchChange.bootstrapSwitch', chkAuthAuthAttendanceSummaryChange);    
            
            <%if(this.Mode==Mode.View){ %>
                $(".chkAuthAttendanceSummary").bootstrapSwitch('readonly', true, true);
            <%} %>

            $(".chkAuthAttendancePayslip").on('switchChange.bootstrapSwitch', chkAuthAuthAttendancePayslipChange);    
            
            <%if(this.Mode==Mode.View){ %>
                $(".chkAuthAttendancePayslip").bootstrapSwitch('readonly', true, true);
            <%} %>

            $(".chkAuthExpenceGroup").on('switchChange.bootstrapSwitch', chkAuthAuthExpenceGroupChange);

            <%if(this.Mode==Mode.View){ %>
            $(".chkAuthExpenceGroup").bootstrapSwitch('readonly', true, true);
            <%} %>

            <%if(this.Mode==Mode.View){ %>
            $(".chkAuthApproval").bootstrapSwitch('readonly', true, true);
            <%} %>
            $(".chkAuthApproval").on('switchChange.bootstrapSwitch', chkAuthApprovalChange);

            
        });
        var checkItem = false;

        function displayAll(colIndex , state){
            var countCheckTrue = 0;
            var countCheckFalse = 0;
            var items = null;
            if(colIndex != 6 && colIndex != 4 && colIndex != 2 && colIndex != 3 && colIndex != 5){
                items = $(".rowDetail");
            } else if (colIndex == 2) {
                items = $(".authInsert");
            }
            else if (colIndex == 3) {
                items = $(".authEdit");
            }
            else if (colIndex == 4) {
                items = $(".authCopy");
            }
            else if (colIndex == 5) {
                items = $(".authDelete");
            }
            else {
                items = $(".authExcel");
            }  
                     
            items.each(function(index) {
                    
                if (colIndex != 1)
                {
                    if($(this).is(':checked')){
                        countCheckTrue ++;
                    }else{
                        countCheckFalse ++;
                    }
                }
                else{
                    if(getCtrlById("chkAuth" + colIndex, index).is(':checked')){
                        countCheckTrue ++;
                    }else{
                        countCheckFalse ++;
                    }
                }
                
            });

            if(countCheckTrue ==  items.length){
                state = true;
                getCtrlById("chkAuthAll" ,colIndex).bootstrapSwitch('state', state, true);
            }

            if(countCheckFalse ==  items.length){
                state = false;
                getCtrlById("chkAuthAll" ,colIndex).bootstrapSwitch('state', state, false);
            }
        }

        function chkAuthAllChange(event,state){
            if(checkItem){
                checkItem = false;
                return;
            } 

            var colIndex = $(event.target).parents("td")[0].cellIndex;
            var isChecked = getCtrlById("chkAuthAll", colIndex).is(':checked');      

            $(".rowDetail").each(function( index ) {
                getCtrlById("chkAuth" + colIndex , index).bootstrapSwitch('state', state, isChecked);
            });

            if(colIndex == 1){
                if(!isChecked){                    
                    for (var i = 2; i <= 6; i++) {
                        getCtrlById("chkAuthAll", i).bootstrapSwitch('state', state, isChecked);

                        $(".rowDetail").each(function (index) {
                            getCtrlById("chkAuth" + i, index).bootstrapSwitch('state', state, isChecked);
                        });
                    }
                }             
                return;
            }

            if(colIndex == 2 || colIndex == 3 || colIndex == 4 || colIndex == 5){
                if(isChecked){                    
                    getCtrlById("chkAuthAll" ,1).bootstrapSwitch('state', state, isChecked);                                        

                    $(".rowDetail").each(function( index ) {
                        getCtrlById("chkAuth" + 1 , index).bootstrapSwitch('state', state, isChecked);
                    });
                }             
                return;                
            }    
            
            if(colIndex == 6){
                if(isChecked){     
                    $(".authExcel").each(function( index ) {
                        getCtrlById("chkAuth" + 1 , index).bootstrapSwitch('state', state, isChecked);
                    });            
                }   
                return;                 
            }               
        }       

        //**********************
        // Auth Change
        //**********************
        function chkAuthChange(event, state) {
            var rowIndex = $(event.target).parents("tr")[0].rowIndex - 1;
            var colIndex = $(event.target).parents("td")[0].cellIndex;
            var isChecked = getCtrlById("chkAuth" + colIndex, rowIndex).is(':checked');                       
            
            displayAll(colIndex, state);

            if(colIndex == 1){
                if(!isChecked){
                    for (var i = 2; i <= 6; i++) {
                        // メール送信
                        setDispAuthMaster(i, rowIndex, isChecked);
                    }
                                        
                    if(getCtrlById("chkAuthAll" ,colIndex).is(':checked')){
                        checkItem = true;
                        getCtrlById("chkAuthAll" ,colIndex).bootstrapSwitch('state', state, false);                         
                    }                   
                }
                return;
            }

            if(colIndex == 2 || colIndex == 3 || colIndex == 4 || colIndex == 5 || colIndex == 6){
                if (isChecked) {
                    // メール送信
                    setDispAuthMaster(1, rowIndex, isChecked);
                    displayAll(1, state);
                }else{ 
                   if(getCtrlById("chkAuthAll" ,colIndex).is(':checked')){
                        checkItem = true;
                        getCtrlById("chkAuthAll" ,colIndex).bootstrapSwitch('state', state, false);                         
                    }     
                }
                return;                
            }                          
        }

        //**********************
        // Auth WorkCalendar Change
        //**********************
        function chkAuthWorkCalendarChange(event, state) {
            var colIndex = $(event.target).parents("td")[0].cellIndex+1;
            var isChecked = getCtrlById("chkWorkCalendarAuth"+colIndex).is(':checked');           

            if(colIndex == 1){
                if(!isChecked){
                
                    setDispAuth("chkWorkCalendarAuth2", false);
                    setDispAuth("chkWorkCalendarAuth3", false);
                    setDispAuth("chkWorkCalendarAuth4", false);
                    setDispAuth("chkWorkCalendarAuth5", false);
                    setDispAuth("chkWorkCalendarAuth6", false);
                    setDispAuth("chkWorkCalendarAuth7", false);
                    setDispAuth("chkWorkCalendarAuth8", false);
                }
                return;
            }

             if(colIndex == 2 || colIndex == 3 || colIndex == 4 || colIndex == 5 || colIndex == 6){
                if(isChecked){
                    setDispAuth("chkWorkCalendarAuth1", isChecked);
                }
                return;                
            }          
        }

        //**********************
        // Auth Attendance Change
        //**********************
        function chkAuthAcceptChange(event, state) {
            var colIndex = $(event.target).parents("td")[0].cellIndex+1;
            var isChecked = getCtrlById("chkAttendanceAuth"+colIndex).is(':checked');           

            if(colIndex == 1){
                if(!isChecked){
                
                    setDispAuth("chkAttendanceAuth2", false);
                    setDispAuth("chkAttendanceAuth3", false);
                    setDispAuth("chkAttendanceAuth4", false);
                    setDispAuth("chkAttendanceAuth5", false);
                    setDispAuth("chkAttendanceAuth6", false);
                    setDispAuth("chkAttendanceAuth7", false);
                    setDispAuth("chkAttendanceAuth8", false);
                    setDispAuth("chkAttendanceAuth9", false);
                }
                return;
            }

             if(colIndex == 2 || colIndex == 3 || colIndex == 4 || colIndex == 5 || colIndex == 6
                || colIndex == 7|| colIndex == 8|| colIndex == 9){

                if(isChecked){
                    setDispAuth("chkAttendanceAuth1", isChecked);
                }
                if(colIndex == 5)
                {
                    if(isChecked){
                        setDispAuth("chkAttendanceAuth6", true);
                    }
                }    
                if(colIndex == 6){
                    if(!isChecked){
                        setDispAuth("chkAttendanceAuth5", false);
                    }
                    if(!isChecked){
                        setDispAuth("chkAttendanceAuth7", false);
                    }
                }
                if(colIndex == 7){
                    if(isChecked){
                        setDispAuth("chkAttendanceAuth6", true);
                    }
                }
                
                return;                
            }
        }

        //**********************
        // Auth AttendanceApproval Change
        //**********************
        function chkAuthAttendanceApprovalChange(event, state) {
            var colIndex = $(event.target).parents("td")[0].cellIndex+1;
            var isChecked = getCtrlById("chkAttendanceApprovalAuth"+colIndex).is(':checked');           

            if(colIndex == 1){
                if(!isChecked){
                
                    setDispAuth("chkAttendanceApprovalAuth2", false);
                    setDispAuth("chkAttendanceApprovalAuth3", false);
                }
                return;
            }

             if(colIndex == 2 || colIndex == 3){
                if(isChecked){
                    setDispAuth("chkAttendanceApprovalAuth1", isChecked);
                }
                return;                
            }          
        }

        //**********************
        // Auth AttendanceSummary Change
        //**********************
        function chkAuthAuthAttendanceSummaryChange(event, state) {
            var colIndex = $(event.target).parents("td")[0].cellIndex+1;
            var isChecked = getCtrlById("chkAuthAttendanceSummaryAuth"+colIndex).is(':checked');           

            if(colIndex == 1){
                if(!isChecked){
                
                    setDispAuth("chkAuthAttendanceSummaryAuth2", false);
                }
                return;
            }

             if(colIndex == 2){
                if(isChecked){
                    setDispAuth("chkAuthAttendanceSummaryAuth1", isChecked);
                }
                return;                
            }          
        }

        //**********************
        // Auth AttendancePayslip Change
        //**********************
        function chkAuthAuthAttendancePayslipChange(event, state) {
            var colIndex = $(event.target).parents("td")[0].cellIndex+1;
            var isChecked = getCtrlById("chkAuthAttendancePayslipAuth"+colIndex).is(':checked');           
            
            if(colIndex == 1){
                if(!isChecked){
                    setDispAuth("chkAuthAttendancePayslipAuth2", false);
                    setDispAuth("chkAuthAttendancePayslipAuth3", false);
                    setDispAuth("chkAuthAttendancePayslipAuth4", false);
                }
                return;
            }

            if(colIndex == 2 || colIndex == 3 || colIndex == 4)
            {
                if(isChecked){
                    setDispAuth("chkAuthAttendancePayslipAuth1", isChecked);
                }
            
                if(colIndex == 2)
                {
                    if(isChecked){
                        setDispAuth("chkAuthAttendancePayslipAuth3", true);
                    }
                }    
                if(colIndex == 3){
                    if(!isChecked){
                        setDispAuth("chkAuthAttendancePayslipAuth2", false);
                    }
                }

                return;
            }        
        }
        
        //**********************
        // Auth ExpenceGroup Change
        //**********************
        function chkAuthAuthExpenceGroupChange(event, state) {
            var colIndex = $(event.target).parents("td")[0].cellIndex + 1;
            var isChecked = getCtrlById("chkAuthExpenceGroupAuth" + colIndex).is(':checked');

            if (colIndex == 1) {
                if (!isChecked) {
                    setDispAuth("chkAuthExpenceGroupAuth2", false);
                    setDispAuth("chkAuthExpenceGroupAuth3", false);
                    setDispAuth("chkAuthExpenceGroupAuth4", false);
                    setDispAuth("chkAuthExpenceGroupAuth5", false);
                    setDispAuth("chkAuthExpenceGroupAuth6", false);
                    setDispAuth("chkAuthExpenceGroupAuth7", false);
                    setDispAuth("chkAuthExpenceGroupAuth8", false);
                    setDispAuth("chkAuthExpenceGroupAuth9", false);
                    setDispAuth("chkAuthExpenceGroupAuth10", false);
                    setDispAuth("chkAuthExpenceGroupAuth11", false);
                    setDispAuth("chkAuthExpenceGroupAuth12", false);
                }
                return;
            }

            if (colIndex != 1 ) {
                if (isChecked) {
                    setDispAuth("chkAuthExpenceGroupAuth1", isChecked);
                }
                if (colIndex == 7) {
                    if (!isChecked) {
                        setDispAuth("chkAuthExpenceGroupAuth8", false);
                        setDispAuth("chkAuthExpenceGroupAuth9", false);
                    }
                }
                if (colIndex == 8) {
                    if (isChecked) {
                        setDispAuth("chkAuthExpenceGroupAuth7", true);
                    }
                }
                if (colIndex == 9) {
                    if (isChecked) {
                        setDispAuth("chkAuthExpenceGroupAuth7", isChecked);
                    }
                }
                return;
            }
        }

        //**********************
        // Auth Approval Change
        //**********************
        function chkAuthApprovalChange(event, state) {
            var colIndex = $(event.target).parents("td")[0].cellIndex+1;
            
            if(colIndex == 1 || colIndex == 2)
            {
                var isChecked1 = getCtrlById("chkAuthApprovalAuth1").is(':checked');           
                var isChecked2 = getCtrlById("chkAuthApprovalAuth2").is(':checked');           
                
                if(!isChecked1 && !isChecked2){
                    setDispAuth("chkAuthApprovalAuth3", false);
                }
            }
            
            if(colIndex == 3)
            {
                var isChecked3 = getCtrlById("chkAuthApprovalAuth3").is(':checked');
                if(isChecked3){
                    setDispAuth("chkAuthApprovalAuth1", true);
                }
            }
        }

        //**********************
        // set Disp Auth Master
        //**********************
        function setDispAuthMaster(colIndex, rowIndex, state) {            
            getCtrlById("chkAuth" + colIndex, rowIndex).bootstrapSwitch('state', state, state);
        }

        //**********************
        // set Disp Auth
        //**********************
        function setDispAuth(chkAuth, state) {            
            getCtrlById(chkAuth).bootstrapSwitch('state', state, state);
        }

        //**********************
        // Set Focus
        //**********************
        function setFocus() {
            <%if(this.Mode==Mode.Insert || this.Mode==Mode.Copy){ %>
                getCtrlById("txtGroupCode").focus().select();
            <%} %>
            
            <%if(this.Mode==Mode.Update){ %>
                getCtrlById("txtGroupName").focus().select();
            <%} %>            
        }     

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <div class="well well-sm">
        <div class="row">
            <%--group code--%>
            <div class="col-md-2">
                <div class="form-group <%=GetClassError("txtGroupCode")%>">
                    <label class="control-label" for="<%= txtGroupCode.ClientID %>">
                        権限グループ<strong class="text-danger"> *</strong>
                    </label>
                    <cc1:ICodeTextBox ID="txtGroupCode" runat="server" CssClass="form-control input-sm"
                        AjaxUrlMethod="GetGroup" LabelNames="txtGroupCode"></cc1:ICodeTextBox>
                </div>
            </div>
            <%--Group Name--%>
            <div class="col-md-4">
                <div class="form-group <%=GetClassError("txtGroupName")%>">
                    <label class="control-label" for="<%= txtGroupName.ClientID %>">
                        権限グループ名<strong class="text-danger"> *</strong></label>
                    <cc1:ITextBox ID="txtGroupName" runat="server" CssClass="form-control input-sm"> </cc1:ITextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <strong>マスタ操作権限</strong>
        </div>
        <div class="panel-body">
            <table class="table table-striped table-condensed">
                <thead>
                    <tr>
                        <asp:Repeater ID="rptFormHeader" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <td class="rowHeader">
                                    <div class="<%# Eval("Text").ToString().Substring(1) == "" ? "hidden" : ""%>">
                                        <strong>
                                            <%# Eval("Text").ToString().Substring(1)%><br />
                                        </strong>
                                        <cc1:ICheckBox ID="chkAuthAll" runat="server" data-on-color="success" data-off-color="danger"
                                            data-size="mini" Class="chkAuthAll" checked='<%# Convert.ToBoolean(Convert.ToInt16(Eval("Text").ToString().Substring(0,1)))%>'>
                                        </cc1:ICheckBox>
                                    </div>
                                </td>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                    </tr>
                </thead>
                <asp:Repeater ID="rptFormDetail" runat="server" OnItemDataBound="rptFormDetail_ItemDataBound">
                    <HeaderTemplate>
                        <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="rowDetail" class="rowDetail">
                            <td>
                                <asp:Label ID="lblFormName" runat="server" Text='<%# Eval("FormName")%>'></asp:Label>
                                <asp:HiddenField ID="hdFormId" runat="server" Value='<%# Eval("FormID")%>' />
                            </td>
                            <td>
                                <cc1:ICheckBox ID="chkAuth1" runat="server" data-on-color="success" data-off-color="danger"
                                    data-size="mini" checked='<%# Convert.ToBoolean(Eval("AuthorityFlag1")) %>' Class="chkAuth">
                                </cc1:ICheckBox>
                            </td>
                            <td>
                                <cc1:ICheckBox ID="chkAuth2" runat="server" data-on-color="success" data-off-color="danger"
                                    data-size="mini" checked='<%# Convert.ToBoolean(Eval("AuthorityFlag2")) %>' Class="chkAuth authInsert">
                                </cc1:ICheckBox>
                            </td>
                            <td>
                                <cc1:ICheckBox ID="chkAuth3" runat="server" data-on-color="success" data-off-color="danger"
                                    data-size="mini" checked='<%# Convert.ToBoolean(Eval("AuthorityFlag3")) %>' Class="chkAuth authEdit">
                                </cc1:ICheckBox>
                            </td>
                            <td>
                                <cc1:ICheckBox ID="chkAuth4" runat="server" data-on-color="success" data-off-color="danger"
                                    data-size="mini" checked='<%# Convert.ToBoolean(Eval("AuthorityFlag4")) %>' Class="chkAuth authCopy">
                                </cc1:ICheckBox>
                            </td>
                            <td>
                                <cc1:ICheckBox ID="chkAuth5" runat="server" data-on-color="success" data-off-color="danger"
                                    data-size="mini" checked='<%# Convert.ToBoolean(Eval("AuthorityFlag5")) %>' Class="chkAuth authDelete">
                                </cc1:ICheckBox>
                            </td>
                            <td>
                                <cc1:ICheckBox ID="chkAuth6" runat="server" data-on-color="success" data-off-color="danger"
                                    data-size="mini" checked='<%# Convert.ToBoolean(Eval("AuthorityFlag6")) %>' Class="chkAuth authExcel">
                                </cc1:ICheckBox>                                
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <strong>勤務カレンダー</strong>
        </div>
        <div class="panel-body">
            <table class="table table-striped table-condensed">
                <thead>
                    <tr>
                        <asp:Repeater ID="rptFormWorkCalendarHeader" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <td>
                                    <strong>
                                        <%# Eval("Text")%>
                                    </strong>
                                </td>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                    </tr>
                </thead>
                <tr id="rowQuotationDetail">
                    <td>
                        <cc1:ICheckBox id="chkWorkCalendarAuth1" class="chkAuthWorkCalendar" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini" ></cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkWorkCalendarAuth2" class="chkAuthWorkCalendar" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkWorkCalendarAuth3" class="chkAuthWorkCalendar" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkWorkCalendarAuth4" class="chkAuthWorkCalendar" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkWorkCalendarAuth5" class="chkAuthWorkCalendar" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkWorkCalendarAuth6" class="chkAuthWorkCalendar" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <strong>勤務表</strong>
        </div>
        <div class="panel-body">
            <table class="table table-striped table-condensed">
                <thead>
                    <tr>
                        <asp:Repeater ID="rptFormWorkScheduleHeader" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <td>
                                    <strong>
                                        <%# Eval("Text")%>
                                    </strong>
                                </td>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                    </tr>
                </thead>
                <tr id="rowAttendanceDetail">
                    <td>
                        <cc1:ICheckBox id="chkAttendanceAuth1" class="chkAuthAttendance" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkAttendanceAuth2" class="chkAuthAttendance" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkAttendanceAuth3" class="chkAuthAttendance" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkAttendanceAuth4" class="chkAuthAttendance" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkAttendanceAuth5" class="chkAuthAttendance" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkAttendanceAuth6" class="chkAuthAttendance" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkAttendanceAuth7" class="chkAuthAttendance" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkAttendanceAuth8" class="chkAuthAttendance" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                    <td>
                        <cc1:ICheckBox id="chkAttendanceAuth9" class="chkAuthAttendance" runat="server"
                            data-on-color="success" data-off-color="danger" data-size="mini">
                        </cc1:ICheckBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <!--AttendanceApproval-->
    <div class="panel panel-default">
        <div class="panel-heading">
            <strong>勤務表承認 </strong>
        </div>
        <div class="panel-body">
                <table class="table table-striped table-condensed">
                    <thead>
                        <tr>
                            <asp:Repeater ID="rptAttendanceApprovalHeader" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td>
                                        <strong>
                                            <%# Eval("Text")%>
                                        </strong>
                                    </td>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                             <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        </tr>
                    </thead>
                    <tr id="rowAttendanceApprovalDetail">
                        <td>
                            <cc1:ICheckBox id="chkAttendanceApprovalAuth1" class="chkAuthAttendanceApproval" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" ></cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAttendanceApprovalAuth2" class="chkAuthAttendanceApproval" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAttendanceApprovalAuth3" class="chkAuthAttendanceApproval" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            <strong>勤務表集計</strong>
        </div>
        <div class="panel-body">
                <table class="table table-striped table-condensed">
                    <thead>
                        <tr>
                            <asp:Repeater ID="rptAttendanceSummaryHeader" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td>
                                        <strong>
                                            <%# Eval("Text")%>
                                        </strong>
                                    </td>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        </tr>
                    </thead>
                    <tr id="rowAttendanceSummaryDetail">
                        <td>
                            <cc1:ICheckBox id="chkAuthAttendanceSummaryAuth1" class="chkAuthAttendanceSummary" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" >
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthAttendanceSummaryAuth2" class="chkAuthAttendanceSummary" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            <strong>給与賞与明細</strong>
        </div>
        <div class="panel-body">
            <table class="table table-striped table-condensed">
                    <thead>
                        <tr>
                            <asp:Repeater ID="rptAttendancePayslipHeader" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td>
                                        <strong>
                                            <%# Eval("Text")%>
                                        </strong>
                                    </td>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        </tr>
                    </thead>
                    <tr id="rowAttendancePayslip">
                        <td>
                            <cc1:ICheckBox id="chkAuthAttendancePayslipAuth1" class="chkAuthAttendancePayslip" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" >
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthAttendancePayslipAuth2" class="chkAuthAttendancePayslip" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthAttendancePayslipAuth3" class="chkAuthAttendancePayslip" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>                        
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthAttendancePayslipAuth4" class="chkAuthAttendancePayslip" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>                        
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <strong>経費申請・承認</strong>
        </div>
        <div class="panel-body">
            <table class="table table-striped table-condensed">
                    <thead>
                        <tr>
                            <asp:Repeater ID="RptExpenceGroupHeader" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td>
                                        <strong>
                                            <%# Eval("Text")%>
                                        </strong>
                                    </td>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tr>
                    </thead>
                    <tr id="rowExpenceGroup">
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth1" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" >
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth2" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth3" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>                        
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth4" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>                        
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth5" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" >
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth6" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth7" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>                        
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth8" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>                        
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth9" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" >
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth10" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth11" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>                        
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthExpenceGroupAuth12" class="chkAuthExpenceGroup" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini">
                            </cc1:ICheckBox>                        
                        </td>
                    </tr>
                </table>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <strong>有給申請</strong>
        </div>
        <div class="panel-body">
                <table class="table table-striped table-condensed">
                    <thead>
                        <tr>
                            <asp:Repeater ID="rptApprovalHeader" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td>
                                        <strong>
                                            <%# Eval("Text")%>
                                        </strong>
                                    </td>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                            
                        </tr>
                    </thead>
                    <tr id="rowApproval">
                        <td>
                            <cc1:ICheckBox id="chkAuthApprovalAuth1" class="chkAuthApproval" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" >
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthApprovalAuth2" class="chkAuthApproval" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" >
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthApprovalAuth3" class="chkAuthApproval" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" >
                            </cc1:ICheckBox>
                        </td>
                        <td>
                            <cc1:ICheckBox id="chkAuthApprovalAuth4" class="chkAuthApproval" runat="server"
                                data-on-color="success" data-off-color="danger" data-size="mini" >
                            </cc1:ICheckBox>
                        </td>
                    </tr>
                </table>
        </div>
    </div>
    <!-- /form well-->
    <%
        if (this.Mode != OMS.Utilities.Mode.Delete)
        {
    %>
    <!-- /form well-->
    <div class="row">
        <!-- Left Button Panel -->
        <div class="col-md-6">
            <div class="well well-sm">
                <div class="btn-group btn-group-justified">
                    <%
            if (this.Mode == OMS.Utilities.Mode.View)
            {
                    %>
                    <%-- Edit button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm loading"
                            OnClick="btnEdit_Click">
                                    <span class="glyphicon glyphicon-pencil"></span>&nbsp;編集
                        </asp:LinkButton>
                    </div>
                    <%-- Copy button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnCopy" runat="server" CssClass="btn btn-default btn-sm loading"
                            OnClick="btnCopy_Click">
                                    <span class="glyphicon glyphicon-paperclip"></span>&nbsp;コピー
                        </asp:LinkButton>
                    </div>
                    <%-- Delete button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-default btn-sm loading"
                            OnClick="btnDelete_Click">
                                    <span class="glyphicon glyphicon-trash"></span>&nbsp;削除
                        </asp:LinkButton>
                    </div>
                    <%
            }
            else if (this.Mode == OMS.Utilities.Mode.Insert || this.Mode == OMS.Utilities.Mode.Copy)
            {                                
                    %>
                    <%-- Insert button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnInsert" runat="server" CssClass="btn btn-primary btn-sm loading"
                            OnClick="btnInsert_Click">
                                    <span class="glyphicon glyphicon-ok"></span>&nbsp;登録
                        </asp:LinkButton>
                    </div>
                    <%
            }
            else if (this.Mode == OMS.Utilities.Mode.Update)
            {                                
                    %>
                    <%-- Update button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm loading"
                            OnClick="btnUpdate_Click">
                                    <span class="glyphicon glyphicon-ok"></span>&nbsp;登録
                        </asp:LinkButton>
                    </div>
                    <%
            }
                    %>
                    <%
            if (this.Mode == OMS.Utilities.Mode.View || this.Mode == OMS.Utilities.Mode.Insert)
            {
                    %>
                    <%-- Back button (back to list page)--%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-default btn-sm loading"
                            PostBackUrl="~/Master/FrmGroupUserList.aspx">
                                    <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                    <%
            }
            else
            {
                    %>
                    <%-- Back button (back to mode show) --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack1" runat="server" CssClass="btn btn-default btn-sm loading"
                            OnClick="btnBack_Click">
                                    <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                    <%
            }
                    %>
                </div>
            </div>
        </div>
        <!-- Right Button Panel -->
        <% if (this.Mode == OMS.Utilities.Mode.View)
           { %>
        <div class="col-md-6">
            <div class="well well-sm">
                <div class="row">
                    <!-- New Buton -->
                    <div class="col-md-6">
                        <div class="btn-group btn-group-justified">
                            <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmGroupUserDetail.aspx"
                                OnCommand="btnNew_Click" CssClass="btn btn-primary btn-sm loading">
                                    <span class="glyphicon glyphicon-plus"></span> 新規
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <% } %>
    </div>
    <!--/処理ボタングループ-->
    <% } %>
</asp:Content>
