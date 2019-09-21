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

    <asp:UpdatePanel ID="UpdatePanelRecords" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-12">
                    <asp:GridView ID="GridViewRecords" CssClass="table table-sm table-responsive-md" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pagination-gridview"
                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary"
                        OnRowDataBound="GridViewRecords_RowDataBound" OnRowCommand="GridViewRecords_RowCommand" ItemType="NUSMed_WebApp.Classes.Entity.Record"
                        OnPageIndexChanging="GridViewRecords_PageIndexChanging" DataKeyNames="id" EmptyDataRowStyle-CssClass="empty-table" runat="server">
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

                            <asp:TemplateField HeaderText="Content">
                                <ItemTemplate>
                                    <asp:Label ID="LabelContent" runat="server" Visible="false"></asp:Label>
                                    <asp:LinkButton ID="LinkbuttonFileView" CssClass="btn btn-info btn-sm" runat="server" Visible="false"></asp:LinkButton>

                                    <a id="FileDownloadLink" class="btn btn-warning btn-sm" runat="server" visible="false">
                                        <i class="fas fa-fw fa-cloud-download-alt"></i></i><span class="d-none d-lg-inline-block">Download</span>
                                    </a>
                                    <asp:Label ID="LabelFileType" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelRecords" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modalFileView" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelFileView" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-eye"></i>
                            View Record File:
                            <asp:Label ID="labelRecordName" runat="server"></asp:Label></h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body text-center">
                        <%-- image, movie, timeseries --%>
                        <asp:Image ID="modalFileViewImage" CssClass="img-fluid" runat="server" />
                        <video id="modalFileViewVideo" style="width: 100%; height: auto;" controls runat="server">
                            <source id="so" type="video/mp4" runat="server">
                            Your browser does not support the video tag.
                        </video>
                        <asp:Label ID="modalFileViewLabelText" runat="server" Visible="false"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <a id="FileDownloadLinkviaModal" class="btn btn-warning btn-sm" runat="server">
                            <i class="fas fa-fw fa-cloud-download-alt"></i></i><span class="d-none d-lg-inline-block">Download</span>
                        </a>
                        <span class="text-info small" runat="server">
                            <i class="fas fa-fw fa-info-circle"></i>File Name:
                            <asp:Label ID="modalFileViewLabelFileName" runat="server"></asp:Label>. File Size: 
                            <asp:Label ID="modalFileViewLabelFileSize" runat="server"></asp:Label>
                        </span>

                        <button type="button" class="btn btn-secondary ml-auto" data-dismiss="modal">Close</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelFileView" DisplayAfter="0" DynamicLayout="false">
                <ProgressTemplate>
                    <div class="loading">Loading</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $(function () {
                $('[data-toggle="tooltip"]').tooltip({ html: true });
            });
        }
    </script>
</asp:Content>
