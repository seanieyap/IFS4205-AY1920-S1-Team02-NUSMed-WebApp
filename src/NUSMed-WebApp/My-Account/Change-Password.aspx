<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Change-Password.aspx.cs" Inherits="NUSMed_WebApp.My_Account.Change_Password" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-key"></i>Change Password</h1>
            <p class="lead mb-1">Change your password here! Remember to keep it safe and try not to forget it!</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelChangePassword" class="container" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="row">
                <div class="col-12 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-4 offset-lg-4">
                    <div class="form-group mb-4">
                        <label for="PasswordPasswordCurrent">Current Password</label>
                        <input id="inputPasswordCurrent" type="password" class="form-control" placeholder="Current Password" runat="server">
                    </div>
                    <div class="form-group">
                        <label for="inputPassword">New Password</label>
                        <input id="inputPasswordNew" type="password" class="form-control" placeholder="New Password" runat="server">
                    </div>
                    <div class="form-group">
                        <input id="inputPasswordNewRepeat" type="password" class="form-control" placeholder="Verify New Password" runat="server">
                        <small id="" class="form-text text-muted">Has to contain symbols, digits and 12 characters in length.</small>
                    </div>
                    <button type="submit" id="buttonChangePassword" class="btn btn-sm btn-success" runat="server" onserverclick="buttonChangePassword_ServerClick"><i class="fas fa-fw fa-edit"></i>Update</button>
                    <span id="spanMessage" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-2" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>
                        <asp:Label ID="labelMessage" runat="server"></asp:Label></span>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelChangePassword" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
