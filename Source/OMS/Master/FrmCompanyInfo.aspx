<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmCompanyInfo.aspx.cs" Inherits="OMS.Master.FrmCompanyInfo" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Import Namespace="OMS.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
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

        /*
        * Set focus control follow mode
        */
        function setFocus() {

             <%if(this.Mode==Mode.Insert || this.Mode==Mode.Update){ %>
                getCtrlById("txtCompanyName1").focus().select();
            <%} %>
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <%= GetMessage()%>
        <div class="well well-sm">
            <div class="row">
                <%-- Company Name --%>
                <div class="col-md-6">
                    <div class='form-group <%=GetClassError("txtCompanyName1")%>'>
                        <label class="control-label" for="<%= txtCompanyName1.ClientID %>">
                            会社名 <strong class="text-danger"> *</strong></label>
                        <cc1:ITextBox ID="txtCompanyName1" runat="server" CssClass="form-control input-sm" Text=""></cc1:ITextBox>
                        <%--<%=GetSpanError("txtCompanyName1")%>--%>
                    </div>
                </div>
            </div>
            <div class="row">
                <%-- Company name2 --%>
                <div class="col-md-6">
                    <div class='form-group <% =GetClassError("txtCompanyName2")%>'>
                        <label class="control-label" for="<%= txtCompanyName2.ClientID %>">
                            略称</label>
                        <cc1:ITextBox ID="txtCompanyName2" runat="server" CssClass="form-control input-sm" Text=""></cc1:ITextBox>
                        <%--<%=GetSpanError("txtCompanyName2")%>--%>
                    </div>
                </div>
            </div>
            <div class="row">
            <%-- Address English --%>
                <div class="col-md-8">
                    <div class='form-group <% =GetClassError("txtAddress1")%> <% =GetClassError("txtAddress2")%> <% =GetClassError("txtAddress3")%>'>
                        <label class="control-label" for="<%= txtAddress1.ClientID %>">
                            住所1</label>
                        <cc1:ITextBox ID="txtAddress1" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                        <%--<%=GetSpanError("txtAddress1")%>--%>
                        <cc1:ITextBox ID="txtAddress2" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                        <%--<%=GetSpanError("txtAddress2")%>--%>
                        <cc1:ITextBox ID="txtAddress3" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                        <%--<%=GetSpanError("txtAddress3")%>--%>
                    </div>
                </div>                
            </div>
            
            <div class="row">
            <%-- Address Viet Name --%>
                <div class="col-md-8">
                    <div class='form-group <% =GetClassError("txtAddress4")%> <% =GetClassError("txtAddress5")%> <% =GetClassError("txtAddress6")%>'>
                        <label class="control-label" for="<%= txtAddress4.ClientID %>">
                            住所2</label>
                        <cc1:ITextBox ID="txtAddress4" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                        <%--<%=GetSpanError("txtAddress4")%>--%>
                        <cc1:ITextBox ID="txtAddress5" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                        <%--<%=GetSpanError("txtAddress5")%>--%>
                        <cc1:ITextBox ID="txtAddress6" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                        <%--<%=GetSpanError("txtAddress6")%>--%>
                    </div>
                </div>                
            </div>
            <div class="row">
                <%-- Email address--%>
                 <div class="col-md-3">
                    <div class='form-group <% =GetClassError("txtEmailAddress")%>'>
                        <label class="control-label" for="<%= txtEmailAddress.ClientID %>">Email</label>
                        <cc1:ICodeTextBox ID="txtEmailAddress" runat="server" UpperCode="false" CssClass="form-control input-sm" CodeType="AlphaNumeric" AllowChars="@,.,_,-" />
                        <%--<%=GetSpanError("txtEmailAddress")%>--%>
                    </div>
                </div>
                <%-- Tel --%>
                <div class="col-md-3">
                    <div class='form-group <% =GetClassError("txtTel")%>'>
                        <label class="control-label" for="<%= txtTel.ClientID %>">Tel</label>
                        <cc1:ICodeTextBox ID="txtTel" runat="server" CodeType="Numeric" CssClass="form-control input-sm" Text="" AllowChars=" ,-,+,(,)" />

                        <%--<%=GetSpanError("txtTel")%>--%>
                    </div>
                </div>
                <%-- Tel2 --%>
                 <div class="col-md-3">
                    <div class='form-group <% =GetClassError("txtTel2")%>'>
                        <label class="control-label" for="<%= txtTel2.ClientID %>">Tel2</label>
                        <cc1:ICodeTextBox ID="txtTel2" runat="server" CodeType="Numeric" CssClass="form-control input-sm" Text="" AllowChars=" ,-,+,(,)" />
                        <%--<%=GetSpanError("txtTel2")%>--%>
                    </div>
                </div>
                <%-- Fax --%>
                <div class="col-md-3">
                    <div class='form-group <% =GetClassError("txtFAX")%>'>
                        <label class="control-label" for="<%= txtFAX.ClientID %>">FAX</label>
                        <cc1:ICodeTextBox ID="txtFAX" runat="server" CodeType="Numeric" CssClass="form-control input-sm" Text="" AllowChars=" ,-,+,(,)" />
                        <%--<%=GetSpanError("txtFAX")%>--%>
                    </div>
                </div>
            </div>
            
             <div class="row">
                <%-- Represent --%>
                <div class="col-md-3">
                    <div class='form-group <% =GetClassError("txtRepresent")%>'>
                        <label class="control-label" for="<%= txtRepresent.ClientID %>">
                        代表者名</label>
                        <cc1:ITextBox ID="txtRepresent" runat="server" CssClass="form-control input-sm" Text=""></cc1:ITextBox>
                        <%--<%=GetSpanError("txtRepresent")%>--%>
                    </div>
                </div>
                <%-- Position --%>
                <div class="col-md-3">
                    <div class='form-group <% =GetClassError("txtPosition")%>'>
                        <label class="control-label" for="<%= txtPosition.ClientID %>">
                        役職名</label>
                        <cc1:ITextBox ID="txtPosition" runat="server" CssClass="form-control input-sm" Text=""></cc1:ITextBox>
                        <%--<%=GetSpanError("txtPosition")%>--%>
                    </div>
                </div>
            </div>

            <!-- / form-->
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
                            <%-- Edit button --%>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnEdit_Click">
                                    <span class="glyphicon glyphicon-pencil"></span> 編集
                                </asp:LinkButton>
                            </div>
                        <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Insert)
                        {                                
                        %>
                            <%-- Insert button --%>
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
                            <%-- Update button --%>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm loading" OnClick="btnUpdate_Click" >
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
                            <%-- Back button (return to menu master) --%>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-default btn-sm loading" PostBackUrl="../Menu/FrmMasterMenu.aspx">
                                    <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                                </asp:LinkButton>
                            </div>
                        <%
                        }else{
                        %>
                            <%-- Back button (return to mode show) --%>
                            <div class="btn-group">
                                <asp:LinkButton ID="btnBack1" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnBack_Click">
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
