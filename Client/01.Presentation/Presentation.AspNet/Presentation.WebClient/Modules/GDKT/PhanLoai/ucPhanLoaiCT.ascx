<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPhanLoaiCT.ascx.cs" Inherits="Presentation.WebClient.Modules.GDKT.PhanLoai.ucPhanLoaiCT" %>
<%@ Import Namespace="Presentation.WebClient.Business" %>

<script>
    $(document).ready(function () {

        $("#tabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
        $("#tabs li").removeClass("ui-corner-top").addClass("ui-corner-left");
        $('input[type=text]').addClass('bovien');
        $('select').addClass('bovien');
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
         
        $('#<%=txtNgayHieuLuc.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
    });

    function fndeletedetail()
    {
        //        var scb = getObj('<--%=raddgrGocLaiVayDS.ClientID %>--').getElementsByTagName('input')        
        //        for (j = 0; j < scb.length; j++) {
        //            if (getObj(scb[j].id).checked == true && scb[j].id.indexOf('cbrow') != -1) {              
        //               $('#<--%=raddgrGocLaiVayDS.ClientID %-->').remove(1);
        //            }
        //        }       
    }
    function fnaction(strconfirm, action) {
        if( validdata() )
        {
            getObj('<%=lblErr.ClientID %>').value=''
               if (confirm(strconfirm)) {
                   getObj('<%=cfaction.ClientID %>').value = action
                getObj('mainForm').submit()
            }
        }
       
    }
    function validdata()
    {
        
        var thongbaotrong='<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThongBao.KhongDuocDeTrong") %>'
          
        if ($('#<%=txtTenPLTK.ClientID %>').val()=='')
        {              
            getObj('<%=lblErr.ClientID %>').innerText=thongbaotrong.replace('{0}','Tên phân loại');  
              $("#dialog").dialog("open");             
              $('#<%=txtTenPLTK.ClientID %>').focus();
              return false;
          }

          else if ($('#<%=txtNgayHieuLuc.ClientID %>').val()=='')
          {
              getObj('<%=lblErr.ClientID %>').innerText=thongbaotrong.replace('{0}','Ngày hiệu lực');  
              $("#dialog").dialog("open");   
              $('<%=txtNgayHieuLuc.ClientID %>').focus()
              return false;
          }

    return true;
      
}
</script>
<div id="dvDetail">
    <input type="hidden" id="cfaction" value="0" runat="server" />
    <input type="hidden" id="inpID" value="0" runat="server" />
    <input type="hidden" id="inpshowresult" value="0" runat="server" />
    <div id="tabs" style="height: auto">
        <div class="navbar" style="height: 45px">
            <table width="100%">
                <tr style="height: 50px; vertical-align: text-top">
                    <td>
                        <%--<input type="button" name="btnaction" disabled="disabled"  value="Thêm" id="Button1" class="clsButton  ic-add" />--%>
                        <input type="button" name="btnaction" onclick="fnaction('Sửa dữ liệu?','edit')" value="Sửa" id="Button2" class="clsButton ic-edit" />
                        <input type="button" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiXoa")%>','delete')" name="btnaction" value="Xóa" id="Button1" class="clsButton ic-delete" />
                        <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet")%>','submit')" value="Trình duyệt" id="Button4" class="clsButton ic-approve" />
                        <%--<input type="submit" name="btnaction" runat="server" value="Lưu tạm" id="Submit5" class="clsButton ic-approve" />--%>
                        <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet")%>','approve')" value="Duyệt" id="btnApprove" class="clsButton ic-approve" />
                        <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet")%>','reject')" value="Từ chối" id="btnCancel" class="clsButton ic-cancel" />
                        <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet")%>','refuse')" value="Thoái duyệt" id="btnRefuse" class="clsButton ic-refuse" />
                        <input type="submit" name="btnaction" runat="server" value="Trợ giúp" id="Submit8" class="clsButton ic-help" />
                        <input type="button" name="btnaction" value="Đóng" id="cmdClose" class="clsButton" />
                    </td>
                    <td align="right" style="width: 0%">
                        <input type="checkbox" id="cbThemnhieulan" runat="server" /><label for="<%=cbThemnhieulan.ClientID %>">Thêm nhiều lần</label></td>
                </tr>
            </table>
        </div>
        <ul style="height: 500px">
            <li><a href="#tabs-ttc">1. Thông tin chung</a></li>
            <li><a href="#tabs-ttks">2. Thông tin kiểm soát</a></li>
        </ul>
        <div id="tabs-ttc">
            <asp:Panel ID="pntrangthai" runat="server" GroupingText="&nbsp;" CssClass="TitlePanel">
                <table class="CsTable">
                    <tr>
                        <td align="left">Trạng thái:</td>
                        <td id="tdTrangthai">
                            <asp:Label ID="lblTrangThai" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="grbThongTinChung" runat="server" GroupingText="Thông tin chung" CssClass="TitlePanel">
                <table class="CsTable" cellpadding="4px" cellspacing="4px">
                    <tr>
                        <td style="width: 20%">Mã PLTK cấp trên:</td>
                        <td style="width: 30%">
                            <asp:TextBox runat="server" ReadOnly="true" ID="txtMaPLTKCapTren" Width="82%"></asp:TextBox>
                            <asp:Button ID="btnMaPLTKCapTren" runat="server" Text="F3" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblTenPLTKCapTren" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">Mã phân loại: <asp:Label ID="lblRedLoaiKHNBo" runat="server" ForeColor="Red">(*)</asp:Label></td>
                        <td style="width: 30%">
                            <asp:TextBox ID="txtMaPLTK" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 20%">Nhóm tài khoản:</td>
                        <td style="width: 30%">
                            <asp:DropDownList ID="cmbKyHieu" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Tên phân loại:<font color="red">(*)</font></td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtTenPLTK" Width="95%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Loại tài khoản: <font color="red">(*)</font></td>
                        <td>
                            <asp:DropDownList ID="cmbLoaiTK" runat="server">
                            </asp:DropDownList></td>
                        <td>Loại KH/ Nội bộ: <font color="red">(*)</font></td>
                        <td>
                            <asp:DropDownList ID="cmbLoaiKHNBo" runat="server">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Loại thu nhập/chi phí: <asp:Label ID="lblRedLoaiTNChiPhi" runat="server" ForeColor="Red">(*)</asp:Label></td>
                        <td>
                            <asp:DropDownList ID="cmbLoaiTNCP" runat="server" AutoPostBack="true">
                            </asp:DropDownList></td>
                        <td>Theo dõi công nợ:</td>
                        <td>
                            <asp:CheckBox ID="chkTheoDoiCongNo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Tính chất: <font color="red">(*)</font></td>
                        <td>
                            <asp:DropDownList ID="cmbTinhChatTK" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>Ngày áp dụng: <font color="red">(*)</font></td>
                        <td>
                            <asp:TextBox ID="txtNgayHieuLuc" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:RegularExpressionValidator ID="Regularexpressionvalidator3" runat="server" ControlToValidate="txtNgayHieuLuc"
                    ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
            </asp:Panel>
        </div>
        <div id="tabs-ttks">
            <asp:Panel ID="Panel1" Height="100%" runat="server" GroupingText="Thông tin kiểm soát" CssClass="TitlePanel">

                <table class="CsTable" style="width: 60%" cellpadding="8px" cellspacing="8px">
                    <tr>
                        <td style="width: 30%" align="left">Trạng thái bản ghi:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtTrangThai" ReadOnly="true" Width="60%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left">Ngày lập:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNgayNhap" ReadOnly="true" Width="60%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left">Người lập:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNguoiLap" ReadOnly="true" Width="60%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left">Ngày cập nhật:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNgayCNhat" ReadOnly="true" Width="60%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left">Người cập nhật:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNguoiCapNhat" ReadOnly="true" Width="60%"></asp:TextBox></td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
</div>
<div id="dialog" title="Thông báo">
    <asp:Label runat="server" ID="lblErr"></asp:Label>
</div>
