<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmExpenceEntry.aspx.cs" Inherits="OMS.Expence.FrmExpenceEntry" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Import Namespace="OMS.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        var findBackCtr = "dtAccountingDate";

        //**********************
        // Init
        //**********************
        $(function () {
            getCtrlById("chkStatusFlag").bootstrapSwitch();
            //getCtrlById("cmbExpenseType").bind("change", cmbExpenseTypeSelectedIndexChanged);
            setFocus();
            focusErrors();
            txtProjectCDOnChanged();
            HiddenTaxAmount();

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
        });

        function HiddenTaxAmount(){
            var tempTaxType;
            $(".valueTaxAmount").each(function (i) {
                tempTaxType = getCtrlById("MainContent_rptDetail_cmbTaxType_" + i).val();
                if (tempTaxType == 2) {
                    $('#MainContent_rptDetail_txtTaxAmount_' + i).addClass("hidden");
                    $('#MainContent_rptDetail_txtTaxAmountDisp_' + i).removeClass("hidden");
                }
                else {
                    $('#MainContent_rptDetail_txtTaxAmount_' + i);
                    $('#MainContent_rptDetail_txtTaxAmountDisp_' + i).addClass("hidden");
                }
                $('#MainContent_rptDetail_txtTaxAmountDisp_' + i).val("(" + $('#MainContent_rptDetail_txtTaxAmount_' + i).val());

            });
        }

        //**********************
        // Load
        //**********************
        OnPageLoad = function () {
            initDetailList();
        }

        function initDetailList() {
            var txtCtrl = $(".value");
            var comboCtrl = $(".combox");
            var taxRateCtrl = $(".comboxTaxRate");
            var taxAmountCtrl = $(".valueTaxAmount");
            var expenseTypeCtrl = $(".comboxExpenseType")
            <%if (this.Mode == Mode.View || this.Mode == Mode.Delete || this.Mode == Mode.Approve || this.Mode == Mode.NotApprove)
        { %>
            txtCtrl.attr("readonly", "true");
            comboCtrl.attr("disabled", "true");
            taxRateCtrl.attr("disabled", "true");
            taxAmountCtrl.attr("readonly", "true");
            <%}
        else
        {%>
            txtCtrl.removeAttr("readonly");

            <%} %>
        }


        //**********************
        // Set Focus
        //**********************
        function setFocus() {

            <%if (this.Mode == Mode.Insert || this.Mode == Mode.Copy || this.Mode == Mode.Update)
        { %>
            getCtrlById("dtAccountingDate").focus().select();
            <%} %>
        }

        //**********************
        // Find Back
        //**********************
        function findBack() {
            hideLoading();
            getCtrlById(findBackCtr).focus().select();
        }

        //**********************
        // Call Search Project
        //**********************
        function callSearchProject() {

            findBackCtr = "MainContent_rptDetail_dtDate_0";

            var projectCD = "";
            var projectName = "";
            var initDate = '1900/01/01';
            showSearchProject(projectCD, projectName, initDate, getCtrlById("txtProjectCode").attr("id"), getCtrlById("txtProjectName").attr("id"));
        };

        function convertToFloat(val) {
            if (val != '') {
                if (val.indexOf(',') !== -1)
                    val.replace(',', '');
                val = parseFloat(val);
                while (/(\d+)(\d{3})/.test(val.toString())) {
                    val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
                }
            }
            return val;
        }

        function TaxTypeChange(sender) {

            var taxAmountValue = 0;
            var Id = sender.id;
            var cmbTaxType = Id.replace('MainContent_rptDetail_cmbTaxType', 'MainContent_rptDetail_cmbTaxType');
            var cmbTaxRate = Id.replace('MainContent_rptDetail_cmbTaxType', 'MainContent_rptDetail_cmbTaxRate');
            var amount = Id.replace('MainContent_rptDetail_cmbTaxType', 'MainContent_rptDetail_txtAmount');
            var txtTaxAmount = Id.replace('MainContent_rptDetail_cmbTaxType', 'MainContent_rptDetail_txtTaxAmount');
            var txtTaxAmountDisp = Id.replace('MainContent_rptDetail_cmbTaxType', 'MainContent_rptDetail_txtTaxAmountDisp');


            var amount = $('#' + amount).val();
            var cmbTaxRateValue = $('#' + cmbTaxRate).val();
            var cmbTaxTypeValue = $('#' + cmbTaxType).val();

            var params1 = {
                'amountValue': amount,
                'cmbTaxRateValue': cmbTaxRateValue,
                'cmbTaxTypeValue': cmbTaxTypeValue
            };

            ajax("GetTaxAmountValue", params1, function (response) {
                if (response.d) {
                    var result1 = eval('(' + response.d + ')');
                    taxAmountValue = result1["taxAmountValue"];

                    $('#' + txtTaxAmount).val(taxAmountValue);
                    $('#' + txtTaxAmountDisp).val("(" + $('#' + txtTaxAmount).val());
                }
            });
            if (document.getElementById(cmbTaxType).value == 2) {
                $('#' + txtTaxAmount).addClass("hidden");
                $('#' + txtTaxAmountDisp).removeClass("hidden");
            }
            else
            {
                $('#' + txtTaxAmount).removeClass("hidden");
                $('#' + txtTaxAmountDisp).addClass("hidden");
            }

            if (document.getElementById(cmbTaxType).value == 3) {
                $('#' + cmbTaxRate).val("0");
                $('#' + txtTaxAmount).val(0);
                $('#' + txtTaxAmountDisp).val("(0");
                getCtrlById(cmbTaxRate).attr("disabled", true);
                getCtrlById(txtTaxAmount).attr("readonly", true);
            }
            else {
                getCtrlById(cmbTaxRate).attr("disabled", false);
                if (document.getElementById(cmbTaxRate).value != 0) {
                    getCtrlById(txtTaxAmount).attr("readonly", false);
                }
            }
            $('#' + txtTaxAmount).trigger('blur')


            var totalExpenceAmount = 0;
            var tempTaxType;

            $(".Amount").each(function (i) {
                tempTaxType = getCtrlById("MainContent_rptDetail_cmbTaxType_" + i).val();
                var aaa = getCtrlById("MainContent_rptDetail_txtTaxAmount_" + i).val();
                if (tempTaxType == 1) {
                    totalExpenceAmount += parseFloat($(this).val().replace(/,/gi, "")) + parseFloat(aaa.replace(/,/gi, ""));
                }
                else {
                    totalExpenceAmount += parseFloat($(this).val().replace(/,/gi, ""));
                }
            });
            var valuesum = totalExpenceAmount.toString();

            getCtrlById("txtExpenceAmount").val(convertToFloat(valuesum));
        }

        function TaxRateChange(sender) {

            var taxAmountValue = 0;
            var Id = sender.id;
            var amount = Id.replace('MainContent_rptDetail_cmbTaxRate', 'MainContent_rptDetail_txtAmount');
            var taxAmount = Id.replace('MainContent_rptDetail_cmbTaxRate', 'MainContent_rptDetail_txtTaxAmount');
            var taxAmountDisp = Id.replace('MainContent_rptDetail_cmbTaxRate', 'MainContent_rptDetail_txtTaxAmountDisp');
            var cmbTaxRate = Id.replace('MainContent_rptDetail_cmbTaxRate', 'MainContent_rptDetail_cmbTaxRate');
            var cmbTaxType = Id.replace('MainContent_rptDetail_cmbTaxRate', 'MainContent_rptDetail_cmbTaxType')

            var amount = $('#' + amount).val();
            var cmbTaxRateValue = sender.value
            var cmbTaxTypeValue = $('#' + cmbTaxType).val();

            var params1 = {
                'amountValue': amount,
                'cmbTaxRateValue': cmbTaxRateValue,
                'cmbTaxTypeValue': cmbTaxTypeValue
            };

            ajax("GetTaxAmountValue", params1, function (response) {
                if (response.d) {
                    var result1 = eval('(' + response.d + ')');
                    taxAmountValue = result1["taxAmountValue"];

                    $('#' + taxAmount).val(taxAmountValue);
                    $('#' + taxAmountDisp).val("(" + $('#' + taxAmount).val());


                }
            });
            if (document.getElementById(cmbTaxRate).value == 0) {
                $('#' + taxAmount).val(0);
                $('#' + taxAmountDisp).val("(0");
                getCtrlById(taxAmount).attr("readonly", true);
            }
            else {
                getCtrlById(taxAmount).attr("readonly", false);

            }
            $('#' + taxAmount).trigger('blur')

            var totalExpenceAmount = 0;
            var tempTaxType;

            $(".Amount").each(function (i) {
                tempTaxType = getCtrlById("MainContent_rptDetail_cmbTaxType_" + i).val();
                var aaa = getCtrlById("MainContent_rptDetail_txtTaxAmount_" + i).val();
                if (tempTaxType == 1) {
                    totalExpenceAmount += parseFloat($(this).val().replace(/,/gi, "")) + parseFloat(aaa.replace(/,/gi, ""));
                }
                else {
                    totalExpenceAmount += parseFloat($(this).val().replace(/,/gi, ""));
                }
            });
            var valuesum = totalExpenceAmount.toString();

            getCtrlById("txtExpenceAmount").val(convertToFloat(valuesum));

        }


        //**********************
        // Amount Change
        //**********************
        function AmountChange(sender) {


            var taxAmountValue = 0;
            var Id = sender.id;
            var amount = Id.replace('MainContent_rptDetail_txtAmount', 'MainContent_rptDetail_txtAmount');
            var taxAmount = Id.replace('MainContent_rptDetail_txtAmount', 'MainContent_rptDetail_txtTaxAmount');
            var taxAmountDisp = Id.replace('MainContent_rptDetail_txtAmount', 'MainContent_rptDetail_txtTaxAmountDisp');
            var cmbTaxRate = Id.replace('MainContent_rptDetail_txtAmount', 'MainContent_rptDetail_cmbTaxRate');
            var cmbTaxType = Id.replace('MainContent_rptDetail_txtAmount', 'MainContent_rptDetail_cmbTaxType');

            var amount = sender.value;
            var cmbTaxRateValue = $('#' + cmbTaxRate).val();
            var cmbTaxTypeValue = $('#' + cmbTaxType).val();

            var params1 = {
                'amountValue': amount,
                'cmbTaxRateValue': cmbTaxRateValue,
                'cmbTaxTypeValue': cmbTaxTypeValue
            };

            ajax("GetTaxAmountValue", params1, function (response) {
                if (response.d) {
                    var result1 = eval('(' + response.d + ')');
                    taxAmountValue = result1["taxAmountValue"];

                    $('#' + taxAmount).val(taxAmountValue);
                    $('#' + taxAmountDisp).val("(" + $('#' + taxAmount).val());
                }
            });
            $('#' + taxAmount).trigger('blur')

            var totalExpenceAmount = 0;
            var tempTaxType;

            $(".Amount").each(function (i) {
                tempTaxType = getCtrlById("MainContent_rptDetail_cmbTaxType_" + i).val();
                var aaa = getCtrlById("MainContent_rptDetail_txtTaxAmount_" + i).val();
                if (tempTaxType == 1) {
                    totalExpenceAmount += parseFloat($(this).val().replace(/,/gi, "")) + parseFloat(aaa.replace(/,/gi, ""));
                }
                else {
                    totalExpenceAmount += parseFloat($(this).val().replace(/,/gi, ""));
                }
            });
            var valuesum = totalExpenceAmount.toString();

            getCtrlById("txtExpenceAmount").val(convertToFloat(valuesum));

        }

        //**********************
        // Amount Change
        //**********************
        function TaxAmountChange(sender) {

            var totalExpenceAmount = 0;
            var tempTaxType;

            $(".Amount").each(function (i) {
                tempTaxType = getCtrlById("MainContent_rptDetail_cmbTaxType_" + i).val();
                var aaa = getCtrlById("MainContent_rptDetail_txtTaxAmount_" + i).val();
                if (tempTaxType == 1) {
                    totalExpenceAmount += parseFloat($(this).val().replace(/,/gi, "")) + parseFloat(aaa.replace(/,/gi, ""));
                }
                else {
                    totalExpenceAmount += parseFloat($(this).val().replace(/,/gi, ""));
                }
            });
            var valuesum = totalExpenceAmount.toString();

            getCtrlById("txtExpenceAmount").val(convertToFloat(valuesum));

        }


        //**********************
        // Project CD On Changed
        //**********************
        function txtProjectCDOnChanged() {
            getCtrlById("txtProjectCode").bind("change", function () {

                if ($.trim(this.value) != "") {
                    var params = { 'projectCD': $.trim(this.value) };
                    ajax('GetProjectName', params, function (response) {
                        if (response.d) {
                            var result = eval('(' + response.d + ')');
                            getCtrlById("txtProjectCode").val(result['projectCD']);
                            getCtrlById("txtProjectName").val(result['projectNm']);
                        }
                    });
                } else {
                    getCtrlById("txtProjectName").val('');
                }
            });
        }

    </script>
    <style>
        .modal-body
        {
            overflow-y: hidden;
        }
        @media (max-width:767px) {
            #modalQuestion .modal-dialog {
                margin: auto 15%;
                width: 70%;
            }
        }

        @media (min-width: 768px) {
            #modalQuestion .modal-dialog {
                margin: auto 15%;
                width: 70%;
            }
        }

        @media (min-width: 992px) {
            #modalQuestion .modal-dialog {
                margin: auto 25%;
                width: 50%;
            }
        }

        @media (min-width: 1290px) {
            #modalQuestion .modal-dialog {
                margin: auto 30%;
                width: 30%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <asp:HiddenField ID="hdInValideDefault" runat="server" />
    <div class="well well-sm">
        <div class="row">
            <%-- --%>
            <div class="col-md-4 col-sm-9">
                <div class="col-md-8">
                    <div class='form-group'>
                        <label class="control-label" for="<%= txtExpenceNo.ClientID %>">
                            経費番号
                        </label>
                        <cc1:ITextBox ID="txtExpenceNo" runat="server" CssClass="form-control input-sm" LabelNames="txtExpenceNo"></cc1:ITextBox>
                    </div>
                </div>
            </div>
            <%-- --%>
            <div class="col-md-8 col-sm-9">
                <div class="col-md-4">
                    <div class='form-group <%=GetClassError("dtAccountingDate")%>'>
                        <label class="control-label" for="<%= dtAccountingDate.ClientID %>">
                            計上日<strong class="text-danger">*</strong></label>
                        <div class="input-group date">
                            <cc1:IDateTextBox ID="dtAccountingDate" runat="server" CssClass="form-control input-sm"
                                PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <%-- --%>
            <div class="col-md-4 col-sm-9">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="control-label" for="<%= cmbExpenseType.ClientID %>">
                            経費種類<strong class="text-danger">*</strong></label>
                        <asp:DropDownList ID="cmbExpenseType" runat="server" CssClass="form-control input-sm"
                            AutoPostBack="True" OnSelectedIndexChanged="cmbExpenseType_SelectedIndexChanged"
                            autocomplete="off">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-8 col-sm-9">
                <%--User code--%>
                <div class="col-md-4">
                    <div class='form-group <%=GetClassError("cmbUser")%>'>
                        <label class="control-label" for="<%= cmbUser.ClientID %>">
                            支払先（社員）<strong class="text-danger">*</strong>
                        </label>
                        <asp:DropDownList ID="cmbUser" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--user name --%>
                <div class="col-md-7">

                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 col-sm-9">
                <%--Department--%>
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="control-label" for="<%= ddlDepartment.ClientID %>">
                            負担部門<strong class="text-danger">*</strong>
                        </label>
                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-8 col-sm-9">
                <%--Project Code--%>
                <div class="col-md-4">
                    <div class='form-group <%=GetClassError("txtProjectCode")%>'>
                        <label class="control-label" for="<%= txtProjectCode.ClientID %>">
                            負担プロジェクト<strong class="text-danger">*</strong>
                        </label>
                        <div class="input-group">
                            <span class="input-group-btn">
                                <button id="btnProjectSearch" runat="server" class="btn btn-default btn-sm loading" type="button" onclick="callSearchProject();return false;">
                                    <span class="glyphicon glyphicon-search"></span>
                                </button>
                            </span>
                            <cc1:ICodeTextBox ID="txtProjectCode" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm"
                                SearchButtonID="btnProjectSearch" AllowChars="-" />
                        </div>
                    </div>
                </div>
                <%--Project Name--%>
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtProjectName.ClientID %>">
                            &nbsp;
                        </label>
                        <cc1:ITextBox ID="txtProjectName" ReadOnly="true" runat="server" TabIndex="-1" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 col-sm-9">
                <%-- --%>
                <div class="col-md-7">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtExpenceAmount.ClientID %>">
                            合計金額
                        </label>
                        <cc1:INumberTextBox ID="txtExpenceAmount" runat="server" CssClass="form-control input-sm"></cc1:INumberTextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%if (this.Mode != Mode.View && this.Mode != Mode.Delete && this.Mode != Mode.Approve && this.Mode != Mode.NotApprove)
        { %>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-4 col-sm-6 col-xs-12">
                <div class="btn-group btn-group-justified">
                    <%---  ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnAddRow" runat="server" CssClass="btn btn-default btn-xs loading" OnClick="btnAddRow_Click">
                                            <span class="glyphicon glyphicon-plus"></span>&nbsp;行追加
                        </asp:LinkButton>
                    </div>
                    <%--- ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnDeleteRow" runat="server" CssClass="btn btn-default btn-xs loading" OnClick="btnRemoveRow_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;行削除
                        </asp:LinkButton>
                    </div>
                    <% %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnUpTop" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnUp_Click">
                                <span class="glyphicon glyphicon-arrow-up"></span>&nbsp;上へ
                        </asp:LinkButton>
                    </div>
                    <% %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnDownTop" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnDown_Click">
                                <span class="glyphicon glyphicon-arrow-down"></span>&nbsp;下へ
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%} %>
    <div style="width:1138px;">
        <asp:Repeater ID="rptDetail" runat="server" OnItemDataBound="rptDetail_ItemDataBound">
            <HeaderTemplate>

                <table class="table table-bordered table-condensed">
                    <%if (this.Mode != Mode.View && this.Mode != Mode.Delete && this.Mode != Mode.Approve && this.Mode != Mode.NotApprove)
                        {

                            if (listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
                            {
                    %>
                    <thead>
                        <tr id="trHeader1" runat="server">
                            <td style="width: 10px;">
                                <label class="control-label">#</label>
                            </td>
                            <td style="width: 20px;"></td>
                            <td>
                                <label class="control-label">利用日<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">支払先<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">経路<strong class="text-danger"></strong></label>
                            </td>
                            <td>
                                <label class="control-label">区分</label>
                            </td>
                            <td>
                                <label class="control-label">税区分</label>
                            </td>
                            <td>
                                <label class="control-label">税率</label>
                            </td>
                            <td>
                                <label class="control-label">金額<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">消費税額</label>
                            </td>
                        </tr>
                    </thead>
                    <%}
                        else
                        {
                    %>
                    <thead>
                        <tr id="tr1" runat="server">
                            <td style="width: 10px;">
                                <label class="control-label">#</label>
                            </td>
                            <td style="width: 20px;"></td>
                            <td>
                                <label class="control-label">利用日<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">支払先<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">目的</label>
                            </td>
                            <td>
                                <label class="control-label">税区分</label>
                            </td>
                            <td>
                                <label class="control-label">税率</label>
                            </td>
                            <td>
                                <label class="control-label">金額<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">消費税額</label>
                            </td>
                        </tr>
                    </thead>
                    <%} %>
                    <%}
                        else
                        {
                            if (listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
                            {
                    %>
                    <thead>
                        <tr id="trHeader2" runat="server">
                            <td style="width: 10px;">
                                <label class="control-label">#</label>
                            </td>
                            <td>
                                <label class="control-label">利用日<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">支払先<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">経路<strong class="text-danger"></strong></label>
                            </td>
                            <td>
                                <label class="control-label">区分</label>
                            </td>
                            <td>
                                <label class="control-label">税区分</label>
                            </td>
                            <td>
                                <label class="control-label">税率</label>
                            </td>
                            <td>
                                <label class="control-label">金額<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">消費税額</label>
                            </td>
                        </tr>
                    </thead>
                    <%}
                        else
                        { %>
                    <thead>
                        <tr id="tr2" runat="server">
                            <td style="width: 10px;">
                                <label class="control-label">#</label>
                            </td>
                            <td>
                                <label class="control-label">利用日<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">支払先<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">目的</label>
                            </td>
                            <td>
                                <label class="control-label">税区分</label>
                            </td>
                            <td>
                                <label class="control-label">税率</label>
                            </td>
                            <td>
                                <label class="control-label">金額<strong class="text-danger">*</strong></label>
                            </td>
                            <td>
                                <label class="control-label">消費税額</label>
                            </td>
                        </tr>
                    </thead>
                    <%} %>
                    <%} %>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <%--- # ---%>
                    <td style="text-align: center; vertical-align: middle;">
                        <label class="control-label"><%# Container.ItemIndex + 1%></label>
                    </td>
                    <%--- Delete checkbox ---%>
                    <%if (this.Mode != Mode.View && this.Mode != Mode.Delete && this.Mode != Mode.Approve && this.Mode != Mode.NotApprove)
                        { %>
                    <td style="text-align: center; vertical-align: middle;">
                        <input id="deleteFlag" class="deleteFlag" type="checkbox" runat="server" checked='<%# Eval("DelFlag")%>'
                            data-size="mini" data-on-color="success" data-off-color="danger" />
                    </td>
                    <%} %>
                    <%-- --%>
                    <td>
                        <div style="width: 140px;">
                            <div runat="server" id="divdtDate">
                                <div class='input-group date' runat="server">
                                    <cc1:IDateTextBox ID="dtDate" CssClass="value form-control input-sm" runat="server" MaxLength="10" Value='<%# Eval("Date")%>' />
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                        </div>
                    </td>
                    <%-- --%>
                    <td>
                        <div style="width: 180px;">
                            <div runat="server" id="divPaidTo">
                                <label class="sr-only"></label>
                                <cc1:ITextBox ID="txtPaidTo" CssClass="value form-control input-sm" runat="server" MaxLength="100" Value='<%# Eval("PaidTo")%>' />
                            </div>
                        </div>
                    </td>
                    <%if (listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
                        { %>
                    <%-- --%>
                    <td>
                        <div style="width: 260px;">
                            <div style="width: 105px;display:inline-block;">
                                <div runat="server" id="divRouteFrom">
                                    <label class="sr-only"></label>
                                    <cc1:ITextBox ID="txtRouteFrom" CssClass="value form-control input-sm" runat="server" MaxLength="100" Value='<%# Eval("RouteFrom")%>' />
                                </div>
                            </div>
                            <div style="width: 40px;display:inline-block; text-align:center;">
                            <label>～</label>
                            </div>
                            <div style="width: 105px;display:inline-block;">
                                <div runat="server" id="divRouteTo">
                                    <label class="sr-only"></label>
                                    <cc1:ITextBox ID="txtRouteTo" CssClass="value form-control input-sm" runat="server" MaxLength="100" Value='<%# Eval("RouteTo")%>' />
                                </div>
                            </div>
                        </div>
                    </td>
                    <%-- --%>
                    <td>
                        <div style="width: 70px;">
                            <div runat="server" id="divRouteType">
                                <label class="sr-only"></label>
                                <asp:DropDownList ID="cmbRouteType" runat="server" CssClass="combox form-control input-sm">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <%}
                        else
                        {
                    %>
                    <td>
                        <div style="width: 330px;">
                            <div runat="server" id="divNote">
                                <label class="sr-only"></label>
                                <cc1:ITextBox ID="txtNote" CssClass="value form-control input-sm" runat="server" MaxLength="300" Value='<%# Eval("Note")%>' />
                            </div>
                        </div>
                    </td>
                    <%} %>

                    <%-- --%>
                    <td>
                        <div style="width: 70px;">
                            <div runat="server" id="divTaxType">
                                <label class="sr-only"></label>
                                <asp:DropDownList ID="cmbTaxType" runat="server" CssClass="combox form-control input-sm comboxExpenseType" onchange="TaxTypeChange(this)">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <%-- --%>
                    <td>
                        <div style="width: 70px;">
                            <div runat="server" id="divTaxRate">
                                <label class="sr-only"></label>
                                <asp:DropDownList ID="cmbTaxRate" runat="server" CssClass="comboxTaxRate form-control input-sm comboxTaxRate" onchange="TaxRateChange(this)">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <%-- --%>
                    <td>
                        <div style="width: 130px;">
                            <div runat="server" id="divAmount">
                                <label class="sr-only"></label>
                                <cc1:INumberTextBox ID="txtAmount" CssClass="value form-control input-sm Amount" runat="server" MaximumValue="99999999" onvaluechange="AmountChange" Value='<%# Eval("Amount")%>'>
                                </cc1:INumberTextBox>
                            </div>
                        </div>
                    </td>
                    <%-- --%>
                    <td>
                        <div runat="server" id="divTaxAmount">
                            <label class="sr-only"></label>
                            <cc1:INumberTextBox ID="txtTaxAmount" CssClass="valueTaxAmount form-control input-sm" runat="server" onchange="TaxAmountChange(this)"
                                MaximumValue="10000000"
                                Value='<%# Eval("TaxAmount")%>'>
                            </cc1:INumberTextBox>
                            <cc1:ITextBox ID="txtTaxAmountDisp" CssClass="form-control input-sm text-right" runat="server" ReadOnly="true"
                                Value='<%# "(" + Eval("TaxAmount", "{0:#,##0}")%>'>
                            </cc1:ITextBox>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                    </table>

            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="well-sm">
        <div class="row">
            <%--- Remarks ---%>
            <div class="col-md-8">
                <div class="form-group">
                    <label class="control-label" for="<%= txtMemo.ClientID %>">
                        備考 <strong class="text-danger"></strong>
                    </label>
                    <cc1:ITextBox ID="txtMemo" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Rows="5" MaxLength="300" />
                </div>
            </div>
        </div>
    </div>

    <!-- /form well-->
    <%
        if (this.Mode != OMS.Utilities.Mode.Delete && this.Mode != OMS.Utilities.Mode.Approve && this.Mode != OMS.Utilities.Mode.NotApprove)
        {
    %>
    <!-- /form well-->

    <div class="well well-sm">
        <div class="row">
            <div class="col-md-6">
                <div class="btn-group btn-group-justified">
                    <%
                        if (this.Mode == OMS.Utilities.Mode.View)
                        {
                    %>
                    <%--Button Edit--%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnEdit_Click">
                                <span class="glyphicon glyphicon-pencil"></span> 編集
                        </asp:LinkButton>
                    </div>
                    <%-- Copy button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnCopy" runat="server" CssClass="btn btn-default btn-sm loading"
                            OnClick="btnCopy_Click">
                                    <span class="glyphicon glyphicon-paperclip"></span>&nbsp;コピー
                        </asp:LinkButton>
                    </div>
                    <%--Button Delete--%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnDelete_Click">
                                <span class="glyphicon glyphicon-trash"></span> 削除
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Insert || this.Mode == OMS.Utilities.Mode.Copy)
                        {
                    %>
                    <%--- Insert button ---%>
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
                    <%--- Update button ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm loading" OnClick="btnUpdate_Click">
                                <span class="glyphicon glyphicon-ok"></span> 登録
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                    %>
                    <%--- Back button ---%>
                    <%
                        if (this.Mode == OMS.Utilities.Mode.View || this.Mode == OMS.Utilities.Mode.Insert)
                        {
                    %>

                    <div class="btn-group">
                        <asp:LinkButton ID="btnBackNew" runat="server" CssClass="btn btn-default btn-sm loading"
                            PostBackUrl="~/Expence/FrmExpenceList.aspx">
                                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>

                    <%
                        }
                        else
                        {
                    %>
                    <div class="btn-group">
                        <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnBack_Click">
                                <span class="glyphicon glyphicon-chevron-left"></span> 戻る
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                    %>
                </div>
            </div>
            <div class="col-md-4">
            </div>
            <div class="col-md-2">
                <div class="btn-group btn-group-justified">
        <%
            if (this.Mode == OMS.Utilities.Mode.View)
            {
                if (this.ApprovedFlag == 1)
                {
        %>
                        <asp:LinkButton ID="btnApproveDelete" runat="server" CssClass="btn btn-danger btn-sm loading" OnClick="btndenied_Click" Text="承認解除"></asp:LinkButton>
        <%
                }
                else {
        %>
                        <asp:LinkButton ID="btnApprove" runat="server" CssClass="btn btn-success btn-sm loading" OnClick="btnApprove_Click" Text="承認"></asp:LinkButton>
        <%
                }
            }
        %>
                  </div>
            </div>
        </div>
    </div>
    <% } %>
</asp:Content>
