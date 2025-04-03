<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmWorkingCalendarEntry.aspx.cs" Inherits="OMS.WorkingCalendar.FrmWorkingCalendarEntry" %>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="oms" %>
<%@ Import Namespace="OMS.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">

    //**********************
    // Init
    //**********************
    $(function () {
        DrawRowAll();
        setFocus();
        focusErrors();
        SetValueTotal();
        checkDisableInitDate();

        <%if(this.IsOutFile==true){ %> 
                 setTimeout(function()
            {
                getCtrlById("btnDownload").click();
                hideLoading();                  
            },0);
       <%} %>

        $("[type=checkbox]").bootstrapSwitch();
                
        <%if(this.IsShowQuestion == true){ %>
            $('#modalQuestion').modal('show');

            $('#modalQuestion').on('shown.bs.modal', function (e) {
                $('<%=this.DefaultButton%>').focus();
            });

        <%} %>
        
        //mesage success
         <%if(this.Success == true){ %>
            showSuccess();
                setTimeout(function() {
                    hideSuccess();
            }, 1000 );
        <%} %>

        <%if(this.Mode == OMS.Utilities.Mode.Insert){%>
            if(getCtrlById("txtInitialDate").val() =="")
            {
                $('#btnModelUser').attr('disabled', true);
            }
        <%}%>

        <%if(this.Mode == OMS.Utilities.Mode.View){ %>
            
            var arrayListData = new Array();
            var arrayListUser = new Array();
            var arrayListUserStr = new Array();
            var arrayListUserId = new Array();
            var workingCalendarCd = getCtrlById("txtCalendarCD").val();
            var params = {  'workingCalendarCd': workingCalendarCd,
                            'existWorkingCalendar': true
                     };
            ajax("GetDataTreeView",params, function (response) {
                var result = eval('(' + response.d + ')');
                arrayListData = result;
                for (var i = 0; i < arrayListData.length; i++) {
                    for (var j = 0; j < arrayListData[i].children.length; j++) {
                        
                        arrayListUser.push(arrayListData[i].children[j].id);
                    }
                }
            });

            for (var i = 0; i < arrayListUser.length; i++) {
                arrayListUserStr = arrayListUser[i].split('_');
                arrayListUserId.push(arrayListUserStr[1]);
            }
            getCtrlById("treeViewSave").val(arrayListUserId.join('|'));
        <%} %>

        let arryUser = getCtrlById("treeViewSave").val().split('|');
        showPaidLeaveBtn(arryUser);
      
    });

    function showPaidLeaveBtn(arrUser)
    {
        if (arrUser.indexOf($(getCtrlById("hidUserID")).val()) >= 0)
        {
            $(".btnPaidLeave").show();
            $( "[id^=checkBoxPaidLeave]" ).each(function( index ) {
                if($(this).val() == "1"){
                    $("#" + this.id.replace("checkBoxPaidLeave", "")).prev().addClass("paid-leave");
                }
            });
        }
        else
        {
            $(".btnPaidLeave").hide();
            $(".paid-leave").removeClass("paid-leave");

        }
                
    }

    function showmodalDownLoad(){ 
        
         $('#modalDownLoad').modal('show');
    }

    function downdLoadExcel(){
        
       var check=$('input[id*=rbExcel1]').is(":checked");
       if(check==true){
       }
    }

    //**********************
    // show modal (calendar detail)
    //**********************
    function showModal(fromDateDetail,toDateDetail, HiddenApproval, obj, isPaidLeave){

            $("#contentKimmu").html('');
            
            var fromDate = new Date(fromDateDetail.toString());
            var toDate = new Date(toDateDetail.toString());

            // lấy độ lệch của 2 mốc thời gian, đơn vị tính là millisecond 
            var beginDate = fromDate.getTime();
            var endDate = toDate.getTime();
            var offset = endDate - beginDate;
            var totalDays = Math.round(offset / 1000 / 60 / 60 / 24) + 1;

            // fromday ()
            var curentDate = fromDate;
            var currentDay = fromDate.getDate();
            var cureentMonth = fromDate.getMonth() + 1;
            var dateInMonth = fromDate.getDaysInMonth();
            var txt = "";
          
            //draw content
            
            for (var i = 0; i < totalDays; i++) {
                var currentDayOnWeek = GetWeekWithDate(curentDate);

                // Value Hidden
                var valueHidden = $("#"+curentDate.yyyymmdd()).val();
                var valueWorkingSystemCD = $("#workingSystemCD"+ curentDate.yyyymmdd()).val();
                var valueTextHoliday = $("#textHoliday" + curentDate.yyyymmdd()).val(); 
                var valueCheckBoxIvent = $("#checkBoxIvent" + curentDate.yyyymmdd()).val(); 
                var valuePaidLeave = $("#checkBoxPaidLeave" + curentDate.yyyymmdd()).val();
                
                var valueCmb = valueWorkingSystemCD + "_" + valueHidden;
                var classValue ='';
                if( currentDayOnWeek == '日')
                {
                    classValue = "text-danger";
                }
                else if(valueHidden == '0')
                {
                    classValue = "";
                }
                if (currentDayOnWeek == '土')
                {
                    classValue = "text-primary";
                }
                if(valueTextHoliday != '')
                {
                    classValue = "text-danger";
                }
                
                // set copy combobox
                var cmbWorkingSystem = getCtrlById('cmbWorkingSystemData').clone(true).prop('id', 'cmb' + i );
                $(cmbWorkingSystem).prop('name', curentDate.yyyymmdd());   
                $(cmbWorkingSystem).find('option').filter(function(){ return this.value == valueCmb}).attr('selected','selected');
                var htmlcmb = cmbWorkingSystem[0].outerHTML;
                
                // set list input
                var inputWorkingCalendar = getCtrlById('WokingDateId').clone(true).prop('id', 'inp' + i );
                $(inputWorkingCalendar).prop('name', curentDate.yyyymmdd());
                $(inputWorkingCalendar).attr('value', valueWorkingSystemCD);
                var htmlinput = inputWorkingCalendar[0].outerHTML;

                //checkBox
                var CheckboxDetail = getCtrlById('CheckboxDetail').clone(true).prop('id', 'cbox' + i );

                $(CheckboxDetail).prop('name', curentDate.yyyymmdd());
                if (isPaidLeave)
                {
                    if(valuePaidLeave == 1){
                        $(CheckboxDetail).attr('checked', 'checked');
                    }else{
                        $(CheckboxDetail).removeAttr('checked');
                    }
               
                }
                else
                {
                    if(valueCheckBoxIvent == 1){
                        $(CheckboxDetail).attr('checked', 'checked');
                    }else{
                        $(CheckboxDetail).removeAttr('checked');
                    }
               
                }
                
                var htmlcheckbox = CheckboxDetail[0].outerHTML;

                txt += '<tr class="content1">';
                if(valueTextHoliday =="")
                {
                    txt += '<td id =tdText_' +i+ ' class ="text-center col-md-5 col-sm-5 col-xs-5 '+ classValue +'">' + cureentMonth + '月' + currentDay + '日' +' ('+ currentDayOnWeek +')' + '</td>';
                }
                else
                {
                    txt += '<td id =tdText_' +i+ ' class ="text-center col-md-5 col-sm-5 col-xs-5 '+ classValue +'">'+
                    '<div class="col-md-1 col-sm-1"></div>' + 
                    '<div class ="col-md-5 col-sm-5 text-lg-right text-md-right text-sm-center ">' + cureentMonth + '月' + currentDay + '日' +' ('+ currentDayOnWeek +') ' +'</div>' + 
                    '<div id = submidHoliday'+ curentDate.yyyymmdd() +' value='+ valueTextHoliday +'  class ="col-md-5 col-sm-5 text-lg-left text-md-left text-sm-center">' + valueTextHoliday + '</div>' + 
                    '<div class ="col-md-1 col-sm-5"></div>' +
                    '</td>';
                }
                
                txt += '<td class ="col-md-2 col-sm-2 col-xs-2">' + htmlinput + ' </td>';
                txt += '<td class ="col-md-4 col-sm-4 col-xs-4">'+ htmlcmb + '</td>';
                txt += '<td class ="col-md-1 col-sm-1 col-xs-" align="center">'+ htmlcheckbox + '</td>';
                txt += '</tr>';

                if (currentDay == curentDate.getDaysInMonth()) {
                    currentDay = 0;
                    if(cureentMonth ==12)
                    {
                        cureentMonth =1;
                    }
                    else
                    {
                        cureentMonth++;
                    }
                }
                currentDay++;
                curentDate.addDays(1);
            }
            $("#contentKimmu").append(txt);
            
            getCtrlById("CurrentButton").val(obj.id);
            var buttonIndex = (obj.id).slice(-2).replace("_", "");
            var buttonId = "HiddButtonValue" +(parseInt(buttonIndex)).toString();
            var buttonValue = getCtrlById(buttonId).val();

            if (isPaidLeave)
            {
                $("#myModalType").val("1");
                $("#myModalTitle").html("個人別・有給休暇予定日登録");
                $("#myModalCbxHeader").html("有休予定");
                $('.my-checkbox-area').addClass("hidden");
                if(obj.value != "")
                {
                    getCtrlById("modalSubmit").addClass("hidden");
                    for(var j = 0; j < totalDays; j++)
                    {
                        $('#cmb' + j).attr('disabled', true);
                        $('#inp' + j).attr('disabled', true);
                        $('#cbox' + j).attr('disabled', true);
                    }
                }
                else
                {
                    getCtrlById("modalSubmit").removeClass("hidden");
                    for(var j = 0; j < totalDays; j++)
                    {
                        var valueCmb = $('#cmb'+ j).val();
                        var arrayValueCmb = valueCmb.split("_");
                        var valueHidden = arrayValueCmb[1];
                        var valueWorkingSystemCd = arrayValueCmb[0];

                        $('#cmb' + j).attr('disabled', true);
                        $('#inp' + j).attr('disabled', true);
                        if (valueWorkingSystemCd == 4 || valueHidden != 0)
                        {
                            $('#cbox' + j).attr('disabled', true);
                        }
                    }
                }
            }
            else
            {
                $("#myModalType").val("0");
                $("#myModalTitle").html("勤務体系カレンダー登録（詳細）");
                $("#myModalCbxHeader").html("月例朝礼");
                $('.my-checkbox-area').removeClass("hidden");
                if(HiddenApproval=='true'){

                    if(buttonValue == '0'){
                        if(getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly') == true)
                        {
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', false, false);
                            getCtrlById("myCheckBoxFlag").attr('checked', false);
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('state', false);
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', true, true);
                        }
                        else
                        {
                            getCtrlById("myCheckBoxFlag").attr('checked', false);
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('state', false);
                        }
                
                        for(var j = 0; j < totalDays; j++)
                        {
                    
                            $('#cmb' + j).attr('disabled', false);
                            $('#inp' + j).attr('disabled', false);
                   
                        }

                        if(obj.value != "")
                        {
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', true, true);
                            getCtrlById("modalSubmit").addClass("hidden");
                            for(var j = 0; j < totalDays; j++)
                            {
                                $('#cmb' + j).attr('disabled', true);
                                $('#inp' + j).attr('disabled', true);
                                $('#cbox' + j).attr('disabled', true);
                            }
                        }
                        else
                        {
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', false, false);
                            getCtrlById("modalSubmit").removeClass("hidden");
                        }
                    }
                    else{
                        if(getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly') == true)
                        {
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', false, false);
                            getCtrlById("myCheckBoxFlag").attr('checked', true);
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('state', true);
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', true, true);
                        }
                        else
                        {
                            getCtrlById("myCheckBoxFlag").attr('checked', true);
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('state', true);
                        }
                
                        for(var j = 0; j < totalDays; j++)
                        {
                    
                            $('#cmb' + j).attr('disabled', true);
                            $('#inp' + j).attr('disabled', true);
                           // $('#cbox' + j).attr('disabled', true);
                        }
                        if(obj.value != "")
                        {
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', true, true);
                            getCtrlById("modalSubmit").addClass("hidden");
                            for(var j = 0; j < totalDays; j++)
                            {
                                $('#cmb' + j).attr('disabled', true);
                                $('#inp' + j).attr('disabled', true);
                                $('#cbox' + j).attr('disabled', true);
                            }
                        }
                        else
                        {
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', false, false);
                            getCtrlById("modalSubmit").removeClass("hidden");
                        }
                    }
                }else{
                  if(buttonValue == '0'){
                      if(getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly') == true)
                      {
                          getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', false, false);
                          getCtrlById("myCheckBoxFlag").attr('checked', false);
                          getCtrlById("myCheckBoxFlag").bootstrapSwitch('state', false);
                          getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', true, true);
                      }
                      else
                      {
                          getCtrlById("myCheckBoxFlag").attr('checked', false);
                          getCtrlById("myCheckBoxFlag").bootstrapSwitch('state', false);
                      }
                   }
                   else{
                        if(getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly') == true)
                        {
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', false, false);
                            getCtrlById("myCheckBoxFlag").attr('checked', true);
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('state', true);
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', true, true);
                        }
                        else
                        {
                            getCtrlById("myCheckBoxFlag").attr('checked', true);
                            getCtrlById("myCheckBoxFlag").bootstrapSwitch('state', true);
                        }
                   }

                   if(obj.value != "")
                   {     
                       getCtrlById("modalSubmit").addClass("hidden");
                   }
                   for(var j = 0; j < totalDays; j++)
                        {
                            $('#cmb' + j).attr('disabled', true);
                            $('#inp' + j).attr('disabled', true);
                            $('#cbox' + j).attr('disabled', true);
                        }

                  
                        getCtrlById("myCheckBoxFlag").bootstrapSwitch('readonly', true, true);
                }
            }

            // set default value
            $('#myModal').on('shown.bs.modal', function () {
                $('#inp0').focus().select();
                $('.modal-body').scrollTop(20);
            });

            // show modal 
            $('#myModal').modal('show');
    }

    $(document).ready(function() {
        $(window).resize(function() {
            var bodyheight = getCtrlById("model-body-treeView").height() - 50;
            $(".table-treeview").height(bodyheight);
            $("#btnChangeTreeView").css("margin-top", ((bodyheight/2) - 35)  + "px");
       });
    });

    // data
    var treeLeft;
    var treeRight;
    var float = 0;
    var dataLeft;
    var dataRight;
    var oldDataLeft = new Array();
    var oldDataRight = new Array();    

    //**********************
    // show modal user
    //**********************
    function showModalUser(){
        
        <%if(this.Mode == OMS.Utilities.Mode.View || (this.Mode == OMS.Utilities.Mode.Update && !base._authority.IsWorkCalendarEdit)){%>
           if (typeof(treeLeft) == "undefined")
           {
                $('#btnMoveLeft').attr('disabled', true);
                $('#btnMoveRight').attr('disabled', true);
                $('#btnTreeViewSave').hide();
           }
        <% }%>
        
        <%if((this.Mode == OMS.Utilities.Mode.Update && base._authority.IsWorkCalendarEdit) || this.Mode == OMS.Utilities.Mode.Insert){%>

            //show data
            $("#treeRight").show();
            $("#treeRight li").show();
            $("#treeLeft").show();
            $("#treeLeft li").show();

            if(typeof(treeLeft) == "undefined" || float == 0){
                //load data treeview 
                if(getCtrlById("treeViewLeftData").val() != '' || getCtrlById("treeViewRightData").val() != '')
                {
                    var lstParentIdUserId = getCtrlById("treeViewLeftData").val();
                    var params = { 'lstParentIdUserId': lstParentIdUserId};
                    ajax("GetDataFromListUser", params, function (response) {
                        if (response.d) {
                            var result = eval('(' + response.d + ')');
                            treeLeft = $('#treeLeft').tree({
                            primaryKey: 'id',
                            uiLibrary: 'bootstrap',
                            dataSource: [ {id: '-1',text: '全社共通', children: result } ],
                            checkboxes: true
                    
                            });
                            dataLeft = result;
                        }
                    });
                    lstParentIdUserId = getCtrlById("treeViewRightData").val();
                    params = {  'lstParentIdUserId': lstParentIdUserId};
                    ajax("GetDataFromListUser", params, function (response) {
                        if (response.d) {
                            var result = eval('(' + response.d + ')');
                            treeRight = $('#treeRight').tree({
                            primaryKey: 'id',
                            uiLibrary: 'bootstrap',
                            dataSource: [ {id: '-1',text: '全社共通', children: result } ],
                            checkboxes: true

                            });
                            dataRight = result;
                        }
                    });

                    // set old data left
                    oldDataLeft = setDataNewArrayFromOldArray(oldDataLeft,dataLeft);
                    // set old data right
                    oldDataRight = setDataNewArrayFromOldArray(oldDataRight,dataRight);
                     
                }
                else
                {
                    var flagTreeRight = true;
                    var existWorkingCalendar = false;
                    var workingCalendarCd = getCtrlById("txtCalendarCD").val();

                    // getdata treeLeft
                    params = { 'workingCalendarCd': workingCalendarCd,
                               'existWorkingCalendar': existWorkingCalendar  
                             };
                
                    ajax("GetDataTreeView", params, function (response) {
                        var result = eval('(' + response.d + ')');

                        treeLeft = $('#treeLeft').tree({
                        primaryKey: 'id',
                        uiLibrary: 'bootstrap',
                        dataSource: [ {id: '-1',text: '全社共通', children: result } ],
                        checkboxes: true
                    
                        });
                        dataLeft = result;
                    });

                    // getdata treeRight
                    existWorkingCalendar = flagTreeRight;
                    params = {  'workingCalendarCd': workingCalendarCd,
                                'existWorkingCalendar': true
                             };

                    ajax("GetDataTreeView",params, function (response) {
                        var result = eval('(' + response.d + ')');
                        treeRight = $('#treeRight').tree({
                        primaryKey: 'id',
                        uiLibrary: 'bootstrap',
                        dataSource: [ {id: '-1',text: '全社共通', children: result } ],
                        checkboxes: true

                        });
                        dataRight = result;
                    });

                    // set old data left
                    oldDataLeft = setDataNewArrayFromOldArray(oldDataLeft,dataLeft);
                    // set old data right
                    oldDataRight = setDataNewArrayFromOldArray(oldDataRight,dataRight);

                    //set data for hidden
                    arrayDataHiddenSave = getDataForHidden(dataRight);
                    getCtrlById("treeViewSave").val(arrayDataHiddenSave.join('|'));
                    showPaidLeaveBtn(arrayDataHiddenSave);
                }
                
                //check to hide
                var listHiddenLeft = new Array();
                var listHiddenRight = new Array();
                for (var i = 0; i < dataLeft.length; i++) {
                    if(dataLeft[i].children.length == 0)
                    {
                        listHiddenLeft.push(dataLeft[i].id)
                    }
                }
                if(listHiddenLeft.length == dataLeft.length)
                {
                    $("#treeLeft").hide();
                }

                for (var i = 0; i < listHiddenLeft.length; i++) {
                    $("#treeLeft li").find("[data-id='" + listHiddenLeft[i] + "']").hide();
                }

                //right
                for (var i = 0; i < dataRight.length; i++) {
                    if(dataRight[i].children.length == 0)
                    {
                        listHiddenRight.push(dataRight[i].id)
                    }
                }
                if(listHiddenRight.length == dataRight.length)
                {
                    $("#treeRight").hide();
                }
                for (var i = 0; i < listHiddenRight.length; i++) {
                    $("#treeRight li").find("[data-id='" + listHiddenRight[i] + "']").hide();
                }
            }
            else
            {
                // Load treeLeft
                treeLeft = $('#treeLeft').tree({
                primaryKey: 'id',
                uiLibrary: 'bootstrap',
                dataSource: [ {id: '-1',text: '全社共通', children: dataLeft } ],
                checkboxes: true
                });

                // Load treeRight
                treeRight = $('#treeRight').tree({
                primaryKey: 'id',
                uiLibrary: 'bootstrap',
                dataSource: [ {id: '-1',text: '全社共通', children: dataRight } ],
                checkboxes: true
                });

                // set old data left
                oldDataLeft = setDataNewArrayFromOldArray(oldDataLeft,dataLeft);
                
                // set old data right
                oldDataRight = setDataNewArrayFromOldArray(oldDataRight,dataRight);
                
                //expandAll
                treeLeft.expandAll();
                treeRight.expandAll();

                //check to hide
                var listHiddenLeft = new Array();
                var listHiddenRight = new Array();
                for (var i = 0; i < dataLeft.length; i++) {
                    if(dataLeft[i].children.length == 0)
                    {
                        listHiddenLeft.push(dataLeft[i].id)
                    }
                }

                if(listHiddenLeft.length == dataLeft.length)
                {
                    $("#treeLeft").hide();
                }

                for (var i = 0; i < listHiddenLeft.length; i++) {
                    $("#treeLeft li").find("[data-id='" + listHiddenLeft[i] + "']").hide();
                }

                //right
                for (var i = 0; i < dataRight.length; i++) {
                    if(dataRight[i].children.length == 0)
                    {
                        listHiddenRight.push(dataRight[i].id)
                    }
                }

                if(listHiddenRight.length == dataRight.length)
                {
                    $("#treeRight").hide();
                }

                for (var i = 0; i < listHiddenRight.length; i++) {
                    $("#treeRight li").find("[data-id='" + listHiddenRight[i] + "']").hide();
                }
            }
            //expandAll
            treeLeft.expandAll();
            treeRight.expandAll();

        <% }%>

        <%if(this.Mode == OMS.Utilities.Mode.View || (this.Mode == OMS.Utilities.Mode.Update && !base._authority.IsWorkCalendarEdit)){%>

            //show data
            $("#treeRight").show();
            $("#treeRight li").show();
            $("#treeLeft").show();
            $("#treeLeft li").show();
            var flagTreeRight = true;
            var existWorkingCalendar = false;
            var workingCalendarCd = getCtrlById("txtCalendarCD").val();

            // get data treeLeft
            params = { 'workingCalendarCd': workingCalendarCd,
                        'existWorkingCalendar': existWorkingCalendar  
                        };
        
            ajax("GetDataTreeView", params, function (response) {
                var result = eval('(' + response.d + ')');
                treeLeft = $('#treeLeft').tree({
                primaryKey: 'id',
                uiLibrary: 'bootstrap',
                dataSource: [ {id: '-1',text: '全社共通', children: result } ],
                checkboxes: false

                });
                dataLeft = result;
            });

            // get data treeRight
            existWorkingCalendar = flagTreeRight;     
            params = {  'workingCalendarCd': workingCalendarCd,
                        'existWorkingCalendar': true
                        };

            ajax("GetDataTreeView",params, function (response) {
                var result = eval('(' + response.d + ')');
                treeRight = $('#treeRight').tree({
                primaryKey: 'id',
                uiLibrary: 'bootstrap',
                dataSource: [ {id: '-1',text: '全社共通', children: result } ],
                checkboxes: false

                });
                dataRight = result;
            });

            // set old data left
            oldDataLeft = setDataNewArrayFromOldArray(oldDataLeft,dataLeft);
            // set old data right
            oldDataRight = setDataNewArrayFromOldArray(oldDataRight,dataRight);

            //expandAll
            treeLeft.expandAll();
            treeRight.expandAll();

            //check to hide
            var listHiddenLeft = new Array();
            var listHiddenRight = new Array();
            for (var i = 0; i < dataLeft.length; i++) {
                if(dataLeft[i].children.length == 0)
                {
                    listHiddenLeft.push(dataLeft[i].id)
                }
            }
            if(listHiddenLeft.length == dataLeft.length)
            {
                $("#treeLeft").hide();
            }

            for (var i = 0; i < listHiddenLeft.length; i++) {
                $("#treeLeft li").find("[data-id='" + listHiddenLeft[i] + "']").hide();
            }

            //right
            for (var i = 0; i < dataRight.length; i++) {
                if(dataRight[i].children.length == 0)
                {
                    listHiddenRight.push(dataRight[i].id)
                }
            }
            if(listHiddenRight.length == dataRight.length)
            {
                $("#treeRight").hide();
            }
            for (var i = 0; i < listHiddenRight.length; i++) {
                $("#treeRight li").find("[data-id='" + listHiddenRight[i] + "']").hide();
            }

        <% }%>

        $('#ModalUser').on('shown.bs.modal', function () {
            var bodyheight = getCtrlById("model-body-treeView").height() - 50;
            $(".table-treeview").height(bodyheight);
            $("#btnChangeTreeView").css("margin-top", ((bodyheight/2) - 35)  + "px");
            SetInvalidUserColor();
        });

        $('#ModalUser').modal('show');

    }

    function SetInvalidUserColor()
    {
        //$( "li.list-group-item[data-id$='_1']" ).css("background-color", "pink");
        $( "li.list-group-item[data-id$='_1']").find( "span[data-role='display']" ).css("background-color", "pink");
    }

    //**********************
    // btnMoveLeft
    //**********************
    getCtrlById("btnMoveLeft").on('click', function(){
        
        //show data
        $("#treeRight").show();
        $("#treeRight li").show();
        $("#treeLeft").show();
        $("#treeLeft li").show();

        var arrayDataNodeAdd = new Array();
        var arrayDataHiddenRight = new Array();
        var arrayDataHiddenLeft = new Array();
        var arrayDataHiddenSave = new Array();
        var parent;
        var parentId;
        var parentData;
        var dataDeleteLeft;
        var dataAddRight;

        //get node check
        var checkedIds = treeLeft.getCheckedNodes();

        for (var i = 0; i < checkedIds.length; i++) {
            if((typeof checkedIds[i]) == 'string')
            {
                var params = { 'userId': checkedIds[i]};
                ajax("GetParentNode", params, function (response) {
                    if (response.d) {
                    var result = eval('(' + response.d + ')');
                    parentId = (result["ParentId"]);
                  }
                });

                parent = treeRight.getNodeById(parentId).children('ul');
                if(parent.length == 0)
                {
                    parent = treeRight.getNodeById(parentId);
                    
                }

                //get ParenData
                parentData = treeRight.getDataById(parentId);

                //change dataleft
                dataLeft = removeChildren(dataLeft,checkedIds[i]);

                //get data node add
                dataAddRight = treeLeft.getDataById(checkedIds[i]);
                treeRight.addNode(dataAddRight, parent, 1);

                //change dataright
                dataRight = addChildren(dataRight, parentData, dataAddRight);

                //get delete node
                dataDeleteLeft = treeLeft.getNodeById(checkedIds[i]);
                treeLeft.removeNode(dataDeleteLeft);
            }
        }

        //load treeLeft
        treeLeft.destroy();
        treeLeft = $('#treeLeft').tree({
        primaryKey: 'id',
        uiLibrary: 'bootstrap',
        dataSource: [ {id: '-1',text: '全社共通', children: dataLeft } ],
        checkboxes: true
        });
        treeLeft.expandAll();

        //reload treeRight
        treeRight.destroy();
        treeRight = $('#treeRight').tree({
        primaryKey: 'id',
        uiLibrary: 'bootstrap',
        dataSource: [ {id: '-1',text: '全社共通', children: dataRight } ],
        checkboxes: true
        });
        treeRight.expandAll();

        //set data for hidden
        //arrayDataHiddenSave = getDataForHidden(dataRight);
        //getCtrlById("treeViewSave").val(arrayDataHiddenSave.join('|'));
        //showPaidLeaveBtn(arrayDataHiddenSave);

        arrayDataHiddenLeft = getDataForTreeViewHidden(dataLeft);
        getCtrlById("treeViewLeftData").val(arrayDataHiddenLeft.join('|'));

        arrayDataHiddenRight = getDataForTreeViewHidden(dataRight);
        getCtrlById("treeViewRightData").val(arrayDataHiddenRight.join('|')); 

        //expandAll
        treeLeft.expandAll();
        treeRight.expandAll();

        //check to hide
        var listHiddenLeft = new Array();
        var listHiddenRight = new Array();
        for (var i = 0; i < dataLeft.length; i++) {
            if(dataLeft[i].children.length == 0)
            {
                listHiddenLeft.push(dataLeft[i].id)
            }
        }
        if(listHiddenLeft.length == dataLeft.length)
        {
            $("#treeLeft").hide();
        }

        for (var i = 0; i < listHiddenLeft.length; i++) {
            $("#treeLeft li").find("[data-id='" + listHiddenLeft[i] + "']").hide();
        }

        //right
        for (var i = 0; i < dataRight.length; i++) {
            if(dataRight[i].children.length == 0)
            {
                listHiddenRight.push(dataRight[i].id)
            }
        }
        if(listHiddenRight.length == dataRight.length)
        {
            $("#treeRight").hide();
        }
        for (var i = 0; i < listHiddenRight.length; i++) {
            $("#treeRight li").find("[data-id='" + listHiddenRight[i] + "']").hide();
        }
        SetInvalidUserColor();
    });

    //**********************
    // btnMoveRight
    //**********************
    getCtrlById("btnMoveRight").on('click', function(){
        
        //show data
        $("#treeRight").show();
        $("#treeRight li").show();
        $("#treeLeft").show();
        $("#treeLeft li").show();
        var arrayDataNodeAdd = new Array();
        var arrayDataHiddenRight = new Array();
        var arrayDataHiddenLeft = new Array();
        var arrayDataHiddenSave = new Array();
        var parent;
        var parentId;
        var parentData;
        var dataDeleteRight;
        var dataAddLeft;

        //get node check
        var checkedIds = treeRight.getCheckedNodes();
        for (var i = 0; i < checkedIds.length; i++) {
            if((typeof checkedIds[i]) == 'string')
            {
                var params = { 'userId': checkedIds[i]};
                ajax("GetParentNode", params, function (response) {
                    if (response.d) {
                    var result = eval('(' + response.d + ')');
                    parentId = (result["ParentId"]);
                    }
                });

                parent = treeLeft.getNodeById(parentId).children('ul');
                if(parent.length == 0)
                {
                    parent = treeLeft.getNodeById(parentId);
                    
                }
                //get ParenData
                parentData = treeLeft.getDataById(parentId);

                //change dataleft
                dataRight = removeChildren(dataRight,checkedIds[i]);

                //get data node add
                dataAddLeft = treeRight.getDataById(checkedIds[i]);
                treeLeft.addNode(dataAddLeft, parent, 1);

                //change dataright
                dataLeft = addChildren(dataLeft, parentData, dataAddLeft);

                //get delete node
                dataDeleteRight = treeRight.getNodeById(checkedIds[i]);
                treeRight.removeNode(dataDeleteRight);
            }
        }

        //load treeLeft
        treeRight.destroy();
        treeRight = $('#treeRight').tree({
        primaryKey: 'id',
        uiLibrary: 'bootstrap',
        dataSource: [ {id: '-1',text: '全社共通', children: dataRight } ],
        checkboxes: true
        });
        treeRight.expandAll();

        //reload treeRight
        treeLeft.destroy();
        treeLeft = $('#treeLeft').tree({
        primaryKey: 'id',
        uiLibrary: 'bootstrap',
        dataSource: [ {id: '-1',text: '全社共通', children: dataLeft } ],
        checkboxes: true
        });
        treeLeft.expandAll();

        //set data for hidden
        //arrayDataHiddenSave = getDataForHidden(dataRight);
        //getCtrlById("treeViewSave").val(arrayDataHiddenSave.join('|'));
        //showPaidLeaveBtn(arrayDataHiddenSave);

        arrayDataHiddenLeft = getDataForTreeViewHidden(dataLeft);
        getCtrlById("treeViewLeftData").val(arrayDataHiddenLeft.join('|'));

        arrayDataHiddenRight = getDataForTreeViewHidden(dataRight);
        getCtrlById("treeViewRightData").val(arrayDataHiddenRight.join('|')); 

        //check to hide
        var listHiddenLeft = new Array();
        var listHiddenRight = new Array();
        for (var i = 0; i < dataLeft.length; i++) {
            if(dataLeft[i].children.length == 0)
            {
                listHiddenLeft.push(dataLeft[i].id)
            }
        }
        if(listHiddenLeft.length == dataLeft.length)
        {
            $("#treeLeft").hide();
        }

        for (var i = 0; i < listHiddenLeft.length; i++) {
            $("#treeLeft li").find("[data-id='" + listHiddenLeft[i] + "']").hide();
        }

        //right
        for (var i = 0; i < dataRight.length; i++) {
            if(dataRight[i].children.length == 0)
            {
                listHiddenRight.push(dataRight[i].id)
            }
        }
        if(listHiddenRight.length == dataRight.length)
        {
            $("#treeRight").hide();
        }
        for (var i = 0; i < listHiddenRight.length; i++) {
            $("#treeRight li").find("[data-id='" + listHiddenRight[i] + "']").hide();
        }

        SetInvalidUserColor();
    });

    //set data for arrayNew
    function setDataNewArrayFromOldArray(arrayNew, arrayOld)
    {
        arrayNew = []; // create empty array to hold copy
        for (var i = 0; i < arrayOld.length; i++) {
                arrayNew[i] = {}; // empty object to hold properties added below

                for (var prop in arrayOld[i]) {
                if(prop != "children")
                {
                    arrayNew[i][prop] = arrayOld[i][prop]; // copy properties
                }
                else
                {
                    arrayNew[i].children = [];
                    for(var j = 0; j<arrayOld[i].children.length; j++)
                    {
                        arrayNew[i].children[j] = {};
                        for(var propchildren in arrayOld[i].children[j])
                        {
                            arrayNew[i].children[j][propchildren] = arrayOld[i].children[j][propchildren]; // copy properties
                        }
                    }       
                }
            }
        }

        return arrayNew;
    }

    // remove children 
    function removeChildren(arrayDataRemove, id){
        
        for (var i = 0; i < arrayDataRemove.length; i++) {
            for (var j = 0; j < arrayDataRemove[i].children.length; j++) {
                if(arrayDataRemove[i].children[j].id == id)
                {
                    arrayDataRemove[i].children.splice(j,1);
                    return arrayDataRemove;
                }
            }
        }
        return arrayDataRemove;
    }

    // add node for array
    function addChildren(arrayDataAdd,ParentData, ChildrenData){
        for (var i = 0; i < arrayDataAdd.length; i++) {
            if(arrayDataAdd[i].id == ParentData.id)
            {
                arrayDataAdd[i].children.push(ChildrenData);
                return arrayDataAdd;
            }
        }
        return arrayDataAdd;
    } 

    //get data for hidden
    function getDataForHidden(arrayData)
    {
        var arrayHidden = new Array();
        var arrayHiddensId = new Array();
        for (var i = 0; i < arrayData.length; i++) {
            for (var j = 0; j < arrayData[i].children.length; j++) {
                arrayHiddensId = arrayData[i].children[j].id.split('_')
                arrayHidden.push(arrayHiddensId[1]);
            }
        }
        return arrayHidden;
    }

    //get data for Left, right tree
    function getDataForTreeViewHidden(arrayData)
    {
        var arrayHidden = new Array();
        for (var i = 0; i < arrayData.length; i++) {
            for (var j = 0; j < arrayData[i].children.length; j++) {
                arrayHidden.push(arrayData[i].children[j].id);
            }
        }
        return arrayHidden;
    }

    //modelTreeViewSave()
    function modelTreeViewSave()
    {
        var arrayDataHiddenLeft = new Array();
        var arrayDataHiddenRight = new Array();
        if(float==0)
        {
            float = 1;
        }
        
        //set data for hidden
        arrayDataHiddenLeft = getDataForTreeViewHidden(dataLeft);
        getCtrlById("treeViewLeftData").val(arrayDataHiddenLeft.join('|'));

        arrayDataHiddenRight = getDataForTreeViewHidden(dataRight);
        getCtrlById("treeViewRightData").val(arrayDataHiddenRight.join('|')); 

        //set data for hidden
        arrayDataHiddenSave = getDataForHidden(dataRight);
        getCtrlById("treeViewSave").val(arrayDataHiddenSave.join('|'));
        showPaidLeaveBtn(arrayDataHiddenSave);

        treeLeft.destroy();
        treeRight.destroy();
    }

    // deletedata()
    function destructorTreeView()
    {
        dataLeft = oldDataLeft;
        dataRight = oldDataRight;

        //set data for hidden
        arrayDataHiddenLeft = getDataForTreeViewHidden(dataLeft);
        getCtrlById("treeViewLeftData").val(arrayDataHiddenLeft.join('|'));

        arrayDataHiddenRight = getDataForTreeViewHidden(dataRight);
        getCtrlById("treeViewRightData").val(arrayDataHiddenRight.join('|')); 
        
        //arrayDataHiddenSave = getDataForHidden(dataRight);
        //getCtrlById("treeViewSave").val(arrayDataHiddenSave.join('|'));
        //showPaidLeaveBtn(arrayDataHiddenSave);
        
        treeLeft.destroy();
        treeRight.destroy();
    }

    //**************************************
    // Set Value when boostrapSwitch change
    //**************************************
    $(".myCheckBoxFlag").on('switchChange.bootstrapSwitch', myCheckBoxChange);

    function myCheckBoxChange(event, state) {
        var countRowInput = $('#contentKimmu tr').length;
        if(state == true)
        {
            for(var j = 0; j < countRowInput; j++)
            {
                    
                $('#cmb' + j).attr('disabled', true);
                $('#inp' + j).attr('disabled', true);  
                $('#cbox' + j).attr('disabled', false);           
            }
        }
        else
        {
            for(var j = 0; j < countRowInput; j++)
            {
                    
                $('#cmb' + j).attr('disabled', false);
                $('#inp' + j).attr('disabled', false);
                $('#cbox' + j).attr('disabled', false);    
               
            }  
        }       
    }

    //********************************
    // // remove character '-' in date
    //********************************
    Date.prototype.yyyymmdd = function() {
      var mm = this.getMonth() + 1; // getMonth() is zero-based
      var dd = this.getDate();

      return [this.getFullYear(),
              (mm>9 ? '' : '0') + mm,
              (dd>9 ? '' : '0') + dd
             ].join('');
    };

    //********************************
    // function check leap year
    //********************************
    Date.isLeapYear = function (year) {
        return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
    };

    // function get day in month
    Date.getDaysInMonth = function (year, month) {
        return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
    };

    // prototy check leap year
    Date.prototype.isLeapYear = function () {
        return Date.isLeapYear(this.getFullYear());
    };

    // get day in month
    Date.prototype.getDaysInMonth = function () {
        return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
    };

    // Get day in week
    function GetWeekWithDate(date){
        var gsDayNames = [
                '日',
                '月',
                '火',
                '水',
                '木',
                '金',
                '土'
            ];
        return gsDayNames[date.getDay()];
    }
    //convert to date time 
    function convertToDateTime(date){
        var CurrentDate = new Date(date.substring(0,4) + "/" + date.substring(4,6) + "/" + date.substring(6,9));
        return CurrentDate;
    }

    // prototype add a day to current date
    Date.prototype.addDays = function (value){
            this.setDate(this.getDate() + value);
            return this;
    }

    //**********************************
    // Set Value when input change value
    //**********************************
    $(document).on('change','.WorkingDateDetail', function(){
        var inputValue = this.value;
        var cmbValueOld = $('#cmb' + this.id.replace('inp','')).val();
        var arrayInputValue = cmbValueOld.split("_");
        var inputValueOld = arrayInputValue[0];
        
        var cmbValuecheck = inputValue;
        var cmbValue = "";
        $('#cmb' + this.id.replace('inp','') + '> option').each(function(index)
        {
            var eachValue =  $(this).val().split("_");
            if(inputValue == eachValue[0])
            {
                cmbValue = $(this).val();     
            }
        });
        $('#cmb' + this.id.replace('inp','')).val(cmbValue);
        if($('#cmb' + this.id.replace('inp',''))[0].selectedIndex == -1){
            $('#cmb' + this.id.replace('inp','')).val(cmbValueOld);
            this.value = inputValueOld;
            this.focus();
        }
        var cmbValue = $('#cmb' + this.id.replace('inp','')).val();
        var arrayCmbValue = cmbValue.split("_");
        var checkClass = arrayCmbValue[1];
        var classValue ='';
        var idElement = 'tdText_'+ this.id.replace('inp','');
        ChangeColorTextDay(idElement, checkClass, false);

    });

    //**********************************
    // Set Value when input change value
    //**********************************
    $(document).on('focus','.WorkingDateDetail', function(){
        var currentInput = this;
        currentInput.select();
    });

    //**********************************************
    // set value when combobox in modal change value
    //**********************************************
    $(document).on('change','.cmbWorkingSystem', function(){
        var cmbValue = this.options[this.selectedIndex].value;
        var inpValue = cmbValue.split("_");
        $('#inp' + this.id.replace('cmb','')).val(inpValue[0]);
        var checkClass = inpValue[1];
        var idElement = 'tdText_'+ this.id.replace('cmb','');
        ChangeColorTextDay(idElement, checkClass, false);

    });

    //*************************************************
    // ChangeColorTextDay when change input or combobox
    //*************************************************
    function ChangeColorTextDay(idText, checkClass, flag)
    {
        if(flag == true)
        {
            var classValue = ''
            if(checkClass == '2')
            {
                classValue = "text-center col-md-6 col-sm-6 col-xs-6 text-danger";
            }
            else if(checkClass == '1')
            {
                classValue = "text-center col-md-6 col-sm-6 col-xs-6 text-primary";
            }
            else
            {
                classValue = "text-center";
            }
            $('#'+ idText).attr('class', classValue);
         }
    }

    //**************************
    // Submit modal(OK)
    //*************************
    $("#modalSubmit").click(function(){
        if ($("#myModalType").val() == "1") //paid-leave set
        {
            var count = $(".cmbWorkingSystem").length -1;
            for (var i = 0; i < count; i++) {
                var idHidden = $('#inp'+ i).attr('name');
                var valuecbox = $('#cbox'+ i).is(":checked");
                    
                $("#checkBoxPaidLeave"+idHidden).val(0);
                if(valuecbox){
                    $("#checkBoxPaidLeave"+idHidden).val(1);
                    $("#"+idHidden).prev().addClass("paid-leave");
                }
                else
                {
                    $("#"+idHidden).prev().removeClass("paid-leave");
                }
            }
        }
        else{
            var count = $(".cmbWorkingSystem").length -1;
            var holiday = 0;
            var nomalday = 0;
            for (var i = 0; i < count; i++) {
                var valueCmb = $('#cmb'+ i).val();
            
                var valuecbox = $('#cbox'+ i).is(":checked");
                var arrayValueCmb = valueCmb.split("_");
                var valueHidden = arrayValueCmb[1];
                var valueWorkingSystemCd = arrayValueCmb[0];
                var idHidden = $('#inp'+ i).attr('name');

                var currentDayOnWeek = GetWeekWithDate(convertToDateTime(idHidden));
                var classValue ="";
                var textColorClass = "";
            
                if(valueHidden == 2)
                {
                    holiday = holiday +1;
                    if ($("#submidHoliday"+ idHidden).text() != "")
                    {
                        classValue = "legal-holiday";
                        textColorClass = "text-color-holiday";
                    }
                    else
                    {
                        classValue = "legal";
                        textColorClass = "text-color-holiday";
                    }
                }
                else if(valueHidden == 1)
                {
                    holiday = holiday +1;
                    if ($("#submidHoliday"+ idHidden).text() != "")
                    {
                        classValue = "legal-holiday";
                        textColorClass = "text-color-holiday";
                    }
                    else
                    {
                        classValue = 'attendance';
                        textColorClass = "text-color-saturday";
                    }
                
                }
                else
                {
                    nomalday = nomalday +1;
                    if(valueWorkingSystemCd == 4){
                        classValue = 'attendance-full-time';
                    }
                    else if ($("#submidHoliday"+ idHidden).text() != "")
                    {
                        classValue = 'work-holiday';
                    }
                    else if (currentDayOnWeek == "日")
                    {
                        classValue = "work-weekend";
                    }
                    else if(currentDayOnWeek == "土")
                    {
                        classValue = "work-weekend-saturday";
                    }
                    else
                    {
                        classValue = "";
                    }
                    textColorClass = "text-color-normalday";
                }

                if(valuecbox){
                   textColorClass = textColorClass +' checkbox-ivent-circle';
                }
            
                $("#"+idHidden).val(valueHidden);
                $("#workingSystemCD"+idHidden).val(valueWorkingSystemCd);
                 $("#checkBoxIvent"+idHidden).val(0);
                if(valuecbox){
                 $("#checkBoxIvent"+idHidden).val(1);
                }
                if(valueWorkingSystemCd == "4" || valueHidden != "0")
                {
                    $("#checkBoxPaidLeave"+idHidden).val(0);
                }

                if ($("#checkBoxPaidLeave"+idHidden).val() == "1")
                {
                    classValue = classValue + " paid-leave";
                }

           
                $("#"+idHidden).prev().attr('class', classValue);

                $("#"+idHidden).prev().children("div").children("div").attr('class', '');
                $("#"+idHidden).prev().children("div").children("div").addClass(textColorClass);

            }
            var ButtonIdCurrent = getCtrlById("CurrentButton").val();
            var buttonIndex = (ButtonIdCurrent).slice(-2).replace("_", "");

            var txtIndex = parseInt(buttonIndex) + 1;

            var buttonId = "HiddButtonValue" +(parseInt(buttonIndex)).toString();

            // set again totaltable
            getCtrlById("txtHolidayInMonth" + txtIndex).text(holiday);
            getCtrlById("txtHolidayInMonth" + txtIndex + "_xs").text(holiday);
            getCtrlById("txtWorkingDate" + txtIndex).text(nomalday);
            getCtrlById("txtWorkingDate" + txtIndex + "_xs").text(nomalday);
            caculateWorkingTime();

            // cacular the totaltable
            SetValueTotal();

            if(getCtrlById("myCheckBoxFlag").is(':checked'))
            {
            
                $('#' + ButtonIdCurrent).removeClass('btn-info');
                $('#' + ButtonIdCurrent).addClass('btn-agreement');
                $('#' + ButtonIdCurrent).html('協定済');
                getCtrlById(buttonId).attr('value','1');
                getCtrlById(buttonId).val('1');
                checkDisableInitDate();
            }
            else
            {
                $('#' + ButtonIdCurrent).addClass('btn-info');
                $('#' + ButtonIdCurrent).removeClass('btn-agreement');
                $('#' + ButtonIdCurrent).html('未確定');
                getCtrlById(buttonId).attr('value','0');
                getCtrlById(buttonId).val('0');
                checkDisableInitDate();
            }
        }             
    });
    getCtrlById('txtInitialDate').bind( "onIDateChange", function() {
      if($(this).val() == ''){
            $("#CalendarWorking").html('');
            $("#tableTotal").html('');
            getCtrlById("btnClearInsert").addClass('disabled');
            getCtrlById("btnModelUser").addClass('disabled');
            
            return;
        }
        execute(getCtrlById("btnSearch"));
    });

    getCtrlById('txtInitialDate').on( "changeDate", function() {
      if($(this).val() == ''){
            $("#CalendarWorking").html('');
            $("#tableTotal").html('');
            getCtrlById("btnClearInsert").addClass('disabled');
            getCtrlById("btnModelUser").addClass('disabled');
            return;
        }
        execute(getCtrlById("btnSearch"));
    });
 
    //**********************
    //Draw Row All Table
    //**********************
    function DrawRowAll() {
        var rowCount1 = $('#table_1 tr').length;
        var rowCount2 = $('#table_2 tr').length;
        var rowCount3 = $('#table_3 tr').length;
        var rowCount4 = $('#table_4 tr').length;
        var rowCount5 = $('#table_5 tr').length;
        var rowCount6 = $('#table_6 tr').length;
        var rowCount7 = $('#table_7 tr').length;
        var rowCount8 = $('#table_8 tr').length;
        var rowCount9 = $('#table_9 tr').length;
        var rowCount10 = $('#table_10 tr').length;
        var rowCount11 = $('#table_11 tr').length;
        var rowCount12 = $('#table_12 tr').length;
        var maxRowCount = CompareRowCount(CompareRowCount(rowCount1, rowCount2), rowCount3);
        DrawOnlyRow(1, rowCount1, maxRowCount);
        DrawOnlyRow(2, rowCount2, maxRowCount);
        DrawOnlyRow(3, rowCount3, maxRowCount);
        maxRowCount = CompareRowCount(CompareRowCount(rowCount4, rowCount5), rowCount6);
        DrawOnlyRow(4, rowCount4, maxRowCount);
        DrawOnlyRow(5, rowCount5, maxRowCount);
        DrawOnlyRow(6, rowCount6, maxRowCount);
        maxRowCount = CompareRowCount(CompareRowCount(rowCount7, rowCount8), rowCount9);
        DrawOnlyRow(7, rowCount7, maxRowCount);
        DrawOnlyRow(8, rowCount8, maxRowCount);
        DrawOnlyRow(9, rowCount9, maxRowCount);
        maxRowCount = CompareRowCount(CompareRowCount(rowCount10, rowCount11), rowCount12);
        DrawOnlyRow(10, rowCount10, maxRowCount);
        DrawOnlyRow(11, rowCount11, maxRowCount);
        DrawOnlyRow(12, rowCount12, maxRowCount);
    }

    //**********************
    //Draw Each Table
    //**********************
    function DrawOnlyRow(tableIndex,rowCount, maxRowCount) {
        var countDraw = maxRowCount - rowCount;
        for (var i = 0; i < countDraw; i++) {
            $('#table_' + tableIndex + ' tr:last').after('<tr><td>&nbsp;</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>');
        }
    }
    //**********************
    //Find Max
    //**********************  
    function CompareRowCount(rowtable1, rowtable2) {
        if (rowtable1 > rowtable2) {
            return rowtable1;
        }
        else {
            return rowtable2;
        }
    }

    //**********************
    // Find Back
    //**********************    
    function findBack() {
        hideLoading();
        getCtrlById("txtCalendarCD").focus().select();
    }

     //**********************
    // Set Focus
    //**********************
    function setFocus() {
        <%if(this.Mode==Mode.Insert){ %>
            getCtrlById("txtCalendarCD").focus().select();

           <%if(flagSetFocus == 1){ %>
                // set focus 
                getCtrlById("btnModelUser").focus();
           <%}%>
        <% }%>
            
        <%if(this.Mode==Mode.Update){ %>
            getCtrlById("txtCalendarName").focus().select();
            
            <%if(flagSetFocus == 1){ %>
                // set focus 
                getCtrlById("btnModelUser").focus();
           <%}%>
        <%} %>            
    }

    //**************************
    // Set Value Total for Table
    //*************************
    function SetValueTotal()
    {
        if(getCtrlById("txtHolidayInMonth1").text() != "")
        {
            getCtrlById("tableTotal").removeClass( "hidden" );
            

            // set Holiday Total
            var holidayInMonthTotal = parseInt(getCtrlById("txtHolidayInMonth1").text()) + parseInt(getCtrlById("txtHolidayInMonth2").text()) + parseInt(getCtrlById("txtHolidayInMonth3").text()) + 
                                        parseInt(getCtrlById("txtHolidayInMonth4").text()) + parseInt(getCtrlById("txtHolidayInMonth5").text()) + parseInt(getCtrlById("txtHolidayInMonth6").text()) +
                                        parseInt(getCtrlById("txtHolidayInMonth7").text()) + parseInt(getCtrlById("txtHolidayInMonth8").text()) + parseInt(getCtrlById("txtHolidayInMonth9").text()) + 
                                        parseInt(getCtrlById("txtHolidayInMonth10").text()) + parseInt(getCtrlById("txtHolidayInMonth11").text()) + parseInt(getCtrlById("txtHolidayInMonth12").text());
            getCtrlById("txtHolidayInMonthTotal").text(holidayInMonthTotal);
            getCtrlById("txtHolidayInMonthTotal_xs").text(holidayInMonthTotal);
            getCtrlById("HiddenHolidayInMonthTotal").val(holidayInMonthTotal);

            // set working Date Total
            var workingDateTotal = parseInt(getCtrlById("txtWorkingDate1").text()) + parseInt(getCtrlById("txtWorkingDate2").text()) + parseInt(getCtrlById("txtWorkingDate3").text()) + 
                                        parseInt(getCtrlById("txtWorkingDate4").text()) + parseInt(getCtrlById("txtWorkingDate5").text()) + parseInt(getCtrlById("txtWorkingDate6").text()) +
                                        parseInt(getCtrlById("txtWorkingDate7").text()) + parseInt(getCtrlById("txtWorkingDate8").text()) + parseInt(getCtrlById("txtWorkingDate9").text()) + 
                                        parseInt(getCtrlById("txtWorkingDate10").text()) + parseInt(getCtrlById("txtWorkingDate11").text()) + parseInt(getCtrlById("txtWorkingDate12").text());
            getCtrlById("txtWorkingDateTotal").text(workingDateTotal);
            getCtrlById("txtWorkingDateTotal_xs").text(workingDateTotal);
            getCtrlById("HiddenWorkingDateTotal").val(workingDateTotal);

            var WorkingTimeInMonthTotal = workingDateTotal * parseInt(getCtrlById("WorkingTimeSystem").val());

            //MinuteToTime
            getCtrlById("txtWorkingTimeInMonthTotal").text(MinuteToTime(WorkingTimeInMonthTotal));
            getCtrlById("txtWorkingTimeInMonthTotal_xs").text(MinuteToTime(WorkingTimeInMonthTotal));
            getCtrlById("HiddenWorkingTimeInMonthTotal").val(MinuteToTime(WorkingTimeInMonthTotal));

            var WorkingTimeWeeklyAverageTotal = WorkingTimeInMonthTotal/(parseFloat(getCtrlById("txtCountDayInMonthTotal").text())/7);
            
            getCtrlById("txtWorkingTimeWeeklyAverageTotal").text(MinuteToTime(WorkingTimeWeeklyAverageTotal));
            getCtrlById("txtWorkingTimeWeeklyAverageTotal_xs").text(MinuteToTime(WorkingTimeWeeklyAverageTotal));
            getCtrlById("HiddenWorkingTimeWeeklyAverageTotal").val(MinuteToTime(WorkingTimeWeeklyAverageTotal));
            
        }
    }

    //*****************************************
    // Caculate total table when change holiday
    //*****************************************
    function caculateWorkingTime()
    {
        for (var i = 1; i < 13; i++) 
        {
             var workingdateValue = parseInt(getCtrlById("txtWorkingDate" + i).text());
             var countDayInMonth = parseInt(getCtrlById("txtCountDayInMonth" + i).text());
              
             var workingTimeInMonth = workingdateValue * getCtrlById("WorkingTimeSystem").val();
             var workingTimeWeeklyAverage = parseFloat(workingTimeInMonth) / (parseFloat(countDayInMonth) / parseFloat(7));
             
             getCtrlById("txtWorkingTimeInMonth" + i).text(MinuteToTime(workingTimeInMonth));
             getCtrlById("txtWorkingTimeInMonth" + i +"_xs").text(MinuteToTime(workingTimeInMonth));
             getCtrlById("txtWorkingTimeWeeklyAverage" + i).text(MinuteToTime(workingTimeWeeklyAverage));
             getCtrlById("txtWorkingTimeWeeklyAverage" + i +"_xs").text(MinuteToTime(workingTimeWeeklyAverage));

        }
    }

    //**********************
    // Change format to caculate (time -> minute)
    //**********************
    function TimeToMinute(pValue)
    {
        var ary = pValue.split(':');
        var nHours = 0;
        var nMinutes = 0;

        if (ary.length != 2)
        {
            return 0;
        }

        nHours = parseInt(ary[0]);
        nMinutes = parseInt(ary[1]);
        if (nMinutes < 0)
        {

            return 0;
        }

        return (nHours * 60 + nMinutes);
    }

    //**********************
    // Change format minute -> time
    //**********************
    function MinuteToTime(mValue)
    {
        if(mValue != 0)
        {
            var sResult = parseInt(mValue / 60);
        }
        else
        {
            var sResult = parseInt(mValue / 60).toString().padLeft(2,'0');
        }
        
        sResult += ":"
        sResult += parseInt(Math.ceil(mValue % 60)).toString().padLeft(2,'0');

        return sResult;
    }

    //**************************************
    // Disable txtInitialDate with each case
    //**************************************
    function checkDisableInitDate()
    {
     <%if(this.Mode==Mode.Update && !base._authority.IsWorkCalendarEdit){ %>
        getCtrlById("txtInitialDate").prop('readonly', true);
        return;
     <%} else if(isApproval == false){ %>
         for (var i = 0; i < 12; i++) {
            if($("#HiddButtonValue"+ i).val() == "1" || ($("#btnWorkingCalendarDetail_"+ i).val() != "" && typeof $("#btnWorkingCalendarDetail_"+ i).val() != 'undefined'))
            {
                getCtrlById("txtInitialDate").prop('readonly', true);
                return;
            }
        }
        getCtrlById("txtInitialDate").prop('readonly', false);

     <%} %>
       
    }

</script>
    <style>
       .modalDetailWorkingCalendarEntry .modal-body{
            overflow-y : visible;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%= GetMessage()%>

<%-- Content --%>
<div class="well well-sm">
    <!--Collapse Button-->
    <div class="row">

        <%--CalendarCD--%>
        <div class="col-md-2">
            <div class='form-group <% =GetClassError("txtCalendarCD")%>'>
                <label class="control-label" for="<%= txtCalendarCD.ClientID %>">
                    コード<strong class="text-danger">*</strong></label>
                <oms:ICodeTextBox ID="txtCalendarCD" CodeType="Numeric" AllowChars="-" runat="server" CssClass="form-control input-sm"></oms:ICodeTextBox>
            </div>
        </div>
            
        <%--CalendarName--%>
        <div class="col-md-4">
            <div class='form-group <% =GetClassError("txtCalendarName")%>'>
                <label class="control-label" for="<%= txtCalendarName.ClientID %>">
                    名称<strong class="text-danger">*</strong></label>
                <oms:ITextBox ID="txtCalendarName" runat="server" CssClass="form-control input-sm"></oms:ITextBox>
            </div>
        </div>
        <div class="col-md-3">
            <div class='form-group <% =GetClassError("txtInitialDate")%>'>
                <label class="control-label" for="<%= txtInitialDate.ClientID %>">
                    起算日<strong class="text-danger">*</strong></label>

                <div class="input-group date">
                    <oms:IDateTextBox ID="txtInitialDate" runat="server" CssClass="form-control input-sm" PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
                    
            </div>
        </div>
    </div>
</div>

<div class="hidden">
 <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default btn-sm loading"
    OnClick="btnSearch_Click">
    <span class="glyphicon glyphicon-search"></span>&nbsp;Search
</asp:LinkButton>
</div>

<div class="well well-sm">
    <div class="row">
        <!-- Left Button Panel -->
        <div class="col-md-6 col-sm-7 col-xs-7">
            <div class="btn-group btn-group-justified">

            <%
                if (this.Mode == OMS.Utilities.Mode.View || this.Mode == OMS.Utilities.Mode.Delete)
                {
            %>
                <div class="btn-group">
                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm loading"  OnClick="btnEdit_Click">
                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;編集
                    </asp:LinkButton>
                </div>
                <%
                  if (!this.isOnlyPaidLeave)
                  {
                  %>
                <div class="btn-group">
                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-default btn-sm loading"
                        OnClick="btnDelete_Click">
                                <span class="glyphicon glyphicon-trash"></span>&nbsp; 削除
                    </asp:LinkButton>
                </div>
                <div class="btn-group">
                    <%if (base._authority.IsWorkCalendarExportExcel)
                      {%>
                        <button id="BtnExcelTop" type="button" class="btn btn-sm btn-default text-left" onclick="showmodalDownLoad();">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</button>                                              
                    <%}
                      else
                      { %>
                        <button id="BtnExcelTop" disabled type="button" class="btn btn-sm btn-default text-left" onclick="showmodalDownLoad();">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</button>    
                    <%} %>


                                      
                </div>
                <%} %>
            <%
                }
                else if (this.Mode == OMS.Utilities.Mode.Update)
                {                                
             %>
            <div class="btn-group">
                <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm loading"
                OnClick="btnUpdate_Click">
                    <span class="glyphicon glyphicon-ok"></span>&nbsp;登録
                </asp:LinkButton>
            </div>
            <%
            if (this._authority.IsWorkCalendarEdit)
            {
            %>
            <div class="btn-group">
                <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-default btn-sm loading"
                    OnClick="btnClear_Click">
                    <span class="glyphicon glyphicon-refresh"></span>&nbsp;クリア
                </asp:LinkButton>
           </div>
           <%
                }
            %>
           <%
                }
                else if (this.Mode == OMS.Utilities.Mode.Insert)
                {                                
            %>
            <div class="btn-group">
                <asp:LinkButton ID="btnInsert" runat="server" CssClass="btn btn-primary btn-sm loading"
                    OnClick="btnInsert_Click">
                    <span class="glyphicon glyphicon-ok"></span>&nbsp;登録
                </asp:LinkButton>
            </div>
            <div class="btn-group">
                <asp:LinkButton ID="btnClearInsert" runat="server" CssClass="btn btn-default btn-sm loading"
                    OnClick="btnClear_Click">
                    <span class="glyphicon glyphicon-refresh"></span>&nbsp;クリア
                </asp:LinkButton>
           </div>
            <%
                }
            %>
            
            <%
                if (this.Mode == OMS.Utilities.Mode.View || this.Mode == OMS.Utilities.Mode.Insert)
                {
            %>
            <div class="btn-group">
                <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-default btn-sm loading"
                    PostBackUrl="~/WorkingCalendar/FrmWorkingCalendarList.aspx">
                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                </asp:LinkButton>
            </div>
            
            <%
                }
                else
                {
            %>
            <div class="btn-group">
                <asp:LinkButton ID="btnBackView" runat="server" CssClass="btn btn-default btn-sm loading"
                      OnClick="btnBack_Click">
                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                </asp:LinkButton>
            </div>
            <%
                }
            %>
            </div>
        </div>
        <div class="col-md-4 col-sm-1 col-xs-1">
            <div class="btn-group btn-group-justified">
            </div>
        </div>
        <!-- Right Button Panel -->
        <div class="col-md-2 col-sm-4 col-xs-4  text-right">
            <div class="btn-group btn-group-justified">
            <%
                if ((this.Mode == OMS.Utilities.Mode.View || this.Mode == OMS.Utilities.Mode.Insert || this.Mode == OMS.Utilities.Mode.Update) && !this.isOnlyPaidLeave)
                {
            %>
                <div id="divModelUser" class="btn-group" runat="server">
                    <button id="btnModelUser" type="button" class="btn btn-sm btn-default text-left" onclick="showModalUser();">利用者設定</button>
                </div>
            <%
                }
            %>
            </div>
        </div>
    </div>
</div>

<!-- Hidden save current button -->
<asp:HiddenField ID="CurrentButton" runat="server" />
<%if (txtInitialDate.Text != "")
  { %>
 <div id= "HeaderCalendarWorking" class ="container">
    <div class="row">
        <div class = "col-md-2 col-sm-4 col-xs-6">
            <div class = "row">
                <div class ="col-md-2 col-sm-2 col-xs-2 ">
                    <div class="bg-work-nomal">
                    </div>
                </div>
                <div class ="col-md-10 col-sm-10 col-xs-10">出勤日</div>
            </div>
        </div>
        <div class = "col-md-2 col-sm-4 col-xs-6">
            <div class = "row">
                <div class = "col-md-2 col-sm-2 col-xs-2">
                    <div class= "bg-work-weekend-saturday">
                    </div>
                </div>
                <div class = "col-md-10 col-sm-10 col-xs-10">出勤日（土）</div>
            </div>
        </div>
        <div class = "col-md-2 col-sm-4 col-xs-6">
            <div class = "row">
                <div class = "col-md-2 col-sm-2 col-xs-2">
                    <div class ="bg-work-holiday"></div>
                </div>
                <div class = "col-md-10 col-sm-10 col-xs-10">出勤日（祝）</div>
            </div>
        </div>
        <div class = "col-md-2 col-sm-4 col-xs-6">
            <div class = "row">
                <div class = "col-md-2 col-sm-2 col-xs-2">
                    <div class="bg-work-weekend"></div>
                </div>
                <div class = "col-md-10 col-sm-10 col-xs-10">出勤日（日）</div> 
            </div>
        </div>
        <div class = "col-md-2 col-sm-4 col-xs-6">
            <div class = "row">
                <div class = "col-md-2 col-sm-2 col-xs-2 ">
                    <div class ="bg-checkbox-ivent"></div>
                </div>
                <div class = "col-md-10 col-sm-10 col-xs-10">月例朝礼日</div>
            </div>
        </div>              
    </div>
    <div class="row">
         <div class = "col-md-2 col-sm-4 col-xs-6">
                <div class = "row">
                    <div class = "col-md-2 col-sm-2 col-xs-2 ">
                        <div class ="bg-attendance-off"></div>
                    </div>
                    <div class = "col-md-10 col-sm-10 col-xs-10">指定有休取得日</div>
                </div>
         </div> 
        <div class = "col-md-2 col-sm-4 col-xs-6">
            <div class = "row">
                <div class = "col-md-2 col-sm-2 col-xs-2">
                    <div class ="bg-attendance"></div>
                </div>
                <div class = "col-md-10 col-sm-10 col-xs-10">所定休日</div>
            </div>
        </div>
        <div class = "col-md-2 col-sm-4 col-xs-6">
            <div class = "row">
                <div class = "col-md-2 col-sm-2 col-xs-2 ">
                    <div class ="bg-legal-holiday"></div>
                </div>
                <div class = "col-md-10 col-sm-10 col-xs-10">祝日</div>
            </div>
        </div>
        <div class = "col-md-2 col-sm-4 col-xs-6">
            <div class = "row">
                <div class = "col-md-2 col-sm-2 col-xs-2 ">
                    <div class ="bg-legal"></div>
                </div>
                <div class = "col-md-10 col-sm-10 col-xs-10">日曜日</div>
            </div>
        </div>
        <div class = "col-md-2 col-sm-4 col-xs-6">
            <div class = "row">
                <div class = "col-md-2 col-sm-2 col-xs-2 ">
                    <div class ="bg-paid-leave"></div>
                </div>
                <div class = "col-md-10 col-sm-10 col-xs-10">有給休暇予定日</div>
            </div>
        </div>                                  
    </div>
    <div class="row">
        <br />
    </div>
</div>
<%} %>

<div id ="CalendarWorking">
    <asp:Repeater ID="rptWorkingCalendarHeader" OnItemDataBound="rptWorkingCalendarHeader_ItemDataBound" runat="server">
        <ItemTemplate>
            <%# Eval("HtmlHeader")%>
            <div class="col-md-4">
                <table id="table_<%#Eval("Index")%>" class="table table-bordered">
                    <caption class="text-center">
                        <table style="width: 100%">
                            <td class="text-center" style="width: 20%;">
                                <span class="text-primary"><b><%# Eval("IndexTable")%></b></span>

                            </td>
                            <td class = "text-dark">
                                <span><%#Eval("InitialDate")%></span>
                            </td>
                            <td class="text-right">
                                <div class="btn-group"> 
                                    <%
                                        if (this.Mode == OMS.Utilities.Mode.View)
                                        {
                                    %>                        
                                        <button id="Button2" type="button" value = "<%=this.Mode%>" class="btn btn-sm btn-success btnPaidLeave" onclick="showModal('<%#Eval("FromDate")%>' , '<%#Eval("ToDate")%>', '<%#Eval("HiddenApproval")%>', this, true)"> 有休</button>
                                    <%} %>
                                    <%else
                                        { %>
                                        <button id="Button3" type="button" class="btn btn-sm btn-success btnPaidLeave" onclick="showModal('<%#Eval("FromDate")%>' , '<%#Eval("ToDate")%>', '<%#Eval("HiddenApproval")%>', this, true)"> 有休</button>
                                            
                                    <%} %>
                                </div>
                                <%
                                if (!this.isOnlyPaidLeave)
                                {
                                %>  
                                <div class="btn-group">     
                                    <%
                                    if (this.Mode == OMS.Utilities.Mode.View || (this.Mode == OMS.Utilities.Mode.Update && !base._authority.IsWorkCalendarEdit))
                                        {
                                    %>                        
                                        <button id='<%#Eval("ButtonID") %>' type="button" value = "<%=OMS.Utilities.Mode.View%>" class="btn btn-sm <%#Eval("StyleButton") %> " onclick="showModal('<%#Eval("FromDate")%>' , '<%#Eval("ToDate")%>', '<%#Eval("HiddenApproval")%>', this, false)"> <%#Eval("Text")%></button>
                                    <%} %>
                                    <%else
                                        { %>
                                        <button id='<%#Eval("ButtonID") %>' type="button" class="btn btn-sm <%#Eval("StyleButton") %> " onclick="showModal('<%#Eval("FromDate")%>' , '<%#Eval("ToDate")%>', '<%#Eval("HiddenApproval")%>', this, false)"> <%#Eval("Text")%></button>
                                            
                                    <%} %>
                                </div>
                                <%} %>
                                <%#Eval("HiddenButton")%>
                            </td>
                        </table>
                            
                        </caption>
                        <thead>
                        <tr class ="table-header">
                            <th class="text-center text-color-holiday">日</th>
                            <th class="text-center">月</th> 
                            <th class="text-center">火</th>
                            <th class="text-center">水</th>
                            <th class="text-center">木</th>
                            <th class="text-center">金</th>
                            <th class="text-center text-color-saturday">土</th>
                        </tr>
                        <tbody style="text-align:center">
                            <asp:Repeater ID="rptWorkingCalendarContent" runat="server">
                                <ItemTemplate>
                                    <%# Eval("HtmlText")%>
                                </ItemTemplate>
                            </asp:Repeater>
                        <tbody>
                    </thead>
                </table>
            </div>
            <%# Eval("HtmlBottom")%>
        </ItemTemplate>
    </asp:Repeater>
</div>

<div id = "tableTotal" class = "hidden">
    <table class="table table-bordered hidden-xs hidden-sm">
        <thead class ="table-header">
        <tr>
            <th></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex1" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex2" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex3" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex4" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex5" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex6" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex7" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex8" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex9" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex10" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex11" runat="server"></asp:Label></th>
            <th class = "text-center"><asp:Label ID="txtMonthIndex12" runat="server"></asp:Label></th>
            <th class = "text-center">合計</th>
        </tr>
        </thead>
        <tbody>
        
        <tr style="text-align: center">
            <th>総日数</th>
            <td><asp:Label ID="txtCountDayInMonth1" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth2" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth3" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth4" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth5" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth6" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth7" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth8" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth9" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth10" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth11" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonth12" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtCountDayInMonthTotal" runat="server"></asp:Label></td>
        </tr>
        <tr style="text-align: center">
            <th>休日日数</th>
            <td><asp:Label ID="txtHolidayInMonth1" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth2" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth3" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth4" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth5" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth6" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth7" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth8" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth9" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth10" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth11" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth12" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonthTotal" runat="server"></asp:Label></td>
            <input id="HiddenHolidayInMonthTotal" name="HiddenHolidayInMonthTotal" type="hidden">
        </tr>
        <tr style="text-align: center">
            <th>労働日数</th>
            <td><asp:Label ID="txtWorkingDate1" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate2" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate3" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate4" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate5" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate6" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate7" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate8" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate9" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate10" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate11" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate12" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDateTotal" runat="server"></asp:Label></td>
            <input id="HiddenWorkingDateTotal" name="HiddenWorkingDateTotal" type="hidden">
        </tr>
       
        <tr style="text-align: center">
            <th>労働時間</th>
            <td><asp:Label ID="txtWorkingTimeInMonth1" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth2" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth3" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth4" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth5" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth6" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth7" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth8" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth9" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth10" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth11" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth12" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonthTotal" runat="server"></asp:Label></td>
            <input id="HiddenWorkingTimeInMonthTotal" name="HiddenWorkingTimeInMonthTotal" type="hidden">

        </tr>
        <tr style="text-align: center">
            <th>週平均</th>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage1" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage2" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage3" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage4" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage5" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage6" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage7" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage8" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage9" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage10" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage11" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage12" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverageTotal" runat="server"></asp:Label></td>
            <input id="HiddenWorkingTimeWeeklyAverageTotal" name="HiddenWorkingTimeWeeklyAverageTotal" type="hidden">
        </tr>
        </tbody>
    </table>
    
    <!--Table for xs-->
    <table class="table table-bordered visible-xs visible-sm">
        <thead class ="table-header">
        <tr>
            <th></th>
            <th class="text-center">総日数</th>
            <th class="text-center">休日日数</th>
            <th class="text-center">労働日数</th>
            <th class="text-center">労働時間</th>
            <th class="text-center">週平均</th>
        </tr>
        </thead>
        <tbody>
        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex1_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth1_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth1_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate1_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth1_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage1_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex2_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth2_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth2_xs" runat="server"></asp:Label></td>     
            <td><asp:Label ID="txtWorkingDate2_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth2_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage2_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex3_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth3_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth3_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate3_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth3_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage3_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex4_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth4_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth4_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate4_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth4_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage4_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex5_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth5_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth5_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate5_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth5_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage5_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex6_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth6_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth6_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate6_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth6_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage6_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex7_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth7_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth7_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate7_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth7_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage7_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex8_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth8_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth8_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate8_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth8_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage8_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex9_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth9_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth9_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate9_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth9_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage9_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex10_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth10_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth10_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate10_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth10_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage10_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex11_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth11_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth11_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate11_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth11_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage11_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center"><asp:Label ID="txtMonthIndex12_xs" runat="server"></asp:Label></th>
            <td><asp:Label ID="txtCountDayInMonth12_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonth12_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDate12_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonth12_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverage12_xs" runat="server"></asp:Label></td>
        </tr>

        <tr class="text-center">
            <th class = "text-center">合計</th>
            <td><asp:Label ID="txtCountDayInMonthTotal_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtHolidayInMonthTotal_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingDateTotal_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeInMonthTotal_xs" runat="server"></asp:Label></td>
            <td><asp:Label ID="txtWorkingTimeWeeklyAverageTotal_xs" runat="server"></asp:Label></td>
        </tr>
        </tbody>
    </table>
</div>
    <div id="itemDate" style="display:none;">
      <asp:DropDownList ID="cmbWorkingSystemData" runat="server" CssClass="form-control input-sm cmbWorkingSystem"></asp:DropDownList>
      <input style ="text-align: right;" maxlength="2" id="WokingDateId" class ="form-control input-sm WorkingDateDetail" type="text">
      <input type="checkbox" id="CheckboxDetail" runat="server"/>  
    </div>

    <!-- Modal -->
      <div class="modalDetail modal fade " id="myModal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
    
          <!-- Modal content-->
          <div class="modal-content">
            <div class="modal-header">
              <button type="button" class="close" data-dismiss="modal">&times;</button>
              <h4 class="modal-title">
                 <span id="myModalTitle" class="label label-default">勤務体系カレンダー登録（詳細）</span>
              </h4>
            </div>
            <div class="modal-body">
             <div class="row">
                    <div class="col-md-12">
                        <table class="table table-bordered">
                            <caption id="capHeaderId" class="text-center"></caption>
                            <thead>
                                <tr>
                                    <th style="text-align: center; width: 20%;">日付</th>
                                    <th colspan="2" style="text-align: center;">勤務体系</th> 
                                    <th id="myModalCbxHeader" style="text-align: center; width: 20%;">月例朝礼</th>
                                </tr>
                            </thead>
                            <tbody id ='contentKimmu'>
                                   
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        
            <div class="modal-footer">
                <div class = "row">
                    <div class="col-md-2 col-sm-2 col-xs-4 text-center">
                        <div class="my-checkbox-area">労使協定</div>
                    </div>
                    <div class="col-md-4 col-sm-3 col-xs-3 text-left">
                        <div class="my-checkbox-area">
                            <%if (base._authority.IsWorkCalendarAgreementSetting)
                              { %>
                                <input type="checkbox" id="myCheckBoxFlag" class="myCheckBoxFlag" runat="server" data-on-color="success"
                                data-off-color="danger" data-size="mini" />  
                            <%
                              }
                              else
                              {           
                            %>
                                <input type="checkbox" disabled id="myCheckBoxFlagDisable" class="myCheckBoxFlag" runat="server" data-on-color="success"
                                data-off-color="danger" data-size="mini" />
                            <%} %>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-7 col-xs-5 text-right">   
                        <input type="hidden" id="myModalType" />
                        <button type="button" id="modalSubmit" class="btn btn-primary btn-sm loading" data-dismiss ="modal">OK</button>
                        <button type="button" class="btn btn-default btn-sm loading" data-dismiss="modal">キャンセル</button>    
                    </div>
                </div>
            </div>        
          </div>
        </div>
     </div>

     <!-- ModalUser -->
      <div class="modalDetail modal fade " id="ModalUser" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">

          <!-- Modal content-->
          <div class="modal-content">
            <div class="modal-header">
              <button type="button" class="close" onclick="destructorTreeView();" data-dismiss="modal">&times;</button>
              <h4 class="modal-title">
                 <span class="label label-default">勤務カレンダー利用者設定</span>
              </h4>
            </div>
            <div id ="model-body-treeView" class="modal-body">
                <div style="padding-left: 15px; padding-right: 15px">
                    <div class = "row">
                        <div class = "col-md-12">
                            <div class = "row">
                                <div class="col-md-5 col-sm-5 col-xs-5" style="padding-left: 0px;padding-right: 0px;">
                                    <div class="header-table-treeview">社員一覧</div>
                                </div>
                                <div class="col-md-2 col-sm-2 col-xs-2 text-center" style="border:thin">
                                </div>
                                <div class="col-md-5 col-sm-5 col-xs-5" style="padding-left: 0px;padding-right: 0px;">
                                    <div class="header-table-treeview">利用者設定</div>
                                </div>
                              </div>
                            <div class ="row">
                                <div class="col-md-5 col-sm-5 col-xs-5 table-treeview">
                                    <div id="treeLeft"></div>
                                </div>
                                <div id ="btnChangeTreeView" class="col-md-2 col-sm-2 col-xs-2 text-center">
                                    <div class ="row">
                                        <div class = "col-md-12">
                                            <button type="button" id="btnMoveLeft" style="width: 90%; margin-bottom: 20px;" class="btn btn-default btn-sm">
                                                <span class="glyphicon glyphicon-arrow-right"></span>&nbsp;設定</button>
                                        </div>
                                    </div>
                                    <div class ="row">
                                        <div class = "col-md-12">
                                            <button type="button" id="btnMoveRight" style="width: 90%"; class="btn btn-default btn-sm">
                                                <span class="glyphicon glyphicon-arrow-left"></span>&nbsp;解除</button>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5 col-sm-5 col-xs-5 table-treeview" style="border:thin overflow-y: auto;">
                                    <div id="treeRight"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        
            <div class="modal-footer">
                <div class = "row">
                    <div class="col-md-12 col-sm-12 col-xs-12 text-right">    
                        <button type="button" id= "btnTreeViewSave" onclick ="modelTreeViewSave()" class="btn btn-primary btn-sm" data-dismiss="modal">OK</button>
                        <button type="button" id = "btnClose" onclick="destructorTreeView();" class="btn btn-default btn-sm" data-dismiss="modal">キャンセル</button>    
                    </div>
                </div>
            </div>        
          </div>
        </div>
     </div>

     <%--(Excel)--%>
   <div class="modalDetailWorkingCalendarEntry modal fade " id="modalDownLoad" tabindex="-1" role="dialog" aria-labelledby="questionTitle" aria-hidden="true" data-backdrop="static">    
        <div class="modal-dialog  modal-sm modal-vertical-centered ">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        Excel出力</h4>
                </div>
                <div class="modal-body">
                   
                    <div class="row">
                            <div class='form-group' style="margin-left: 5%;">
                                <asp:RadioButton id="rbExcel2"　Checked=true runat="server" GroupName="SearchDate"></asp:RadioButton>
                                <label class="control-label" for="<%= rbExcel1.ClientID %>" style="text-align:left;font-weight: normal">年間勤務カレンダー出力（Excel）</label>
                            </div>                                            
                     </div>     

                    <div class="row">

                        <div class='form-group' style="margin-left: 5%;">
                            <asp:RadioButton id="rbExcel1"  runat="server" GroupName="SearchDate"></asp:RadioButton>
                            <label class="control-label" for="<%= rbExcel1.ClientID %>" style="text-align:left;font-weight: normal">労働日数時間集計表出力（Excel）</label>
                        </div>                                                                     
                        <button type="button" id="btnDownload" class="btn btn-default btn-sm hide" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;Download
                        </button>  
                    </div>
  
                </div>
                <div class="modal-footer">
                     <div class="col-md-12 col-sm-12 col-xs-12 text-right">    
                        <asp:LinkButton ID="btnExel" runat="server" CssClass="btn btn-primary btn-sm loading" OnClientClick="$('#modalDownLoad').modal('hide');"  OnCommand="btnExcel_Click">
                            </span>&nbsp;OK
                         </asp:LinkButton>
                            <asp:Button ID="btnIssueNo" runat="server" Text="キャンセル" class="btn btn-default btn-sm loading" data-dismiss="modal" />
                       
                    </div>
                   
                </div>
            </div>
        </div>
    </div>
<%if (txtInitialDate.Text != "")
  { %>
<div class="well well-sm">
    <div class="row">
        <!-- Left Button Panel -->
        <div class="col-md-6 col-sm-7 col-xs-7">
            <div class="btn-group btn-group-justified">

            <%
      if (this.Mode == OMS.Utilities.Mode.View || this.Mode == OMS.Utilities.Mode.Delete)
      {
            %>
                <div class="btn-group">
                    <asp:LinkButton ID="btnEditBottom" runat="server" CssClass="btn btn-default btn-sm loading"  OnClick="btnEdit_Click">
                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;編集
                    </asp:LinkButton>
                </div>
                  <%
                  if (!this.isOnlyPaidLeave)
                  {
                  %>
                <div class="btn-group">
                    <asp:LinkButton ID="btnDeleteBottom" runat="server" CssClass="btn btn-default btn-sm loading"
                        OnClick="btnDelete_Click">
                                <span class="glyphicon glyphicon-trash"></span>&nbsp; 削除
                    </asp:LinkButton>
                </div>
                <div class="btn-group">
                    <%if (base._authority.IsWorkCalendarExportExcel)
                      {%>
                        <button id="BntExcelBottom" type="button" class="btn btn-sm btn-default text-left" onclick="showmodalDownLoad();">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</button>                                              
                    <%}
                      else
                      { %>
                        <button id="Button1" disabled type="button" class="btn btn-sm btn-default text-left" onclick="showmodalDownLoad();">
                            <span class="glyphicon glyphicon-cloud-download"></span>&nbsp;Excel</button>    
                    <%} %>
                   
                </div>
                 <%} %>
            <%
      }
      else if (this.Mode == OMS.Utilities.Mode.Update)
      {                                
             %>
            <div class="btn-group">
                <asp:LinkButton ID="btnUpdateBottom" runat="server" CssClass="btn btn-primary btn-sm loading"
                OnClick="btnUpdate_Click">
                    <span class="glyphicon glyphicon-ok"></span>&nbsp;登録
                </asp:LinkButton>
            </div>
            <%
            if (this._authority.IsWorkCalendarEdit)
            {
            %>
            <div class="btn-group">
                <asp:LinkButton ID="btnClearBottom" runat="server" CssClass="btn btn-default btn-sm loading"
                    OnClick="btnClear_Click">
                    <span class="glyphicon glyphicon-refresh"></span>&nbsp;クリア
                </asp:LinkButton>
           </div>
           <%
            }
            %>
           <%
      }
      else if (this.Mode == OMS.Utilities.Mode.Insert && txtInitialDate.Text != "")
      {                                
            %>
            <div class="btn-group">
                <asp:LinkButton ID="btnInsertBottom" runat="server" CssClass="btn btn-primary btn-sm loading"
                    OnClick="btnInsert_Click">
                    <span class="glyphicon glyphicon-ok"></span>&nbsp;登録
                </asp:LinkButton>
            </div>
            <div class="btn-group">
                <asp:LinkButton ID="btnClearInsertBottom" runat="server" CssClass="btn btn-default btn-sm loading"
                    OnClick="btnClear_Click">
                    <span class="glyphicon glyphicon-refresh"></span>&nbsp;クリア
                </asp:LinkButton>
           </div>
            <%
      }
            %>
            
            <%
      if (this.Mode == OMS.Utilities.Mode.View)
      {
            %>
            <div class="btn-group">
                <asp:LinkButton ID="btnBackBottom" runat="server" CssClass="btn btn-default btn-sm loading"
                    PostBackUrl="~/WorkingCalendar/FrmWorkingCalendarList.aspx">
                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                </asp:LinkButton>
            </div>
            <%
      }
            %>
            <%
      else if (this.Mode == OMS.Utilities.Mode.Insert && txtInitialDate.Text != "")
      {
            %>
            <div class="btn-group">
                <asp:LinkButton ID="btnBackInsertBottom" runat="server" CssClass="btn btn-default btn-sm loading"
                    PostBackUrl="~/WorkingCalendar/FrmWorkingCalendarList.aspx">
                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                </asp:LinkButton>
            </div>
            <%
      } 
            %>
            <%
      else if (this.Mode != OMS.Utilities.Mode.Insert)
      {
            %>
            <div class="btn-group">
                <asp:LinkButton ID="btnBackBottomEvent" runat="server" CssClass="btn btn-default btn-sm loading"
                    OnClick="btnBack_Click">
                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                </asp:LinkButton>
            </div>
            <%
      }
            %>
              </div>
            </div>
            <div class="col-md-4 col-sm-1 col-xs-1">
                <div class="btn-group btn-group-justified">
                </div>
            </div>
            <!-- Right Button Panel -->
            <div class="col-md-2 col-sm-4 col-xs-4  text-right">
                <div class="btn-group btn-group-justified">
                    <%
      if ((this.Mode == OMS.Utilities.Mode.View || this.Mode == OMS.Utilities.Mode.Insert || this.Mode == OMS.Utilities.Mode.Update) && !this.isOnlyPaidLeave)
      {
                    %>
                        <div class="btn-group">
                            <button id="btnModelUserBottom" type="button" class="btn btn-sm btn-default text-left" onclick="showModalUser();">利用者設定</button>
                        </div>
                    <%
      }
                    %>
                </div>
            </div>
    </div>
</div>
<%} %>
     <!--Save WorkingTimeSytem value from server-->
    <asp:HiddenField ID="WorkingTimeSystem" runat="server" />
    <asp:HiddenField ID="InitDateSave" runat="server" />
    <asp:HiddenField ID="treeViewSave" runat="server" />
    <asp:HiddenField ID="treeViewLeftData" runat="server" />
    <asp:HiddenField ID="hidUserID" runat="server" />
    <asp:HiddenField ID="treeViewRightData" runat="server" />
</asp:Content>
