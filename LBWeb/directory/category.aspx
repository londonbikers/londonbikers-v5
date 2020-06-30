<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="category.aspx.cs" Inherits="Tetron.Directory.Category" %>
<%@ Register TagPrefix="Controls" TagName="SearchBox" Src="~/_controls/DirectorySearchBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/directory.css" />
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
        <h1 class="mb5"><span class="lightText">Category:</span> <asp:Literal id="_categoryTitle" runat="server" /></h1>
        <div class="smallText">
            <asp:Literal id="_breadcrumbTrail" runat="server" />
        </div>
        <div class="mt10">
			<img src="/_images/silk/add.png" width="16" height="16" alt="" />
			<asp:HyperLink 
				id="_submitLink" 
				runat="server" 
				text="submit an item" 
				tooltip="submit an item to this category" />
			<span id="_registerSpan" runat="server" visible="false" />
		</div>
        <hr />

        <div id="_subCategoriesDiv" runat="server">
			<div class="lightText">
                Sub Categories:
            </div>
			<table cellpadding="0" cellspacing="0" class="mt10">
				<tr>
					<asp:Literal id="_subCatsGrid" runat="server" />
				</tr>
			</table>	
			<hr class="mt10 mb10"/>
		</div>

        <table width="100%" cellpadding="0" cellspacing="0">
			<tr>
				<td>
					<div class="arial mb10">
						<b>Category Items</b>
					</div>
					<div class="lightText mt5 smallText">
						<asp:Literal id="_paginationStats" runat="server" />
					</div>
				</td>
				<td align="right" valign="top">
					<asp:Literal id="_topPaginationControls" runat="server" />
				</td>
			</tr>
		</table>

        <div class="mt10">
			<asp:Repeater 
				id="_items" 
				runat="server" 
				onitemdatabound="ItemDataBoundHandler">
				<HeaderTemplate>
					<asp:Literal id="_noResultsText" runat="server" />
				</HeaderTemplate>
				<ItemTemplate>
					<img src="/_images/silk/zoom.png" alt="" />
					<asp:HyperLink id="_itemLink" runat="server" cssclass="big arial bold" /></span><br /><br />
					<asp:Literal id="_itemDescription" runat="server" /><br />
				</ItemTemplate>
				<SeparatorTemplate>
					<br />
					<br />
				</SeparatorTemplate>
		    </asp:Repeater>
		</div>

        <table width="100%" cellpadding="0" cellspacing="0">
			<tr>
				<td>
					&nbsp;
				</td>
				<td align="right">
					<asp:Literal id="_bottomPaginationControls" runat="server" />
				</td>
			</tr>
		</table>

    </div>
</asp:Content>