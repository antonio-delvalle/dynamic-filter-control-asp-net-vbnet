<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="cldUserControl.ascx.vb"
    Inherits="DynamicFilterControl.cldUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table class="TablaFiltro" height="25" border="0" cellspacing="0">
    <tr>
        <td align="left" class="txt_gral_blanco">
            <div style="width: 180px;">
                <asp:Label ID="Label1" CssClass="txt_gral_blanco" Height="19px" runat="server">Fecha solicitud.:</asp:Label>
            </div>
        </td>
        <td align="left">
            <div style="width: 160px">
                <asp:TextBox ID="TxtFecSolIni" class="txt_gral" runat="server" Width="135px" MaxLength="10"></asp:TextBox>
                <asp:CalendarExtender ID="TxtFecSolIni_CalendarExtender" runat="server" TargetControlID="TxtFecSolIni"
                    Enabled="True" PopupButtonID="imgFec3" >
                </asp:CalendarExtender>
            </div>
        </td>
        <td align="left">
            <div style="width: 72px">
                <asp:Image ID="imgFec3" runat="server" src="/UserControls/filters/images/Calendar.gif"
                    Style="cursor: hand" />
            </div>
        </td>
        <td align="left">
            <div style="width: 160px">
                <asp:TextBox ID="TxtFecSolFin" class="txt_gral" runat="server" Width="135px" MaxLength="10"></asp:TextBox>
                <asp:CalendarExtender ID="TxtFecSolFin_CalendarExtender" runat="server" TargetControlID="TxtFecSolFin"
                    Enabled="True" PopupButtonID="imgFec2" >
                </asp:CalendarExtender>
            </div>
        </td>
        <td align="left">
            <div style="width: 71px">
                <asp:Image ID="imgFec2" runat="server" src="/UserControls/filters/images/Calendar.gif"
                    Style="cursor: hand" />
            </div>
        </td>
        <td align="left">
            <div style="width: 25px">
                <asp:Button ID="btnDelete" runat="server" Text="x"></asp:Button>
            </div>
        </td>
    </tr>
</table>
