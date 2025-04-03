<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLogin.aspx.cs" Inherits="OMS.Login.FrmLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Controls" Namespace="OMS.Controls" TagPrefix="oms" %>

<html lang="en"><head id="Head1" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>AdvanceSoft</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <!-- Bootstrap core CSS -->
    <link href="../Styles/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- Custom styles for this template -->
    <link href="../Styles/signin.css" rel="stylesheet" type="text/css" />
	<!--Maximimage CSS-->
    <link href="../Styles/jquery.maximage.css" rel="stylesheet" type="text/css" media="screen" charset="utf-8"/>
    
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <style>
        .input-code.upper
        {
            text-transform: uppercase;
        }
        #loading
        {
            background: url("../Images/loading.GIF") no-repeat scroll center center #ffffff;
            z-index: 10;
            filter: alpha(opacity=75);
            -moz-opacity: 0.75;
            opacity: 0.75;
        }
    </style>
</head>
<body>
	<!--Image-->
	<div id="maximage">
        <img src="../Images/maximage02.JPG"/>
	</div>
    

	<div class="container">
		
	    <%= GetMessage()%>
            
    <div id="loading" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">        
    </div> 
    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSignIn">
      <form id="frmSignIn" class="form-signin" role="form" runat="server">
      <br />
      <br />
      <br />
      <br />
        <div>	
        <%= GetLogoImage()%>
        <%--<img src="../Images/vista_logo_1.png"/>--%>
		<%--<h1 class="text-center"><strong>Order Management System</strong></h1>--%>
	    </div>
        <br />
        <%--<h3 class="text">Please sign in.</h3>--%>
        <%--<div>
            <label class="sr-only"></label>
            <oms:ICodeTextBox ID="txtCompanyCD" runat="server" Text="" placeholder="Company CD" CssClass="form-control input-lg" MaxLength='20' CodeType="AlphaNumeric"></oms:ICodeTextBox>
        </div>--%>
        <div>
            <label class="sr-only control-label"></label>
            <oms:ICodeTextBox ID="txtLoginID" UpperCode="false" runat="server" Text="" CssClass="form-control input-lg disable-ime" placeholder="Login ID"
                 CodeType="AlphaNumeric" AllowChars="-,<,>,.,/,?,:,;,',[,{,},],\,|,+,=,_,),(,*,&,^,%,$,#,@,!,`,~"/>
        </div>
        <div>
            <label class="sr-only control-label"></label>
            <oms:ITextBox ID="txtPassword" runat="server" Text="" CssClass="form-control input-lg" placeholder="Password" TextMode="password"/>
        </div>
        <div class="checkbox">
          <label>
            <input type="checkbox" id="ckbRemember" value="remember-me" runat="server" /> Remember me
          </label>
        </div>
        <asp:LinkButton class="btn btn-lg btn-info btn-block" id="btnSignIn" runat="server" onclick="btnSignIn_Click" >Sign in</asp:LinkButton>
		<br />
		<p class="text-right"><span class="glyphicon glyphicon-copyright-mark"></span> ISV VIETNAM CO.,LTD.</p>
		
      </form>
</asp:Panel>
    </div> <!-- /container -->


    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="../Scripts/jquery.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.cycle.all.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.maximage.min.js" type="text/javascript"></script>
    <script src="../Scripts/moment.min.js" type="text/javascript"></script>
    <script src="../Scripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../Scripts/oms.js" type="text/javascript"></script>
    <script src="../Scripts/common.js" type="text/javascript"></script>
	<script type="text/javascript" charset="utf-8">
	    $(function () {
	        jQuery('#maximage').maximage();
	    });
    </script>
</body>
</html>
