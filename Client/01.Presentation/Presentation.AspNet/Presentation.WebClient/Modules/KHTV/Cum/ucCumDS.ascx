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
        $("#<%=txtSearch.ClientID%>").on('keyup', function () {
            table.search($(this).val()).draw();
        });

        $('#mainForm').on('submit', function (e) {
            e.preventDefault();
            var form = this;
            var btn = $(document.activeElement);
            var v_strMsg = "";

            if (btn.attr('id').indexOf('btnDelete'))
            {
                v_strMsg = "Bạn chắc chắn xoá cụm?";
            }
            
            if (confirm(v_strMsg))
            {
                form.submit();
            }
        });
    });
</script>
<asp:HiddenField ID="hdIDCUM" runat="server" />
<div id="dvDS" runat="server">
    <div class="navbar cls" style="height:33px;padding:3px 3px 0px 3px">
            <asp:Button ID="btnAdd" runat="server" CssClass="clsButton ic-add" Text="Thêm mới">
            </asp:Button>
            <asp:Button ID="btnEdit" runat="server" CssClass="clsButton ic-edit" Text="Sửa">
            </asp:Button>
            <asp:Button ID="btnDel" runat="server" CssClass="clsButton ic-delete" Text="Xoá">
            </asp:Button>
            <asp:Button ID="btnView" runat="server" CssClass="clsButton ic-view" Text="Xem">
            </asp:Button>
            <asp:Button ID="btnSearch" runat="server" CssClass="clsButton ic-search" Text="Tìm kiếm">
            </asp:Button>
            <asp:Button ID="btnExport" runat="server" CssClass="clsButton ic-exportexcel" Text="Xuất excel">
            </asp:Button>
    </div>
    <div class="ui-tabs-panel ui-corner-bottom ui-widget-content">
        <table cellspacing="2" cellpading="0" width="100%" >
            <tr>
                <td style="width:30%;border:solid 1px #ccc;vertical-align:top"  >
                    <div class="fl" style="overflow-x:auto;overflow-y:hidden">
                        <asp:TreeView ID="tvKhuVuc" runat="server" ShowLines="true" EnableClientScript="true" OnSelectedNodeChanged="tvKhuVuc_SelectedNodeChanged" >
                            <HoverNodeStyle CssClass="bold" />
                            <SelectedNodeStyle CssClass="bold" />
                        </asp:TreeView>
                    </div>
                </td>
                <td style="width:70%;vertical-align:top"  >
                    <div>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="textbox" Width="100%" ></asp:TextBox>
                    </div>
                    <table id="grdCum" class="display" cellspacing="0" width="100%" style="border:solid 1px #ccc">
                        <thead>
                            <tr class="tbDataFlowList">
                                <th>ID ấp</td>
                                <th>Mã ấp</div>
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
</div>
