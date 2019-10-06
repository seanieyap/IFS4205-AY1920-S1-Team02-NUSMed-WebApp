<%@ Page Title="Search Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search-Data.aspx.cs" Inherits="NUSMed_WebApp.Researcher.Search_Data" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Content/bootstrap-select.min.css" rel="stylesheet" type="text/css" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">

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
                        <label id="labelInputAge" class="col-3" runat="server">
                            Age 
                            <span id="labelTitleAge" class="text-info" tabindex="0" data-toggle="tooltip" runat="server"><i class="fas fa-fw fa-info-circle"></i></span>
                        </label>

                        <div class="offset-1 col-7">
                            <input id="inputAgeLevel0" type="text" class="form-control form-control-sm" placeholder="Age" runat="server" visible="false">
                            
                            <select id="inputAgeLevel1" class="selectpicker form-control form-control-sm" runat="server" visible="false">
                                <option value="">-- select --</option>
                                <option value="0-9">0 - 9</option>
                                <option value="10-19">10 - 19</option>
                                <option value="20-29">20 - 29</option>
                                <option value="30-39">30 - 39</option>
                                <option value="40-49">40 - 49</option>
                                <option value="50-59">50 - 59</option>
                                <option value="60-69">60 - 69</option>
                                <option value="70-79">70 - 79</option>
                                <option value="80-89">80 - 89</option>
                                <option value="90-99">90 - 99</option>
                            </select>

                            <select id="inputAgeLevel2" class="selectpicker form-control form-control-sm" runat="server" visible="false">
                                <option value="">-- select one --</option>
                                <option value="0-19">0 - 19</option>
                                <option value="20-39">20 - 39</option>
                                <option value="40-59">40 - 59</option>
                                <option value="60-79">60 - 79</option>
                                <option value="80-99">80 - 99</option>
                            </select>

                            <select id="inputAgeLevel3" class="selectpicker form-control form-control-sm" runat="server" visible="false" disabled="disabled">
                                <option value="*">*</option>
                            </select>
                        </div>
                    </div>
                </div>

                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-3" for="inputCountryofBirth">Sex</label>
                        <select class="form-control form-control-sm offset-1 col-7" id="Select1" runat="server">
                            <option value="">-- select one --</option>
                            <option value="Afghanistan">Afghanistan</option>
                            <option value="Albania">Albania</option>
                        </select>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-3" for="inputCountryofBirth">Gender</label>
                        <select class="form-control form-control-sm offset-1 col-7" id="Select2" runat="server">
                            <option value="">-- select one --</option>
                            <option value="Afghanistan">Afghanistan</option>
                            <option value="Albania">Albania</option>
                        </select>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-3" for="inputCountryofBirth">Postal Code</label>
                        <select class="form-control form-control-sm offset-1 col-7" id="Select3" runat="server">
                            <option value="">-- select one --</option>
                            <option value="Afghanistan">Afghanistan</option>
                            <option value="Albania">Albania</option>
                        </select>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-3" for="inputCountryofBirth">Marital Status</label>
                        <select class="form-control form-control-sm offset-1 col-7" id="Select4" runat="server">
                            <option value="">-- select one --</option>
                            <option value="Afghanistan">Afghanistan</option>
                            <option value="Albania">Albania</option>
                        </select>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-3" for="inputCountryofBirth">Diagnosis</label>
                        <select class="form-control form-control-sm offset-1 col-7" id="Select8" runat="server">
                            <option value="">-- select one --</option>
                            <option value="Afghanistan">Afghanistan</option>
                            <option value="Albania">Albania</option>
                        </select>
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
                        <label class="col-3" for="inputCountryofBirth">Record Type</label>
                        <select class="form-control form-control-sm offset-1 col-7" id="Select5" runat="server">
                            <option value="">-- select one --</option>
                            <option value="Afghanistan">Afghanistan</option>
                            <option value="Albania">Albania</option>
                        </select>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-3" for="inputCountryofBirth">Diagnosis</label>
                        <select class="form-control form-control-sm offset-1 col-7" id="Select6" runat="server">
                            <option value="">-- select one --</option>
                            <option value="Afghanistan">Afghanistan</option>
                            <option value="Albania">Albania</option>
                        </select>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group row">
                        <label class="col-3" for="inputCountryofBirth">Creation Data</label>
                        <select class="form-control form-control-sm offset-1 col-7" id="Select7" runat="server">
                            <option value="">-- select one --</option>
                            <option value="Afghanistan">Afghanistan</option>
                            <option value="Albania">Albania</option>
                        </select>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12 mb-4 text-center">
                    <button type="submit" id="buttonFilter" onserverclick="buttonFilter_ServerClick" class="btn btn-success" runat="server">Filter Patients and Records</button>
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
                            <asp:TemplateField HeaderText="Records" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewRecords" CssClass="btn btn-info btn-sm" runat="server">
                                        <i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span>
                                    </asp:LinkButton>
                                    <%--<asp:LinkButton ID="LinkButton1" CssClass="btn btn-info btn-sm" runat="server" CommandArgument='<%# Item.nric %>' CommandName="ViewPermission"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>--%>
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

                $.fn.selectpicker.Constructor.BootstrapVersion = '4';
                // Enable Multiple Select
                //$('.multi-select').multiselect({
                //    templates: {
                //        li: '<li><a href="javascript:void(0);"><label class="pl-2"></label></a></li>'
                //    }
                //});
                //$('.my-select').selectpicker();

            });
        }
    </script>
</asp:Content>
