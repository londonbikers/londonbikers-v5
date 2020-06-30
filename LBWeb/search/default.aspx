<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Search.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="padded">
    <h1><asp:Literal ID="_title" runat="server" /></h1>
    <div id="cse" style="width: 100%;">Loading</div>
    <script src="http://www.google.com/jsapi" type="text/javascript"></script>
    <script type="text/javascript">
        google.load('search', '1', { language: 'en', style: google.loader.themes.SHINY, term: 'honda' });
        google.setOnLoadCallback(function () {
            var customSearchControl = new google.search.CustomSearchControl('010770798718925201963:e3iagmmbqmy');
            customSearchControl.setResultSetSize(google.search.Search.FILTERED_CSE_RESULTSET);
            customSearchControl.draw('cse');
            <asp:Literal id="_searchTerm" runat="server" />
        }, true);
    </script>
    <link rel="stylesheet" href="/css/search.css" />
</div>
</asp:Content>