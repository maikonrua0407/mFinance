<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Popup.aspx.cs" Inherits="Presentation.WebClient.Popup" %>
<%@ Register src="Modules/Navigator/ctlPopup.ascx" tagname="ctlPopup" tagprefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.: NGV Group Finane Portal :. </title>
   
    <meta content="phần mềm" name="description" />
    <meta content="phần mềm" name="keywords" />
    
    <link href="CSS/Style.css" type="text/css" rel="Stylesheet" />
    <!--<link href="CSS/jquery.modal.min.css" type="text/css" rel="Stylesheet" media="screen" />-->
    <link rel="stylesheet" type="text/css" href="CSS/jquery-ui.css">
    <script src="Scripts/Common.js" type="text/javascript" ></script>
    <script src="Scripts/jquery-3.1.1.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <link href="/Config/Template/NGV/Attach/theme.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:ctlPopup ID="ctlPopup1" runat="server" />
    </form>
</body>
</html>