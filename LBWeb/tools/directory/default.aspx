<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Directory.DefaultPage" Codebehind="default.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<controls:header id="_header" runat="server"/>
<form runat="server">
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<table border="0" cellpadding="0" cellspacing="1" width="100%">
					<tr class="normal">
						<td colspan="6">
							<table width="100%" cellspacing="0" cellpadding="0">
								<tr>
									<td class="TableBlankCell" valign="top">
										<b>Directory</b><br />
										<a href="./">latest submissions</a><br />
									</td>
									<td align="right" class="TableBlankCell" valign="top">
										Keyword Search<br />
										<asp:TextBox id="_keyword" cssclass="box" runat="server" style="margin-top: 3px; margin-bottom: 7px;" />
										<asp:ImageButton style="vertical-align: bottom; margin-bottom: 9px;" ImageURL="../resources/images/btn_find.gif" onclick="SearchHandler" runat="server" ID="Imagebutton1"/>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<asp:Repeater id="_items" runat="server" OnItemDataBound="ItemDataBoundHandler">
						<HeaderTemplate>
							<tr>
								<td class="TableHeaderCell">
									Title
								</td>
								<td class="TableHeaderCell">
									Category
								</td>
								<td class="TableHeaderCell">
									Submitter
								</td>
								<td class="TableHeaderCell">
									Created
								</td>
								<td class="TableHeaderCell" width="77">
									Status
								</td>
								<td class="TableHeaderCell" width="60">
									&nbsp;
								</td>
							</tr>
							<tr id="_noResultsRow" runat="server">
								<td class="TableBlankCell" colspan="6">
									no results to show
								</td>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr class="normal" bgcolor="#EAF1DD">
								<td class="TableBlankCell">
									<asp:Literal id="_title" runat="server" />
								</td>
								<td class="TableBlankCell" class="td_dark">
									<asp:HyperLink id="_categoryLink" runat="server" target="_blank" />
								</td>
								<td class="TableBlankCell" class="td_dark">
									<asp:HyperLink id="_submitterLink" runat="server" />
								</td>
								<td class="TableBlankCell" class="td_dark" width="160">
									<asp:Literal id="_when" runat="server" />
								</td>
								<td class="TableBlankCell" align="right">
									<asp:DropDownList id="_status" runat="server" />
								</td>
								<td class="TableBlankCell">
									<asp:HyperLink id="_editLink" ToolTip="Edit this item" runat="server" ImageURL="../resources/images/btn_edit.gif" />
								</td>
							</tr>
						</ItemTemplate>
						<FooterTemplate>
							<tr>
								<td class="TableHeaderCell" colspan="4">
									&nbsp;
								</td>
								<td class="TableHeaderCell" align="center">
									<asp:ImageButton id="_changeStatuses" title="Update item status'" src="../resources/images/btn_update.gif" runat="server" onclick="ChangeStatusHandler"/>
								</td>
								<td class="TableHeaderCell">
									&nbsp;
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