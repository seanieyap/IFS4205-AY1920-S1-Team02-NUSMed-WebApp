<%@ Page Title="Record Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Record-Search.aspx.cs" Inherits="NUSMed_WebApp.Researcher.Record_Search" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-search"></i>Record Search</h1>
            <p class="lead">Records provided here have been anonymized and may not fit your exact search criteria</p>
        </div>
    </div>
    <asp:GridView ID="GridViewAnonRecords" CssClass="table table-hover" runat="server" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pagination-gridview"
        OnPageIndexChanging="GridViewAnonRecords_PageIndexChanging" AutoGenerateColumns="true" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary"
        EmptyDataRowStyle-CssClass="empty-table">
    </asp:GridView>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
