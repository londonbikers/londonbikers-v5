<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Content.DefaultPage" Codebehind="default.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<Controls:Header id="_header" runat="server"/>
<form runat="server">
	<b>Documents</b>
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
		<tr>
			<td>
				<div style="background-color: #f4f2f2; padding: 5px;">
					<table cellpadding="0" cellspacing="0" border="0">
						<tr class="normal">
							<td>Title:</td>
							<td>Tag:</td>
							<td>Type:</td>
							<td>Status:</td>
							<td>Author:</td>
							<td>Date range:</td>
							<td>Site:</td>
							<td>&nbsp;</td>
						</tr>
						<tr class="normal">
							<td style="padding-right: 8px;">
								<asp:Textbox cssclass="box" id="_keyword" runat="server"/>
							</td>
							<td style="padding-right: 8px;">
								<asp:Textbox cssclass="box" id="_tag" runat="server"/>
							</td>
							<td style="padding-right: 8px;">
								<asp:Checkbox id="_byType" runat="server" /><asp:DropDownList onchange="_byType.checked=true;" id="_type" runat="server" />
							</td>
							<td style="padding-right: 8px;">
								<asp:Checkbox id="_byStatus" runat="server"/><asp:DropDownList onchange="_byStatus.checked=true;" id="_status" runat="server"/>
							</td>
							<td style="padding-right: 8px;">
								<asp:Checkbox id="_byAuthor" runat="server"/><asp:DropDownList id="_author" onchange="_byAuthor.checked=true;" runat="server"/>
							</td>
							<td style="padding-right: 8px;">
								<asp:DropDownList id="_range" runat="server">
									<asp:ListItem>Today</asp:ListItem>
									<asp:ListItem>This week</asp:ListItem>
									<asp:ListItem selected="selected">This month</asp:ListItem>
									<asp:ListItem>All</asp:ListItem>
								</asp:DropDownList>
							</td>
							<td style="padding-right: 8px;">
								<asp:Checkbox id="_bySite" runat="server"/><asp:DropDownList id="_site" onchange="_bySite.checked=true;" runat="server"/>
							</td>
							<td>
								<asp:ImageButton id="_findbtn" src="../resources/images/btn_find.gif" title="find content" runat="server" onclick="FindDocumentsHandler" />
							</td>
						</tr>
					</table>
				</div>			
				<asp:Repeater 
					id="_grid" 
					runat="server"
					OnItemCreated="ItemCreatedHandler">
					<HeaderTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr><td colspan="7"><img src="../resources/images/t.gif" width="100%" height="1" /></td></tr>
							<tr class="normal" bgcolor="#D4E0BE" style="padding:3px;">
								<td style="padding:3px;" width="20">&nbsp;</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Title</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Type</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Site(s)</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Author</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Status</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;" width="150">Created</td>
							</tr>
					</HeaderTemplate>
					<ItemTemplate>
							<tr><td colspan="7"><img src="../resources/images/t.gif" width="100%" height="1" /></td></tr>
							<tr class="normal" bgcolor="#EAF1DD">
								<td style="padding:3px;" align="center" width="20"><asp:Image id="_icon" runat="server" /></td>
								<td style="padding:3px;"><asp:HyperLink id="_titleLink" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_type" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_site" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_author" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_status" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_created" runat="server" /></td>
							</tr>
					</ItemTemplate>
					<FooterTemplate>
							<tr>
								<td colspan="7"><img src="../resources/images/t.gif" width="100%" height="1" /></td>
							</tr>
							<tr bgcolor="#F4F2F2">
								<td style="padding: 5px; color: #9b9b9b;" colspan="7">
									<asp:Literal id="_footer" runat="server"/>
								</td>
							</tr>
						</table>
					</FooterTemplate>
				</asp:Repeater>
			</td>
		</tr>	
	</table>
	<div class="LightNoteBox" style="margin-top: 10px;">
		<b>Key:</b>
		<br />
		<br />
		<img src="/_images/silk/newspaper.png" align="absmiddle" /> - News<br />
		<img src="/_images/silk/layout.png" align="absmiddle" /> - Article<br />
		<img src="/_images/silk/page_white.png" align="absmiddle" /> - Generic<br />
		<img src="/_images/silk/page_white_link.png" align="absmiddle" /> - Generic, section default-document<br />
	</div>
</form>
<Controls:Footer id="_footer" runat="server"/>