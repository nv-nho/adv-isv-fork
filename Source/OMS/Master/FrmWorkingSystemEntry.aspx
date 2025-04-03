<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmWorkingSystemEntry.aspx.cs" Inherits="OMS.WorkingSystem.FrmWorkingSystemEntry" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Import Namespace="OMS.Utilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
    
        //**********************
        // Init
        //**********************
        $(function () {
            getCtrlById("chkStatusFlag").bootstrapSwitch();
            setFocus();
            //focusErrors();

            // check show
            if(getCtrlById("optWorkingType_0").is(':checked'))
            {
                $(".classrequire").show();
            }
            else
            {
                $(".classrequire").hide();
            }

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
            
            <%if(this.StatusDisplayBreakType == 0){ %>
                    $('.breakTypeKara').show();
                    $('.breakEnd').show();

                    $('#breakTypeKara1').text("～");
                    $('#lblTimeBreak1').text('休憩時間1');
                    
                    $('#breakType2').show();
                    $('#breakType3').show();
                    $('#breakType4').show();
            <%}else if(this.StatusDisplayBreakType == 1){ %>
                    $('#breakTypeKara1').text("～");
                    $('#lblTimeBreak1').text('休憩時間');

                    getCtrlById("timeBreak1_End").val('');
                    getCtrlById("timeBreak2_End").val('');
                    getCtrlById("timeBreak3_End").val('');
                    getCtrlById("timeBreak4_End").val('');
                    
                    getCtrlById("timeBreak2_Start").val('');
                    getCtrlById("timeBreak3_Start").val('');
                    getCtrlById("timeBreak4_Start").val('');

                    $('#breakType2').hide();
                    $('#breakType3').hide();
                    $('#breakType4').hide();

                    $('.breakTypeKara').hide();
                    $('.breakEnd').hide();
            <% } else {%>
                    $('.breakTypeKara').show();
                    $('.breakEnd').show();

                    $('#lblTimeBreak1').text('休憩時間');
                    $('#breakTypeKara1').text("毎");

                    getCtrlById("timeBreak2_Start").val('');
                    getCtrlById("timeBreak3_Start").val('');
                    getCtrlById("timeBreak4_Start").val('');

                    getCtrlById("timeBreak2_End").val('');
                    getCtrlById("timeBreak3_End").val('');
                    getCtrlById("timeBreak4_End").val('');

                    $('#breakType2').hide();
                    $('#breakType3').hide();
                    $('#breakType4').hide();
            <% } %>

            <%if(this.StatusDisplayWorking == 0){ %>
                    getCtrlById("timeWorking_Start").prop('disabled', false);
                    getCtrlById("timeWorking_End").prop('disabled', false);
                    getCtrlById("timeWorking_End_2").prop('disabled', false);
  
                    getCtrlById("timeDateSwitchTime").prop('disabled', false);
                    getCtrlById("timeFirst_End").prop('disabled', false);
                    getCtrlById("timeLatter_Start").prop('disabled', false);
                    getCtrlById("timeAllOff_Hours").prop('disabled', false);
                    getCtrlById("timeFirstOff_Hours").prop('disabled', false);
                    getCtrlById("timeLatterOff_Hours").prop('disabled', false);

            <%}else { %>
                    getCtrlById("timeWorking_Start").val('');
                    getCtrlById("timeWorking_Start").prop('disabled', true);
                    getCtrlById("timeWorking_End").val('');
                    getCtrlById("timeWorking_End").prop('disabled', true);
                    getCtrlById("timeWorking_End_2").val('');
                    getCtrlById("timeWorking_End_2").prop('disabled', true);

                    getCtrlById("timeDateSwitchTime").val('');
                    getCtrlById("timeFirst_End").val('');
                    getCtrlById("timeLatter_Start").val('');
                    getCtrlById("timeAllOff_Hours").val('');
                    getCtrlById("timeFirstOff_Hours").val('');
                    getCtrlById("timeLatterOff_Hours").val('');

                    getCtrlById("timeDateSwitchTime").prop('disabled', true);
                    getCtrlById("timeFirst_End").prop('disabled', true);
                    getCtrlById("timeLatter_Start").prop('disabled', true);
                    getCtrlById("timeAllOff_Hours").prop('disabled', true);
                    getCtrlById("timeFirstOff_Hours").prop('disabled', true);
                    getCtrlById("timeLatterOff_Hours").prop('disabled', true);

            <% } %>

            <%if(this.IsDisplayDiveOverTime == true){ %>
                    getCtrlById("overTimeId").show();
            <%}else{ %>
                    getCtrlById("overTimeId").hide();
            <%} %>
            
          
        });

        //**********************
        // Set Focus
        //**********************
        function setFocus() {
            
            <%if(this.Mode == Mode.Insert || this.Mode == Mode.Copy){ %>
                getCtrlById("txtWorkingSystemCD").focus().select();
            <%} %>
//            <%if(this.Mode == Mode.View){ %>
//                getCtrlById("txtWorkingSystemCD").focus().select();
//            <%} %>
            <%if(this.Mode == Mode.Update){ %>
                getCtrlById("txtWorkingSystemName").focus().select();
            <%} %>
        }
            
        //**********************
        // Find Back
        //**********************    
        function findBack() {
            hideLoading();
            getCtrlById("txtWorkingSystemCD").focus().select();
        }        
            
        $("[id^= MainContent_optBreakType_]").change(function(){
            var index=$(this).prop("id").split("_").pop();
            
            switch(index)
            {
                case '0':
                    $('.breakTypeKara').show();
                    $('.breakEnd').show();

                    $('#lblTimeBreak1').text('休憩時間1');
                    $('#breakTypeKara1').text("～");

                    $('#breakType2').show();
                    $('#breakType3').show();
                    $('#breakType4').show();

                    getCtrlById("timeBreak1_End").val('');
                    getCtrlById("timeBreak2_End").val('');
                    getCtrlById("timeBreak3_End").val('');
                    getCtrlById("timeBreak4_End").val('');
                    
                    getCtrlById("timeBreak1_Start").val('');
                    getCtrlById("timeBreak2_Start").val('');
                    getCtrlById("timeBreak3_Start").val('');
                    getCtrlById("timeBreak4_Start").val('');
                     break;
                case '1':
                    $('#breakTypeKara1').text("～");
                    $('#lblTimeBreak1').text('休憩時間');

                    getCtrlById("timeBreak1_End").val('');
                    getCtrlById("timeBreak2_End").val('');
                    getCtrlById("timeBreak3_End").val('');
                    getCtrlById("timeBreak4_End").val('');
                    
                    getCtrlById("timeBreak1_Start").val('');
                    getCtrlById("timeBreak2_Start").val('');
                    getCtrlById("timeBreak3_Start").val('');
                    getCtrlById("timeBreak4_Start").val('');

                    $('#breakType2').hide();
                    $('#breakType3').hide();
                    $('#breakType4').hide();

                    $('.breakTypeKara').hide();
                    $('.breakEnd').hide();
                    break;
                case '2':
                    $('.breakTypeKara').show();
                    $('.breakEnd').show();

                     $('#breakTypeKara1').text("毎");
                     $('#lblTimeBreak1').text('休憩時間');

                    getCtrlById("timeBreak1_End").val('');
                    getCtrlById("timeBreak2_End").val('');
                    getCtrlById("timeBreak3_End").val('');
                    getCtrlById("timeBreak4_End").val('');
                    
                    getCtrlById("timeBreak1_Start").val('');
                    getCtrlById("timeBreak2_Start").val('');
                    getCtrlById("timeBreak3_Start").val('');
                    getCtrlById("timeBreak4_Start").val('');

                    $('#breakType2').hide();
                    $('#breakType3').hide();
                    $('#breakType4').hide();
                    break;
            }
        });

        $("[id^= MainContent_optWorkingType_]").change(function(){
            var index=$(this).prop("id").split("_").pop();
            
            $(".workingtype-required").removeClass("has-error");
            switch(index)
            {
                case '0':
                    getCtrlById("timeWorking_Start").val('');
                    getCtrlById("timeWorking_Start").prop('disabled', false);
                    getCtrlById("timeWorking_End").val('');
                    getCtrlById("timeWorking_End").prop('disabled', false);
                    getCtrlById("timeWorking_End_2").val('');
                    getCtrlById("timeWorking_End_2").prop('disabled', false);

                    getCtrlById("timeDateSwitchTime").val('');
                    getCtrlById("timeFirst_End").val('');
                    getCtrlById("timeLatter_Start").val('');
                    getCtrlById("timeAllOff_Hours").val('');
                    getCtrlById("timeFirstOff_Hours").val('');
                    getCtrlById("timeLatterOff_Hours").val('');

                    getCtrlById("timeDateSwitchTime").prop('disabled', false);
                    getCtrlById("timeFirst_End").prop('disabled', false);
                    getCtrlById("timeLatter_Start").prop('disabled', false);
                    getCtrlById("timeAllOff_Hours").prop('disabled', false);
                    getCtrlById("timeFirstOff_Hours").prop('disabled', false);
                    getCtrlById("timeLatterOff_Hours").prop('disabled', false);
                    
                    $(".classrequire").show();
                     break;
                case '1':
                    getCtrlById("timeWorking_Start").val('');
                    getCtrlById("timeWorking_Start").prop('disabled', true);
                    getCtrlById("timeWorking_End").val('');
                    getCtrlById("timeWorking_End").prop('disabled', true);
                    getCtrlById("timeWorking_End_2").val('');
                    getCtrlById("timeWorking_End_2").prop('disabled', true);

                    getCtrlById("timeDateSwitchTime").val('');
                    getCtrlById("timeFirst_End").val('');
                    getCtrlById("timeLatter_Start").val('');
                    getCtrlById("timeAllOff_Hours").val('');
                    getCtrlById("timeFirstOff_Hours").val('');
                    getCtrlById("timeLatterOff_Hours").val('');

                    getCtrlById("timeDateSwitchTime").prop('disabled', true);
                    getCtrlById("timeFirst_End").prop('disabled', true);
                    getCtrlById("timeLatter_Start").prop('disabled', true);
                    getCtrlById("timeAllOff_Hours").prop('disabled', true);
                    getCtrlById("timeFirstOff_Hours").prop('disabled', true);
                    getCtrlById("timeLatterOff_Hours").prop('disabled', true);

                    $(".classrequire").hide();
                    break;
                case '2':
                    getCtrlById("timeWorking_Start").val('');
                    getCtrlById("timeWorking_Start").prop('disabled', true);
                    getCtrlById("timeWorking_End").val('');
                    getCtrlById("timeWorking_End").prop('disabled', true);
                    getCtrlById("timeWorking_End_2").val('');
                    getCtrlById("timeWorking_End_2").prop('disabled', true);

                    getCtrlById("timeDateSwitchTime").val('');
                    getCtrlById("timeFirst_End").val('');
                    getCtrlById("timeLatter_Start").val('');
                    getCtrlById("timeAllOff_Hours").val('');
                    getCtrlById("timeFirstOff_Hours").val('');
                    getCtrlById("timeLatterOff_Hours").val('');

                    getCtrlById("timeDateSwitchTime").prop('disabled', true);
                    getCtrlById("timeFirst_End").prop('disabled', true);
                    getCtrlById("timeLatter_Start").prop('disabled', true);
                    getCtrlById("timeAllOff_Hours").prop('disabled', true);
                    getCtrlById("timeFirstOff_Hours").prop('disabled', true);
                    getCtrlById("timeLatterOff_Hours").prop('disabled', true);

                    $(".classrequire").hide();
                    break;
            }
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class='form-group <%=GetClassError("txtWorkingSystemCD")%>'>
                        <label class="control-label" for="<%= txtWorkingSystemCD.ClientID %>">
                            コード<strong class="text-danger">*</strong></label>
                        <cc1:ICodeTextBox ID="txtWorkingSystemCD" runat="server" CodeType="Numeric" AjaxUrlMethod="GetWorkingSystem"
                            CssClass="form-control input-sm" LabelNames="txtWorkingSystemCD"></cc1:ICodeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-4 col-sm-4">
                    <div class='form-group <%=GetClassError("txtWorkingSystemName")%>'>
                        <label class="control-label" for="<%= txtWorkingSystemName.ClientID %>">
                            勤務体系名称<strong class="text-danger">*</strong></label>
                        <cc1:ITextBox ID="txtWorkingSystemName" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITextBox>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class='form-group'>
                        <label class="control-label" for="<%= txtWorkingSystemName2.ClientID %>">
                            略称
                        </label>
                        <cc1:ITextBox ID="txtWorkingSystemName2" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-6 col-sm-8">
                    <div class='form-group'>
                        <label class="control-label" for="<%= optWorkingType.ClientID %>">
                            出勤区分 <strong class="text-danger">*</strong></label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-6 col-sm-7">
                    <div class='form-group <%=GetClassError("optWorkingType")%>'>
                        <asp:RadioButtonList runat="server" Width="100%" ID="optWorkingType" name = "optWorkingType" RepeatDirection="Horizontal"
                            Style="display: inline">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-9 col-sm-10">
                <div class="col-md-3 col-sm-3">
                    <div class="form-group workingtype-required <%=GetClassError("timeWorking_Start")%>">
                        <label class="control-label" for="<%= timeWorking_Start.ClientID %>">
                            就業時間（開始）<strong class="text-danger classrequire">*</strong>
                        </label>
                        <cc1:ITimeTextBox ID="timeWorking_Start" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group workingtype-required <%=GetClassError("timeWorking_End")%>">
                        <label class="control-label" for="<%= timeWorking_End.ClientID %>">
                        就業時間（終了）<strong class="text-danger classrequire">*</strong>
                        </label>
                        <cc1:ITimeTextBox ID="timeWorking_End" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
                 <div class="col-md-3 col-sm-3">
                    <div class="form-group <%=GetClassError("timeWorking_End_2")%>">
                        <label class="control-label" for="<%= timeWorking_End_2.ClientID %>">
                        就業時間（終了）2
                        </label>
                        <cc1:ITimeTextBox ID="timeWorking_End_2" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="well well-sm " id="overTimeId" runat="server">
        <asp:Repeater ID="rptOverTime" runat="server">
            <ItemTemplate>
                <div class="row">
                    <div class="col-md-8 col-sm-9">
                        <div class="col-md-2 col-sm-3">
                            <div id="divOverTimeStart" runat="server" class="form-group">
                                <label runat="server" class="control-label">
                                    <%# Eval("Value2") %>
                                </label>
                                <cc1:ITimeTextBox ID="timeOverTimeStart" runat="server" CssClass="form-control input-sm"
                                    Text=""></cc1:ITimeTextBox>
                            </div>
                        </div>
                        <div class="col-md-1 col-sm-1">
                            <div class='form-group' style="text-align: center; padding-top: 9px;">
                                <br />
                                <label>
                                    ～</label>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-3">
                            <div id="divOverTimeEnd" runat="server" class="form-group">
                                <label class="control-label" style="padding-top: 15px;">
                                </label>
                                <cc1:ITimeTextBox ID="timeOverTimeEnd" runat="server" CssClass="form-control input-sm"
                                    Text=""></cc1:ITimeTextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class="form-group <%=GetClassError("timeDateSwitchTime")%>">
                        <label class="control-label" for="<%= timeDateSwitchTime.ClientID %>">
                            日替時刻
                        </label>
                        <cc1:ITimeTextBox ID="timeDateSwitchTime" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class="form-group workingtype-required <%=GetClassError("timeFirst_End")%>">
                        <label class="control-label" for="<%= timeFirst_End.ClientID %>" style="font-size: 13.5px;">
                            前半終了時刻<strong class="text-danger classrequire">*</strong>
                            </label>
                        <cc1:ITimeTextBox ID="timeFirst_End" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class="form-group workingtype-required <%=GetClassError("timeLatter_Start")%>">
                        <label class="control-label" for="<%= timeLatter_Start.ClientID %>" style="font-size: 13.5px;">
                            後半開始時刻<strong class="text-danger classrequire">*</strong></label>
                        <cc1:ITimeTextBox ID="timeLatter_Start" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class="form-group <%=GetClassError("timeAllOff_Hours")%>">
                        <label class="control-label" for="<%= timeAllOff_Hours.ClientID %>">
                            全休労働時間
                        </label>
                        <cc1:ITimeTextBox ID="timeAllOff_Hours" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class="form-group <%=GetClassError("timeFirstOff_Hours")%>">
                        <label class="control-label" for="<%= timeFirstOff_Hours.ClientID %>">
                            前休労働時間
                        </label>
                        <cc1:ITimeTextBox ID="timeFirstOff_Hours" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class="form-group <%=GetClassError("timeLatterOff_Hours")%>">
                        <label class="control-label" for="<%= timeLatterOff_Hours.ClientID %>">
                            後休労働時間
                        </label>
                        <cc1:ITimeTextBox ID="timeLatterOff_Hours" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-6 col-sm-7">
                    <div class='form-group'>
                        <label class="control-label" for="<%= optBreakType.ClientID %>">
                            休憩時間区分
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-6 col-sm-7">
                    <div class='form-group <%=GetClassError("optBreakType")%>'>
                        <asp:RadioButtonList runat="server" ID="optBreakType" RepeatDirection="Horizontal"
                            Style="display: inline">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="breakType1">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class='form-group <%=GetClassError("timeBreak1_Start")%>'>
                        <label id="lblTimeBreak1" class="control-label" for="<%= timeBreak1_Start.ClientID %>">
                            休憩時間1
                        </label>
                        <cc1:ITimeTextBox ID="timeBreak1_Start" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1 breakTypeKara">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label id="breakTypeKara1">
                            ～</label>
                    </div>
                </div>
                <div class="col-md-2 col-sm-3 breakEnd">
                    <div class='form-group <%=GetClassError("timeBreak1_End")%>'>
                        <label class="control-label" for="<%= timeBreak1_End.ClientID %>" style="padding-top: 15px;">
                        </label>
                        <cc1:ITimeTextBox ID="timeBreak1_End" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="breakType2">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class='form-group <%=GetClassError("timeBreak2_Start")%>'>
                        <label class="control-label" for="<%= timeBreak2_Start.ClientID %>">
                            休憩時間2
                        </label>
                        <cc1:ITimeTextBox ID="timeBreak2_Start" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1 breakTypeKara">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>
                <div class="col-md-2 col-sm-3 breakEnd">
                    <div class='form-group <%=GetClassError("timeBreak2_End")%>'>
                        <label class="control-label" for="<%= timeBreak2_End.ClientID %>" style="padding-top: 15px;">
                        </label>
                        <cc1:ITimeTextBox ID="timeBreak2_End" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="breakType3">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class='form-group <%=GetClassError("timeBreak3_Start")%>'>
                        <label class="control-label" for="<%= timeBreak3_Start.ClientID %>">
                            休憩時間3
                        </label>
                        <cc1:ITimeTextBox ID="timeBreak3_Start" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1 breakTypeKara">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>
                <div class="col-md-2 col-sm-3 breakEnd">
                    <div class='form-group <%=GetClassError("timeBreak3_End")%>'>
                        <label class="control-label" for="<%= timeBreak3_End.ClientID %>" style="padding-top: 15px;">
                        </label>
                        <cc1:ITimeTextBox ID="timeBreak3_End" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="breakType4">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-2 col-sm-3">
                    <div class='form-group <%=GetClassError("timeBreak4_Start")%>'>
                        <label class="control-label" for="<%= timeBreak4_Start.ClientID %>">
                            休憩時間4
                        </label>
                        <cc1:ITimeTextBox ID="timeBreak4_Start" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1 breakTypeKara">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>
                <div class="col-md-2 col-sm-3 breakEnd">
                    <div class='form-group <%=GetClassError("timeBreak4_End")%>'>
                        <label class="control-label" for="<%= timeBreak4_End.ClientID %>" style="padding-top: 15px;">
                        </label>
                        <cc1:ITimeTextBox ID="timeBreak4_End" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITimeTextBox>
                    </div>
                </div>
            </div>
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
        <div class="col-md-6 col-sm-7">
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
                            PostBackUrl="~/Master/FrmWorkingSystemList.aspx">
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
        <div class="col-md-6 col-sm-7">
            <div class="well well-sm">
                <div class="row">
                    <!-- New Buton -->
                    <div class="col-md-6 col-sm-7">
                        <div class="btn-group btn-group-justified">
                            <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmWorkingSystemEntry.aspx"
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
