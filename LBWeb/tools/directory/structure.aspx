<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Directory.StructurePage" Codebehind="structure.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>

<controls:header id="_header" runat="server"/>
<form runat="server">
	<h1>Directory Structure</h1>
	<div style="margin-top: 1px; margin-bottom: 20px;">
		<img src="/_images/silk/folder_add.png" style="vertical-align: middle;" />
		<a href="category.aspx" class="GreyLink">new top-level category</a>
		<hr class="ThinSilver" />
	</div>
	<asp:Label id="_prompt" runat="server" cssclass="prompt" />	
	<asp:TreeView id="_treeView" runat="server" />
</form>
<controls:footer id="_footer" runat="server"/>