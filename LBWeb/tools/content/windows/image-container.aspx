<%@ Page language="c#" Inherits="Tetron.Tools.Content.Windows.ImageContainer" Codebehind="image-container.aspx.cs" %>
<html>
	<head>
		<link rel="stylesheet" href="../../resources/styles/tools.css" type="text/css">
		<script language="javascript">
			function ShowImporter(){
				var res = window.open('../image-importer.aspx?container=<%= Request.QueryString["container"] %>','importerPopup','width=575,height=260,scrollbars=no,resizable=no,status=no,toolbar=no,menubar=no');	
			}
		</script>
	</head>
	<body bgcolor="#F5F5F5" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<div id="_importDiv" runat="server" class="general" style="height: 16px; background-color: #eaeaea; border-top: 1px #d2d2d2 dotted; border-bottom: 1px #CCCCCC solid; padding: 5px; padding-top: 6px; margin-bottom: 10px;">
			<div style="float: left; font-weight: bold; color: #4e4e4e;">
				<img src="/_images/silk/images.png" align="absmiddle" />
				Images
			</div>
			<div style="float: right;">
				<a class="bluelink" href="#" onclick="ShowImporter();">import slideshow</a> &laquo;
			</div>
		</div>
		<table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<td align="center">
					<div class="bevel">
						<iframe id="_currentImages" name="_currentImages" runat="server" marginwidth="0" marginheight="0" frameBorder="0" scrolling="yes" width="100%" height="290" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="normal" style="padding-top: 10px; padding-bottom: 10px;">
					<form action="find-images.aspx" target="_findImages" method="get">
						<input type="hidden" name="container" value="<%= Request.QueryString["container"] %>" />
						<input type="hidden" name="search" value="true" />
						Find Images:<br />
						<input type="text" style="margin-top: 5px; width:134px;" name="keyword" class="box">
						<input type="image" src="../../resources/images/btn_find.gif" align="absmiddle" title="find images" style="margin-bottom: 3px;"/>
						<br />
					</form>
				</td>
			</tr>
			<tr>
				<td align="center">
					<div class="bevel">
						<iframe id="_findImages" name="_findImages" runat="server"  marginwidth="0" marginheight="0" frameBorder="0" scrolling="yes" width="100%" height="290" />
					</div>
				</td>
			</tr>
		</table>
	</body>
</html>