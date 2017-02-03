<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlLogin.ascx.cs" Inherits="Presentation.WebClient.Modules.System.ctlLogin" %>
<div style="width:100%;min-height:450px" >
    <div class="fl" style="width:48%" >&nbsp;
    </div>
    <div class="fl pgsep" style="width:20px" ></div>
    <div class="fr" style="width:48%" >
        <div style="height:50px" class="cls" ></div>
        <table style="width: 100%; border: none;">
            <tr>
                <td style="width:40%;" align="right">
                    <img src="/images/lock.gif" />
                </td>
                <td style="width:60%;" align="left">
                    <span class="bold blue ml15" >Đăng nhập hệ thống</span>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td>
                    <asp:Label ID="lblErrorDesc" runat="server" CssClass="text red" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblUserName" runat="server" CssClass="text" >Người dùng</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" Width="70%" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPass" runat="server" CssClass="text" >Mật khẩu</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPass" runat="server" CssClass="textbox" Width="70%" TextMode="Password" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td>
                    <asp:CheckBox ID="chkRemember" runat="server"/>
                    <asp:Label ID="lblRememberPass" runat="server" CssClass="bold" >Nhớ mật khẩu đăng nhập</asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:Button ID="cmdLogin" runat="server" CssClass="Button" Text="Đăng nhập" OnClick="cmdLogin_Click" />
                </td>
            </tr>
        </table>
    </div>
</div>