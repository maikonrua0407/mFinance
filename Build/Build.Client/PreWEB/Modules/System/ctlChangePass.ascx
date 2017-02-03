<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlChangePass.ascx.cs" Inherits="Presentation.WebClient.Modules.System.ctlChangePass" EnableViewState="true" %>
<asp:ScriptManager ID="sp1" runat="server">
</asp:ScriptManager>
<div style="height:20px" class="cls" ></div>
<asp:UpdatePanel ID="upMain" runat="server" EnableViewState="true" UpdateMode="Conditional">
    <ContentTemplate>
<table id="tblChangePass" runat="server" cellspacing="2" cellpading="1" border="0" width="100%" >
    <tr>
        <td style="width:30%;text-align:right" ></td>
        <td colspan="2" >
            <asp:Label ID="lblErrorMsg" runat="server" CssClass="red" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width:30%;text-align:right" >
            <span class="text">Mật khẩu cũ</span>
        </td>
        <td style="width:40%;" >
            <asp:TextBox ID="txtOldPass" runat="server"  TextMode="Password" Width="200px" CssClass="textbox"></asp:TextBox>
        </td>
        <td style="width:30%" ></td>
    </tr>
    <tr>
        <td style="text-align:right" >
            <span class="text">Mật khẩu mới</span>
        </td>
        <td>
            <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" Width="200px" CssClass="textbox" ></asp:TextBox>
        </td>
        <td></td>
    </tr>
    <tr>
        <td style="text-align:right" >
            <span class="text">Gõ lại mật khẩu</span>
        </td>
        <td>
            <asp:TextBox ID="txtConfirmPass" runat="server" TextMode="Password" Width="200px" CssClass="textbox" ></asp:TextBox>
        </td>
        <td></td>
    </tr>
    <tr>
        <td style="text-align:right" >
        </td>
        <td>
            <asp:Button ID="cmdSave" runat="server" CssClass="buttons" Text="Thay đổi" OnClick="cmdSave_Click" />
        </td>
        <td></td>
    </tr>
</table>
<table id="tblResult" runat="server" cellpading="1" cellspacing="1" border="0" width="100%" >
    <tr>
        <td style="text-align:center">
            <span class="text" >Đổi mật khẩu thành công! Hệ thống sẽ tự động chuyển về màn hình đăng nhập</span>
        </td>
    </tr>
    <tr>
        <td style="text-align:center">
            <span class="text" >
                Vui vòng <a href="/Login.aspx" class="link" ><b>bấm vào đây</b></a> nếu trình duyệt không tự động chuyển
            </span>
        </td>
    </tr>
    <tr>
        <td style="text-align:center">
            <div style="margin:auto" >
                <img src="/Images/progress_bar.gif" />
            </div>
        </td>
    </tr>
</table>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="cmdSave" />
    </Triggers>
</asp:UpdatePanel>
