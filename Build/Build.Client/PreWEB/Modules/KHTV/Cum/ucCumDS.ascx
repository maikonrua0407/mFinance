<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCumDS.ascx.cs" Inherits="Presentation.WebClient.Modules.KHTV.Cum.ucCumDS" %>
<%if (dvDS.Visible)
    {
        %>
    <link rel="stylesheet" type="text/css" href="Scripts/DataTables/Scroller-1.4.2/css/scroller.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="Scripts/DataTables/DataTables-1.10.13/css/jquery.dataTables.min.css"/>
    <script type="text/javascript" src="Scripts/DataTables/datatables.js"></script>
<%
    } %>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<script type="text/javascript">
    $(function () {
        <%
    if (dvDS.Visible)
    {
        %>
        var table = $("#grdCum").DataTable({
            "columnDefs": [
                {
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                }
            ],
            "bInfo": false,
            "pageLength": 10,
            "dom": '<"top"i>rt<"bottom"flp><"clear">'
        });

        $('#grdCum tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $("#<%=hdIDCUM.ClientID%>").val(table.row(this).data()[0]);
            }
        });

        $("#<%=txtSearch.ClientID%>").on('keyup keypress change', function () {
            table.search($(this).val()).draw();
        });
    <%
    }
    else if (dvDetail.Visible)
    {
        %>
        var $j = jQuery.noConflict();
        $j("#<%=txtNgayThanhLap.ClientID%>").datepicker({ dateFormat: 'dd/mm/yy' });
        <%
    }
    %>
    });
</script>
<asp:Literal ID="lstSript" runat="server"></asp:Literal>
<asp:UpdatePanel ID="pnlMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>
        <asp:HiddenField ID="hdIDCUM" runat="server" />
        <div id="dvDetail" runat="server" >
            <table cellpadding="3" cellspacing="2" border="0" width="100%" >
                <tr>
                    <td colspan="3" class="CLineHeader" >
                        <asp:Label ID="lblDetailHeader" runat="server" CssClass="text bold" >Chi tiết thông tin cụm</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:25%" ></td>
                    <td style="width:75%" colspan="2" >
                        <asp:Label ID="lblError" runat="server" CssClass="text red bold" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;text-align:right">
                        <span class="text" >Mã cụm</span>
                    </td>
                    <td style="width:50%">
                        <asp:TextBox ID="txtMaCum" runat="server" Width="98%" CssClass="text" ></asp:TextBox>
                    </td>
                    <td style="width:25%"></td>
                </tr>
                <tr>
                    <td style="width:25%;text-align:right">
                        <span class="text" >Khu vực</span>
                    </td>
                    <td style="width:50%">
                        <asp:DropDownList ID="cboKVCT" runat="server" CssClass="text" Width="98%" ></asp:DropDownList>
                    </td>
                    <td style="width:25%"></td>
                </tr>
                <tr>
                    <td style="width:25%;text-align:right">
                        <span class="text" >Tên cụm</span>
                    </td>
                    <td style="width:50%">
                        <asp:TextBox ID="txtTenCum" runat="server" Width="98%" CssClass="text" ></asp:TextBox>
                    </td>
                    <td style="width:25%"></td>
                </tr>
                <tr>
                    <td style="width:25%;text-align:right">
                        <span class="text" >Tên tắt</span>
                    </td>
                    <td style="width:50%">
                        <asp:TextBox ID="txtTenTat" runat="server" Width="98%" CssClass="text" ></asp:TextBox>
                    </td>
                    <td style="width:25%"></td>
                </tr>
                <tr>
                    <td style="width:25%;text-align:right">
                        <span class="text" >Ngày thành lập</span>
                    </td>
                    <td style="width:50%">
                        <asp:TextBox ID="txtNgayThanhLap" runat="server" Width="200px" CssClass="text" ></asp:TextBox>
                    </td>
                    <td style="width:25%"></td>
                </tr>
                <tr>
                    <td style="width:25%;text-align:right">
                        <span class="text" >CBQL</span>
                    </td>
                    <td style="width:50%">
                        <asp:DropDownList ID="cboCBQL" runat="server" CssClass="text" Width="98%" ></asp:DropDownList>
                    </td>
                    <td style="width:25%"></td>
                </tr>
                <tr>
                    <td style="width:25%;text-align:right"></td>
                    <td style="width:75%" colspan="2">
                        <asp:UpdatePanel ID="pnlNS" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>
                        <table cellspacing="0" cellpading="2" border="0" width="100%" >
                            <tr>
                                <td style="width:30%">
                                    <asp:CheckBox ID="chkLaKH" runat="server" CssClass="textbox" Text="Là KH" />
                                </td>
                                <td style="width:40%">
                                    <asp:TextBox ID="txtNS" runat="server" Width="98%" CssClass="textbox" ></asp:TextBox>
                                </td>
                                <td style="width:30%">
                                    <asp:Button ID="btnNSAdd" runat="server" CssClass="buttons" Text="Thêm" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" >
                                    <table cellspacing="1" cellpadding="2" width="100%" border="0" >
                                        <tr>
                                            <td style="width:10%" class="GridHeader" >ID</td>
                                            <td style="width:15%" class="GridHeader" >Mã NS</td>
                                            <td style="width:45%" class="GridHeader" >Tên NS</td>
                                            <td style="width:20%" class="GridHeader" >Loại Hình</td>
                                            <td style="width:10%" class="GridHeader" ></td>
                                        </tr>
                                    <asp:Repeater ID="rptNS" runat="server" OnItemCommand="rptNS_ItemCommand"  >
                                        <ItemTemplate>
                                        <tr>
                                            <td><%#DataBinder.Eval(Container.DataItem,"ID_NS_HO_SO") %></td>
                                            <td><%#DataBinder.Eval(Container.DataItem,"MA_NS_HO_SO") %></td>
                                            <td><%#DataBinder.Eval(Container.DataItem,"TEN_HO_SO") %></td>
                                            <td><%#DataBinder.Eval(Container.DataItem,"LOAI_HINH_NS") %></td>
                                            <td><asp:LinkButton ID="btnNSDelete" runat="server" CssClass="icon-delete" CommandName="Delete" ></asp:LinkButton></td>
                                        </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    </table>
                                </td>
                            </tr>
                        </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;text-align:right">
                    </td>
                    <td style="width:75%" colspan="2">
                        <asp:Button ID="cmdSave" runat="server" Text="Lưu lại" CssClass="buttons" OnClick="cmdSave_Click" />
                        <asp:Button ID="cmdBack" runat="server" Text="Quay lại" CssClass="buttons" OnClick="cmdBack_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="dvDS" runat="server">
        <table cellspacing="1" cellpading="2" border="0" width="100%">
            <tr>
                <td class="CLineHeader">
                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="linkbutton" OnClick="btnAdd_Click">
                        <span class="icon-add" ></span>
                        <span class="text bold" >Thêm mới</span>
                    </asp:LinkButton>
                    |
                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="linkbutton" OnClick="btnEdit_Click">
                        <span class="icon-save" ></span>
                        <span class="text bold" >Sửa</span>
                    </asp:LinkButton>
                    |
                    <asp:LinkButton ID="btnDel" runat="server" CssClass="linkbutton">
                        <span class="icon-del" ></span>
                        <span class="text bold" >Xoá</span>
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="2" cellpadding="1" border="0" width="100%" >
                        <tr>
                            <td style="width:5%;text-align:right" >
                                <span class="text bold" >Cụm</span>
                            </td>
                            <td style="width:20%">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="textbox" Width="99%" ></asp:TextBox>
                            </td>
                            <td style="width:5%;text-align:right" >
                                <span class="text bold" >Khu vực</span>
                            </td>
                            <td style="width:60%">
                                <asp:DropDownList ID="cboKhuVuc" runat="server" CssClass="textbox" Width="98%" ></asp:DropDownList>
                            </td>
                            <td style="width:10%;text-align:center" >
                                <asp:Button ID="cmdSearch" runat="server" CssClass="buttons" Text="Tìm kiếm" OnClick="cmdSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <!--Grid-->
                    <table id="grdCum" class="display" cellspacing="0" border="0" width="100%">
                        <thead>
                            <tr>
                                <th>ID ấp</td>
                                <th>Mã ấp</td>
                                <th>Tên ấp</td>
                                <th>Ngày lập</td>
                                <th>CB quản lý</td>
                            </tr>
                        </thead>
                        <tbody>
                    <asp:Repeater ID="rptList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#DataBinder.Eval(Container.DataItem,"ID") %></td>
                                <td><%#DataBinder.Eval(Container.DataItem,"MA_CUM") %></td>
                                <td><%#DataBinder.Eval(Container.DataItem,"TEN_CUM") %></td>
                                <td><%#DataBinder.Eval(Container.DataItem,"NGAY_TLAP") %></td>
                                <td><%#DataBinder.Eval(Container.DataItem,"CBQL") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnAdd" />
        <asp:PostBackTrigger ControlID="btnEdit" />
        <asp:PostBackTrigger ControlID="cmdSearch" />
    </Triggers>
</asp:UpdatePanel>
