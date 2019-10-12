<%@ Page Title="Search Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search-Data.aspx.cs" Inherits="NUSMed_WebApp.Researcher.Search_Data" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Content/bootstrap-select.min.css" rel="stylesheet" type="text/css" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/Scripts/bootstrap-select.min.js") %>
    </asp:PlaceHolder>

    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-search"></i>Search Data</h1>
            <p class="lead">Records here have been anonymized and may not fit your search criteria exactly</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelPatientAnonymised" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-12 mb-3">
                    <h2>Filter By Patient Data</h2>
                </div>
            </div>

            <div class="row mb-4">
                <div class="col-6">
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
                <div class="col-6">
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
                <div class="col-6">
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
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-4" for="inputPostal" runat="server">
                            Postal Code
                        </label>

                        <div class="col-8">
                            <asp:ListBox ID="inputPostal" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                        </div>
                    </div>
                </div>
                <div class="col-6">
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
            </div>

            <div class="row">
                <div class="col-12 mb-3">
                    <h2>Filter By Record Data</h2>
                </div>
            </div>

            <div class="row mb-4">
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-4" for="inputRecordType" runat="server">
                            Record Type 
                        </label>

                        <div class="col-8">
                            <asp:ListBox ID="inputRecordType" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                        </div>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-4" for="inputDiagnosis" runat="server">
                            Diagnosis 
                        </label>

                        <div class="col-8">
                            <asp:ListBox ID="inputDiagnosis" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                        </div>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-4" for="inputCreationDate" runat="server">
                            Record Creation Date
                            <span id="labelTitleCreationDate" class="text-info" tabindex="0" data-toggle="tooltip" runat="server"><i class="fas fa-fw fa-info-circle"></i></span>
                        </label>

                        <div class="col-8">
                            <asp:ListBox ID="inputCreationDate" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12 mb-4 text-center">
                    <button type="button" id="buttonFilter" onserverclick="buttonFilter_ServerClick" class="btn btn-success" runat="server">Filter Patients and Records</button>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <h2>Anonymised Patients and their Records</h2>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <asp:GridView ID="GridViewPatientAnonymised" CssClass="table table-sm small" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" ItemType="NUSMed_WebApp.Classes.Entity.PatientAnonymised"
                        OnRowCommand="GridViewPatientAnonymised_RowCommand" OnPageIndexChanging="GridViewPatientAnonymised_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table"
                        runat="server" OnRowDataBound="GridViewPatientAnonymised_RowDataBound">
                        <Columns>
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
                            <asp:TemplateField HeaderText="Records" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewRecords" CssClass="btn btn-info btn-sm" runat="server">
                                        <i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span>
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
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelPatientAnonymised" DisplayAfter="200" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>
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
