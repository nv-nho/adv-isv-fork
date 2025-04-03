<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagingHeaderControl.ascx.cs"
    Inherits="OMS.UserControls.PagingHeaderControl" %>
<asp:HiddenField ID="hdIsCloseForm" runat="server" />
<asp:HiddenField ID="hdIsShowColor" runat="server" />
<asp:HiddenField ID="hdAddClass" runat="server" />
<asp:HiddenField ID="hdCurrentPage" runat="server" />

<div class="row" style="height:40px;">
<% if (this.TotalRow > 0 || this.IsCloseForm)
    {%>

    <% if (IsShowColor)
        {
    %>
    <div class="col-sm-5">
        <span class="label alert-danger"><% =this.DangerText%></span>
        <span class="label alert-warning"><% =this.WarningText%></span>
        <span class="label alert-finish"><% =this.FinishText%></span>
        <span class="label alert-info"><% =this.InfoText%></span>
        <span class="label alert-success"><% =this.SuccessText%></span>
        <span class="label alert-primary"><% =this.PrimaryText%></span>
        <span class="label alert-light-gray"><% =this.LightGrayText%></span>
    </div>
    <%}
        else
        { %>
    <div class="col-sm-5">
    </div>
    <%} %>
    <div class="col-sm-7">
        <div class="btn-group pull-right">
            <div class='btn-group'>
                <asp:LinkButton ID="btn_Prev" runat="server" OnClick="Paging_OnClick">
                    <span class="glyphicon glyphicon-arrow-left loading"></span>
                </asp:LinkButton>
            </div>
            <div class="btn-group">
                <asp:LinkButton ID="btn_Next" runat="server" OnClick="Paging_OnClick">
                    <span class="glyphicon glyphicon-arrow-right loading"></span>
                </asp:LinkButton>
            </div>
            <% if (IsCloseForm)
                { %>
            <div class="btn-group">
                <asp:LinkButton ID="btnClose" runat="server" class="btn btn-block btn-default btn-sm loading"
                    OnClientClick="window.opener.hideLoading(); window.close();return false;">                                
                    <span class="glyphicon glyphicon-remove"></span>                
                </asp:LinkButton>
            </div>
            <%
                }
            %>
            <div class="btn-group">
                <button type="button" class="btn <%= AddClass %> btn-sm" disabled="disabled">
                    <%= RowNumFrom %>
                    ～
                    <%= RowNumTo %></button>
                <button type="button" class="btn <%= AddClass %> btn-sm" disabled="disabled">
                    <%= TotalRow %></button>
            </div>
            <div class="btn-group">
                <asp:DropDownList ID="dlTotalRowOnPage" runat="server" AutoPostBack="true" CssClass="form-control input-sm"
                    OnSelectedIndexChanged="dlTotalRowOnPage_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
    </div>

<%}%>
</div>