<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlHorizonalNavigator.ascx.cs" Inherits="Presentation.WebClient.Modules.Navigator.ctlHorizonalNavigator" %>
<div class="navbar">
    <div class="mt5 ml15" >
        <span class="icon-home"></span><a href="/">Trang chủ</a><%=NavigatorString %>
        <div class="fr" style="margin-top:-3px" >
            <span style="position:relative;">
                <asp:DropDownList ID="cboChiNhanh" runat="server" CssClass="text" Width="210px" OnSelectedIndexChanged="cboChiNhanh_SelectedIndexChanged" ></asp:DropDownList>
            </span>
            <asp:CheckBox ID="chkToogle" runat="server" />
            <span class="white mr10" >Ẩn menu panel</span>
        </div>
    </div>
</div>