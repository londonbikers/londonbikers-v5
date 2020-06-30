<%@ Page language="c#" Inherits="Tetron.Tools.Content.Windows.RelatedContainer" Codebehind="related-container.aspx.cs" %>
<html>
	<head>
		<link rel="stylesheet" href="../../resources/styles/tools.css" type="text/css">
	</head>
	<body bgcolor="#F5F5F5" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<div class="general" style="color: #4e4e4e; font-weight: bold; background-color: #eaeaea; border-top: 1px #d2d2d2 dotted; border-bottom: 1px #CCCCCC solid; padding: 5px; padding-top: 6px; margin-bottom: 10px;">
			<img src="/_images/silk/page_white_stack.png" width="16" height="16" align="absmiddle" /> Related Content
		</div>
		<table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<td align="center">
					<div class="bevel">
						<iframe id="_currentRelated" name="_currentRelated" runat="server" marginwidth="0" marginheight="0" frameBorder="0" scrolling="yes" width="100%" height="290" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="normal" style="padding-top: 10px; padding-bottom: 10px;">
					<form action="find-related.aspx" target="_findRelated" method="get">
						<input type="hidden" name="container" value="<%= Request.QueryString["container"] %>" />
						<input type="hidden" name="search" value="true" />
						<div style=""><b>Find Documents</b></div>
						Title:
						<input type="text" style="width:134px;" name="keyword" class="box" />
						Tag:
						<input type="text" style="width:134px;" name="tag" class="box" />
						<input type="checkbox" name="tuse" value="1"> <asp:Literal ID="_typeDropDown" Runat="server"/>
						<input type="image" src="../../resources/images/btn_find.gif" title="find content" align="texttop" />
					</form>
				</td>
			</tr>
			<tr>
				<td align="center">
					<div class="bevel">
						<iframe id="_findRelated" name="_findRelated" runat="server"  marginwidth="0" marginheight="0" frameBorder="0" scrolling="yes" width="100%" height="290" />
					</div>
				</td>
			</tr>
		</table>
	</body>
</html>