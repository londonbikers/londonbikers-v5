<%@ Page language="c#" Inherits="Tetron.Tools.Content.ImageImporter" Codebehind="image-importer.aspx.cs" %>
<%@ Register TagPrefix="Controls" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<html>
	<head>
		<title>Slideshow Image Importer</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" href="../resources/styles/tools.css" type="text/css">
	</head>
	<body bgcolor="#444444" style="margin:10px;" id="_body" runat="server">
		<table 
			border="0" 
			cellpadding="5" 
			cellspacing="0" 
			width="555" 
			style="border-style:solid; border-width: 1px; border-color: #cccccc;">
			<tr>
				<td style="background-color: #eeeeee;" class="normal">
					<div style="padding-left: 10px;">
						<b>import slideshow images</b>
					</div>
					<br />
					<Controls:UploadApplet 
						id="_uploader" 
						runat="server" 
						AppletLocation="../../_system/ActiveUpload.jar"
						License="F4165B29CA61E63FEFD862FCA76A9A9EA4DF3094174678783734185AD2EC6166CC57335BCCD4F4D0">
					</Controls:UploadApplet>
				</td>
			</tr>
		</table>
	</body>
</html>