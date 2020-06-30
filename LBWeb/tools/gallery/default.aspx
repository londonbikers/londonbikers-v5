<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Gallery.DefaultPage" Codebehind="default.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>

<controls:header id="_header" runat="server"/>
<h1>Gallery Management</h1>
<form runat="server">
	Select a site below to manage the internal categories and individual galleries for that site.
	<br />
	<br />
	<div class="LightNoteBox">
		Site:
		<asp:DropDownList 
			id="_siteList" 
			runat="server" 
			AutoPostBack="true"
			OnSelectedIndexChanged="SiteChangedHandler" />
	</div>
	<br />
	<br />
	Options:
	<div style="margin-top: 1px; margin-bottom: 20px;">
		<img src="/_images/silk/folder_add.png" style="vertical-align: middle;" />
		<asp:HyperLink id="_newCategoryLink" runat="server" CssClass="GreyLink">new category</asp:HyperLink>
		<hr class="ThinSilver" />
	</div>
	<asp:Label id="_prompt" runat="server" cssclass="prompt" />	
	<asp:TreeView id="_treeView" runat="server" />
</form>
<controls:footer id="_footer" runat="server"/>