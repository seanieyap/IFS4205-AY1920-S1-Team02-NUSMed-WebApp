﻿<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="NUSMed_WebApp.Admin.Dashboard" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-tachometer-alt"></i>Admin Dashboard</h1>
            <p class="lead">Restricted. This page is meant for administrators only.</p>
        </div>
    </div>
    
    <div class="container">
        <div class="row">
            <div class="card col-12 col-md-6 border-0">
                <div class="card-body text-center text-md-left">
                        <h5 class="card-title"><i class="fas fa-fw fa-users-cog"></i>Manage Accounts</h5>
                    <p class="card-text">Manage all registered accounts in the system or register an account.</p>
                    <a href="~/Admin/Manage-Accounts/View" class="btn btn-nus-orange" runat="server">View Accounts <i class="fa fa-angle-double-right"></i></a>
                    <a href="~/Admin/Manage-Accounts/Register" class="btn btn-nus-orange" runat="server">Register an Account <i class="fa fa-angle-double-right"></i></a>
                </div>
            </div>
            <div class="card col-12 col-md-6 border-0">
                <div class="card-body text-center text-md-left">
                        <h5 class="card-title"><i class="fas fa-fw fa-database"></i>Manage Logs</h5>
                    <p class="card-text">Manage Application Logs of events invoked by the system. Find out why did what and etc.</p>
                    <a href="~/Admin/Manage-Logs/Account-Logs" class="btn btn-nus-orange" runat="server">View Account Logs <i class="fa fa-angle-double-right"></i></a>
                    <a href="~/Admin/Manage-Logs/Record-Logs" class="btn btn-nus-orange" runat="server">View Record Logs <i class="fa fa-angle-double-right"></i></a>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
