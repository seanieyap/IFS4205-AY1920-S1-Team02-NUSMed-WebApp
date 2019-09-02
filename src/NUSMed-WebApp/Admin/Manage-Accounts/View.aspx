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
                                    <i class="fas fa-fw fa-id-card"></i>Personal Details
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <button type="button" class="view-modal btn btn-info btn-sm">
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </button>
                                    <div class="modal" tabindex="-1" role="dialog" data-keyboard="false">
                                        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-id-card"></i><%# Item.nric %>: View Personal Information</h5>
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
                                                                    <input type="text" class="form-control" readonly value='<%# Item.nric %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Date of Birth</label>
                                                                    <input type="date" class="form-control" readonly value='<%# Item.dateOfBirth.ToString("dd/MM/yyyy") %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>First Name</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.firstName %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Last Name</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.lastName %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Country of Birth</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.countryOfBirth %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Nationality</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.nationality %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Sex</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.sex %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Gender</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.gender %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Martial Status</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.martialStatus %>'>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="modal-footer">
                                                    <p class="text-info mb-0 mx-auto"><i class="fas fa-fw fa-info-circle"></i>Administrators are unable to edit personal information.</p>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-phone-square"></i>Contact Details
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <button type="button" class="view-modal btn btn-info btn-sm">
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </button>
                                    <div class="modal" tabindex="-1" role="dialog" data-keyboard="false">
                                        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-phone-square"></i><%# Item.nric %>: View Contact Details</h5>
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
                                                                    <input type="text" class="form-control" readonly value='<%# Item.address %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Postal Code</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.addressPostalCode %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Email Address</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.email %>'>
                                                                </div>
                                                            </div>
                                                            <div class="col-12 col-lg-6">
                                                                <div class="form-group">
                                                                    <label>Contact Number</label>
                                                                    <input type="text" class="form-control" readonly value='<%# Item.contactNumber %>'>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <p class="text-info mb-0 mx-auto"><i class="fas fa-fw fa-info-circle"></i>Administrators are unable to edit contact information.</p>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <HeaderTemplate>
                                    <i class="fas fa-fw fa-user-circle"></i>Account Details
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <button type="button" class="view-modal btn btn-info btn-sm">
                                        <i class="fas fa-fw fa-eye"></i>View
                                    </button>
                                    <div class="modal" tabindex="-1" role="dialog" data-keyboard="false">
                                        <div class="modal-dialog modal-dialog-centered" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-user-circle"></i><%# Item.nric %>: View Account</h5>
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
                                                                    <div class="btn-group btn-group-sm" role="group" aria-label="Account Status">
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
