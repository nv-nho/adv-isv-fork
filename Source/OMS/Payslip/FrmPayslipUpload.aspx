<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmPayslipUpload.aspx.cs" Inherits="OMS.Payslip.FrmPayslipUpload" %>

<%@ Import Namespace="OMS.Utilities" %>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <%
        if (this.back_Flag)
        {
    %>
    <style>
        #lblMode
        {
            display:none;
        }
    </style>
    <%
        }
    %>

    <script type="text/javascript">
        var fileUpload = [];

        $(function () {
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

            $(document).on('change', '#' + '<%=this.filespicker.ClientID%>', function (e) {
                var files = e.target.files;

                var arrUserCD = [];
                $('input[name=txtUserCD]').each(function() {
                    arrUserCD.push($(this).val());
                });

                if (document.getElementById('panelError') != null) {
                    document.getElementById('panelError').remove();
                }

                var panel = document.getElementById('panelError0');
                while (panel.firstChild) {
                    panel.removeChild(panel.firstChild);
                }
                
                let isNoFile = !checkNoFile(files);
                if (isNoFile) {
                    $('#panelError91').clone().removeClass('hide').appendTo('#panelError0');
                    $('#' + '<%=this.filespicker.ClientID%>').val('');
                    return;
                }

                let isDupFile = !checkDupFile(files,arrUserCD);
                if (isDupFile) {
                    $('#panelError92').clone().removeClass('hide').appendTo('#panelError0');
                    $('#' + '<%=this.filespicker.ClientID%>').val('');
                    return;
                }

                let isMapErr = !checkMapErr(files, arrUserCD);
                if (isMapErr) {
                    $('#panelError90').clone().removeClass('hide').appendTo('#panelError0');
                    $('#' + '<%=this.filespicker.ClientID%>').val('');
                    return;
                }

                var flag = false;
                $('#' + '<%=this.overrideFlag.ClientID%>').val('0');
                fileUpload = [];
                for (let i=0; i<files.length; i++) {
                    let curFile = files[i];

                    // The file is not in a subfolder, 
                    if (files[i].webkitRelativePath.match(/\//g).length == 1) {
                        var id = arrUserCD.indexOf(curFile.name.substring(0, 6)) + 1;

                        if (id != 0) {
                            var text = "txtFile" + id;
                            fileUpload.push(curFile);
                            if ($('#' + text).children($('[type=text]')).first().val() != '') {
                                flag = true;
                                $('#' + '<%=this.overrideFlag.ClientID%>').val('1');
                            }
                            //Set value
                            //readFile(curFile, id);
                        }
                    }
                }

                $('#' + '<%=this.filespicker.ClientID%>').val('');

                if (flag) {
                    $('#modalQuestion1').modal('show');
                } else {
                    mapFile();
                }
            });
        });

        function mapFile() {
            var arrUserCD = [];
            $('input[name=txtUserCD]').each(function() {
                arrUserCD.push($(this).val());
            });

            for (let i = 0; i < fileUpload.length; i++) {
                let curFile = fileUpload[i];
                var id = arrUserCD.indexOf(curFile.name.substring(0, 6)) + 1;
                if (id != 0) {
                    var text = "txtFile" + id;
                    var button = "btnChoice" + id;
                    var btnRemove_ID = "btnRemove" + id;
                    var btnDownload_ID = "btnDownload" + id;
                    var btnDelete_ID = "btnDelete" + id;
                    var rowUploadDate_ID = "rowUploadDate" + id;
                    var rowDownloadDate_ID = "rowDownloadDate" + id;

                    $('#' + text).children($('[type=text]')).first().val(curFile.name);
                    $('#' + text).children($('p')).html(curFile.name);
                    $('#' + btnRemove_ID).show();
                    $('#' + btnDownload_ID).hide();
                    $('#' + btnDelete_ID).hide();
                    $('#' + rowUploadDate_ID).html("");
                    $('#' + rowDownloadDate_ID).html("");

                    //Set value
                    readFile(curFile, id);
                }
            }
        }

        function clearMapFile() {
            var arrUserCD = [];
            $('input[name=txtUserCD]').each(function() {
                arrUserCD.push($(this).val());
            });

            for (id in arrUserCD) {
                var text = "txtFile" + (id+1);
                var childText = $('#' + text).children($('[type=text]'));
                if (childText.last().val() != '') {
                    childText.last().val('');
                    childText.next().val('');
                }
            }
        }

        function readFile(file, i) {
            var reader = new FileReader();
            reader.onload = function () {
                var txtFileContent = "MainContent_rptList_txtFileContent_" + (i - 1);
                $('#' + txtFileContent).val(reader.result);

                var txtFileName = "MainContent_rptList_txtFileName_" + (i - 1);
                $('#' + txtFileName).val(file.name);
            };
            reader.readAsDataURL(file);
        }

        function checkNoFile(files) {
            //return !(files.length == 0);
            var length = 0;
            for (let i = 0; i < files.length; i++) {
                if (files[i].webkitRelativePath.match(/\//g).length == 1) {
                    length++;
                }
            }
            return !(length == 0);
        }

        function checkDupFile(files, arrUserCD) {
            var object = {};
            var result = [];

            Array.from(files).forEach(function (item) {
                // The file is not in a subfolder, 
                if (item.webkitRelativePath.match(/\//g).length == 1) {
                    if (!object[item.name.substring(0, 6)])
                        object[item.name.substring(0, 6)] = 0;
                    object[item.name.substring(0, 6)] += 1;
                }
            })

            for (var prop in object) {
                if(object[prop] >= 2) {
                    result.push(prop);
                }
            }

            return (result.length == 0)
        }

        function checkMapErr(files, arrUserCD) {
            for (let i = 0; i < files.length; i++) {
               // The file is not in a subfolder, 
                if (files[i].webkitRelativePath.match(/\//g).length == 1) {
                    if (arrUserCD.indexOf(files[i].name.substring(0, 6)) == -1) {
                        return false;
                    }
                }
           }
            return true;
        }

        function choiceFile(id) {
            var button = "btnChoice" + id;
            var text = "txtFile" + id;
            var btnRemove_ID = "btnRemove" + id;
            var btnDownload_ID = "btnDownload" + id;
            var btnDelete_ID = "btnDelete" + id;

            var rowUploadDate_ID = "rowUploadDate" + id;
            var rowDownloadDate_ID = "rowDownloadDate" + id;

            var file_ID = $('#' + button).siblings($('[type=file]')).attr('id');

            $('#' + file_ID).trigger('click');
            
            $(document).on('change', '#' + file_ID, function (e) {
                $('#' + text).children($('[type=text]')).first().val(e.target.files[0].name);

                $('#' + text).children($('p')).html(e.target.files[0].name);

                //readFile(e.target.files[0], btnRemove_ID.slice(-1));

                $('#' + btnRemove_ID).show();

                $('#' + btnDownload_ID).hide();

                $('#' + btnDelete_ID).hide();

                $('#' + rowUploadDate_ID).html("");
                $('#' + rowDownloadDate_ID).html("");
            });
        }

        function btnUploadDir() {
            $('#'+'<%=this.filespicker.ClientID%>').trigger('click');
        }

        function ClearFile(id) {
            var text = "txtFile" + id;
            var btnRemove_ID = "btnRemove" + id;

            $('#' + text).children($('[type=text]')).val("");
            $('#' + text).children($('p')).html("");
            $('#' + btnRemove_ID).hide();
            var file_ID = $('#btnChoice' + id).siblings($('[type=file]')).attr('id');
            $('#' + file_ID).val('');

            $('#' + '<%=this.filespicker.ClientID%>').val('');
        }

        function Clear(){
            var inputID = getCtrlById("hdInValideDefault");
            getCtrlById("cmbInvalidData").val(inputID.val());

            var inputDateDefault = getCtrlById("hdDateDefault");
            getCtrlById("txtYear").val(inputDateDefault.val());
            getCtrlById("txtYear").trigger('change');
        }

        //download file
        function download(id) {
            getCtrlById("hdnFileName").val(id);
            getCtrlById("btnDownload").click();
            hideLoading();
        }

        //download file
        function Remove(id) {
            getCtrlById("hdnRemoveFile").val(id);
            getCtrlById("btnRemove").click();
            hideLoading();
        }

        function RemoveAll_Click(id) {
            $('#modalQuestionDeleteAll').modal('show');
        }

        function RemoveAll() {
            getCtrlById("btnRemoveAllOk").click();
        }
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="overrideFlag" runat="server" />
    <div id='panelError0'></div>
    <asp:Label id="Label0" style="display: none" Text="Label1 Control" runat="server"/>
    <asp:Label id="Label1" style="display: none" Text="Label1 Control" runat="server"/>
    <asp:Label id="Label2" style="display: none" Text="Label2 Control" runat="server"/>
    <asp:Label id="Label3" style="display: none" Text="Label3 Control" runat="server"/>
    <asp:Label id="Label4" style="display: none" Text="Label4 Control" runat="server"/>

     <div id='panelError90' class='alert alert-danger alert-dismissible hide' role='alert'>
     <button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span><span class='sr-only'></span></button>
      <span class='glyphicon glyphicon-remove-sign'></span><strong> 警告!</strong>
      <h5><%=Label0.Text%></h5>
      </div>

     <div id='panelError91' class='alert alert-danger alert-dismissible hide' role='alert'>
        <button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span><span class='sr-only'></span></button>
        <span class='glyphicon glyphicon-remove-sign'></span><strong> 警告!</strong>
        <h5><%=Label1.Text%></h5>
      </div>

     <div id='panelError92' class='alert alert-danger alert-dismissible hide' role='alert'>
        <button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span><span class='sr-only'></span></button>
        <span class='glyphicon glyphicon-remove-sign'></span><strong> 警告!</strong>
        <h5><%=Label2.Text%></h5>
      </div>

        <div class="modalAlert modal fade" id="modalQuestion1" tabindex="-1" role="dialog" aria-labelledby="questionTitle" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
        <div class="modal-content">
        <div class="modal-header">
            <h4 class="modal-title"><span class='glyphicon glyphicon-question-sign'></span> 確認</h4>
            </div>
            <div class="modal-body" runat="server">
                <p> <%=Label3.Text%></p>
            </div>
            <div class="modal-footer">
            <asp:LinkButton ID="btnYes" runat="server" class="btn btn-default" OnClientClick="mapFile();$('#modalQuestion1').modal('hide');return false;">OK</asp:LinkButton>
            <asp:LinkButton ID="btnNo" runat="server" class="btn btn-default" OnClientClick="$('#modalQuestion1').modal('hide');return false;">キャンセル</asp:LinkButton>
            </div>
        </div>
        </div>
    </div>

    <div class="modalAlert modal fade" id="modalQuestionDeleteAll" tabindex="-1" role="dialog" aria-labelledby="questionTitle1" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title"><span class='glyphicon glyphicon-question-sign'></span> 確認</h4>
                </div>
                <div class="modal-body">
                    <p> <%=Label4.Text%></p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-default" OnClientClick="RemoveAll();$('#modalQuestionDeleteAll').modal('hide');return false;">OK</asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" class="btn btn-default" OnClientClick="$('#modalQuestionDeleteAll').modal('hide');return false;">キャンセル</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <%= GetMessage()%>
    <%-- hidden field default--%>
    <asp:HiddenField ID="hdInValideDefault" runat="server" />
    <asp:HiddenField ID="hdDateDefault" runat="server" />
    <asp:HiddenField ID="hdnFileName" runat="server" />
    <button type="button" id="btnDownload" class="hide" runat="server">
        <span class="glyphicon glyphicon-search"></span>&nbsp;Download
    </button>

    <asp:HiddenField ID="hdnRemoveFile" runat="server" />
    <button type="button" id="btnRemove" class="hide" runat="server">
        <span class="glyphicon glyphicon-search"></span>&nbsp;Remove
    </button>
    <button type="button" id="btnRemoveAllOk" class="hide" runat="server">
        <span class="glyphicon glyphicon-search"></span>&nbsp;Remove All
    </button>


    <%--Condition Search--%>
    <div class="well well-sm">
        <div class="row">
            <%--Config Code--%>
            <div class="col-md-2">
                <div class="form-group">
                    <label class="control-label" for="<%= txtYear.ClientID %>">
                            対象年月</label>
                   
                    <%-- 授業料請求開始月 --%>
                    <div class='<% =GetClassError("txtYear")%>'>
                        <div class="input-group date" id='DivStartMonth'>
                             <cc1:IDateTextBox ID="txtYear" runat="server" CssClass="form-control input-sm" PickDate="true"
                        PickTime="false" PickFormat="YYYY/MM"/>                            
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>

                        <asp:HiddenField ID="hdn_txtYear" runat="server"/>
                    </div>

                </div>
            </div>

            <%--Invalid Data--%>
            <div class="col-md-2">
                <div class="form-group">
                    <label class="control-label" for="<%= cmbInvalidData.ClientID %>">
                        明細区分</label>
                    <asp:DropDownList ID="cmbInvalidData" runat="server" CssClass="form-control input-sm">
                    </asp:DropDownList>

                    <asp:HiddenField ID="hdn_cmbInvalidData" runat="server"/>
                </div>
            </div>


            <%
                if (this.back_Flag)
                {
            %>
                <div class="col-md-4">
                    <div>
                        <label class="control-label">&nbsp;</label>
                    </div>

                    <div class="btn-group">
                        <button type="button" id="btnSearch" class="btn btn-default btn-sm loading" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;実行
                        </button>
                        <button type="button" id="btnClear" class="btn btn-default btn-sm" onclick="Clear();">
                            <span class="glyphicon glyphicon-refresh"></span>&nbsp;クリア
                        </button>
                    </div>
                </div>
            <%
                }
            %>



        </div>
    </div>

    <%--Create New and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <% 
            if (!this.Init_flag)
            { 
            %>
            <div class="col-md-6">
                <div class="btn-group btn-group-justified">
            <%
            }
            else
            { 
            %>
            <div class="col-md-3">
                <div class="btn-group btn-group-justified">
            <%
            }
            %>

            <%
            if (this.Mode == OMS.Utilities.Mode.View )
            {
            %>

                <% 
                if (!this.Init_flag)
                { 
                %>
                    <%--Button Edit--%>
                    <div class="btn-group">
                        <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnEdit_Click">
                            <span class="glyphicon glyphicon-pencil"></span> 編集
                        </asp:LinkButton>
                    </div>
                <%
                }
                %>
            <%
            }
            else
            {                                
            %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmPayslipUpload.aspx" OnClick="btnNew_Click"
                            CssClass="btn btn-primary btn-sm loading"> <span class="glyphicon glyphicon-ok"></span>&nbsp;登録 </asp:LinkButton>
                    </div>
            <%
            }
            %>


            <%
                if (this.back_Flag)
                { 
            %>
                <div class="btn-group">
                    <button type="button" id="btnBackToList" class="btn btn-default btn-sm loading" runat="server">
                            <span class="glyphicon glyphicon-chevron-left"></span> 戻る
                    </button>
                </div>
            <%              
                }
                else { 
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
            <%
            if (this.Mode != OMS.Utilities.Mode.View )
            {
            %>
                <div class="col-md-2 ">
                    <asp:FileUpload ID="filespicker" style="display: none" runat="server" webkitdirectory directory/>
                </div>
                <%
                if (this.Mode == OMS.Utilities.Mode.Update )
                {
                %>
                <div class="col-md-4">
                <%
                }else{
                %>
                <div class="col-md-2  pull-right">
                <%
                }
                %>
                    <div class="btn-group btn-group-justified">
                        <%
                        if (this.Mode == OMS.Utilities.Mode.Update )
                        {
                        %>
                        <div class="btn-group">
                            <button type="button" ID="btnRemoveAll" runat="server" onclick="RemoveAll_Click();return false;"
                                class="btn btn-danger btn-sm  pull-right"> <span class="glyphicon glyphicon-trash"></span>&nbsp;明細一括削除 </button>
                        </div>
                        <%
                        }
                        %>
                        <div class="btn-group">
                            <button type="button" ID="btnUpDirUp" runat="server" onclick="btnUploadDir();return false;"
                                class="btn btn-success btn-sm  pull-right"> <span class="glyphicon glyphicon-folder-open"></span>&nbsp;明細一括アップロード </button>
                        </div>
                    </div>
                 </div>
            <%
            }
            %>
        </div>
    </div>
    <%
    if (this.rptList.DataSource != null)
    { 
    %>       
    <div class="row">
        <div class="col-sm-12">
            <span class="label alert-light-gray"><%= this.textKakunin%></span>
        </div>
    </div>  
    </br>
    <%
    }
    %>  

    <%-- List Config--%>
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <asp:Repeater ID="rptList" runat="server">
        <HeaderTemplate>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr <%# (Eval("DownloadDate_fm") != "") ? "class='light-gray'" : "" %>>
                <td>
                    <%# Eval("RowNumber") %>

                </td>
                <td width="80px">
                    <asp:HiddenField ID="txtUserCD" runat="server" Value='<%# Eval("UserCD")%>'/>
                    <input type="hidden" name="txtUserCD" value='<%# Eval("UserCDLong")%>' />
                    <%# Eval("UserCD")%>
                </td>
                <td>
                    <%# Eval("UserName1")%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("DepartmentName", "{0}"))%>
                </td>

                <%
                if (this.Mode == OMS.Utilities.Mode.View)
                {
                %>
                <td>

                    <button class="btn btn-success btn-sm" type="button" disabled="true">
                                <span class="glyphicon glyphicon-folder-open"></span>
                    </button>

                    <button class="btn btn-info btn-sm" id="Button1" type="button"
                                onclick="download('<%# Eval("ID") %>');return false;"
                                <%# (Eval("Filepath") == "") ? "style='display:none;'" : "" %>
                                >
                                <span class="glyphicon glyphicon-cloud-download"></span>
                    </button>

                    <button class="btn btn-danger btn-sm" type="button" disabled="true"
                                <%# (Eval("Filepath") == "") ? "style='display:none;'" : "" %>
                                >
                                <span class="glyphicon glyphicon-remove"></span>
                    </button>
                </td>
                <%
                }
                else
                { 
                %>

                <td>
                    
                    <asp:FileUpload  runat="server" ID="authPath" Style="display: none;"/>
                    
                    <button class="btn btn-success btn-sm" id="btnChoice<%# Eval("RowNumber") %>" type="button" onclick="choiceFile('<%# Eval("RowNumber") %>')">
                                <span class="glyphicon glyphicon-folder-open"></span>
                    </button>


                    <button class="btn btn-info btn-sm" id="btnDownload<%# Eval("RowNumber") %>" type="button"
                                onclick="download('<%# Eval("ID") %>');return false;"
                                <%# (Eval("Filepath") == "") ? "style='display:none;'" : "" %>
                                >
                                <span class="glyphicon glyphicon-cloud-download"></span>
                    </button>



                    <button class="btn btn-danger btn-sm" type="button" id="btnDelete<%# Eval("RowNumber") %>"
                                onclick="Remove('<%# Eval("ID") %>');"
                                <%# (Eval("Filepath") == "") ? "style='display:none;'" : "" %>
                                >
                                <span class="glyphicon glyphicon-remove"></span>
                                
                    </button>


                    <button class="btn btn-danger btn-sm" id="btnRemove<%# Eval("RowNumber") %>" onclick="ClearFile('<%# Eval("RowNumber") %>');" type="button" style='display:none;'>
                            <span class="glyphicon glyphicon-remove"></span>
                    </button>

                </td>
                <%
                }
                %>
                <td id="txtFile<%# Eval("RowNumber") %>">
                    <asp:HiddenField ID="txtFilepath" runat="server" Value='<%# Eval("Filepath")%>'/>
                    <p ID="lableFilepath" runat="server"><%# Eval("Filepath")%></p>
                    <asp:HiddenField ID="txtFileContent" runat="server" />
                    <asp:HiddenField ID="txtFileName" runat="server" />
                    <%--<asp:HiddenField ID="txtOldFilePath" runat="server" Value='<%# Eval("OldFilepath")%>' />--%>
                </td>

                <td>
                    <p id="rowUploadDate<%# Eval("RowNumber") %>"><%# Server.HtmlEncode(Eval("UploadDate_fm", "{0}"))%></p>
                    <asp:HiddenField ID="hdfUploadDate" runat="server" Value='<%# Eval("UploadDate_fm")%>'/>
                </td>

                <td>
                    <p id="rowDownloadDate<%# Eval("RowNumber")%>"><%# Server.HtmlEncode(Eval("DownloadDate_fm", "{0}"))%></p>
                </td>

                <td style="display:none;">
                    <asp:HiddenField ID="txtID" runat="server" Value='<%# Eval("ID")%>'/>
                </td>

            </tr>
            
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>

            <% 
            if (!this.Init_flag)
            { 
            %>
                <%--Create New and Back Button--%>
                <div class="well well-sm">
                    <div class="row">
                <%
                if (this.Mode == OMS.Utilities.Mode.View )
                {
                %>
                <div class="col-md-6">
                    <div class="btn-group btn-group-justified">
                        <%--Button Edit--%>
                        <div class="btn-group">
                            <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnEdit_Click">
                                <span class="glyphicon glyphicon-pencil"></span> 編集
                            </asp:LinkButton>
                        </div>
                        <div class="btn-group">
                            <asp:LinkButton ID="LinkButton6" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnBack_Click">
                                <span class="glyphicon glyphicon-chevron-left"></span> 戻る
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <%
                }
                else
                {                                
                %>
                <div class="col-md-6">
                    <div class="btn-group btn-group-justified">
                        <div class="btn-group">
                            <asp:LinkButton ID="LinkButton7" runat="server" PostBackUrl="FrmPayslipUpload.aspx" OnCommand="btnNew_Click"
                                CssClass="btn btn-primary btn-sm loading"> <span class="glyphicon glyphicon-ok"></span>&nbsp;登録 </asp:LinkButton>
                        </div>
                        <div class="btn-group">
                            <asp:LinkButton ID="LinkButton8" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnBack_Click">
                                <span class="glyphicon glyphicon-chevron-left"></span> 戻る
                            </asp:LinkButton>
                        </div>
                     </div>
                </div>

                <%
                }
                %>
                </div>
            </div>
            <%
            }
            %>
    </div>
    </div>
</asp:Content>
