<%@ Page language="c#" Inherits="Tetron.Tools.Content.IncomingPage" Codebehind="incoming.aspx.cs" EnableEventValidation="false" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<Controls:Header id="_header" runat="server"/>
<form runat="server">
	<b>Incoming News</b>
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
		<tr>
			<td>
			    <div style="background-color: #f4f2f2; padding: 5px;">
					These are the latest news stories coming in to news@londonbikers.com.
				</div>	
				<asp:Repeater 
					id="_grid" 
					runat="server"
					OnItemCreated="ItemCreatedHandler">
					<HeaderTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr><td colspan="7"><img src="../resources/images/t.gif" width="100%" height="1" /></td></tr>
							<tr class="normal" bgcolor="#E0E0E0" style="padding:3px;">
								<td style="padding: 5px; color: #9b9b9b; border-bottom: solid 1px #BCBCBC;" colspan="7">
									<asp:ImageButton imageurl="/tools/resources/images/btn_delete.gif" onclick="DeleteDocumentHandler" runat="server" />
								</td>
							</tr>
							<tr class="normal" bgcolor="#D4E0BE" style="padding:3px;">
							    <td style="padding:3px;" width="20">&nbsp;</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Title</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;">Site(s)</td>
								<td class="td_dark" style="padding:3px; font-weight: bold;" width="150">Created</td>
							</tr>
					</HeaderTemplate>
					<ItemTemplate>
							<tr><td colspan="7"><img src="../resources/images/t.gif" width="100%" height="1" /></td></tr>
							<tr class="normal" bgcolor="#EAF1DD">
							    <td style="padding:3px;" align="center" width="20"><asp:Checkbox id="_selector" runat="server" /></td>
								<td style="padding:3px;"><asp:Image id="_imagesIcon" imageurl="/_images/silk/images.png" runat="server" align="absmiddle" style="margin-right: 5px;" /><asp:HyperLink id="_titleLink" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_site" runat="server" /></td>
								<td style="padding:3px;" class="td_dark"><asp:Literal id="_created" runat="server" /></td>
							</tr>
					</ItemTemplate>
					<FooterTemplate>
							<tr><td colspan="7"><img src="../resources/images/t.gif" width="100%" height="1" /></td></tr>
							<tr class="normal" bgcolor="#E0E0E0" style="padding:3px;">
								<td style="padding: 5px; color: #9b9b9b; border-bottom: solid 1px #BCBCBC;" colspan="7">
									<asp:ImageButton imageurl="/tools/resources/images/btn_delete.gif" onclick="DeleteDocumentHandler" runat="server" />
								</td>
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
		<b>Key:</b><br /><br />
		<img src="/_images/silk/newspaper.png" align="absmiddle" alt="News" /> - News<br />
		<img src="/_images/silk/layout.png" align="absmiddle" alt="Article" /> - Article<br />
		<img src="/_images/silk/page_white.png" align="absmiddle" alt="Generic" /> - Generic<br />
		<img src="/_images/silk/page_white_link.png" align="absmiddle" alt="Generic, section default" /> - Generic, section default-document<br />
	</div>
</form>
<Controls:Footer id="_footer" runat="server"/>