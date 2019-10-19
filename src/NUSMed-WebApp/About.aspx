<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="NUSMed_WebApp.About" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-question-circle"></i>About NUSMed</h1>
        </div>

        <div class="row">
            <div class="col-12 text-center">
                <h2 class="display-5">The Project</h2>
                <p class="lead text-justify">This project was developed in the National Univeristy of Singapore for Information Security Capstone Project (IFS4205) in academic year 19/20 Semester 1.
                    NUSMed consists of a web and android application that are both open source. Both repositories are hosted on github and are located as stated below:
                </p>
                <ul class="text-left">
                   <li><a href="https://github.com/seanieyap/IFS4205-AY1920-S1-Team02-NUSMed-WebApp" target="_blank">Web Application Repository</a></li> 
                   <li><a href="https://github.com/seanieyap/IFS4205-AY1920-S1-Team02-NUSMed-AndroidApp" target="_blank">Android Application Repository</a></li> 
                </ul>

                <h2 class="display-5 mt-5">The Team</h2>
                <dl class="row">
                    <dt class="col-6 text-right">Sean Yap</dt>
                    <dd class="col-6 text-left">Overall Security Architect, DevOps.</dd>

                    <dt class="col-6 text-right">Lim Ding Heng</dt>
                    <dd class="col-6 text-left">2FA tag architect, Mobile Developer,</dd>

                    <dt class="col-6 text-right">Sun Shengran</dt>
                    <dd class="col-6 text-left">Pentester, Hardware Guru</dd>

                    <dt class="col-6 text-right">Toh Yunqi Cheryl</dt>
                    <dd class="col-6 text-left">Anonymization Expert, Data guru</dd>
                </dl>
            </div>
        </div>
    </div>
</asp:Content>
