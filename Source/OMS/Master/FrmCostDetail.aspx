<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmCostDetail.aspx.cs" Inherits="OMS.Master.FrmCostDetail" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="oms" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="pagingFooterControl" %>
<%@ Import Namespace="OMS.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        //**********************
        // Init
        //**********************
        $(function () {
            //getCtrlById("chkStatusFlag").bootstrapSwitch();
            //setFocus();
            //focusErrors();

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

        //**********************
        // Load
        //**********************
        OnPageLoad = function () {
            initDetailList();
        }

        /*
        * Init detail list
        */
        function initDetailList() {
            var length = $(".dtEffectDate").length;
            var deleteFlagLength = $(".deleteFlag").length;

            for (i = 0; i < length; i++) {
                var dtEffectDate = $(".dtEffectDate").eq(i);
                var dtExpireDate = $(".dtExpireDate").eq(i);
                var inumCostAmount = $(".inumCostAmount").eq(i);

                <%if (this.Mode == Mode.View)
        { %>
                dtEffectDate.attr("readonly", "true");
                dtExpireDate.attr("readonly", "true");
                inumCostAmount.attr("readonly", "true");
                <%}
        else
        {%>
                dtEffectDate.removeAttr("readonly");
                dtExpireDate.removeAttr("readonly");
                inumCostAmount.removeAttr("readonly");

                dtExpireDate.attr("readonly", "true");
                dtExpireDate.attr("tabIndex", "-1");
                <%} %>
            }

            var dtEffectDate0 = $(".dtEffectDate").eq(length - 1);

            dtEffectDate0.attr("readonly", "true");

            var deleteFlag = $(".deleteFlag").eq(deleteFlagLength - 1);
            deleteFlag.attr("disabled", "true");
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <div class="well well-sm">
        <%--Cost Name--%>
        <div class="row">
            <div class="col-md-2">
                <div class='form-group <%=GetClassError("txtCostName")%>'>
                    <label class="control-label" for="<%= txtCostName.ClientID %>">原価名称</label>
                    <oms:ICodeTextBox ID="txtCostName" runat="server" CssClass="form-control input-sm" MaxLength="5"></oms:ICodeTextBox>
                </div>
            </div>
        </div>
    </div>

    <%if (this.Mode != Mode.View && this.Mode != Mode.Delete)
        { %>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-2">
                <div class="btn-group btn-group-justified">
                    <%--- Add row button---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnAddRow" runat="server" CssClass="btn btn-default btn-xs loading" OnClick="btnAddRow_Click">
                                            <span class="glyphicon glyphicon-plus"></span>&nbsp;Add row
                        </asp:LinkButton>
                    </div>
                    <%--- Delete row button ---%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnDeleteRow" runat="server" CssClass="btn btn-default btn-xs loading" OnClick="btnRemoveRow_Click">
                                            <span class="glyphicon glyphicon-remove"></span>&nbsp;Del row
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%} %>

    <div class="row">
        <label class="col-md-2 control-label">行数: <%= this.DetailLists.Count %> 行</label>
    </div>

    <%--Detail Input--%>
    <asp:Repeater ID="rptListCost" runat="server">

        <HeaderTemplate>
            <div class="" style="width: 700px;">
                <table class="table table-bordered table-condensed">
                    <%if (this.Mode != Mode.View && this.Mode != Mode.Delete)
                        { %>
                    <thead>
                        <tr id="trHeader1" runat="server">
                            <td style="width: 20px;"></td>
                            <td style="width: 10px;">
                                <label class="control-label">#</label>
                            </td>
                            <td style="width: 190px;">
                                <label class="control-label">開始日</label>
                                <%--Effect Date--%>
                            </td>
                            <td style="width: 190px;">
                                <label class="control-label">終了日</label>
                                <%--Expire Date--%>
                            </td>
                            <td style="width: 200px;">
                                <label class="control-label">原価（時給単価）<strong class="text-danger">*</strong></label>
                                <%--Cost Amount--%>
                            </td>
                        </tr>
                    </thead>
                    <%}
                        else
                        { %>
                    <thead>
                        <tr id="trHeader2" runat="server">
                            <td style="width: 10px;">
                                <label class="control-label">#</label>
                            </td>
                            <td style="width: 180px;">
                                <label class="control-label">開始日</label>
                                <%--Effect Date--%>
                            </td>
                            <td style="width: 180px;">
                                <label class="control-label">終了日</label>
                                <%--Expire Date--%>
                            </td>
                            <td style="width: 200px;">
                                <label class="control-label">原価（時給単価）<strong class="text-danger">*</strong></label>
                                <%--Cost Amount--%>
                            </td>
                        </tr>
                    </thead>
                    <%} %>
                    <tbody>
        </HeaderTemplate>

        <ItemTemplate>
            <tr>
                <%if (this.Mode != Mode.View && this.Mode != Mode.Delete)
                    { %>
                <%--- Delete checkbox ---%>
                <td style="text-align: center; vertical-align: middle;">
                    <input id="deleteFlag" class="deleteFlag" type="checkbox" runat="server" checked='<%# Eval("DelFlag")%>' />
                </td>
                <%} %>
                <td style="text-align: center; vertical-align: middle;">
                    <%--- # ---%>
                    <label class="control-label"><%# (this.CurrenIndexPage-1)* OMS.Models.Constant.DEFAULT_NUMBER_ROW + Container.ItemIndex + 1%></label>
                </td>
                <td>
                    <%--- Effect date ---%>
                    <div runat="server" id="divEffectDate">
                        <div class='input-group date' runat="server">
                            <oms:IDateTextBox ID="dtEffectDate" CssClass="dtEffectDate form-control input-sm" runat="server" MaxLength="10" Value='<%# Eval("EffectDate")%>' />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                        </div>
                    </div>
                </td>
                <td>
                    <%-- Expire date --%>
                    <div class='input-group date'>
                        <oms:IDateTextBox ID="dtExpireDate" CssClass="dtExpireDate form-control input-sm" runat="server" MaxLength="10" Value='<%# Eval("ExpireDate")%>' />
                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                    </div>
                </td>
                <td>
                    <%-- Cost amount --%>
                    <div runat="server" id="divCostAmount">
                        <label class="sr-only"></label>
                        <oms:INumberTextBox ID="inumCostAmount" CssClass="inumCostAmount form-control input-sm" runat="server"
                            MinimumValue="0"
                            MaximumValue="999999"
                            Value='<%# Eval("CostAmount")%>' />
                    </div>
                </td>
            </tr>
        </ItemTemplate>

        <FooterTemplate>
            </tbody>
            </table>
            </div>
        </FooterTemplate>
    </asp:Repeater>

    <%--Paging--%>
    <pagingFooterControl:PagingFooterControl ID="PagingFooter" runat="server" />

    <div class="row">
        <!-- Left Button Panel -->
        <div class="col-md-6">
            <div class="well well-sm">
                <div class="btn-group btn-group-justified">
                    <%
                        if (this.Mode == OMS.Utilities.Mode.View)
                        {
                    %>
                    <%--Button Edit--%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnEdit_Click">
                                <span class="glyphicon glyphicon-pencil"></span>&nbsp; 編集
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Update)
                        {
                    %>
                    <%-- Update button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-sm loading" OnClick="btnUpdate_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp; 登録
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                    %>
                    <%-- Back button --%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnBack_Click">
                                <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
