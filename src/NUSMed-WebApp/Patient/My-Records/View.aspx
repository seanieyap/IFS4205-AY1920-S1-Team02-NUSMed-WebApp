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

    <asp:UpdatePanel ID="UpdatePanelRecords" class="container-fluid" runat="server" UpdateMode="Conditional">
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
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <%# Item.description %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="type">
                                <ItemTemplate>
                                    <%# Item.type.name %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created By">
                                <ItemTemplate>
                                    <%# Item.creatorLastName + " " + Item.creatorFirstName %>
                                    <asp:Label ID="LabelIsEmergency" runat="server" Visible="false" class="text-danger" data-toggle="tooltip">
                                        <i class="fas fa-fw fa-info-circle"></i>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Creation Time">
                                <ItemTemplate>
                                    <%# Item.createTime %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Data" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="LabelContent" runat="server" Visible="false"></asp:Label>
                                    <asp:LinkButton ID="LinkbuttonFileView" CssClass="btn btn-info btn-sm" runat="server" Visible="false"></asp:LinkButton>

                                    <a id="FileDownloadLink" class="btn btn-warning btn-sm" runat="server" visible="false">
                                        <i class="fas fa-fw fa-cloud-download-alt"></i>
                                    </a>
                                    <asp:Label ID="LabelFileType" TabIndex="0" data-toggle="tooltip" runat="server" Visible="false"><i class="fas fa-fw fa-info-circle"></i></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Permissions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonViewFineGrain" CssClass="btn btn-info btn-sm" runat="server">
                                        <i class="fas fa-fw fa-eye"></i></i><span class="d-none d-lg-inline-block">View</span>
                                    </asp:LinkButton>
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

    <div id="modalFineGrain" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <asp:UpdatePanel ID="UpdatePanelFineGrain" class="modal-content" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title text-capitalize"><i class="fas fa-fw fa-user-injured"></i>
                            <asp:Label ID="modalLabelFineGrainRecordTitle" runat="server"></asp:Label>: View Record Status / Fine Grain Permissions</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-12">
                                    <h4>Record Status</h4>
                                    <p class="text-muted">Setting this allows/disallows all therapists and any other parties from accessing this record.</p>
                                    <div class="form-group">
                                        <div class="btn-group btn-group-sm border" role="group" aria-label="Record Status">
                                            <asp:LinkButton ID="LinkButtonStatusDisable" runat="server" CommandName="StatusDisable" OnClick="LinkButtonStatusDisable_Click">
                                                <i class="fas fa-fw fa-lock"></i> Disable Record
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="LinkButtonStatusEnable" runat="server" CommandName="StatusEnable" OnClick="LinkButtonStatusEnable_Click">
                                                <i class="fas fa-fw fa-unlock-alt"></i> Enable Record
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3">
                                <div class="col-12">
                                    <h4>Fine Grain Permissions</h4>
                                    <p class="text-muted">Setting this allows/disallows specified therapists from accessing this record. The default will revert access control to record type permissions.</p>
                                </div>

                                <div class="col-12">
                                    <p class="lead mb-2">Your Therapist.</p>
                                </div>
                                <div class="col-12 col-sm-12 col-md-12 col-lg-10 col-xl-8 mx-auto">
                                    <asp:Panel CssClass="input-group" runat="server" DefaultButton="LinkButtonFineGrainAllow">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">Search</span>
                                        </div>
                                        <asp:TextBox ID="TextboxSearchFineGrainAllow" CssClass="form-control" placeholder="First Name / Last Name" runat="server"></asp:TextBox>
                                        <div class="input-group-append">
                                            <asp:LinkButton ID="LinkButtonFineGrainAllow" CssClass="btn btn-outline-info" OnClick="LinkButtonFineGrainAllow_Click" runat="server">
                                                <i class="fas fa-fw fa-search"></i> Go
                                            </asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="col-12 mt-3">
                                    <asp:GridView ID="GridViewFineGrain" CssClass="table table-sm table-responsive-md" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pagination-gridview"
                                        AutoGenerateColumns="false" CellPadding="0" EnableTheming="False" GridLines="None" FooterStyle-CssClass="table-secondary" EditRowStyle-CssClass="table-active"
                                        ItemType="NUSMed_WebApp.Classes.Entity.Therapist" DataKeyNames="nric" OnRowCommand="GridViewFineGrain_RowCommand" OnRowDataBound="GridViewFineGrain_RowDataBound"
                                        OnPageIndexChanging="GridViewFineGrain_PageIndexChanging" EmptyDataRowStyle-CssClass="empty-table" runat="server">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Therapist Name">
                                                <ItemTemplate>
                                                    <%# Item.lastName + " " + Item.firstName %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job Title">
                                                <ItemTemplate>
                                                    <%# Item.therapistJobTitle == string.Empty ?  "Nil" : Item.therapistJobTitle %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department">
                                                <ItemTemplate>
                                                    <%# Item.therapistDepartment == string.Empty ?  "Nil" : Item.therapistDepartment %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <div class="form-group">
                                                        <div class="btn-group btn-group-sm border" role="group" aria-label="Record Status">
                                                            <asp:LinkButton ID="LinkButtonRecordStatusDefault" runat="server" CommandName="DefaultTherapist" CommandArgument='<%# Item.nric %>'>
                                                                Default
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="LinkButtonRecordStatusEnable" runat="server" CommandName="AllowTherapist" CommandArgument='<%# Item.nric %>'>
                                                                <i class="fas fa-fw fa-lock"></i> Allow
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="LinkButtonRecordStatusDisable" runat="server" CommandName="DisallowTherapist" CommandArgument='<%# Item.nric %>'>
                                                                <i class="fas fa-fw fa-unlock-alt"></i> Disallow
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="alert alert-info text-center py-4" role="alert">
                                                <h4 class="alert-heading"><i class="fas fa-fw fa-info-circle mr-2"></i>No Results.
                                                </h4>
                                                <p>There are no Therapists Allowed access to this record.</p>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <p class="text-info mb-0 mx-auto text-center"><i class="fas fa-fw fa-info-circle"></i>Fine Grain Permissions overrides Record Type Permissions.</p>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelFineGrain" DisplayAfter="0" DynamicLayout="false">
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
