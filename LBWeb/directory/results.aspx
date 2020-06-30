<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="results.aspx.cs" Inherits="Tetron.Directory.Results" %>
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
        <h1>Directory Search</h1>
        <asp:Literal id="_searchCriteria" runat="server" />.

        <div id="_categoriesDiv" runat="server" class="standoutBox mb20 mt20">
		    <div class="arial bold">Matching Categories</div>
		    <div class="smallText mt5 lightText">
			    <asp:Literal id="_catResultStats" runat="server" />
		    </div>
		    <table class="mt10" cellpadding="0" cellspacing="0">
			    <tr>
				    <asp:Literal id="_categoryGrid" runat="server" />
			    </tr>
		    </table>	
	    </div>

	    <table width="100%" cellpadding="0" cellspacing="0" class="mb20">
		    <tr>
			    <td>
				    <div class="arial bold">
					    Matching Items
				    </div>
				    <div class="smallText mt5 lightText">
					    <asp:Literal id="_paginationStats" runat="server" />
				    </div>
			    </td>
			    <td align="right" valign="top">
					 <asp:Literal id="_topPaginationControls" runat="server" />
			    </td>
		    </tr>
	    </table>
	    
	    <div id="_promptDiv" runat="server" visible="false" />
		<asp:Repeater 
			id="_items" 
			runat="server" 
			onitemdatabound="ItemDataBoundHandler">
			<HeaderTemplate>
				<asp:Literal id="_noResultsText" runat="server" />
			</HeaderTemplate>
			<ItemTemplate>
				<img src="/_images/silk/zoom.png" valign="absmiddle" />
				<asp:HyperLink id="_itemLink" runat="server" cssclass="big arial bold" />
                <p>
				    <asp:Literal id="_itemDescription" runat="server" />
                </p>
				<div class="smallText mt5 lightText" style="line-height: 17px;">
					<asp:Literal id="_itemCategoryPath" runat="server" />
				</div>
			</ItemTemplate>
			<SeparatorTemplate>
				<br />
				<br />
			</SeparatorTemplate>
		</asp:Repeater>
	    

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