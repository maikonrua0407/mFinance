<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTaiKhoanCT.ascx.cs" Inherits="Presentation.WebClient.Modules.GDKT.TaiKhoan.ucTaiKhoanCT" %>
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
         
        $('#<%=teldtNgaySoLieu.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
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

        if ($('#<%=teldtNgaySoLieu.ClientID %>').val()=='')
        {
            getObj('<%=lblErr.ClientID %>').innerText=thongbaotrong.replace('{0}','Ngày số liêu');  
            $("#dialog").dialog("open");   
            $('<%=teldtNgaySoLieu.ClientID %>').focus()
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
                        <input type="button" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiXoa")%>    ','delete')" name="btnaction" value="Xóa" id="Button1" class="clsButton ic-delete" />
                        <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet")%>    ','submit')" value="Trình duyệt" id="Button4" class="clsButton ic-approve" />
                        <%--<input type="submit" name="btnaction" runat="server" value="Lưu tạm" id="Submit5" class="clsButton ic-approve" />--%>
                        <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet")%>    ','approve')" value="Duyệt" id="btnApprove" class="clsButton ic-approve" />
                        <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet")%>    ','reject')" value="Từ chối" id="btnCancel" class="clsButton ic-cancel" />
                        <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet")%>    ','refuse')" value="Thoái duyệt" id="btnRefuse" class="clsButton ic-refuse" />
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
                            <asp:Label ID="lblTrangThai" runat="server"></asp:Label></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="grbThongTinChung" runat="server" GroupingText="Thông tin chung" CssClass="TitlePanel">
                <table class="CsTable" cellpadding="4px" cellspacing="4px">
                    <tr>
                        <td>Mã đơn vị: <font color="red">(*)</font></td>
                        <td>
                            <asp:DropDownList ID="cmbLoaiTK" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>Nguốn vốn: <font color="red">(*)</font></td>
                        <td>
                            <asp:DropDownList ID="cmbLoaiKhangNbo" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">Mã phân loại tài khoản:</td>
                        <td style="width: 30%">
                            <asp:TextBox runat="server" ReadOnly="true" ID="txtMaPLTK" Width="82%"></asp:TextBox>
                            <asp:Button ID="btnMaPLTK" runat="server" Text="F3" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblTenPLTK" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PN_THANHPHAN_CAUTRUC" Height="100%" runat="server" GroupingText="Thành phần cấu trúc" CssClass="TitlePanel">
                <asp:DataGrid runat="server" ID="grdTphanCtruc" AutoGenerateColumns="false"
                    BorderColor="#D9D9D9" CellPadding="4"
                    AlternatingItemStyle-BackColor="#F1F1F2"
                    BackColor="White" Width="100%" BorderWidth="1px">
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
                                Mã thành phần
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "ID_PHAN_HE")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                            <HeaderTemplate>
                                Tên thành phần
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "MA_DTUONG")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                            <HeaderTemplate>
                                Đối tượng
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "MA_KY_HIEU")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </asp:Panel>
            <asp:Panel ID="grbThonTinTaiKhoan" runat="server" GroupingText="Thông tin tài khoản" CssClass="TitlePanel">
                <table class="CsTable" cellpadding="4px" cellspacing="4px">
                    <tr>
                        <td style="width: 20%">Số tài khoản: <font color="red">(*)</font></td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ReadOnly="true" ID="TextBox2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Tên tài khoản:<font color="red">(*)</font></td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="TextBox3" Width="95%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Ngày số liệu: <font color="red">(*)</font></td>
                        <td>
                            <asp:TextBox runat="server" ID="teldtNgaySoLieu"></asp:TextBox>
                        </td>
                        <td>Tính chất: <font color="red">(*)</font></td>
                        <td>
                            <asp:DropDownList ID="DropDownList5" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Số dư:</td>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                        </td>
                        <td>Loại đối tượng: <font color="red">(*)</font></td>
                        <td>
                            <asp:DropDownList ID="DropDownList3" runat="server">
                            </asp:DropDownList></td>
                    </tr>
                </table>
                <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" ControlToValidate="teldtNgaySoLieu"
                    ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
            </asp:Panel>
            <asp:Panel ID="PN_DANHSACH_DTUONG" Height="100%" runat="server" GroupingText="Danh sách đối tượng tài khoản" CssClass="TitlePanel">
                <asp:DataGrid runat="server" ID="grdDsachDtuong" AutoGenerateColumns="false"
                    BorderColor="#D9D9D9" CellPadding="4"
                    AlternatingItemStyle-BackColor="#F1F1F2"
                    BackColor="White" Width="100%" BorderWidth="1px">
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
                                Mã đối tượng
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "ID_PHAN_HE")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                            <HeaderTemplate>
                                Tên đối tượng
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "MA_DTUONG")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                            <HeaderTemplate>
                                Số dư
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "MA_KY_HIEU")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
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
                            <asp:TextBox runat="server" ID="teldtNgayNhap" ReadOnly="true" Width="60%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left">Người lập:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNguoiLap" ReadOnly="true" Width="60%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left">Ngày cập nhật:</td>
                        <td>
                            <asp:TextBox runat="server" ID="teldtNgayCNhat" ReadOnly="true" Width="60%"></asp:TextBox></td>
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
