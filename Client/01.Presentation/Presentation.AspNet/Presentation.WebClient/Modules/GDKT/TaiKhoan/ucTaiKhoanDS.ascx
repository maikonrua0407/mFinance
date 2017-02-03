<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTaiKhoanDS.ascx.cs" Inherits="Presentation.WebClient.Modules.GDKT.TaiKhoan.ucTaiKhoanDS" %>
<%@ Register Assembly="Presentation.WebClient" Namespace="Presentation.WebClient.UI.Control" TagPrefix="cc1" %>
<%@ Import Namespace="Presentation.Process" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Utilities.Common" %>
<%@ Import Namespace="Presentation.WebClient.Business" %>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<script language="javascript" type="text/javascript">
    var griddataid = '<%=grdTaiKhoanDS.ClientID %>'
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


        var IDTKHOAN = 0
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
                    IDTKHOAN = tcb[j].id.replace('cbrow', '')
                    break
                }

                if (IDTKHOAN != 0) {
                    showpopup(linkpopup + "&ID=" + IDTKHOAN + "&v=1", window.outerWidth - 120, window.outerHeight - 100)
                }
            }

        });

        $('#<%=teldtSoDuNgay.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy' });
        $("#btnAdd").on("click", function () {
            showpopup(linkpopup + "&ID=0", window.outerWidth - 120, window.outerHeight - 100)
        });
        $("#btnEdit").on("click", function () {
            var scb = getObj(griddataid).getElementsByTagName('input')
            for (j = 0; j < scb.length; j++) {
                if (getObj(scb[j].id).checked == true) {
                    IDTKHOAN = scb[j].id.replace('cbrow', '')
                    break;
                }
            }

            if (IDTKHOAN != 0) {
                showpopup(linkpopup + "&ID=" + IDTKHOAN + "&e=1", window.outerWidth - 120, window.outerHeight - 100)
            }
        });
        $("#btnView").on("click", function () {
            var scb = getObj(griddataid).getElementsByTagName('input')
            for (j = 0; j < scb.length; j++) {
                if (getObj(scb[j].id).checked == true) {
                    IDTKHOAN = scb[j].id.replace('cbrow', '')
                    break;
                }
            }
            if (IDTKHOAN != 0) {
                showpopup(linkpopup + "&ID=" + IDTKHOAN + "&v=1", window.outerWidth - 120, window.outerHeight - 100)
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

<div id="divmain" class="ui-tabs ui-corner-all ui-widget ui-widget-content ui-tabs-vertical ui-helper-clearfix" style="height: auto">
    <input type="hidden" id="chkvalgrid" value="" runat="server" />
    <input type="hidden" id="cfaction" value="0" runat="server" />
    <input type="hidden" id="inpshowresult" value="0" runat="server" />
    <div class="navbar" style="height: 36px">
        <table width="100%">
            <tr style="height: 50px; vertical-align: text-top">
                <td>
                    <input type="button" name="btnaction" value="Thêm" id="btnAdd" class="clsButton ic-add" />
                    <input type="button" name="btnaction" value="Sửa" id="btnEdit" class="clsButton ic-edit" />
                    <input type="button" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiXoa") %>    ','delete')" name="btnaction" value="Xóa" id="Button1" class="clsButton ic-delete" />
                    <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>    ','approve')" value="Duyệt" id="btnApprove" class="clsButton ic-approve" />
                    <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>    ','reject')" value="Từ chối" id="btnCancel" class="clsButton ic-cancel" />
                    <input type="button" name="btnaction" onclick="fnaction('<%=LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.HoiDuyet") %>    ','refuse')" value="Thoái duyệt" id="btnRefuse" class="clsButton ic-refuse" />
                    <input type="button" name="btnaction" value="Xem" id="btnView" class="clsButton ic-view" />
                    <asp:Button runat="server" ID="btnExport" Text="Xuất Excel"
                        CssClass="clsButton ic-exportexcel" OnClick="btnExport_Click" />
                    <asp:Button runat="server" ID="btnTimkiem" Text="Tìm kiếm"
                        CssClass="clsButton ic-search" OnClick="btnTimkiem_Click" />
                    <input type="button" name="btnaction" value="Trợ giúp" id="btnHelp" class="clsButton ic-help" />

                </td>
            </tr>
        </table>
    </div>
    <div class="ui-tabs-panel ui-corner-bottom ui-widget-content" style="width: 100%; vertical-align: text-top">
        <table class="CsTable">
            <tr>
                <td style="width: 25%; height: 445px; border: 1px solid; border-color: #999999;" valign="top">
                    <table class="CsTable">
                        <tr>
                            <td>
                                <asp:DropDownList ID="cmbDonVi" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <div style="overflow: auto; width: 260px; height: 550px">
                                    <asp:TreeView ID="tvwPhanLoai" ShowLines="true" runat="server"
                                        NodeIndent="15" Width="150px" EnableClientScript="true" ShowCheckBoxes="All">
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
                                <td id="tdtrangthaicheckbox">
                                    <input type="checkbox" runat="server" onclick="javascript:SelectAllChecBoxWithgroup(this,'tdtrangthaicheckbox')" name="cbtrangthai" checked="checked" id="cball" /><label for="<%=cball.ClientID %>">Tất cả</label>
                                    <input type="checkbox" runat="server" value="CDU" checked="checked" id="cbChoduyet" /><label for="<%=cbChoduyet.ClientID %>">Chờ duyệt</label>
                                    <input type="checkbox" runat="server" value="DDU" checked="checked" id="cbDaduyet" /><label for="<%=cbDaduyet.ClientID %>">Đã duyệt</label>
                                    <input type="checkbox" runat="server" value="TCD" checked="checked" id="cbTuchoi" /><label for="<%=cbTuchoi.ClientID %>">Từ chối</label>
                                    <input type="checkbox" runat="server" value="THD" checked="checked" id="cbThoaiduyet" /><label for="<%=cbThoaiduyet.ClientID %>">Thoái duyệt</label>
                                </td>
                            </tr>
                            <tr>
                                <td>Mã tài khoản:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtMaPhanLoaiTK"></asp:TextBox></td>
                                <td>Số dư ngày:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="teldtSoDuNgay"></asp:TextBox>
                            </tr>
                        </table>
                        <asp:RegularExpressionValidator ID="Regularexpressionvalidator3" runat="server" ControlToValidate="teldtSoDuNgay"
                            ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                    </asp:Panel>

                    <asp:Panel ID="pnDanhsach" runat="server" GroupingText="Danh sách" CssClass="TitlePanel">

                        <div class="scroll-pane" id="divtableContent" style="min-height: 200px">
                            <asp:DataGrid runat="server" ID="grdTaiKhoanDS" AutoGenerateColumns="false"
                                BorderColor="#D9D9D9" CellPadding="4"
                                AlternatingItemStyle-BackColor="#F1F1F2"
                                BackColor="White" Width="1200px" BorderWidth="1px"
                                OnItemDataBound="grdTaiKhoanDS_ItemDataBound" CssClass="display">
                                <ItemStyle CssClass="bordergrid" Font-Names="Arial" Font-Size="12px" ForeColor="#000"
                                    BackColor="White"></ItemStyle>
                                <HeaderStyle ForeColor="#FFFFFF" CssClass="tbDataFlowList"></HeaderStyle>

                                <Columns>

                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center" Width="15px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <HeaderTemplate>

                                            <input type="checkbox" id="chkAll" onclick="SelectAllChecBoxWithName(this)" name="cbgrid" />

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
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.TaiKhoan.ucTaiKhoanDS.MaTaiKhoan")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "SO_TAI_KHOAN")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.TaiKhoan.ucTaiKhoanDS.TenTaiKhoan")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "TEN_TAI_KHOAN")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.TaiKhoan.ucTaiKhoanDS.SoDu")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "SODU")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.TaiKhoan.ucTaiKhoanDS.DuNoDuCo")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "MA_LSDU")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.TaiKhoan.ucTaiKhoanDS.DuNo")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "DU_NO")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.TaiKhoan.ucTaiKhoanDS.DuCo")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "DU_CO")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.TaiKhoan.ucTaiKhoanDS.NgaySoLieu")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "NGAY_DL")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <%=LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.KeToan.TaiKhoan.ucTaiKhoanDS.TrangThai")%>
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
