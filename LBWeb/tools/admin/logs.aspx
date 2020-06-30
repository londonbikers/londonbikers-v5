<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Admin.LogsPage" Codebehind="logs.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<controls:header id="_header" runat="server"/>
<form runat="server" ID="Form1">
	<b>Site Logs</b>
	<br />
	<br />
	The site will record instances of warnings or errors that may occur whilst live. The latest of these logs are visible below.
	<br />
	<br />
	<!--<img src="/_images/silk/delete.png" width="16" height="16" align="absmiddle" />
	<asp:LinkButton id="_flushBtn" tooltip="clear all the logs..." runat="server" text="flush logs" onclick="FlushLogsHandler" />
	<br />
	<br />-->
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<table border="0" cellpadding="0" cellspacing="1" width="100%">
					<asp:Repeater 
						id="_grid" 
						runat="server" 
						OnItemDataBound="ItemDataBoundHandler">
						<HeaderTemplate>
							<tr>
								<td class="TableHeaderCell">
									Type
								</td>
								<td class="TableHeaderCell">
									Message
								</td>
								<td class="TableHeaderCell">
									When
								</td>								
							</tr>
							<tr id="_noResultsRow" runat="server">
								<td class="TableBlankCell" colspan="3">
									no log entries to show
								</td>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr class="normal" bgcolor="#EAF1DD">
								<td class="TableBlankCell" style="color: gray;">
									<asp:Literal id="_type" runat="server" />
								</td>
								<td class="TableBlankCell">
									<asp:HyperLink id="_link" runat="server" text="view" /> -
									<asp:Literal id="_message" runat="server" />
								</td>
								<td class="TableBlankCell" nowrap="true" style="color: gray;">
									<asp:Literal id="_when" runat="server" />
								</td>
							</tr>
						</ItemTemplate>
						<FooterTemplate>
							<tr>
								<td class="TableHeaderCell" colspan="3">
									The top 100 entries
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