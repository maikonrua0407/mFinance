<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupContent.aspx.cs" Inherits="Presentation.WebClient.PopupContent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>.: NGV Group Finane Portal :. </title>    
    <link rel="shortcut icon" href="" />    
    <meta content="phần mềm" name="description" />
    <meta content="phần mềm" name="keywords" />    
    <link href="CSS/Style.css" type="text/css" rel="Stylesheet" />    
    <link rel="stylesheet" href="CSS/jquery-ui.css">
    <script src="Scripts/Common.js" type="text/javascript" ></script>
    <script src="Scripts/jquery-3.1.1.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <script>

        function disableF5(e) {            
//           if ((e.which || e.keyCode) == 116 || (e.which || e.keyCode) == 82 || (e.which || e.keyCode) == 123) e.preventDefault();
//           if (event.ctrlKey && event.shiftKey && event.keyCode == 73) {
//               e.preventDefault();
//           }
//           else if (event.ctrlKey &&  event.keyCode == 73) {
//               e.preventDefault();
//           }
//           else if (event.ctrlKey && event.shiftKey && event.keyCode == 74) {
//               e.preventDefault();
//           }
//           if (event.ctrlKey && event.keyCode == 74) {
//               e.preventDefault();
//           }
//           else if (event.ctrlKey && event.shiftKey && event.keyCode == 85) {
//               e.preventDefault();
//           }
//           if (event.ctrlKey && event.keyCode == 85) {
//               e.preventDefault();
//           }
         };
        $(function () {
            $("#cmdClose").on("click", function () {
                window.close()
            });


            if (document.layers) {
                document.captureEvents(Event.MOUSEDOWN);
                document.onmousedown = clickNS4;
            }
            else if (document.all && !document.getElementById) {
                document.onmousedown = clickIE4;
            }

        //  document.oncontextmenu = new Function("return false")

//                        if (window.name != 'PopupWindow') {
//                            document.location.href = 'Login.aspx'
//                        }
        });
        $(document).ready(function () {
        $(document).on("keydown", disableF5);
    });
       
    </script>
</head>

<body>
    <form id="mainForm" runat="server" style="position:relative">
    </form>
</body>
</html>
