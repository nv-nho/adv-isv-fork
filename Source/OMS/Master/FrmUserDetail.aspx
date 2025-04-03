<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmUserDetail.aspx.cs" Inherits="OMS.Master.FrmUserDetail" %>

<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Import Namespace="OMS.Utilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
    
    var findBackCtr = "txtUserCode";
        //**********************
        // Init
        //**********************
        $(function () {
            initDetailList();
            getCtrlById("chkStatusFlag").bootstrapSwitch();
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

        //**********************
        // Set Focus
        //**********************
        function setFocus() {
            
            <%if(this.Mode == Mode.Insert || this.Mode == Mode.Copy){ %>
                getCtrlById("txtUserCode").focus().select();
            <%} %>
//            <%if(this.Mode == Mode.View){ %>
//                getCtrlById("btnEdit").focus().select();
//            <%} %>
            <%if(this.Mode == Mode.Update){ %>
                getCtrlById("txtUserCode").focus().select();
            <%} %>
        }

        //**********************
        // Call Search Group User
        //**********************
        function callSearchGroupUser() {

            findBackCtr = "txtLoginID";
            //2018.01.18 nv-nho search all
            //var groupCD = getCtrlById("txtGroupCD").val(); 
            //var groupNm = getCtrlById("txtGroupNm").val(); 
            var groupCD = ""; 
            var groupNm = ""; 
            showSearchGroupUser(groupCD, groupNm, getCtrlById("txtGroupCD").attr("id"), getCtrlById("txtGroupNm").attr("id"));
            
        };

        //**********************
        // Call Search Group User
        //**********************
        function callSearchDepartment() {
            findBackCtr = "txtGroupCD";
            //2018.01.18 nv-nho search all
            //var departmentCD = getCtrlById("txtDepartmentCD").val(); 
            //var departmentNm = getCtrlById("txtDepartmentNm").val(); 
            var departmentCD = ""; 
            var departmentNm = ""; 
            showSearchDepartment(departmentCD, departmentNm, getCtrlById("txtDepartmentCD").attr("id"), getCtrlById("txtDepartmentNm").attr("id"));
            
        };
            
        //**********************
        // Find Back
        //**********************    
        function findBack() {
            hideLoading();
            getCtrlById(findBackCtr).focus().select();
        }
        
        /*
        * Init detail list
        */
        function initDetailList() {
            var detailCtrl = $(".value");
            var invalidFlagCtrl = $(".InvalidFlag");
            <%if(this.Mode == Mode.View || this.Mode == Mode.Delete){ %>
            detailCtrl.attr("readonly", "true");
            invalidFlagCtrl.attr("disabled", "true");
            <%} else{%>
            detailCtrl.removeAttr("readonly");
            invalidFlagCtrl.removeAttr("disabled");
            <%} %>
        }        
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= GetMessage()%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-2">
                <div class='form-group <%=GetClassError("txtUserCode")%>'>
                    <label class="control-label" for="<%= txtUserCode.ClientID %>">
                        社員コード <strong class="text-danger">*</strong></label>
                    <cc1:ICodeTextBox ID="txtUserCode" runat="server" CodeType="Numeric" FillChar="0"
                        CssClass="form-control input-sm" Text=""></cc1:ICodeTextBox>
                    <%--<%=GetSpanError("txtUserCode")%>--%>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <div class='form-group <% =GetClassError("txtUserName1")%>'>
                    <label class="control-label" for="<%= txtUserName1.ClientID %>">
                        社員名 <strong class="text-danger">*</strong></label>
                    <cc1:ITextBox ID="txtUserName1" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    <%--<%=GetSpanError("txtUserName1")%>--%>
                </div>
            </div>       
        </div>

       <div class="row">
            <div class="col-md-5">
                <div class='form-group <% =GetClassError("txtPosition1")%>'>
                    <label class="control-label" for="<%= txtPosition1.ClientID %>">
                        役職 </label>
                    <cc1:ITextBox ID="txtPosition1" runat="server" CssClass="form-control input-sm"></cc1:ITextBox>
                    <%--<%=GetSpanError("txtUserName1")%>--%>
                </div>
            </div>         
        </div>

        <div class="row">
            <div class="col-md-2">
                <div class='form-group <% =GetClassError("txtDepartmentCD")%>'>
                    <label class="control-label" for="<%= txtDepartmentCD.ClientID %>">
                        部門 <strong class="text-danger">*</strong></label>
                    <div class="input-group">
                        <span class="input-group-btn">
                            <button id="btnSearchDepartment" class="btn btn-default btn-sm loading btnSearchKeypress" type="button" onclick="callSearchDepartment();return false;"
                                runat="server">
                                <span class="glyphicon glyphicon-search"></span>
                            </button>
                        </span>
                        <cc1:ICodeTextBox ID="txtDepartmentCD" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm"
                            LabelNames="departmentCD:txtDepartmentCD,departmentNm:txtDepartmentNm" AjaxUrlMethod="GetDepartmentName" Text="" SearchButtonID="btnSearchDepartment" AllowChars="-"/>
                    </div>
                    <%--<%=GetSpanError("txtGroupCD")%>--%>
                    <!-- /input-group -->
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <label class="control-label" for="<%= txtDepartmentCD.ClientID %>">
                        部門名</label>
                    <cc1:ITextBox ID="txtDepartmentNm" ReadOnly="true" runat="server" TabIndex="-1" CssClass="form-control input-sm"></cc1:ITextBox>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-2">
                <div class='form-group <% =GetClassError("txtGroupCD")%>'>
                    <label class="control-label" for="<%= txtGroupCD.ClientID %>">
                        権限グループ <strong class="text-danger">*</strong></label>
                    <div class="input-group">
                        <span class="input-group-btn">
                            <button id="btnSearchGroup" class="btn btn-default btn-sm loading btnSearchKeypress" type="button" onclick="callSearchGroupUser();return false;"
                                runat="server">
                                <span class="glyphicon glyphicon-search"></span>
                            </button>
                        </span>
                        <cc1:ICodeTextBox ID="txtGroupCD" runat="server" CodeType="AlphaNumeric" CssClass="form-control input-sm"
                            LabelNames="groupCD:txtGroupCD,groupNm:txtGroupNm" AjaxUrlMethod="GetGroupName" Text="" SearchButtonID="btnSearchGroup"/>
                    </div>
                    <%--<%=GetSpanError("txtGroupCD")%>--%>
                    <!-- /input-group -->
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <label class="control-label" for="<%= txtGroupCD.ClientID %>">
                        権限グループ名</label>
                    <cc1:ITextBox ID="txtGroupNm" ReadOnly="true" runat="server" TabIndex="-1" CssClass="form-control input-sm"></cc1:ITextBox>
                </div>
            </div>
        </div>
        <!-- /form-group -->
        <div class="row">
            <div class="col-md-5">
                <div class='form-group <% =GetClassError("txtLoginID")%>'>
                    <label class="control-label" for="<%= txtLoginID.ClientID %>">
                        ログインID <strong class="text-danger">*</strong></label>
                    <cc1:ICodeTextBox ID="txtLoginID" runat="server" UpperCode="false" CssClass="form-control input-sm" Text="" CodeType="AlphaNumeric" AllowChars="-,<,>,.,/,?,:,;,',[,{,},],\,|,+,=,_,),(,*,&,^,%,$,#,@,!,`,~"
                        AutoComplete="off"/>
                    <%--<%=GetSpanError("txtLoginID")%>--%>
                </div>
            </div>
            <div class="col-md-3">
                <div class='form-group <% =GetClassError("txtPassword")%>'>
                    <label class="control-label" for="<%= txtPassword.ClientID %>">
                        パスワード <strong class="text-danger">*</strong></label>
                    <cc1:ITextBox ID="txtPassword" runat="server" CssClass="form-control input-sm" TextMode="password"
                        AutoComplete="off"></cc1:ITextBox>
                    <%--<%=GetSpanError("txtPassword")%>--%>
                </div>
            </div>
        </div>
        <%-- Email address--%>
        <div class="row">
             <div class="col-md-3">
                <div class='form-group <% =GetClassError("txtEmailAddress")%>'>
                    <label class="control-label" for="<%= txtEmailAddress.ClientID %>">Email <strong class="text-danger">*</strong></label>
                    <cc1:ICodeTextBox ID="txtEmailAddress" runat="server" UpperCode="false" CssClass="form-control input-sm" CodeType="AlphaNumeric" AllowChars="@,.,_,-" />
                        <%--<%=GetSpanError("txtEmailAddress")%>--%>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <div class="form-group">
                    <label class="control-label input-group">
                        有効/無効</label>
                    <input id="chkStatusFlag" type="checkbox" runat="server" data-on-color="success"
                        data-off-color="danger" data-size="mini" />
                </div>
            </div>
        </div>
    </div>

    <!--Paid vacation-->
    <div class="row">
        <div class="container col-md-6">
            <div class="panel panel-default">
            <%if (this.Mode != Mode.View && this.Mode != Mode.Delete)
            { %>
                <div class="well well-sm">
                    <div class="row">
                        <div class="col-lg-4 col-md-6">
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
                <asp:Repeater ID="rptList" runat="server">
                    <HeaderTemplate>
                        <table class="table table-bordered" style="border-top: #ddd solid 1px">
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
                                        <label class="control-label"> 対象年</label>
                                    </td>
                                    <td>
                                        <label class="control-label"> 有給休暇日数</label>
                                    </td>
                                    <td  style="width: 80px;">
                                        <label class="control-label"> 無効</label>
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
                                        <label class="control-label"> 対象年</label>
                                    </td>
                                    <td>
                                        <label class="control-label"> 有給休暇日数</label>
                                    </td>
                                    <td style="width: 80px;">
                                        <label class="control-label"> 無効</label>
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
                                <div  runat="server" id="divYear">
                                    <label class="sr-only"></label>
                                    <cc1:ICodeTextBox ID="txtYear" CssClass="value form-control input-sm" CodeType="Numeric" runat="server" MaxLength="4" Value='<%# Eval("Year")%>'/>
                            </div>
                            </td>
                            <td>
                                <div  runat="server" id="divVacationDay">
                                    <label class="sr-only"></label>
                                    <cc1:INumberTextBox ID="txtVacationDay" CssClass="value form-control input-sm" CodeType="Numeric" runat="server" MaxLength="5" DecimalDigit="2" Value='<%# Eval("VacationDay")%>'/>
                            </div>

                            </td>
                            <td align="center">
                                <input id="invalidFlag" class="InvalidFlag" type="checkbox" runat="server" checked='<%# (short)Eval("InvalidFlag") == 1%>'
                                        data-size="mini" data-on-color="success" data-off-color="danger"/>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <!--End Paid Vacation-->

    <div class="row">
            <!-- Left Button Panel -->
            <div class="col-md-6">
                <div class="well well-sm">
                    <div class="btn-group btn-group-justified">
                        <%
                        if (this.Mode == OMS.Utilities.Mode.View)
                        {
                    %>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-default btn-sm loading"
                            OnClick="btnEdit_Click">
                            <span class="glyphicon glyphicon-pencil"></span>&nbsp;編集
                        </asp:LinkButton>
                    </div>
                    <div class="btn-group">
                        <asp:LinkButton ID="btnCopy" runat="server" CssClass="btn btn-default btn-sm loading"
                            OnClick="btnCopy_Click">
                            <span class="glyphicon glyphicon-paperclip"></span>&nbsp;コピー
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                        else if (this.Mode == OMS.Utilities.Mode.Insert || this.Mode == OMS.Utilities.Mode.Copy)
                        {                                
                    %>
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
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-default btn-sm loading"
                            PostBackUrl="~/Master/FrmUserList.aspx">
                                <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                    <%
                        }
                        else
                        {
                    %>
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
            <% if (this.Mode == OMS.Utilities.Mode.View) { %>
                <div class="col-md-6">
                    <div class="well well-sm">
                        <div class="row">
                            <!-- New Buton -->
                            <div class="col-md-6">
                                <div class="btn-group btn-group-justified">
                                    <asp:LinkButton ID="btnNew" runat="server" PostBackUrl="FrmUserDetail.aspx"
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
    
</asp:Content>
