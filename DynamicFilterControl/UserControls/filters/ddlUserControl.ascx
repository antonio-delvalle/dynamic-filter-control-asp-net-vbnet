<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ddlUserControl.ascx.vb"
    Inherits="DynamicFilterControl.ddlUserControl" %>
<table class="TablaFiltro" height="25" id="ddlCustomControlFilter" border="0" cellspacing="0">
    <tr>
        <td align="left">
            <div style="width: 180px">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </div>
        </td>
        <td alignt="left">
            <div style="width: 469px;text-align:left;">
                <asp:DropDownList ID="ucDropDownList" runat="server" Style="margin-left: 0px;"
                    Width="420px">
                </asp:DropDownList>
            </div>
        </td>
        <td align="left">
            <div style="width: 25px">
                <asp:Button ID="btnDelete" runat="server" Text="x" />
            </div>
        </td>
    </tr>
</table>
