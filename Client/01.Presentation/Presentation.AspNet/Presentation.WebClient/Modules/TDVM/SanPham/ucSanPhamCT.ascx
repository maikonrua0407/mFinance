<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSanPhamCT.ascx.cs" Inherits="Presentation.WebClient.Modules.TDVM.SanPham.ucSanPhamCT" %>
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
        changeTypeKQ();
        $("#<%=radNumSoTienTuongTro.ClientID %>").keyup(function () {
            this.value = commaSplit(removeCharN(this.value, ','), '');
        });
        $("#<%=numSoTienKyQuy.ClientID %>").keyup(function () {
            this.value = commaSplit(removeCharN(this.value, ','), '');
        });
        $("#<%=numTyLeKyQuy.ClientID %>").keyup(function () {
            this.value = commaSplit(removeCharN(this.value, ','), '');
        });
        $("#<%=txtBienDo.ClientID %>").keyup(function () {

            if ($(this).val().length > 3) {
                if (isNaN(parseFloat(this.value))) return;
                this.value = parseFloat(this.value.substring(0, 4))
            }
            return this; //for chaining
        });
        var grname = '<%=grdTKhoan.ClientID %>'
        <%if (!AppConfig.LoginedUser.MaDonViGiaoDich.Equals(AppConfig.LoginedUser.MaDonVi)){%>
        $('#' + grname + ' td:nth-child(5)').hide();
        $('#' + grname + ' td:nth-child(6)').hide();
      <%} %>

         $('#<%=raddgrGocLaiVayDS.ClientID %>').on('click', 'td', function () {
            var col = $(this).parent().children().index($(this));
            var row = $(this).parent().parent().children().index($(this).parent());
            if (col != 0 && row > 0) {
                var scb = document.getElementsByName('cbrow')
                for (j = 0; j < scb.length; j++) {
                    if (j == row)
                        getObj(scb[j].id).checked = true;
                    else
                        getObj(scb[j].id).checked = false;
                }
            }            
        });
         
           $('#<%=teldtNgayHieuLuc.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
        $('#<%=teldtNgayHetHieuLuc.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
    });

    function changeTypeKQ() {
        if (getObj('<%=rbTuongdoi.ClientID %>').checked == true) {
            $('#lblSoTienKyQuy').html('Tỷ lệ:')
            $("#<%=cmbPPTinh.ClientID %>").attr("style", "display:");
            $("#<%=numTyLeKyQuy.ClientID %>").attr("style", "display:");
            $("#<%=numSoTienKyQuy.ClientID %>").attr("style", "display:none");
            $("#<%=cmbLoaiTienKyQuy.ClientID %>").attr("style", "display:none");
        }
        else {
            $('#lblSoTienKyQuy').html('Số tiền:')
            $("#<%=cmbPPTinh.ClientID %>").attr("style", "display:none");
            $("#<%=numTyLeKyQuy.ClientID %>").attr("style", "display:none");
            $("#<%=numSoTienKyQuy.ClientID %>").attr("style", "display:");
            $("#<%=cmbLoaiTienKyQuy.ClientID %>").attr("style", "display:");
        }
    }
    function changecmbLoaiHachToan(id,dvht,dv,htdl) {
    <%if (!AppConfig.LoginedUser.MaDonViGiaoDich.Equals(AppConfig.LoginedUser.MaDonVi)){%>
        var grname = '<%=grdTKhoan.ClientID %>'
        $('#' + grname + ' tr > *:nth-child(3)').toggle();
        $('#' + grname + ' tr > *:nth-child(4)').toggle();
        $('#' + grname + ' tr > *:nth-child(5)').toggle();
        $('#' + grname + ' tr > *:nth-child(6)').toggle();
         <%} %>

    }
    function changebiendo(P, dls) {        
        $('#<%=txtLaiSuat.ClientID %>').val(Math.round(parseFloat(dls) + parseFloat(P.value),4));
    }

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
          
          if ($('#<%=txtTenSanPham.ClientID %>').val()=='')
          {              
              getObj('<%=lblErr.ClientID %>').innerText=thongbaotrong.replace('{0}','Tên sản phẩm');  
              $("#dialog").dialog("open");             
              $('#<%=txtTenSanPham.ClientID %>').focus();
              return false;
          }

          else if ($('#<%=teldtNgayHieuLuc.ClientID %>').val()=='')
          {
              getObj('<%=lblErr.ClientID %>').innerText=thongbaotrong.replace('{0}','Ngày hiệu lực');  
              $("#dialog").dialog("open");   
              $('<%=teldtNgayHieuLuc.ClientID %>').focus()
              return false;
          }
            else if ($('#<%=teldtNgayHetHieuLuc.ClientID %>').val()=='')
          {
              getObj('<%=lblErr.ClientID %>').innerText=thongbaotrong.replace('{0}','Ngày hết hiệu lực');  
              $("#dialog").dialog("open");   
              $('<%=teldtNgayHetHieuLuc.ClientID %>').focus()
              return false;
          }

            return true;
      
    }
</script>
<div id="dvDetail" >
<input type="hidden" id="cfaction" value="0" runat="server" />
<input type="hidden" id="inpID" value="0" runat="server" />
<input type="hidden" id="inpshowresult" value="0" runat="server" />
 <div id="tabs" style="height:auto">
   <div class="navbar"  style="height:45px">
   <table width="100%">
   <tr style="height:50px; vertical-align:text-top">
        <td>
            <%--<input type="button" name="btnaction" disabled="disabled"  value="Thêm" id="Button1" class="clsButton  ic-add" />--%>
            <input type="button" name="btnaction"  onclick="fnaction('Sửa dữ liệu?','edit')"  value="Sửa" id="Button2" class="clsButton ic-edit" />
             <input type="button" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiXoa") %>','delete')" name="btnaction" value="Xóa" id="Button1" class="clsButton ic-delete" />
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','submit')" value="Trình duyệt" id="Button4" class="clsButton ic-approve" />
            <%--<input type="submit" name="btnaction" runat="server" value="Lưu tạm" id="Submit5" class="clsButton ic-approve" />--%>
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','approve')" value="Duyệt" id="btnApprove" class="clsButton ic-approve" />
            <input type="button" name="btnaction"  onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','reject')" value="Từ chối" id="btnCancel" class="clsButton ic-cancel" />
            <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>','refuse')"  value="Thoái duyệt" id="btnRefuse" class="clsButton ic-refuse" />
            <input type="submit" name="btnaction" runat="server" value="Trợ giúp" id="Submit8" class="clsButton ic-help" />
            <input type="button" name="btnaction" value="Đóng" id="cmdClose" class="clsButton" />            
        </td>
        <td align="right" style="width:0%"><input type="checkbox" id="cbThemnhieulan" runat=server /><label for="<%=cbThemnhieulan.ClientID %>">Thêm nhiều lần</label></td>
    </tr>
   </table>
</div>
  <ul style="height:500px">
    <li><a href="#tabs-ttc">1. Thông tin chung</a></li>
    <li><a href="#tabs-glv">2. Gốc lãi vay</a></li>
    <li><a href="#tabs-tkht">3. Tài khoản hạch toán</a></li>
    <li><a href="#tabs-ttks">4. Thông tin kiểm soát</a></li>
  </ul>
  <div id="tabs-ttc" >
     <asp:Panel ID="pntrangthai" runat="server" GroupingText="&nbsp;" CssClass="TitlePanel">
       <table  class="CsTable">
        <tr>
            <td align="left">Trạng thái:</td>
            <td id="tdTrangthai">
                <asp:Label ID="lblTrangThai" runat="server" Text="Label"></asp:Label></td>
        </tr>
       </table>
    </asp:Panel>
    <asp:Panel ID="grbThongTinChung" runat="server" GroupingText="Thông tin chung" CssClass="TitlePanel">
        <table  class="CsTable" cellpadding=4px cellspacing="4px">
            <tr>
                <td style="width:20%">Mã sản phẩm</td>
                <td style="width:30%"><asp:TextBox runat=server ReadOnly="true" ID="txtMaSanPham" ></asp:TextBox></td>
                <td style="width:20%">Hình thức cho vay: <font color="red">(*)</font></td>
                <td style="width:30%"> <asp:DropDownList ID="cmbHinhThucVay" runat="server" >
                            </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Tên sản phẩm:<font color="red">(*)</font></td>
                <td colspan="3"><asp:TextBox runat=server ID="txtTenSanPham"  Width="95%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Mục đích sử dụng:</td>
                <td> <asp:DropDownList ID="cmbMucDichVayVon" runat="server" >
                            </asp:DropDownList></td>
                <td>Loại sản phẩm: <font color="red">(*)</font></td>
                <td> <asp:DropDownList ID="cmbLoaiSanPham" runat="server" >
                            </asp:DropDownList></td>
            </tr>
             <tr>
                <td>Nhóm vòng vay:</td>
                <td><asp:DropDownList ID="cmbNhomVongVay" runat="server"   AutoPostBack="true"
                        onselectedindexchanged="cmbNhomVongVay_SelectedIndexChanged">
                            </asp:DropDownList></td>
                <td>Thời gian vay:</td>
                <td><asp:TextBox runat=server ID="telThoiGianVay" TextMode="Number"  Width="45%"></asp:TextBox>
                <asp:DropDownList ID="cmbThoiHanVay" runat="server"  Width="44%">
                            </asp:DropDownList>
                </td>
            </tr>
             <tr>
                <td>Ngày áp dụng: <font color="red">(*)</font></td>
                <td><asp:TextBox runat=server ID="teldtNgayHieuLuc"   ></asp:TextBox></td>
                <td>Ngày hết hiệu lực:</td>
                <td><asp:TextBox runat=server ID="teldtNgayHetHieuLuc"   ></asp:TextBox></td>
            </tr>
             <tr>
                <td>Phương thức tính lãi:</td>
                <td><asp:DropDownList ID="cmbPhuongThucTinh" runat="server"  >
                            </asp:DropDownList></td>
                <td>Cơ sở tính lãi: <font color="red">(*)</font></td>
                <td><asp:DropDownList ID="cmbCSTinhLai" runat="server"  >
                            </asp:DropDownList></td>
            </tr>
        </table>
       <asp:RegularExpressionValidator ID="Regularexpressionvalidator3" runat="server" ControlToValidate="teldtNgayHieuLuc"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" ControlToValidate="teldtNgayHetHieuLuc"
                        ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
    </asp:Panel>
     <asp:Panel ID="grbKyQuy"  runat="server" GroupingText="Thông tin Ký quỹ" CssClass="TitlePanel">
         <table class="CsTable"  cellpadding=4px cellspacing="4px">
            <tr>
                <td colspan="4"><input type="radio" id="rbTuyetdoi" runat="server" name="rbthongtinkq_loai"  checked="true"/><label for="<%=rbTuyetdoi.ClientID %>">Tuyệt đối</label>
                <input type="radio" id="rbTuongdoi"  runat="server" name="rbthongtinkq_loai" /><label for="<%=rbTuongdoi.ClientID %>">Tương đối</label>
                </td>
            </tr>
             <tr>
                <td style="width:20%" id="lblSoTienKyQuy">Số tiền:</td>
                <td style="width:30%">
                <asp:TextBox runat="server" ID="numTyLeKyQuy" style="text-align:right; width:29%"   ></asp:TextBox>
                <asp:TextBox runat="server" ID="numSoTienKyQuy" style="text-align:right; width:60%"    ></asp:TextBox>
                <asp:DropDownList ID="cmbPPTinh" runat="server"   Width="60%">
                            </asp:DropDownList>
                     <asp:DropDownList ID="cmbLoaiTienKyQuy" runat="server"   Width="29%">
                            </asp:DropDownList>
                </td>                
                <td style="width:20%">Hình thức nộp</td>
                <td style="width:30%"><asp:DropDownList ID="cmbHinhThucNop" runat="server"  >
                            </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Số tiền tương trợ:</td>
                <td><asp:TextBox runat=server ID="radNumSoTienTuongTro" style="text-align:right"  ></asp:TextBox></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
             <tr style="display:none">
                <td></td>
                <td><asp:TextBox runat=server ID="numSoTienGop" style="text-align:right"  ></asp:TextBox></td>
                <td><asp:TextBox runat=server ID="numSoTienGoc" style="text-align:right"  ></asp:TextBox></td>
                 <td><asp:TextBox runat=server ID="numSoTienLai" style="text-align:right"  ></asp:TextBox></td>
            </tr>
        </table>
          
    </asp:Panel>
     <asp:Panel ID="grbLaiSuat" runat="server" GroupingText="Lãi suất/Barem tính lãi tiền vay" CssClass="TitlePanel">
        <table class="CsTable" style="width:100%" cellpadding=4px cellspacing="4px">
            <tr>
                <td style="width:20%">Mã lãi suất:</td>
                <td style="width:30%"><asp:TextBox runat="server" ID="txtMaLaiSuat" ReadOnly="true" Width="82%" ></asp:TextBox>
                    <asp:Button ID="btnMaLSuat" runat="server" Text="F3" 
                        onclick="btnMaLSuat_Click" />
                </td>
                <td colspan="2"><asp:Label runat="server" ID="lblTenLSuat" Text=""></asp:Label></td>
              
            </tr>
             <tr>
                <td>Biên độ:</td>
                <td><asp:TextBox runat=server ID="txtBienDo"  style="text-align:right" TextMode=Number  Width="82%"></asp:TextBox>(%)</td>
                <td style="width:20%">Lãi suất</td>
                <td style="width:30%"><asp:TextBox runat=server ID="txtLaiSuat" ReadOnly=true  Width="82%"></asp:TextBox>(%)</td>
            </tr>
             <tr>
                <td>Loại lãi suất:</td>
                <td><asp:DropDownList ID="DropDownList1" runat="server"  >
                            </asp:DropDownList></td>
                <td>Tần suất đánh giá lại</td>
                <td><asp:TextBox runat=server ID="TextBox1"  Width="45%"></asp:TextBox>
                <asp:DropDownList ID="cmbTanSuatDanhGia" runat="server" Width="44%"  >
                            </asp:DropDownList></td>
            </tr>
        </table>
    </asp:Panel>
  </div> 
   <div id="tabs-glv" >
    <asp:Panel ID="grbGocLaiVay" Height="100%" runat="server" GroupingText="Thông tin chung" CssClass="TitlePanel">
        <table  class="CsTable"  cellpadding="8px" cellspacing="8px">
             <tr>
                <td style="width:20%">Hạn mức gốc vay:</td>
                <td style="width:30%"><asp:DropDownList ID="cmbHanMucGocVay" runat="server"  >
                            </asp:DropDownList></td>
                <td style="width:20%">Hạn mức kỳ hạn: <font color="red">(*)</font></td>
                <td style="width:30%"><asp:DropDownList ID="cmbHanMucKHan" runat="server"  >
                            </asp:DropDownList></td>
            </tr>
             <tr>
                <td>Tỷ lệ hoàn trả gốc:</td>
                <td><asp:TextBox runat="server" ID="numTyLeHoanTraGoc" ></asp:TextBox></td>
                <td colspan="2">&nbsp;</td>              
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel5" Height="100%" runat="server" GroupingText="&nbsp;" CssClass="TitlePanel">
        <div style="height:45px">
   <table width="100%">
   <tr style="height:50px; vertical-align:text-top">
        <td>
            <input type="button" name="btnaction"  id="Button3" class="clsButton ic-add-detail" />
          <%--  <input type="button" name="btnaction"  id="Button4" class="clsButton ic-edit-detail" />--%>
            <input type="button" name="btnaction" onclick="fndeletedetail()" id="Submit9" class="clsButton ic-delete-detail" />                        
        </td>      
    </tr>
   </table>
</div>
        <asp:DataGrid runat="server" ID="raddgrGocLaiVayDS" AutoGenerateColumns="false"
                                        BorderColor="#D9D9D9" CellPadding="4"  DataKeyField="ID"
                                        AlternatingItemStyle-BackColor="#F1F1F2"
                                        BackColor="White" Width="100%" 
            BorderWidth="1px" onitemdatabound="raddgrGocLaiVayDS_ItemDataBound">
                                        <ItemStyle CssClass="bordergrid"   Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                                            BackColor="White"></ItemStyle>
                                        <HeaderStyle ForeColor="#FFFFFF" CssClass="tbDataFlowList"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" Width="15px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                        <input name="cbrow" type="checkbox" onclick="SelectAllChecBoxWithName(this)" id="chkAll" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    
                                                    <input name="cbrow" type="checkbox" id="cbrow<%#DataBinder.Eval(Container.DataItem, "ID")%>" />                                                    
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    STT
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "ID")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn Visible=false>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Kỳ hạn
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "KY_HAN")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Chu kỳ
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <input type="text" class="bovien"   value="<%#DataBinder.Eval(Container.DataItem, "SO_THU_TU")%>" id="inp_chuky_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Toán tử
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <input type="text" class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "TOAN_TU")%>" readonly="readonly" id="inp_toantu_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    Gốc + Lãi
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <input type="text" class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "SO_TIEN_MOI_KY", "{0:#,##0}")%>" style="text-align:right" id="inp_SO_TIEN_MOI_KY_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                  
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    Gốc
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <input type="text" class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "SO_TIEN_GOC_MOI_KY", "{0:#,##0}")%>" style="text-align:right" readonly="readonly" id="inp_SO_TIEN_MOI_KY_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                   
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    Lãi
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <input type="text" class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "SO_TIEN_LAI_MOI_KY", "{0:#,##0}")%>" 
                                                style="text-align:right" readonly="readonly" id="inp_SO_TIEN_LAI_MOI_KY_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                  
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    Tổng gốc + lãi
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <input type="text" class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "SO_TIEN", "{0:#,##0}")%>" 
                                                style="text-align:right" readonly="readonly" id="inp_SO_TIEN_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    Tổng gốc
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <input type="text" class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "SO_TIEN_GOC", "{0:#,##0}")%>" 
                                                style="text-align:right" readonly="readonly" id="inp_SO_TIEN_GOC_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    Tổng lãi
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <input type="text" class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "SO_TIEN_LAI", "{0:#,##0}")%>" 
                                                style="text-align:right" readonly="readonly" id="inp_SO_TIEN_LAI_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    Tiết kiệm BB
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <input type="text" class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "SO_TIEN_TKBB", "{0:#,##0}")%>" 
                                                style="text-align:right" id="inp_SO_TIEN_TKBB_<%#DataBinder.Eval(Container.DataItem, "ID")%>" /> 
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                                                                     
                                        </Columns>
                                    </asp:DataGrid>
        
    </asp:Panel>
   </div>
   <div id="tabs-tkht" >
   <asp:Panel ID="PN_GOCLAIVAY_01" Height="100%" runat="server" GroupingText="&nbsp;" CssClass="TitlePanel">
    <table  class="CsTable" style="width:60%" cellpadding="8px" cellspacing="8px">
        <tr>
            <td style="margin-left:200px">
            <asp:DropDownList ID="cmbLoaiHachToan" Width="35%" runat="server"  
                     >
                            </asp:DropDownList></td>
        </tr>
   </table>
    </asp:Panel>
    <asp:Panel ID="PN_GOCLAIVAY_02" Height="100%" runat="server" GroupingText="&nbsp;" CssClass="TitlePanel">
                <asp:DataGrid runat="server" ID="grdTKhoan" AutoGenerateColumns="false"
                                        BorderColor="#D9D9D9" CellPadding="4" 
                                        AlternatingItemStyle-BackColor="#F1F1F2"
                                        BackColor="White" Width="100%" BorderWidth="1px" 
                    onitemdatabound="grdTKhoan_ItemDataBound">
                                        <ItemStyle CssClass="bordergrid" Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                                            BackColor="White"></ItemStyle>
                                        <HeaderStyle ForeColor="#FFFFFF" CssClass="tbDataFlowList"></HeaderStyle>
                                        <Columns>                                         
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
                                              <asp:TemplateColumn Visible="false">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    ID_PHAN_HE
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "ID_PHAN_HE")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn Visible="false">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    MA_DTUONG
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "MA_DTUONG")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn Visible="false">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    MA_KY_HIEU
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "MA_KY_HIEU")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                                <HeaderTemplate>
                                                    Ký hiệu hạch toán
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                 <%#DataBinder.Eval(Container.DataItem, "TEN_KY_HIEU")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>  
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"  Width="15%"></ItemStyle>
                                                <HeaderTemplate>
                                                    &nbsp;
                                                </HeaderTemplate>
                                                <ItemTemplate>                                                 
                                                 <input class="bovien" value=<%#DataBinder.Eval(Container.DataItem, "MA_PLOAI")%> id="Inp_key_doc_lap_<%#DataBinder.Eval(Container.DataItem, "MA_KY_HIEU")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>   
                                            <asp:TemplateColumn >
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Tên phân loại tài khoản
                                                </HeaderTemplate>
                                                <ItemTemplate>                                                 
                                                  <input class="bovien" style="width:99%" value="<%#DataBinder.Eval(Container.DataItem, "TEN_PLOAI")%>" id="Inp_value_doc_lap_<%#DataBinder.Eval(Container.DataItem, "MA_KY_HIEU")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>    
                                            <asp:TemplateColumn  >
                                                <HeaderStyle HorizontalAlign="Center"  Width="15%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"  Width="15%"></ItemStyle>
                                                <HeaderTemplate>
                                                    Phân loại TK báo sổ
                                                </HeaderTemplate>
                                                <ItemTemplate>                                                 
                                                 <input class="bovien" value="<%#DataBinder.Eval(Container.DataItem, "MA_PLOAI_BSO")%>" id="Inp_key_bao_so_<%#DataBinder.Eval(Container.DataItem, "MA_KY_HIEU")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>   
                                            <asp:TemplateColumn  >
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Tên phân loại TK báo sổ
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <input class="bovien" style="width:99%" value="<%#DataBinder.Eval(Container.DataItem, "TEN_PLOAI_BSO")%>" id="Inp_value_bao_so_<%#DataBinder.Eval(Container.DataItem, "MA_KY_HIEU")%>" />
                                                 
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                                                        
                                        </Columns>
                                    </asp:DataGrid>            
    </asp:Panel>
   </div>
   <div id="tabs-ttks" >
    <asp:Panel ID="Panel3" Height="100%" runat="server" GroupingText="Thông tin kiểm soát" CssClass="TitlePanel">
     
       <table  class="CsTable" style="width:60%" cellpadding="8px" cellspacing="8px">
        <tr>
            <td style="width:30%" align="left">Trạng thái bản ghi:</td>
            <td><asp:TextBox runat=server ID="txtTrangThai" ReadOnly="true"   Width="60%" ></asp:TextBox></td>
        </tr>
         <tr>
            <td align="left">Ngày lập:</td>
            <td><asp:TextBox runat=server ID="teldtNgayNhap" ReadOnly="true"   Width="60%"></asp:TextBox></td>
        </tr>
         <tr>
            <td align="left">Người lập:</td>
            <td><asp:TextBox runat=server ID="txtNguoiLap" ReadOnly="true"    Width="60%"></asp:TextBox></td>
        </tr>
         <tr>
            <td align="left">Ngày cập nhật:</td>
            <td><asp:TextBox runat=server ID="teldtNgayCNhat"  ReadOnly="true"   Width="60%"></asp:TextBox></td>
        </tr>
         <tr>
            <td align="left">Người cập nhật:</td>
            <td><asp:TextBox runat=server ID="txtNguoiCapNhat"  ReadOnly="true"   Width="60%"></asp:TextBox></td>
        </tr>
       </table>
    </asp:Panel>
   </div>
</div>
</div>
<div id="dialog" title="Thông báo">
  <asp:Label runat="server" ID="lblErr"></asp:Label>
</div>
