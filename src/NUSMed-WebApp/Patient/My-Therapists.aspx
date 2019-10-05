<%@ Page Title="My Therapists" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="My-Therapists.aspx.cs" Inherits="NUSMed_WebApp.Patient.My_Therapists" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-user-md"></i>My Therapists</h1>
            <p class="lead">View your Therapists and manage their permissions.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelAccounts" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row mb-4">
                <div class="col-12 col-sm-8 col-md-6 col-lg-5 col-xl-4 mx-auto">
                    <asp:Panel CssClass="input-group input-group-sm" runat="server" DefaultButton="ButtonSearch">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Search</span>
                        </div>
                        <asp:TextBox ID="TextboxSearch" CssClass="form-control form-control-sm" placeholder="First Name / Last Name" runat="server"></asp:TextBox>
                        <div class="input-group-append">
                            <asp:LinkButton ID="ButtonSearch" CssClass="btn btn-outline-info" OnClick="ButtonSearch_Click" runat="server">
                                <i class="fas fa-fw fa-search"></i> Go
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <asp:GridView ID="GridViewTherapist" CssClass="table table-sm" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None"  
                        ItemType="NUSMed_WebApp.Classes.Entity.Therapist" DataKeyNames="nric" OnRowCommand="GridViewTherapist_RowCommand"
                        OnPageIndexChanging="GridViewTherapist_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server" OnRowDataBound="GridViewTherapist_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Therapist's Name" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.lastName + " " + Item.firstName %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Title" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.therapistJobTitle == string.Empty ?  "Nil" : Item.therapistJobTitle %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.therapistDepartment == string.Empty ?  "Nil" : Item.therapistDepartment %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Permissions" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelPermissionStatus" TabIndex="0" data-toggle="tooltip" runat="server"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:Label ID="LabelPermissionEmergencyStatus" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:LinkButton ID="LinkButtonViewPermission" CssClass="btn btn-info btn-sm" runat="server" CommandArgument='<%# Item.nric %>' CommandName="ViewPermission">
                                        <i class="fas fa-fw fa-eye"></i></i><span class="d-none d-lg-inline-block">View</span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info text-center py-4" role="alert">
                                <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Search returned no results.
                                </h4>
                                <p>Do try widening your search parameter.</p>
                                <hr>
                                <p class="mb-0">New here? Try entering a search term and hit "Go"!</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelAccounts" DisplayAfter="200" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modalPermissions" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelPermissions" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title">Therapist 
                            <asp:Label ID="LabelTherapistName" runat="server"></asp:Label>: View Permissions</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <h4>Instructions</h4>
                                <ul class="small">
                                    <li>To Approve request sent by therapist, validate the existing permissions he/she has and assess the permissions requested for. When done, simply click "Approve".</li>
                                    <li>In event of unsatisfactory request, you are able to edit the request and approve a different set of permissions.</li>
                                    <li>In event of end of treatment and to prevent therapists from viewing your information and diagnosis; reset permissions to an "Unapproved" status by using the "Revoke" button.</li>
                                </ul>
                            </div>
                        </div>

                        <hr />

                        <div class="row">
                            <div class="col-12">
                                <h4 class="mb-4">Current Permissions</h4>
                                <div class="row">
                                    <div class="col-12">
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeHeightMeasurementApproved" CssClass="form-check-input" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeHeightMeasurementApproved.ClientID %>">Height Measurement</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeWeightMeasurementApproved" CssClass="form-check-input" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeWeightMeasurementApproved.ClientID %>">Weight Measurement</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeTemperatureReadingApproved" CssClass="form-check-input" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeTemperatureReadingApproved.ClientID %>">Temperature Reading</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeBloodPressureReadingApproved" CssClass="form-check-input" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeBloodPressureReadingApproved.ClientID %>">Blood Pressure Reading</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeECGReadingApproved" CssClass="form-check-input" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeECGReadingApproved.ClientID %>">ECG Reading</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeMRIApproved" CssClass="form-check-input" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeMRIApproved.ClientID %>">MRI</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeXRayApproved" CssClass="form-check-input" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeXRayApproved.ClientID %>">X-ray</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeGaitApproved" CssClass="form-check-input" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeGaitApproved.ClientID %>">Gait</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <hr />

                        <div class="row">
                            <div class="col-12">
                                <h4 class="mb-4">Requested Permissions
                                    <div class="float-right">
                                        <button type="button" class="btn btn-sm btn-info" runat="server" onclick=" $('.checkboxes .form-check-input input:checkbox').prop('checked', true);">Select All</button>
                                        <button type="button" class="btn btn-sm btn-info mr-2" runat="server" onclick=" $('.checkboxes .form-check-input input:checkbox').prop('checked', false);">Deselect All</button>
                                    </div>
                                </h4>
                                <div class="row">
                                    <div class="col-12">
                                        <div class="row mb-2 checkboxes">
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeHeightMeasurement" CssClass="form-check-input" runat="server" ClientIDMode="Static" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeHeightMeasurement.ClientID %>">Height Measurement</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeWeightMeasurement" CssClass="form-check-input" runat="server" ClientIDMode="Static" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeWeightMeasurement.ClientID %>">Weight Measurement</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeTemperatureReading" CssClass="form-check-input" runat="server" ClientIDMode="Static" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeTemperatureReading.ClientID %>">Temperature Reading</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeBloodPressureReading" CssClass="form-check-input" runat="server" ClientIDMode="Static" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeBloodPressureReading.ClientID %>">Blood Pressure Reading</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeECGReading" CssClass="form-check-input" runat="server" ClientIDMode="Static" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeECGReading.ClientID %>">ECG Reading</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeMRI" CssClass="form-check-input" runat="server" ClientIDMode="Static" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeMRI.ClientID %>">MRI</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeXRay" CssClass="form-check-input" runat="server" ClientIDMode="Static" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeXRay.ClientID %>">X-ray</label>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-lg-4">
                                                <div class="form-check form-check-inline">
                                                    <asp:CheckBox ID="CheckBoxTypeGait" CssClass="form-check-input" runat="server" ClientIDMode="Static" />
                                                    <label class="form-check-label" for="<%= CheckBoxTypeGait.ClientID %>">Gait</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-12">
                                <div id="DivModalPermissionStatus" role="alert" runat="server">
                                    <i class="fas fa-fw fa-info-circle"></i>
                                    <asp:Label ID="modalPermissionStatus" runat="server"></asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="buttonPermissionApprove" type="button" class="btn btn-sm btn-success" runat="server" onserverclick="buttonPermissionApprove_ServerClick"><i class="fas fa-fw fa-clipboard-check"></i>Approve</button>
                        <asp:LinkButton ID="buttonPermissionRevoke" CssClass="btn btn-sm btn-danger" runat="server" data-toggle="confirmation" data-title="Therapist will not be able to view your information, diagnosis and records. Confirm?" OnClick="buttonPermissionRevoke_ServerClick"><i class="fas fa-fw fa-ban"></i>Revoke</asp:LinkButton>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelPermissions" DisplayAfter="200" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $(function () {
                // Enable Tooltips
                $('[data-toggle="tooltip"]').tooltip({ html: true });

                // Enable confirmations
                $('[data-toggle=confirmation]').confirmation({
                    rootSelector: '[data-toggle=confirmation]'
                });
            });
        }
    </script>
</asp:Content>
