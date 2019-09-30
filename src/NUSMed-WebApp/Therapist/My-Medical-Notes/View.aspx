<%@ Page Title="View Medical Note" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="NUSMed_WebApp.Therapist.My_Medical_Notes.View" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
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
                            <asp:TemplateField HeaderText="Title">
                                <ItemTemplate>
                                    <%# Item.title %>
                                </ItemTemplate>
                            </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Patient / Subject" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.patient.lastName + " " +Item.patient.firstName %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Created By" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%# Item.therapist.lastName + " " +Item.therapist.firstName %>
                                </ItemTemplate>
                            </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Creation Time">
                                <ItemTemplate>
                                    <%# Item.createTime %>
                                </ItemTemplate>
                            </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%--<%# Item.createTime %>--%>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--                            <asp:TemplateField HeaderText="Information" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewInformation" runat="server"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Diagnoses" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewDiagnosis" runat="server"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Records" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelRecordStatus" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                    <asp:LinkButton ID="LinkButtonViewRecords" runat="server"><i class="fas fa-fw fa-eye"></i><span class="d-none d-lg-inline-block">View</span></asp:LinkButton>
                                    <asp:HyperLink ID="LinkButtonNewRecord" runat="server"><i class="fas fa-fw fa-file-medical"></i><span class="d-none d-lg-inline-block">New Record</span></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
