<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Admin.LogEntryPage" Codebehind="logentry.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<controls:header id="_header" runat="server"/>
	<h3>Log Entry</h3>
	<br />
	<b>ID:</b><br />
	<asp:Literal id="_id" runat="server" />
	<br />
	<br />
	<b>When:</b><br />
	<asp:Literal id="_when" runat="server" />
	<br />
	<br />
	<b>Context:</b><br />
	<asp:Literal id="_context" runat="server" />
	<br />
	<br />
	<b>Message:</b><br />
	<asp:Literal id="_message" runat="server" />
	<br />
	<br />
	<b>Stack Trace:</b><br />
	<asp:Literal id="_stackTrace" runat="server" />	
<controls:footer id="_footer" runat="server"/>