<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Content.Comments" Codebehind="comments.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>

<Controls:Header id="_header" runat="server"/>
<form runat="server">
	<b>Member Comments</b>
	<div class="ErrorBox" id="_screenMessage" runat="server" style="margin-top: 10px;" visible="false" />
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
		<tr>
			<td>
				<div style="background-color: #f4f2f2; padding: 5px;">
					<asp:Button id="_latestCommentsButton" runat="server" text="Latest Comments" cssclass="SoftBtn" onclick="LatestCommentsViewHandler" />
					<asp:Button id="_reportedCommentsButton" runat="server" text="Reported Comments" cssclass="SoftBtn" onclick="ReportedCommentsViewHandler" />
				</div>			
				<asp:Repeater 
					id="_grid" 
					runat="server"
					OnItemCreated="ItemCreatedHandler">
					<HeaderTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr><td colspan="7"><img src="../resources/images/t.gif" width="100%" height="1" /></td></tr>
							<tr class="normal" bgcolor="#D4E0BE" style="padding: 3px;">
								<td style="padding: 3px;" width="20">&nbsp;</td>
								<td class="td_dark" style="padding: 5px; font-weight: bold;">Media</td>
								<td class="td_dark" style="padding: 5px; font-weight: bold;">Author</td>
								<td class="td_dark" style="padding: 5px; font-weight: bold;">Created</td>
								<td class="td_dark" style="padding: 5px; font-weight: bold;">Status</td>
								<td class="td_dark" style="padding: 5px; font-weight: bold;">Comment</td>
								<td style="padding: 3px;">&nbsp;</td>
							</tr>
					</HeaderTemplate>
					<ItemTemplate>
							<tr><td colspan="7"><img src="../resources/images/t.gif" width="100%" height="1" /></td></tr>
							<tr class="normal" bgcolor="#EAF1DD">
								<td style="padding: 12px 3px 3px 3px;" align="center" valign="top" width="20"><asp:Image id="_icon" runat="server" /></td>
								<td style="padding: 12px 3px 3px 3px;" valign="top" nowrap="nowrap"><asp:HyperLink id="_mediaLink" runat="server" target="_blank" /></td>
								<td style="padding: 12px 3px 3px 3px;" class="td_dark" valign="top" nowrap="nowrap">
									<img src="/_images/silk/user_green.png" align="absmiddle" />
									<asp:HyperLink id="_authorLink" runat="server" />
								</td>
								<td style="padding: 12px 3px 3px 3px; width: 100px;" class="GridCellLowLight" valign="top"><asp:Literal id="_created" runat="server" /></td>
								<td style="padding: 12px 3px 3px 3px;" class="GridCellLowLight" valign="top"><asp:Literal id="_status" runat="server" /></td>
								<td style="padding: 3px;" class="td_dark" valign="top">
									<div style="padding: 5px; margin: 3px; border: dotted 1px #6d6d6d; background-color: #dfe6d3;">
										<asp:Literal id="_commentText" runat="server" />
									</div>
								</td>
								<td style="padding: 6px;" align="center" valign="middle" nowrap="nowrap">
									<asp:HyperLink id="_editLink" runat="server" ImageURL="/tools/resources/images/btn_edit.gif" />
								</td>
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
		<img src="/_images/silk/images.png" align="absmiddle" /> - Gallery<br />
		<img src="/_images/silk/image.png" align="absmiddle" /> - Gallery Image<br />
		<img src="/_images/silk/report.png" align="absmiddle" /> - Directory Item<br />
	</div>
</form>
<Controls:Footer id="_footer" runat="server"/>