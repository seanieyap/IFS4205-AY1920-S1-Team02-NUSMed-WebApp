<%@ Page Title="Role Selection" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Role-Selection.aspx.cs" Inherits="NUSMed_WebApp.RoleSelection" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
        <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-user-tag"></i>Role Section</h1>
            <p class="lead">Your Account has Multiple Roles, Select a Role to Login !</p>
        </div>
    </div>

    <div class="container">
        <div class="col-12 text-center">
            <button type="submit" id="buttonLoginPatient" class="btn btn-success mx-2" runat="server" visible="false" onserverclick="buttonLoginPatient_ServerClick"><i class="fas fa-fw fa-user-injured"></i>Patient</button>
            <button type="submit" id="buttonLoginTherapist" class="btn btn-success mx-2" runat="server" visible="false" onserverclick="buttonLoginTherapist_ServerClick"><i class="fas fa-fw fa-user-md"></i>Therapist</button>
            <button type="submit" id="buttonLoginResearcher" class="btn btn-success mx-2" runat="server" visible="false" onserverclick="buttonLoginResearcher_ServerClick"><i class="fas fa-fw fa-user-graduate"></i>Researcher</button>
            <button type="submit" id="buttonLoginAdmin" class="btn btn-success mx-2" runat="server" visible="false" onserverclick="buttonLoginAdmin_ServerClick"><i class="fas fa-fw fa-user-secret"></i>Admin</button>
        </div>
    </div>
</asp:Content>