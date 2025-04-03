<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmAttendanceEntry.aspx.cs" Inherits="OMS.Attendance.FrmAttendanceEntry" %>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>

<%@ Import Namespace="OMS.Utilities" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<script type ="text/javascript">

    //**********************
    // Init
    //**********************
    $(function () {

        getCtrlById("cmbWorkSchedule").bind("change", cmbWorkScheduleSelectedIndexChanged);
        initDetailList();

        // change int to time
        $( ".timetoint" ).each(function() {

            var id = this.id;
            var value = this.value;
            if(value.indexOf(":") == -1)
            {
                $("#"+ id).val( IntToTime(value));
            }
        });
        LockHolidayWorkingTimeByWorkingType(getCtrlById("cmbWorkSchedule").val());
        LockControlText();
        CheckToLockCombobox();
        SetDisplayExchangeDate();

        <%if(this.Mode == Mode.View || this.Mode == Mode.Delete || this.Mode == Mode.Approval || this.Mode == Mode.Request){ %>
            lock(1);
        <%}%>

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

        setFocus();
        focusErrors();
    });

    //************************************
    // cmbWorkScheduleSelectedIndexChanged
    //************************************
    function cmbWorkScheduleSelectedIndexChanged() {
        
        var workingSystemId = getCtrlById("cmbWorkSchedule").val();
        
        var params = { 'workingSystemId': workingSystemId};

        ajax("GetValueByWorkingSystemId", params, function (response) {
            if (response.d) {
            var result = eval('(' + response.d + ')');
            var startTime = "";
            var leaveTime = "";
            var workingType = "";
            var workingCd = "";
            
            if(result["WorkingStart"] != "0")
            {
                startTime = MinuteToTime(result["WorkingStart"]);
            }
            if(result["WorkingEnd"] !=  "0")
            {
                leaveTime = MinuteToTime(result["WorkingEnd"]); 
            }

            getCtrlById("txtEntryTime").val(startTime);
            getCtrlById("txtExitTime").val(leaveTime);

            // remove readonly 
            LockEntry_ExitTime(0);
            workingType = result["WorkingType"];
            workingCd = result["WorkingCd"];

            var offAllDay = getCtrlById("cmbAllHolidays").val();
            var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
            var offAfternoon = getCtrlById("cmbLateHoliday").val();
            
            if(workingCd != "4" && workingType == 0)
            {
                offAllDay = -1;
            }

            //call function to set value for server
            GetAndSetValueForTable(workingSystemId, startTime, leaveTime, offAllDay, offMorning, offAfternoon);
            
            if(workingType != 0)
            {
                if(getCtrlById("cmbAllHolidays").val() != "-1")
                {
                    getCtrlById("cmbAllHolidays").val("-1"); 
                }
                if(getCtrlById("cmbBeforeHalfHoliday").val() != "-1")
                {
                    getCtrlById("cmbBeforeHalfHoliday").val("-1");
                }
                if(getCtrlById("cmbLateHoliday").val() != "-1")
                {
                    getCtrlById("cmbLateHoliday").val("-1");
                }

                // Lock comboboxOffs
                lockComboboxOffs(1);

            }
            else 
            {
                if(workingCd == "4")
                {
                    //ClearEntry_ExitTime
                    ClearEntry_ExitTime();
                    // Lock comboboxOffs
                    getCtrlById("txtWorkingHours").val("");
                    getCtrlById("txtTheTotalWorkingHours").val("");

                    // get FirstHours
                    /*var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
                    var offAfternoon = getCtrlById("cmbLateHoliday").val();
                    var params = { 'workingSystemId': workingSystemId,
                               'offMorning':offMorning,
                               'offAfternoon':offAfternoon
                             };
                     ajax("GetWorkingHourByVacation", params, function (response) {
                        if (response.d) {
                        var result = eval('(' + response.d + ')');

                        getCtrlById("txtWorkingHours").removeAttr('readonly');
                        getCtrlById("txtWorkingHours").val(result["WorkingHours"]);

                        getCtrlById("txtTheTotalWorkingHours").val(result["TotalWorkingHours"]);

                        }
                    });*/

                    getCtrlById("cmbAllHolidays").val(0);
                    if(getCtrlById("cmbAllHolidays").val() != "-1")
                    {
                        getCtrlById("cmbBeforeHalfHoliday").val(-1);
                    }
                    if(getCtrlById("cmbAllHolidays").val() != "-1")
                    {
                        getCtrlById("cmbLateHoliday").val(-1);
                    }

                    getCtrlById("cmbAllHolidays").attr('disabled', false);
                    getCtrlById("cmbBeforeHalfHoliday").attr('disabled', true);
                    getCtrlById("cmbLateHoliday").attr('disabled', true);
                }
                else
                {
                    // Lock LockEntry_ExitTime
                    LockEntry_ExitTime(0);

                    // Lock comboboxOffs
                    lockComboboxOffs(0)
                    getCtrlById("txtEntryTime").focus();
                    getCtrlById("cmbAllHolidays").val(-1);
                }
            }

            // Check Lock Combobox (dayOff, morningOff, afternoonOff)
            CheckToLockCombobox();

            if(workingCd != "4" && workingType == 0)
            {
                offAllDay = -1;
                /*if(getCtrlById("cmbBeforeHalfHoliday").val() == 0 && getCtrlById("cmbLateHoliday").val() == 0)
                {
                    // get FirstHours
                     var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
                    var offAfternoon = getCtrlById("cmbLateHoliday").val();
                    var params = { 'workingSystemId': workingSystemId,
                               'offMorning':offMorning,
                               'offAfternoon':offAfternoon
                             };
                     ajax("GetWorkingHourByVacation", params, function (response) {
                        if (response.d) {
                        var result = eval('(' + response.d + ')');
                        getCtrlById("txtWorkingHours").val(result["WorkingHours"]);
                        getCtrlById("txtTheTotalWorkingHours").val(result["TotalWorkingHours"]);

                        getCtrlById("txtWorkingHours").removeAttr('readonly');

                        getCtrlById("txtEntryTime").val("");
                        getCtrlById("txtExitTime").val("");
                        }
                    });
                    getCtrlById("cmbWorkSchedule").focus();
                }*/
            }

            SetDisplayExchangeFlag(workingType);
            SetDisplayExchangeDate();
          }
       });
    };

    //******************************
    // when txtEntryTime change value
    //******************************
    getCtrlById("txtEntryTime").bind("change",function(){

        var workingSystemId = getCtrlById("cmbWorkSchedule").val();
        var workingType = GetWorkingType(workingSystemId);
        var startTime = ChangeFormat(getCtrlById("txtEntryTime").val());
        var leaveTime = getCtrlById("txtExitTime").val();

        var offAllDay = getCtrlById("cmbAllHolidays").val();
        var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
        var offAfternoon = getCtrlById("cmbLateHoliday").val();

        if(leaveTime == startTime)
        {
            startTime = "";
        }

        if(startTime != "" && leaveTime != "")
        {
            //call function to set value for server
            GetAndSetValueForTable(workingSystemId, startTime, leaveTime, offAllDay, offMorning, offAfternoon);
        }
        else
        {
            if(workingType == 0)
            {
                getCtrlById("txtWorkingHours").val("");
            }
            else if(workingType == 1)
            {
                getCtrlById("txtCertainHoliday").val("");
            }
            else if(workingType == 2)
            {
                getCtrlById("txtLegalHoliday").val("");
            }

            getCtrlById("txtLate").val("");
            getCtrlById("txtEarlyHours").val("");

            //Clear table Overtime 
            ClearTableOverTime();
        }

        if(offAllDay != -1)
        {
            LockTableClosingTime(1);
        }
        offAllDay = -1;
        /*if(getCtrlById("cmbBeforeHalfHoliday").val() == 0 && getCtrlById("cmbLateHoliday").val() == 0)
        {
            // get FirstHours
                var params = { 'workingSystemId': workingSystemId };
                ajax("GetWorkingHourByVacation", params, function (response) {
                if (response.d) {
                var result = eval('(' + response.d + ')');
                getCtrlById("txtWorkingHours").val(result["WorkingHours"]);
                getCtrlById("txtTheTotalWorkingHours").val(result["TotalWorkingHours"]);

                getCtrlById("txtWorkingHours").removeAttr('readonly');

                getCtrlById("txtEntryTime").val("");
                getCtrlById("txtExitTime").val("");
                }
            });
        }*/
    });

    //******************************
    // when txtExitTime change value
    //******************************
    getCtrlById("txtExitTime").bind("change",function(){

        var workingSystemId = getCtrlById("cmbWorkSchedule").val();
        var workingType = GetWorkingType(workingSystemId);

        var startTime = getCtrlById("txtEntryTime").val();
        var leaveTime = ChangeFormat(getCtrlById("txtExitTime").val());

        var offAllDay = getCtrlById("cmbAllHolidays").val();
        var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
        var offAfternoon = getCtrlById("cmbLateHoliday").val();


        if(leaveTime == startTime)
        {
            leaveTime = "";
        }

        if(startTime != "" && leaveTime != "")
        {
            //call function to set value for server
            GetAndSetValueForTable(workingSystemId, startTime, leaveTime, offAllDay, offMorning, offAfternoon);
        }
        else 
        {
            if(workingType == 0)
            {
                getCtrlById("txtWorkingHours").val("");
            }
            else if(workingType == 1)
            {
                getCtrlById("txtCertainHoliday").val("");
            }
            else if(workingType == 2)
            {
                getCtrlById("txtLegalHoliday").val("");    
            }

            getCtrlById("txtLate").val("");
            getCtrlById("txtEarlyHours").val("");

            // Clear Table OverTime
            ClearTableOverTime();
        }
        if(offAllDay != -1)
        {
            LockTableClosingTime(1);
        }
        offAllDay = -1;
//        if((getCtrlById("cmbBeforeHalfHoliday").val() == 0 && getCtrlById("cmbLateHoliday").val() == 0) || (getCtrlById("cmbBeforeHalfHoliday").val() != 0 && getCtrlById("cmbLateHoliday").val() == 0) || (getCtrlById("cmbBeforeHalfHoliday").val() == 0 && getCtrlById("cmbLateHoliday").val() != 0))
//        {
//            // get FirstHours
//                var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
//                var offAfternoon = getCtrlById("cmbLateHoliday").val();
//                var params = { 'workingSystemId': workingSystemId,
//                               'offMorning':offMorning,
//                               'offAfternoon':offAfternoon
//                             };
//                ajax("GetWorkingHourByVacation", params, function (response) {
//                if (response.d) {
//                    var result = eval('(' + response.d + ')');
//                    getCtrlById("txtWorkingHours").val(result["WorkingHours"]);
//                    getCtrlById("txtTheTotalWorkingHours").val(result["TotalWorkingHours"]);

//                    getCtrlById("txtWorkingHours").removeAttr('readonly');

//                    getCtrlById("txtEntryTime").val("");
//                    getCtrlById("txtExitTime").val("");
//                }
//            });
//        }
    });

    //*************************
    // GetAndSetValueForTable
    //*************************
    function GetAndSetValueForTable(workingSystemId, startTime, leaveTime, offAllDay, offMorning, offAfternoon)
    {
        var params1 = { 'workingSystemId': workingSystemId,
                        'entryTimeValue': startTime,
                        'exitTimeValue': leaveTime,
                        'offAllDay': offAllDay,
                        'offMorning': offMorning,
                        'offAfternoon': offAfternoon
                      };

        ajax("GetValueTableClosingTime", params1, function (response) {
            if (response.d) {
            var result1 = eval('(' + response.d + ')');

            var workingHours = result1["WorkingHours"];
            var LateHours = result1["LateHours"];
            var EarlyHours = result1["EarlyHours"];
            var EarlyOT = result1["EarlyOT"];
            var NormalOT = result1["NormalOT"];
            var LateNightOT = result1["LateNightOT"];
            var OT_04 = result1["OT_04"];
            var OT_05 = result1["OT_05"];
            var TotalOverTimeHours = result1["TotalOverTimeHours"];
            var TotalWorkingHours = result1["TotalWorkingHours"];
            var workingType =  GetWorkingType(workingSystemId);

            $("#MainContent_rpttbConfig4_Data_txtOverTime_0").val(EarlyOT);
            $("#MainContent_rpttbConfig4_Data_txtOverTime_1").val(NormalOT);
            $("#MainContent_rpttbConfig4_Data_txtOverTime_2").val(LateNightOT);
            $("#MainContent_rpttbConfig4_Data_txtOverTime_3").val(OT_04);
            $("#MainContent_rpttbConfig4_Data_txtOverTime_4").val(OT_05);

            getCtrlById("txtTheTotalOvertime").removeAttr('readonly');
            getCtrlById("txtTheTotalOvertime").val(TotalOverTimeHours);
            getCtrlById("txtTheTotalOvertime").attr('readonly','true');

            getCtrlById("txtTheTotalWorkingHours").removeAttr('readonly');
            getCtrlById("txtTheTotalWorkingHours").val(TotalWorkingHours);
            getCtrlById("txtTheTotalWorkingHours").attr('readonly','true');

            if(workingType == "0")
            {
                // remove attr readonly
                getCtrlById("txtWorkingHours").removeAttr('readonly');
                getCtrlById("txtLate").removeAttr('readonly');
                getCtrlById("txtEarlyHours").removeAttr('readonly');

                // set tabindex
                getCtrlById("txtCertainHoliday").enableTab();
                getCtrlById("txtLegalHoliday").enableTab();

                //set value
                getCtrlById("txtWorkingHours").val(workingHours);
                getCtrlById("txtCertainHoliday").val("");
                getCtrlById("txtLegalHoliday").val("");

                getCtrlById("txtLate").val(LateHours);
                getCtrlById("HidOldLate").val(LateHours);
                
                getCtrlById("txtEarlyHours").val(EarlyHours);
                getCtrlById("HidOldEarlyHours").val(EarlyHours);
                
                // set realonly
                getCtrlById("txtCertainHoliday").attr('readonly','true');
                getCtrlById("txtLegalHoliday").attr('readonly','true');

                // set tabindex
                getCtrlById("txtCertainHoliday").disableTab();
                getCtrlById("txtLegalHoliday").disableTab();

            }
            else if(workingType == "1")
            {
                // remove readonly
                getCtrlById("txtCertainHoliday").removeAttr('readonly');
                getCtrlById("txtCertainHoliday").enableTab();

                getCtrlById("txtWorkingHours").val("");
                getCtrlById("txtCertainHoliday").val(workingHours);
                getCtrlById("txtLegalHoliday").val("");
                getCtrlById("txtLate").val("");
                getCtrlById("txtEarlyHours").val("");

                // set readonly
                getCtrlById("txtWorkingHours").attr('readonly','true');
                getCtrlById("txtLegalHoliday").attr('readonly','true');
                getCtrlById("txtLate").attr('readonly','true');
                getCtrlById("txtEarlyHours").attr('readonly','true');

                // set tabindex
                getCtrlById("txtWorkingHours").disableTab();
                getCtrlById("txtLegalHoliday").disableTab();
                getCtrlById("txtLate").disableTab();
                getCtrlById("txtEarlyHours").disableTab();

            }
            else if(workingType == "2")
            {
                // set readonly
                getCtrlById("txtLegalHoliday").removeAttr('readonly');
                getCtrlById("txtLegalHoliday").enableTab();

                getCtrlById("txtWorkingHours").val("");
                getCtrlById("txtCertainHoliday").val("");
                getCtrlById("txtLegalHoliday").val(workingHours);
                getCtrlById("txtLate").val("");
                getCtrlById("txtEarlyHours").val("");

                //set realonly
                getCtrlById("txtWorkingHours").attr('readonly','true');
                getCtrlById("txtCertainHoliday").attr('readonly','true');
                getCtrlById("txtLate").attr('readonly','true');
                getCtrlById("txtEarlyHours").attr('readonly','true');

                // set tabindex
                getCtrlById("txtWorkingHours").disableTab();
                getCtrlById("txtCertainHoliday").disableTab();
                getCtrlById("txtLate").disableTab();
                getCtrlById("txtEarlyHours").disableTab();
            }
          }
       });
    }

    //***************
    // GetWorkingType
    //***************
    function GetWorkingType(workingSystemId)
    {
        var workingType;
        var params = {  'workingSystemId': workingSystemId };

            ajax("GetValueByWorkingSystemId", params, function (response) {
            if (response.d) {

                var result = eval('(' + response.d + ')');
                workingType = result["WorkingType"];
            }
        });

        return workingType;
    }

    //*************************
    // SetValueTableClosingTime
    //*************************
    function SetValueTableClosingTime(workingSystemId ,entryTimeValue, exitTimeValue)
    {
        var params = { 'workingSystemId': workingSystemId,
                        'entryTimeValue': entryTimeValue,
                        'exitTimeValue': exitTimeValue
                     };

            ajax("GetValueTableClosingTime", params, function (response) {
            if (response.d) {

                var result = eval('(' + response.d + ')');
               
            }
        });
    }

    //**********************
    // Call Search Project
    //**********************
    function callSearchProject(codeID, nameID) {
        getCtrlById("txtProjectNm").attr('readonly','false');

        //2018.01.18 nv-nho search all
        //var projectCD = $('#' + codeID).val();
        //var projectNm = $('#' + nameID).val();
        var projectCD = "";
        var projectNm = "";
        var initDate = getCtrlById("InitDateHidden").val();

        showSearchProject(projectCD, projectNm, initDate, codeID, nameID);
        getCtrlById("CurerrentProjectId").val(codeID);
    };

    //*********************************
    // when cmbAllHolidays change value
    //*********************************
    getCtrlById("cmbAllHolidays").bind("change",function(){
        SetDisplayExchangeDate();
        if(getCtrlById("cmbAllHolidays").val() != -1){
            
            //disable cmbBeforeHalfHoliday, cmbLateHoliday
            getCtrlById("cmbBeforeHalfHoliday").attr('disabled', true);
            getCtrlById("cmbLateHoliday").attr('disabled', true);

            //set value for workingHours and TotalWorkingHours.
            getCtrlById("txtWorkingHours").val("");

            //set value
            ClearEntry_ExitTime();
            LockEntry_ExitTime(1);

            //Clear value
            ClearTableClosingTime();
            LockTableClosingTime(1);

            ClearTableOverTime(1);
            LockTableOverTime(1);


            var vacation = getCtrlById("cmbAllHolidays").val();
            var workingSystemId = getCtrlById("cmbWorkSchedule").val();
            /*if(vacation == "0")
            {

                // get FirstHours
                var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
                var offAfternoon = getCtrlById("cmbLateHoliday").val();
                var params = { 'workingSystemId': workingSystemId,
                                'offMorning':offMorning,
                                'offAfternoon':offAfternoon
                                };
                 ajax("GetWorkingHourByVacation", params, function (response) {
                    if (response.d) {
                        var result = eval('(' + response.d + ')');
                        getCtrlById("txtWorkingHours").val(result["WorkingHours"]);
                        getCtrlById("txtTheTotalWorkingHours").val(result["TotalWorkingHours"]);
                        if(getCtrlById("cmbAllHolidays").val() == 0)
                        {
                            getCtrlById("txtWorkingHours").removeAttr('readonly');
                        }
                    }
                });
            }
            else
            {
                getCtrlById("txtTheTotalWorkingHours").removeAttr('readonly');
                getCtrlById("txtTheTotalWorkingHours").val("");
                getCtrlById("txtTheTotalWorkingHours").attr('readonly','true');
            }*/
            getCtrlById("txtTheTotalWorkingHours").removeAttr('readonly');
            getCtrlById("txtTheTotalWorkingHours").val("");
            getCtrlById("txtTheTotalWorkingHours").attr('readonly','true');
        }
        else
        {
            getCtrlById("cmbBeforeHalfHoliday").attr('disabled', false);
            getCtrlById("cmbLateHoliday").attr('disabled', false);

            //removelock
            LockEntry_ExitTime(0);
            LockTableOverTime(0);

            var workingSystemId = getCtrlById("cmbWorkSchedule").val();
            var workingType = GetWorkingType(workingSystemId);
            if(workingType == "0")
            {
                //remove readonly
                getCtrlById("txtWorkingHours").removeAttr('readonly');
                getCtrlById("txtLate").removeAttr('readonly');
                getCtrlById("txtEarlyHours").removeAttr('readonly');

                //set tabindex
                getCtrlById("txtWorkingHours").enableTab();
                getCtrlById("txtLate").enableTab();
                getCtrlById("txtEarlyHours").enableTab();
            }
            else if(workingType =="1")
            {
                getCtrlById("txtCertainHoliday").removeAttr('readonly');
                getCtrlById("txtCertainHoliday").enableTab();
            }
            else if(workingType == "2")
            {
                getCtrlById("txtLegalHoliday").removeAttr('readonly');
                getCtrlById("txtLegalHoliday").enableTab();
            }
            var workingSystemId = getCtrlById("cmbWorkSchedule").val();
            var workingType = GetWorkingType(workingSystemId);
        
            var startTime = getCtrlById("txtEntryTime").val();
            var leaveTime = ChangeFormat(getCtrlById("txtExitTime").val());

            var offAllDay = getCtrlById("cmbAllHolidays").val();
            var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
            var offAfternoon = getCtrlById("cmbLateHoliday").val();

            //call function to set value for server
            GetAndSetValueForTable(workingSystemId, startTime, leaveTime, offAllDay, offMorning, offAfternoon);
        }
    });

    //****************************************
    // when cmbBeforeHalfHoliday change value
    //****************************************
    getCtrlById("cmbBeforeHalfHoliday").bind("change",function(){

        var workingSystemId = getCtrlById("cmbWorkSchedule").val();
        var workingType = GetWorkingType(workingSystemId);
        var beforeHalfHoliday = getCtrlById("cmbBeforeHalfHoliday").val();
        var lateHoliday = getCtrlById("cmbLateHoliday").val();

        SetDisplayExchangeDate();
        if(getCtrlById("cmbBeforeHalfHoliday").val() != -1){
            getCtrlById("cmbAllHolidays").attr('disabled', true);
            if(getCtrlById("cmbLateHoliday").val() != -1)
            {
                //set value
                ClearTableClosingTime();
                ClearEntry_ExitTime();
                ClearTableOverTime();

                //set readonly
                LockEntry_ExitTime(1);
                LockTableClosingTime(1);
                LockTableOverTime(1);

                getCtrlById("txtTheTotalWorkingHours").removeAttr('readonly');
                getCtrlById("txtTheTotalWorkingHours").val("");
                getCtrlById("txtTheTotalWorkingHours").attr('readonly','true');

                /*if((getCtrlById("cmbBeforeHalfHoliday").val() == 0 && getCtrlById("cmbLateHoliday").val() == 0) ||
                   (getCtrlById("cmbBeforeHalfHoliday").val() != 0 && getCtrlById("cmbLateHoliday").val() == 0) ||
                   (getCtrlById("cmbBeforeHalfHoliday").val() == 0 && getCtrlById("cmbLateHoliday").val() != 0))
                {
                    // get FirstHours
                    var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
                    var offAfternoon = getCtrlById("cmbLateHoliday").val();
                    var params = { 'workingSystemId': workingSystemId,
                                   'offMorning':offMorning,
                                   'offAfternoon':offAfternoon
                                 };
                    ajax("GetWorkingHourByVacation", params, function (response) {
                    if (response.d) {
                    var result = eval('(' + response.d + ')');
                    getCtrlById("txtWorkingHours").val(result["WorkingHours"]);
                    getCtrlById("txtTheTotalWorkingHours").val(result["TotalWorkingHours"]);

                    getCtrlById("txtWorkingHours").removeAttr('readonly');

                        }
                    });
                }*/
                return;
            }
            else
            {
                //ClearEntry_ExitTime();
                ClearTableClosingTime();
                ClearTableOverTime();

                if(workingType == "0")
                {
                    getCtrlById("txtWorkingHours").removeAttr('readonly');
                    getCtrlById("txtLate").removeAttr('readonly');
                    getCtrlById("txtEarlyHours").removeAttr('readonly');

                    // set tabindex
                    getCtrlById("txtWorkingHours").enableTab();
                    getCtrlById("txtLate").enableTab();
                    getCtrlById("txtEarlyHours").enableTab();
                }
                else if(workingType =="1")
                {
                    getCtrlById("txtCertainHoliday").removeAttr('readonly');
                    getCtrlById("txtCertainHoliday").enableTab();
                }
                else if(workingType == "2")
                {
                    getCtrlById("txtLegalHoliday").removeAttr('readonly');
                    getCtrlById("txtLegalHoliday").enableTab();
                }
            }
        }
        else 
        {
            //remove lock
            LockEntry_ExitTime(0);
            LockTableClosingTime(0);
            LockTableOverTime(0);

            if(getCtrlById("cmbLateHoliday").val() != -1)
            {
                getCtrlById("cmbAllHolidays").attr('disabled', true);

                var workingSystemId = getCtrlById("cmbWorkSchedule").val();
                var workingType = GetWorkingType(workingSystemId);
                if(workingType == "0")
                {
                    //remove readonly
                    getCtrlById("txtWorkingHours").removeAttr('readonly');
                    getCtrlById("txtLate").removeAttr('readonly');
                    getCtrlById("txtEarlyHours").removeAttr('readonly');

                    //set tabindex
                    getCtrlById("txtWorkingHours").enableTab();
                    getCtrlById("txtLate").enableTab();
                    getCtrlById("txtEarlyHours").enableTab();
                }
                else if(workingType =="1")
                {
                    getCtrlById("txtCertainHoliday").removeAttr('readonly');
                    getCtrlById("txtCertainHoliday").enableTab();
                }
                else if(workingType == "2")
                {
                    getCtrlById("txtLegalHoliday").removeAttr('readonly');
                    getCtrlById("txtLegalHoliday").enableTab();
                }
            }

            if(getCtrlById("cmbLateHoliday").val() == -1)
            {
                getCtrlById("cmbAllHolidays").attr('disabled', false);
            }  
        }
        // get FirstHours
         var params = { 'workingSystemId': workingSystemId,
                        'beforeHalfHoliday': beforeHalfHoliday,
                        'lateHoliday': lateHoliday
                      };

         ajax("GetValueFirstHour_LatterStart", params, function (response) {
            if (response.d) {
            var result = eval('(' + response.d + ')');
            getCtrlById("txtEntryTime").val(result["WorkingStart"]);
            getCtrlById("txtExitTime").val(result["WorkEnd"]);
            }
        });

        var workingSystemId = getCtrlById("cmbWorkSchedule").val();
        var workingType = GetWorkingType(workingSystemId);
        
        var startTime = getCtrlById("txtEntryTime").val();
        var leaveTime = ChangeFormat(getCtrlById("txtExitTime").val());

        var offAllDay = getCtrlById("cmbAllHolidays").val();
        var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
        var offAfternoon = getCtrlById("cmbLateHoliday").val();

        //call function to set value for server
        GetAndSetValueForTable(workingSystemId, startTime, leaveTime, offAllDay, offMorning, offAfternoon);
        getCtrlById("cmbBeforeHalfHoliday").focus();
    });

    //**********************************
    // when cmbLateHoliday change value
    //**********************************
    getCtrlById("cmbLateHoliday").bind("change",function(){

        var workingSystemId = getCtrlById("cmbWorkSchedule").val();
        var workingType = GetWorkingType(workingSystemId);
        var beforeHalfHoliday = getCtrlById("cmbBeforeHalfHoliday").val();
        var lateHoliday = getCtrlById("cmbLateHoliday").val();

        SetDisplayExchangeDate();
        if(getCtrlById("cmbLateHoliday").val() != -1){
            getCtrlById("cmbAllHolidays").attr('disabled', true);

            if(getCtrlById("cmbBeforeHalfHoliday").val() != -1)
            {
                //set value
                ClearTableClosingTime();
                ClearEntry_ExitTime();
                ClearTableOverTime();

                //set readonly
                LockEntry_ExitTime(1);
                LockTableClosingTime(1);
                LockTableOverTime(1);

                //set readonly
                getCtrlById("txtTheTotalWorkingHours").removeAttr('readonly');
                getCtrlById("txtTheTotalWorkingHours").val("");
                getCtrlById("txtTheTotalWorkingHours").attr('readonly','true');

                /*if((getCtrlById("cmbBeforeHalfHoliday").val() == 0 && getCtrlById("cmbLateHoliday").val() == 0) ||
                   (getCtrlById("cmbBeforeHalfHoliday").val() != 0 && getCtrlById("cmbLateHoliday").val() == 0) ||
                   (getCtrlById("cmbBeforeHalfHoliday").val() == 0 && getCtrlById("cmbLateHoliday").val() != 0))
                {
                    // get FirstHours
                    var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
                    var offAfternoon = getCtrlById("cmbLateHoliday").val();
                    var params = { 'workingSystemId': workingSystemId,
                                   'offMorning':offMorning,
                                   'offAfternoon':offAfternoon
                                 };
                    ajax("GetWorkingHourByVacation", params, function (response) {
                    if (response.d) {
                    var result = eval('(' + response.d + ')');
                    getCtrlById("txtWorkingHours").val(result["WorkingHours"]);
                    getCtrlById("txtTheTotalWorkingHours").val(result["TotalWorkingHours"]);

                    getCtrlById("txtWorkingHours").removeAttr('readonly');

                        }
                    });
                }*/
                return;
            }
            else
            {
                //ClearEntry_ExitTime();
                ClearTableClosingTime();
                ClearTableOverTime();

                if(workingType == "0")
                {
                    //remove readonly
                    getCtrlById("txtWorkingHours").removeAttr('readonly');
                    getCtrlById("txtLate").removeAttr('readonly');
                    getCtrlById("txtEarlyHours").removeAttr('readonly');
                    
                    // set tabindex
                    getCtrlById("txtWorkingHours").enableTab();
                    getCtrlById("txtLate").enableTab();
                    getCtrlById("txtEarlyHours").enableTab();
                }
                else if(workingType =="1")
                {
                    getCtrlById("txtCertainHoliday").removeAttr('readonly');
                    getCtrlById("txtCertainHoliday").enableTab();
                }
                else if(workingType == "2")
                {
                    getCtrlById("txtLegalHoliday").removeAttr('readonly');
                    getCtrlById("txtLegalHoliday").enableTab();
                }
            }
        }
        else 
        {
            //remove lock.
            LockEntry_ExitTime(0);
            LockTableClosingTime(0);
            LockTableOverTime(0);
            if(getCtrlById("cmbBeforeHalfHoliday").val() != -1)
            {
                getCtrlById("cmbAllHolidays").attr('disabled', true);

                var workingSystemId = getCtrlById("cmbWorkSchedule").val();
                var workingType = GetWorkingType(workingSystemId);
                if(workingType == "0")
                {
                    getCtrlById("txtWorkingHours").removeAttr('readonly');
                    getCtrlById("txtLate").removeAttr('readonly');
                    getCtrlById("txtEarlyHours").removeAttr('readonly');

                    //set tabindex
                    getCtrlById("txtWorkingHours").enableTab();
                    getCtrlById("txtLate").enableTab();
                    getCtrlById("txtEarlyHours").enableTab();
                }
                else if(workingType =="1")
                {
                    getCtrlById("txtCertainHoliday").removeAttr('readonly');
                    getCtrlById("txtCertainHoliday").enableTab();
                }
                else if(workingType == "2")
                {
                    getCtrlById("txtLegalHoliday").removeAttr('readonly');
                    getCtrlById("txtLegalHoliday").enableTab();
                }
            }

            if(getCtrlById("cmbBeforeHalfHoliday").val() == -1)
            {
                getCtrlById("cmbAllHolidays").attr('disabled', false);
            }
        }

         // get FirstHours
         var params = { 'workingSystemId': workingSystemId,
                        'beforeHalfHoliday': beforeHalfHoliday,
                        'lateHoliday': lateHoliday
                      };

         ajax("GetValueFirstHour_LatterStart", params, function (response) {
            if (response.d) {
            var result = eval('(' + response.d + ')');
            getCtrlById("txtEntryTime").val(result["WorkingStart"]);
            getCtrlById("txtExitTime").val(result["WorkEnd"]);
            }
        });

        var workingSystemId = getCtrlById("cmbWorkSchedule").val();
        var workingType = GetWorkingType(workingSystemId);
        
        var startTime = getCtrlById("txtEntryTime").val();
        var leaveTime = ChangeFormat(getCtrlById("txtExitTime").val());

        var offAllDay = getCtrlById("cmbAllHolidays").val();
        var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
        var offAfternoon = getCtrlById("cmbLateHoliday").val();

        //call function to set value for server
        GetAndSetValueForTable(workingSystemId, startTime, leaveTime, offAllDay, offMorning, offAfternoon);

        //call function to set value for server
        GetAndSetValueForTable(workingSystemId, startTime, leaveTime, offAllDay, offMorning, offAfternoon);

        getCtrlById("cmbLateHoliday").focus();
    });

    getCtrlById("txtWorkingHours").bind("change",function(){
        setTotalWorkingHours(this.value);
    });
    getCtrlById("txtLate").bind("change",function(){
        setTotalWorkingHours(this.value);
    });    
    getCtrlById("txtEarlyHours").bind("change",function(){
        setTotalWorkingHours(this.value);
    });
    getCtrlById("txtCertainHoliday").bind("change",function(){
        setTotalWorkingHours(this.value);
    });
    getCtrlById("txtLegalHoliday").bind("change",function(){
        setTotalWorkingHours(this.value);
    });
    
    //***************************
    // when overtime change value
    //***************************
    $(".overtime").bind("change",function(){
        var value = this.value;
        var totalOverTime = 0;
        $('.overtime').each(function(){
            if($(this).val() != "")
            {
                if($(this).val() == value)
                {
                    value = ChangeFormat(value);
                    totalOverTime = totalOverTime + TimeToMinute(value);
                    
                }
                else 
                {
                    totalOverTime += TimeToMinute($(this).val());
                }
            }
        });

        var totalWorkingHours = 0;
        var workingId = getCtrlById("cmbWorkSchedule").val();
        var workingType = GetWorkingType(workingId);

        if(workingType == "0")
        {
            totalWorkingHours = TimeToMinute(getCtrlById("txtWorkingHours").val()) 
                                + totalOverTime;
        }
        else 
        {
            if(workingType ==  "1") 
            {
                totalWorkingHours = TimeToMinute(getCtrlById("txtCertainHoliday").val()) 
                                    + totalOverTime;
            }
            else if (workingType ==  "2") 
            {
                totalWorkingHours = TimeToMinute(getCtrlById("txtLegalHoliday").val()) 
                                    + totalOverTime;
            }
        }
        if(totalWorkingHours < 0)
        {
            totalWorkingHours = 0;
        }
        getCtrlById("txtTheTotalWorkingHours").removeAttr('readonly');
        getCtrlById("txtTheTotalWorkingHours").val(MinuteToTime(totalWorkingHours));
        getCtrlById("txtTheTotalWorkingHours").attr('readonly','true');

        getCtrlById("txtTheTotalOvertime").removeAttr('readonly');
        getCtrlById("txtTheTotalOvertime").val(MinuteToTime(totalOverTime));
        getCtrlById("txtTheTotalOvertime").attr('readonly','true');
        
    });

    //********************
    //CalTotalWorkingHours
    //********************
    function setTotalWorkingHours(value)
    {
        
        var totalWorkingHours = 0;
        var totalOverTime = 0;
        $('.overtime').each(function(){
            if($(this).val() != "")
            {
                totalOverTime += TimeToMinute($(this).val());
            }
        });
        if(getCtrlById("txtWorkingHours").val() == value)
        {
            getCtrlById("txtWorkingHours").val(ChangeFormat(value))
        }
        else if(getCtrlById("txtLate").val() == value)
        {
            getCtrlById("txtLate").val(ChangeFormat(value))
        }
        else if(getCtrlById("txtEarlyHours").val() == value)
        {
            getCtrlById("txtEarlyHours").val(ChangeFormat(value))
        }
        else if(getCtrlById("txtCertainHoliday").val() == value)
        {
            getCtrlById("txtCertainHoliday").val(ChangeFormat(value))
        }
        else if(getCtrlById("txtLegalHoliday").val() == value)
        {
            getCtrlById("txtLegalHoliday").val(ChangeFormat(value))
        }

        var workingId = getCtrlById("cmbWorkSchedule").val();
        var workingType = GetWorkingType(workingId);

        if(workingType == "0")
        {
            totalWorkingHours = TimeToMinute(getCtrlById("txtWorkingHours").val()) + totalOverTime;
        }
        else 
        {
            if(workingType == "1")
            {
                totalWorkingHours = TimeToMinute(getCtrlById("txtCertainHoliday").val()) + totalOverTime;
            }
            else if(workingType == "2")
            {
                totalWorkingHours = TimeToMinute(getCtrlById("txtLegalHoliday").val()) + totalOverTime;
            }
        }

        if(totalWorkingHours < 0)
        {
            totalWorkingHours = 0;
        }

        getCtrlById("txtTheTotalWorkingHours").removeAttr('readonly');
        getCtrlById("txtTheTotalWorkingHours").val(MinuteToTime(totalWorkingHours));
        getCtrlById("txtTheTotalWorkingHours").attr('readonly','true');
    }

    //*********************
    //ChangeFormat
    //*********************
    function ChangeFormat(d)
    {
        var value = d.trim();
        value = WorkTimeFormatCommon(value);
        if (!isTime(value)) {
            return '';
        } else {
            d = WorkTimeFormatCommon(value);
            return value;
        }
    }

    //**********************
    // CheckToLockCombobox
    //**********************    
    function CheckToLockCombobox()
    {
        if(getCtrlById("cmbAllHolidays").val() != -1){
            getCtrlById("cmbBeforeHalfHoliday").attr('disabled', true);
            getCtrlById("cmbLateHoliday").attr('disabled', true);
            LockEntry_ExitTime(1);
            LockTableClosingTime(1);
            LockTableOverTime(1);
            /*if(getCtrlById("cmbAllHolidays").val() == 0)
            {
                getCtrlById("txtWorkingHours").removeAttr('readonly');
            }*/
        }

        else if(getCtrlById("cmbBeforeHalfHoliday").val() != -1 || getCtrlById("cmbLateHoliday").val() != -1){
            getCtrlById("cmbAllHolidays").attr('disabled', true);
            if(getCtrlById("cmbBeforeHalfHoliday").val() != -1 && getCtrlById("cmbLateHoliday").val() != -1)
            {
                LockEntry_ExitTime(1);
                LockTableClosingTime(1);
                LockTableOverTime(1);
            }

            if((getCtrlById("cmbBeforeHalfHoliday").val() == "0") && (getCtrlById("cmbLateHoliday").val() == "0"))
            {
                getCtrlById("txtWorkingHours").removeAttr('readonly');
            }
        }
    }

    //**********************
    // CheckToLockWorkHour
    //**********************
    function CheckToLockWorkHour()
    {
        if(getCtrlById("cmbAllHolidays").val() != -1){
            getCtrlById("cmbBeforeHalfHoliday").attr('disabled', true);
            getCtrlById("cmbLateHoliday").attr('disabled', true);
        }

        else if(getCtrlById("cmbBeforeHalfHoliday").val() != -1 || getCtrlById("cmbLateHoliday").val() != -1){
            getCtrlById("cmbAllHolidays").attr('disabled', true);
            
        }
    }

    //*****************************
    // CheckLockTextboxFromComboboxOff
    //*****************************
    function CheckLockTextboxFromComboboxOff()
    {
        if(getCtrlById("cmbAllHolidays").val() != -1){
            LockEntry_ExitTime(1);
            LockTableClosingTime(1);
            LockTableOverTime(1);
        }
        else
        {
            if(getCtrlById("cmbBeforeHalfHoliday").val() != -1 && getCtrlById("cmbLateHoliday").val() != -1)
            {
                LockEntry_ExitTime(1);
                LockTableClosingTime(1);
                LockTableOverTime(1);
            }
            else 
            {
                LockEntry_ExitTime(0);
                LockTableClosingTime(0);
                LockTableOverTime(0);
            }
        }
    }

    //**********************
    // Find Back
    //**********************    
    function findBack() {
        hideLoading();
        
        ShowProjectInfo(getCtrlById("CurerrentProjectId").val());
        
        getCtrlById("txtProjectNm").attr('readonly','true');
        getCtrlById("txtProjectCD").focus().select();
    }

    //**********************
    // Project Changed
    //**********************
    function projectCDChange(sender) {
        var codeID = sender.id;
        var nameID = codeID.replace('MainContent_rptDetail_txtProjectCD','MainContent_rptDetail_txtProjectNm');
        var workPlaceID = codeID.replace('MainContent_rptDetail_txtProjectCD','MainContent_rptDetail_txtWorkPlace');
        var projectCdValue = sender.value;
        var params = { 'projectCD': projectCdValue};

         ajax("GetProjectName", params, function (response) {
            if (response.d) {
            var result = eval('(' + response.d + ')');
            $('#' + codeID).val(result["projectCd"]);
            $('#' + nameID).val(result["ProjectName"]);
            $('#' + workPlaceID).val(result["WorkPlace"]);
            }
        });
    }

    //**********************
    // Show ProjectName, WorkPlace
    //**********************
    function ShowProjectInfo(id) {
        var codeID = id;
        var nameID = codeID.replace('MainContent_rptDetail_txtProjectCD','MainContent_rptDetail_txtProjectNm');
        var workPlaceID = codeID.replace('MainContent_rptDetail_txtProjectCD','MainContent_rptDetail_txtWorkPlace');
        var projectCdValue = $('#' + codeID).val();
        var params = { 'projectCD': projectCdValue};

         ajax("GetProjectName", params, function (response) {
            if (response.d) {
            var result = eval('(' + response.d + ')');
            $('#' + codeID).val(result["projectCd"]);
            $('#' + nameID).val(result["ProjectName"]);
            $('#' + workPlaceID).val(result["WorkPlace"]);
            }
        });
        if(codeID == "MainContent_rptDetail_txtProjectCD_0")
        {
            var rowCount = $('#tableDetail tr').length;

            //check row in repeater
            var checkData = true; 
            for(var i = 1;i< rowCount -1 ; i++ )
            {
                if(getCtrlById("MainContent_rptDetail_txtStartTime_" + i).val() != "")
                {
                    checkData = false;
                } 
            }
            var entryTime = getCtrlById("txtEntryTime").val();
            var exitTime = getCtrlById("txtExitTime").val();
            var workTime = getCtrlById("txtTheTotalWorkingHours").val();
            if(entryTime != "" && exitTime !="")
            {
                if(getCtrlById("MainContent_rptDetail_txtStartTime_0").val() == "" 
                   && getCtrlById("MainContent_rptDetail_txtEndTime_0").val() == "" 
                   && getCtrlById("MainContent_rptDetail_txtWorkTime_0").val() ==""
                   && checkData == true)
                {
                    getCtrlById("MainContent_rptDetail_txtStartTime_0").val(entryTime);
                    getCtrlById("MainContent_rptDetail_txtEndTime_0").val(exitTime);
                    getCtrlById("MainContent_rptDetail_txtWorkTime_0").val(workTime);
                }
            }
        }
    }

    //**********************
    // StartTime Change
    //**********************
    function StartTimeChange(sender){
        var startTimeId = sender.id;
        var endTimeId = startTimeId.replace('MainContent_rptDetail_txtStartTime','MainContent_rptDetail_txtEndTime');
        var workingTimeId = startTimeId.replace('MainContent_rptDetail_txtStartTime','MainContent_rptDetail_txtWorkTime');
        var startTimeValue = sender.value; 
        var endTimeValue = $('#' + endTimeId).val();
        
        var workingTimeProjectValue = 0;
        // define parameter for ajax
        var workingSystemId = getCtrlById("cmbWorkSchedule").val();
        var startTime = startTimeValue;            
        var leaveTime = endTimeValue;
        var offAllDay = getCtrlById("cmbAllHolidays").val();
        var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
        var offAfternoon = getCtrlById("cmbLateHoliday").val();
        var params1 = { 'workingSystemId': workingSystemId,
                        'entryTimeValue': startTime,
                        'exitTimeValue': leaveTime,
                        'offAllDay': offAllDay,
                        'offMorning': offMorning,
                        'offAfternoon': offAfternoon
                      };

        //call function to set value for server
        if(startTime != "" && leaveTime != "")
        {
            if(TimeToMinute(startTime) < TimeToMinute(leaveTime))
            {
                ajax("GetWorkingTimeProjectValue", params1, function (response) {
                if (response.d) 
                {
                    var result1 = eval('(' + response.d + ')');
                    workingTimeProjectValue = TimeToMinute(result1["workingTimeProject"]);
                    if(workingTimeProjectValue  > 0)
                    {
                        $('#'+workingTimeId).val(MinuteToTime(workingTimeProjectValue));
                    }
                    else 
                    {
                        $('#'+workingTimeId).val("");
                    }
                   }
                });
            }
            else
            {
                $('#'+workingTimeId).val("");
            }
        }        
    }

    //**********************
    // EndTime Change
    //**********************
    function EndTimeChange(sender){
        var id = sender.id;
        var endTimeId = sender.id;
        var startTimeId = endTimeId.replace('MainContent_rptDetail_txtEndTime','MainContent_rptDetail_txtStartTime');
        var workingTimeId = endTimeId.replace('MainContent_rptDetail_txtEndTime','MainContent_rptDetail_txtWorkTime');
        var endTimeValue = sender.value; 
        var startTimeValue = $('#' + startTimeId).val();

        var workingTimeProjectValue = 0;
        // define parameter for ajax
        var workingSystemId = getCtrlById("cmbWorkSchedule").val();
        var startTime = startTimeValue;            
        var leaveTime = endTimeValue;
        var offAllDay = getCtrlById("cmbAllHolidays").val();
        var offMorning = getCtrlById("cmbBeforeHalfHoliday").val();
        var offAfternoon = getCtrlById("cmbLateHoliday").val();

        var params1 = { 'workingSystemId': workingSystemId,
                        'entryTimeValue': startTime,
                        'exitTimeValue': leaveTime,
                        'offAllDay': offAllDay,
                        'offMorning': offMorning,
                        'offAfternoon': offAfternoon
                      };

        //call function to set value for server
        if(startTime != "" && leaveTime != "")
        {
            if(TimeToMinute(startTime) < TimeToMinute(leaveTime))
            {
                ajax("GetWorkingTimeProjectValue", params1, function (response) {
                if (response.d) 
                {
                    var result1 = eval('(' + response.d + ')');
                    workingTimeProjectValue = TimeToMinute(result1["workingTimeProject"]);
                    if(workingTimeProjectValue  > 0)
                    {
                        $('#'+workingTimeId).val(MinuteToTime(workingTimeProjectValue));
                    }
                    else 
                    {
                        $('#'+workingTimeId).val("");
                    }
                   }
                });
            }
            else
            {
                $('#'+workingTimeId).val("");
            }
        }      
    }
    
    //**********************
    // Init detail list
    //**********************
    function initDetailList(){
        var detailCtrl = $(".value");
        <%if(this.Mode == Mode.View || this.Mode == Mode.Delete){ %>
            detailCtrl.attr("readonly","true");
            detailCtrl.disableTab();
        <%} else{%>
            detailCtrl.removeAttr("readonly");
            detailCtrl.enableTab();
        <%} %>
    }

    //************************************
    // LockHolidayWorkingTimeByWorkingType
    //************************************
    function LockHolidayWorkingTimeByWorkingType(workingId)
    {
        var workingType = GetWorkingType(workingId);
        if(workingType == "0")
        {
            getCtrlById("txtCertainHoliday").val("");
            getCtrlById("txtLegalHoliday").val("");
                
            // set readonly
            getCtrlById("txtCertainHoliday").attr('readonly','true');
            getCtrlById("txtLegalHoliday").attr('readonly','true'); 

            // set tabindex.
            getCtrlById("txtCertainHoliday").disableTab();
            getCtrlById("txtLegalHoliday").disableTab();

        }
        else if(workingType == "1")
        {
            getCtrlById("txtWorkingHours").val("");
            getCtrlById("txtLegalHoliday").val("");
            getCtrlById("txtLate").val("");
            getCtrlById("txtEarlyHours").val("");

            // set readonly.
            getCtrlById("txtWorkingHours").attr('readonly','true');
            getCtrlById("txtLegalHoliday").attr('readonly','true');
            getCtrlById("txtLate").attr('readonly','true');
            getCtrlById("txtEarlyHours").attr('readonly','true');

            // set tabindex.
            getCtrlById("txtWorkingHours").disableTab();
            getCtrlById("txtLegalHoliday").disableTab();
            getCtrlById("txtLate").disableTab();
            getCtrlById("txtEarlyHours").disableTab();


            // Lock comboboxOffs
            lockComboboxOffs(1)
        }
        else if(workingType == "2")
        {
            getCtrlById("txtWorkingHours").val("");
            getCtrlById("txtCertainHoliday").val("");
            getCtrlById("txtLate").val("");
            getCtrlById("txtEarlyHours").val("");

            //set readonly.
            getCtrlById("txtWorkingHours").attr('readonly','true');
            getCtrlById("txtCertainHoliday").attr('readonly','true');
            getCtrlById("txtLate").attr('readonly','true');
            getCtrlById("txtEarlyHours").attr('readonly','true');

            //set tabindex.
            getCtrlById("txtWorkingHours").disableTab();
            getCtrlById("txtCertainHoliday").disableTab();
            getCtrlById("txtLate").disableTab();
            getCtrlById("txtEarlyHours").disableTab();

            // Lock comboboxOffs.
            lockComboboxOffs(1)
        }

        SetDisplayExchangeFlag(workingType);
    }

    //*****************
    // LockControlText
    //*****************
    function LockControlText()
    {
        getCtrlById("txtTheTotalOvertime").attr("readonly","true");
        getCtrlById("txtTheTotalWorkingHours").attr("readonly","true");
        getCtrlById("txtTheTotalOvertime").disableTab();
        getCtrlById("txtTheTotalWorkingHours").disableTab();
        $(".projectNm").attr("readonly","true");
    }

    //**************
    // Lock Control
    //**************
    function lock(status){
            //Text input
            $("input[type != 'hidden'], textarea").each(function(i, ctr){
                $(ctr).prop("readonly", status == 1);
            });
            //Select, checkbox, button
            $("select, input[type = 'checkbox'], input[type = 'radio'] , button[data-toggle != 'collapse']:not('.Fbtn, .close')").each(function(i, ctr){
                $(ctr).prop("disabled", status == 1);
            });
            <%  if (this.Mode == Mode.Approval)
                {
            %>
                getCtrlById("txtApprovalNote").attr("readonly", false);
            <%
                } 
            %>

            <%  if (this.Mode == Mode.Request)
                {
            %>
            getCtrlById("txtRequestNote").attr("readonly", false);
            <%
                } 
            %>
        }
    //***************
    // Lock Combobox
    //***************
    function lockComboboxOffs(status){
    
        getCtrlById("cmbAllHolidays").attr('disabled', status == 1);
        getCtrlById("cmbBeforeHalfHoliday").attr('disabled', status == 1);
        getCtrlById("cmbLateHoliday").attr('disabled', status == 1);
    }

    //*********************
    // Clear Entry_ExitHours
    //*********************
    function ClearEntry_ExitTime()
    {
        getCtrlById("txtEntryTime").val("");
        getCtrlById("txtExitTime").val("");
    }

    //*********************
    // Lock Entry_ExitHours
    //*********************
    function LockEntry_ExitTime(status)
    {
        getCtrlById("txtEntryTime").prop('readonly',status==1);
        getCtrlById("txtExitTime").prop('readonly',status==1);

        if(status ==1)
        {
            getCtrlById("txtEntryTime").disableTab();
            getCtrlById("txtExitTime").disableTab();
        }
        else
        {
            getCtrlById("txtEntryTime").enableTab();
            getCtrlById("txtExitTime").enableTab();
        }
        
    }

    //***********************
    // Clear TableClosingTime
    //***********************
    function ClearTableClosingTime()
    {
        getCtrlById("txtWorkingHours").val("");
        getCtrlById("txtLate").val("");
        getCtrlById("txtEarlyHours").val("");
        getCtrlById("txtCertainHoliday").val("");
        getCtrlById("txtLegalHoliday").val("");
    }

    //**********************
    // Lock TableClosingTime
    //**********************
    function LockTableClosingTime(status)
    {
        getCtrlById("txtWorkingHours").prop('readonly',status==1);
        getCtrlById("txtLate").prop('readonly',status==1);
        getCtrlById("txtEarlyHours").prop('readonly',status==1);
        getCtrlById("txtCertainHoliday").prop('readonly',status==1);
        getCtrlById("txtLegalHoliday").prop('readonly',status==1);

        if(status ==1)
        {
            getCtrlById("txtWorkingHours").disableTab();
            getCtrlById("txtLate").disableTab();
            getCtrlById("txtEarlyHours").disableTab();
            getCtrlById("txtCertainHoliday").disableTab();
            getCtrlById("txtLegalHoliday").disableTab();
        }
        else
        {
            getCtrlById("txtWorkingHours").enableTab();
            getCtrlById("txtLate").enableTab();
            getCtrlById("txtEarlyHours").enableTab();
            getCtrlById("txtCertainHoliday").enableTab();
            getCtrlById("txtLegalHoliday").enableTab();
        }

    }

    //*******************
    // Lock TableOverTime
    //*******************
    function LockTableOverTime(status)
    {
        $("#MainContent_rpttbConfig4_Data_txtOverTime_0").prop('readonly',status==1);
        $("#MainContent_rpttbConfig4_Data_txtOverTime_1").prop('readonly',status==1);
        $("#MainContent_rpttbConfig4_Data_txtOverTime_2").prop('readonly',status==1);
        $("#MainContent_rpttbConfig4_Data_txtOverTime_3").prop('readonly',status==1);
        $("#MainContent_rpttbConfig4_Data_txtOverTime_4").prop('readonly',status==1);

        if(status == 1)
        {
            $("#MainContent_rpttbConfig4_Data_txtOverTime_0").disableTab();
            $("#MainContent_rpttbConfig4_Data_txtOverTime_1").disableTab();
            $("#MainContent_rpttbConfig4_Data_txtOverTime_2").disableTab();
            $("#MainContent_rpttbConfig4_Data_txtOverTime_3").disableTab();
            $("#MainContent_rpttbConfig4_Data_txtOverTime_4").disableTab();
        }
        else
        {
            $("#MainContent_rpttbConfig4_Data_txtOverTime_0").enableTab();
            $("#MainContent_rpttbConfig4_Data_txtOverTime_1").enableTab();
            $("#MainContent_rpttbConfig4_Data_txtOverTime_2").enableTab();
            $("#MainContent_rpttbConfig4_Data_txtOverTime_3").enableTab();
            $("#MainContent_rpttbConfig4_Data_txtOverTime_4").enableTab();
        }
    }

    //*******************
    // Clear TableOverTime
    //*******************
    function ClearTableOverTime()
    {
        $("#MainContent_rpttbConfig4_Data_txtOverTime_0").val("");
        $("#MainContent_rpttbConfig4_Data_txtOverTime_1").val("");
        $("#MainContent_rpttbConfig4_Data_txtOverTime_2").val("");
        $("#MainContent_rpttbConfig4_Data_txtOverTime_3").val("");
        $("#MainContent_rpttbConfig4_Data_txtOverTime_4").val("");
        getCtrlById("txtTheTotalOvertime").val("");
        getCtrlById("txtTheTotalWorkingHours").val("");
    }

    //**********************
    // Set Focus
    //**********************
    function setFocus() {

        <%if(this.Mode==Mode.Update || this.Mode == Mode.Insert){ %>
            if (getCtrlById("txtEntryTime").is('[readonly]')) 
            {
                var workingSystemId = getCtrlById("cmbWorkSchedule").val();
                var params = { 'workingSystemId': workingSystemId};

                ajax("GetValueByWorkingSystemId", params, function (response) {

                    if (response.d) {
                        var result = eval('(' + response.d + ')');
                        var workingCd = result["WorkingCd"];
                        if(workingCd == "4")
                        {
                            getCtrlById("cmbAllHolidays").focus();
                        }
                        else
                        {
                            getCtrlById("cmbWorkSchedule").focus();
                        }
                    }
                });
            }
            else
            {
                getCtrlById("txtEntryTime").focus().select();
            }
        <%} %>            
    }

    $.prototype.disableTab = function() {
    this.each(function() {
            $(this).attr('tabindex', '-1');
        });
    };
    $.prototype.enableTab = function() {
    this.each(function() {
            $(this).removeAttr("tabindex");
        });
    };

    function showmodalVerification() {

        $('#modalVerification').modal('show');
    }

    function SetDisplayExchangeFlag(workingType) {
        if (workingType == "0")
        {
            $(getCtrlById("chkExchangeFlag")).prop('checked', false);
            $(".divChkExchangeFlag").addClass("hidden");
        }
        else
        {
            $(".divChkExchangeFlag").removeClass("hidden");
        }
    }

    function SetDisplayExchangeDate() {
        if (getCtrlById("cmbAllHolidays").val() == "1" || getCtrlById("cmbBeforeHalfHoliday").val() == "1" || getCtrlById("cmbLateHoliday").val() == "1") {
            $(".divCmbExchangeDate").removeClass("hidden");
        }
        else
        {
            $(".divCmbExchangeDate").addClass("hidden");
            getCtrlById("cmbExchangeDate").val("-1")
        }
    }
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <div class="container well-sm" style="<%= GetApprovalStatus("dislay") %>">
        <div class="row">
            <label id="lblMode" class="<%= GetApprovalStatus("class")%>" style="font-size: 14px;"><%= GetApprovalStatus("text")%></label>
        </div>
    </div>

<%-- Search Condition--%>
    <div class="container well-sm">
        <div class="row">
            <%--Date--%>
            <div class="col-md-3 col-sm-3">
                <div class='form-group <% =GetClassError("txtDate")%>'>
                    <label class="control-label" for="<%= txtDate.ClientID %>">
                        日付</label>
                    <cc1:ITextBox ID="txtDate" runat="server" ReadOnly="true" CssClass="form-control input-sm"></cc1:ITextBox>
                </div>
            </div>

            <%--Employee Name--%>
            <div class="col-md-4 col-sm-4">
                <div class="form-group">
                    <label class="control-label" for="<%= txtEmployeeName.ClientID %>">
                        社員名</label>
                    <cc1:ITextBox ID="txtEmployeeName" runat="server" ReadOnly="true" CssClass="form-control input-sm"></cc1:ITextBox>
                </div>
            </div>
        </div>
    </div>
     <div class="well well-sm">
        <div class="row">
            <%--Work schedule--%>
            <div class="col-md-3 col-sm-7">
                <div class="form-group">
                    <label class="control-label">
                        勤務体系<strong class="text-danger">*</strong></label>
                    <asp:DropDownList ID="cmbWorkSchedule" runat="server" CssClass="form-control input-sm cmbWorkingSystem"></asp:DropDownList>
                </div>
            </div>

            <%--Attendance--%>
            <div class="col-md-2 col-sm-7">
                <div class="col-md-10">
                    <div class='form-group <% =GetClassError("txtEntryTime")%>'>
                        <label class="control-label" for="<%= txtEntryTime.ClientID %>">
                            出勤<strong class="text-danger">*</strong></label>
                            <cc1:ITimeTextBox ID="txtEntryTime" CodeType="Numeric" FillChar="0" CssClass="form-control input-sm text-center"  runat="server" MaxLength="5" />
                    </div>   
                </div>
            </div>

            <%--Leave--%>
            <div class="col-md-2 col-sm-7">
                <div class="col-md-10">
                    <div class='form-group <% =GetClassError("txtExitTime")%>'>
                        <label class="control-label" for="<%= txtExitTime.ClientID %>">
                            退出<strong class="text-danger">*</strong></label>                      
                        <cc1:ITimeTextBox ID="txtExitTime" CodeType="Numeric" FillChar="0" CssClass="form-control input-sm text-center"  runat="server" MaxLength="5" />
                    </div>
               </div>
            </div>
            <%--振替休暇を取得する--%>
            <div class="col-md-3 col-sm-7 divChkExchangeFlag">
                <br />
                <div class="col-md-12">
                    <div class="checkbox">
                        <label><input id="chkExchangeFlag" runat="server" type="checkbox"/>振替休暇を取得する</label>
                    </div>                   
               </div>
            </div>
            <div class="col-md-2 col-sm-7 divCmbExchangeDate">
                <div class='form-group <% =GetClassError("cmbExchangeDate")%>'>
                    <label class="control-label">
                        振休対象日<strong class="text-danger">*</strong></label>
                    <asp:DropDownList ID="cmbExchangeDate" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                </div>
            </div>
        </div>
    </div>

    <div class="well well-sm">
        <div class="row">
            <%--All Holiday--%>
            <div class="col-md-3 col-sm-3">
                <div class="form-group">
                    <label class="control-label">
                        全休</label>
                    <asp:DropDownList ID="cmbAllHolidays" runat="server" CssClass="form-control input-sm cmbWorkingSystem"></asp:DropDownList>
                </div>
            </div>

            <%--Before Half Holiday--%>
            <div class="col-md-3 col-sm-3">
                <div class="form-group">
                    <label class="control-label">
                        前半休</label>
                    <asp:DropDownList ID="cmbBeforeHalfHoliday" runat="server" CssClass="form-control input-sm cmbWorkingSystem"></asp:DropDownList>
                </div>
            </div>

            <%--Product Id--%>
            <div class="col-md-3 col-sm-3">
                <div class="form-group">
                    <label class="control-label">
                        後半休</label>
                    <asp:DropDownList ID="cmbLateHoliday" runat="server" CssClass="form-control input-sm cmbWorkingSystem"></asp:DropDownList>
                </div>
            </div>
            <%if (this.Mode != Mode.View && this.Mode != Mode.Delete)
            { %>
                <%--VacationDay--%>
                <div class="col-md-1 col-sm-7">
                    <div class='form-group'>
                        <label class="control-label">
                            有給残数</label>
                        <cc1:ITextBox ID="txtVacationDay" runat="server" ReadOnly="true" CssClass="form-control input-sm text-center"></cc1:ITextBox>
                    </div>
                </div>

                <%--Verification--%>
                <div class="col-md-1 col-sm-7">
                    <div class='form-group'>
                        <button id="btnVerification" type="button" style="text-align: center; vertical-align: middle; line-height: 26px; width: 75px; margin-top: 25px;" class="btn btn-default btn-xs" onclick="showmodalVerification();">確認</button>
                    </div>
                </div>
            <%} %>
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

    <div class="container well-sm">
        <div class="row">
            <%--Product Id--%>
            <div class="col-md-8 col-sm-10">
                <div class="form-group scrollmenu" style="overflow: auto;white-space: nowrap;">
                    <label class="control-label">
                        就業時間</label>

                    <table class="table table-bordered" style="width: 540px;">
                        <thead class ="table-header">
                        <tr>
                            <th class ="text-center">出勤</th>
                            <th class ="text-center">遅刻</th>
                            <th class ="text-center">早退</th>
                            <th class ="text-center">所定休日</th>
                            <th class ="text-center">法定休日</th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr style="text-align: center">
                            <td class = "col-md-2">   
                                <cc1:ITimeTextBox ID="txtWorkingHours" CssClass="form-control input-sm text-center" runat="server" MaxLength="5"/>
                            </td>
                            <td class = "col-md-2">
                                <cc1:ITimeTextBox ID="txtLate" CssClass="form-control input-sm text-center" runat="server" MaxLength="5"/>    
                            </td>
                            <td class = "col-md-2">
                                <cc1:ITimeTextBox ID="txtEarlyHours" CssClass="form-control input-sm text-center" runat="server" MaxLength="5"/>    
                            </td>
                            <td class = "col-md-2">
                                <cc1:ITimeTextBox ID="txtCertainHoliday" CssClass="form-control input-sm text-center" runat="server" MaxLength="5"/>    
                            </td>
                            <td class = "col-md-2">
                                <cc1:ITimeTextBox ID="txtLegalHoliday" CssClass="form-control input-sm text-center" runat="server" MaxLength="5"/>    
                            </td>
                        </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
   </div>
   
   <div class="container well-sm">
        <div class="row">
            <%--Product Id--%>
            <div class="col-md-10">
                <div class="form-group scrollmenu" style="overflow: auto;white-space: nowrap;">
                    <label class="control-label">
                        残業時間</label>

                    <table class="table table-bordered" style="width: 750px;">
                        <thead class ="table-header">
                        <tr>
                            <asp:Repeater ID="rpttbConfig4_Header" runat="server">
                                <ItemTemplate>
                                    <%# Eval("Header")%>
                                </ItemTemplate>
                            </asp:Repeater>
                            <th class ="text-center">総残業時間</th>
                            <th class ="text-center">総労働時間</th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr style="text-align: center">
                            <asp:Repeater ID="rpttbConfig4_Data" runat="server">
                                <ItemTemplate>
                                    <td>   
                                        <cc1:ITimeTextBox ID="txtOverTime" Value='<%# Eval("Data") %>' CssClass="overtime form-control input-sm text-center" runat="server" MaxLength="5"></cc1:ITimeTextBox>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td style="width: 14%;">
                                <cc1:ITimeTextBox ID="txtTheTotalOvertime" CssClass="form-control input-sm text-center"  runat="server" MaxLength="5" />                         
                            </td>
                            <td style="width: 14%;">
                                <cc1:ITimeTextBox ID="txtTheTotalWorkingHours" CssClass="form-control input-sm text-center" runat="server" MaxLength="5" />  
                            </td>
                        </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
   </div>

   <%if (this.Mode != Mode.View && this.Mode != Mode.Delete)
           { %>
        <div class="well well-sm">
            <div class="row">
                <div class="col-md-4 col-sm-6 col-xs-12">
                    <div class="btn-group btn-group-justified">
                        <div class="btn-group">
                            <asp:LinkButton ID="btnDeleteRow" runat="server" CommandName="Add" CssClass="btn btn-default btn-xs loading" OnClick="btnRemoveRow_Click">
                                    <span class="glyphicon glyphicon-remove"></span>&nbsp;行削除
                            </asp:LinkButton>
                        </div>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnAddRow" runat="server" CommandName="Add" CssClass="btn btn-default btn-xs loading" OnClick="btnAddRow_Click">
                                    <span class="glyphicon glyphicon-plus"></span>&nbsp;行追加
                            </asp:LinkButton>
                        </div>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnUpTop" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnUp_Click">
                                <span class="glyphicon glyphicon-arrow-up"></span>&nbsp;上へ
                            </asp:LinkButton>
                        </div>                           
                        <div class="btn-group">
                            <asp:LinkButton ID="btnDownTop" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnDown_Click">
                                <span class="glyphicon glyphicon-arrow-down"></span>&nbsp;下へ
                            </asp:LinkButton>
                        </div>
                    </div>

                </div>
                <div class ="col-md-8 col-sm-7 col-xs-12">
                    <div class="form-group">
                        <div class ="col-md-3 col-sm-7 col-xs-6" style="padding-left:0px;">
                            <div class="input-group date">
                                <cc1:IDateTextBox ID="dtCopyDate" runat="server" CssClass="form-control input-sm" PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                </span>
                            </div>
                        </div>
                    <div class ="col-md-1 col-sm-5 col-xs-6">
                        <div class="btn-group">
                            <asp:LinkButton ID="btnCopyFromDate" runat="server" style="text-align: center;vertical-align: middle;line-height: 26px; width:75px;" CssClass="btn btn-default btn-xs" OnClick="btnCopyFromDate_Click">
                                <span class="glyphicon glyphicon-paperclip"></span>&nbsp;コピー
                            </asp:LinkButton>
                        </div>
                    </div>
                    </div>
              </div>
            </div>
        </div>
    <%} %>
    <div class="panel panel-default" style="overflow: auto;white-space: nowrap;">
            <asp:Repeater ID="rptDetail" OnItemDataBound="rptDetail_ItemDataBound" runat="server">
                <HeaderTemplate>
                   <table class="table table-bordered" id = "tableDetail" style="width: 1137px;">
                   <%if (this.Mode != Mode.View && this.Mode != Mode.Delete)
                         { %>
                         <thead>
                            <tr id="trHeader1" runat="server">
                                <td style="width: 20px;">
                                </td>
                                <td  style="width: 10px;">
                                    <label class="control-label"> #</label>
                                </td>
                                <td>
                                    <label class="control-label"> プロジェクト</label>
                                </td>
                                <td>
                                    <label class="control-label"> 開始時間</label>
                                </td>
                                <td>
                                    <label class="control-label"> 終了時間</label>
                                </td>
                                <td>
                                    <label class="control-label"> 作業時間</label>
                                </td>
                                <td>
                                    <label class="control-label"> 作業場所</label>
                                </td>
                                <td>
                                    <label class="control-label"> 備考</label>
                                </td>       
                            </tr>
                        </thead>
                         <%}
                         else
                         { %>
                         <thead>
                            <tr id="trHeader2" runat="server">
                                <td  style="width: 10px;">
                                    <label class="control-label"> #</label>
                                </td>
                                <td>
                                    <label class="control-label"> プロジェクト</label>
                                </td>
                                <td>
                                    <label class="control-label"> 開始時間</label>
                                </td>
                                <td>
                                    <label class="control-label"> 終了時間</label>
                                </td>
                                <td>
                                    <label class="control-label"> 作業時間</label>
                                </td>
                                <td>
                                    <label class="control-label"> 作業場所</label>
                                </td>
                                <td>
                                    <label class="control-label"> 備考</label>
                                </td>       
                            </tr>
                        </thead>
                        <%} %>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <%if (this.Mode != Mode.View && this.Mode != Mode.Delete)
                          { %>
                            <td align="center">
                               <input id="deleteFlag" class="deleteFlag" type="checkbox" runat="server" checked='<%# Eval("DelFlag")%>'
                                        data-size="mini" data-on-color="success" data-off-color="danger"/>
                            </td>
                        <%} %>
                        <td>
                            <label class="control-label"> <%# Container.ItemIndex + 1%></label>
                        </td>
                        <td class ="col-md-5">
                            <div class ="row">
                                <div class="col-md-5 col-sm-5 col-xs-5">
                                    <div class='form-group'>
                                        <div class="input-group">
                                            <span class="input-group-btn">
                                                <button id="btnSearchProject" class="btn btn-default btn-sm loading btnSearchKeypress" type="button" runat="server">
                                                    <span class="glyphicon glyphicon-search"></span>
                                                </button>
                                            </span>
                                            <div  runat="server" id="divProjectCD">
                                            <cc1:ICodeTextBox ID="txtProjectCD" runat="server" CssClass="value detail form-control input-sm " AllowChars="-"
                                            Value='<%# Eval("ProjectCD")%>' onvaluechange="projectCDChange" sub-class="Sell" map-field="ProjectCD" item-no='' SearchButton='<%# Container.FindControl("btnSearchProject") %>' />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                               <div class="col-md-7 col-sm-7 col-xs-7">
                                    <div class="form-group">
                                        <cc1:ITextBox ID="txtProjectNm" Value= '<%# Eval("ProjectName")%>' runat="server" TabIndex="-1" CssClass="projectNm form-control input-sm "></cc1:ITextBox>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td class ="col-md-1">
                            <div  runat="server" id="divStartTime">
                                <label class="sr-only"></label>
                                <cc1:ITimeTextBox ID="txtStartTime" CssClass="value timetoint form-control input-sm text-center" onvaluechange="StartTimeChange" CodeType="Numeric" runat="server" MaxLength="4" Value='<%# Eval("StartTime")%>'/>
                            </div>
                        </td>
                        <td class ="col-md-1">
                            <div  runat="server" id="divEndTime">
                                <label class="sr-only"></label>
                                <cc1:ITimeTextBox ID="txtEndTime" CssClass="value timetoint dtExpireDate form-control input-sm text-center" onvaluechange="EndTimeChange" runat="server" MaxLength="4" Value='<%# Eval("EndTime")%>'/>
                            </div>
                        </td>
                        <td class ="col-md-1">
                            <div  runat="server" id="divWorkTime">
                                <label class="sr-only"></label>
                                <cc1:ITimeTextBox ID="txtWorkTime" CssClass="value timetoint form-control input-sm text-center" runat="server" MaxLength="4" Value='<%# Eval("WorkTime")%>'/>
                            </div>
                        </td>
                        <td class ="col-md-2">
                            <div  runat="server" id="divWorkPlace">
                                <label class="sr-only"></label>
                                <cc1:ITextBox ID="txtWorkPlace" CssClass="value dtExpireDate form-control input-sm" runat="server" Value='<%# Eval("WorkPlace")%>'/>
                            </div>
                        </td>
                         <td class ="col-md-2">
                            <div  runat="server" id="divMemo">
                                <label class="sr-only"></label>
                                <cc1:ITextBox ID="txtMemo" CssClass="value dtExpireDate form-control input-sm" runat="server" MaxLength="50" Value='<%# Eval("Memo")%>'/>
                        </div>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
    </div>
    <div class="well-sm">
        <div class="row">
        <%--- Remarks ---%>
            <div class="col-md-8">
                <div class="form-group">
                    <label class="control-label" for="<%= txtMemo.ClientID %>">
                    備考 <strong class="text-danger"></strong></label>
                    <cc1:ITextBox ID="txtMemo" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Rows="5" MaxLength="1000"/>
                </div>
            </div>
        </div>
    </div>

    <%  if (this.Mode == OMS.Utilities.Mode.Request || this.Mode == OMS.Utilities.Mode.Approval 
            || ((this.AttendanceApprovalStatus != OMS.Utilities.AttendanceApprovalStatus.None && this.AttendanceApprovalStatus != OMS.Utilities.AttendanceApprovalStatus.NeedApproval) && this.Mode == OMS.Utilities.Mode.View))
        {
    %>
        <div class="well-sm">
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtRequestNote.ClientID %>">申請事由</label>
                        <cc1:ITextBox ID="txtRequestNote" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Rows="5" MaxLength="1000" />
                    </div>
                </div>
            </div>
        </div>
    <%
        } 
    %>

    <%  if (this.Mode == OMS.Utilities.Mode.Approval
            || ((this.AttendanceApprovalStatus == OMS.Utilities.AttendanceApprovalStatus.Cancel || this.AttendanceApprovalStatus == OMS.Utilities.AttendanceApprovalStatus.Approved) && this.Mode == OMS.Utilities.Mode.View))
        {
    %>
        <div class="well-sm">
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtApprovalNote.ClientID %>">承認事由</label>
                        <cc1:ITextBox ID="txtApprovalNote" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Rows="5" MaxLength="1000" />
                    </div>
                </div>
            </div>
        </div>
    <%
        } 
    %>

    <%
        if (this.Mode != OMS.Utilities.Mode.Delete)
        {
         %>
        <div class="well well-sm">
            <div class="row">
                <div class="col-md-6">
                    <div class="btn-group btn-group-justified">
                        <%
                        if (this.Mode == OMS.Utilities.Mode.Request)
                        {
                        %>
                        <%--Button Edit--%>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnRequest" runat="server" CssClass="btn btn-info btn-sm loading" OnClick="btnRequest_Click">
                                    <span class="glyphicon glyphicon-send"></span> 申請
                            </asp:LinkButton>
                        </div>
                        <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.View )
                        {
                        %>
                        <%--Button Edit--%>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnEdit_Click">
                                <span class="glyphicon glyphicon-pencil"></span> 編集
                            </asp:LinkButton>
                        </div>

                        <%--Button Delete--%>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnDelete_Click">
                                <span class="glyphicon glyphicon-trash"></span> 削除
                            </asp:LinkButton>
                        </div>
                        <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Approval)
                        {
                        %>
                        <%-- Approval button --%>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnApproval" runat="server" OnCommand="btnApproval_Click"
                                CssClass="btn btn-primary btn-sm loading">
                                                      <span class="glyphicon glyphicon-ok"> </span>&nbsp;承認
                            </asp:LinkButton>
                        </div>

                        <%-- Cancel button --%>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnCancel" runat="server" OnCommand="btnCancel_Click"
                                CssClass="btn btn-warning btn-sm loading">
                                                   <span class="glyphicon glyphicon-remove"> </span>&nbsp;差戻
                            </asp:LinkButton>
                        </div>
                        <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Insert)
                        {                                
                        %>
                        <%--- Insert button ---%>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnInsert" runat="server" CssClass="btn btn-primary btn-sm loading" OnClick="btnInsert_Click">
                                <span class="glyphicon glyphicon-ok"></span> 登録
                            </asp:LinkButton>
                        </div>
                        <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Update)
                        {
                        %>
                        <%--- Update button ---%>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm loading" OnClick="btnUpdate_Click">
                                <span class="glyphicon glyphicon-ok"></span> 登録
                            </asp:LinkButton>
                        </div>
                        <%
                        }
                        %>
                        <%--- Back button ---%>
                        <%
            if (this.Mode == OMS.Utilities.Mode.View || this.Mode == OMS.Utilities.Mode.Insert || this.Mode == OMS.Utilities.Mode.Request)
                        {
                        %>
                            <%if(this.Mode == OMS.Utilities.Mode.Insert) %>
                            <%
                            {
                            %>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnBackView" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnBackNew_Click">
                                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                                </asp:LinkButton>
                            </div>
                            <%
                            }
                            else
                            { 
                            %>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnBackNew" runat="server" CssClass="btn btn-default btn-sm loading"
                                            PostBackUrl="~/Attendance/FrmAttendanceList.aspx">
                                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                                    </asp:LinkButton>
                                </div>
                            <%
                            } 
                            %>
                        <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Approval)
                        {
                        %>
                        <div class="btn-group">
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnBackApproval_Click">
                                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                            </asp:LinkButton>
                        </div>
                        <%
                        }
                        else
                        {
                        %>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnBack_Click">
                                <span class="glyphicon glyphicon-chevron-left"></span> 戻る
                            </asp:LinkButton>
                        </div>
                        <%
                        }
                        %>
                    </div>
                </div>
            </div>
        </div>
        <%} %>
    <asp:HiddenField ID="CurerrentProjectId" runat="server" />
    <asp:HiddenField ID="InitDateHidden" runat="server" />
    <asp:HiddenField ID="HidOldLate" runat="server" />
    <asp:HiddenField ID="HidOldEarlyHours" runat="server" />
</asp:Content>

