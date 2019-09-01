<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="NUSMed_WebApp.Admin.Dashboard" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-tachometer-alt"></i>Admin Dashboard</h1>
            <p class="lead">!</p>
        </div>
    </div>

    <asp:Label runat="server" ID="a"></asp:Label>
    <asp:Label runat="server" ID="b"></asp:Label>
    <asp:Label runat="server" ID="c"></asp:Label>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
