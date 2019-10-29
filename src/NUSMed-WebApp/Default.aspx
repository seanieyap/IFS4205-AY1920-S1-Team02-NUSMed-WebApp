<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NUSMed_WebApp._Default" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">

    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4">Welcome to NUSMed</h1>
            <p class="lead mb-0">Start your journey by logging in, remember to keep your token on standby !</p>
            <p class="text-muted">Caution: 3 Attempts within 5 minutes will result in Account Lockout.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelAccount" class="container" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="row">
                <asp:Panel CssClass="col-12 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-4 offset-lg-4" runat="server" DefaultButton="ButtonLogin">
                    <div class="form-group">
                        <label for="inputNRIC">NRIC</label>
                        <input id="inputNRIC" type="text" class="form-control" placeholder="NRIC" runat="server" autocomplete="off">
                    </div>
                    <div class="form-group">
                        <label for="inputPassword">Password</label>
                        <input id="inputPassword" type="password" class="form-control" placeholder="Password" runat="server" autocomplete="off">
                        <small id="passwordHelp" class="form-text text-muted">Please approach help-desk to reset forgotten passwords.</small>
                    </div>
                    <asp:LinkButton ID="ButtonLogin" CssClass="btn btn-success" runat="server" OnClick="ButtonLogin_ServerClick">Login</asp:LinkButton>
                    <span id="spanMessage" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>Invalid Credentials or Account Disabled</span>
                </asp:Panel>
            </div>

            <%--Multiple logins detected--%>
            <asp:Panel ID="multipleLoginsModal" class="modal fade" TabIndex="-1" role="dialog" runat="server" data-backdrop="static" data-keyboard="false" Visible="false" ClientIDMode="Static">
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
                            <p class="mb-1 text-muted small">If you did not perform this action, please report this incident to help-desk.</p>
                        </div>
                        <div class="modal-footer">
                            <button id="buttonMultipleLoginModalClose" type="button" class="btn btn-secondary" data-dismiss="modal" runat="server" onserverclick="buttonCloseModal_ServerClick">Close</button>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="MFAFailModal" class="modal fade" TabIndex="-1" role="dialog" runat="server" data-backdrop="static" data-keyboard="false" Visible="false" ClientIDMode="Static">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"><i class="fas fa-fw fa-exclamation-circle text-danger"></i>Something went wrong with MFA</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" runat="server" onserverclick="buttonCloseModal_ServerClick">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <p class="mb-0">There were issues with Multi-Factor Authentication (MFA).</p>
                            <p class="mb-1 text-muted small">If you suspect Suspicious Activity, please report to help-desk.</p>
                        </div>
                        <div class="modal-footer">
                            <button id="button1" type="button" class="btn btn-secondary" data-dismiss="modal" runat="server" onserverclick="buttonCloseModal_ServerClick">Close</button>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="failAuthModal" class="modal fade" TabIndex="-1" role="dialog" runat="server" data-backdrop="static" data-keyboard="false" Visible="false" ClientIDMode="Static">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"><i class="fas fa-fw fa-exclamation-circle text-danger"></i>Something went wrong with Authentication</h5>
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

            <asp:Panel ID="NoRoleModal" runat="server" class="modal fade" TabIndex="-1" role="dialog" data-backdrop="static" data-keyboard="false" Visible="false" ClientIDMode="Static">
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
                            <a class="btn btn-secondary mx-auto" href="~/" role="button" runat="server">Cancel</a>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelAccount" DisplayAfter="200" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <%--MFA Modal--%>
    <div id="modalMFA" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered text-center" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mx-auto">Multi-Factor Authentication (MFA)</h5>
                </div>
                <asp:UpdatePanel ID="UpdatePanelMFA" class="modal-body text-warning" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Timer ID="TimerMFA" OnTick="TimerMFA_Tick" runat="server" Interval="1000" Enabled="false" />
                        <p class="mt-2">
                            <asp:Label ID="LabelTimer" runat="server" CssClass="display-1" Text="30"></asp:Label>
                            <asp:Label ID="LabelSeconds" runat="server" CssClass="text-muted small" Text="Seconds"></asp:Label>
                        </p>
                        <p id="pSubMessage" class="text-muted" runat="server">Please use your Registered Device to scan your Token.</p>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="TimerMFA" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <a class="btn btn-secondary mx-auto" href="~/" role="button" runat="server">Cancel</a>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
