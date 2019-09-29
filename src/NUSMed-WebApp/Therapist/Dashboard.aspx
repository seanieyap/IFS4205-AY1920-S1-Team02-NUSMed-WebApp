<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="NUSMed_WebApp.Therapist.Dashboard" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-tachometer-alt"></i>Therapist Dashboard</h1>
            <p class="lead">Restricted. This page is meant for therapists only.</p>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-12 col-md-6">
                <div class="card border-0">
                    <div class="card-body text-center text-md-left">
                        <h5 class="card-title"><i class="fas fa-fw fa-user-injured"></i>My Patients</h5>
                        <p class="card-text">Manage your patients all in one place. Start by submitting a request for permissions via the "New Request" page. Following that, view patient information and records via the "view" page.</p>
                        <a href="~/Therapist/My-Patients/View" class="btn btn-nus-orange mb-2" runat="server">View Patients <i class="fa fa-angle-double-right"></i></a>
                        <a href="~/Therapist/My-Patients/New-Request" class="btn btn-nus-orange mb-2" runat="server">Submit New Request <i class="fa fa-angle-double-right"></i></a>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="card border-0">
                    <div class="card-body text-center text-md-left">
                        <h5 class="card-title"><i class="fas fa-fw fa-database"></i>My Medical Notes</h5>
                        <p class="card-text">Create and view medical notes for communications between therapists like yourself. Sharing of medical notes can also be done via the "view" page!</p>
                        <a href="~/Therapist/My-Medical-Notes/View" class="btn btn-nus-orange mb-2" runat="server">View Medical Notes<i class="fa fa-angle-double-right"></i></a>
                        <a href="~/Therapist/My-Medical-Notes/New-Medical-Note" class="btn btn-nus-orange mb-2" runat="server">Create New Medical Note <i class="fa fa-angle-double-right"></i></a>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
