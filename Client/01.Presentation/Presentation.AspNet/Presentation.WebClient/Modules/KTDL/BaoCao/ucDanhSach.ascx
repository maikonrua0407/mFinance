<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDanhSach.ascx.cs" Inherits="Presentation.WebClient.Modules.KTDL.BaoCao.ucDanhSach" %>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<%if (dvDS.Visible)
    {
        %>
    <link rel="stylesheet" type="text/css" href="Scripts/DataTables/Scroller-1.4.2/css/scroller.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="Scripts/DataTables/DataTables-1.10.13/css/jquery.dataTables.min.css"/>
    <script type="text/javascript" src="Scripts/DataTables/datatables.js"></script>
<%
    } %>
<script type="text/javascript">
    $(function () {
        <%
    if (dvDS.Visible)
    {
        %>
        var table = $("#grdBaoCao").DataTable({
            "bInfo": false,
            "pageLength": 10,
            "dom": '<"top"i>rt<"bottom"flp><"clear">'
        });

        $('#grdBaoCao tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $("#<%=hdIDBaoCao.ClientID%>").val(table.row(this).data()[0]);
            }
        });

        table.on('select', function (e, dt, type, indexes) {
            if (type === 'row') {
                
            }
        })

        $("#<%=txtSearch.ClientID%>").on('keyup keypress change', function () {
            table.search($(this).val()).draw();
        });
    <%
    }
    else if (dvParam.Visible)
    {
        %>
        var $j = jQuery.noConflict();
        <%
    }
    %>
    });
</script>
<asp:UpdatePanel ID="pnlMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hdIDBaoCao" runat="server" />
        <div class="CLineHeader" >
            <asp:Label ID="lblReportHeader" runat="server" CssClass="text bold" >Danh sách báo cáo</asp:Label>
        </div>
        <div id="dvReport" runat="server" >
        </div>
        <div id="dvParam" runat="server" >
            <table cellpadding="3" cellspacing="2" border="0" width="100%" >
                <tr>
                    <td style="width:25%" ></td>
                    <td style="width:75%" >
                        <asp:Label ID="lblError" runat="server" CssClass="text red bold" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="ReportParam" id="rptParam" runat="server" >
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width:25%;text-align:right">
                    </td>
                    <td style="width:75%">
                        <asp:Button ID="cmdGenReport" runat="server" Text="Tạo báo cáo" CssClass="buttons" />
                        <asp:Button ID="cmdBack" runat="server" Text="Quay lại" CssClass="buttons" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="dvDS" runat="server">
        <table cellspacing="1" cellpading="2" border="0" width="100%">
            <tr>
                <td>
                    <table cellspacing="2" cellpadding="1" border="0" width="100%" >
                        <tr>
                            <td style="width:10%;text-align:right" >
                                <span class="text bold" >Điều kiện</span>
                            </td>
                            <td style="width:40%">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="textbox" Width="99%" ></asp:TextBox>
                            </td>
                            <td style="width:10%;text-align:right" >
                                <span class="text bold" >Danh mục</span>
                            </td>
                            <td style="width:40%">
                                <asp:DropDownList ID="cboDanhMucBC" runat="server" CssClass="textbox" Width="98%" ></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <!--Grid-->
                    <table id="grdBaoCao" class="display" cellspacing="0" border="0" width="100%">
                        <thead>
                            <tr>
                                <th>Mã báo cáo</td>
                                <th>Tên báo cáo</td>
                                <th>Mã danh mục</td>
                                <th>Danh mục</td>
                                <th>Mã định kỳ</td>
                                <th>Định kỳ</td>
                            </tr>
                        </thead>
                        <tbody>
                    <asp:Repeater ID="rptList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#DataBinder.Eval(Container.DataItem,"MA_BC") %></td>
                                <td><%#DataBinder.Eval(Container.DataItem,"TEN_BC") %></td>
                                <td><%#DataBinder.Eval(Container.DataItem,"MA_DM") %></td>
                                <td><%#DataBinder.Eval(Container.DataItem,"TEN_DM") %></td>
                                <td><%#DataBinder.Eval(Container.DataItem,"DINH_KY") %></td>
                                <td><%#DataBinder.Eval(Container.DataItem,"TEN_DINHKY") %></td>
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
</asp:UpdatePanel>