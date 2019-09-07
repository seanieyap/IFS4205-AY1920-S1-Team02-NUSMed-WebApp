<%@ Page Title="View Accounts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="NUSMed_WebApp.Admin.Account.View" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-users"></i>View Accounts</h1>
            <p class="lead mb-1">Restricted. This page is meant for administrators only.</p>
            <p class="text-muted">Note: Search is set to return maximum results of 50.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelAccounts" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row mb-4">
                <div class="col-12 col-sm-8 col-md-6 col-lg-5 col-xl-4 mx-auto">
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Search</span>
                        </div>
                        <asp:TextBox ID="TextboxSearch" CssClass="form-control" placeholder="NRIC" runat="server"></asp:TextBox>
                        <div class="input-group-append">
                            <asp:LinkButton ID="ButtonSearch" CssClass="btn btn-outline-info" OnClick="ButtonSearch_Click" runat="server">
                                        <i class="fas fa-fw fa-search"></i> Go
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <asp:GridView ID="GridViewAccounts" CssClass="table table-sm table-responsive-md" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary" EditRowStyle-CssClass="table-active"
                        ItemType="NUSMed_WebApp.Classes.Entity.Account" DataKeyNames="nric" OnRowDeleting="GridViewAccounts_RowDeleting" OnRowCommand="GridViewAccounts_RowCommand"
                        OnPageIndexChanging="GridViewAccounts_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="NRIC">
                                <ItemTemplate>
                                    <%# Item.nric %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-id-card"></i>Personal
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton CssClass="view-modal btn btn-info btn-sm" runat="server" CommandName="ViewPersonal" CommandArgument='<%# Item.nric %>'>
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-phone-square"></i>Contact
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton CssClass="view-modal btn btn-info btn-sm" runat="server" CommandName="ViewContact" CommandArgument='<%# Item.nric %>'>
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-user-injured"></i>Patient
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton CssClass="view-modal btn btn-info btn-sm" runat="server" CommandName="ViewPatient" CommandArgument='<%# Item.nric %>'>
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </asp:LinkButton>
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-user-md"></i>Therapist
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton CssClass="view-modal btn btn-info btn-sm" runat="server" CommandName="ViewTherapist" CommandArgument='<%# Item.nric %>'>
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-user-graduate"></i>Researcher
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton CssClass="view-modal btn btn-info btn-sm" runat="server" CommandName="ViewResearcher" CommandArgument='<%# Item.nric %>'>
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-user-circle"></i>Status
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <button type="button" class="view-modal btn btn-info btn-sm">
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </button>
                                    <div class="modal" tabindex="-1" role="dialog" data-keyboard="false">
                                        <div class="modal-dialog model-lg modal-dialog-centered" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-user-circle"></i><%# Item.nric %>: View Status</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="container-fluid">
                                                        <div class="row text-left">
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Last 1FA Login</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.last1FALogin == null ? "Nil" : Item.last1FALogin.ToString() %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Last Full Login</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.lastFullLogin == null ? "Nil" : Item.lastFullLogin.ToString() %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Registered On</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.createTime.ToString() %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12">
                                                                <div class="form-group">
                                                                    <label>Account Status: <%# Item.accountStatus %></label>
                                                                    <br />
                                                                    <div class="btn-group btn-group-sm border" role="group" aria-label="Account Status">
                                                                        <asp:LinkButton CssClass='<%# Convert.ToInt32(Eval("status")) == 0 ? "btn disabled" : "btn btn-danger" %>' runat="server" CommandName="StatusDisable" data-toggle="confirmation" data-title="Confirm?" CommandArgument='<%# Eval("nric") %>'>
                                                                            <i class="fas fa-fw fa-lock"></i> Disable Account
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton CssClass='<%# Convert.ToInt32(Eval("status")) == 1 ? "btn disabled" : "btn btn-success" %>' runat="server" CommandName="StatusEnable" data-toggle="confirmation" data-title="Confirm?" CommandArgument='<%# Eval("nric") %>'>
                                                                            <i class="fas fa-fw fa-unlock-alt"></i> Enable Account
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton CssClass='<%# Convert.ToInt32(Eval("status")) == 2 ? "btn disabled" : "btn btn-warning" %>' runat="server" CommandName="StatusEnableWoMFA" data-toggle="confirmation" data-title="Confirm?" CommandArgument='<%# Eval("nric") %>'>
                                                                            <i class="fas fa-fw fa-unlock"></i> Enable Account and Omit from MFA 
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <p class="text-info mb-0 mx-auto"><i class="fas fa-fw fa-info-circle"></i>Account Information cannot be Edited.</p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-mobile-alt"></i>MFA
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <button type="button" class="view-modal btn btn-info btn-sm">
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </button>
                                    <div class="modal" tabindex="-1" role="dialog" data-keyboard="false">
                                        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-mobile-alt"></i><%# Item.nric %>: View MFA Information</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="container-fluid">
                                                        <div class="row text-left">
                                                            <div class="col-12">
                                                                <div class="form-group">
                                                                    <label>MFA Token Status: <%# Item.MFATokenStatus %></label>
                                                                    <div class="input-group">
                                                                        <div class="input-group-prepend">
                                                                            <span class="input-group-text">MFA Token ID</span>
                                                                        </div>
                                                                        <asp:TextBox ID="TextboxMFATokenIDUpdate" CssClass="form-control" placeholder="Leave blank to Disable" aria-label="Leave blank to Disable" aria-describedby="LinkButtonMFATokenIDEdit" runat="server"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <asp:LinkButton CssClass="btn btn-outline-success" runat="server" CommandName="MFATokenIDUpdate" data-toggle="confirmation" data-title="Confirm?" CommandArgument='<%# Item.nric  + "; " + Container.DataItemIndex %>'>
                                                                                <i class="far fa-fw fa-save"></i>
                                                                            </asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-12">
                                                                <div class="form-group">
                                                                    <label>MFA Device Status: <%# Item.MFADeviceStatus %></label>
                                                                    <div class="form-group input-group">
                                                                        <div class="input-group-prepend">
                                                                            <span class="input-group-text">MFA Device ID</span>
                                                                        </div>
                                                                        <asp:TextBox ID="TextboxMFADeviceIDUpdate" CssClass="form-control" placeholder="Leave blank to Enable Reset" aria-label="Leave blank to Enable Reset" aria-describedby="LinkButtonMFADeviceIDEdit" runat="server"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <asp:LinkButton CssClass="btn btn-outline-success" runat="server" CommandName="MFADeviceIDUpdate" data-toggle="confirmation" data-title="Confirm?" CommandArgument='<%# Item.nric  + "; " + Container.DataItemIndex %>'>
                                                                                <i class="far fa-fw fa-save"></i>
                                                                            </asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <p class="text-info mb-0 mx-auto"><i class="fas fa-fw fa-info-circle"></i>MFA Device cannot be automatically registered when first logged into mobile app.</p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Roles" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-user-tag"></i>Roles
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <button type="button" class="view-modal btn btn-info btn-sm">
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </button>
                                    <div class="modal" tabindex="-1" role="dialog" data-keyboard="false">
                                        <div class="modal-dialog modal-dialog-centered" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-user-tag"></i><%# Item.nric %>: Account Roles</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="container-fluid">
                                                        <div class="row text-left">
                                                            <div class="col-12">
                                                                <div class="form-group">
                                                                    <label><i class="fas fa-fw fa-user-injured"></i>Patient: <%# Convert.ToBoolean(Eval("patientStatus")) ? "Enabled" : "Disabled" %></label>
                                                                    <asp:LinkButton CssClass="float-right" runat="server" CommandName="RoleDisablePatient" data-toggle="confirmation" data-title="Disable?" CommandArgument='<%# Eval("nric") %>' Visible='<%# Convert.ToBoolean(Eval("patientStatus")) %>'>
                                                                        <i class="fas fa-fw fa-toggle-off"></i>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton CssClass="float-right" runat="server" CommandName="RoleEnablePatient" data-toggle="confirmation" data-title="Enable?" CommandArgument='<%# Eval("nric") %>' Visible='<%# !Convert.ToBoolean(Eval("patientStatus")) %>'>
                                                                        <i class="fas fa-fw fa-toggle-on"></i>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                            <div class="col-12">
                                                                <div class="form-group">
                                                                    <label><i class="fas fa-fw fa-user-md"></i>Therapist: <%# Convert.ToBoolean(Eval("therapistStatus")) ? "Enabled" : "Disabled" %></label>
                                                                    <asp:LinkButton CssClass="float-right" runat="server" CommandName="RoleDisableTherapist" data-toggle="confirmation" data-title="Disable?" CommandArgument='<%# Eval("nric") %>' Visible='<%# Convert.ToBoolean(Eval("therapistStatus")) %>'>
                                                                        <i class="fas fa-fw fa-toggle-off"></i>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton CssClass="float-right" runat="server" CommandName="RoleEnableTherapist" data-toggle="confirmation" data-title="Enable?" CommandArgument='<%# Eval("nric") %>' Visible='<%# !Convert.ToBoolean(Eval("therapistStatus")) %>'>
                                                                        <i class="fas fa-fw fa-toggle-on"></i>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                            <div class="col-12">
                                                                <div class="form-group">
                                                                    <label><i class="fas fa-fw fa-user-graduate"></i>Researcher: <%# Convert.ToBoolean(Eval("researcherStatus")) ? "Enabled" : "Disabled" %></label>
                                                                    <asp:LinkButton CssClass="float-right" runat="server" CommandName="RoleDisableResearcher" data-toggle="confirmation" data-title="Disable?" CommandArgument='<%# Eval("nric") %>' Visible='<%# Convert.ToBoolean(Eval("researcherStatus")) %>'>
                                                                        <i class="fas fa-fw fa-toggle-off"></i>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton CssClass="float-right" runat="server" CommandName="RoleEnableResearcher" data-toggle="confirmation" data-title="Enable?" CommandArgument='<%# Eval("nric") %>' Visible='<%# !Convert.ToBoolean(Eval("researcherStatus")) %>'>
                                                                        <i class="fas fa-fw fa-toggle-on"></i>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                            <div class="col-12">
                                                                <div class="form-group">
                                                                    <label><i class="fas fa-fw fa-user-secret"></i>Administrator: <%# Convert.ToBoolean(Eval("adminStatus")) ? "Enabled" : "Disabled" %></label>
                                                                    <asp:LinkButton CssClass="float-right" runat="server" CommandName="RoleDisableAdmin" data-toggle="confirmation" data-title="Disable?" CommandArgument='<%# Eval("nric") %>' Visible='<%# Convert.ToBoolean(Eval("adminStatus")) %>'>
                                                                        <i class="fas fa-fw fa-toggle-off"></i>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton CssClass="float-right" runat="server" CommandName="RoleEnableAdmin" data-toggle="confirmation" data-title="Enable?" CommandArgument='<%# Eval("nric") %>' Visible='<%# !Convert.ToBoolean(Eval("adminStatus")) %>'>
                                                                        <i class="fas fa-fw fa-toggle-on"></i>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Admin Actions" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton CssClass="btn btn-danger btn-sm" runat="server" CommandName="Delete" data-toggle="confirmation" data-title="Confirm Deletion?">
                                        Delete <i class="fas fa-fw fa-trash"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <EmptyDataTemplate>
                            <div class="alert alert-info text-center py-4" role="alert">
                                <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Search has no results.
                                </h4>
                                <p>Do try widening your search parameter.</p>
                                <hr>
                                <p class="mb-0">First visit? Try entering a search term and hit "Go"!</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgressAccounts" runat="server" AssociatedUpdatePanelID="UpdatePanelAccounts" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modalPersonal" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelPersonal" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-id-card"></i>
                            <asp:Label ID="labelPersonalNRIC" runat="server"></asp:Label>: View Personal Information</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row text-left">
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>NRIC</label>
                                        <input id="inputPersonalNRIC" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Date of Birth</label>
                                        <input id="inputPersonalDoB" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>First Name</label>
                                        <input id="inputPersonalFirstName" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Last Name</label>
                                        <input id="inputPersonalLastName" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Country of Birth</label>
                                        <input id="inputPersonalCountryofBirth" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Nationality</label>
                                        <input id="inputPersonalNationality" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Sex</label>
                                        <input id="inputPersonalSex" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Gender</label>
                                        <input id="inputPersonalGender" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Martial Status</label>
                                        <input id="inputPersonalMartialStatus" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <p class="text-info mb-0 mx-auto"><i class="fas fa-fw fa-info-circle"></i>Administrators are unable to edit personal information.</p>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelPersonal" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>
    </div>

    <div id="modalContact" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelContact" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-phone-square"></i>
                            <asp:Label ID="labelContactNRIC" runat="server"></asp:Label>: View Contact Information</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row text-left">
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Address</label>
                                        <input id="inputAddress" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Postal Code</label>
                                        <input id="inputPostalCode" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Email Address</label>
                                        <input id="inputEmailAddress" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Contact Number</label>
                                        <input id="inputContactNumber" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <p class="text-info mb-0 mx-auto"><i class="fas fa-fw fa-info-circle"></i>Administrators are unable to edit Contact information.</p>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelPersonal" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>
    </div>

    <div id="modalPatient" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelPatient" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-user-injured"></i>
                            <asp:Label ID="labelPatientNRIC" runat="server"></asp:Label>: View Patient Information</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row text-left">
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Name of Next of Kin</label>
                                        <input id="inputPatientNokName" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Contact Number of Next of Kin</label>
                                        <input id="inputPatientNokContact" type="text" class="form-control" readonly runat="server">
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-12">
                                    <h4>Emergency Configuration</h4>
                                    <p class="text-muted">(Setting this allows Therapist to authenticate using patient's token via his/her MFA device)</p>
                                </div>

                                <div class="col-12">
                                    <p class="lead mb-0">Attending Emergency Therapists</p>
                                </div>
                                <div class="col-12 mt-3">
                                    <asp:GridView ID="GridViewTherapists2" CssClass="table table-sm table-responsive-md" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary" EditRowStyle-CssClass="table-active"
                                        ItemType="NUSMed_WebApp.Classes.Entity.Account" DataKeyNames="nric" OnRowCommand="GridViewTherapists2_RowCommand"
                                        OnPageIndexChanging="GridViewTherapists2_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server">
                                        <Columns>
                                            <asp:TemplateField HeaderText="NRIC">
                                                <ItemTemplate>
                                                    <%# Item.nric %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="First Name">
                                                <ItemTemplate>
                                                    <%# Item.firstName %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Name">
                                                <ItemTemplate>
                                                    <%# Item.lastName %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job Title">
                                                <ItemTemplate>
                                                    <%# Item.therapistJobTitle == string.Empty ?  "Nil" : Item.therapistJobTitle %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department">
                                                <ItemTemplate>
                                                    <%# Item.therapistDepartment == string.Empty ?  "Nil" : Item.therapistDepartment %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="ButtonSearchTherapist" CssClass="btn btn-sm btn-danger" CommandName="RemoveEmergencyTherapist" CommandArgument='<%# Item.nric %>' runat="server">
                                                        <i class="fas fa-fw fa-minus-square"></i> Remove
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="alert alert-info text-center py-4" role="alert">
                                                <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>No Results.
                                                </h4>
                                                <p>There are no Emergency Therapists attending to this Patient.</p>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>


                                <div class="col-12">
                                    <p class="lead mb-0">Add Emergency Therapist</p>
                                    <p class="text-muted">Note: Search is set to return maximum results of 25.</p>
                                </div>
                                <div class="col-12 col-sm-12 col-md-12 col-lg-10 col-xl-8 mx-auto">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">Search</span>
                                        </div>
                                        <asp:TextBox ID="TextboxSearchTherapist" CssClass="form-control" placeholder="NRIC" runat="server"></asp:TextBox>
                                        <div class="input-group-append">
                                            <asp:LinkButton ID="ButtonSearchTherapist" CssClass="btn btn-outline-info" OnClick="ButtonSearchTherapist_Click" runat="server">
                                                <i class="fas fa-fw fa-search"></i> Go
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-12 mt-3">
                                    <asp:GridView ID="GridViewTherapists" CssClass="table table-sm table-responsive-md" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary" EditRowStyle-CssClass="table-active"
                                        ItemType="NUSMed_WebApp.Classes.Entity.Account" DataKeyNames="nric" OnRowCommand="GridViewTherapists_RowCommand"
                                        OnPageIndexChanging="GridViewTherapists_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server">
                                        <Columns>
                                            <asp:TemplateField HeaderText="NRIC">
                                                <ItemTemplate>
                                                    <%# Item.nric %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="First Name">
                                                <ItemTemplate>
                                                    <%# Item.firstName %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Name">
                                                <ItemTemplate>
                                                    <%# Item.lastName %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job Title">
                                                <ItemTemplate>
                                                    <%# Item.therapistJobTitle %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department">
                                                <ItemTemplate>
                                                    <%# Item.therapistDepartment %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="ButtonSearchTherapist" CssClass="btn btn-sm btn-success" CommandName="AddEmergencyTherapist" CommandArgument='<%# Item.nric %>' runat="server">
                                                        <i class="fas fa-fw fa-plus-square"></i> Add
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="alert alert-info text-center py-4 small" role="alert">
                                                <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Search has no results.
                                                </h4>
                                                <p>Do try widening your search parameter.</p>
                                                <hr>
                                                <p class="mb-0">use the textbox above to search for a Therapist to attend to the patient!</p>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <p class="text-info mb-0 mx-auto text-center"><i class="fas fa-fw fa-info-circle"></i>Once the patient has been assigned an Emergency Therapist, the therapist requires physical approval via token scan to receive full permissions and access to patient.</p>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelPatient" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalTherapist" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelTherapist" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-user-md"></i>
                            <asp:Label ID="labelTherapistNRIC" runat="server"></asp:Label>: View Therapist Information</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row text-left">
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Job Title</label>
                                        <input id="inputTherapistJobTitle" type="text" class="form-control" runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Department</label>
                                        <input id="inputTherapistDepartment" type="text" class="form-control" runat="server">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="buttonTherapistUpdate" type="button" class="btn btn-sm btn-success" runat="server" onserverclick="buttonTherapistUpdate_ServerClick"><i class="fas fa-fw fa-edit"></i>Update</button>
                        <span id="spanMessageTherapistDetailsUpdate" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>There are errors in the form.</span>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelTherapist" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalResearcher" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelResearcher" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-user-graduate"></i>
                            <asp:Label ID="labelResearcherNRIC" runat="server"></asp:Label>: View Researcher Information</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row text-left">
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Job Title</label>
                                        <input id="inputResearcherJobTitle" type="text" class="form-control" runat="server">
                                    </div>
                                </div>
                                <div class="col-12 col-lg-6">
                                    <div class="form-group">
                                        <label>Department</label>
                                        <input id="inputResearcherDepartment" type="text" class="form-control" runat="server">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="buttonResearcherUpdate" type="button" class="btn btn-sm btn-success" runat="server" onserverclick="buttonResearcherUpdate_ServerClick"><i class="fas fa-fw fa-edit"></i>Update</button>
                        <span id="spanMessageResearcherUpdate" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>There are errors in the form.</span>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelResearcher" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

</asp:Content>

<asp:Content ID="FooterContent" ContentPlaceHolderID="FooterContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $(function () {
                // Enable confirmations
                $('[data-toggle=confirmation]').confirmation({
                    rootSelector: '[data-toggle=confirmation]'
                });
            });

            $(".view-modal").click(function () {
                $($(this).parent().find(".modal")).modal('show');
            });
        }


        function hideGridViewModals() {
            $('.modal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
    </script>
</asp:Content>
