<%@ Page Title="View My Patients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="NUSMed_WebApp.Therapist.My_Patients.View" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-user-injured"></i>View My Patients</h1>
            <p class="lead">View your Patients' Information, Records and Request for Permissions.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelAccounts" class="container-fluid" runat="server" UpdateMode="Conditional">
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
                    <asp:GridView ID="GridViewPatient" CssClass="table table-sm" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary" EditRowStyle-CssClass="table-active"
                        ItemType="NUSMed_WebApp.Classes.Entity.Patient" DataKeyNames="nric" OnRowCommand="GridViewPatient_RowCommand"
                        OnPageIndexChanging="GridViewPatient_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server" OnRowDataBound="GridViewPatient_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="NRIC" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.nric %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelNameStatus" title="No Permission" CssClass="text-danger" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:Label ID="LabelName" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Information" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelInformationStatus" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:LinkButton ID="LinkButtonViewInformation" runat="server"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Records" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelRecordStatus" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:LinkButton ID="LinkButtonViewRecords" runat="server"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
                                    <asp:HyperLink ID="LinkButtonNewRecords" runat="server"><i class="fas fa-fw fa-file-medical"></i><span class="d-none d-lg-inline-block">New Record</span></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Permissions" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelPermissionStatus" TabIndex="0" data-toggle="tooltip" runat="server"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:LinkButton ID="LinkButtonViewPermission" CssClass="btn btn-info btn-sm" runat="server" CommandArgument='<%# Item.nric %>' CommandName="ViewPermission"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
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
                        <h5 class="modal-title">Patient 
                            <asp:Label ID="LabelPatientNRIC" runat="server"></asp:Label>: View Permissions</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <h4>Instructions</h4>
                                <ul>
                                    <li>To Update your existing request, simply change your "pending request" and click "Submit".</li>
                                    <li>To Remove your existing request, simply update the "pending request" with no record types selected.</li>
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
                                <div class="alert alert-info my-2 text-center" role="alert">
                                    <i class="fas fa-fw fa-info-circle"></i>
                                    <asp:Label ID="modalPermissionStatus" runat="server"></asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="buttonPermissionRequest" type="button" class="btn btn-sm btn-success" runat="server" onserverclick="buttonPermissionRequest_ServerClick"><i class="fas fa-fw fa-share"></i>Submit</button>
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

    <div id="modalInformation" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelInformation" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title">Patient 
                            <asp:Label ID="LabelInformationNRIC" runat="server"></asp:Label>: View Information</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <h4 class="mb-3 border-bottom">Personal Details</h4>
                        <div class="row text-left mb-3">
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>NRIC</label>
                                    <input id="inputNRIC" type="text" class="form-control" readonly runat="server" disabled="disabled">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Date of Birth</label>
                                    <input id="DateofBirth" type="text" class="form-control" readonly runat="server" disabled="disabled">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>First Name</label>
                                    <input id="FirstName" type="text" class="form-control" readonly runat="server" disabled="disabled">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Last Name</label>
                                    <input id="LastName" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Country of Birth</label>
                                    <input id="CountryofBirth" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Nationality</label>
                                    <input id="Nationality" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Sex</label>
                                    <input id="Sex" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Gender</label>
                                    <input id="Gender" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Marital Status</label>
                                    <input id="MaritalStatus" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                        </div>

                        <%-- Contact Details --%>
                        <h4 class="mb-3 border-bottom">Contact Details</h4>
                        <div class="row text-left mb-3">
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Address</label>
                                    <input id="Address" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Postal Code</label>
                                    <input id="PostalCode" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Email Address</label>
                                    <input id="EmailAddress" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Contact Number</label>
                                    <input id="ContactNumber" type="text" class="form-control" readonly runat="server">
                                </div>
                            </div>
                        </div>

                        <%-- Patient Details --%>
                        <h4 class="mb-3 border-bottom">Next of Kin Details</h4>
                        <div class="row text-left mb-3" runat="server">
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Name of Next of Kin</label>
                                    <input id="NOKName" type="text" class="form-control" runat="server" readonly>
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Contact Number of Next of Kin</label>
                                    <input id="NOKContact" type="text" class="form-control" runat="server" readonly>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelInformation" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalRecords" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelRecords" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title">Patient 
                            <asp:Label ID="LabelRecordsNRIC" runat="server"></asp:Label>: View Records</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <asp:GridView ID="GridViewRecords" CssClass="table table-sm table-responsive-md small" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pagination-gridview"
                                    AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary"
                                    OnRowDataBound="GridViewRecords_RowDataBound" OnRowCommand="GridViewRecords_RowCommand" ItemType="NUSMed_WebApp.Classes.Entity.Record"
                                    OnPageIndexChanging="GridViewRecords_PageIndexChanging" DataKeyNames="id" EmptyDataRowStyle-CssClass="empty-table" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Title">
                                            <ItemTemplate>
                                                <%# Item.title %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <%# Item.type.name %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <%# Item.description %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created By">
                                            <ItemTemplate>
                                                <%# Item.creatorLastName + " " + Item.creatorFirstName %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Creation Time">
                                            <ItemTemplate>
                                                <%# Item.permited ? Item.createTime.ToString() : string.Empty%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Data" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelContent" runat="server" Visible="false"></asp:Label>
                                                <asp:LinkButton ID="LinkbuttonFileView" CssClass="btn btn-info btn-sm" runat="server" Visible="false"></asp:LinkButton>

                                                <a id="FileDownloadLink" class="btn btn-warning btn-sm" runat="server" visible="false">
                                                    <i class="fas fa-fw fa-cloud-download-alt"></i>
                                                </a>
                                                <asp:Label ID="LabelFileType" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                                <asp:Label ID="LabelRecordPermissionStatus" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                    <EmptyDataTemplate>
                                        <div class="alert alert-info text-center py-4" role="alert">
                                            <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Patient has no Records.
                                            </h4>
                                            <p>Perhaps add a record for this patient.</p>
                                            <hr>
                                            <p class="mb-0">Use the button in the footer to visit the Submit New Records page.</p>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:HyperLink ID="modalRecordsHyperlinkNewRecord" CssClass="btn btn-sm btn-info" runat="server"><i class="fas fa-fw fa-file-medical"></i>New Record</asp:HyperLink>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelRecords" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalFileView" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelFileView" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-eye"></i>
                            View Record File:
                            <asp:Label ID="labelRecordName" runat="server"></asp:Label></h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body text-center">
                        <asp:Image ID="modalFileViewImage" CssClass="img-fluid" runat="server" />
                        <video id="modalFileViewVideo" style="width: 100%; height: auto;" controls runat="server">
                            <source id="so" type="video/mp4" runat="server">
                            Your browser does not support the video tag.
                        </video>
                        <asp:Label ID="modalFileViewLabelText" runat="server" Visible="false"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <a id="FileDownloadLinkviaModal" class="btn btn-warning btn-sm" runat="server">
                            <i class="fas fa-fw fa-cloud-download-alt"></i></i><span class="d-none d-lg-inline-block">Download</span>
                        </a>
                        <span class="text-info small" runat="server">
                            <i class="fas fa-fw fa-info-circle"></i>File Name:
                            <asp:Label ID="modalFileViewLabelFileName" runat="server"></asp:Label>. File Size: 
                            <asp:Label ID="modalFileViewLabelFileSize" runat="server"></asp:Label>
                        </span>

                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelFileView" DisplayAfter="0" DynamicLayout="false">
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
                $('[data-toggle="tooltip"]').tooltip({ html: true });
            });
        }
    </script>
</asp:Content>
