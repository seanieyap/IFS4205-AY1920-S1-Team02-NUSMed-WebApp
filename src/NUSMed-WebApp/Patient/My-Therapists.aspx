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
                    <asp:GridView ID="GridViewTherapist" CssClass="table table-sm" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary" EditRowStyle-CssClass="table-active"
                        ItemType="NUSMed_WebApp.Classes.Entity.Therapist" DataKeyNames="nric" OnRowCommand="GridViewPatient_RowCommand"
                        OnPageIndexChanging="GridViewPatient_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server" OnRowDataBound="GridViewPatient_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.lastName + " " + Item.firstName %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Title" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.therapistJobTitle %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.therapistDepartment %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--                            <asp:TemplateField HeaderText="Information" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelInformationStatus" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:LinkButton ID="LinkButtonViewInformation" runat="server"><i class="fas fa-fw fa-eye"></i>View</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Permissions" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelPermissionStatus" TabIndex="0" data-toggle="tooltip" runat="server"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:LinkButton ID="LinkButtonViewPermission" CssClass="btn btn-info btn-sm" runat="server" CommandArgument='<%# Item.nric %>' CommandName="ViewPermission"><i class="fas fa-fw fa-eye"></i>View</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fine-Grain Permissions" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelRecordStatus" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:LinkButton ID="LinkButtonViewRecords" runat="server"><i class="fas fa-fw fa-eye"></i>View</asp:LinkButton>
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

    <div id="modalPermissions" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelPermissions" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title">View Permissions of
                            <asp:Label ID="LabelTherapistName" runat="server"></asp:Label></h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <h4 class="text-center">Instructions</h4>
                                <ul>
                                    <li>To Approve request sent by therapist, validate the existing permissions he/she has and assess the permissions requested for. When done, simply click "Approve".</li>
                                    <li>In event of unsatisfactory request, you are able to edit the request and approve a different set of permissions.</li>
                                </ul>
                            </div>
                        </div>

                        <hr />

                        <div class="row">
                            <div class="col-12">
                                <h4 class="text-center">Current Permissions</h4>

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
                                <h4 class="mb-3 text-center">Requested Permissions</h4>

                                <div class="row">
                                    <div class="col-12 text-center mb-3">
                                        <button type="button" class="btn btn-sm btn-info" runat="server" onclick=" $('.checkboxes .form-check-input input:checkbox').prop('checked', true);">Select All</button>
                                        <button type="button" class="btn btn-sm btn-info mr-2" runat="server" onclick=" $('.checkboxes .form-check-input input:checkbox').prop('checked', false);">Deselect All</button>
                                    </div>

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
                                <div class="alert alert-info my-2 text-center" role="alert">
                                    <i class="fas fa-fw fa-info-circle"></i>
                                    <asp:Label ID="modalPermissionStatus" runat="server"></asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="buttonPermissionApprove" type="button" class="btn btn-sm btn-success" runat="server" onserverclick="buttonPermissionApprove_ServerClick"><i class="fas fa-fw fa-clipboard-check"></i>Approve</button>
                        <%--<span class="text-muted small">(Use this button to update your requests too)</span>--%>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelPermissions" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
