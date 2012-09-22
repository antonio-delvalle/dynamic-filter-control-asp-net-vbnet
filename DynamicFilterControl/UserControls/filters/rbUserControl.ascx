<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="rbUserControl.ascx.vb"
    Inherits="DynamicFilterControl.rbUserControl" %>
<table class="TablaFiltro" height="25" id="txtFilter" border="0" cellspacing="0">
    <tr>
        <td align="left">
            <div style="width: 180px">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </div>
        </td>
        <td align="left">
            <div style="width: 469px">
                <asp:RadioButtonList ID="ucRadioButton" class="txt_gral_blanco" runat="server" AutoPostBack="False"
                    RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </div>
        </td>
        <td align="left">
            <div style="width: 25px">
                <asp:Button ID="btnDelete" runat="server" Text="x" />
            </div>
        </td>
    </tr>
</table>
