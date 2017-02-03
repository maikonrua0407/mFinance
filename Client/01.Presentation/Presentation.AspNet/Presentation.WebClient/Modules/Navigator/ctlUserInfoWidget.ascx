<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlUserInfoWidget.ascx.cs" Inherits="Presentation.WebClient.Modules.Navigator.ctlUserInfoWidget" %>
<div style="min-height:20px;" class="ml15 mt10">
    <div class="textrow">
        <span class="text" >Đơn vị :</span>
        <span id="spDonVi" runat="server" class="text red bold" ></span>
    </div>
    <div>
        <div class="UserImage" >
            <img class="img-polaroid" src="/Images/user_48.png" />
        </div>
        <div class="line18">
            <span class="fl text" >Họ tên : </span>
            <div class="fl text bold" id="dvFullName" runat="server" ></div>
            <br />
        </div>
        <div  class="line18">
            <span class="fl text" >Ngày : </span>
            <div class="fl text bold" id="dvNgayLamViec" runat="server" ></div>
            <br />
        </div>
        <div  class="line18">
            <span class="icon-changepass"></span>
            <asp:LinkButton ID="cmdChangePass" runat="server" CssClass="link" Text="Đổi mật khẩu" OnClick="cmdChangePass_Click" ></asp:LinkButton>
            <br />
        </div>
        <div  class="line18">
            <span class="icon-logout"></span>
            <asp:LinkButton ID="cmdLogout" runat="server" CssClass="link" Text="Đăng xuất" OnClick="cmdLogout_Click" ></asp:LinkButton>
            <br />
        </div>
    </div>
</div>