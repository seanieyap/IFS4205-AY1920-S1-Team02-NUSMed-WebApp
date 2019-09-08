<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NUSMed_WebApp._Default" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">

    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4">Welcome to NUSMed</h1>
            <p class="lead">Start your journey by logging in, remember to keep your token on standby !</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelAccount" class="container" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="row">
                <div class="col-12 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-4 offset-lg-4">
                    <div class="form-group">
                        <label for="inputNRIC">NRIC</label>
                        <input id="inputNRIC" type="text" class="form-control" placeholder="NRIC" runat="server">
                    </div>
                    <div class="form-group">
                        <label for="inputPassword">Password</label>
                        <input id="inputPassword" type="password" class="form-control" placeholder="Password" runat="server">
                        <small id="passwordHelp" class="form-text text-muted">Please approach help-desk to reset forgotten passwords.</small>
                    </div>
                    <button type="submit" id="buttonLogin" class="btn btn-success" runat="server" onserverclick="ButtonLogin_ServerClick">Login</button>
                    <span id="spanMessage" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>Invalid NRIC/Password</span>
                </div>
            </div>

            <%--Multiple logins detected--%>
            <asp:Panel ID="multipleLoginsModal" class="modal" TabIndex="-1" role="dialog" runat="server" data-backdrop="static" data-keyboard="false" Visible="false" ClientIDMode="Static">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"><i class="fas fa-fw fa-exclamation-circle text-danger"></i>Multiple Logins Detected</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" runat="server" onserverclick="buttonCloseModal_ServerClick">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <p class="mb-0">You have been logged in via another location.</p>
                            <p class="mb-1 text-muted small">If you did not perform these actions, please report this incident to help-desk.</p>
                        </div>
                        <div class="modal-footer">
                            <button id="buttonMultipleLoginModalClose" type="button" class="btn btn-secondary" data-dismiss="modal" runat="server" onserverclick="buttonCloseModal_ServerClick">Close</button>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="failAuthModal" class="modal" TabIndex="-1" role="dialog" runat="server" data-backdrop="static" data-keyboard="false" Visible="false" ClientIDMode="Static">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"><i class="fas fa-fw fa-exclamation-circle text-danger"></i>Fail to Authenticate</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" runat="server" onserverclick="buttonCloseModal_ServerClick">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <p class="mb-0">There were issues with Authenticating this session.</p>
                            <p class="mb-1 text-muted small">If you suspect Suspicious Activity, please report to help-desk.</p>
                        </div>
                        <div class="modal-footer">
                            <button id="buttonFailAuthModalClose" type="button" class="btn btn-secondary" data-dismiss="modal" runat="server" onserverclick="buttonCloseModal_ServerClick">Close</button>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="NoRoleModal" class="modal" TabIndex="-1" role="dialog" runat="server" data-backdrop="static" data-keyboard="false" Visible="false" ClientIDMode="Static">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"><i class="fas fa-fw fa-exclamation-circle text-danger"></i>Error</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" runat="server" onserverclick="buttonCloseModal_ServerClick">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <p class="mb-0">You There are no Roles enabled for this account.</p>
                            <p class="mb-1 text-muted small">Please contact Administrator if there is not expected.</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal" runat="server" onserverclick="buttonCloseModal_ServerClick">Close</button>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgressOptions" runat="server" AssociatedUpdatePanelID="UpdatePanelAccount" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    
            <%--MFA Modal--%>
            <%--            <asp:Panel ID="multipleRolesModal" class="modal" TabIndex="-1" role="dialog" runat="server" data-backdrop="static" data-keyboard="false" Visible="false" ClientIDMode="Static">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"><i class="fas fa-fw fa-tag text-warning"></i>Select Roles</h5>
                        </div>
                        <div class="modal-body">
                            <p class="mb-0">There are multiple roles associated with your account.</p>
                            <p class="mb-1 text-muted small">Please select.</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                        </div>
                    </div>
                </div>
            </asp:Panel>--%>


</asp:Content>
