<%@ Page Title="Patient Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="NUSMed_WebApp.Patient.Dashboard" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-tachometer-alt"></i>Patient Dashboard</h1>
            <p class="lead">Restricted. This page is meant for Patients only.</p>
        </div>
    </div>

    <div class="container">
        <div class="row">
                        <div class="col-12 col-md-6">
                <div class="card border-0">
                    <div class="card-body text-center text-md-left">
                        <h5 class="card-title"><i class="fas fa-fw fa-poll-h"></i>My Diagnoses</h5>
                        <p class="card-text">View your diagnoses that therapists have attributed you with.</p>
                        <a href="~/Patient/My-Diagnoses" class="btn btn-nus-orange mb-2" runat="server">View My Diagnoses <i class="fa fa-angle-double-right"></i></a>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="card border-0">
                    <div class="card-body text-center text-md-left">
                        <h5 class="card-title"><i class="fas fa-fw fa-user-md"></i>My Therapists</h5>
                        <p class="card-text">View your therapists and manage their permissions.</p>
                        <a href="~/Patient/My-Therapists" class="btn btn-nus-orange mb-2" runat="server">View Therapists <i class="fa fa-angle-double-right"></i></a>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="card border-0">
                    <div class="card-body text-center text-md-left">
                        <h5 class="card-title"><i class="fas fa-fw fa-file-medical"></i>My Records</h5>
                        <p class="card-text">Create and view your Medical Records. Viewing of Medical Records can be done via the "view" page and creation of new records can be done via the "New Records" page.</p>
                        <a href="~/Patient/My-Records/View" class="btn btn-nus-orange mb-2" runat="server">View Records <i class="fa fa-angle-double-right"></i></a>
                        <a href="~/Patient/My-Records/New-Record" class="btn btn-nus-orange mb-2" runat="server">Create New Record <i class="fa fa-angle-double-right"></i></a>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
