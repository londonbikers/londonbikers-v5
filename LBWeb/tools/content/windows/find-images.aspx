<%@ Page language="c#" Inherits="Tetron.Tools.Content.Windows.FindImages" Codebehind="find-images.aspx.cs" EnableEventValidation="false" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
	<head>
		<link rel="stylesheet" href="../../resources/styles/tools.css" type="text/css">
		<script language="javascript">
			function preview(url, w, h) {
				var popup = window.open(url,"preview","width="+w+",height="+h+",scrollbars=no,resizable=no,status=no,toolbar=no,menubar=no");
			}
		</script>
	</head>
	<body bgcolor="#EAEFDF" id="_body" runat="server">
		<form runat="server">
			<asp:Repeater ID="_results" Runat="server">
				<HeaderTemplate>
					<table border="0" cellpadding="0" cellspacing="5" width="98%" align="center">
						<tr>
							<td>
								<span class="darktext"><%= Prompt %></span>
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
										<td valign="top" width="66"><%= Property("Image") %></td>
										<td class="smalltext" valign="top" style="padding-left: 5px;">
											<b><%= Property("Name")%></b><br />
											<%= Property("Type") %>
										</td>
										<td align="right" valign="top"><input type="checkbox" name="<%= Property("ID") %>" /></td>
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
								<asp:ImageButton ID="_bottomSubmit" AlternateText="add selected images to document" ImageUrl="../../resources/images/btn_add.gif" Runat="server" OnClick="AddImages"/>
							</td>
						</tr>
					</table>
				</FooterTemplate>
			</asp:Repeater>
		</form>
	</body>
</html>