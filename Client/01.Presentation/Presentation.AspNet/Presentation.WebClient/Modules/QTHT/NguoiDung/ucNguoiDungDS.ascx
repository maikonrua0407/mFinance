<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNguoiDungDS.ascx.cs" Inherits="Presentation.WebClient.Modules.QTHT.NguoiDung.ucNguoiDungDS" %>
<%@ Register Assembly="Presentation.WebClient" Namespace="Presentation.WebClient.UI.Control" TagPrefix="cc1" %>
<%@ Import Namespace="Presentation.Process" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Utilities.Common" %>
<%@ Import Namespace="Presentation.WebClient.Business" %>
 <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<script language="javascript" type="text/javascript">
    var griddataid = '<%=grdNhomNSD.ClientID %>'
    var oTable;
    $(document).ready(function () {
        $('input[type=text]').addClass('bovien');
        $('select').addClass('bovien');
        $('#' + griddataid).prepend($("<thead></thead>").append($('#' + griddataid).find("tr:first")))
        $("#" + griddataid).DataTable({
            "bInfo": false,
            "bSort": false
        });
        $("#dialog").dialog({
            autoOpen: false,
            show: {
                effect: "blind",
                duration: 1000
            },
            hide: {
                effect: "explode",
                duration: 10
            }
        });
        if (getObj('<%=inpshowresult.ClientID %>').value == "1") {
            getObj('<%=inpshowresult.ClientID %>').value = "";
            $("#dialog").dialog("open");

        }
        $("#showhidesearch").click(function (evt) {
            evt.preventDefault();
            $("#tblSearch").slideToggle("fast");
        });


        var IDSANPHAM = 0
        $('#' + griddataid).on('click', 'td', function () {
            var col = $(this).parent().children().index($(this));
            var row = $(this).parent().parent().children().index($(this).parent());

            if (col > 0) {
                var scb = getObj(griddataid).getElementsByTagName('input')
                for (j = 0; j < scb.length; j++) {
                    if (j == row + 1)
                        getObj(scb[j].id).checked = true;
                    else
                        getObj(scb[j].id).checked = false;
                }
            }
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
                    IDSANPHAM = tcb[j].id.replace('cbrow', '')
                    break
                }

                if (IDSANPHAM != 0) {
                    showpopup(linkpopup + "&ID=" + IDSANPHAM + "&v=1", window.outerWidth - 120, window.outerHeight - 100)
                }
            }

        });

        $('#ctl00_MainContent_txtNamSinh')
        $("#btnAdd").on("click", function () {
            showpopup(linkpopup + "&ID=0", window.outerWidth - 120, window.outerHeight - 100)
        });
        $("#btnEdit").on("click", function () {
            var scb = getObj(griddataid).getElementsByTagName('input')
            for (j = 0; j < scb.length; j++) {
                if (getObj(scb[j].id).checked == true) {
                    IDSANPHAM = scb[j].id.replace('cbrow', '')
                    break;
                }
            }

            if (IDSANPHAM != 0) {
                showpopup(linkpopup + "&ID=" + IDSANPHAM + "&e=1", window.outerWidth - 120, window.outerHeight - 100)
            }
        });
        $("#btnView").on("click", function () {
            var scb = getObj(griddataid).getElementsByTagName('input')
            for (j = 0; j < scb.length; j++) {
                if (getObj(scb[j].id).checked == true) {
                    IDSANPHAM = scb[j].id.replace('cbrow', '')
                    break;
                }
            }
            if (IDSANPHAM != 0) {
                showpopup(linkpopup + "&ID=" + IDSANPHAM + "&v=1", window.outerWidth - 120, window.outerHeight - 100)
            }
        });
    });

    function fnaction(strconfirm, action) {
        getcheckeditemgrid()
        if (getObj('<%=chkvalgrid.ClientID %>').value != "") {
            if (confirm(strconfirm)) {

                getObj('<%=cfaction.ClientID %>').value = action
                getObj('mainForm').submit()
            }
        }
    }
    function getcheckeditemgrid() {
        var scb = getObj(griddataid).getElementsByTagName('input')
        getObj('<%=chkvalgrid.ClientID %>').value = ""
        for (j = 0; j < scb.length; j++) {
            if (getObj(scb[j].id).checked == true && scb[j].id.indexOf('cbrow') != -1) {
                getObj('<%=chkvalgrid.ClientID %>').value = getObj('<%=chkvalgrid.ClientID %>').value + scb[j].id.replace('cbrow', '') + ';'
            }
        }
        if (getObj('<%=chkvalgrid.ClientID %>').value != "") {
            getObj('<%=chkvalgrid.ClientID %>').value = getObj('<%=chkvalgrid.ClientID %>').value.substring(0, getObj('<%=chkvalgrid.ClientID %>').value.length - 1)

        }
    }

</script>



<div id="divmain" class="ui-tabs ui-corner-all ui-widget ui-widget-content ui-tabs-vertical ui-helper-clearfix" style="height:auto">
<input type="hidden" id="chkvalgrid" value="" runat="server" />
<input type="hidden" id="cfaction" value="0" runat="server" />
<input type="hidden" id="inpshowresult" value="0" runat="server" />
<div class="navbar"  style="height:36px">
   <table width="100%">
    <tr style="height:50px; vertical-align:text-top">
        <td>
            <asp:Button runat="server" onclick="btnAdd_Click" Text="Thêm" id="btnAdd" class="clsButton ic-add" />
            <input type="button" name="btnaction" value="Sửa" id="btnEdit" class="clsButton ic-edit" />
            <input type="button" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiXoa") %>','delete')" name="btnaction" value="Xóa" id="Button1" class="clsButton ic-delete" />            
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','approve')" value="Duyệt" id="btnApprove" class="clsButton ic-approve" />
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','reject')" value="Từ chối" id="btnCancel" class="clsButton ic-cancel" />
            <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','refuse')"  value="Thoái duyệt" id="btnRefuse" class="clsButton ic-refuse" />
            <input type="button" name="btnaction" value="Xem" id="btnView" class="clsButton ic-view" />
            <asp:Button runat="server" ID="btnExport" Text="Xuất Excel"     
                CssClass="clsButton ic-exportexcel" onclick="btnExport_Click"  />
            <asp:Button runat="server" ID="btnTimkiem" Text="Tìm kiếm" 
                CssClass="clsButton ic-search" onclick="btnTimkiem_Click" />
            <input type="button" name="btnaction" value="Trợ giúp" id="btnHelp" class="clsButton ic-help" />           
             
        </td>      
    </tr>
   </table>
</div>
<div class="ui-tabs-panel ui-corner-bottom ui-widget-content" style="width:100%;vertical-align:text-top">
<table class="CsTable">   
    <tr>
        <td style="width: 25%;" valign="top">   
            <asp:Panel ID="pnTree" runat="server" GroupingText="Danh sách loại danh mục" CssClass="TitlePanel">                 
                  <asp:TreeView ID="tvwTree" runat="server"  OnSelectedNodeChanged="tvwTree_SelectedNodeChanged"></asp:TreeView>
            </asp:Panel>          
        </td>
        <td valign="top">        
                   
                                 
                <div id="divtableContent">
                    <asp:TextBox runat=server ID="txtTimKiemNhanh" AutoPostBack="true"  CssClass="bovien" Width="100%"></asp:TextBox>                    
                </div> 
                <div class="scroll-pane" id="divtableContent" style="min-height:200px"> 
                        <asp:DataGrid runat="server" ID="grdNhomNSD" AutoGenerateColumns="false"
                                        BorderColor="#D9D9D9" CellPadding="4" 
                                        AlternatingItemStyle-BackColor="#F1F1F2"
                                        BackColor="White" Width="1200px" BorderWidth="1px" 
                                onitemdatabound="grdNhomNSD_ItemDataBound" CssClass="display">
                                        <ItemStyle CssClass="bordergrid" Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                                            BackColor="White"></ItemStyle>                                       
                                        <HeaderStyle ForeColor="Navy"  CssClass="tbDataFlowList" BackColor="#F1F1F2"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" Width="15px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                     <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllChecBoxWithValue(this,1);" runat="server"
                                                        ToolTip="Select/Deselect All"></asp:CheckBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    
                                                    <input type="checkbox" id="cbrow<%#DataBinder.Eval(Container.DataItem, "ID")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                     STT
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "STT")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                     Mã
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "MA")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn >
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Tên
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "TEN")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Đơn vị
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "DON_VI")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                   Mô tả
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <%--<%#DataBinder.Eval(Container.DataItem, "MO_TA")%>--%>   
                                                  <%#DataBinder.Eval(Container.DataItem, "PHAN_LOAI")%>                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>  
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                  Trạng thái
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <%#DataBinder.Eval(Container.DataItem, "TTHAI")%>                                                    
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                          
                                                                                                                                                                       
                                        </Columns>
                                    </asp:DataGrid>                      
                     
                        </div>
                  
         
        </td>
    </tr>
</table>
</div>
</div>
<div id="dialog" title="Thông báo">
  <asp:Label runat="server" ID="lblErr"></asp:Label>
</div>