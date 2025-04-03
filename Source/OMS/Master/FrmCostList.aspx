<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmCostList.aspx.cs" Inherits="OMS.Master.FrmCostList" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="oms" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="pagingFooterControl" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="pagingHeaderControl" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="headerGridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script lang="javascript" type="text/javascript">
        /*
        * Load init
        */
        $(function () {
            setFocus();
        });

        /*
        * Set focus
        */
        function setFocus() {

        }

        //**********************
        // Clear Form
        //**********************
        function clearForm() {

            $(":text").val("");
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--Create Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-3">
                <div class="btn-group btn-group-justified">
                    <%--Back button--%>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" PostBackUrl="../Menu/FrmMasterMenu.aspx"
                            CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- List Detail--%>
    <pagingHeaderControl:PagingHeaderControl ID="PagingHeader" runat="server" />

    <headerGridControl:HeaderGridControl ID="HeaderGrid" runat="server" />

    <asp:Repeater ID="rptCostList" runat="server">
        <HeaderTemplate>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("RowNumber") %>
                    <asp:HiddenField ID="ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                </td>
                <td>
                    <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID") %>'
                        PostBackUrl="FrmCostDetail.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading">
                        <span class="glyphicon glyphicon-pencil"></span>
                    </asp:LinkButton>
                </td>
                <td>
                    <%# Eval("CostName")%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("EffectDate","{0:dd/MM/yyyy}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("ExpireDate","{0:dd/MM/yyyy}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("CostAmount", "{0:#,###,##0}"))%>
                </td>

            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>

    <pagingFooterControl:PagingFooterControl ID="PagingFooter" runat="server" />
</asp:Content>
