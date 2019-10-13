<%@ Page Title="Search Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search-Data.aspx.cs" Inherits="NUSMed_WebApp.Researcher.Search_Data" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Content/bootstrap-select.min.css" rel="stylesheet" type="text/css" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/Scripts/plotly-latest.min.js") %>
        <%: Scripts.Render("~/Scripts/bootstrap-select.min.js") %>
    </asp:PlaceHolder>

    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-search"></i>Search Data</h1>
            <p class="lead mb-0">Records here have been anonymized and may not fit your search criteria exactly</p>
            <p class="text-muted">Page is limited to 200 patients per search.</p>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-12 mb-3">
                <h2>Filter By Patient Data</h2>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-12 col-md-6">
                <div class="form-group row">
                    <label class="col-4" for="inputAgeLevel" runat="server">
                        Age 
                            <span id="labelTitleAge" class="text-info" tabindex="0" data-toggle="tooltip" runat="server"><i class="fas fa-fw fa-info-circle"></i></span>
                    </label>

                    <div class="col-8">
                        <asp:ListBox ID="inputAgeLevel" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group row">
                    <label class="col-4" for="inputSexLevel" runat="server">
                        Sex 
                            <span id="labelTitleSex" class="text-info" tabindex="0" data-toggle="tooltip" runat="server"><i class="fas fa-fw fa-info-circle"></i></span>
                    </label>

                    <div class="col-8">
                        <asp:ListBox ID="inputSexLevel" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group row">
                    <label class="col-4" for="inputGenderLevel" runat="server">
                        Gender 
                            <span id="labelTitleGender" class="text-info" tabindex="0" data-toggle="tooltip" runat="server"><i class="fas fa-fw fa-info-circle"></i></span>
                    </label>

                    <div class="col-8">
                        <asp:ListBox ID="inputGenderLevel" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group row">
                    <label class="col-4" for="inputPostal" runat="server">
                        Postal Code
                    </label>

                    <div class="col-8">
                        <asp:ListBox ID="inputPostal" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group row">
                    <label class="col-4" for="inputMaritalStatusLevel" runat="server">
                        Marital Status 
                            <span id="labelTitleMaritalStatus" class="text-info" tabindex="0" data-toggle="tooltip" runat="server"><i class="fas fa-fw fa-info-circle"></i></span>
                    </label>

                    <div class="col-8">
                        <asp:ListBox ID="inputMaritalStatusLevel" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group row">
                    <label class="col-4" for="inputDiagnosis" runat="server">
                        Diagnosis 
                    </label>

                    <div class="col-8">
                        <asp:ListBox ID="inputDiagnosis" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12 mb-3">
                <h2>Filter By Record Data</h2>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-12 col-md-6">
                <div class="form-group row">
                    <label class="col-4" for="inputRecordType" runat="server">
                        Type 
                    </label>

                    <div class="col-8">
                        <asp:ListBox ID="inputRecordType" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group row">
                    <label class="col-4" for="inputDiagnosis" runat="server">
                        Diagnosis 
                    </label>

                    <div class="col-8">
                        <asp:ListBox ID="inputRecordDiagnosis" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6">
                <div class="form-group row">
                    <label class="col-4" for="inputCreationDate" runat="server">
                        Creation Date
                    </label>

                    <div class="col-8">
                        <asp:ListBox ID="inputCreationDate" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanelPatientAnonymised" class="row" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="col-12 mb-4 text-center">
                    <asp:LinkButton ID="ButtonFilter" OnClick="ButtonFilter_ServerClick" CssClass="btn btn-success" runat="server"><i class="fas fa-fw fa-filter"></i>Filter</asp:LinkButton>
                </div>
                <div class="col-12">
                    <h2>View Anonymised Patients and their Records</h2>
                </div>
                <div class="col-12">
                    <asp:GridView ID="GridViewPatientAnonymised" CssClass="table table-sm table-responsive-sm small" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" ItemType="NUSMed_WebApp.Classes.Entity.PatientAnonymised"
                        OnRowCommand="GridViewPatientAnonymised_RowCommand" OnPageIndexChanging="GridViewPatientAnonymised_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table"
                        runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="Age">
                                <ItemTemplate>
                                    <%# Item.age %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sex">
                                <ItemTemplate>
                                    <%# Item.sex %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <%# Item.gender %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Martial Status">
                                <ItemTemplate>
                                    <%# Item.maritalStatus %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Postal">
                                <ItemTemplate>
                                    <%# Item.postal %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Diagnoses" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewDiagnosis" CssClass="btn btn-info btn-sm" CommandArgument="<%# Item.recordIDs %>" CommandName="ViewDiagnosis" runat="server"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Records" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewRecords" CssClass="btn btn-info btn-sm" CommandArgument="<%# Item.recordIDs %>" CommandName="ViewRecords" runat="server"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info text-center py-4" role="alert">
                                <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Filter returned no results.
                                </h4>
                                <p>Do try widening your filter parameter(s).</p>
                                <hr>
                                <p class="mb-0">New here? Try choosing some filter parameters and hit "Filter"!</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>

                </div>
                <div class="col-12 mb-3">
                    <h2>Download Data</h2>
                </div>
                <div class="col-12 text-center">
                    <asp:LinkButton ID="LinkButton1" CssClass="btn btn-success" runat="server"><i class="fas fa-fw fa-cloud-download-alt"></i>Download Patient Data</asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" CssClass="btn btn-success" runat="server"><i class="fas fa-fw fa-cloud-download-alt"></i>Download Record Data</asp:LinkButton>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonFilter" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelPatientAnonymised" DisplayAfter="200" DynamicLayout="false">
            <ProgressTemplate>
                <div class="loading">Loading</div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <div id="modalRecords" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelRecords" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title">View Records of Anonymised Patient</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <asp:GridView ID="GridViewRecords" CssClass="table table-sm table-responsive-sm small" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pagination-gridview"
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
                                        <asp:TemplateField HeaderText="Creation Time">
                                            <ItemTemplate>
                                                <%# Item.createTime.ToString() %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Diagnosis" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
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
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-info text-center py-4" role="alert">
                                            <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Patient has no Records.</h4>
                                            <p>Therapists probably did not add any records for this patient.</p>
                                            <hr>
                                            <p class="mb-0">If this is a mistake, please report to the help-desk.</p>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelRecords" DisplayAfter="200" DynamicLayout="false">
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

    <div id="modalDiagnosisView" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelDiagnosisView" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-eye"></i>
                            View Diagnoses of Anonymized Patient</h5>
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
                                            AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" ItemType="NUSMed_WebApp.Classes.Entity.PatientDiagnosis"
                                            OnPageIndexChanging="GridViewPatientDiagnoses_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server">
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

                                                <asp:TemplateField HeaderText="Start">
                                                    <ItemTemplate>
                                                        <%# Item.start %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="alert alert-info text-center py-4" role="alert">
                                                    <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Patient does not have any Diagnosis attributed to him/her.
                                                    </h4>
                                                    <p>No therapists had probably attributed any diagnoses to the patient.</p>
                                                    <hr>
                                                    <p class="mb-0">If this is a mistake, please report to the help-desk.</p>
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

    <div id="modalRecordDiagnosisView" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
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
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="alert alert-info text-center py-4" role="alert">
                                                    <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Record does not have any Diagnosis attributed to it.
                                                    </h4>
                                                    <p>No therapists had probably attributed any diagnoses to the record.</p>
                                                    <hr>
                                                    <p class="mb-0">If this is a mistake, please report to the help-desk.</p>
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $(function () {
                // Enable Tooltips
                $('[data-toggle="tooltip"]').tooltip({ html: true });

                // Enable Multiple Select
                $.fn.selectpicker.Constructor.BootstrapVersion = '4';
                $('.selectpicker').selectpicker();
            });
        }
    </script>
</asp:Content>
