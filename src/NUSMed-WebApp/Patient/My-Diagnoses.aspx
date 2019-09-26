<%@ Page Title="My Diagnoses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="My-Diagnoses.aspx.cs" Inherits="NUSMed_WebApp.Patient.My_Diagnoses" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-poll-h"></i>My Diagnoses</h1>
            <p class="lead">View your Diagnoses recorded by your therapists.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelDiagnoses" class="container-fluid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-12">
                    <asp:GridView ID="GridViewPatientDiagnoses" CssClass="table table-sm" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary" EditRowStyle-CssClass="table-active"
                        ItemType="NUSMed_WebApp.Classes.Entity.PatientDiagnosis" OnPageIndexChanging="GridViewPatientDiagnoses_PageIndexChanging" 
                        EmptyDataRowStyle-CssClass="empty-table" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="Diagnosis Code">
                                <ItemTemplate>
                                    <%# Item.diagnosis.code %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Diagnosis Description">
                                <ItemTemplate>
                                    <%# Item.diagnosis.descriptionShort %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Diagnosis Category">
                                <ItemTemplate>
                                    <%# Item.diagnosis.categoryTitle %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Therapist">
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
                                    <%# Item.end == null ? "Present" : Item.end.ToString() %>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info text-center py-4" role="alert">
                                <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>You have no Diagnosis attributed to you.
                                </h4>
                                <p>Your therapists probably had not attributed any diagnoses to you.</p>
                                <hr>
                                <p class="mb-0">If this is a mistake, please contact the help-desk for assistance.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelDiagnoses" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
