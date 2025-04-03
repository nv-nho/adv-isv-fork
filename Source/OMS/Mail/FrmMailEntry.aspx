<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="FrmMailEntry.aspx.cs" Inherits="OMS.Mail.FrmMailEntry" %>
<%--Import Using--%>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Import Namespace="OMS.Utilities" %>
<%--Java Script--%>
<asp:Content ID="ctHeader" ContentPlaceHolderID="HeadContent" runat="server">
     <script type="text/javascript">

         $(function () {
            <%if(this.Success == true){ %>
                showSuccess();
                 setTimeout(function() {
                     hideSuccess();
                }, 1500 );
            <%} %>

             //show confirm email before sending
            <%if(this.IsShowQuestion == true && !this.IsShowDialog){ %>
                    $('#ModalConfirmEmail').modal('show');
                    //init value modal
                    $('#ModalConfirmEmail').on('shown.bs.modal', function (e) {
                        let subject = $('#<%= this.txtSubject.ClientID%>').val();
                        let reDate = $('#<%= this.txtReplyDueDate.ClientID%>').val();
                        let content = $('#<%= this.txtContent.ClientID%>').val();
                        $('#<%= this.txtCfSubject.ClientID%>').val(subject);
                        $('#<%= this.txtCfReplyDueDate.ClientID%>').val(reDate);
                        $('#<%= this.txtCfContent.ClientID%>').val(content);
                        $('#<%= this.txtCfFileNm1.ClientID%>').val(getCtrlById("hdnFileUpload1").val());
                        $('#<%= this.txtCfFileNm2.ClientID%>').val(getCtrlById("hdnFileUpload2").val());
                        $('#<%= this.txtCfFileNm3.ClientID%>').val(getCtrlById("hdnFileUpload3").val());
                        $('#lstUserSelected').clone().prependTo('#lstUserModalConfirm');
                    });
            <%}else if(this.IsShowQuestion == true && this.IsShowDialog) {%>
                $('#modalQuestion').modal('show');
                $('#modalQuestion').on('shown.bs.modal', function (e) {
                    $('<%=this.DefaultButton%>').focus();
                });
            <%}%>
         });
         $(document).ready(function () {

             <% if (this.Mode != OMS.Utilities.Mode.View)
         {%>
             if ($('#<%= txtFile1.ClientID%>').val() == '' && getCtrlById("hdnFileUpload1").val() == '') {
                 $('#<%= btnRemove1.ClientID%>').prop('disabled', true);
             } else {
                 $('#<%= btnRemove1.ClientID%>').prop('disabled', false);
             }
             if ($('#<%= txtFile2.ClientID%>').val() == '' && getCtrlById("hdnFileUpload2").val() == '') {
                 $('#<%= btnRemove2.ClientID%>').prop('disabled', true);
             } else {
                 $('#<%= btnRemove2.ClientID%>').prop('disabled', false);
             }
             if ($('#<%= txtFile3.ClientID%>').val() == '' && getCtrlById("hdnFileUpload3").val() == '') {
                 $('#<%= btnRemove3.ClientID%>').prop('disabled', true);
             } else {
                 $('#<%= btnRemove3.ClientID%>').prop('disabled', false);
             }
             <%}%>
                //lock control
             if ($("#panelError").val() === undefined || $('#<%= chkResendFlag.ClientID %>').prop('checked') == false) {
                getCtrlById("cmbBaseDivision").prop('disabled', true);
                getCtrlById("cmbStartDivision").prop('disabled', true);
                getCtrlById("txtStartDate").prop('disabled', true);
                getCtrlById("cmbEndDivison").prop('disabled', true);
                getCtrlById("txtEndDate").prop('disabled', true);
                getCtrlById("txtResendInterval").prop('disabled', true);
                getCtrlById("txtResendTime").prop('disabled', true);
                 //defaut selected
                 if (getCtrlById("cmbBaseDivision").prop('selectedIndex') == 0) {
                     getCtrlById("cmbStartDivision").prop('selectedIndex', 1);
                     getCtrlById("cmbEndDivison").prop('selectedIndex', 1);
                 }
                 //update combobox date
                updateCbDate();
             }
             <%if (this.Mode == OMS.Utilities.Mode.Insert || this.Mode == OMS.Utilities.Mode.Update)
         {%>
                     if ($('#<%= chkResendFlag.ClientID %>').prop('checked') == true) {
                         if (getCtrlById("cmbBaseDivision").prop('selectedIndex') == 0) {
                             getCtrlById("cmbStartDivision").prop('selectedIndex', 1);
                             getCtrlById("cmbEndDivison").prop('selectedIndex', 1);
                             getCtrlById("cmbStartDivision").prop('disabled', true);
                             getCtrlById("cmbEndDivison").prop('disabled', true);
                         } else {
                             getCtrlById("cmbStartDivision").prop('disabled', false);
                             getCtrlById("cmbEndDivison").prop('disabled', false);
                             updateCbDate();
                         }
                         getCtrlById("cmbBaseDivision").prop('disabled', false);
                         getCtrlById("txtStartDate").prop('disabled', false);
                         getCtrlById("txtEndDate").prop('disabled', false);
                         getCtrlById("txtResendInterval").prop('disabled', false);
                         getCtrlById("txtResendTime").prop('disabled', false);
                     } else {
                        getCtrlById("cmbBaseDivision").prop('disabled', true);
                        getCtrlById("cmbStartDivision").prop('disabled', true);
                        getCtrlById("txtStartDate").prop('disabled', true);
                        getCtrlById("cmbEndDivison").prop('disabled', true);
                        getCtrlById("txtEndDate").prop('disabled', true);
                        getCtrlById("txtResendInterval").prop('disabled', true);
                        getCtrlById("txtResendTime").prop('disabled', true);
                        getCtrlById("txtStartDate").val('');
                        getCtrlById("txtEndDate").val('');
                        getCtrlById("txtResendInterval").val('');
                        getCtrlById("txtResendTime").val('');
                     }
              <%}
         else
         { %>
                getCtrlById("cmbBaseDivision").prop('disabled', true);
                getCtrlById("cmbStartDivision").prop('disabled', true);
                getCtrlById("txtStartDate").prop('disabled', true);
                getCtrlById("cmbEndDivison").prop('disabled', true);
                getCtrlById("txtEndDate").prop('disabled', true);
                getCtrlById("txtResendInterval").prop('disabled', true);
                getCtrlById("txtResendTime").prop('disabled', true);
             <%}%>

             $('#ModalConfirmEmail').on('hidden.bs.modal', function () {
                 if ($('#<%= chkResendFlag.ClientID %>').prop('checked') == true) {
                     if (getCtrlById("cmbBaseDivision").prop('selectedIndex') == 0) {
                         getCtrlById("cmbStartDivision").prop('selectedIndex', 1);
                         getCtrlById("cmbEndDivison").prop('selectedIndex', 1);
                         getCtrlById("cmbStartDivision").prop('disabled', true);
                         getCtrlById("cmbEndDivison").prop('disabled', true);
                     } else {
                         getCtrlById("cmbStartDivision").prop('disabled', false);
                         getCtrlById("cmbEndDivison").prop('disabled', false);
                         updateCbDate();
                     }
                     getCtrlById("cmbBaseDivision").prop('disabled', false);
                     getCtrlById("txtStartDate").prop('disabled', false);
                     getCtrlById("txtEndDate").prop('disabled', false);
                     getCtrlById("txtResendInterval").prop('disabled', false);
                     getCtrlById("txtResendTime").prop('disabled', false);
                 }
             });

             //save txtFile Upload
             if ($('#<%= txtFile1.ClientID%>').val() == '' && getCtrlById("hdnFileUpload1").val() != '') {
                 $('#<%= txtFile1.ClientID%>').val(getCtrlById("hdnFileUpload1").val());
             }
             if ($('#<%= txtFile2.ClientID%>').val() == '' && getCtrlById("hdnFileUpload2").val() != '') {
                 $('#<%= txtFile2.ClientID%>').val(getCtrlById("hdnFileUpload2").val());
             }
             if ($('#<%= txtFile3.ClientID%>').val() == '' && getCtrlById("hdnFileUpload3").val() != '') {
                 $('#<%= txtFile3.ClientID%>').val(getCtrlById("hdnFileUpload3").val());
             }
             //END

             $(window).resize(function () {
                 var bodyheight = getCtrlById("model-body-treeView").height() - 50;
                 $(".table-treeview").height(bodyheight);
                 $("#btnChangeTreeView").css("margin-top", ((bodyheight / 2) - 35) + "px");
             });

             //hidden button show email info
             var idArray = [];
             $('.spReceiveDate').each(function () {
                 idArray.push(this.id);
             });
             if (idArray.length > 0) {
                 for (let i = 0; i < idArray.length; i++) {
                     let itemBtn = 'btnDetail' + idArray[i].match(/\d+/g);
                     if ($('#' + idArray[i]).val() == '') {                         
                         $('#' + itemBtn).prop('disabled', true);
                         $('#' + itemBtn).removeClass('btn-info');
                         $('#' + itemBtn).addClass('btn-default');
                     }

                     if (getCtrlById("hdnIsMailDisplay").val() == 'False') {
                         $('#' + itemBtn).prop('disabled', true);
                     }
                 }
             }

             //validate text box  value 0 ~ 365
             $(document).on('change', '#<%= txtStartDate.ClientID%>', function (e) {
                 var reg = new RegExp('^\\d+$');
                 if ($('#<%=txtStartDate.ClientID %>').val() > 365 || $('#<%=txtStartDate.ClientID %>').val() < 0 || (!reg.test($('#<%=txtStartDate.ClientID %>').val()))) {
                     $('#<%=txtStartDate.ClientID %>').val('');
                 }
             });
            $(document).on('change', '#<%= txtEndDate.ClientID%>', function (e) {
                 var reg = new RegExp('^\\d+$');
                 if ($('#<%=txtEndDate.ClientID %>').val() > 365 || $('#<%=txtEndDate.ClientID %>').val() < 0 || (!reg.test($('#<%=txtEndDate.ClientID %>').val()))) {
                     $('#<%=txtEndDate.ClientID %>').val('');
                 }
             });

            $(document).on('change', '#<%= txtResendInterval.ClientID%>', function (e) {
                 var reg = new RegExp('^\\d+$');
                 if ($('#<%=txtResendInterval.ClientID %>').val() < 0 || (!reg.test($('#<%=txtResendInterval.ClientID %>').val()))) {
                     $('#<%=txtResendInterval.ClientID %>').val('');
                 }
             });

             //event checkbox when checked
             $(document).on('change', '#<%= chkResendFlag.ClientID%>', function (e) {
                 //unlock control
                 if (this.checked) {
                     getCtrlById("cmbBaseDivision").prop('disabled', false);
                     getCtrlById("cmbStartDivision").prop('disabled', false);
                     getCtrlById("txtStartDate").prop('disabled', false);
                     getCtrlById("cmbEndDivison").prop('disabled', false);
                     getCtrlById("txtEndDate").prop('disabled', false);
                     getCtrlById("txtResendInterval").prop('disabled', false);
                     getCtrlById("txtResendTime").prop('disabled', false);
                    if (getCtrlById("cmbBaseDivision").prop('selectedIndex') == 0) {
                        getCtrlById("cmbStartDivision").prop('selectedIndex', 1);
                        getCtrlById("cmbEndDivison").prop('selectedIndex', 1);
                        getCtrlById("cmbStartDivision").prop('disabled', true);
                        getCtrlById("cmbEndDivison").prop('disabled', true);
                     }
                     //update combobox
                     updateCbDate();
                 } else {
                     getCtrlById("cmbBaseDivision").prop('disabled', true);
                     getCtrlById("cmbStartDivision").prop('disabled', true);
                     getCtrlById("txtStartDate").prop('disabled', true);
                     getCtrlById("cmbEndDivison").prop('disabled', true);
                     getCtrlById("txtEndDate").prop('disabled', true);
                     getCtrlById("txtResendInterval").prop('disabled', true);
                     getCtrlById("txtResendTime").prop('disabled', true);
                     getCtrlById("txtStartDate").val('');
                     getCtrlById("txtEndDate").val('');
                     getCtrlById("txtResendInterval").val('');
                     getCtrlById("txtResendTime").val('');
                 }
             });

             //event when selected combobox
             $(document).on('change', '#<%= cmbBaseDivision.ClientID%>', function (e) {
                 if (getCtrlById("cmbBaseDivision").prop('selectedIndex') == 0) {
                     getCtrlById("cmbStartDivision").prop('selectedIndex', 1);
                     getCtrlById("cmbEndDivison").prop('selectedIndex', 1);
                     getCtrlById("cmbStartDivision").prop('disabled', true);
                     getCtrlById("cmbEndDivison").prop('disabled', true);
                 } else {
                     getCtrlById("cmbStartDivision").prop('selectedIndex', 0);
                     getCtrlById("cmbStartDivision").prop('disabled', false);
                     getCtrlById("cmbEndDivison").prop('disabled', false);
                 } 
             });
             //
             $(document).on('change', '#<%= cmbStartDivision.ClientID%>', function (e) {
                 updateCbDate();
             });
         });

        // data init for modal user
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
         function showModalUser() {
             //Mode new
            $("#treeRight").show();
            $("#treeRight li").show();
            $("#treeLeft").show();
            $("#treeLeft li").show();
            if(typeof(treeLeft) == "undefined" || float == 0){
                
            //    //load data treeview 
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
                    var hid = getCtrlById("hdnHID").val().trim();
                    if (hid == "" || hid === undefined) {
                       hid = 0;
                    }
                    // getdata treeLeft
                     params = {"HID" : hid};
                
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
                    ajax("GetDataTreeRight",params, function (response) {
                        var result = eval('(' + response.d + ')');
                            treeRight = $('#treeRight').tree({
                            primaryKey: 'id',
                            uiLibrary: 'bootstrap',
                            dataSource: [ {id: '-1',text: '全社共通', children: result } ],
                            checkboxes: true
                        });
                        dataRight = result;
                    });
                    //set data for hidden
                    arrayDataHiddenSave = getDataForHidden(dataRight);
                    getCtrlById("treeViewSave").val(arrayDataHiddenSave.join('|'));
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
            ////expandAll
            treeLeft.expandAll();
            treeRight.expandAll();

             $('#ModalUser').on('shown.bs.modal', function () {
                var bodyheight = getCtrlById("model-body-treeView").height() - 50;
                $(".table-treeview").height(bodyheight);
                $("#btnChangeTreeView").css("margin-top", ((bodyheight/2) - 35)  + "px");
            });
            $('#ModalUser').modal('show');
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
            arrayDataHiddenSave = getDataForHidden(dataRight);
            getCtrlById("treeViewSave").val(arrayDataHiddenSave.join('|'));

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
            arrayDataHiddenSave = getDataForHidden(dataRight);
            getCtrlById("treeViewSave").val(arrayDataHiddenSave.join('|'));

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

            treeLeft.destroy();
            treeRight.destroy();

            getCtrlById("btnShowUserSelect").click();
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
        
            treeLeft.destroy();
            treeRight.destroy();
         }

        //upload file
        function uploadFile(id)
        {
            switch (id)
            {
                case getCtrlById("btnFile1").attr("id"):
                    $('#<%= fileUpload1.ClientID%>').trigger('click');
                    var fileUpload = document.querySelector('#<%= fileUpload1.ClientID%>').files[0];
                    if (fileUpload != null) {
                        getCtrlById("hdnFileUpload1").val(fileUpload.name);
                        $('#<%= txtFile1.ClientID%>').val(fileUpload.name);
                         $('#<%= btnRemove1.ClientID%>').prop('disabled',false);
                    }
                    $(document).on('change', '#<%= fileUpload1.ClientID%>', function (e) {
                        getCtrlById("hdnFileUpload1").val(e.target.files[0].name);
                        $('#<%= txtFile1.ClientID%>').val(e.target.files[0].name);
                        if ($('#<%= txtFile1.ClientID%>').val() == '' && getCtrlById("hdnFileUpload1").val() == '') {
                             $('#<%= btnRemove1.ClientID%>').prop('disabled', true);
                         } else {
                             $('#<%= btnRemove1.ClientID%>').prop('disabled',false);
                         }
                    });
                    break;
                case getCtrlById("btnFile2").attr("id"):
                    $('#<%= fileUpload2.ClientID%>').trigger('click');
                    var fileUpload = document.querySelector('#<%= fileUpload2.ClientID%>').files[0];
                    if (fileUpload != null) {
                        $('#<%= txtFile2.ClientID%>').val(fileUpload.name);
                        getCtrlById("hdnFileUpload2").val(fileUpload.name);
                         $('#<%= btnRemove2.ClientID%>').prop('disabled',false);
                    }
                    $(document).on('change', '#<%= fileUpload2.ClientID%>', function (e) {
                        $('#<%= txtFile2.ClientID%>').val(e.target.files[0].name);
                        getCtrlById("hdnFileUpload2").val(e.target.files[0].name);

                         if ($('#<%= txtFile2.ClientID%>').val() == '' && getCtrlById("hdnFileUpload2").val() == '') {
                             $('#<%= btnRemove2.ClientID%>').prop('disabled', true);
                         } else {
                             $('#<%= btnRemove2.ClientID%>').prop('disabled',false);
                         }
                    });
                    break;
                case getCtrlById("btnFile3").attr("id"):
                    $('#<%= fileUpload3.ClientID%>').trigger('click');
                    var fileUpload = document.querySelector('#<%= fileUpload3.ClientID%>').files[0];
                    if (fileUpload != null) {
                        $('#<%= txtFile3.ClientID%>').val(fileUpload.name);
                        getCtrlById("hdnFileUpload3").val(fileUpload.name);
                        $('#<%= btnRemove3.ClientID%>').prop('disabled',false);
                    }
                    $(document).on('change', '#<%= fileUpload3.ClientID%>', function (e) {
                        $('#<%= txtFile3.ClientID%>').val(e.target.files[0].name);
                        getCtrlById("hdnFileUpload3").val(e.target.files[0].name);

                         if ($('#<%= txtFile3.ClientID%>').val() == '' && getCtrlById("hdnFileUpload3").val() == '') {
                             $('#<%= btnRemove3.ClientID%>').prop('disabled', true);
                         } else {
                             $('#<%= btnRemove3.ClientID%>').prop('disabled',false);
                         }
                    });
                    break;
            }
        }

        //download file
        function download(id){            
            $("#MainContent_hdnFileName").val(id);
            getCtrlById("btnDownload").click();
            hideLoading();
        }

        //download file
        function downloadAll(){            
            getCtrlById("btnDownloadAll").click();
            hideLoading();
        }

         //update combox Start Date, End Date
         function updateCbDate() {
             if (getCtrlById("cmbBaseDivision").prop('selectedIndex') == 1) {
                 if (getCtrlById("cmbStartDivision").prop('selectedIndex') == 1) {
                    getCtrlById("cmbEndDivison").prop('disabled', true);
                    getCtrlById("cmbEndDivison").prop('selectedIndex', 1);
                } else {
                    getCtrlById("cmbEndDivison").prop('disabled', false);
                }
            }
         }

         function showReplyStatus()
         {
            var hid = $("#MainContent_hdnHID").val();
            var params = {'HID': hid};
            var dataEmail= new Array();
            ajax("GetReplyStatus", params, function (response) {
                if (response.d) {
                    $.each(response.d, function( index, value ) {
                        var res = value.split(":");
                        $("#spanReplyStatus_" + res[0]).html(res[1]);
                        
                    });
                }
            });
         }

         //show modal Email
        function showMailInfo(id) {
            var hid = $("#MainContent_hdnHID").val();
            var params = { 'HID': hid, 'UID': id };
            var dataEmail= new Array();
            ajax("GetDataEmailUser", params, function (response) {
                if (response.d) {
                    var result = eval('(' + response.d + ')');
                    dataEmail = result;
                }
            });
            if (dataEmail != null) {
                $('#<%= txtmForm.ClientID %>').val(dataEmail.MailAddress);
                $('#<%= txtmSubject.ClientID %>').val(dataEmail.Subject);
                $('#<%= txtmContent.ClientID %>').val(dataEmail.BodyMail);
                //var strDate = dataEmail.ReceiveDate.split('(')[1].split(')')[0];
                //var dateReceive = new Date(parseInt(strDate));
               // strDate = moment(dateReceive).format('YYYY/MM/DD');
                $('#<%= txtRecevieDate.ClientID %>').val(dataEmail.ReceiveDate);
                $('#<%= txtmReplyDueDate.ClientID %>').val(dataEmail.ReplyDueDate);
                $('#<%= txtFileNm1.ClientID%>').val(dataEmail.FilePath1);
                $('#<%= txtFileNm2.ClientID%>').val(dataEmail.FilePath2);
                $('#<%= txtFileNm3.ClientID%>').val(dataEmail.FilePath3);
                if (dataEmail.FilePath1 == '') {
                    getCtrlById("btnDownloadFile1").prop('disabled', true);
                } else {
                    getCtrlById("btnDownloadFile1").prop('disabled', false);
                }
                if (dataEmail.FilePath2 == '') {
                    getCtrlById("btnDownloadFile2").prop('disabled', true);
                }else {
                    getCtrlById("btnDownloadFile2").prop('disabled', false);
                }
                if (dataEmail.FilePath3 == '') {
                    getCtrlById("btnDownloadFile3").prop('disabled', true);
                }else {
                    getCtrlById("btnDownloadFile3").prop('disabled', false);
                }
                getCtrlById("hdnUID_Receive").val(dataEmail.ID);
            }
            $('#ModalMail').modal('show');
         }

         // remove file upload
         function clearFileUpload(id) {
            switch (id) {
                case getCtrlById("btnRemove1").attr("id"):
                    $('#<%= fileUpload1.ClientID%>').val('');
                    getCtrlById("hdnFileUpload1").val('');
                    $('#<%= txtFile1.ClientID%>').val('');
                    $('#<%= btnRemove1.ClientID%>').prop('disabled', true);
                    break;
                case getCtrlById("btnRemove2").attr("id"):
                    $('#<%= fileUpload2.ClientID%>').val('');
                    getCtrlById("hdnFileUpload2").val('');
                     $('#<%= txtFile2.ClientID%>').val('');
                    $('#<%= btnRemove2.ClientID%>').prop('disabled', true);
                    break;
                case getCtrlById("btnRemove3").attr("id"):
                    $('#<%= fileUpload3.ClientID%>').val('');
                    getCtrlById("hdnFileUpload3").val('');
                    $('#<%= txtFile3.ClientID%>').val('');
                    $('#<%= btnRemove3.ClientID%>').prop('disabled', true);
                    break;
            }
         }
    </script>
 </asp:Content>
<%--Page Content--%>
<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
        <asp:HiddenField ID="hdnFileName" runat="server" />
        <asp:HiddenField ID="hdnHID" runat="server" />
        <asp:HiddenField ID="hdnUID_Receive" runat="server" />
        <asp:HiddenField ID="hdnFileUpload1" runat="server" />
        <asp:HiddenField ID="hdnFileUpload2" runat="server" />
        <asp:HiddenField ID="hdnFileUpload3" runat="server" />
        <asp:HiddenField ID="hdnFileUpload1Old" runat="server" />
        <asp:HiddenField ID="hdnFileUpload2Old" runat="server" />
        <asp:HiddenField ID="hdnFileUpload3Old" runat="server" />
        <asp:HiddenField ID="hdInValideDefault" runat="server" />
        <asp:HiddenField ID="hdnTempPath" runat="server" />
        <asp:HiddenField ID="hdnIsMailDisplay" runat="server" />
        <button type="button" id="btnDownload" class="hide" runat="server"> 
        <span class="glyphicon glyphicon-search"></span>&nbsp;Download</button>
        <button type="button" id="btnDownloadAll" class="hide" runat="server"></button>
        <button type="button" id="btnShowUserSelect" class="hide" runat="server"></button>
         <%= GetMessage()%>
         <%
            if (this.Mode == OMS.Utilities.Mode.Insert)
            {
            %>
            <div class="row">
                <div class="col-md-6">
                    <div class="well well-sm">
                         <div class="btn-group btn-group-justified">
                            <div class="btn-group">
                                <asp:LinkButton ID="btnSendTop" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSend_Click">
                                    <span class="glyphicon glyphicon-send"></span>&nbsp;送信
                                </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnDraftTop" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnCreateDraft">
                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;下書き保存
                                </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-default btn-sm" OnClick="backToForm">
                                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                                </asp:LinkButton>
                            </div>
                          </div>
                    </div>
                </div>
            </div>
            <%} %>
          <%
        if (this.Mode == OMS.Utilities.Mode.View && this.DraftMail == 0)
        {
            %>
                <div class="row">
                    <div class="col-md-8">
                        <div class="well well-sm">
                                <div class="btn-group btn-group-justified">
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnEditTop" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnEdit_Click">
                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;編集
                                    </asp:LinkButton>
                                </div>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnReSendAllTop" runat="server" CssClass="btn btn-success btn-sm" OnClick="ReSendingAll_Click">
                                        <span class="glyphicon glyphicon-repeat"></span>&nbsp;再送信
                                    </asp:LinkButton>
                                </div>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnReSendNoReplyTop" runat="server" CssClass="btn btn-info btn-sm" OnClick="ReSendingNotReply_Click">
                                        <span class="glyphicon glyphicon-refresh"></span>&nbsp;未返信者のみ再送信
                                    </asp:LinkButton>
                                </div>
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnDeteleEmailTop" runat="server" CssClass="btn btn-danger btn-sm" OnClick="btnDeteleEmail_Click">
                                        <span class="glyphicon glyphicon-trash"></span>&nbsp;削除
                                    </asp:LinkButton>
                                </div>
                                <div class="btn-group">
                                    <asp:LinkButton ID="LinkButton7" runat="server" CssClass="btn btn-default btn-sm" PostBackUrl="~/Mail/FrmMailList.aspx">
                                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                                    </asp:LinkButton>
                                </div>
                                </div>
                        </div>
                    </div>
                </div>
         <%  }
           else if(this.Mode == OMS.Utilities.Mode.Update)
        { %>
            <div class="row">
                <div class="col-md-6">
                    <div class="well well-sm">
                         <div class="btn-group btn-group-justified">
                            <div class="btn-group">
                                <asp:LinkButton ID="btnUpdateEmailTop" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnUpdate_Click">
                                    <span class="glyphicon glyphicon-ok"></span>&nbsp;登録 
                                </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnCannelTop" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="backToForm">
                                    <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                                </asp:LinkButton>
                           </div>
                        </div>
                    </div>
                </div>
            </div>
        <%} %>
        <div class="well well-lg">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label" for="<%=txtSubject.ClientID %>">件名<strong class="text-danger"> *</strong></label>
                        <div class='form-group <% =GetClassError("txtSubject")%>'>
                            <cc1:ITextBox ID="txtSubject" runat="server" CssClass="form-control input-sm" MaxLength="200"></cc1:ITextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3 col-sm-4 col-xs-4">
                     <div class="form-group">
                        <label class="control-label" for="<%=txtReplyDueDate.ClientID %>">返信期限<strong class="text-danger"> *</strong></label>
                        <div class="input-group date <% =GetClassError("txtReplyDueDate")%>" >
                                <cc1:IDateTextBox ID="txtReplyDueDate" runat="server" CssClass="form-control input-sm" PickDate="true" PickFormat="YYYY/MM/DD"/>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                         </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-10">
                      <div class="form-group">
                        <label class="control-label" for="<%=txtContent.ClientID %>"> 本文</label>
                        <div class='form-group <% =GetClassError("txtContent")%>'>
                            <cc1:ITextBox ID="txtContent" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="12" MaxLength="2000" Style="font-size:14px;"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7"> 
                    <div class="form-group">
                        <label class="control-label" for="<%=btnFile1.ClientID %>">添付ファイル</label>
                        <div class='form-group <% =GetClassError("txtFile1")%>'>
                            <div class="input-group">
                                <span class="input-group-btn">
                                    <button class="btn btn-default btn-sm" id="btnFile1" type="button" runat="server"
                                        onclick="uploadFile($(this).attr('id'));">
                                        <span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Choose File...
                                    </button>
                                </span>
                                <cc1:ITextBox ID="txtFile1" runat="server" CssClass="form-control input-sm" ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                                <span class="input-group-btn">
                                    <button class="btn btn-default btn-sm" id="btnDownload1" type="button" runat="server"
                                        onclick="download('1');return false;">
                                        <span class="glyphicon glyphicon-cloud-download"></span>
                                    </button>
                                    <button class="btn btn-default btn-sm" id="btnRemove1" type="button" runat="server"
                                        onclick="clearFileUpload($(this).attr('id'));return false;">
                                        <span class="glyphicon glyphicon-remove"></span>
                                    </button>
                                </span>
                            </div>
                            <asp:FileUpload ID="fileUpload1" runat="server" Style="display: none;" />
                        </div>
                        <div class='form-group <% =GetClassError("txtFile2")%>'>
                            <div class="input-group">
                                <span class="input-group-btn">
                                    <button class="btn btn-default btn-sm" id="btnFile2" type="button" runat="server"
                                        onclick="uploadFile($(this).attr('id'));">
                                        <span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Choose File...
                                    </button>
                                </span>
                                <cc1:ITextBox ID="txtFile2" runat="server" CssClass="form-control input-sm" ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                                <span class="input-group-btn">
                                    <button class="btn btn-default btn-sm" id="btnDownload2" type="button" runat="server"
                                        onclick="download('2');return false;">
                                        <span class="glyphicon glyphicon-cloud-download"></span>
                                    </button>
                                    <button class="btn btn-default btn-sm" id="btnRemove2" type="button" runat="server"
                                        onclick="clearFileUpload($(this).attr('id'));return false;">
                                        <span class="glyphicon glyphicon-remove"></span>
                                    </button>
                                </span>
                            </div>
                            <asp:FileUpload ID="fileUpload2" runat="server" Style="display: none;"/>
                        </div>
                        <div class='form-group <% =GetClassError("txtFile3")%>'>
                            <div class="input-group">
                                <span class="input-group-btn">
                                    <button class="btn btn-default btn-sm" id="btnFile3" type="button" runat="server"
                                        onclick="uploadFile($(this).attr('id'));">
                                        <span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Choose File...
                                    </button>
                                </span>
                                <cc1:ITextBox ID="txtFile3" runat="server" CssClass="form-control input-sm" ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                                <span class="input-group-btn">
                                    <button class="btn btn-default btn-sm" id="btnDownload3" type="button" runat="server"
                                        onclick="download('3');return false;">
                                        <span class="glyphicon glyphicon-cloud-download"></span>
                                    </button>
                                    <button class="btn btn-default btn-sm" id="btnRemove3" type="button" runat="server"
                                        onclick="clearFileUpload($(this).attr('id'));return false;">
                                        <span class="glyphicon glyphicon-remove"></span>
                                    </button>
                                </span>
                            </div>
                            <asp:FileUpload ID="fileUpload3" runat="server" Style="display: none;" />
                        </div>
                    </div>
                </div>
            </div>
            <!--button selected user-->
            <div class="row">
                <div class="col-md-3">
                    <div class="btn-group">
                     <div class='form-group <% =GetClassError("btnModelUser")%>'>
                        <button type="button" class="btn btn-sm btn-success" runat="server" id="btnModelUser" onclick="showModalUser();">
                          <span class="glyphicon glyphicon-plus"></span>&nbsp;送信先選択
                        </button>
                    </div>
                </div>
                </div>
            </div>

            <!--list user sending email-->
            <%
                if (this.Mode == OMS.Utilities.Mode.Insert || this.Mode == OMS.Utilities.Mode.Update || (this.Mode == OMS.Utilities.Mode.View && this.DraftMail == 1 ))
                {%> 
                   <div class="row form-group" id="lstUserSelected">
                       <ul>
                            <asp:Repeater ID="rptUser" runat="server">
                                <ItemTemplate>
                                    <li> <span class="userSelected"><%# Server.HtmlEncode(Eval("UserCD", "{0}"))%></span> - <span> <%# Server.HtmlEncode(Eval("UserName1", "{0}"))%></span></li>
                                </ItemTemplate>
                            </asp:Repeater>
                       </ul>
                  </div>
            <%  } %>
            <!--end list user sending email-->

            <div class="row">
                <div class="col-md-8">
                  <div class="col-md-12 row">
                        <input id="chkResendFlag" type="checkbox" runat="server" data-on-color="success"
                            data-off-color="danger" data-size="mini" />                    
                        <label class="control-label" for="<%=chkResendFlag.ClientID %>">再送信設定を利用する</label>
                    </div>
                    <div class="row">
                        <div class="col-md-3 form-group">
                            <cc1:IDropdownList ID="cmbBaseDivision" runat="server" CssClass="form-control input-group"/>
                        </div>
                        <div class="col-md-9 col-xs-12 form-group" style="padding-right: 0">
                            <div class="col-md-2 col-xs-6 <% =GetClassError("txtStartDate")%>" style="display:table; padding: 0">
                                <cc1:ITextBox ID="txtStartDate" runat="server" CssClass="form-control input-sm" Style="text-align: right;"/>
                                <span style="padding-left: 5px; display: table-cell; vertical-align: middle;">日</span>
                            </div>
                            <div class="col-md-3 col-xs-6">                     
                                <cc1:IDropdownList ID="cmbStartDivision" runat="server" CssClass="form-control"/>
                            </div>
                                <label class="col-md-2 breakTypeKara col-xs-12" style="text-align: center; padding-top: 9px;">～</label>
                            <div class="col-md-2 col-xs-6 <% =GetClassError("txtEndDate")%>" style="display:table; padding: 0">
                                <cc1:ITextBox ID="txtEndDate" runat="server" CssClass="form-control input-sm" Style="text-align: right;"/>
                                <span style="padding-left: 5px; display: table-cell; vertical-align: middle;">日</span>
                            </div>
                            <div class="col-md-3 col-xs-6">
                                <cc1:IDropdownList ID="cmbEndDivison" runat="server" CssClass="form-control"/>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4" style="margin-top:5px;">
                    <div> 
                        <label class="control-label" for="<%=txtResendInterval.ClientID %>">再送間隔と再送信時開</label>
                    </div>
                    <div>
                        <div class="col-md-6 col-sm-6 col-xs-6" style="padding: 0;">
                            <div class='<% =GetClassError("txtResendInterval")%>' style="display:table">
                                <cc1:ITextBox ID="txtResendInterval" runat="server" CssClass="form-control input-sm" MaxLength="1" Style="text-align: right; display:inline; width:60%"/>
                                <span style="padding-left: 5px; display: inline; vertical-align: middle; width:40%">日間隔</span>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6" style="padding-left:20px;">
                             <div class='<% =GetClassError("txtResendTime")%>' style="display:table;">
                                <cc1:ITimeTextBox ID="txtResendTime" runat="server" CssClass="form-control input-sm" Text=""></cc1:ITimeTextBox>
                                 <span style="padding-left: 5px; display: table-cell; vertical-align: middle;">時</span>
                            </div>
                        </div>
                    </div>
                    <div style="margin-top:3px;"> 
                        <label style="color:red; font-weight:100" for="<%=txtResendInterval.ClientID %>">*毎日の場合は、0をセット</label>
                    </div>
                </div>
            </div>
        </div>
                 <%
            if (this.Mode == OMS.Utilities.Mode.Insert)
            {
            %>
            <div class="row">
                <div class="col-md-6">
                    <div class="well well-sm">
                         <div class="btn-group btn-group-justified">
                            <div class="btn-group">
                                <asp:LinkButton ID="btnSend" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSend_Click">
                                    <span class="glyphicon glyphicon-send"></span>&nbsp;送信
                                </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnDraft" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnCreateDraft">
                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;下書き保存
                                </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="LinkButton6" runat="server" CssClass="btn btn-default btn-sm" OnClick="backToForm">
                                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                                </asp:LinkButton>
                            </div>
                          </div>
                    </div>
                </div>
            </div>
            <%} %>

         <!--List user has sent email -->
        <%
    if (this.Mode == OMS.Utilities.Mode.View && this.DraftMail == 0)
    {
        %>
                <table class="table table-striped table-condensed">
                    <thead>
                        <tr>
                            <th style="vertical-align: middle;">
                                <strong>社員番号</strong>
                            </th>
                            <th style="vertical-align:middle;">
                                <strong>社員名</strong>
                            </th>
                            <th style="vertical-align:middle;">
                                <strong>返信受信日時</strong>
                            </th>
                            <th style="vertical-align:middle;">
                                <button class="btn btn-info btn-sm" onclick="showReplyStatus();" style="width:140px;" type="button">
                                    出欠状況表示
                                </button>
                            </th>
                            <th style="vertical-align:middle;">
                                <button class="btn btn-info btn-sm" id="btnDownloadAllDisp" onclick="downloadAll();return false;" style="width:140px;" type="button" runat="server">
                                    添付一括ダウンロード
                                </button>
                            </th>
                        </tr>
                    </thead>
                    <asp:Repeater ID="rptDetail" runat="server">
                    <ItemTemplate>
                       <tr>
                            <td>
                                <%# Server.HtmlEncode(Eval("UserCD", "{0}"))%>
                            </td>
                            <td>
                                <%# Server.HtmlEncode(Eval("UserName1", "{0}"))%>
                            </td>
                            <td>
                                <input type="hidden" class="spReceiveDate" id="spDate<%# Eval("ID","{0}")%>" value="<%# Eval("ReceiveDate","{0}")%>"/>
                                <%# Server.HtmlEncode(Eval("ReceiveDate", "{0}"))%>
                            </td>
                            <td>
                                <span id="spanReplyStatus_<%#Eval("ID", "{0}")%>"></span>
                            </td>
                            <td style="vertical-align:middle;">
                                <button type="button" id="btnDetail<%#Eval("ID", "{0}")%>" style="width:140px;" class="btn btn-info btn-sm loading" onclick="showMailInfo(<%# ((MailDetailInfo)Container.DataItem).ID %>)">
                                    <span class="glyphicon glyphicon-ok-circle"></span>&nbsp;表示
                                </button>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </table>
        <%  } %>

            <%
    if (this.Mode == OMS.Utilities.Mode.View && this.DraftMail == 0)
    {
        %>
            <div class="row">
                <div class="col-md-8">
                    <div class="well well-sm">
                            <div class="btn-group btn-group-justified">
                            <div class="btn-group">
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnEdit_Click">
                                    <span class="glyphicon glyphicon-pencil"></span>&nbsp;編集
                                </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnReSendAll" runat="server" CssClass="btn btn-success btn-sm" OnClick="ReSendingAll_Click">
                                    <span class="glyphicon glyphicon-repeat"></span>&nbsp;再送信
                                </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnReSendNoReply" runat="server" CssClass="btn btn-info btn-sm" OnClick="ReSendingNotReply_Click">
                                    <span class="glyphicon glyphicon-refresh"></span>&nbsp;未返信者のみ再送信
                                </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnDeteleEmail" runat="server" CssClass="btn btn-danger btn-sm" OnClick="btnDeteleEmail_Click">
                                    <span class="glyphicon glyphicon-trash"></span>&nbsp;削除
                                </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-default btn-sm" PostBackUrl="~/Mail/FrmMailList.aspx">
                                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                                </asp:LinkButton>
                            </div>
                            </div>
                    </div>
                </div>
            </div>
     <%  }
       else if(this.Mode == OMS.Utilities.Mode.Update)
    { %>
        <div class="row">
            <div class="col-md-6">
                <div class="well well-sm">
                     <div class="btn-group btn-group-justified">
                        <div class="btn-group">
                            <asp:LinkButton ID="btnUpdateEmail" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnUpdate_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;登録 
                            </asp:LinkButton>
                        </div>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnCannel" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="backToForm">
                                <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                            </asp:LinkButton>
                       </div>
                    </div>
                </div>
            </div>
        </div>
    <%} %>

      <!-- ModalUser -->
      <div class="modalDetail modal fade " id="ModalUser" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">

          <!-- Modal content-->
          <div class="modal-content">
            <div class="modal-header">
              <button type="button" class="close" onclick="destructorTreeView();" data-dismiss="modal">&times;</button>
              <h4 class="modal-title">
                 <span class="label label-default">送信先選択</span>
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
                                                <span class="glyphicon glyphicon-arrow-right"></span>&nbsp;選択</button>
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

     <!-- Modal Mail Info -->
    <div class="modalDetail modal fade " id="ModalMail" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                  <h4 class="modal-title">
                     <span class="label label-default">内容確認</span>
                  </h4>
                </div>
                <div id ="model-body" class="modal-body">
                    <div style="padding-left: 15px; padding-right: 15px">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label" for="<%=txtmForm.ClientID %>">氏名</label>
                                            <cc1:ITextBox ID="txtmForm" runat="server" CssClass="form-control input-sm" ReadOnly="true"></cc1:ITextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                 <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="control-label" for="<%=txtmSubject.ClientID %>">件名</label>
                                            <cc1:ITextBox ID="txtmSubject" runat="server" CssClass="form-control input-sm" ReadOnly="true"></cc1:ITextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3" >
                                        <div class="form-group">
                                             <label class="control-label" for="<%=txtReplyDueDate.ClientID %>">返信期限</label>
                                            <cc1:IDateTextBox ID="txtmReplyDueDate" runat="server" CssClass="form-control input-sm" PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD" ReadOnly="true"/>
                                        </div>
                                    </div>
                                    <div class="col-md-5">
                                        <div class="form-group" >
                                            <label class="control-label" for="<%=txtReplyDueDate.ClientID %>">受信日</label>
                                            <cc1:IDateTextBox ID="txtRecevieDate" runat="server" CssClass="form-control input-sm" PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD" ReadOnly="true"/>
                                        </div>
                                     </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12" >
                                        <div class="form-group">
                                            <label class="control-label" for="<%=txtContent.ClientID %>"> 本文</label>
                                            <cc1:ITextBox ID="txtmContent" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="14" MaxLength="2000" Style="font-size:14px;" ReadOnly="true"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                     <div class="col-md-8" >
                                    <label class="control-label" for="<%=txtFileNm1.ClientID %>">添付ファイル</label>
                                    <div class="input-group form-group">
                                        <cc1:ITextBox ID="txtFileNm1" runat="server" CssClass="form-control input-sm"
                                            ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                                        <span class="input-group-btn">
                                            <button class="btn btn-default btn-sm" id="btnDownloadFile1" type="button" runat="server"
                                                onclick="download('4');return false;">
                                                <span class="glyphicon glyphicon-cloud-download"></span>
                                            </button>
                                        </span>
                                    </div>
                                    <div class="input-group form-group">
                                        <cc1:ITextBox ID="txtFileNm2" runat="server" CssClass="form-control input-sm"
                                            ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                                        <span class="input-group-btn">
                                            <button class="btn btn-default btn-sm" id="btnDownloadFile2" type="button" runat="server"
                                                onclick="download('5');return false;">
                                                <span class="glyphicon glyphicon-cloud-download"></span>
                                            </button>
                                        </span>
                                    </div>
                                    <div class="input-group form-group">
                                        <cc1:ITextBox ID="txtFileNm3" runat="server" CssClass="form-control input-sm"
                                            ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                                        <span class="input-group-btn">
                                            <button class="btn btn-default btn-sm" id="btnDownloadFile3" type="button" runat="server"
                                                onclick="download('6');return false;">
                                                <span class="glyphicon glyphicon-cloud-download"></span>
                                            </button>
                                        </span>
                                    </div>
                                         </div>
                                </div>
                            </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class = "row">
                        <div class="col-md-12 col-sm-12 col-xs-12 text-right">    
                            <button type="button" id = "btnCloseEmail" class="btn btn-default btn-sm" data-dismiss="modal" onclick="hideLoading()">キャンセル</button>    
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>  
    <!--End Modal Mail--> 

    <!--Begin Modal Question Mail-->
        <div class="modalDetail modal fade " id="ModalConfirmEmail" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                  <h4 class="modal-title">
                      <span class='glyphicon glyphicon-question-sign'></span> 確認
                  </h4>
                </div>
                <div id ="model-body-confirm" class="modal-body">
                    <div style="padding-left: 15px; padding-right: 15px">
                        <div class = "row">
                            <div class ="col-md-12">
                                <div class="row">
                                    <div class="col-md-12">
                                         <div class="form-group">
                                             <label class="control-label" for="<%=txtCfSubject.ClientID %>">件名</label>
                                            <cc1:ITextBox ID="txtCfSubject" runat="server" CssClass="form-control input-sm" ReadOnly="true"></cc1:ITextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group date " >
                                            <label class="control-label" for="<%=txtCfReplyDueDate.ClientID %>">返信期限</label>
                                            <cc1:IDateTextBox ID="txtCfReplyDueDate" runat="server" CssClass="form-control input-sm" PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD" ReadOnly="true"/>
                                         </div>
                                    </div>
                                </div>
                                <div class="row">
                                     <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="control-label" for="<%=txtCfContent.ClientID %>"> 本文</label>
                                            <cc1:ITextBox ID="txtCfContent" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="13" MaxLength="2000" Style="font-size:14px;" ReadOnly="true"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8">
                                        <div class="form-group">
                                            <label class="control-label" for="<%=txtCfFileNm1.ClientID %>">添付ファイル</label>
                                            <div class="form-group">
                                                <cc1:ITextBox ID="txtCfFileNm1" Text="" runat="server" CssClass="form-control input-sm"
                                                    ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                                            </div>
                                            <div class="form-group">
                                                <cc1:ITextBox ID="txtCfFileNm2" Text="" runat="server" CssClass="form-control input-sm"
                                                    ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                                            </div>
                                            <div class="form-group">
                                                <cc1:ITextBox ID="txtCfFileNm3" Text="" runat="server" CssClass="form-control input-sm"
                                                    ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8">
                                        <label class="control-label">添付利用者設定</label>
                                        <div class="form-group" id="lstUserModalConfirm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class = "row">
                        <div class="col-md-12 col-sm-12 col-xs-12 text-right">
                            <button type="button" id = "btnConfirmEmail" class="btn btn-primary btn-sm" data-dismiss="modal" runat="server">OK</button>    
                            <button type="button" id = "btnCloseConfirmEmail" class="btn btn-default btn-sm" data-dismiss="modal" onclick="hideLoading()">キャンセル</button>    
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div> 
    <!--END-->

     <!--Save WorkingTimeSytem value from server-->
    <asp:HiddenField ID="treeViewSave" runat="server" />
    <asp:HiddenField ID="treeViewLeftData" runat="server" />
    <asp:HiddenField ID="treeViewRightData" runat="server" />
</asp:Content>