<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="image.aspx.cs" Inherits="Tetron.EditorialImagePage" %>
<%@ Register TagPrefix="Controls" TagName="FacebookLike" Src="~/_controls/FacebookLike.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HyperLink id="_image" runat="server" tooltip="click to view full-size image" />
    <div class="padded">
    <Controls:FacebookLike ID="FacebookLike1" runat="server" />
        <hr />
        <h2 class="mb10"><span class="lightText">Hi-def Image:</span> <asp:Literal id="_title" runat="server" /></h2>
        <!-- google_ad_section_start -->
	    <asp:Literal id="_description" runat="server" />
	    <!-- google_ad_section_end -->
        <div class="mt10">
            <span class="arial bold"><asp:Hyperlink id="_documentLink" runat="server" /></span>
        </div>
        <div class="smallText mt10 subText">
		    All images are the exclusive property of a third-party or <%= ConfigurationManager.AppSettings["Global.Domain"] %> and may not be distributed without any shown watermark.
	    </div>
    </div>
</asp:Content>