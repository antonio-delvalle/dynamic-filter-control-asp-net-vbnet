<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucFilter.ascx.vb" Inherits="DynamicFilterControl.ucFiltro" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table id="ucFiltroContainer" style="height: 25px; width: 812px;">
    <tr>
        <td align="center">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </td>
        <td align="right" style="width: 150px" rowspan="2">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:DropDownList ID="ddlAddFilter" runat="server" AutoPostBack="True">
                <asp:ListItem Selected="True">-- Add Filter --</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
