<%@ Page Title="Record Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Record-Search.aspx.cs" Inherits="NUSMed_WebApp.Researcher.Record_Search" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
        <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-search"></i>Record Search</h1>
            <p class="lead">!</p>
        </div>
        </div>
  <asp:GridView ID="GridViewAnonRecords" Cssclass="table table-hover" runat="server">

</asp:GridView>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
