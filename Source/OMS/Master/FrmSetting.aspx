<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmSetting.aspx.cs" Inherits="OMS.Master.FrmSetting" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Import Namespace="OMS.Utilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
       
        $(function () {
            setFocus();
            //focusErrors();

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

        });

        /*
        * Set focus control follow mode
        */
        function setFocus() {

             <%if(this.Mode==Mode.Insert || this.Mode==Mode.Update){ %>
                getCtrlById("txtQuoteNo").focus();
            <%} %>
       
        }

        //Display Logo
        function displayLogo(imgLogoID, fileLogoID) {
            var imgLogo = document.querySelector(imgLogoID);
            var fileUpload = document.querySelector(fileLogoID).files[0];
            var reader = new FileReader();
            if(reader !=null){
                reader.onload = function () {
                    imgLogo.src =reader.result;
                }
            }

            var imgSrc=imgLogo.src;

            if (fileUpload) {
                reader.readAsDataURL(fileUpload);
                if(fileUpload.name.indexOf('no-image') < 0 )
                {
                    if(imgLogo.id.indexOf('imgLogo1')>=0 && (fileUpload.name.indexOf('.jpg')>=0 || fileUpload.name.indexOf('.png')>=0 || fileUpload.name.indexOf('.gif')>=0 || fileUpload.name.indexOf('.jpeg')>=0 || fileUpload.name.indexOf('.bmp')>=0))
                    {
                        getCtrlById("imgLogo1").removeAttr("style");
                    }
                    else if(imgLogo.id.indexOf('imgLogo2')>=0)
                    {
                        getCtrlById("imgLogo2").removeAttr("style");
                    }
                }
                
            }else
            {
                if(imgSrc!=null && imgSrc.indexOf('no-image') >= 0)
                {
                    if(imgLogo.id.indexOf('imgLogo1')>=0)
                    {
                        getCtrlById("imgLogo1").attr("style","width:175px;height:160px;");
                    }
                    if(imgLogo.id.indexOf('imgLogo2')>=0)
                    {
                        getCtrlById("imgLogo2").attr("style","width:175px;height:160px;");
                    }
                }
            }
        }
       
        //Upload the logo
        function uploadLogo(id)
        {
            switch(id)
            {
              
                //Display Logo1
            case getCtrlById("btnBrowse11").attr("id"):
                $('#<%= fileLogo1.ClientID%>').trigger('click'); 
                     
                var imgSrc=document.querySelector('#<%=imgLogo1.ClientID %>').src;  
                if(imgSrc!=null && imgSrc.indexOf('no-image')>=0)
                {
                getCtrlById("imgLogo1").attr("style","width:175px;height:160px;");
                }else{
                getCtrlById("imgLogo1").removeAttr("style");
                }

                $(document).on('change', '#<%= fileLogo1.ClientID%>', function (e) {
                $('#<%= txtLogo1.ClientID%>').val(e.target.files[0].name);
                displayLogo('#<%=imgLogo1.ClientID %>','#<%=fileLogo1.ClientID %>');
                });

                     
                var fileUpload = document.querySelector('#<%= fileLogo1.ClientID%>').files[0];
                if(fileUpload !=null)
                {
                $('#<%= txtLogo1.ClientID%>').val(fileUpload.name);
                }

                displayLogo('#<%=imgLogo1.ClientID %>','#<%=fileLogo1.ClientID %>');
                      
                break;

                 //Display Logo2
                case getCtrlById("btnBrowse12").attr("id"):

                     $('#<%= fileLogo2.ClientID%>').trigger('click');  

                     var imgSrc=document.querySelector('#<%=imgLogo2.ClientID %>').src;  
                     if(imgSrc!=null && imgSrc.indexOf('no-image')>=0)
                     {
                        getCtrlById("imgLogo2").attr("style","width:175px;height:160px;");
                     }else{
                        getCtrlById("imgLogo2").removeAttr("style");
                     }

                     $(document).on('change', '#<%= fileLogo2.ClientID%>', function (e) {
                        $('#<%= txtLogo2.ClientID%>').val(e.target.files[0].name);
                        displayLogo('#<%=imgLogo2.ClientID %>','#<%=fileLogo2.ClientID %>');
                     });

                     var fileUpload = document.querySelector('#<%= fileLogo2.ClientID%>').files[0];

                     if(fileUpload !=null)
                     {
                        $('#<%= txtLogo2.ClientID%>').val(fileUpload.name);
                     }
                     
                     displayLogo('#<%=imgLogo2.ClientID %>','#<%=fileLogo2.ClientID %>');
                     
                     break;
                }
            }

        //download file
        function download(id){            
            $("#MainContent_hdnFileName").val(id);
            getCtrlById("btnDownload").click();
            hideLoading();
        }

        function clearText(id)
        {
            $("#MainContent_hdnFileClear").val(id);
            getCtrlById("btnClear").click();
            hideLoading();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <asp:HiddenField ID="hdnFileName" runat="server" />
    <asp:HiddenField ID="hdnFileClear" runat="server" />
    <button type="button" id="btnDownload" class="hide" runat="server">
        <span class="glyphicon glyphicon-search"></span>&nbsp;Download
    </button>
    <button type="button" id="btnClear" class="hide" runat="server">
        <span class="glyphicon glyphicon-search"></span>&nbsp;Clear
    </button>
    <div class="well well-sm">            
        <div class="row">
            <div class="col-md-5">
                <div class='form-group <%=GetClassError("txtAttachPath")%>'>
                    <label class="control-label" for="<%= txtAttachPath.ClientID %>">
                        Attach Path<strong class="text-danger"> *</strong></label>
                    <cc1:ITextBox ID="txtAttachPath" runat="server" CssClass="form-control input-sm"
                        Text=""></cc1:ITextBox>
                    <%--<%=GetSpanError("txtInvoiceNo")%>--%>
                </div>
            </div>
        </div>               
        <div class="row">
            <div class="col-md-5">
                <div class='form-group <% =GetClassError("txtLogo1")%>'>
                    <label class="control-label" for="<%= txtLogo1.ClientID %>">
                        Logo1</label>
                    <div class="input-group">
                        <span class="input-group-btn">
                            <button class="btn btn-default btn-sm" id="btnBrowse11" type="button" runat="server"
                                onclick="uploadLogo($(this).attr('id'));">
                                <span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Choose File...
                            </button>
                        </span>
                        <cc1:ITextBox ID="txtLogo1" Text="" runat="server" CssClass="form-control input-sm"
                            ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                        <span class="input-group-btn">
                            <button class="btn btn-default btn-sm" id="btnDownloadLogo1" type="button" runat="server"
                                onclick="download('11');return false;">
                                <span class="glyphicon glyphicon-cloud-download"></span>
                            </button>
                        </span>
                    </div>
                    <asp:FileUpload ID="fileLogo1" runat="server" Style="display: none;" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <img id="imgLogo1" class="img img-rounded" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <span>&nbsp;</span>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <div class='form-group <% =GetClassError("txtLogo2")%>'>
                    <label class="control-label" for="<%= txtLogo2.ClientID %>">
                        Logo2</label>
                    <div class="input-group">
                        <span class="input-group-btn">
                            <button class="btn btn-default btn-sm" id="btnBrowse12" type="button" runat="server"
                                onclick="uploadLogo($(this).attr('id'));">
                                <span class="glyphicon glyphicon-folder-open"></span>&nbsp;&nbsp;Choose File...
                            </button>
                        </span>
                        <cc1:ITextBox ID="txtLogo2" Text="" runat="server" CssClass="form-control input-sm"
                            ReadOnly="true" TabIndex="-1"></cc1:ITextBox>
                        <span class="input-group-btn">
                            <button class="btn btn-default btn-sm" id="btnDownloadLogo2" type="button" runat="server"
                                onclick="download('12');return false;">
                                <span class="glyphicon glyphicon-cloud-download"></span>
                            </button>
                        </span>
                    </div>
                    <asp:FileUpload ID="fileLogo2" runat="server" Style="display: none;" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <img id="imgLogo2" class="img img-rounded" alt="" style="width: 175px; height: 160px"
                    runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <div class='form-group <%=GetClassError("txtExtension")%>'>
                    <label class="control-label" for="<%= txtExtension.ClientID %>">メール添付ファイル拡張子</label>
                    <cc1:ITextBox ID="txtExtension" runat="server" CssClass="form-control input-sm"
                        Text=""></cc1:ITextBox>
                </div>
            </div>
        </div>
    </div>
    <!-- /form well-->
    <div class="well well-sm">
        <!-- 処理Buttonグループ -->
        <div class="row">
            <div class="col-md-6">
                <div class="btn-group btn-group-justified">
                    <%
                        if (this.Mode == OMS.Utilities.Mode.View)
                        {
                    %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm loading"
                            OnClick="btnEdit_Click">
                        <span class="glyphicon glyphicon-pencil"></span> 編集
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Insert)
                        {
                    %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnInsert" runat="server" CssClass="btn btn-primary btn-sm loading"
                            OnClick="btnInsert_Click">
                        <span class="glyphicon glyphicon-ok"></span> 登録
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Update)
                        {
                    %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm loading"
                            OnClick="btnUpdate_Click">
                        <span class="glyphicon glyphicon-ok"></span> 登録
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
                            PostBackUrl="../Menu/FrmMasterMenu.aspx">
                        <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                        else
                        {
                    %>
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
    </div>
    <!--/処理ボタングループ-->
</asp:Content>
