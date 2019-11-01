<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Change-Password.aspx.cs" Inherits="NUSMed_WebApp.My_Account.Change_Password" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-key"></i>Change Password</h1>
            <p class="lead mb-1">Change your password here! Remember to keep it safe and try not to forget it!</p>
            <p class="text-muted">Has to contain upper and lower case characters, symbols, digits and be at least 12 characters in length.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelChangePassword" class="container" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="row">
                <div class="col-12 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-4 offset-lg-4">
                    <div class="form-group mb-4">
                        <label for="PasswordPasswordCurrent">Current Password</label>
                        <input id="inputPasswordCurrent" type="password" class="form-control form-control-sm" placeholder="Current Password" runat="server">
                    </div>
                    <div class="form-group">
                        <label for="inputPassword">New Password</label>
                        <input id="inputPasswordNew" type="password" class="form-control form-control-sm" placeholder="New Password" runat="server">
                    </div>
                    <div class="form-group">
                        <input id="inputPasswordNewRepeat" type="password" class="form-control form-control-sm" placeholder="Verify New Password" runat="server">
                    </div>
                    <button type="button" id="buttonChangePassword" class="btn btn-sm btn-success" runat="server" onserverclick="buttonChangePassword_ServerClick"><i class="fas fa-fw fa-edit"></i>Update</button>
                    <span id="spanMessage" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-2" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>
                        <asp:Label ID="labelMessage" runat="server"></asp:Label></span>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelChangePassword" DisplayAfter="200" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modelSuccess" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered text-center" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mx-auto">Password has been Updated successfully.</h5>
                </div>
                <div class="modal-body text-success">
                    <p class="mt-2"><i class="fas fa-check-circle fa-8x"></i></p>
                    <p class="text-muted">You have been Logged out.<br />
                        Please use your new password to Login.</p>
                </div>
                <div class="modal-footer">
                    <a class="btn btn-secondary mx-auto" href="~/" role="button" runat="server">Return to Login Page</a>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
