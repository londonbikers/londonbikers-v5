<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Admin.ReferrersPage" Codebehind="referrers.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<controls:header id="_header" runat="server"/>
<form runat="server" ID="Form1">
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<table border="0" cellpadding="0" cellspacing="1" width="100%">
					<tr class="normal">
						<td colspan="2" class="TableBlankCell">
							<b>Blacklisted Referrers</b><br />
							Adding a blacklisted URL here will block the host from hot-linking to any local content. Note
							that not just the specific URL will be blocked, but the whole domain.
							<br />
							<br />
							<asp:TextBox 
								id="_url" 
								runat="server" 
								cssclass="box" 
								enableviewstate="false"
								style="vertical-align: bottom" />
							<asp:ImageButton 
								imageurl="../resources/images/btn_add.gif" 
								runat="server" 
								onclick="AddBlacklistedReferrerHandler" />
						</td>
					</tr>
					<asp:Repeater id="_grid" runat="server" OnItemDataBound="ItemDataBoundHandler">
						<HeaderTemplate>
							<tr>
								<td class="TableHeaderCell">
									Referrer
								</td>
								<td class="TableHeaderCell">
									&nbsp;
								</td>								
							</tr>
							<tr id="_noResultsRow" runat="server">
								<td class="TableBlankCell" colspan="2">
									no referrers to show
								</td>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr class="normal" bgcolor="#EAF1DD">
								<td class="TableBlankCell">
									<asp:Literal id="_url" runat="server" />
								</td>
								<td class="TableBlankCell" width="58">
									<asp:HyperLink id="_removeLink" runat="server" imageurl="../resources/images/btn_remove.gif" />
								</td>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
				</table>
			</td>
		</tr>	
	</table>
</form>
<controls:footer id="_footer" runat="server"/>