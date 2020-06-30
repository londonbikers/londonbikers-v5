<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Users.User" Codebehind="user.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<Controls:Header id="_header" runat="server"/>
<form runat="server">
	<table border="0" bgcolor="#BCBCBC" width="100%" cellpadding="1" cellspacing="0">
		<tr>
			<td>
				<table bgcolor="#F5F5F5" width="100%" cellpadding="5" cellspacing="0">
					<tr class="normal">
						<td width="10">
							<asp:Image id="_image" runat="server" />
						</td>
						<td valign="top" width="75" align="right" style="padding-right:10px; line-height: 17px;">
							Username:<br />
							Name:<br />
							Joined:<br /><br />
							Status:
						</td>
						<td valign="top" style="line-height: 17px;">
							<b><asp:Literal id="_username" runat="server" /></b><br />
							<asp:Literal id="_name" runat="server" /><br />
							<asp:Literal id="_created" runat="server" /><br /><br />
							<asp:DropDownList id="_status" runat="server" autopostback="true" OnSelectedIndexChanged="HandleUserStatusChange" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br />
	<table border="0" bgcolor="#DFDFDB" width="100%" cellpadding="1" cellspacing="0">
		<tr>
			<td>
				<table bgcolor="#FFFFF1" width="100%" cellpadding="5" cellspacing="0">
					<tr class="normal">
						<td>
							<table width="100%">
								<tr class="normal">
									<td align="right" style="padding-right: 5px;">
										Username:
									</td>
									<td>
										<asp:TextBox id="_editUsername" runat="server" cssclass="box" style="width:150px;" />
									</td>
								</tr>
								<tr class="normal">
									<td align="right" style="padding-right: 5px;">
										First Name:
									</td>
									<td>
										<asp:TextBox id="_editFirstname" runat="server" cssclass="box" style="width:150px;" />
									</td>
								</tr>
								<tr class="normal">
									<td align="right" style="padding-right: 5px;">
										Last name:
									</td>
									<td>
										<asp:TextBox id="_editLastname" runat="server" cssclass="box" style="width:150px;" />
									</td>
								</tr>
								<tr class="normal">
									<td align="right" style="padding-right: 5px;">
										Email:
									</td>
									<td>
										<asp:TextBox id="_editEmail" runat="server" cssclass="box" style="width:150px;" />
									</td>
								</tr>
								<tr class="normal">
									<td align="right" style="padding-right: 5px;">
										New Password:
									</td>
									<td>
										<asp:TextBox id="_editPassword" runat="server" cssclass="box" style="width:150px;" />
									</td>
								</tr>
							</table>
							<br />
							<asp:ImageButton id="_updateDetailsBtn" imageurl="../resources/images/btn_update.gif" runat="server" onclick="HandleUserDetailsUpdate" />
							<span class="alert"><asp:Literal id="_prompt" runat="server" /></span>
						</td>
						<td valign="top" style="padding-left: 10px;">
							<table>
								<tr>
									<td>
										<asp:ListBox rows="8" multilinemode="true" style="width:150px;" id="_availableRoles" runat="server" />
									</td>
									<td>
										<asp:Button id="_roleAdditionBtn" runat="server" text=">>" onclick="HandleRoleAddition" /><br />
										<asp:Button id="_roleRemovalBtn" runat="server" text="<<" onclick="HandleRoleRemoval" />
									</td>
									<td>
										<table border="0" bgcolor="#DFDFDB" width="100%" cellpadding="1" cellspacing="0">
											<tr>
												<td>
													<table border="0" bgcolor="#F5F5F5" width="100%" cellpadding="5" cellspacing="0">
														<tr class="normal">
															<td>
																<asp:ListBox rows="8" multilinemode="true" style="width:150px;" id="_userRoles" runat="server" />
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</form>
<Controls:Footer id="_footer" runat="server"/>