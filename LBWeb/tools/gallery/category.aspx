<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Gallery.Category" Codebehind="category.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>

<controls:header id="_header" runat="server"/>
<form runat="server">
	<b><asp:Literal id="_pageMode" runat="server" /> category</b>
	<br />
	<br />
	Name:<br />
	<asp:TextBox id="_name" cssclass="box" style="width: 100%" runat="server" />
	<br />
	<br />
	Description:<br />
	<asp:TextBox id="_description" textmode="multiline" cssclass="box" style="width: 100%; height:150px;" runat="server" />
	<br />
	<br />
	<asp:Button runat="server" onclick="PersistCategory" text="submit" cssclass="btn" />
	<br />
	<br />
	<asp:Label id="_prompt" runat="server" cssclass="prompt" />	
</form>
<controls:footer id="_footer" runat="server"/>