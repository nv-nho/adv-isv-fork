<%@ Page Title="" Language="C#" MasterPageFile="~/SiteSearch.Master" AutoEventWireup="true"
    CodeBehind="FrmUserSearch.aspx.cs" Inherits="OMS.Search.FrmUserSearch" %>

    <%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="pagingFooter" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    
    <script type="text/javascript">
    $(function () {
//        getCtrlById("txtGroupCode").attr("readonly", "true");
        getCtrlById("txtGroupName").attr("readonly", "true");
        getCtrlById("txtDepartmentName").attr("readonly", "true");          
        setFocus();
    });

    //Clear form
    function setFocus() {
        getCtrlById("txtUserCD").focus().select();
    }

    //Select row on grid
    function selectRow(index) {

        window.opener.$('#' + getCtrlById("Out1").val()).val(getCtrlById("UserCD", index).val());
        window.opener.$('#' + getCtrlById("Out2").val()).val(getCtrlById("UserName1", index).val());
        window.opener.$('#' + getCtrlById("Out3").val()).val(getCtrlById("UserName2", index).val());
        window.opener.$('#' + getCtrlById("Out4").val()).val(getCtrlById("GroupCD", index).val());
        window.opener.$('#' + getCtrlById("Out5").val()).val(getCtrlById("DepartmentCD", index).val());
        window.opener.findBack();
        closeWindow();
    }



    //Clear form
    function clearForm() {
        $(":text").val("");
        getCtrlById("txtUserCD").focus().select();
    }

    //Call Search Group User
    function callSearchGroupUser() {
        //2018.01.18 nv-nho search all
        //var groupCD = getCtrlById("txtGroupCD").val();
        //var groupName = getCtrlById("txtGroupName").val();
        var groupCD = "";
        var groupName = "";
        showSearchGroupUser(groupCD, groupName, getCtrlById("txtGroupCD").attr("id"), getCtrlById("txtGroupName").attr("id"));
    };

    //**********************
    // Call Search Department User
    //**********************
    function callSearchDepartment() {
        //2018.01.18 nv-nho search all
        //var departmentCD = getCtrlById("txtDepartmentCode").val();
        //var departmentName = getCtrlById("txtDepartmentName").val();
        var departmentCD = "";
        var departmentName = "";
        showSearchDepartment(departmentCD, departmentName, getCtrlById("txtDepartmentCode").attr("id"), getCtrlById("txtDepartmentName").attr("id"));
    };

    //
    function findBack() {
        getCtrlById("txtUserCD").focus().select();
    }

</script>
</asp:Content>

<%--Title Web--%>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleWeb" runat="server">
    User Search</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="server">
    User Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="Out1" runat="server" />
    <asp:HiddenField ID="Out2" runat="server" />
    <asp:HiddenField ID="Out3" runat="server" />
    <asp:HiddenField ID="Out4" runat="server" />
     <asp:HiddenField ID="Out5" runat="server" />
 <div class="well well-sm">
    <!--Collapse Button-->
        <button id="viewDetailPress" type="button" class="btn btn-default btn-xs" data-toggle="collapse"
            data-target="#viewdetails">
            <span class="glyphicon glyphicon-align-justify"></span>
        </button>
        <div class="collapse <%= this.Collapse %>" id="viewdetails">
             <div class="row">
                 <%--UserCD--%>
                <div class="col-md-1">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtUserCD.ClientID %>">
                            User Code</label>
                        <cc1:ICodeTextBox ID="txtUserCD" Text= "" FillChar='0' runat="server" CssClass="form-control input-sm"
                            CodeType="Numeric"></cc1:ICodeTextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                 <%--LoginID--%>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtLoginID.ClientID %>">
                            LoginID</label>
                        <cc1:ITextBox ID="txtLoginID" Text= "" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                 <%--UserName1--%>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtUserName1.ClientID %>">
                            User Name</label>
                        <cc1:ITextBox ID="txtUserName1" Text= "" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    </div>
                </div>
            </div>
            <%--<div class="row">--%>
                <%--UserName2--%>
                <%--<div class="col-md-1">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtUserName2.ClientID %>">
                            User Name 2</label>
                        <cc1:ITextBox ID="txtUserName2" Text= "" runat="server" CssClass="form-control input-sm" ></cc1:ITextBox>
                    </div>
                </div>--%>
            <%--</div>--%>
            <div class="row">
                <%--department--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtDepartmentCD.ClientID %>">
                            Department</label>
                        <div class="input-group">
                            <span class="input-group-btn">
                                <button class="btn btn-default btn-sm loading" type="button" onclick="callSearchDepartment(); return false;">
                                        <span class="glyphicon glyphicon-search"></span>
                                </button>
                            </span>
                            <cc1:ICodeTextBox ID="txtDepartmentCD" Text= "" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm" LabelNames="departmentCD:txtDepartmentCD,groupName:txtDepartmentName" AjaxUrlMethod="ShowDepartmentName"></cc1:ICodeTextBox>
                            
                        </div>
                    </div>
                </div>
                <%--depaermentName--%>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label">&nbsp;</label>
                        <cc1:ITextBox ID="txtDepartmentName" Text= "" runat="server" CssClass="form-control input-sm" TabIndex="-1"></cc1:ITextBox>
                    </div>
                </div>
             </div>
            <div class="row">
                <%--Group--%>
                <div class="col-md-2">
                    <div class="form-group">
                        <label class="control-label" for="<%= txtGroupCD.ClientID %>">
                            Group</label>
                        <div class="input-group">
                            <span class="input-group-btn">
                                <button class="btn btn-default btn-sm loading" type="button" onclick="callSearchGroupUser(); return false;">
                                        <span class="glyphicon glyphicon-search"></span>
                                </button>
                            </span>
                            <cc1:ICodeTextBox ID="txtGroupCD" Text= "" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm" LabelNames="groupCD:txtGroupCD,groupName:txtGroupName" AjaxUrlMethod="ShowGroupName"></cc1:ICodeTextBox>
                            
                        </div>
                    </div>
                </div>
                <%--Group--%>
                <div class="col-md-5">
                    <div class="form-group">
                        <label class="control-label">&nbsp;</label>
                        <cc1:ITextBox ID="txtGroupName" Text= "" runat="server" CssClass="form-control input-sm" TabIndex="-1"></cc1:ITextBox>
                    </div>
                </div>
             </div>
               
             <%--Search and Clear Button--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="btn-group">
                        <button type="button" id="btnSearch" class="btn btn-default btn-sm loading" runat="server">
                            <span class="glyphicon glyphicon-search"></span>&nbsp;Search
                        </button>
                        <button type="button" class="btn btn-default btn-sm" onclick="clearForm();">
                            <span class="glyphicon glyphicon-refresh"></span>&nbsp;Clear
                        </button>
                    </div>
                </div>
            </div>
      </div>
 </div>

 <!-- / collapse-->
    <uc1:PagingHeaderControl ID="PagingHeader" runat="server" />
    <uc2:HeaderGridControl ID="HeaderGrid" runat="server" />
    <asp:Repeater ID="rptUserList" runat="server">
        <HeaderTemplate>
           <tbody>
        </HeaderTemplate>
        <ItemTemplate>    
            <tr>
                  <td>
                      <%#Eval("RowNumber")%>
                  </td>
                  <td>
                     <button type="button" class="btn btn-info btn-xs" onclick="selectRow('<%#Container.ItemIndex%>');" >
                        <span class="glyphicon glyphicon-ok"></span>
                    </button>
                  </td>
                  <td>
                    <asp:HiddenField  ID="UserCD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "UserCD") %>' />
                    <%#Eval("UserCD")%>
                  </td>
                  <td>
                    <asp:HiddenField ID="LoginID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "LoginID") %>' />
                     <%# Server.HtmlEncode(Eval("LoginID", "{0}"))%>
                  </td>
                  <td>
                    <asp:HiddenField ID="UserName1" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "UserName1") %>' />
                    <%# Server.HtmlEncode(Eval("UserName1", "{0}"))%>
                  </td>
                  <td>
                    <asp:HiddenField ID="UserName2" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "UserName2") %>' />
                    <%# Server.HtmlEncode(Eval("UserName2", "{0}"))%>
                  </td>
                  <td>
                    <asp:HiddenField ID="GroupCD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "GroupCD") %>' />
                    <asp:HiddenField ID="GroupName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "GroupName") %>' />
                    <%# Server.HtmlEncode(Eval("GroupName", "{0}"))%>
                  </td>
                  <td>
                    <asp:HiddenField ID="DepartmentCD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "DepartmentCD") %>' />
                    <asp:HiddenField ID="DepartmentName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "DepartmentName") %>' />
                    <%# Server.HtmlEncode(Eval("DepartmentName", "{0}"))%>
                  </td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
    </asp:Repeater>  
   <pagingFooter:PagingFooterControl ID="PagingFooter" runat="server" />  
</asp:Content>

