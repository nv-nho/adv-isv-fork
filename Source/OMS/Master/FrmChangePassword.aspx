<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmChangePassword.aspx.cs" Inherits="OMS.ChangePassword.FrmChangePassword" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%--<%@ Import Namespace="OMS.Utilities" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        //**********************
        // Init
        //**********************
        $(function () {
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
            getCtrlById("txtOldPassword").focus().select();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <div class="well well-sm">
        <%--User Name--%>
        <div class="row">
            <div class="col-md-4">
                <div class='form-group <% =GetClassError("txtUserName")%>'>
                    <label class="control-label" for="<%= txtUserName.ClientID %>">
                        社員名</label>
                    <cc1:ITextBox ID="txtUserName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    <%--<%=GetSpanError("txtUserName")%>--%>
                </div>
            </div>
        </div>
         <div class="row">
            <div class="col-md-4">
                <div class='form-group <% =GetClassError("txtLoginID")%>'>
                    <label class="control-label" for="<%= txtLoginID.ClientID %>">
                        ログインID</label>
                    <cc1:ICodeTextBox ID="txtLoginID" runat="server" UpperCode="false" CssClass="form-control input-sm"
                        AutoComplete="off"/>
                    <%--<%=GetSpanError("txtLoginID")%>--%>
                </div>
            </div>
        </div>
        <%--Old Password--%>
        <div class="row">
            <div class="col-md-4">
                <div class='form-group <% =GetClassError("txtOldPassword")%>'>
                    <label class="control-label" for="<%= txtOldPassword.ClientID %>">
                        旧パスワード <strong class="text-danger">*</strong></label>
                    <cc1:ITextBox ID="txtOldPassword" runat="server" CssClass="form-control input-sm"
                        TextMode="Password"></cc1:ITextBox>
                    <%--<%=GetSpanError("txtOldPassword")%>--%>
                </div>
            </div>
        </div>
        <%--New Password--%>
        <div class="row">
            <div class="col-md-4">
                <div class='form-group <% =GetClassError("txtNewPassword")%>'>
                    <label class="control-label" for="<%= txtNewPassword.ClientID %>">
                        新パスワード <strong class="text-danger">*</strong></label>
                    <cc1:ITextBox ID="txtNewPassword" runat="server" CssClass="form-control input-sm"
                        TextMode="Password"></cc1:ITextBox>
                    <%--<%=GetSpanError("txtNewPassword")%>--%>
                </div>
            </div>
        </div>
        <%-- Confirm new password --%>
        <div class="row">
            <div class="col-md-4">
                <div class='form-group <% =GetClassError("txtConfirm")%>'>
                    <label class="control-label" for="<%= txtConfirm.ClientID %>">
                        新パスワード（確認用）<strong class="text-danger">*</strong></label>
                    <cc1:ITextBox ID="txtConfirm" runat="server" CssClass="form-control input-sm" TextMode="Password"></cc1:ITextBox>
                    <%--<%=GetSpanError("txtConfirm")%>--%>
                </div>
            </div>
        </div>
    </div>
    <div class="well well-sm">
        <%--処理Buttonグループ--%>
        <div class="row">
            <div class="col-md-6">
                <div class="btn-group btn-group-justified">
                    <%-- Change button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnChange" runat="server" CssClass="btn btn-primary btn-sm loading"
                            OnClick="btnChange_Click">
                            <span class="glyphicon glyphicon-ok"></span>&nbsp;登録
                        </asp:LinkButton>
                    </div>
                    <%-- Cancel button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnCancel" runat="server" PostBackUrl="../Menu/FrmMasterMenu.aspx" CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
