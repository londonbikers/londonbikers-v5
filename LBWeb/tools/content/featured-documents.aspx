<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Content.FeaturedDocumentsPage" Codebehind="featured-documents.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<Controls:Header id="_header" runat="server"/>
<form runat="server">
	<b>Featured Documents</b>
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
		<tr>
			<td>
				<div style="background-color: #f4f2f2; padding: 5px;">
					<table width="100%" cellpadding="0" cellspacing="0">
						<tr>
							<td>
								Section:
								<div style="padding-top: 2px;">
									<asp:DropDownList id="_sectionList" runat="server" autopostback="true" OnSelectedIndexChanged="FindDocumentsHandler" />
								</div>
							</td>
							<td align="right">
								Add Document:
								<div style="padding-top: 2px;">
									<asp:DropDownList id="_documentsToAddList" runat="server" /><br />
									<asp:ImageButton imageurl="../resources/images/btn_preview.gif" onclick="PreviewDocumentHandler" runat="server" style="margin-top: 5px;" />
									<asp:ImageButton imageurl="../resources/images/btn_add.gif" onclick="AddDocumentHandler" runat="server" style="margin-top: 5px;" />
								</div>
							</td>
						</tr>
					</table>
				</div>	
				<asp:Repeater 
					id="_documents" 
					runat="server"
					OnItemCreated="ItemCreatedHandler">
					<HeaderTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr><td colspan="6"><img src="../resources/images/t.gif" width="100%" height="1" /></td></tr>
							<tr class="normal" bgcolor="#D4E0BE" style="padding:3px;">
								<td style="padding:3px;" width="20">&nbsp;</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Title</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Author</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Status</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;" width="120">Published</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;" width="60">&nbsp;</td>
							</tr>
					</HeaderTemplate>
					<ItemTemplate>
							<tr><td colspan="6"><img src="../resources/images/t.gif" width="100%" height="1" /></td></tr>
							<tr class="normal" bgcolor="#EAF1DD">
								<td style="padding:3px;" align="center" width="20"><asp:Image id="_icon" runat="server" /></td>
								<td style="padding:3px;"><asp:HyperLink id="_titleLink" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_author" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_status" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_published" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:HyperLink id="_removeLink" runat="server" /></td>
							</tr>
					</ItemTemplate>
					<FooterTemplate>
							<tr>
								<td colspan="6"><img src="../resources/images/t.gif" width="100%" height="1" /></td>
							</tr>
							<tr bgcolor="#F4F2F2">
								<td style="padding: 5px; color: #9b9b9b;" colspan="6">
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
		<img src="/_images/silk/newspaper.png" /> - News<br />
		<img src="/_images/silk/layout.png" /> - Article<br />
		<img src="/_images/silk/page_white.png" /> - Generic<br />
		<img src="/_images/silk/page_white_link.png" /> - Generic, section default-document<br />
	</div>
</form>
<Controls:Footer id="_footer" runat="server"/>