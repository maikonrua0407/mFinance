<%@ Control Language="C#" AutoEventWireup="true"  %>
<div class="CBoxHeader" >
    <div class="mt5 ml15" >
        <span id="spContainerIcon" runat="server"></span>
        <asp:Label ID="lblContainerTitle" runat="server" CssClass="bold white" ></asp:Label>
        <span class="icon-arrowup fr" onclick="javascript:togglecontrol(this,'<%=dvContent.ClientID%>');" ></span>
    </div>
</div>
<div id="dvContent" runat="server" class="CBoxContent" >
    <div id="MAIN_CONTENT" runat="server"  >
    </div>
    <div style="height:10px"></div>
</div>
<div class="cls"></div>