<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="DynamicFilterControl.WebForm1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UserControls/ucFilter.ascx" TagName="ucFilter" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scrip1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <uc1:ucFilter ID="ucFilter1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnFilter" runat="server" Height="22px" UseSubmitBehavior="false"
            Text="Filter" Width="130"></asp:Button>
        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFilter" />
            </Triggers>
            <ContentTemplate>
                <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true">
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
