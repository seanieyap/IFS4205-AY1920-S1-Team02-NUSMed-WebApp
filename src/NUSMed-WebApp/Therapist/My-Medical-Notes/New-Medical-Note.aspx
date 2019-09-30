<%@ Page Title="New Medical Note" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="New-Medical-Note.aspx.cs" Inherits="NUSMed_WebApp.Therapist.My_Medical_Notes.New_Medical_Note" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-notes-medical"></i>New Medical Note</h1>
            <p class="lead">Create a New Medical Note using the form below.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelNewMedicalNote" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-12">
                    <h2>Basic Information</h2>
                </div>
            </div>

            <div class="row pb-3">
                <div class="col-12">
                    <div class="form-group">
                        <label for="inputTitle">Title <span class="text-muted small">(Maximum of 45 characters)</span></label>
                        <input id="inputTitle" type="text" class="form-control form-control-sm" placeholder="Title" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Title is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group">
                        <label for="inputDescription">Description <span class="text-muted small">(Maximum of 120 characters)</span></label>
                        <input id="inputDescription" type="text" value="nil" class="form-control form-control-sm" placeholder="Description" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Description is invalid.
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <h2>Select Subject / Patient</h2>
                </div>
            </div>

            <div class="row mb-4">

                <div class="col-12 col-sm-8 col-md-6 col-lg-5 col-xl-4 mx-auto">
                    <asp:Panel CssClass="input-group input-group-sm" runat="server" DefaultButton="ButtonSearch">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Search</span>
                        </div>
                        <asp:TextBox ID="TextboxSearch" CssClass="form-control form-control-sm" placeholder="NRIC" runat="server"></asp:TextBox>
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
                    <asp:GridView ID="GridViewPatient" CssClass="table table-sm mb-0" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" ItemType="NUSMed_WebApp.Classes.Entity.Patient" DataKeyNames="nric"
                        OnRowCommand="GridViewPatient_RowCommand" OnPageIndexChanging="GridViewPatient_PageIndexChanging" OnRowDataBound="GridViewPatient_RowDataBound"
                        EmptyDataRowStyle-CssClass="empty-table" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="NRIC" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.nric %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelName" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Information" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewInformation" runat="server"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Diagnoses" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewDiagnosis" runat="server"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User Action" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewSelectPatient" runat="server">
                                        <i class="fas fa-fw fa-hand-pointer"></i><span class="d-none d-lg-inline-block">Select</span>
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
                <div class="col-12">
                    <span id="spanGridViewPatientMessage" class="small text-danger d-block d-sm-inline-block mb-3" runat="server" visible="false">Please select a Subject / Patient.
                    </span>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <h2>Choose Records</h2>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12 text-left">
                    <asp:GridView ID="GridViewRecords" CssClass="table table-sm table-responsive-md small" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None"
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

                            <asp:TemplateField HeaderText="Diagnosis" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelRecordPermissionStatusDiagnosis" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:LinkButton ID="LinkButtonRecordDiagnosisView" CssClass="btn btn-success btn-sm" runat="server" Visible="false">
                                        <i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Data" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelContent" runat="server" Visible="false"></asp:Label>
                                    <asp:LinkButton ID="LinkbuttonFileView" CssClass="btn btn-success btn-sm" runat="server" Visible="false"></asp:LinkButton>
                                    <asp:Label ID="LabelRecordPermissionStatusContent" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="User Action" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewSelectRecord" runat="server">
                                        <i class="fas fa-fw fa-hand-pointer"></i><span class="d-none d-lg-inline-block">Select</span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info text-center py-4" role="alert">
                                <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>No Patient has been selected or Patient has no Records.
                                </h4>
                                <p>Perhaps select a different patient or add a record for this patient.</p>
                                <hr>
                                <p class="mb-0">If this is a mistake, please contact the help-desk for assistance.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12 text-left">
                    <button type="submit" id="buttonSubmit" class="btn btn-success mr-auto ml-auto" runat="server" onserverclick="buttonSubmit_ServerClick">Submit</button>
                    <span id="spanMessage" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false">
                        <i class="fas fa-exclamation-circle fa-fw"></i>There are errors in the form.
                    </span>
                </div>
            </div>

            <div id="modelSuccess" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-dialog-centered text-center" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title mx-auto">Medical Note have been created Successfully.</h5>
                        </div>
                        <div class="modal-body text-success">
                            <p class="mt-2"><i class="fas fa-check-circle fa-8x"></i></p>
                            <p class="display-3">success</p>
                        </div>
                        <div class="modal-footer">
                            <a class="btn btn-secondary mr-auto" href="~/Therapist/My-Medical-Notes/View" role="button" runat="server">View your Medical Notes</a>
                            <button id="buttonSuccessCreateAnother" type="button" class="btn btn-secondary" data-dismiss="modal" runat="server" onserverclick="buttonSuccessCreateAnother_ServerClick">Create Another</button>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelNewMedicalNote" DisplayAfter="200" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

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
                                    <input id="inputNRIC" type="text" class="form-control form-control-sm" readonly runat="server" disabled="disabled">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Date of Birth</label>
                                    <input id="DateofBirth" type="text" class="form-control form-control-sm" readonly runat="server" disabled="disabled">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>First Name</label>
                                    <input id="FirstName" type="text" class="form-control form-control-sm" readonly runat="server" disabled="disabled">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Last Name</label>
                                    <input id="LastName" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Country of Birth</label>
                                    <input id="CountryofBirth" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Nationality</label>
                                    <input id="Nationality" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Sex</label>
                                    <input id="Sex" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Gender</label>
                                    <input id="Gender" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Marital Status</label>
                                    <input id="MaritalStatus" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                        </div>

                        <%-- Contact Details --%>
                        <h4 class="mb-3 border-bottom">Contact Details</h4>
                        <div class="row text-left mb-3">
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Address</label>
                                    <input id="Address" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Postal Code</label>
                                    <input id="PostalCode" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Email Address</label>
                                    <input id="EmailAddress" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Contact Number</label>
                                    <input id="ContactNumber" type="text" class="form-control form-control-sm" readonly runat="server">
                                </div>
                            </div>
                        </div>

                        <%-- Patient Details --%>
                        <h4 class="mb-3 border-bottom">Next of Kin Details</h4>
                        <div class="row text-left mb-3" runat="server">
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Name of Next of Kin</label>
                                    <input id="NOKName" type="text" class="form-control form-control-sm" runat="server" readonly>
                                </div>
                            </div>
                            <div class="col-12 col-md-6">
                                <div class="form-group">
                                    <label>Contact Number of Next of Kin</label>
                                    <input id="NOKContact" type="text" class="form-control form-control-sm" runat="server" readonly>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelInformation" DisplayAfter="200" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalFileView" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
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
                        <asp:Image ID="modalFileViewImage" CssClass="img-fluid" runat="server" Visible="false" />
                        <video id="modalFileViewVideo" style="width: 100%; height: auto;" controls runat="server" visible="false">
                            <source id="modalFileViewVideoSource" src="/" type="video/mp4" runat="server">
                            Your browser does not support the video tag.
                        </video>
                        <asp:Label ID="modalFileViewLabelText" runat="server" Visible="false"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <span class="text-info small" runat="server">
                            <i class="fas fa-fw fa-info-circle"></i>File Name:
                            <asp:Label ID="modalFileViewLabelFileName" runat="server"></asp:Label>. File Size: 
                            <asp:Label ID="modalFileViewLabelFileSize" runat="server"></asp:Label>
                        </span>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelFileView" DisplayAfter="200" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalDiagnosisView" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelDiagnosisView" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-eye"></i>
                            View Diagnoses:
                            <asp:Label ID="labelDiagnosisName" runat="server"></asp:Label></h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <h4>Attributed Diagnoses</h4>
                                <div class="row">
                                    <div class="col-12">
                                        <asp:GridView ID="GridViewPatientDiagnoses" CssClass="table table-sm small" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                                            AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" 
                                            ItemType="NUSMed_WebApp.Classes.Entity.PatientDiagnosis" OnPageIndexChanging="GridViewPatientDiagnoses_PageIndexChanging"
                                            EmptyDataRowStyle-CssClass="empty-table" runat="server">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Code">
                                                    <ItemTemplate>
                                                        <%# Item.diagnosis.code %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <%# Item.diagnosis.descriptionShort %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Category">
                                                    <ItemTemplate>
                                                        <%# Item.diagnosis.categoryTitle %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Assigned By">
                                                    <ItemTemplate>
                                                        <%# Item.therapist.lastName + " " + Item.therapist.firstName %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Start">
                                                    <ItemTemplate>
                                                        <%# Item.start %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="End">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelPatientDiagnosesEnd" runat="server" Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="LinkButtonPatientDiagnosesEnd" CssClass="btn btn-sm btn-success" data-toggle="confirmation" data-title="This is Permanent. Confirm?"
                                                            CommandName="UpdateEndPatientDiagnosis" CommandArgument='<%# Item.diagnosis.code %>' runat="server" Visible="false">
                                                        <i class="fas fa-fw fa-check-circle"></i> Declare End 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="alert alert-info text-center py-4" role="alert">
                                                    <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Patient does not have any Diagnosis attributed to him/her.
                                                    </h4>
                                                    <p>No therapists had probably attributed any diagnoses to the patient.</p>
                                                    <hr>
                                                    <p class="mb-0">If this is a mistake, please contact the help-desk for assistance.</p>
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelDiagnosisView" DisplayAfter="200" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalRecordDiagnosisView" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelRecordDiagnosisView" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-eye"></i>
                            View Record Diagnosis:
                            <asp:Label ID="labelRecordNameDiagnosis" runat="server"></asp:Label></h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <h4>Attributed Diagnosis</h4>
                                <div class="row">
                                    <div class="col-12">
                                        <asp:GridView ID="GridViewRecordDiagnoses" CssClass="table table-sm small" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                                            AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" 
                                            ItemType="NUSMed_WebApp.Classes.Entity.RecordDiagnosis" OnPageIndexChanging="GridViewRecordDiagnoses_PageIndexChanging"
                                            EmptyDataRowStyle-CssClass="empty-table" runat="server">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Code">
                                                    <ItemTemplate>
                                                        <%# Item.diagnosis.code %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <%# Item.diagnosis.descriptionShort %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Category">
                                                    <ItemTemplate>
                                                        <%# Item.diagnosis.categoryTitle %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Attributed By">
                                                    <ItemTemplate>
                                                        <%# Item.therapist.lastName + " " + Item.therapist.firstName %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="alert alert-info text-center py-4" role="alert">
                                                    <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Record does not have any Diagnosis attributed to it.
                                                    </h4>
                                                    <p>No therapists had probably attributed any diagnoses to the record.</p>
                                                    <hr>
                                                    <p class="mb-0">If this is a mistake, please contact the help-desk for assistance.</p>
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelRecordDiagnosisView" DisplayAfter="200" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
