﻿<%@ Page Title="Record Logs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Record-Logs.aspx.cs" Inherits="NUSMed_WebApp.Admin.View_Logs.Record_Logs" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Content/log" rel="stylesheet" type="text/css" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/LogJs") %>
    </asp:PlaceHolder>

    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-database"></i>Record Logs</h1>
            <p class="lead mb-1">Restricted. This page is meant for administrators only.</p>
            <p class="text-muted">Page is limited to 200 log event per search.</p>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-12 mb-3">
                <h2>Search Options</h2>
            </div>
        </div>

        <div class="row">
            <div class="col-12 col-md-6 col-lg-5">
                <div class="form-group row">
                    <label class="col-4" for="InputSubjectNRICs" runat="server">
                        Subject 
                    </label>
                    <div class="col-8">
                        <asp:ListBox ID="InputSubjectNRICs" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6 col-lg-5">
                <div class="form-group row">
                    <label class="col-4" for="inputActions" runat="server">
                        Action 
                    </label>
                    <div class="col-8">
                        <asp:ListBox ID="inputActions" CssClass="selectpicker form-control form-control-sm" multiple="multiple" data-live-search="true" SelectionMode="Multiple" data-none-selected-text="All" runat="server" ClientIDMode="Static"></asp:ListBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-12 col-md-6 col-lg-5">
                <div class="form-group row">
                    <label class="col-4" for="inputDateTimeFrom" runat="server">
                        Date From 
                    </label>
                    <div class="input-group input-group-sm date col-8" id="datetimepickerDateFrom" data-target-input="nearest">
                        <input id="inputDateTimeFrom" type="text" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepickerDateFrom" runat="server" />
                        <div class="input-group-append" data-target="#datetimepickerDateFrom" data-toggle="datetimepicker">
                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-6 col-lg-5">
                <div class="form-group row">
                    <label class="col-4" for="inputDateTimeTo" runat="server">
                        Date To 
                    </label>
                    <div class="input-group input-group-sm date col-8" id="datetimepickerDateTo" data-target-input="nearest">
                        <input id="inputDateTimeTo" type="text" class="form-control form-control-sm datetimepicker-input" data-target="#datetimepickerDateTo" runat="server" />
                        <div class="input-group-append" data-target="#datetimepickerDateTo" data-toggle="datetimepicker">
                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 col-lg-2">
                <div class="input-group input-group-sm">
                    <asp:LinkButton ID="ButtonSearch" CssClass="btn btn-sm btn-outline-info ml-auto" OnClick="ButtonSearch_Click" runat="server">
                            <i class="fas fa-fw fa-search"></i> Search
                    </asp:LinkButton>
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanelLogs" class="row" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="col-12">
                    <asp:GridView ID="GridViewLogs" CssClass="table table-sm table-responsive-sm small text-truncate" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" OnPageIndexChanging="GridViewAccounts_PageIndexChanging"
                        ItemType="NUSMed_WebApp.Classes.Entity.LogEvent" DataKeyNames="id" EmptyDataRowStyle-CssClass="empty-table" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="Date/Time">
                                <ItemTemplate>
                                    <%# Item.createTime.ToString() %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Subject">
                                <ItemTemplate>
                                    <%# Item.creatorNRIC %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <%# Item.action %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <%# Item.description %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info text-center py-4" role="alert">
                                <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>Search returned no results.</h4>
                                <p>Do try widening your search parameter(s).</p>
                                <hr>
                                <p class="mb-0">New here? Try entering some search parameters and hit "Search"!</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonSearch" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelLogs" DisplayAfter="200" DynamicLayout="false">
            <ProgressTemplate>
                <div class="loading">Loading</div>
            </ProgressTemplate>
        </asp:UpdateProgress>
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

                // Enable date and time picker
                $.fn.datetimepicker.Constructor.Default = $.extend({}, $.fn.datetimepicker.Constructor.Default, {
                    icons: {
                        time: 'fas fa-clock',
                        date: 'fas fa-calendar',
                        up: 'fas fa-arrow-up',
                        down: 'fas fa-arrow-down',
                        previous: 'fas fa-chevron-left',
                        next: 'fas fa-chevron-right',
                        today: 'fas fa-calendar-check-o',
                        clear: 'fas fa-trash',
                        close: 'fas fa-times'
                    }
                });
                $('#datetimepickerDateFrom').datetimepicker();
            });
        }
    </script>
</asp:Content>