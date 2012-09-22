<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="txtUserControl.ascx.vb"
    Inherits="DynamicFilterControl.txtUserControl" %>
<table class="TablaFiltro" height="25" id="txtFilter" border="0" cellspacing="0">
    <tr>
        <td align="left">
            <div style="width: 180px">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </div>
        </td>
        <td align="left">
            <div style="width: 160px">
                <asp:TextBox ID="TextBox1" class="txt_gral" Width="135px" runat="server"></asp:TextBox>
            </div>
        </td>
        <td align="left">
            <div style="width: 73px">
                <asp:Label ID="Label2" runat="server"></asp:Label>
            </div>
        </td>
        <td align="left">
            <div style="width: 232px">
                <asp:TextBox ID="TextBox2" class="txt_gral" Width="135px" runat="server"></asp:TextBox>
            </div>
        </td>
        <td>
            <div style="width: 25px">
                <asp:Button ID="btnDelete" runat="server" Text="x" />
            </div>
        </td>
    </tr>
</table>
