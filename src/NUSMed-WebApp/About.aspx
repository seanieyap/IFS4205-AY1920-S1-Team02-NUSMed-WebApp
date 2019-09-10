<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="NUSMed_WebApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Your application description page.</h3>
    <p>Use this area to provide additional information.</p>
    <asp:Button runat="server" OnClick="Unnamed_Click" Text="fake mfa success login"/>
    <asp:Button runat="server" OnClick="Unnamed_Click1" Text="fake mfa fail login"/>
</asp:Content>
