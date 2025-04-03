<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmInformationDetail.aspx.cs"
    Inherits="OMS.Master.FrmInformationDetail" MasterPageFile="~/Site.Master" Title="" %>

<%--Import Using--%>
<%------------------------------------------------------------------------------------------------------------------------------%>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="oms" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="pagingFooter" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="pagingHeader" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="headerGrid" %>
<%@ Import Namespace="OMS.Utilities" %>
<%------------------------------------------------------------------------------------------------------------------------------%>
<%--Java Script--%>
<%------------------------------------------------------------------------------------------------------------------------------%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
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
                getCtrlById("txtInformationName").focus().select();
            <%} %>
//            <%if(this.Mode == Mode.View){ %>
//                getCtrlById("btnEdit").focus().select();
//            <%} %>
            <%if(this.Mode == Mode.Update){ %>
                getCtrlById("txtInformationName").focus().select();
            <%} %>
        }

        /*
        * Clear form
        */
        function clearForm() {
            $(":text").val("");
            getCtrlById("txtInformationName").focus().select();
        }

    </script>
</asp:Content>
<%------------------------------------------------------------------------------------------------------------------------------%>
<%--Page Content--%>
<%------------------------------------------------------------------------------------------------------------------------------%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <div class="well well-sm">
       
        <div class="row">
            <%--- Information Name ---%>
            <div class="col-md-8">
                <div class='form-group <% =GetClassError("txtInformationName")%>'>
                    <label class="control-label" for="<%= txtInformationName.ClientID %>">
                        お知らせ情報名<strong class="text-danger"> *</strong></label>
                    <oms:ITextBox ID="txtInformationName" runat="server" CssClass="form-control input-sm" />
                    <%--<%=GetSpanError("txtInformationName")%>--%>
                </div>
            </div>
        </div>
       
        <!--txtBeginDate-->
        <div class="row">
            <div class="col-md-3">
                <div class='form-group <%=GetClassError("dtBeginDate")%>'>
                            <label class="control-label" for="<%= dtBeginDate.ClientID %>">
                               開始日<strong class="text-danger"> *</strong></label>
                    <div class="input-group date">
                            <oms:IDateTextBox ID="dtBeginDate" runat="server" CssClass="form-control input-sm" PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD HH:mm:ss"/>
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class='form-group <%=GetClassError("dtEndDate")%>'>
                   <label class="control-label" for="<%= dtEndDate.ClientID %>">終了日<strong class="text-danger"> *</strong></label>
                   <div class="input-group date" >
                        <oms:IDateTextBox ID="dtEndDate" runat="server" CssClass="form-control input-sm" PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD HH:mm:ss"/>
                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row">
        <%--- Information Content ---%>
            <div class="col-md-8">
                <div class="form-group <% =GetClassError("txtInformationContent")%>">
                    <label class="control-label" for="<%= txtInformationContent.ClientID %>">
                    お知らせ内容 <strong class="text-danger"></strong></label>
                    <oms:ITextBox ID="txtInformationContent" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Rows="5" MaxLength="500"/>
                        <%--<%=GetSpanError("txtInformationContent")%>--%>
                </div>
            </div>
        </div>
        
    </div>
    <%
        if (this.Mode != OMS.Utilities.Mode.Delete)
        {
    %>
     <div class="row">
        <!-- Left Button Panel -->
        <div class="col-md-6">
            <div class="well well-sm">
                <div class="btn-group btn-group-justified">
                     <%
             if (this.Mode == OMS.Utilities.Mode.View)
             {
                    %>
                    <%--- Edit button ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnEdit_Click">
                            <span class="glyphicon glyphicon-pencil"></span>&nbsp;編集
                        </asp:LinkButton>
                    </div>
                    <%--- Copy button ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnCopy" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnCopy_Click">
                            <span class="glyphicon glyphicon-paperclip"></span>&nbsp;コピー
                        </asp:LinkButton>
                    </div>
                    <%--- Delete button ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnDelete_Click">
                                <span class="glyphicon glyphicon-trash"></span>&nbsp;削除
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Insert || this.Mode == OMS.Utilities.Mode.Copy)
                        {
                    %>
                    <%--- Insert button ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnInsert" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnInsert_Click">
                            <span class="glyphicon glyphicon-ok"></span>&nbsp;登録
                        </asp:LinkButton>
                    </div>
                    <%
                    }
                        else if (this.Mode == OMS.Utilities.Mode.Update)
                        {
                    %>
                    <%--- Update button ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnUpdate_Click">
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
                    <%--- Back button (return to list)---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-default btn-sm" PostBackUrl="~/Master/FrmInformationList.aspx">
                                <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                        else
                        {
                    %>
                    <%--- Back button (return to mode show) ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnBack_Click">
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
            <div class="col-md-6">
                <div class="well well-sm">
                    <div class="row">
                        <!-- New Buton -->
                        <div class="col-md-6">
                            <div class="btn-group btn-group-justified">
                                <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="~/Master/FrmInformationDetail.aspx"
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
    
    <% } %>
</asp:Content>
<%------------------------------------------------------------------------------------------------------------------------------%>