<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucKiemSoatDS.ascx.cs" Inherits="Presentation.WebClient.Modules.GDKT.KiemSoat.ucKiemSoatDS" %>
<%@ Import Namespace="Presentation.Process" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Utilities.Common" %>
<%@ Import Namespace="Presentation.WebClient.Business" %>
<link rel="stylesheet" type="text/css" href="Scripts/DataTables/Scroller-1.4.2/css/scroller.dataTables.min.css"/>
<link rel="stylesheet" type="text/css" href="Scripts/DataTables/DataTables-1.10.13/css/jquery.dataTables.min.css"/>
<script type="text/javascript" src="Scripts/DataTables/datatables.js"></script>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<script type="text/javascript">
    var griddataid = '<%=grdKiemSoatDS.ClientID %>'
    var oTable;
    $(document).ready(function () {
        $('input[type=text]').addClass('bovien');
        $('select').addClass('bovien');
        $('#' + griddataid).prepend($("<thead></thead>").append($('#' + griddataid).find("tr:first")))

        $("#" + griddataid).DataTable({
            "bInfo": false,
            "bSort": false,
            "scrollY":        "200px",
            "scrollCollapse": true,
            "paging":         false,
            "dom": '<"top"i>rt<"bottom"flp><"clear">'
        });

        if (getObj('<%=inpshowresult.ClientID %>').value == "1") {
            getObj('<%=inpshowresult.ClientID %>').value = "";
            $("#dialog").dialog("open");
        }
        $("#showhidesearch").click(function (evt) {
            evt.preventDefault();
            $("#tblSearch").slideToggle("fast");
        });

        $("#showhidegd").click(function (evt) {
            evt.preventDefault();
            $("#<%=grdKiemSoatDS.ClientID%>_wrapper").slideToggle("fast");
        });

        $("#showhidect").click(function (evt) {
            evt.preventDefault();
            $("#divCT").slideToggle("fast");
        });

        var IDGiaodich = 0
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
                    IDGiaodich = tcb[j].id.replace('cbrow', '')
                    break
                }

                if (IDGiaodich != 0) {
                    //Show detail
                    $("#<%=hdIDGD.ClientID%>").val(IDGiaodich);
                    getObj('mainForm').submit();
                }
            }

        });

        $("#btnEdit").on("click", function () {
            var scb = getObj(griddataid).getElementsByTagName('input')
            for (j = 0; j < scb.length; j++) {
                if (getObj(scb[j].id).checked == true) {
                    IDGiaodich = scb[j].id.replace('cbrow', '')
                    break;
                }
            }

            if (IDGiaodich != 0) {
                showpopup(linkpopup + "&ID=" + IDGiaodich + "&e=1", window.outerWidth - 120, window.outerHeight - 100)
            }
        });

        $("#btnView").on("click", function () {
            var scb = getObj(griddataid).getElementsByTagName('input')
            for (j = 0; j < scb.length; j++) {
                if (getObj(scb[j].id).checked == true) {
                    $("#<%=hdIDGD.ClientID%>").val(scb[j].id.replace('cbrow', ''));
                    getObj('mainForm').submit();
                    break;
                }
            }
        });

        $('#dialog').dialog({
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
<input type="hidden" id="hdIDGD" value="0" runat="server" />
<input type="hidden" id="inpshowresult" value="0" runat="server" />
<div class="navbar"  style="height:36px">
   <table width="100%">
    <tr style="height:50px; vertical-align:text-top">
        <td>
            <input type="button" name="btnaction" value="Sửa" id="btnEdit" class="clsButton ic-edit" />
            <input type="button" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiXoa") %>','delete')" name="btnaction" value="Xóa" id="btnDelete" class="clsButton ic-delete" />            
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','approve')" value="Duyệt" id="btnApprove" class="clsButton ic-approve" />
            <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiThoaiDuyet") %>','refuse')"  value="Thoái duyệt" id="btnRefuse" class="clsButton ic-refuse" />
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiTuChoiDuyet") %>','reject')" value="Từ chối" id="btnCancel" class="clsButton ic-cancel" />
            <input type="button" name="btnaction" value="Xem" id="btnView" class="clsButton ic-view" />
            <input type="button" name="btnaction" value="Xem chi tiết" id="btnViewDetail" class="clsButton ic-view" />
            <asp:Button runat="server" ID="btnTimkiem" Text="Tìm kiếm" CssClass="clsButton ic-search" />
            <asp:Button runat="server" ID="btnExport" Text="Xuất Excel" CssClass="clsButton ic-exportexcel" />
            <input type="button" name="btnaction" value="Trợ giúp" id="btnHelp" class="clsButton ic-help" />           
        </td>      
    </tr>
   </table>
</div>
<div class="ui-tabs-panel ui-corner-bottom ui-widget-content" style="width:100%;vertical-align:text-top">
<table class="CsTable">   
    <tr>
        <td style="width: 25%; height:445px; border:1px solid;border-color: #999999;" valign="top">           
            <table class="CsTable">
                <tr>
                    <td>
                        <asp:DropDownList ID="cmbDonVi" runat="server" Width=100% >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <div style="overflow:auto; width:260px;height:550px">
                            <asp:TreeView ID="tvwLoaiGD" ShowLines="true" runat="server" 
                                NodeIndent="15" Width="150px" EnableClientScript="true" >
                                <HoverNodeStyle CssClass="bold" />
                                <SelectedNodeStyle CssClass="bold" />
                            </asp:TreeView>
                         </div>
                    </td>
                </tr>
            </table>          
        </td>
        <td valign="top">
            <asp:Panel ID="pnSearch" runat="server" GroupingText="<a href='#' id='showhidesearch' > Tìm kiểm nâng cao</a>" CssClass="TitlePanel">
                <table class="CsTable" cellpadding="5px" id="tblSearch" cellspacing="5px">
                    <tr>
                        <td style="width: 20%">Trạng thái bản ghi</td>
                        <td colspan="3" id="tdtrangthaicheckbox">
                        <input type="checkbox" runat="server"  onclick="javascript:SelectAllChecBoxWithgroup(this,'tdtrangthaicheckbox')"  name="cbtrangthai" checked="checked" id="cball" /><label for="<%=cball.ClientID %>">Tất cả</label>
                        <input type="checkbox" runat="server"  value="CDU" checked="checked" id="cbChoduyet" /><label for="<%=cbChoduyet.ClientID %>">Chờ duyệt</label>
                        <input type="checkbox" runat="server"  value="DDU" checked="checked" id="cbDaduyet" /><label for="<%=cbDaduyet.ClientID %>">Đã duyệt</label>
                        <input type="checkbox" runat="server"  value="TCD" checked="checked" id="cbTuchoi" /><label for="<%=cbTuchoi.ClientID %>">Từ chối</label>
                        <input type="checkbox" runat="server"  value="THD"  checked="checked" id="cbThoaiduyet" /><label for="<%=cbThoaiduyet.ClientID %>">Thoái duyệt</label>
                        </td>
                    </tr>
                    <tr>
                        <td>Ngày giao dịch từ:</td>
                        <td style="width: 30%">
                            <asp:TextBox runat=server ID="teldtNgayGDTu" ></asp:TextBox>
                        </td>
                        <td style="width: 20%">Ngày giao dịch đến:</td>
                        <td style="width: 30%">
                          <asp:TextBox runat=server ID="teldtNgayGDDen" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Số giao dịch:</td>
                        <td style="width: 30%">
                            <asp:TextBox runat=server ID="tetxSoGiaoDich" ></asp:TextBox>
                        </td>
                        <td style="width: 20%">Số phiếu:</td>
                        <td style="width: 30%">
                          <asp:TextBox runat=server ID="tetxSoPhieu" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:RegularExpressionValidator ID="Regularexpressionvalidator3" runat="server" ControlToValidate="teldtNgayGDTu"
                ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" ControlToValidate="teldtNgayGDDen"
                ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
          </asp:Panel>
           <asp:Panel ID="pnDanhsach" runat="server" GroupingText="<a href='#' id='showhidegd' > Danh sách GD</a>" CssClass="TitlePanel">
                <asp:DataGrid runat="server" 
                    ID="grdKiemSoatDS" AutoGenerateColumns="false"
                    BorderColor="#D9D9D9" CellPadding="4" 
                    AlternatingItemStyle-BackColor="#F1F1F2" 
                    BackColor="White" Width="100%" BorderWidth="1px" 
                    onitemdatabound="grdKiemSoatDS_ItemDataBound"  
                    CssClass="display">
                    <ItemStyle CssClass="bordergrid" Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                        BackColor="White"></ItemStyle>                                       
                    <HeaderStyle ForeColor="#FFFFFF"  CssClass="tbDataFlowList"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" Width="15px" ></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                        <input type="checkbox" id="chkAll" onclick="SelectAllChecBoxWithName(this)"  name="cbgrid"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" name="cbgrid" id="cbrow<%#DataBinder.Eval(Container.DataItem, "ID")%>" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                            <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.HeaderDataColumn.STT")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "STT")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                            <HeaderTemplate>
                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.SoGiaoDich")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "SO_GDICH")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                            <asp:TemplateColumn >
                            <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                            <HeaderTemplate>
                                <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.SoTien")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "TONG_TIEN","{0:###,###,##0}")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                            <HeaderTemplate>
                                <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.TrangThai")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "TRANG_THAI")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                            <HeaderTemplate>
                                <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.NguoiGiaoDich")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "NGUOI_GD")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.NguoiDuyet")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "NGUOI_DUYET")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.NoiDung")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "DIEN_GIAI")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>                                                                                                                           
                    </Columns>
                </asp:DataGrid>                         
            </asp:Panel>
            <asp:Panel ID="pnChiTiet" runat="server" GroupingText="<a href='#' id='showhidect' >Thông tin chi tiết giao dịch</a>" CssClass="TitlePanel">
                        <div class="scroll-pane" id="divCT" style="height:200px;"> 
                        <asp:DataGrid runat="server" ID="grdChiTietGD" AutoGenerateColumns="false"
                                BorderColor="#D9D9D9" CellPadding="4" 
                                AlternatingItemStyle-BackColor="#F1F1F2" 
                                BackColor="White" Width="100%" BorderWidth="1px" CssClass="display">
                                <ItemStyle CssClass="bordergrid" Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                                    BackColor="White"></ItemStyle>                                       
                                <HeaderStyle ForeColor="#FFFFFF"  CssClass="tbDataFlowList"></HeaderStyle>
                                <Columns>
                                        <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <HeaderTemplate>
                                                <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.HeaderDataColumn.STT")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "STT")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        <HeaderTemplate>
                                                <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.LoaiChungTu")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "LOAI_CTU")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                        <asp:TemplateColumn >
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.SoTaiKhoan")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "ID_PLOAI")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.No")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "PSN")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.Co")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "PSC")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.Co")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "PSC")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.DienGiai")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "DIEN_GIAI")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.KiemSoat.ucKiemSoatGD.TrangThai")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "TRANG_THAI")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>                                                                                                                          
                                </Columns>
                            </asp:DataGrid>                      
                        </div>   
            </asp:Panel>
        </td>
    </tr>
</table>
</div>
</div>
<div id="dialog" title="Thông báo">
  <asp:Label runat="server" ID="lblErr"></asp:Label>
</div>