﻿<%@ Page Title="New Record" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="New-Record.aspx.cs" Inherits="NUSMed_WebApp.Patient.My_Records.New_Record" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-file-medical"></i>New Record</h1>
            <p class="lead">Create a New Record for yourself!</p>
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
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" />
                        <label class="form-check-label" for="<%= RadioButtonTypeHeightMeasurement.ClientID %>">Height Measurement</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeWeightMeasurement" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" />
                        <label class="form-check-label" for="<%= RadioButtonTypeWeightMeasurement.ClientID %>">Weight Measurement</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeTemperatureReading" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" />
                        <label class="form-check-label" for="<%= RadioButtonTypeTemperatureReading.ClientID %>">Temperature Reading</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeBloodPressureReading" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" />
                        <label class="form-check-label" for="<%= RadioButtonTypeBloodPressureReading.ClientID %>">Blood Pressure Reading</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeECGReading" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" />
                        <label class="form-check-label" for="<%= RadioButtonTypeECGReading.ClientID %>">ECG Reading</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeMRI" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" />
                        <label class="form-check-label" for="<%= RadioButtonTypeMRI.ClientID %>">MRI</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeXRay" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" />
                        <label class="form-check-label" for="<%= RadioButtonTypeXRay.ClientID %>">X-ray</label>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="form-check form-check-inline">
                        <asp:RadioButton ID="RadioButtonTypeGait" CssClass="form-check-input" runat="server" GroupName="recordType"
                            ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="RadioButtonType_CheckedChanged" />
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
                        <asp:TextBox ID="inputMethodContent" CssClass="form-control" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                        <asp:Label ID="LabelMethodContent" CssClass="small text-muted" for="<%= inputMethodContent.ClientID %>" runat="server" Text="Note: something."></asp:Label>
                    </div>
                </asp:Panel>
                <asp:Panel ID="PanelFile" class="col-12" runat="server" Visible="false">
                    <div class="form-group mt-2 mb-0">
                        <div class="custom-file">
                            <asp:FileUpload ID="inputMethodFile" CssClass="custom-file-input" runat="server" accept=".txt, .csv" />
                            <label class="custom-file-label" for="<%= inputMethodFile.ClientID %>">Choose File</label>
                            <asp:Label ID="LabelMethodFile" CssClass="small text-muted" runat="server" Text="Note: something."></asp:Label>
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
                        <label for="inputTitle">Title</label>
                        <input id="inputTitle" type="text" class="form-control" placeholder="Title" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Title is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group">
                        <label for="inputDescription">Description</label>
                        <input id="inputDescription" type="text" class="form-control" placeholder="Description" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Description is invalid.
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12 text-left">
                    <button type="submit" id="buttonSubmit" class="btn btn-success mr-auto ml-auto" runat="server" onserverclick="buttonSubmit_ServerClick">Register</button>
                    <span id="spanMessage" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>There are errors in the form.</span>
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
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelNewRecord" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modelSuccess" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
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
                    <button type="submit" class="btn btn-success mr-auto" runat="server" onserverclick="buttonRefresh_ServerClick">Submit New Record</button>
                    <a class="btn btn-secondary" href="~/Patient/Dashboard" role="button" runat="server">Return to Dashboard</a>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
