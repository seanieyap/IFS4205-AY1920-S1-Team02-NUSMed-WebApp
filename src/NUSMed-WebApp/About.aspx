<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="NUSMed_WebApp.About" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
                <div id="DivTimeSeries">
</div>

    <script src="/Scripts/plotly-latest.min.js" type="text/javascript"></script>
    <script>
        //Plotly.d3.csv("/Scripts/finance-charts-apple.txt", function (err, rows) {

            //function unpack(rows, key) {
            //    return rows.map(function (row) { return row[key]; });
            //}

        //var layout = {
        //    title: 'Basic Time Series',
        //};

        //    var trace1 = {
        //        type: "scatter",
        //        mode: "lines",
        //        name: 'ECG',
        //        y: [],
        //        x: [],
        //        line: { color: '#17BECF' }
        //    }

        //    //var trace2 = {
        //    //    type: "scatter",
        //    //    mode: "lines",
        //    //    name: 'AAPL Low',
        //    //    x: unpack(rows, 'Date'),
        //    //    y: unpack(rows, 'AAPL.Low'),
        //    //    line: { color: '#7F7F7F' }
        //    //}trace1

        //    //var data = [];


        //Plotly.newPlot('DivTimeSeries', [trace1], layout);
        //})
    </script>
</asp:Content>
