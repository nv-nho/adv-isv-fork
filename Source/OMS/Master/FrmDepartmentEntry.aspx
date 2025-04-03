<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmDepartmentEntry.aspx.cs" Inherits="OMS.Master.FrmDepartmentEntry" %>

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
            focusErrors();

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

        //**********************
        // Set Focus
        //**********************
        function setFocus() {
            
            <%if(this.Mode == Mode.Insert || this.Mode == Mode.Copy){ %>
                getCtrlById("txtDepartmentCD").focus().select();
            <%} %>
//            <%if(this.Mode == Mode.View){ %>
//                getCtrlById("txtDepartmentCD").focus().select();
//            <%} %>
            <%if(this.Mode == Mode.Update){ %>
                getCtrlById("txtDepartmentName").focus().select();
            <%} %>
        }
            
        //**********************
        // Find Back
        //**********************    
        function findBack() {
            hideLoading();
            getCtrlById("txtDepartmentCD").focus().select();
        }        
            
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-3 col-sm-3">
                    <div class='form-group <%=GetClassError("txtDepartmentCD")%>'>
                        <label class="control-label" for="<%= txtDepartmentCD.ClientID %>">
                            コード <strong class="text-danger">*</strong></label>
                        <cc1:ICodeTextBox ID="txtDepartmentCD" runat="server" CodeType="AlphaNumeric"  AjaxUrlMethod="GetDepartment" AllowChars="-"
                            CssClass="form-control input-sm" LabelNames="txtDepartmentCD"></cc1:ICodeTextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-4 col-sm-4">
                    <div class='form-group <%=GetClassError("txtDepartmentName")%>'>
                        <label class="control-label" for="<%= txtDepartmentName.ClientID %>">
                            部門名<strong class="text-danger">*</strong></label>
                        <cc1:ITextBox ID="txtDepartmentName" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITextBox>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class='form-group'>
                        <label class="control-label" for="<%= txtDepartmentName2.ClientID %>">
                            略称
                        </label>
                        <cc1:ITextBox ID="txtDepartmentName2" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITextBox>
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
                            PostBackUrl="~/Master/FrmDepartmentList.aspx">
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
            <% if (this.Mode == OMS.Utilities.Mode.View) { %>
                <div class="col-md-6 col-sm-7">
                    <div class="well well-sm">
                        <div class="row">
                            <!-- New Buton -->
                            <div class="col-md-6 col-sm-7">
                                <div class="btn-group btn-group-justified">
                                    <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmDepartmentEntry.aspx"
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
