<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderGridControl.ascx.cs"
    Inherits="OMS.UserControls.HeaderGridControl" %>
<asp:Repeater ID="rptHGrid" runat="server" OnItemDataBound="rptHGrid_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-striped table-condensed">
            <thead>
                <tr>
    </HeaderTemplate>
    <ItemTemplate>
        <th runat="server" id="th">
            <strong>
                <div>
                    <asp:LinkButton ID="lnkSortCode" Text='<%# Eval("Text")%>' runat="server" CommandArgument='<%#Eval("ColumnIndex") %>'
                        OnClick="Sort_Click" class="loading">
                    </asp:LinkButton>
                </div>
            </strong>
        </th>
    </ItemTemplate>
    <FooterTemplate>
        </tr> </thead>
    </FooterTemplate>
</asp:Repeater>
