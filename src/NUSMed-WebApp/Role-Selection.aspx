<%@ Page Title="Role Selection" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Role-Selection.aspx.cs" Inherits="NUSMed_WebApp.RoleSelection" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-user-tag"></i>Role Section</h1>
            <p class="lead">Your Account has Multiple Roles, Select a Role to Login !</p>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div id="divPatient" class="col-12 mb-2" runat="server" visible="false">
                <div class="card">
                    <h5 class="card-header"><i class="fas fa-fw fa-user-injured"></i>Login as a Patient</h5>
                    <div class="card-body">
                        <h5 class="card-title">Role: Patient</h5>
                        <p class="card-text">
                            Create and View your Medical Records, Medical Diagnosis and more.                       
                            <button type="button" id="buttonLoginPatient" class="btn btn-success float-right" runat="server" onserverclick="buttonLoginPatient_ServerClick">Select</button>
                        </p>
                    </div>
                </div>
            </div>
            <div id="divTherapist" class="col-12 mb-2" runat="server" visible="false">
                <div class="card">
                    <h5 class="card-header"><i class="fas fa-fw fa-user-md"></i>Login as a Therapist</h5>
                    <div class="card-body">
                        <h5 class="card-title">Role: Therapist</h5>
                        <p class="card-text">
                            View your Patients, their Information, their Medical Records and Diagnosis.                       
                            <button type="button" id="buttonLoginTherapist" class="btn btn-success float-right" runat="server" onserverclick="buttonLoginTherapist_ServerClick">Select</button>
                        </p>
                    </div>
                </div>
            </div>
            <div id="divResearcher" class="col-12 mb-2" runat="server" visible="false">
                <div class="card">
                    <h5 class="card-header"><i class="fas fa-fw fa-user-graduate"></i>Login as a Researcher</h5>
                    <div class="card-body">
                        <h5 class="card-title">Role: Researcher</h5>
                        <p class="card-text">
                            Select and Download Aggregated Data and Records.                       
                            <button type="button" id="buttonLoginResearcher" class="btn btn-success float-right" runat="server" onserverclick="buttonLoginResearcher_ServerClick">Select</button>
                        </p>
                    </div>
                </div>
            </div>
            <div id="divAdmin" class="col-12 mb-2" runat="server" visible="false">
                <div class="card">
                    <h5 class="card-header"><i class="fas fa-fw fa-user-secret"></i>Login as a Administrator</h5>
                    <div class="card-body">
                        <h5 class="card-title">Role: Administrator</h5>
                        <p class="card-text">
                            Manage User Accounts, Information and Status.                       
                            <button type="button" id="buttonLoginAdmin" class="btn btn-success float-right" runat="server" onserverclick="buttonLoginAdmin_ServerClick">Select</button>
                        </p>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
