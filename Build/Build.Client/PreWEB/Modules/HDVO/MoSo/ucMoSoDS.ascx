<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMoSoDS.ascx.cs" Inherits="Presentation.WebClient.Modules.HDVO.MoSo.ucMoSoDS" %>
<%@ Import Namespace="Presentation.WebClient.Business" %>
<script language="javascript" type="text/javascript">
    var griddataid = '<%=grSoTienGuiDS.ClientID %>'
    $(document).ready(function () {

        $('input[type=text]').addClass('bovien');
        
        resizeElementHeight('divtreeview')
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
            resizeElementHeight('divtreeview')
        });

       
        var IDVALUE = 0
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
                    IDVALUE = tcb[j].id.replace('cbrow', '')
                    break
                }

                if (IDVALUE != 0) {
                    showpopup(linkpopup + "&ID=" + IDSANPHAM + "&t=1", window.outerWidth - 120, window.outerHeight - 100)
                }
            }
        });

        $('#<%=raddtNgayMoSoTu.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
        $('#<%=raddtNgayMoSoDen.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
        $('#<%=raddtNgayDaoHanTu.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
        $('#<%=raddtNgayDaoHanDen.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
        $('#ctl00_MainContent_txtNamSinh')
        $("#btnAdd").on("click", function () {
            showpopup(linkpopup + "&ID=0&t=0", window.outerWidth - 120, window.outerHeight - 100)
        });
        $("#btnEdit").on("click", function () {
            var scb = getObj(griddataid).getElementsByTagName('input')
            for (j = 0; j < scb.length; j++) {
                if (getObj(scb[j].id).checked == true) {
                    IDSANPHAM = scb[j].id.replace('cbrow', '')
                    break;
                }
            }

            if (IDVALUE != 0) {
                showpopup(linkpopup + "&ID=" + IDSANPHAM + "&t=2", window.outerWidth - 120, window.outerHeight - 100)
            }
        });
        $("#btnView").on("click", function () {
            var scb = getObj(griddataid).getElementsByTagName('input')
            for (j = 0; j < scb.length; j++) {
                if (getObj(scb[j].id).checked == true) {
                    IDVALUE = scb[j].id.replace('cbrow', '')
                    break;
                }
            }
            if (IDVALUE != 0) {
                showpopup(linkpopup + "&ID=" + IDSANPHAM + "&t=1", window.outerWidth - 120, window.outerHeight - 100)
            }
        });

        $("#<%=numSoDuTu.ClientID %>").keyup(function () {
            this.value = commaSplit(removeCharN(this.value, ','), '');
        });
        $("#<%=numSoDuDen.ClientID %>").keyup(function () {
            this.value = commaSplit(removeCharN(this.value, ','), '');
        });
        $("#<%=numKyHanTu.ClientID %>").keyup(function () {
            this.value = commaSplit(removeCharN(this.value, ','), '');
        });
        $("#<%=numKyHanDen.ClientID %>").keyup(function () {
            this.value = commaSplit(removeCharN(this.value, ','), '');
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
    function onchangekyhan(P) {
        var clv = $(P).val()
        $('#<%= cmbKyHanTu.ClientID%>').val(clv)
        $('#<%= cmbKyHanDen.ClientID%>').val(clv)
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
            <input type="button" name="btnaction" value="Thêm" id="btnAdd" class="clsButton ic-add" />
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
                        <div  class="scroll-div" id="divtreeview">
                                   <asp:TreeView ID="tvwLoaiVay"   EnableClientScript="true" ShowLines=true                        
                          runat="server" ShowCheckBoxes="All"></asp:TreeView>
                     </div>
                    </td>
                </tr>
            </table>          
        </td>
        <td valign="top">
            <asp:Panel ID="pnSearch" runat="server" GroupingText="<a href='#' id='showhidesearch' > Tìm kiếm nâng cao - Thông tin sổ tiền gửi</a>" CssClass="TitlePanel">
                <table class="CsTable" cellpadding="5px" id="tblSearch" cellspacing="5px">
                    <tr>
                        <td style="width: 20%">Trạng thái bản ghi</td>
                        <td colspan="3">
                        <input type="checkbox" runat="server" name="cbtrangthai"  id="cball" /><label for="<%=cball.ClientID %>">Tất cả</label>
                        <input type="checkbox" runat="server" name="cbtrangthai"  value="CDU"  id="cbChoduyet" /><label for="<%=cbChoduyet.ClientID %>">Chờ duyệt</label>
                        <input type="checkbox" runat="server" name="cbtrangthai" value="DDU"  id="cbDaduyet" /><label for="<%=cbDaduyet.ClientID %>">Đã duyệt</label>
                        <input type="checkbox" runat="server" name="cbtrangthai" value="TCD"  id="cbTuchoi" /><label for="<%=cbTuchoi.ClientID %>">Từ chối</label>
                        <input type="checkbox" runat="server" name="cbtrangthai" value="THD"  id="cbThoaiduyet" /><label for="<%=cbThoaiduyet.ClientID %>">Thoái duyệt</label>
                        </td>
                    </tr>
                   
                    <tr>
                        <td >Số sổ:</td>
                        <td ><asp:TextBox runat=server ID="txtSoSoTG"></asp:TextBox></td>
                        <td >Khách hàng:</td>
                        <td ><asp:TextBox runat=server ID="txtKhachHang"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td >Tên khách hàng:</td>
                        <td  colspan="3"><asp:TextBox runat=server ID="txtTenKhachHang" Width="96%"></asp:TextBox></td>                       
                    </tr>
                    <tr>
                        <td >Ngày mở sổ từ:</td>
                        <td ><asp:TextBox runat=server ID="raddtNgayMoSoTu" Width="40%"></asp:TextBox>
                    
                        đến <asp:TextBox runat=server ID="raddtNgayMoSoDen"  Width="40%"></asp:TextBox>
                        </td>
                        <td >Ngày đáo hạn từ:</td>
                        <td ><asp:TextBox runat=server ID="raddtNgayDaoHanTu" Width="40%"></asp:TextBox>
                        đến <asp:TextBox runat=server ID="raddtNgayDaoHanDen"  Width="40%"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td >Số dư từ:</td>
                        <td ><asp:TextBox runat=server ID="numSoDuTu"  Width="40%"></asp:TextBox>                    
                        đến <asp:TextBox runat=server ID="numSoDuDen"  Width="40%"></asp:TextBox>
                        </td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td >Kỳ hạn từ:</td>
                        <td ><asp:TextBox runat=server ID="numKyHanTu" Width="40%"></asp:TextBox>
                         <asp:DropDownList ID="cmbKyHanTu" runat="server" Width=50% >
                        </asp:DropDownList>
                        </td>
                        <td >đến:</td>
                        <td ><asp:TextBox runat=server ID="numKyHanDen" Width="40%"></asp:TextBox>
                         <asp:DropDownList ID="cmbKyHanDen" runat="server" Width=50% >
                        </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:RegularExpressionValidator ID="Regularexpressionvalidator3" runat="server" ControlToValidate="raddtNgayMoSoTu"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" ControlToValidate="raddtNgayMoSoDen"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="Regularexpressionvalidator2" runat="server" ControlToValidate="raddtNgayDaoHanTu"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="Regularexpressionvalidator4" runat="server" ControlToValidate="raddtNgayDaoHanDen"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
          </asp:Panel>
       
            <asp:Panel ID="pnDanhsach" runat="server" GroupingText="Danh sách" CssClass="TitlePanel">
                                 
                        <div class="scroll-pane" id="divtableContent" style="min-height:200px"> 
                        <asp:DataGrid runat="server" ID="grSoTienGuiDS" AutoGenerateColumns="false"
                                        BorderColor="#D9D9D9" CellPadding="4" 
                                        AlternatingItemStyle-BackColor="#F1F1F2"
                                        onitemdatabound="grSoTienGuiDS_ItemDataBound"  CssClass="display"
                                        BackColor="White" Width="1200px" BorderWidth="1px" >
                                        <ItemStyle CssClass="bordergrid" Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                                            BackColor="White"></ItemStyle>                                       
                                        <HeaderStyle ForeColor="#FFFFFF"  CssClass="tbDataFlowList"></HeaderStyle>
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
                                                     <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.SoSoTienGui")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "SO_SO_TG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn >
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.MaKH_Grid")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "MA_KHANG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left" Width="15%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.TenKH_Grid")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "TEN_KHANG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.ChungMinhThu")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "DD_GTLQ_SO")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                   <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.TenSanPham")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <%#DataBinder.Eval(Container.DataItem, "TEN_SAN_PHAM")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.SoDu")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                   <%#DataBinder.Eval(Container.DataItem, "SO_DU", "{0:#,##0}")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.KyHan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <%#DataBinder.Eval(Container.DataItem, "KY_HAN")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.NgayMo")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "NGAY_MO_SO")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.NgayHD")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "NGAY_DEN_HAN")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>   
                                             <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.LaiSuat")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "LAI_SUAT")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>  
                                             <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.TrangThai_TatToan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "TTHAI_SOTG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>  
                                             <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.TrangThai_Grid")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "TTHAI_NVU")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                                                                                                           
                                        </Columns>
                                    </asp:DataGrid>                      
                     
                        </div>
                    <div>
                    <table class="CsTable">
                        <tr>
                            <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.TongSoSo")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblSumSoSo"></asp:Label></td>  
                             <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.TongSoDu")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblSumSoDu"></asp:Label></td>       
                             <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.SoDuBinhQuan")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblSoDuBQ"></asp:Label></td>                                
                            <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.TongDuGoc")%></td>
                           <td align="left"><asp:Label runat="server" Text="0" ID="lblSumSoDuGoc"></asp:Label></td>    
                            <td align="right"><%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.TongDuLai")%></td>
                            <td align="left"><asp:Label runat="server" Text="0" ID="lblSumSoDuLai"></asp:Label></td>    
                        </tr>
                    </table>
                   
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