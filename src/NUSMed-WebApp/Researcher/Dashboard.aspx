<%@ Page Title="Researcher" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="NUSMed_WebApp.Researcher.Dashboard" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-tachometer-alt"></i>Researcher Dashboard</h1>
            <p class="lead">Restricted. This page is meant for Researchers only.</p>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-12 col-md-6">
                <div class="card border-0">
                    <div class="card-body text-center text-md-left">
                        <h5 class="card-title"><i class="fas fa-fw fa-search"></i>Data Search</h5>
                        <p class="card-text">Search for anonymised Records for your research purposes.</p>
                        <a href="~/Researcher/Search-Data" class="btn btn-nus-orange mb-2" runat="server">Go to Search Data <i class="fa fa-angle-double-right"></i></a>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
