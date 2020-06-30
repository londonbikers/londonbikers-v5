<%@ Page language="c#" Inherits="Tetron.Tools.Content.ImagePage" Codebehind="image.aspx.cs" %>
<html>
	<head>
		<title>New Image</title>
		<link rel="stylesheet" href="../resources/styles/tools.css" type="text/css">
	</head>
	<body id="_body" runat="server" bgcolor="#444444" style="margin:10px;">
		<form enctype="multipart/form-data" runat="server">
			<table style="border-style:solid; border-width:1px; border-color:#F7FDEB" border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td bgcolor="#C7D5AE" width="46" style="padding:8px;">
						&nbsp;
					</td>
					<td class="normal" bgcolor="#D4E0BE" style="padding:8px;">
						<asp:Label ID="_intro" Runat="server">
							Give your image a name and browse to the image file on your computer. Only web 
							image formats are allowed (.jpg, .jpe, .gif).
						</asp:Label>
						<asp:Label CssClass="alert" ID="_prompt" Runat="server" />
					</td>
				</tr>
				<tr>
					<td class="normal" bgcolor="#C7D5AE" width="46" style="padding:8px;">
						name:
					</td>
					<td bgcolor="#D4E0BE" style="padding:8px;">
						<asp:TextBox ID="_name" Runat="server" CssClass="box" style="width:100%" />
					</td>
				</tr>
				<tr>
					<td class="normal" bgcolor="#C7D5AE" width="46" style="padding:8px;">
						image:
					</td>
					<td bgcolor="#D4E0BE" style="padding:8px;">
						<input type="file" id="_file" class="box" style="width: 100%;" runat="server" NAME="_file"/>
					</td>
				</tr>
				<tr>
					<td class="normal" bgcolor="#C7D5AE" width="46" style="padding:8px;">
						type:
					</td>
					<td bgcolor="#D4E0BE" style="padding:8px;">
						<asp:DropDownList ID="_type" Runat="server" CssClass="box" />
					</td>
				</tr>
				<tr>
					<td style="padding:6px;" align="center" colspan="2" bgcolor="#7E856D">
					    <asp:ImageButton ID="_saveBtn" Runat="server" ImageUrl="../resources/images/btn_save_center.gif" OnClick="SaveImageEvnt" />
                    </td>
				</tr>
			</table>
		</form>
	</body>
</html>