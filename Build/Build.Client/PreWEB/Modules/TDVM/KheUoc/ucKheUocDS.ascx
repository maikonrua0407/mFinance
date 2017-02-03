<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucKheUocDS.ascx.cs" Inherits="Presentation.WebClient.Modules.TDVM.KheUoc.ucKheUocDS" %>
<%@ Register Assembly="Presentation.WebClient" Namespace="Presentation.WebClient.UI.Control" TagPrefix="cc1" %>
<%@ Import Namespace="Presentation.Process" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Utilities.Common" %>
<%@ Import Namespace="Presentation.WebClient.Business" %>

 <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<script language="javascript" type="text/javascript">
    var griddataid = '<%=grdDangKyKheUocDS.ClientID %>'
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

        $('#<%=txtNgayNhanNoTu.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
        $('#<%=txtNgayNhanNoDen.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
        $('#<%=txtNgayDaoHanTu.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
        $('#<%=txtNgayDaoHanDen.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
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
                           <asp:TreeView ID="tvwKhuVuc" OnTreeNodeCheckChanged="tvwKhuVuc_TreeNodeCheckChanged"   EnableClientScript="true" ShowLines=true   runat="server" ShowCheckBoxes="All"></asp:TreeView>
                       </td>
                   </tr>
                  
               </table>
        </td>
        <td valign="top">
            <asp:Panel ID="pnSearch" runat="server" GroupingText="<a href='#' id='showhidesearch' > Tìm kiếm nâng cao</a>" CssClass="TitlePanel">
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
                        <td >Số đơn vay vốn:</td>
                        <td style="width: 30%">
                            <asp:TextBox runat=server ID="txtSoDonVayVon" CssClass="bovien"></asp:TextBox>
                        </td>
                        <td style="width: 20%">Số khế ước:</td>
                        <td style="width: 30%">
                          <asp:TextBox ID="txtSoKheUoc" runat="server" CssClass="bovien"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >Sản phẩm:</td>
                        <td ><asp:DropDownList ID="cmbSanPham" runat="server" CssClass="bovien"></asp:DropDownList></td>
                        <td >Nhóm nợ:</td>
                        <td ><asp:DropDownList ID="cmbNhomNo" runat="server" CssClass="bovien" ></asp:DropDownList></td>
                    </tr>                    
                    <tr>
                        <td >Ngày nhận nợ:</td>
                        <td ><asp:TextBox runat=server ID="txtNgayNhanNoTu" CssClass="bovien" Width="40%"></asp:TextBox>
                    
                        đến <asp:TextBox runat=server ID="txtNgayNhanNoDen" CssClass="bovien" Width="40%"></asp:TextBox>
                        </td>
                        <td >Ngày đáo hạn:</td>
                        <td ><asp:TextBox runat=server ID="txtNgayDaoHanTu" CssClass="bovien" Width="40%"></asp:TextBox>
                        đến <asp:TextBox runat=server ID="txtNgayDaoHanDen" CssClass="bovien" Width="40%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >Số tiền giải ngân:</td>
                        <td ><asp:TextBox runat=server ID="txtSoTienGNTu" CssClass="bovien" Width="40%"></asp:TextBox>
                    
                        đến <asp:TextBox runat=server ID="txtSoTienGNDen" CssClass="bovien" Width="40%"></asp:TextBox>
                        </td>
                        <td >Số dư:</td>
                        <td ><asp:TextBox runat=server ID="txtSoDuTu" CssClass="bovien" Width="40%"></asp:TextBox>
                        đến <asp:TextBox runat=server ID="txtSoDuDen" CssClass="bovien" Width="40%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >Thời hạn vay:</td>
                        <td >
                            <asp:TextBox runat="server" ID="txtThoiHanVayTu" CssClass="bovien" Width="10%"></asp:TextBox>
                            <asp:DropDownList runat=server ID="cmbThoiHanVayTu" CssClass="bovien" Width="28%"></asp:DropDownList>
                    
                        đến <asp:TextBox runat="server" ID="txtThoiHanVayDen" CssClass="bovien" Width="10%"></asp:TextBox>
                            <asp:DropDownList runat=server ID="cmbThoiHanVayDen" CssClass="bovien" Width="28%"></asp:DropDownList>
                        </td>
                        <td >Lãi xuất:</td>
                        <td ><asp:TextBox runat=server ID="txtLaiXuatTu" CssClass="bovien" Width="35%"></asp:TextBox>
                        % đến <asp:TextBox runat=server ID="txtLaiXuatDen" CssClass="bovien" Width="35%"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td >Mã khách hàng:</td>
                        <td style="width: 30%">
                            <asp:TextBox runat=server ID="txtMaKH" CssClass="bovien"></asp:TextBox>
                        </td>
                        <td style="width: 20%">Tên khách hàng:</td>
                        <td style="width: 30%">
                          <asp:TextBox ID="txtTenKH" runat="server" CssClass="bovien"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >Điện thoại:</td>
                        <td ><asp:TextBox ID="txtDienThoai" runat="server" CssClass="bovien"></asp:TextBox></td>
                        <td >Email:</td>
                        <td ><asp:TextBox ID="txtEmail" runat="server" CssClass="bovien"></asp:TextBox></td>
                    </tr> 
                    
                </table>
                <asp:RegularExpressionValidator ID="Regularexpressionvalidator3" runat="server" ControlToValidate="txtNgayNhanNoTu"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" ControlToValidate="txtNgayNhanNoDen"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="Regularexpressionvalidator2" runat="server" ControlToValidate="txtNgayDaoHanTu"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="Regularexpressionvalidator4" runat="server" ControlToValidate="txtNgayDaoHanDen"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
          </asp:Panel>
       
           <asp:Panel ID="pnDanhsach" runat="server" GroupingText="Danh sách khế ước" CssClass="TitlePanel">
                                 
                        <div class="scroll-pane" id="divtableContent">
                            <asp:TextBox runat=server ID="txtTimKiemNhanh" AutoPostBack="true"  CssClass="bovien" Width="100%"></asp:TextBox>                    
                        </div> 
                        <div class="scroll-pane" id="divtableContent" style="min-height:200px"> 
                        <asp:DataGrid runat="server" ID="grdDangKyKheUocDS" AutoGenerateColumns="false"
                                        BorderColor="#D9D9D9" CellPadding="4" 
                                        AlternatingItemStyle-BackColor="#F1F1F2" 
                                        BackColor="White" Width="1200px" BorderWidth="1px" 
                                onitemdatabound="grdDangKyKheUocDS_ItemDataBound"  CssClass="display"
                               >
                                        <ItemStyle CssClass="bordergrid" Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                                            BackColor="White"></ItemStyle>                                       
                                        <HeaderStyle ForeColor="#FFFFFF"  CssClass="tbDataFlowList"></HeaderStyle>
                                        
                                        <Columns>
                                            
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" Width="15px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                     
                                                         <input type="checkbox" id="chkAll" onclick="SelectAllChecBoxWithName(this)"  name="cbgrid"/>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    
                                                    <input type="checkbox" name="cbgrid" id="cbrow<%#DataBinder.Eval(Container.DataItem, "ID")%>" />
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
                                                     Số HĐBL
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "MA_GDICH")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn >
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Số khế ước
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "MA_KUOCVM")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Sản phẩm
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "TEN_SAN_PHAM")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Ngày nhận nợ
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "NGAY_GIAI_NGAN")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                   Ngày đáo hạn
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <%#DataBinder.Eval(Container.DataItem, "NGAY_DAO_HAN")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Khách hàng
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                   <%#DataBinder.Eval(Container.DataItem, "TEN_KHANG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Cụm
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <%#DataBinder.Eval(Container.DataItem, "TEN_CUM")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Nhóm
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "TEN_NHOM")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Số tiền giải ngân
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "SO_TIEN_GIAI_NGAN")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>  
                                             <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Số dư
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <%#DataBinder.Eval(Container.DataItem, "SO_DU")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Lãi suất
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "LAI_SUAT")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Trạng thái
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "TEN_TTHAINVU")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                                                                                                          
                                        </Columns>
                                    </asp:DataGrid>                      
                     
                        </div>
                  
                           
            </asp:Panel>
            <asp:Panel ID="pnPhanTrang" runat="server" CssClass="TitlePanel">
                <asp:DataPager ID="dtPage" PageSize="10"></asp:DataPager>
            </asp:Panel>
        </td>
    </tr>
</table>
</div>
</div>
<div id="dialog" title="Thông báo">
  <asp:Label runat="server" ID="lblErr"></asp:Label>
</div>