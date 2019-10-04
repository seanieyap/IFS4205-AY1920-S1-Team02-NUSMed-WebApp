<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="My-Profile.aspx.cs" Inherits="NUSMed_WebApp.My_Profile" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-id-card"></i>My Profile</h1>
            <p class="lead mb-1">View and Update your all your Information here !</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelMyProfile" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%-- Personal Details --%>
            <h2 class="mb-3 pb-2 border-bottom">Personal Details
                <button type="button" class="btn btn-sm btn-secondary float-right disabled" data-toggle="tooltip" title="May not be edited" runat="server"><i class="fas fa-fw fa-edit"></i>Edit</button>
            </h2>
            <div class="row text-left mb-3">
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>NRIC</label>
                        <input id="nric" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Date of Birth</label>
                        <input id="DateofBirth" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>First Name</label>
                        <input id="FirstName" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Last Name</label>
                        <input id="LastName" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Country of Birth</label>
                        <input id="CountryofBirth" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Nationality</label>
                        <input id="Nationality" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Sex</label>
                        <input id="Sex" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Gender</label>
                        <input id="Gender" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Marital Status</label>
                        <input id="MaritalStatus" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
            </div>

            <%-- Contact Details --%>
            <h2 class="mt-5 mb-3 pb-2 border-bottom">Contact Details
                <button id="buttonContactDetailsEdit" type="button" class="btn btn-sm btn-warning float-right" runat="server" onserverclick="buttonContactDetailsEdit_ServerClick"><i class="fas fa-fw fa-edit"></i>Edit</button>
            </h2>
            <div class="row text-left">
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Address</label>
                        <input id="Address" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Postal Code</label>
                        <input id="PostalCode" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Email Address</label>
                        <input id="EmailAddress" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Contact Number</label>
                        <input id="ContactNumber" type="text" class="form-control form-control-sm" readonly runat="server">
                    </div>
                </div>
            </div>

            <%-- Account Details --%>
            <h2 class="mt-5 mb-3 pb-2 border-bottom">Account Details
                <button type="button" class="btn btn-sm btn-secondary float-right disabled" data-toggle="tooltip" title="May not be edited" runat="server"><i class="fas fa-fw fa-edit"></i>Edit</button>
            </h2>
            <div class="row text-left">
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Last Full Login <span class="text-muted small">(Finished entire login process)</span></label>
                        <input id="LastFullLogin" type="text" class="form-control form-control-sm" runat="server" readonly>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Registered On</label>
                        <input id="Registration" type="text" class="form-control form-control-sm" runat="server" readonly>
                    </div>
                </div>
            </div>

            <%-- Patient Details --%>
            <h2 id="HeaderPatient" class="mt-5 mb-3 pb-2 border-bottom" runat="server" visible="false">Next of Kin Details
                <button id="buttonPatientDetailsEdit" type="button" class="btn btn-sm btn-warning float-right" runat="server" onserverclick="buttonPatientDetailsEdit_ServerClick"><i class="fas fa-fw fa-edit"></i>Edit</button>
            </h2>
            <div id="DivPatient" class="row text-left" runat="server" visible="false">
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Name of Next of Kin</label>
                        <input id="NOKName" type="text" class="form-control form-control-sm" runat="server" readonly>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Contact Number of Next of Kin</label>
                        <input id="NOKContact" type="text" class="form-control form-control-sm" runat="server" readonly>
                    </div>
                </div>
            </div>

            <%-- Therapist Details --%>
            <h2 id="HeaderTherapist" class="mt-5 mb-3 pb-2 border-bottom" runat="server" visible="false">Therapist Details
                <button type="button" class="btn btn-sm btn-secondary float-right disabled" data-toggle="tooltip" title="May not be edited" runat="server"><i class="fas fa-fw fa-edit"></i>Edit</button>
            </h2>
            <div id="DivTherapist" class="row text-left" runat="server" visible="false">
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Job Title</label>
                        <input id="TherapistJobTile" type="text" class="form-control form-control-sm" runat="server" readonly>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Department</label>
                        <input id="TherapistDepartment" type="text" class="form-control form-control-sm" runat="server" readonly>
                    </div>
                </div>
            </div>

            <%-- Researcher Details --%>
            <h2 id="HeaderResearcher" class="mt-5 mb-3 pb-2 border-bottom" runat="server" visible="false">Researcher Details
                <button type="button" class="btn btn-sm btn-secondary float-right disabled" data-toggle="tooltip" title="May not be edited" runat="server"><i class="fas fa-fw fa-edit"></i>Edit</button>
            </h2>
            <div id="DivResearcher" class="row text-left" runat="server" visible="false">
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Job Title</label>
                        <input id="ResearcherJobTitle" type="text" class="form-control form-control-sm" runat="server" readonly>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group form-group-sm">
                        <label>Department</label>
                        <input id="ResearcherDepartment" type="text" class="form-control form-control-sm" runat="server" readonly>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelMyProfile" DisplayAfter="200" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modalContactDetails" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelContactDetails" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-edit"></i>Edit Contact Details</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row text-left">
                            <div class="col-12 col-md-6">
                                <div class="form-group form-group-sm">
                                    <label>Address</label>
                                    <input id="AddressEdit" type="text" class="form-control form-control-sm" runat="server">
                                    <div class="invalid-feedback" runat="server">
                                        Address is invalid.
                                    </div>
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group form-group-sm">
                                    <label>Postal Code <span class="text-muted small">(6-digits)</span></label>
                                    <input id="PostalCodeEdit" type="text" class="form-control form-control-sm" runat="server">
                                    <div class="invalid-feedback" runat="server">
                                        Postal Code is invalid.
                                    </div>

                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group form-group-sm">
                                    <label>Email Address</label>
                                    <input id="EmailAddressEdit" type="text" class="form-control form-control-sm" runat="server">
                                    <div class="invalid-feedback" runat="server">
                                        Email Address is invalid.
                                    </div>

                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group form-group-sm">
                                    <label>Contact Number <span class="text-muted small">(8-digits)</span></label>
                                    <input id="ContactNumberEdit" type="text" class="form-control form-control-sm" runat="server">
                                    <div class="invalid-feedback" runat="server">
                                        Contact Number is invalid.
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer text-left">
                        <button id="buttonContactDetailsUpdate" type="button" class="btn btn-sm btn-success" runat="server" onserverclick="buttonContactDetailsUpdate_ServerClick"><i class="fas fa-fw fa-edit"></i>Update</button>
                        <span id="spanMessageContactDetailsUpdate" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>There are errors in the form.</span>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="buttonContactDetailsEdit" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelContactDetails" DisplayAfter="200" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalPatientDetails" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelPatientDetails" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-edit"></i>Edit Next of Kin Details</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row text-left">
                            <div class="col-12 col-md-6">
                                <div class="form-group form-group-sm">
                                    <label>Name of Next of Kin</label>
                                    <input id="NOKNameEdit" type="text" class="form-control form-control-sm" runat="server">
                                    <div class="invalid-feedback" runat="server">
                                        Name is invalid.
                                    </div>
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group form-group-sm">
                                    <label>Contact Number of Next of Kin <span class="text-muted small">(8-digits)</span></label>
                                    <input id="NOKContactEdit" type="text" class="form-control form-control-sm" runat="server">
                                    <div class="invalid-feedback" runat="server">
                                        Contact Number is invalid.
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer text-left">
                        <button type="button" class="btn btn-sm btn-success" runat="server" onserverclick="buttonPatientDetailsUpdate_ServerClick"><i class="fas fa-fw fa-edit"></i>Update</button>
                        <span id="spanMessagePatientDetailsUpdate" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>There are errors in the form.</span>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="buttonPatientDetailsEdit" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelPatientDetails" DisplayAfter="200" DynamicLayout="false">
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
                // Enable tooltips
                $(function () { $('[data-toggle="tooltip"]').tooltip() })
            });
        }
    </script>
</asp:Content>
