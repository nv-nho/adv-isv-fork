<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FrmPageDefault.aspx.cs" Inherits="OMS.Search.FrmPageDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OMS VISTA</title>
</head>
<body>
<form runat="server">
<p>Session expired please login again.
<asp:Button ID="Button1" runat="server" Text="Close" OnClientClick="window.opener.hideLoading(); window.close();return false;" /></p>
</form>
</body>
</html>
