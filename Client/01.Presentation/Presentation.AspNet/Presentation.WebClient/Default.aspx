<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Presentation.WebClient.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.: NGV Group Finane Portal :. </title>
    
    <link rel="shortcut icon" href="" />
    
    <meta content="phần mềm" name="description" />
    <meta content="phần mềm" name="keywords" />
    
    <link href="CSS/Style.css" type="text/css" rel="Stylesheet" />
    <!--<link href="CSS/jquery.modal.min.css" type="text/css" rel="Stylesheet" media="screen" />-->
    <!--<link rel="stylesheet" type="text/css" href="CSS/jquery-ui.css">-->
    <script src="Scripts/Common.js" type="text/javascript" ></script>
    <script src="Scripts/jquery-3.1.1.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <script>
        $(document).ready(function () {
            $('#divtreeview').height(window.innerHeight)
        });
    </script>
</head>
<body>
    <form id="mainForm" runat="server" style="position:relative">
    </form>
</body>
</html>