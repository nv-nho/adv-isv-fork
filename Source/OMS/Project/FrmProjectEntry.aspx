<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmProjectEntry.aspx.cs" Inherits="OMS.Project.FrmProjectEntry" %>

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

            <%if (this.Success == true)
        { %>
            showSuccess();
            setTimeout(function () {
                hideSuccess();
            }, 1000);
            <%} %>

            <%if (this.IsShowQuestion == true)
        { %>
            $('#modalQuestion').modal('show');

            $('#modalQuestion').on('shown.bs.modal', function (e) {
                $('<%=this.DefaultButton%>').focus();
            });

            <%} %>

            //検収フラグの値変更の処理
            <%if(this.Mode != Mode.View && this.Mode != Mode.Delete){ %>
                $("#<%= chkAcceptanceFlag.ClientID %>").bind('change', AcceptanceFlagChange);
                AcceptanceFlagChange();
            <%} %>
        });

        //***********************************************
        //検収フラグの値変更の処理
        //***********************************************
        function AcceptanceFlagChange()
        {
            var isChecked = getCtrlById("chkAcceptanceFlag").is(':checked'); 
            getCtrlById("dtAcceptanceDate").prop('readonly', !isChecked);
            if (!isChecked)
            {
                getCtrlById("dtAcceptanceDate").val("");
            }
        }

        //**********************
        // Set Focus
        //**********************
        function setFocus() {

            <%if (this.Mode == Mode.Insert || this.Mode == Mode.Copy)
        { %>
            getCtrlById("txtProjectCD").focus().select();
            <%} %>
//            <%if (this.Mode == Mode.View)
        { %>
//                getCtrlById("txtProjectCD").focus().select();
//            <%} %>
            <%if (this.Mode == Mode.Update)
        { %>
            getCtrlById("txtProjectCD").focus().select();
            <%} %>
        }

        //**********************
        // Find Back
        //**********************    
        function findBack() {
            hideLoading();
            getCtrlById("txtProjectCD").focus().select();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-3 col-sm-3">
                    <div class='form-group <%=GetClassError("txtProjectCD")%>'>
                        <label class="control-label" for="<%= txtProjectCD.ClientID %>">
                            コード <strong class="text-danger">*</strong></label>
                        <cc1:ICodeTextBox ID="txtProjectCD" runat="server" CodeType="AlphaNumeric" AjaxUrlMethod="GetProject" AllowChars="-"
                            CssClass="form-control input-sm" LabelNames="txtProjectCD"></cc1:ICodeTextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-12 col-sm-4">
                    <div class='form-group <%=GetClassError("txtProjectName")%>'>
                        <label class="control-label" for="<%= txtProjectName.ClientID %>">
                            プロジェクト名<strong class="text-danger">*</strong></label>
                        <cc1:ITextBox ID="txtProjectName" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-8 col-sm-9">
                <div class="col-md-5 col-sm-4">
                    <div class='form-group'>
                        <label class="control-label" for="<%= txtWorkPlace.ClientID %>">
                            作業場所
                        </label>
                        <cc1:ITextBox ID="txtWorkPlace" runat="server" MaxLength="50" CssClass="form-control input-sm"
                            Text=""></cc1:ITextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-8 col-sm-9">
                <%--Department--%>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label">
                            部門<strong class="text-danger">*</strong></label>
                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--UserID--%>
                <div class="col-md-4">
                    <div class="form-group <%=GetClassError("ddlUser")%> ">
                        <label class="control-label">
                            管理者<strong class="text-danger">*</strong></label>
                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control input-sm"
                            autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-8 col-sm-9">
                <%--Start date--%>
                <div class="col-md-3">
                    <div class='form-group <%=GetClassError("dtStartDate")%>'>
                        <label class="control-label" for="<%= dtStartDate.ClientID %>">
                            開始日<strong class="text-danger">*</strong></label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtStartDate" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <%--Kara--%>
                <div class="col-md-1">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            ～</label>
                    </div>
                </div>
                <%--End date--%>
                <div class="col-md-3">
                    <div class='form-group <%=GetClassError("dtEndDate")%>'>
                        <label class="control-label">
                            終了日</label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtEndDate" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-8 col-sm-9">
                <%--Delivery date--%>
                <div class="col-md-3">
                    <div class='form-group <%=GetClassError("dtDeliveryDate")%>'>
                        <label class="control-label" for="<%= dtDeliveryDate.ClientID %>">
                            納期</label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtDeliveryDate" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="true" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <%--Kara--%>
                <div class="col-md-1">
                    <div class='form-group' style="text-align: center; padding-top: 9px;">
                        <br />
                        <label>
                            </label>
                    </div>
                </div>


                <%--Acceptance date--%>
                <div class="col-md-4">
                    <div class='form-group <%=GetClassError("dtAcceptanceDate")%>'>
                        <label class="control-label" for="<%= dtAcceptanceDate.ClientID %>">
                            検収日（検収完了)</label>
                        <%--Acceptance flag--%>
                        <div class='form-group'>
                            <div class="col-md-3" style="padding-top: 4px;">
                                <input id="chkAcceptanceFlag" type="checkbox" runat="server"
                                    data-size="mini" />
                            </div>
                            <div class="input-group date">
                                <cc1:IDateTextBox ID="dtAcceptanceDate" runat="server" CssClass="form-control input-sm"
                                    PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div class="row">
            <%--Order amount--%>
            <div class="col-md-8 col-sm-9">
                <div class="col-md-4">
                    <div class="form-group <%=GetClassError("txtOrderAmount")%>">
                        <label class="control-label" for="<%= txtOrderAmount.ClientID %>">
                            受注金額<strong class="text-danger ">*</strong></label>
                        <cc1:INumberTextBox ID="txtOrderAmount" runat="server" CssClass="form-control input-sm"></cc1:INumberTextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <%--Memo--%>
            <div class="col-md-8 col-sm-9">
                <div class="col-md-7 col-sm-4">
                    <div class='form-group'>
                        <label class="control-label" for="<%= txtMemo.ClientID %>">
                            備考
                        </label>
                        <cc1:ITextBox ID="txtMemo" MaxLength="50" runat="server" CssClass="form-control input-sm"
                            Text=""></cc1:ITextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <%--Status Flag--%>
            <div class="col-md-8 col-sm-9">
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label input-group">
                            有効</label>
                        <input id="chkStatusFlag" type="checkbox" runat="server" data-on-color="success"
                            data-off-color="danger" data-size="mini" />
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
                            PostBackUrl="~/Project/FrmProjectList.aspx">
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
                            <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmProjectEntry.aspx"
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
