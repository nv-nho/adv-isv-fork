<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmApprovalList.aspx.cs" Inherits="OMS.Approval.FrmApprovalList" %>

<%@ Import Namespace="OMS.Utilities" %>
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PagingFooterControl.ascx" TagName="PagingFooterControl"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PagingHeaderControl.ascx" TagName="PagingHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/HeaderGridControl.ascx" TagName="HeaderGridControl"
    TagPrefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .temp 
        {
        	background-color:"#b5b5b5"
        }
        .bg-danger {
            background-color: #f2dede;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('table thead tr').find('td:nth-child(2),th:nth-child(2)').append('<input type="checkbox" id="chkSelectAll" />');

            $('#chkSelectAll').change(function () {
                if ($(this).prop("checked")) {
                    $(".select-item").prop('checked', true);
                } else {
                    $(".select-item").prop('checked', false);
                }
                SetDisableApprovalBtn();
            });

            $('.select-item').change(function () {
                SetCheckAll();
                SetDisableApprovalBtn();
            });

            if ($(getCtrlById("hidApprovedIDs")).val() != "")
            {
                $('.select-item').each(function(index, ojb){
                    if ($(getCtrlById("hidApprovedIDs")).val().includes($(ojb).val()))
                    {
                        $(ojb).prop('checked', true);
                    }
                });

                SetCheckAll();
                SetDisableApprovalBtn();
            }

            if($('.select-item').length == 0)
            {
                $("#divApprovalBtn").addClass("hide");
            }
            else
            {
                $("#divApprovalBtn").removeClass("hide");
            }

            $('#btnApproval').click(function (event) {
                event.preventDefault();
                if (GetSelectItem()) {
                    $(getCtrlById("txtApprovalNote")).val("");
                    $('#lblConfirmTitle').html("承認確認");
                    $('#lblConfirmTitle').removeClass("label-warning");
                    $('#lblConfirmTitle').addClass("label-primary");
                    $(getCtrlById("hidApprovedMode")).val("Approval");
                    $('#ModalConfirm').modal("show");
                }
            });

            $('#btnCancel').click(function (event) {
                event.preventDefault();
                if (GetSelectItem()) {
                    $(getCtrlById("txtApprovalNote")).val("");
                    $('#lblConfirmTitle').html("差戻確認");
                    $('#lblConfirmTitle').removeClass("label-primary");
                    $('#lblConfirmTitle').addClass("label-warning");
                    $(getCtrlById("hidApprovedMode")).val("Cancel");
                    $('#ModalConfirm').modal("show");
                }
            });

            $('#btnAprovalOk').click(function (event) {
                event.preventDefault();
               $(getCtrlById("btnAprrovalProcess")).click();
            });

        });

        function GetSelectItem() {
            let n = $('.select-item:checked').length;
            let arrIds = [];
            if (n != 0) {
                $('.select-item:checked').each(function () {
                    arrIds.push($(this).val());
                });

                $(getCtrlById("hidApprovedIDs")).val(arrIds.join(','));
                $("#errorMessage").html("");
                return true;
            } else {
                let arrErrors = [];
                arrErrors.push($(getCtrlById("lblMessageSelect")).text());
                $(getCtrlById("hidApprovedIDs")).val('');
                $("#errorMessage").html(GetMesssageHtml(arrErrors));
                return false;
            }
        }

        function SetCheckAll()
        {
            let n = $('.select-item:not(:checked)').length;
            if (n == 0) {
                $("#chkSelectAll").prop('checked', true);
            } else {
                $("#chkSelectAll").prop('checked', false);
            }
        }

        function SetDisableApprovalBtn()
        {
            if ($(".select-item:checked[status='3']" ).length > 0)
            {
                $('#btnApproval').attr('disabled', true);
            }
            else
            {
                $('#btnApproval').removeAttr('disabled');
            }

            if ($( ".select-item:checked[status='4']" ).length > 0)
            {
                $('#btnCancel').attr('disabled', true);
            }
            else
            {
                $('#btnCancel').removeAttr('disabled');
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="errorMessage"></div>
    <%= GetMessage()%>
    <asp:Label id="lblMessageSelect" style="display: none" runat="server"/>
    <asp:HiddenField ID="hidApprovedMode" runat="server"/>
    <asp:HiddenField ID="hidApprovedIDs" runat="server"/>
    <button type="button" id="btnAprrovalProcess" class="hide" runat="server"></button>

    <%--Condition Search--%>
    <div class="well well-sm">
        <div class="row">
             <%--From Date--%>
            <div class="col-md-2 total_Condition">
                <div class='form-group'>
                    <label class="control-label" for="<%= dtStartDate.ClientID %>">
                        勤務年月日</label>
                    <div class="input-group date">
                        <cc1:IDateTextBox ID="dtStartDate" runat="server" CssClass="form-control input-sm"
                            PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD"/>
                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>

            <%--Kara--%>
            <div class="col-md-1 total_Condition">
                <div class='form-group' style="text-align: center; padding-top: 9px;">
                    <br />
                    <label>
                        ～</label>
                </div>
            </div>

            <%--End date--%>
            <div class="col-md-2 total_Condition">
                <div class='form-group'>
                    <label class="control-label" for="<%= dtEndDate.ClientID %>">&nbsp;</label>
                    <div class="input-group date">
                        <cc1:IDateTextBox ID="dtEndDate" runat="server" CssClass="form-control input-sm"
                            PickDate="true" PickTime="false" PickFormat="YYYY/MM/DD"/>
                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>

            <%--Department--%>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="control-label" for="<%= ddlDepartment.ClientID %>">
                        部門</label>
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control input-sm"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"
                        autocomplete="off">
                    </asp:DropDownList>
                </div>
            </div>

            <%--EmployeeName--%>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="control-label" for="<%= ddlUser.ClientID %>">
                        社員名</label>
                    <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control input-sm"
                        AutoPostBack="false" autocomplete="off">
                    </asp:DropDownList>
                </div>
            </div>


            <div class="col-md-2">
                <div class="form-group">
                    <label class="control-label" for="<%= ddlType.ClientID %>">
                        状態</label>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control input-sm"
                        AutoPostBack="false" autocomplete="off">
                    </asp:DropDownList>
                </div>
            </div>

            <%--Search and Clear Button--%>
            <div class="col-md-12">
                <div class="btn-group">
                    <button type="button" id="btnSearch" class="btn btn-default btn-sm loading" runat="server">
                        <span class="glyphicon glyphicon-search"></span>&nbsp;検索
                    </button>
                    <button type="button" id="btnClear" class="btn btn-default btn-sm" runat="server">
                        <span class="glyphicon glyphicon-refresh"></span>&nbsp;クリア
                    </button>
                </div>
            </div>

        </div>
    </div>

    <%--Create New and Back Button--%>
    <div class="well well-sm">
        <div class="row">
            <div class="col-md-3">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnBack" runat="server" PostBackUrl="../Menu/FrmMainMenu.aspx" CssClass="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-5 col-sm-5">
            </div>
            <%if (base._authority.IsApproval || base._authority.IsApprovalALL)
              { %>
            <div class="col-md-4 col-sm-4" id="divApprovalBtn">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <button id="btnApproval" class="btn btn-primary btn-sm loading">
                            <span class="glyphicon glyphicon-ok"></span>&nbsp;承認
                        </button>
                    </div>
                   
                    <div class="btn-group">
                        <button id="btnCancel" class="btn btn-warning  btn-sm loading">
                            <span class="glyphicon glyphicon glyphicon-remove"></span>&nbsp;差戻</button>
                    </div>
                </div>
            </div>
            <%} %>
        </div>
    </div>

    <div class="modal fade modal-approval-confirm" id="ModalConfirm" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog">

          <!-- Modal content-->
          <div class="modal-content">
            <div class="modal-header">
              <button type="button" class="close" data-dismiss="modal">&times;</button>
              <h4 class="modal-title">
                 <span class="label label-primary" id="lblConfirmTitle">承認確認</span>
              </h4>
            </div>
            <div  class="modal-body">
                <div style="padding-left: 15px; padding-right: 15px">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-12" >
                                <div class="form-group">
                                    <label class="control-label" id="lblAprovalNote" for="<%=txtApprovalNote.ClientID %>">承認事由</label>
                                    <cc1:ITextBox ID="txtApprovalNote" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="5" MaxLength="1000"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        
            <div class="modal-footer">
                <div class = "row">
                    <div class="col-md-12 col-sm-12 col-xs-12 text-right">    
                        <button type="button" id= "btnAprovalOk" class="btn btn-primary btn-sm" data-dismiss="modal">OK</button>
                        <button type="button" id = "btnClose" class="btn btn-default btn-sm" data-dismiss="modal">キャンセル</button>    
                    </div>
                </div>
            </div>        
          </div>
        </div>
     </div>

    <uc2:PagingHeaderControl ID="PagingHeader" runat="server" />
    <%-- List Config--%>
    <uc3:HeaderGridControl ID="HeaderGrid" runat="server" />
    <asp:Repeater ID="rptList" runat="server">
        <HeaderTemplate>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("RowNumber") %>
                </td>
                <td>
                    <input type="checkbox" value='<%# Eval("ID") + ":" +  Eval("UpdateDate")%>' status='<%# Eval("ApprovalStatus")%>' class="select-item" />
                </td>
                <td>
                    <%
                        if (base._authority.IsApproval || base._authority.IsApprovalALL)
                        {
                    %>
                        <div>
                            <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("ID")+"," + Eval("UID")+","+Eval("Date")+ ",,,FrmApprovalList"%>'
                                PostBackUrl="~/Attendance/FrmAttendanceEntry.aspx" OnCommand="btnDetail_Click" CssClass="btn btn-info btn-sm loading">
                                    <span class="glyphicon glyphicon-pencil"></span>
                            </asp:LinkButton>
                        </div>
                    <%
                        }
                        else
                        {
                    %>
                        <div>
                            <asp:LinkButton ID="btnView" runat="server" CommandArgument='<%# Eval("ID")+"," + Eval("UID")+","+Eval("Date")+ ",,,FrmApprovalList_View"%>'
                                PostBackUrl="~/Attendance/FrmAttendanceEntry.aspx" OnCommand="btnView_Click" CssClass="btn btn-info btn-sm loading">
                                    <span class="glyphicon glyphicon-pencil"></span>
                            </asp:LinkButton>
                        </div>
                    <%
                        }
                    %>

                </td>
                <td width="20%">
                    <p><%# Eval("ApprovalStatusName")%></p>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("UserName", "{0}"))%>
                </td>
                <td>
                    <%# Server.HtmlEncode(Eval("Date", "{0}"))%>
                </td>
                <td>
                    <p><%# Eval("ApplyContent")%></p>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <uc1:PagingFooterControl ID="PagingFooter" runat="server" />

</asp:Content>