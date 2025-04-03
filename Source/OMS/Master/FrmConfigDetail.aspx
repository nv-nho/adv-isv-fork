<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="FrmConfigDetail.aspx.cs" Inherits="OMS.Master.FrmConfigDetail" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>

<%@ Import Namespace="OMS.Utilities" %>

<asp:Content ID="ctHeader" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        /*
        * Init
        */
        $(function () {
        
            initDetailList();
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

        /*
        * Set focus
        */
        function setFocus() {
            <%if(this.Mode == Mode.Insert || this.Mode == Mode.Copy){ %>
                getCtrlById("txtConfigCD").focus().select();
            <%} %>
//            <%if(this.Mode == Mode.View){ %>
//                getCtrlById("btnEdit").focus().select();
//            <%} %>
            <%if(this.Mode == Mode.Update){ %>
                getCtrlById("txtConfigName").focus().select();
            <%} %>
        }

        /*
        * Init detail list
        */
        function initDetailList(){
            var detailCtrl = $(".value");
            <%if(this.Mode == Mode.View || this.Mode == Mode.Delete){ %>
                detailCtrl.attr("readonly","true");
            <%} else{%>
                detailCtrl.removeAttr("readonly");
            <%} %>
        }

    </script>
</asp:Content>

<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
     <div class="well well-md">
            <div class="row">
                <%--Config Code--%>
                <div class="col-md-2">
                    <div class='form-group <%=GetClassError("txtConfigCD")%>'>
                        <label class="control-label" for="<%= txtConfigCD.ClientID %>">
                            Config Code <strong class="text-danger"> *</strong></label>
                        
                        <cc1:ICodeTextBox ID="txtConfigCD" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm"></cc1:ICodeTextBox>

                        <%--<%=GetSpanError("txtConfigCD")%>--%>
                    </div>
                </div>
                <%-- Config Name --%>
                <div class="col-md-4">
                    <div class='form-group <% =GetClassError("txtConfigName")%>'>
                        <label class="control-label" for="<%= txtConfigName.ClientID %>">
                            Config Name <strong class="text-danger"> *</strong></label>
                       
                        <cc1:ITextBox ID="txtConfigName" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                        <%--<%=GetSpanError("txtConfigName")%>--%>
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
                            <div class="btn-group">
                            <asp:LinkButton ID="btnAddRow" runat="server" CommandName="Add" CssClass="btn btn-default btn-xs loading" OnClick="btnAddRow_Click">
                                    <span class="glyphicon glyphicon-plus"></span>&nbsp;Add row
                            </asp:LinkButton>
                            </div>
                            <div class="btn-group">
                            <asp:LinkButton ID="btnDeleteRow" runat="server" CommandName="Add" CssClass="btn btn-default btn-xs loading" OnClick="btnRemoveRow_Click">
                                    <span class="glyphicon glyphicon-remove"></span>&nbsp;Del row
                            </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        <%} %>
        <div class="panel panel-default">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                   <table class="table table-bordered">
                       <%if (this.Mode != Mode.View && this.Mode != Mode.Delete)
                         { %>
                         <thead>
                            <tr id="trHeader1" runat="server">
                                <td style="width: 20px;">
                                </td>
                                <td  style="width: 10px;">
                                    <label class="control-label"> #</label>
                                </td>
                                <td>
                                    <label class="control-label"> Value 1</label>
                                </td>
                                <td>
                                    <label class="control-label"> Value 2</label>
                                </td>
                                <td>
                                    <label class="control-label"> Value 3</label>
                                </td>
                                <td>
                                    <label class="control-label"> Value 4</label>
                                </td>
                            </tr>
                        </thead>
                       <%}
                         else
                         { %>
                          <thead>
                            <tr id="trHeader2" runat="server">
                                <td style="width: 10px;">
                                    <label class="control-label"> #</label>
                                </td>
                                <td>
                                    <label class="control-label"> Value 1</label>
                                </td>
                                <td>
                                    <label class="control-label"> Value 2</label>
                                </td>
                                <td>
                                    <label class="control-label"> Value 3</label>
                                </td>
                                <td>
                                    <label class="control-label"> Value 4</label>
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
                            <td align="center">
                               <input id="deleteFlag" class="deleteFlag" type="checkbox" runat="server" checked='<%# Eval("DelFlag")%>'
                                        data-size="mini" data-on-color="success" data-off-color="danger"/>
                            </td>
                        <%} %>
                        <td>
                            <label class="control-label"> <%# Container.ItemIndex + 1%></label>
                        </td>
                        <td>
                            <div  runat="server" id="divValue1">
                                <label class="sr-only"></label>
                                <cc1:ICodeTextBox ID="txtValue1" CssClass="value form-control input-sm" CodeType="Numeric" runat="server" MaxLength="4" Value='<%# Eval("Value1")%>'/>
                        </div>
                        </td>
                        <td>
                            <div  runat="server" id="divValue2">
                                <label class="sr-only"></label>
                                <cc1:ITextBox ID="txtValue2" CssClass="value dtExpireDate form-control input-sm" runat="server" MaxLength="255" Value='<%# Eval("Value2")%>'/>
                        </div>
                        </td>
                        <td>
                            <div  runat="server" id="divValue3">
                                <label class="sr-only"></label>
                                <cc1:ITextBox ID="txtValue3" CssClass="value dtExpireDate form-control input-sm" runat="server" MaxLength="255" Value='<%# Eval("Value3")%>'/>
                        </div>
                        </td>
                        <td>
                            <div  runat="server" id="divValue4">
                                <label class="sr-only"></label>
                                <cc1:ITextBox ID="txtValue4" CssClass="value dtExpireDate form-control input-sm" runat="server" MaxLength="255" Value='<%# Eval("Value4")%>'/>
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
         <%
        if (this.Mode != OMS.Utilities.Mode.Delete)
        {
         %>
        <div class="well well-sm">
            <div class="row">
                <div class="col-md-6">
                    <div class="btn-group btn-group-justified">
                        <%
                        if (this.Mode == OMS.Utilities.Mode.View )
                        {
                        %>
                        <%--Button Edit--%>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnEdit_Click">
                                <span class="glyphicon glyphicon-pencil"></span> 編集
                            </asp:LinkButton>
                        </div>

                        <%--Button Copy--%>
                        <div class="btn-group">
                            <asp:LinkButton ID="btnCopy" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnCopy_Click">
                                <span class="glyphicon glyphicon-paperclip"></span> コピー
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
                        <div class="btn-group">
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-default btn-sm loading" OnClick="btnBack_Click">
                                <span class="glyphicon glyphicon-chevron-left"></span> 戻る
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%} %>
</asp:Content>