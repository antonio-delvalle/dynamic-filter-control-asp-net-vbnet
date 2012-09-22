<%@ Page Title="Home Page" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeBehind="Default.aspx.vb" Inherits="DynamicFilterControl._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UserControls/ucFilter.ascx" TagName="ucFilter" TagPrefix="uc2" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <uc2:ucFilter ID="ucFilter1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnFilter" runat="server" Text="Filter" />
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
</asp:Content>
