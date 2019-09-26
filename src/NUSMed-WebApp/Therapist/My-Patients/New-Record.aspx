<%@ Page Title="New Record" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="New-Record.aspx.cs" Inherits="NUSMed_WebApp.Therapist.My_Records.New_Record" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/Scripts/bs-custom-file-input.min.js") %>
    </asp:PlaceHolder>

    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-file-medical"></i>New Record</h1>
            <p class="lead">Create a New Record for your Patient!
                <br />Selected Patient: <asp:Label ID="LabelPatientNRIC" runat="server"></asp:Label>
            </p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelNewRecord" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="py-3">
                <h2>Type</h2>
            </div>
            <div class="row pb-3">
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeHeightMeasurement" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" Enabled="false" />
                        <label class="form-check-label" for="<%= RadioButtonTypeHeightMeasurement.ClientID %>">Height Measurement</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeWeightMeasurement" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" Enabled="false" />
                        <label class="form-check-label" for="<%= RadioButtonTypeWeightMeasurement.ClientID %>">Weight Measurement</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeTemperatureReading" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" Enabled="false" />
                        <label class="form-check-label" for="<%= RadioButtonTypeTemperatureReading.ClientID %>">Temperature Reading</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeBloodPressureReading" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" Enabled="false" />
                        <label class="form-check-label" for="<%= RadioButtonTypeBloodPressureReading.ClientID %>">Blood Pressure Reading</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeECGReading" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" Enabled="false" />
                        <label class="form-check-label" for="<%= RadioButtonTypeECGReading.ClientID %>">ECG Reading</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeMRI" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" Enabled="false" />
                        <label class="form-check-label" for="<%= RadioButtonTypeMRI.ClientID %>">MRI</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeXRay" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" Enabled="false" />
                        <label class="form-check-label" for="<%= RadioButtonTypeXRay.ClientID %>">X-ray</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeGait" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" Enabled="false" />
                        <label class="form-check-label" for="<%= RadioButtonTypeGait.ClientID %>">Gait</label>
                    </div>
                </div>
            </div>

            <div class="py-3">
                <h2>Content</h2>
            </div>
            <div class="row pb-3">
                <asp:Panel ID="PanelContent" class="col-12" runat="server">
                    <div class="form-group mt-2 mb-0">
                        <label for="inputContent">
                            <asp:Label ID="LabelContent" runat="server" Text="Height Measurement"></asp:Label>
                            <asp:Label ID="LabelContentHelper" class="text-muted small" runat="server" Text="(Format: Centimetre, cm. Values: 0 - 280)"></asp:Label></label>
                        <input id="inputContent" type="text" class="form-control" runat="server" placeholder="cm">
                        <div class="invalid-feedback" runat="server">
                            <i class="fas fa-fw fa-exclamation-circle"></i>Content is Invalid.
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="PanelFile" class="col-12" runat="server" Visible="false">
                    <div class="form-group mt-2 mb-0">
                        <label for="inputFile">
                            <asp:Label ID="LabelFile" runat="server" Text="Height Measurement"></asp:Label>
                            <asp:Label ID="LabelFileHelper" class="text-muted small" runat="server"></asp:Label></label>
                        <div class="custom-file">
                            <asp:FileUpload ID="inputFile" CssClass="custom-file-input" runat="server" />
                            <label class="custom-file-label" for="<%= inputFile.ClientID %>">Choose File...</label>
                            <asp:Label ID="LabelFileError" CssClass="small text-danger" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="py-3">
                <h2>Information</h2>
            </div>
            <div class="row pb-3">
                <div class="col-12">
                    <div class="form-group">
                        <label for="inputTitle">Title <span class="text-muted small">(Maximum of 45 characters)</span></label>
                        <input id="inputTitle" type="text" class="form-control" placeholder="Title" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Title is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group">
                        <label for="inputDescription">Description <span class="text-muted small">(Maximum of 120 characters)</span></label>
                        <input id="inputDescription" type="text" value="nil" class="form-control" placeholder="Description" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Description is invalid.
                        </div>
                    </div>
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="RadioButtonTypeHeightMeasurement" />
            <asp:AsyncPostBackTrigger ControlID="RadioButtonTypeWeightMeasurement" />
            <asp:AsyncPostBackTrigger ControlID="RadioButtonTypeTemperatureReading" />
            <asp:AsyncPostBackTrigger ControlID="RadioButtonTypeBloodPressureReading" />
            <asp:AsyncPostBackTrigger ControlID="RadioButtonTypeECGReading" />
            <asp:AsyncPostBackTrigger ControlID="RadioButtonTypeMRI" />
            <asp:AsyncPostBackTrigger ControlID="RadioButtonTypeXRay" />
            <asp:AsyncPostBackTrigger ControlID="RadioButtonTypeGait" />
            <asp:PostBackTrigger ControlID="buttonSubmit" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelNewRecord" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modelSuccess" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered text-center" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mx-auto">Record has been Submitted Successfully.</h5>
                </div>
                <div class="modal-body text-success">
                    <p class="mt-2"><i class="fas fa-check-circle fa-8x"></i></p>
                    <p class="display-3">success</p>
                </div>
                <div class="modal-footer">
                    <a class="btn btn-secondary mr-auto" href="~/Therapist/My-Patients/View" role="button" runat="server">View your Patients</a>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Continue Adding Records</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(function () {
                // init file input
                bsCustomFileInput.init();
            });
        }
    </script>
</asp:Content>
