<%@ Page language="c#" Inherits="Tetron.Tools.Content.Windows.TagContainer" Codebehind="tag-container.aspx.cs" %>
<html>
	<head>
		<link rel="stylesheet" href="/tools/resources/styles/tools.css" type="text/css">
		<script language="javascript">
			function ShowImporter(){
				var res = window.open('../image-importer.aspx?container=<%= Request.QueryString["container"] %>','importerPopup','width=575,height=260,scrollbars=no,resizable=no,status=no,toolbar=no,menubar=no');	
			}
			function AddTag(tag){
				var tags = document.getElementById("_tags");
				if (tags.value != "")
					tags.value = tags.value + ", ";
				tags.value = tags.value + tag;
			}
		</script>
	</head>
	<body bgcolor="#F5F5F5" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form runat="server">
			<div class="general" style="color: #4e4e4e; font-weight: bold; background-color: #eaeaea; border-top: 1px #d2d2d2 dotted; border-bottom: 1px #CCCCCC solid; padding: 5px; padding-top: 6px; margin-bottom: 10px;">
				<img src="/_images/silk/tag_blue.png" width="16" height="16" align="absmiddle" /> Tags
			</div>
			<div class="GreenNoteBox">
				Documents need tags, they're keywords which describe the context of the document. They're also the way documents are connected to one-another and how documents 
				can be found when searched for. When adding tags, ensure that the first one is the primary top-level way of describing the piece, and if appropriate, one of the main
				tags used in the site navigation.			
			</div>
			<div class="NoteBox" style="text-align: left; margin-top: 10px; margin-bottom: 10px;">
				<div style="margin-bottom: 10px;">
					<b>Common Primary Tags:</b><br />
					<span class="lighttext">(click to add)</span>
				</div>
				<!-- manual for now -->
				<a href="javascript:AddTag('london');" class="bluelink">london</a>,
				<a href="javascript:AddTag('products');" class="bluelink">products</a>,
				<a href="javascript:AddTag('motorcycles');" class="bluelink">motorcycles</a>,
				<a href="javascript:AddTag('offroad');" class="bluelink">offroad</a>,
				<a href="javascript:AddTag('supermoto');" class="bluelink">supermoto</a>,
				<a href="javascript:AddTag('scooters');" class="bluelink">scooters</a>,
				<a href="javascript:AddTag('motogp');" class="bluelink">motogp</a>,
				<a href="javascript:AddTag('valentino rossi');" class="bluelink">valentino rossi</a>,
				<a href="javascript:AddTag('250gp');" class="bluelink">250gp</a>,
				<a href="javascript:AddTag('125gp');" class="bluelink">125gp</a>,
				<a href="javascript:AddTag('ama');" class="bluelink">ama</a>,
				<a href="javascript:AddTag('ama superbike');" class="bluelink">ama superbike</a>,
				<a href="javascript:AddTag('ama supercross');" class="bluelink">ama supercross</a>,
				<a href="javascript:AddTag('wsb');" class="bluelink">wsb</a>,
				<a href="javascript:AddTag('sbk');" class="bluelink">sbk</a>,
				<a href="javascript:AddTag('wss');" class="bluelink">wss</a>,
				<a href="javascript:AddTag('superstock 1000 fim cup');" class="bluelink">superstock 1000 fim cup</a>,
				<a href="javascript:AddTag('superstock 600 european championship');" class="bluelink">superstock 600 european championship</a>,
				<a href="javascript:AddTag('bsb');" class="bluelink">bsb</a>,
				<a href="javascript:AddTag('superbikes');" class="bluelink">superbikes</a>,
				<a href="javascript:AddTag('metzeler racetec national superstock');" class="bluelink">metzeler racetec national superstock</a>,
				<a href="javascript:AddTag('bss');" class="bluelink">bss</a>,
				<a href="javascript:AddTag('british gp125');" class="bluelink">british gp125</a>,
				<a href="javascript:AddTag('mx');" class="bluelink">mx</a>,
				<a href="javascript:AddTag('honda');" class="bluelink">honda</a>,
				<a href="javascript:AddTag('suzuki');" class="bluelink">suzuki</a>,
				<a href="javascript:AddTag('yamaha');" class="bluelink">yamaha</a>,
				<a href="javascript:AddTag('kawasaki');" class="bluelink">kawasaki</a>,
				<a href="javascript:AddTag('ducati');" class="bluelink">ducati</a>,
				<a href="javascript:AddTag('bike shows');" class="bluelink">bike shows</a>
				<!-- end popular tags -->
			</div>
			<div class="NoteBox" style="text-align: left;">
				<b>Assigned Tags:</b><br />
				<span class="lighttext">(seperate with a comma, case is irrelevant)</span>
				<asp:TextBox ID="_tags" Runat="server" TextMode="MultiLine" style="width: 100%; margin-top: 5px;" Rows="3" />
			</div>
			<div class="GreyNoteBox" style="text-align: right; margin-top: 10px; padding: 5px;"><span class="prompt" style="font-size: 11px;"><asp:Literal ID="_prompt" Runat="server" /></span><asp:ImageButton align="absmiddle" style="margin-left: 10px;" OnClick="SaveTagsHandler" Runat="server" ImageUrl="/tools/resources/images/btn_save.gif" /></div>
		</form>
	</body>
</html>