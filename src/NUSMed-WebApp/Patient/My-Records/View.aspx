<%@ Page Title="View My Records" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="NUSMed_WebApp.Patient.My_Records.View" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-file-medical"></i>View My Records</h1>
            <p class="lead">View your Medical Records here!</p>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-12">
                <asp:GridView ID="GridViewRecords" CssClass="table table-sm table-responsive-md" AutoGenerateColumns="false" CellPadding="0"
                    EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary"
                    ItemType="NUSMed_WebApp.Classes.Entity.Record" DataKeyNames="id"
                    EmptyDataRowStyle-CssClass="empty-table" runat="server">
                    <Columns>
                        <asp:TemplateField HeaderText="Title">
                            <ItemTemplate>
                                <%# Item.title %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="type">
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
                                <%# Item.createTime %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <%# Item.content +" " +  Item.type.prefix %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <EmptyDataTemplate>
                        <div class="alert alert-info text-center py-4" role="alert">
                            <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>You have no Records.
                            </h4>
                            <p>Try adding a Record of your own.</p>
                            <hr>
                            <p class="mb-0">First visit? Try visting the New Records page.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
