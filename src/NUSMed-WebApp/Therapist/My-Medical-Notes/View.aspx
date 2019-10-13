<%@ Page Title="View Medical Note" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="NUSMed_WebApp.Therapist.My_Medical_Notes.View" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/Scripts/plotly-latest.min.js") %>
    </asp:PlaceHolder>

    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-notes-medical"></i>View My Medical Notes</h1>
            <p class="lead">View Medical Notes both sent from others or created by yourself.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelMedicalNote" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row mb-4">
                <div class="col-12 col-sm-8 col-md-6 col-lg-5 col-xl-4 mx-auto">
                    <asp:Panel CssClass="input-group input-group-sm" runat="server" DefaultButton="ButtonSearch">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Search</span>
                        </div>
                        <asp:TextBox ID="TextboxSearch" CssClass="form-control form-control-sm" placeholder="Title" runat="server"></asp:TextBox>
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
                    <asp:GridView ID="GridViewMedicalNote" CssClass="table table-sm" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" ItemType="NUSMed_WebApp.Classes.Entity.Note" DataKeyNames="id"
                        OnRowCommand="GridViewMedicalNote_RowCommand" OnRowDataBound="GridViewMedicalNote_RowDataBound"
                        OnPageIndexChanging="GridViewMedicalNote_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="Title" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.title %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject NRIC" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.patient.nric %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject Name" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelPatientName" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created By" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.therapist.lastName + " " + Item.therapist.firstName %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Creation Time" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.createTime %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton CssClass="btn btn-success btn-sm" runat="server" CommandName="ViewNote" CommandArgument='<%# Item.id %>'>
                                        <i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span>
                                    </asp:LinkButton>
                                    <asp:LinkButton CssClass="btn btn-warning btn-sm" runat="server" CommandName="ViewSendNoteModal" CommandArgument='<%# Item.id %>'>
                                        <i class="fas fa-fw fa-share-square"></i><span class="d-none d-lg-inline-block">Send</span>
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
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelMedicalNote" DisplayAfter="200" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modalNote" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelNote" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title">View Note</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">                       
                        <asp:Panel ID="modalNoteAccordion" CssClass="accordion" ClientIDMode="Static" runat="server">
                            <asp:Panel CssClass="card" runat="server" ClientIDMode="Static">
                                <div class="card-header" id="NoteInformationHeader">
                                    <h2 class="mb-0">
                                        <button class="btn btn-link" type="button" data-toggle="collapse" data-target="#NoteInformation" aria-expanded="false" aria-controls="NoteInformation">
                                            Note Information
                                        </button>
                                    </h2>
                                </div>

                                <div id="NoteInformation" class="collapse" aria-labelledby="NoteInformationHeader" data-parent="#modalNoteAccordion" >
                                    <div class="card-body">
                                        <div class="row text-left mb-3">
                                            <div class="col-12">
                                                <div class="form-group">
                                                    <label>Title</label>
                                                    <input id="inputTitle" type="text" class="form-control form-control-sm" readonly runat="server" disabled="disabled">
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                <div class="form-group">
                                                    <label>Content</label>
                                                    <asp:TextBox ID="TextBoxContent" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="10" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-12 col-md-6">
                                                <div class="form-group">
                                                    <label>Created By</label>
                                                    <input id="inputCreateBy" type="text" class="form-control form-control-sm" readonly runat="server" disabled="disabled">
                                                </div>
                                            </div>
                                            <div class="col-12 col-md-6">
                                                <div class="form-group">
                                                    <label>Create Time</label>
                                                    <input id="inputCreateTime" type="text" class="form-control form-control-sm" readonly runat="server" disabled="disabled">
                                                </div>
                                            </div>
                                            <div class="col-12 col-md-6">
                                                <div class="form-group">
                                                    <label>Subject / Patient NRIC</label>
                                                    <input id="inputPatientNRIC" type="text" class="form-control form-control-sm" readonly runat="server" disabled="disabled">
                                                </div>
                                            </div>
                                            <div class="col-12 col-md-6">
                                                <div class="form-group">
                                                    <label>Subject / Patient Name</label>
                                                    <input id="inputPatientName" type="text" class="form-control form-control-sm" readonly runat="server" disabled="disabled">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PanelPatientPersonalInformation" CssClass="card" runat="server" ClientIDMode="Static">
                                <div class="card-header" id="PatientPersonalInformationHeader">
                                    <h2 class="mb-0">
                                        <button class="btn btn-link" type="button" data-toggle="collapse" data-target="#PatientPersonalInformation" aria-expanded="false" aria-controls="PatientPersonalInformation">
                                            Subject / Patient Personal Information
                                        </button>
                                    </h2>
                                </div>

                                <div id="PatientPersonalInformation" class="collapse" aria-labelledby="PatientPersonalInformationHeader" data-parent="#modalNoteAccordion">
                                    <div class="card-body">
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
                                        <div class="row text-left" runat="server">
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
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PanelPatientDiagnosis" CssClass="card" runat="server" ClientIDMode="Static">
                                <div class="card-header" id="PatientDiagnosisHeader">
                                    <h2 class="mb-0">
                                        <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#PatientDiagnosis" aria-expanded="false" aria-controls="PatientDiagnosis">
                                            Subject / Patient Attributed Diagnoses
                                        </button>
                                    </h2>
                                </div>
                                <div id="PatientDiagnosis" class="collapse" aria-labelledby="PatientDiagnosisHeader" data-parent="#modalNoteAccordion">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-12">
                                                <asp:GridView ID="GridViewPatientDiagnoses" CssClass="table table-sm small" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                                                    AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None"
                                                    ItemType="NUSMed_WebApp.Classes.Entity.PatientDiagnosis" OnPageIndexChanging="GridViewPatientDiagnoses_PageIndexChanging" OnRowDataBound="GridViewPatientDiagnoses_RowDataBound"
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
                                                                <asp:Label ID="LabelPatientDiagnosesEnd" runat="server"></asp:Label>
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
                            </asp:Panel>
                            <asp:Panel ID="PanelNoteRecords" CssClass="card" runat="server" ClientIDMode="Static">
                                <div class="card-header" id="NoteRecordsHeader">
                                    <h2 class="mb-0">
                                        <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#NoteRecords" aria-expanded="false" aria-controls="NoteRecords">
                                            Attached Records
                                        </button>
                                    </h2>
                                </div>
                                <div id="NoteRecords" class="collapse" aria-labelledby="NoteRecordsHeader" data-parent="#modalNoteAccordion">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-12">
                                                <asp:GridView ID="GridViewRecords" CssClass="table table-sm table-responsive-sm small" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
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

                                                                <a id="FileDownloadLink" class="btn btn-warning btn-sm" runat="server" visible="false">
                                                                    <i class="fas fa-fw fa-cloud-download-alt"></i>
                                                                </a>
                                                                <asp:Label ID="LabelRecordPermissionStatusContent" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>

                                                    <EmptyDataTemplate>
                                                        <div class="alert alert-info text-center py-4" role="alert">
                                                            <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Patient has no Records.
                                                            </h4>
                                                            <p>Perhaps add a record for this patient.</p>
                                                            <hr>
                                                            <p class="mb-0">Use the button at the bottom to visit the Submit New Records page.</p>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </asp:Panel>

                        <asp:Panel ID="PanelNoteUnauthorized" CssClass="row" runat="server" Visible="false">
                            <div class="col-12">
                                <div class="alert alert-danger text-center py-4" role="alert">
                                    <h4 class="alert-heading"><i class="fas fa-fw fa-times-circle mr-2"></i>Unauthorized.
                                    </h4>
                                    <p>
                                        You do not have permissions granted by the Subject / Patient of this Medical Note.<br />
                                        You are unable to view patient information, patient diagnoses and Records attached to this note.
                                    </p>
                                    <hr>
                                    <p class="mb-0">To gain access, you required approved permissions from the Subject / Patient.</p>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelNote" DisplayAfter="200" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalFileView" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelFileView" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-eye"></i>
                            View Record File:
                            <asp:Label ID="labelRecordName" runat="server"></asp:Label></h5>
                        <button id="buttonCloseModalFileViewTop" type="button" class="close" runat="server" onserverclick="CloseModalFileView_ServerClick">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body text-center">
                        <asp:Image ID="modalFileViewImage" CssClass="img-fluid" runat="server" Visible="false" />
                        <video id="modalFileViewVideo" style="width: 100%; height: auto;" controls runat="server" visible="false">
                            <source id="modalFileViewVideoSource" src="/" type="video/mp4" runat="server">
                            Your browser does not support the video tag.
                        </video>
                        <asp:Panel ID="modalFileViewPanelText" runat="server" Visible="false" ClientIDMode="Static"></asp:Panel>
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

                        <button id="buttonCloseModalFileViewBottom" type="button" class="btn btn-secondary ml-auto" runat="server" onserverclick="CloseModalFileView_ServerClick">Close</button>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="buttonCloseModalFileViewTop" />
                    <asp:AsyncPostBackTrigger ControlID="buttonCloseModalFileViewBottom" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelFileView" DisplayAfter="200" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalRecordDiagnosisView" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelRecordDiagnosisView" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-eye"></i>
                            View Record Diagnosis:
                            <asp:Label ID="labelRecordNameDiagnosis" runat="server"></asp:Label></h5>
                        <button id="buttonCloseModalRecordDiagnosisViewTop" type="button" class="close" runat="server" onserverclick="CloseModalRecordDiagnosisView_ServerClick">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <h4>Instructions</h4>
                                <ul>
                                    <li>Records can be attributed a diagnosis for both research and informational purposes.</li>
                                    <li>Diagnoses cannot be edited nor deleted once added.</li>
                                </ul>
                            </div>
                        </div>
                        <hr />
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
                        <button id="buttonCloseModalRecordDiagnosisViewBottom" type="button" class="btn btn-secondary ml-auto" runat="server" onserverclick="CloseModalRecordDiagnosisView_ServerClick">Close</button>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="buttonCloseModalRecordDiagnosisViewTop" />
                    <asp:AsyncPostBackTrigger ControlID="buttonCloseModalRecordDiagnosisViewBottom" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelRecordDiagnosisView" DisplayAfter="200" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div id="modalSendNote" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelSendNote" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-eye"></i>
                            Send Medical Note</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <h4>List of Therapists</h4>
                                <div class="row mb-4">
                                    <div class="col-12 col-sm-8 col-md-6 col-lg-5 col-xl-4 mx-auto">
                                        <asp:Panel CssClass="input-group input-group-sm" runat="server" DefaultButton="ButtonSearchSendNote">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">Search</span>
                                            </div>
                                            <asp:TextBox ID="TextBoxSearchTherapist" CssClass="form-control form-control-sm" placeholder="First Name / Last Name" runat="server"></asp:TextBox>
                                            <div class="input-group-append">
                                                <asp:LinkButton ID="ButtonSearchSendNote" CssClass="btn btn-outline-info" OnClick="ButtonSearchSendNote_Click" runat="server">
                                                    <i class="fas fa-fw fa-search"></i> Go
                                                </asp:LinkButton>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12">
                                        <asp:GridView ID="GridViewTherapistSendNote" CssClass="table table-sm" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                                            AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" ItemType="NUSMed_WebApp.Classes.Entity.Therapist"
                                            DataKeyNames="nric" OnRowCommand="GridViewTherapistSendNote_RowCommand" OnPageIndexChanging="GridViewTherapistSendNote_PageIndexChanging"
                                            EmptyDataRowStyle-CssClass="empty-table" runat="server" OnRowDataBound="GridViewTherapistSendNote_RowDataBound">
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
                                                        <asp:LinkButton ID="LinkButtonViewSelectTherapist" runat="server">
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
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="buttonSendNote" type="button" class="btn btn-sm btn-success" runat="server" onserverclick="buttonSendNote_ServerClick"><i class="fas fa-fw fa-share"></i>Send</button>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelSendNote" DisplayAfter="200" DynamicLayout="false">
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
            });
        }
    </script>
</asp:Content>
