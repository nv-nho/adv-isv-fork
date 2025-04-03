<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="OMS.FrmErrorPages.Oops" MasterPageFile="~/Site.Master"  %>

<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">

     <div class="panel panel-default">
        <div class="panel-heading" style="color:Red;font-size:medium;font-weight:bold;">
            <span class="glyphicon glyphicon-exclamation-sign"></span>&nbsp;System error / Lỗi
        </div>
        <div class="panel-body error-panel">
             <div style="padding:10px;">
                <%--An unexpected error occurred on our website. Please try again. If the problem continues, contact the person who manages your server.--%>
                    <%= this.ErrorMessage%>
                </div>
             <ul>
                <li>
                    <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="~/Menu/FrmMainMenu.aspx">Return to the homepage</asp:HyperLink>
                </li>
            </ul>
        </div>
     </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style >
        .error-panel 
        {
            color: #312124;      
            font-size: 12.5px;
            line-height:23px;
            font-weight: bold;      
            text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.1);
            vertical-align: baseline;
            min-height:200px;
        }
    </style>
</asp:Content>