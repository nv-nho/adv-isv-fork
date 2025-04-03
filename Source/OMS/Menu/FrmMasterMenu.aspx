<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmMasterMenu.aspx.cs" Inherits="OMS.Menu.FrmMasterMenu" %>


<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4">
            <!--User-->
            <div class="well well-sm" align="center">
                <p>
                    社員情報の登録、<br />
                    編集を行います。
                </p>
                <asp:LinkButton ID="btnFrmUserList" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmUserList.aspx">
                <span class="glyphicon glyphicon-user"></span>&nbsp;社員マスタ
                </asp:LinkButton>
            </div>
        </div>
        <div class="col-md-4">
            <!--Group-->
            <div class="well well-sm" align="center">
                <p>
                    部門の登録、<br />
                    編集を行います。
                </p>
                <asp:LinkButton ID="btnFrmDepartmentList" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmDepartmentList.aspx">
                    <span class="glyphicon glyphicon-credit-card"></span>&nbsp;部門マスタ
                </asp:LinkButton>
            </div>
        </div>
        <div class="col-md-4">
            <!--Group-->
            <div class="well well-sm" align="center">
                <p>
                    権限グループの登録、<br />
                    編集を行います。
                </p>
                <asp:LinkButton ID="btnFrmGroupUserList" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmGroupUserList.aspx">
                    <span class="glyphicon glyphicon-flag"></span>&nbsp;権限マスタ
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <!--/End row-->
    <div class="row">
        <div class="col-md-4">
            <div class="well well-sm" align="center">
                <p>
                    パスワードを変更を行います。<br />
                    &nbsp;
                </p>
                <asp:LinkButton ID="btnFrmChangePassword" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmChangePassword.aspx">
                    <span class="glyphicon glyphicon-transfer"></span>&nbsp;パスワード変更
                </asp:LinkButton>
            </div>
        </div>
        <div class="col-md-4">
            <div class="well well-sm" align="center">
                <p>
                    勤務体系情報の登録、<br />
                    編集を行います。
                </p>
                <asp:LinkButton ID="btnFrmWorkingSystem" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmWorkingSystemList.aspx">
                <span class="glyphicon glyphicon-briefcase"></span>&nbsp;勤務体系マスタ
                </asp:LinkButton>
            </div>
        </div>
        <div class="col-md-4">
            <div class="well well-sm" align="center">
                <p>
                    メニュー画面でのお知らせ情報を、<br />
                    登録、編集します。
                    &nbsp;
                </p>
                <asp:LinkButton ID="btnFrmInformation" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmInformationList.aspx">
                    <span class="glyphicon glyphicon-info-sign"></span>&nbsp;お知らせ情報  
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <!--/End row-->
    <div class="row">
        <div class="col-md-4">
            <div class="well well-sm" align="center">
                <p>
                    直接費・間接費の原価登録、<br />
                    編集を行います。
                </p>
                <asp:LinkButton ID="btnFrmCostList" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmCostList.aspx">
                    <span class="glyphicon glyphicon-usd"></span>&nbsp;原価マスタ
                </asp:LinkButton>
            </div>
        </div>
        <div class="col-md-4">
            <div class="well well-sm" align="center">
                <p>
                    自社情報の登録、<br />
                    編集を行います。
                </p>
                <asp:LinkButton ID="btnFrmCompanyInfo" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmCompanyInfo.aspx">
                <span class="glyphicon glyphicon-home"></span>&nbsp;会社情報
                </asp:LinkButton>
            </div>
        </div>
        <div class="col-md-4">
            <div class="well well-sm" align="center">
                <p>
                    基本情報の登録、<br />
                    編集を行います。
                </p>
                <asp:LinkButton ID="btnFrmSetting" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmSetting.aspx">
                <span class="glyphicon glyphicon-cog"></span>&nbsp;基本設定
                </asp:LinkButton>
            </div>
        </div>
    </div>
    <!--/End row-->

    <div class="row">
        <div class="col-md-4">
            <div class="well well-sm" align="center">
                <p>
                    システムコンフィグ設定<br />
                    変更しないで下さい。
                    &nbsp;
                </p>
                <asp:LinkButton ID="btnFrmConfigList" runat="server" class="btn btn-default btn-lg btn-block"
                    PostBackUrl="~/Master/FrmConfigList.aspx">
                    <span class="glyphicon glyphicon-wrench"></span>&nbsp;コンフィグ設定
                </asp:LinkButton>
            </div>
        </div>
    </div>
    <!--/End row-->

    <div class="well well-sm">
        <div class="row">
            <div class="col-md-4">
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnFrmMainMenu" runat="server" PostBackUrl="~/Menu/FrmMainMenu.aspx"
                            class="btn btn-default btn-sm loading">
                            <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;戻る
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
