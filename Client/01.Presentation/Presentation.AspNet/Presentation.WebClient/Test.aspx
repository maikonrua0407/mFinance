<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Presentation.WebClient.Test" %>
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
    <script type="text/javascript">
        function openPopUpWindow(url,targetField,w,h) {
            var w = window.open(url, '_blank', 'width=' + w + ',height=' + h + ',scrollbars=1');
            // pass the targetField to the pop up window
            w.targetField = targetField;
            w.focus();
        }

        function setSearchResult(targetField, returnValue) {
            targetField.value = returnValue;
            window.focus();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="txtData" type="text" style="width:120px" />
        <input type="button" id="cmdPop" value="Open" onclick="openPopUpWindow('Popup.aspx?m=POPUP_DS_LAISUAT&q=TDVM|0001', document.forms[0].txtData,'800','400')" />
    </div>
    </form>
</body>
</html>
