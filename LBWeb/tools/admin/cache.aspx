<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Admin.CachePage" Codebehind="cache.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<controls:header id="_header" runat="server"/>
<form runat="server">
	<b>Cache Manager</b>
	<br />
	<br />
	The cache aids site performance by making sure frequently requested objects are kept in memory, so
	that they can be retrieved quickly, and without needing to query the database, which plases 
	unecessary strain on it.
	<br />
	<br />
	<i>Items cached: <asp:Literal id="_cacheCount" runat="server" /></i>
	<br />
	<br />
	&raquo; <asp:LinkButton id="_flushBtn" tooltip="Clear all the items in the cache..." runat="server" text="flush cache" onclick="FlushCacheHandler" />
	<br />
	<br />
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<table border="0" cellpadding="0" cellspacing="1" width="100%">
					<asp:Repeater id="_grid" runat="server" OnItemDataBound="ItemDataBoundHandler">
						<HeaderTemplate>
							<tr>
								<td class="TableHeaderCell">
									Name
								</td>
								<td class="TableHeaderCell">
									Type
								</td>
								<td class="TableHeaderCell">
									Hits
								</td>								
							</tr>
							<tr id="_noResultsRow" runat="server">
								<td class="TableBlankCell" colspan="3">
									no cached items to show
								</td>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr class="normal" bgcolor="#EAF1DD">
								<td class="TableBlankCell">
									<asp:Literal id="_name" runat="server" />
								</td>
								<td class="TableBlankCell">
									<asp:Literal id="_type" runat="server" />
								</td>
								<td class="TableBlankCell">
									<asp:Literal id="_hits" runat="server" />
								</td>
							</tr>
						</ItemTemplate>
						<FooterTemplate>
							<tr>
								<td class="TableHeaderCell" colspan="3">
									The top 100 items
								</td>
							</tr>
						</FooterTemplate>
					</asp:Repeater>
				</table>
			</td>
		</tr>	
	</table>
</form>
<controls:footer id="_footer" runat="server"/>