<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Content.images" Codebehind="images.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<Controls:Header id="_header" runat="server"/>
<script language="javascript">
	function imagePopup()
	{
		var res = window.open('image.aspx','imagePopup','width=320,height=224,scrollbars=no,resizable=no,status=no,toolbar=no,menubar=no');	
	}
	function preview(url, w, h)
	{
		var popup = window.open(url,"preview","width="+w+",height="+h+",scrollbars=no,resizable=no,status=no,toolbar=no,menubar=no");
	}
</script>
<form runat="server">
	options | <a href="javascript:imagePopup();">new image</a>
	<br />
	<br />
	<table border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<table border="0" bgcolor="#F4F2F2" cellpadding="4" cellspacing="0" class="tableContainer" width="100%">
					<tr class="normal">
						<td class="normal" width="195">
							Keyword: <asp:Textbox class="box" id="_keyword" runat="server"/>&nbsp;&nbsp;&nbsp;
							Type: <asp:Checkbox id="_byType" runat="server" /> <asp:DropDownList onchange="_byType.checked=true;" id="_type" runat="server"/>
							<asp:ImageButton id="_findbtn" src="../resources/images/btn_find.gif" title="find images" runat="server" onclick="FindImagesEvnt" />
						</td>
					</tr>
				</table>
				<br />
				<table border="0" cellpadding="1" cellspacing="0">
					<tr>
						<td bgcolor="#cccccc">
							<table width="100%" bgcolor="#F4F2F2" cellspacing="0" cellpadding="0">
								<tr>
									<td style="padding:4px;" class="normal" colspan="5"><asp:Literal id="_prompt" runat="server"/></td>
								</tr>
								<tr>
									<td colspan="5" bgcolor="#cccccc"><img src="../resources/images/t.gif" width="100%" height="1" /></td>
								</tr>
								<asp:Repeater id="_grid" runat="server">
									<ItemTemplate>
										<tr class="normal" bgcolor="#EAF1DD">
											<td valign="top" width="80" style="padding:6px;">
												<%= Property("Image") %><br />
												<b><%= Property("Name") %></b><br />
												<%= Property("Type") %><br />
												<%= Property("Dimensions") %><br />
												<%= Property("DeleteLink") %>
											</td>
											<td valign="top" width="80" style="padding:6px;">
												<%= Property("Image") %><br />
												<b><%= Property("Name") %></b><br />
												<%= Property("Type") %><br />
												<%= Property("Dimensions") %><br />
												<%= Property("DeleteLink") %>
											</td>
											<td valign="top" width="80" style="padding:6px;">
												<%= Property("Image") %><br />
												<b><%= Property("Name") %></b><br />
												<%= Property("Type") %><br />
												<%= Property("Dimensions") %><br />
												<%= Property("DeleteLink") %>
											</td>
											<td valign="top" width="80" style="padding:6px;">
												<%= Property("Image") %><br />
												<b><%= Property("Name") %></b><br />
												<%= Property("Type") %><br />
												<%= Property("Dimensions") %><br />
												<%= Property("DeleteLink") %>
											</td>
											<td valign="top" width="80" style="padding:6px;">
												<%= Property("Image") %><br />
												<b><%= Property("Name") %></b><br />
												<%= Property("Type") %><br />
												<%= Property("Dimensions") %><br />
												<%= Property("DeleteLink") %>
											</td>
										</tr>
										<tr>
											<td colspan="5" bgcolor="#cccccc"><img src="../resources/images/t.gif" width="100%" height="1" /></td>
										</tr>
									</ItemTemplate>
								</asp:Repeater>
								<tr>
									<td colspan="5">&nbsp;</td>
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