<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NUSMed_WebApp._Default" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">

        <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4">Welcome to NUSMed</h1>
            <p class="lead">Start your journey by logging in, remember to keep your token on standby !</p>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-12 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-4 offset-lg-4">
                <div class="form-group">
                    <label for="inputNRIC">NRIC</label>
                    <input id="inputNRIC" type="text" class="form-control" placeholder="NRIC" required="required" runat="server">
                    <div class="invalid-feedback" runat="server" visible="false">
                        Please Enter your NRIC.
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputPassword">Password</label>
                    <input id="inputPassword" type="password" class="form-control" placeholder="Password" required="required" runat="server">
                    <small id="passwordHelp" class="form-text text-muted">Please approach help-desk to reset forgotten passwords.</small>
                    <div class="invalid-feedback" runat="server" visible="false">
                        Please Enter your Password.
                    </div>
                </div>
                <button type="submit" id="buttonLogin" class="btn btn-success mr-auto ml-auto" runat="server" onserverclick="ButtonLogin_ServerClick">Login</button>
                <span id="spanMessage" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>Invalid NRIC/Password</span>
            </div>
        </div>
    </div>

</asp:Content>
