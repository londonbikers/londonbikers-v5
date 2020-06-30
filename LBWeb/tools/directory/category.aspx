<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Directory.CategoryPage" Codebehind="category.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<controls:header id="_header" runat="server"/>
<form runat="server">
	<b>Directory Category</b>
	<br />
	<br />
	<table width="100%" bgcolor="#BCBCBC" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<table border="0" cellpadding="0" cellspacing="1" width="100%">
					<tr class="normal">
						<td class="TableBlankCell" colspan="5">
							<table width="100%" cellpadding="3" cellspacing="0" border="0">
								<tr>
									<td align="right" class="normal" width="130">
										Name:
									</td>
									<td>
										<asp:TextBox style="width: 200px;" id="_name" runat="server" cssclass="box" />
									</td>
								</tr>
								<tr>
									<td align="right" valign="top" class="normal">
										Description:
									</td>
									<td>
										<asp:TextBox style="width: 300px; height: 50px;" id="_description" runat="server" TextMode="MultiLine" cssclass="box" />
									</td>
								</tr>
								<tr>
									<td align="right" class="normal">
										Keywords:
									</td>
									<td class="normal">
										<asp:TextBox id="_keywords" style="width: 300px;" runat="server" cssclass="box" />
										(comma seperated)
									</td>
								</tr>
								<tr>
									<td align="right" class="normal">
										Membership Required?
									</td>
									<td>
										<asp:CheckBox id="_membershipRequired" runat="server" />
										<asp:Label id="_prompt" runat="server" cssclass="prompt" />	
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td class="TableHeaderCell">
							<asp:ImageButton runat="server" ImageURL="../resources/images/btn_update.gif" onclick="CategoryUpdateHandler" tooltip="update this category" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</form>
<controls:footer id="_footer" runat="server"/>