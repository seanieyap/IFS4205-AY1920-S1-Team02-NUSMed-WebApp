<%@ Page Title="New Request" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="New-Request.aspx.cs" Inherits="NUSMed_WebApp.Therapist.My_Patients.New_Request" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-plus-circle"></i>New Request</h1>
            <p class="lead">Search for your Patients in order to Request for Permissions!</p>
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
                <div class="col-12 offset-lg-3 col-lg-6">
                    <asp:GridView ID="GridViewPatient" CssClass="table table-sm" AllowPaging="true" PageSize="5" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary" EditRowStyle-CssClass="table-active"
                        ItemType="NUSMed_WebApp.Classes.Entity.Account" DataKeyNames="nric" OnRowCommand="GridViewPatient_RowCommand"
                        OnPageIndexChanging="GridViewPatient_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server" OnRowDataBound="GridViewPatient_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="NRIC" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.nric %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonRequest" runat="server">
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
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelAccounts" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modalSelectPermissions" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelSelectPermissions" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize">Select Permissions</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <h4 class="mb-2">Records Types
                                    <button type="button" class="btn btn-sm btn-info float-none float-md-right" runat="server" onclick=" $('.checkboxes .form-check-input input:checkbox').prop('checked', true);">Select All</button>
                                    <button type="button" class="btn btn-sm btn-info float-none float-md-right mr-2" runat="server" onclick=" $('.checkboxes .form-check-input input:checkbox').prop('checked', false);">Deselect All</button>
                                </h4>
                            </div>

                            <div class="col-12">
                                <div class="row ml-3 checkboxes">
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
                    <div class="modal-footer">
                        <button id="buttonRequest" type="button" class="btn btn-sm btn-success" runat="server" onserverclick="buttonRequest_ServerClick"><i class="fas fa-fw fa-share"></i>Submit Request</button>
                        <span id="spanMessageRequest" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>There are errors in the form.</span>
                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelSelectPermissions" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
