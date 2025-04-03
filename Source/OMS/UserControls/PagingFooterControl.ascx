<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagingFooterControl.ascx.cs"
    Inherits="OMS.UserControls.PagingFooterControl" %>

<asp:Repeater ID="rptPager" runat="server" 
    onitemdatabound="rptPager_ItemDataBound">
    <HeaderTemplate>
                <ul class="pagination pagination-sm">
    </HeaderTemplate>
    <ItemTemplate>
        <li runat="server" id="liPage">
            <asp:LinkButton ID="linkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                OnClick="Paging_Click" class="loading">
            </asp:LinkButton>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>