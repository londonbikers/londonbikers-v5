<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="item.aspx.cs" Inherits="Tetron.Directory.Item" %>
<%@ Register TagPrefix="Controls" TagName="SearchBox" Src="~/_controls/DirectorySearchBox.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Comments" Src="~/_controls/Comments.ascx" %>
<%@ Register TagPrefix="Controls" TagName="FacebookLike" Src="~/_controls/FacebookLike.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/directory.css" />
    <% if (HaveMap) { %>
    <script type="text/javascript">
        var markerLat = "<%= Latitude %>";
        var markerLong = "<%= Longitude %>";
    </script>
    <script type="text/javascript" src="http://www.google.com/jsapi"></script>
    <script type="text/javascript" src="/js/map.js"></script>
    <% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="searchBox">
        <div class="floatLeft">
            <h2>The Directory</h2>
        </div>
        <div class="floatRight">
            <Controls:SearchBox 
				ShowModeControls="false" 
				id="_searchBox" 
				runat="server" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="padded">

        <!-- google_ad_section_start -->
		<h1><asp:Literal id="_title" runat="server" /></h1>
		<!-- google_ad_section_end -->
        <hr />
        <Controls:FacebookLike id="_facebookApi" runat="server" />
        
        <div class="arial bold mb10 mt10">
			Description
		</div>
		
		<!-- google_ad_section_start -->
		<div class="LightNoteBox" id="_description" runat="server" />
		<!-- google_ad_section_end -->

		<div id="_phoneDiv" runat="server">
			<div class="arial bold" style="margin-top: 20px;">
				Telephone Number
			</div>
			<div id="_telephoneNumberDiv" runat="server" class="mt10 mb10" style="font-style: italic; font-size: 18px; color: #747474;" />
		</div>		

		<div id="_keywordDiv" runat="server">
			<div class="arial bold">
				Tags
			</div>
			<div class="mt10 mb10 smallText">
				You can also see similar items by using the tags below.
			</div>
			<div style="line-height: 23px;">
				<img src="/_images/silk/tag_red.png" width="16" height="16" alt="" />
				<asp:Repeater 
					id="_keywords" 
					runat="server" 
					onitemdatabound="KeywordDataBoundHandler">
					<ItemTemplate><asp:HyperLink id="_listKeyword" runat="server" /></ItemTemplate>
					<SeparatorTemplate>, </SeparatorTemplate>
				</asp:Repeater>
			</div>
		</div>
						
		<div id="_linksDiv" runat="server">
			<div class="arial bold mt20">
				Links
			</div>
			<div class="GeneralNormal">
				<asp:Repeater 
					id="_links" 
					runat="server" 
					onitemdatabound="LinkDataBoundHandler">
					<HeaderTemplate>
						<ul style="line-height: 22px;">
					</HeaderTemplate>
					<ItemTemplate>
							<li><asp:HyperLink target="_blank" id="_listLink" runat="server" /></li>
					</ItemTemplate>
					<FooterTemplate>
						</ul>
					</FooterTemplate>
				</asp:Repeater>
			</div>
		</div>

		<div class="arial bold mt20">
			Shown in
		</div>
		<asp:Repeater 
			id="_categories" 
			runat="server" 
			onitemdatabound="CategoryDataBoundHandler">
			<HeaderTemplate>
				<ul style="line-height: 22px;">
			</HeaderTemplate>
			<ItemTemplate>
					<li><asp:Literal id="_listCategory" runat="server" /></li>
			</ItemTemplate>
			<FooterTemplate>
				</ul>
			</FooterTemplate>
		</asp:Repeater>

        <asp:PlaceHolder ID="_mapHolder" runat="server" Visible="false">
            <div class="arial bold mt20">Location</div>
            <div id="mapCanvas"></div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="_imagesHolder" runat="server">
            <div class="arial bold mt20">
			    Images
		    </div>
		    <hr />
		    <asp:Repeater 
			    id="_images" 
			    runat="server" 
			    onitemdatabound="ImageDataBoundHandler">
			    <ItemTemplate>
				    <table cellpadding="0" cellspacing="0" align="center" width="10">
					    <tr>
						    <td class="ThumbCell"><asp:Image id="_listImage" runat="server" /></td>
					    </tr>
				    </table>
			    </ItemTemplate>
			    <SeparatorTemplate>
				    <br />
			    </SeparatorTemplate>
		    </asp:Repeater>
        </asp:PlaceHolder>

        <div class="mt20">
            <Controls:Comments id="_comments" runat="server" />
        </div>

    </div>
</asp:Content>