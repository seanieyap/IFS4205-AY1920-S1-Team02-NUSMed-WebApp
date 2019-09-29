<%@ Page Title="Researcher Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Researcher-Data.aspx.cs" Inherits="NUSMed_WebApp.Admin.Researcher_Data" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-table"></i>Researcher Data</h1>
            <p class="lead mb-1">Restricted. This page is meant for administrators only.</p>
            <p class="text-muted">Use the button bellow to re-anonymise data for researcher functionalities.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-12 text-center">
                    <asp:LinkButton ID="LinkbuttonDataGenerate" CssClass="btn btn-success" runat="server" OnClick="LinkbuttonDataGenerate_Click"><i class="fas fa-fw fa-redo"></i>Re-Generate Data</asp:LinkButton>
                    <br />
                    <asp:Label ID="tempLabel" runat="server">

                    </asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanel" DisplayAfter="200" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
