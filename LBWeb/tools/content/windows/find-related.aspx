<%@ Page language="c#" Inherits="Tetron.Tools.Content.Windows.find_related" Codebehind="find-related.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
	<head>
		<link rel="stylesheet" href="../../resources/styles/tools.css" type="text/css">
	</head>
	<body bgcolor="#EAEFDF" id="_body" runat="server">
		<form runat="server">
			<asp:Repeater ID="_results" Runat="server">
				<HeaderTemplate>
					<table border="0" cellpadding="0" cellspacing="5" width="98%" align="center">
						<tr>
							<td>
								<span class="darktext"><%= _prompt %></span>
							</td>
						</tr>
						<tr>
							<td><img src="../../resources/images/1x1-dark-green.gif" width="100%" height="1" /></td>
						</tr>
				</HeaderTemplate>
				<ItemTemplate>
						<tr>
							<td class="smalltext">
								<table border="0" cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td valign="top" width="25">
											<img src="/_images/silk/<%= Property("Icon") %>.png" width="16" height="16" />
										</td>
										<td class="darktext">
											<b><%= Property("Tag") %></b>:
											<b><%= Property("Title") %></b><br />
											<%= Property("Author") %><br />
											<font color="#717171"><%= Property("Created") %></font>
										</td>
										<td align="right" valign="top">
											<input type="checkbox" name="<%= Property("ID") %>" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
				</ItemTemplate>
				<SeparatorTemplate>
						<tr>
							<td><img src="../../resources/images/1x1-dark-green.gif" width="100%" height="1" /></td>
						</tr>
				</SeparatorTemplate>
				<FooterTemplate>
						<tr>
							<td><img src="../../resources/images/1x1-dark-green.gif" width="100%" height="1" /></td>
						</tr>
						<tr>
							<td align="right">
								<asp:ImageButton ID="_bottomSubmit" AlternateText="add selected related content" ImageUrl="../../resources/images/btn_add.gif" Runat="server" OnClick="AddRelated"/>
							</td>
						</tr>
					</table>
				</FooterTemplate>
			</asp:Repeater>
		</form>
	</body>
</html>