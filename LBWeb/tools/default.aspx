<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.DefaultPage" Codebehind="default.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="resources/controls/footer.ascx" %>
<Controls:Header id="_header" runat="server"/>
<asp:Literal id="_salutation" runat="server"/>
<Controls:Footer id="_footer" runat="server"/>