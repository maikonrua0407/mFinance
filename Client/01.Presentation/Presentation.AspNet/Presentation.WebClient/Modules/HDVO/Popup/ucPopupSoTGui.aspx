<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ucPopupSoTGui.aspx.cs" Inherits="Presentation.WebClient.Modules.HDVO.Popup.ucPopupSoTGui" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="Presentation.Process" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Utilities.Common" %>
<%@ Import Namespace="Presentation.WebClient.Business" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
<title>	Cổng thông tin điện tử NGV</title>
    <link rel="shortcut icon" href="../../../Image/icon.ico" />
    <meta content="Cong thong tin dien tu" name="description" />
    <meta content="phan mem" name="keywords" />
    <link href="../../../CSS/Style.css" type="text/css" rel="Stylesheet" />
    <link rel="stylesheet" href="../../../CSS/jquery-ui.css" />
    <script src="../../../Scripts/Common.js" type="text/javascript" ></script>
    <script src="../../../Scripts/jquery-3.1.1.js"></script>    
    <script src="../../../Scripts/jquery-ui.js"></script>
    <script>
        
        function disableF5(e) {
                       if ((e.which || e.keyCode) == 116 || (e.which || e.keyCode) == 82 || (e.which || e.keyCode) == 123) e.preventDefault();
                       if (event.ctrlKey && event.shiftKey && event.keyCode == 73) {
                           e.preventDefault();
                       }
                       else if (event.ctrlKey &&  event.keyCode == 73) {
                           e.preventDefault();
                       }
                       else if (event.ctrlKey && event.shiftKey && event.keyCode == 74) {
                           e.preventDefault();
                       }
                       if (event.ctrlKey && event.keyCode == 74) {
                           e.preventDefault();
                       }
                       else if (event.ctrlKey && event.shiftKey && event.keyCode == 85) {
                           e.preventDefault();
                       }
                       if (event.ctrlKey && event.keyCode == 85) {
                           e.preventDefault();
                       }
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

//             document.oncontextmenu = new Function("return false")

//            if (window.name != 'PopupWindow') {
//                document.location.href = 'Login.aspx'
//            }
        });
       
        
    </script>
</head><script>
    var griddataid = '<%=grSoTienGuiDS.ClientID %>'
    $(document).ready(function () {
        
        $(document).on("keydown", disableF5);
        $('#divtreeview').height(window.innerHeight);
        $('#' + griddataid).prepend($("<thead></thead>").append($('#' + griddataid).find("tr:first")))
        $("#" + griddataid).DataTable({
            "bInfo": false,
            "bSort": false,
            "aLengthMenu": [[10, 15, 20, 50, 100, -1], [10, 15, 20, 50, 100, "All"]],
            "pageLength": 15
        });
        var IDVALUE = 0
        $('#' + griddataid).on('click', 'td', function () {
            var col = $(this).parent().children().index($(this));
            var row = $(this).parent().parent().children().index($(this).parent());
             <%if (bool.Parse(HttpContext.Current.Request.QueryString["m"].ToString()) == true)
            { %>
            if (col > 0) {
                var scb = getObj(griddataid).getElementsByTagName('input')
                for (j = 0; j < scb.length; j++) {
                    if (j == row + 1)
                        getObj(scb[j].id).checked = true;
                    else
                        getObj(scb[j].id).checked = false;
                }
            }
            <%}else{ %>
              var scb = getObj(griddataid).getElementsByTagName('input')
                for (j = 0; j < scb.length; j++) {
                    if (j == row )
                        getObj(scb[j].id).checked = true;
                    else
                        getObj(scb[j].id).checked = false;
                }
            <%} %>
        });
        $('#' + griddataid).on('dblclick', 'tr', function () {
            if (this.rowIndex > 0) {
                var scb = getObj(griddataid).getElementsByTagName('input')
                for (j = 0; j < scb.length; j++) {
                    getObj(scb[j].id).checked = false;
                }
                var tcb = this.getElementsByTagName('input')
                for (j = 0; j < tcb.length; j++) {
                    getObj(tcb[j].id).checked = true;
                    IDVALUE = tcb[j].id.replace('cbrow', '')
                    break
                }

                 if (IDVALUE != 0) {
                    window.opener.mainForm.inpIDTarget.value = IDVALUE
                    
                    window.close();
                 }
            }
        });
    });

    function getallcheckvalue()
    {
        var strresult='';
        var scb = getObj(griddataid).getElementsByTagName('input')
        for (j = 0; j < scb.length; j++) {
            if (getObj(scb[j].id).checked == true)
             {
                strresult+=scb[j].id+'#'
             }
        }
        if (strresult.length>0)
            strresult=strresult.substring(0,strresult,length-1)
        return strresult;
    }
    function fnchon()
    {
        //--window.opener.setSearchResult(targetField,getallcheckvalue());
        window.opener.mainForm.inpIDTarget.value = IDVALUE   
        window.close();
    }

</script>
<body style="margin:0 0 0 0">
    <form id="form1" runat="server">
    <div>
    <table class="CsTable" style="border: 1px solid" rules="cols">
        <tr>
            <td valign="top" style="width:35%" rowspan="100%">
                <div class="scroll-div" id="divtreeview">
                    <asp:TreeView runat="server" ID="tvSearch" ShowLines=true ShowCheckBoxes="All"></asp:TreeView>
                </div>
            </td>
            <td valign="top" style="height:45px">
               <div class="navbar"  style="height:45px">
                   <table width="100%">
                   <tr style="height:50px; vertical-align:text-top">
                        <td>                          
                            <input type="button" name="btnaction" onclick="fnchon()"  value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.Chon") %>" id="btnchon" class="clsButton ic-add" />
                            <asp:Button runat="server" ID="btnTimkiem" Text="Tìm kiếm"  CssClass="clsButton ic-search" onclick="btnTimkiem_Click" />
                            
                            <input type="button" name="btnaction"  value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.TroGiup") %>" id="Submit8" class="clsButton ic-help" />
                            <input type="button" name="btnaction" value="<%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.Dong") %>" id="cmdClose" class="clsButton" />            
                        </td>                        
                    </tr>
                   </table>
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Panel ID="pnDanhsach" runat="server" CssClass="TitlePanel" Width="100%">
                        <div class="scroll-pane" id="divtableContent" style="min-height:200px"> 
                        <asp:DataGrid runat="server" ID="grSoTienGuiDS" AutoGenerateColumns="false"
                                        BorderColor="#D9D9D9" CellPadding="4" 
                                        AlternatingItemStyle-BackColor="#F1F1F2" 
                                        BackColor="White" Width="100%" BorderWidth="1px" 
                                CssClass="display" onitemdatabound="grSoTienGuiDS_ItemDataBound"
                               >
                                        <ItemStyle CssClass="bordergrid" Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                                            BackColor="White"></ItemStyle>                                       
                                        <HeaderStyle ForeColor="#FFFFFF"  CssClass="tbDataFlowList"></HeaderStyle>                                        
                                        <Columns>
                                            
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" Width="15px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>                                                     
                                                        <%if (bool.Parse(HttpContext.Current.Request.QueryString["m"].ToString()) == true)
                                                          { %>
                                                         <input type="checkbox" id="chkAll" onclick="SelectAllChecBoxWithName(this)"  name="cbgrid"/>
                                                         <%}
                                                           else{ %>&nbsp;
                                                           <%} %>
                                                </HeaderTemplate>
                                                <ItemTemplate>                                                    
                                                    <input type="checkbox" name="cbgrid" id="cbrow<%#DataBinder.Eval(Container.DataItem, "ID")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                     <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.STT")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "STT")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                     <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.SoSoTienGui")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "SO_SO_TG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn >
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.MaKH")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "MA_KHANG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.TenKH")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "TEN_KHANG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.NgayMoSo")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "NGAY_MO_SO")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                   <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.NgayDaoHan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <%#DataBinder.Eval(Container.DataItem, "NGAY_DEN_HAN")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.SoDu")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                   <%#DataBinder.Eval(Container.DataItem, "SO_DU", "{0:#,##0}")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.LaiSuat")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <%#DataBinder.Eval(Container.DataItem, "LAI_SUAT")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                                                                                                                                                     
                                        </Columns>
                                    </asp:DataGrid>
                        </div>
                         <div>
                    <table class="CsTable">
                        <tr>
                            <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.TongSoSo")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblSumSoSo"></asp:Label></td>  
                             <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.TongSoDu")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblSumSoDu"></asp:Label></td>       
                             <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.SoDuBinhQuan")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblSoDuBQ"></asp:Label></td>                                
                           
                        </tr>
                    </table>
                   
                    </div>
            </asp:Panel>
            </td>
        </tr>
    </table>
</div>
    </form>
</body>
</html>
