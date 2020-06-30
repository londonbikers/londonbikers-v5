<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Users.DefaultPage" Codebehind="default.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<Controls:Header id="_header" runat="server"/>
<form runat="server">
	<table border="0" bgcolor="#BCBCBC" width="100%" cellpadding="1" cellspacing="0">
		<tr>
			<td>
				<table border="0" bgcolor="#EEECEC" width="100%" cellpadding="5" cellspacing="0">
					<tr class="normal">
						<td width="110">
							Name:
							<br />
							<asp:TextBox class="box" id="_name" runat="server"/>
						</td>
						<td width="110">
							Username:
							<br />
							<asp:TextBox class="box" id="_username" runat="server"/>
						</td>
						<td width="110">
							Email:
							<br />
							<asp:TextBox class="box" id="_email" runat="server"/>
						</td>
						<td width="110">
							Other:
							<br />
							<asp:DropDownList id="_other" runat="server">
								<asp:ListItem>-</asp:ListItem>
								<asp:ListItem>New Users</asp:ListItem>
								<asp:ListItem>Staff</asp:ListItem>
							</asp:DropDownList>
						</td>
						<td>
							<br />
							<asp:ImageButton id="_findbtn" src="../resources/images/btn_find.gif" title="find users" runat="server" onclick="HandleUserSearch" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br />
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="1" id="_resultsTable" runat="server">
		<tr>
			<td>
				<asp:Repeater id="_users" runat="server" onitemcreated="HandleResultItemCreation">
					<HeaderTemplate>
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr class="normal" bgcolor="#D4E0BE" style="padding:3px;">
								<td style="padding:3px;" width="20">
									&nbsp;
								</td>
								<td style="padding:3px;">
									<b>
										Name
									</b>
								</td>
								<td style="padding:3px;">
									<b>
										Username
									</b>
								</td>
								<td style="padding:3px;">
									<b>
										Email
									</b>
								</td>
								<td style="padding:3px;" width="150">
									<b>
										Created
									</b>
								</td>
								<td style="padding:3px;" width="65">
									&nbsp;
								</td>
							</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td colspan="6"><img src="../resources/images/t.gif" width="100%" height="1" /></td>
						</tr>
						<tr class="normal" bgcolor="#EAF1DD">
							<td style="padding:3px;" align="center" width="20">
								<asp:Image id="_resultIcon" runat="server" />
							</td>
							<td style="padding:3px;">
								<asp:Literal id="_resultName" runat="server" />
							</td>
							<td style="padding:3px;" class="td_dark">
								<asp:Literal id="_resultUsername" runat="server" />
							</td>
							<td style="padding:3px;" class="td_dark">
								<asp:HyperLink id="_resultEmail" runat="server" />
							</td>
							<td style="padding:3px;" class="td_dark">
								<asp:Literal id="_resultCreated" runat="server" />
							</td>
							<td style="padding:3px;" align="right">
								<asp:HyperLink id="_resultManageLink" runat="server" />
							</td>
						</tr>
					</ItemTemplate>
					<FooterTemplate>
						<tr>
							<td colspan="6"><img src="../resources/images/t.gif" width="100%" height="1" /></td>
						</tr>
						<tr bgcolor="#F4F2F2" style="padding:3px;">
							<td style="padding:3px;" colspan="6"><img src="../resources/images/t.gif" width="10" height="15" /></td>
						</tr>
					</FooterTemplate>
				</asp:Repeater>
			</td>
		</tr>	
	</table>
</form>
<Controls:Footer id="_footer" runat="server"/>