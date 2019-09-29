<%@ Page Title="Record Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Record-Search.aspx.cs" Inherits="NUSMed_WebApp.Researcher.Record_Search" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-search"></i>Record Search</h1>
            <p class="lead">Records here have been anonymized and may not fit your search criteria exactly</p>
        </div>
    </div>
    <asp:GridView ID="GridViewAnonRecords" CssClass="table table-sm table-responsive-md" runat="server" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pagination-gridview"
        OnPageIndexChanging="GridViewAnonRecords_PageIndexChanging" AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" 
        EmptyDataRowStyle-CssClass="empty-table" OnRowDataBound="GridViewRecords_RowDataBound" ItemType="NUSMed_WebApp.Classes.Entity.RecordAnonymised">
        <Columns>
            <asp:TemplateField HeaderText="Age">
                <ItemTemplate>
                    <%# Item.age %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="sex" HeaderText="Sex" />
            <asp:BoundField DataField="gender" HeaderText="Gender" />
            <asp:BoundField DataField="marital_status" HeaderText="Marital Status" />
            <asp:BoundField DataField="postal" HeaderText="Postal Code" />
            <asp:BoundField DataField="diagnosis_code" HeaderText="Diagnosis Code" />
            <asp:BoundField DataField="record_type" HeaderText="Record Type" />
            <asp:BoundField DataField="record_creation_date" HeaderText="Record Creation Date" />
            <asp:TemplateField HeaderText="Data" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                <ItemTemplate>
                    <asp:Label ID="LabelContent" runat="server" Visible="false"></asp:Label>
                    <%--                    <asp:LinkButton ID="LinkbuttonFileView" CssClass="btn btn-info btn-sm" runat="server" Visible="false"></asp:LinkButton>--%>

                    <a id="FileDownloadLink" class="btn btn-warning btn-sm" runat="server" visible="false">
                        <i class="fas fa-fw fa-cloud-download-alt"></i></i>
                    </a>
                    <asp:Label ID="LabelFileType" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>


        </Columns>
    </asp:GridView>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
