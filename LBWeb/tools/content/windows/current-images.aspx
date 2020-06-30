<%@ Page language="c#" Inherits="Tetron.Tools.Content.Windows.current_images" Codebehind="current-images.aspx.cs" EnableEventValidation="false" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
	<head>
		<link rel="stylesheet" href="../../resources/styles/tools.css" type="text/css">
		<script language="javascript">
			function SelectCImage(index)
			{
				document.forms[0]._coverImageIndex.value = index;
				document.forms[0].submit();
			}
			function SelectIImage(index)
			{
				document.forms[0]._introImageIndex.value = index;
				document.forms[0].submit();
			}
			function preview(url, w, h)
			{
				var popup = window.open(url,"preview","width="+w+",height="+h+",scrollbars=no,resizable=no,status=no,toolbar=no,menubar=no");
			}
		</script>
	</head>
	<body bgcolor="#EAEFDF">
		<form runat="server">
			<input type="hidden" name="_coverImageIndex" value="" />
			<input type="hidden" name="_introImageIndex" value="" />
			&nbsp;&nbsp;<span class="darktext">current images:</span>
			<asp:Label ID="_prompt" CssClass="darktext" Runat="server"/>
			<asp:Repeater ID="_currentImages" Runat="server">
				<HeaderTemplate>
					<table border="0" cellpadding="0" cellspacing="0" width="98%" align="center">
						<tr>
							<td><img src="../../resources/images/1x1-dark-green.gif" width="100%" height="1" /></td>
						</tr>
				</HeaderTemplate>
				<ItemTemplate>
						<tr>
							<td class="smalltext" style="padding-bottom: 5px; padding-top: 5px;">
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td valign="top" width="66"><%= Property("Image") %></td>
										<td class="smalltext" valign="top">
											<table border="0" cellpadding="0" cellspacing="0" width="100%">
												<tr>
													<td class="smalltext" width="60">
														<input type="checkbox" <%= Property("CoverChecked") %> onclick="SelectCImage('<%= Property("Index") %>')" value="1" name="cimage" />cover<br />
														<input type="checkbox" <%= Property("IntroChecked") %> onclick="SelectIImage('<%= Property("Index") %>')" value="1" name="iimage" />intro
													</td>
													<td class="smalltext" valign="top">
														&nbsp;<b><%= Property("Name")%></b><br />
														&nbsp;<%= Property("Type") %>
													</td>
													<td width="15" valign="top">
														<input type="checkbox" name="<%= Property("ID") %>" />
													</td>
												</tr>
											</table>
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
							<td align="right" style="padding-top: 5px;">
								<asp:ImageButton ID="_bottomSubmit" AlternateText="remove selected images..." ImageUrl="../../resources/images/btn_remove.gif" Runat="server" OnClick="RemoveImages"/>
							</td>
						</tr>
					</table>
				</FooterTemplate>
			</asp:Repeater>
		</form>
	</body>
</html>