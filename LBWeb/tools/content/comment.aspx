<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Content.CommentPage" Codebehind="comment.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>

<Controls:Header id="_header" runat="server"/>
<form runat="server">
	<b>Member Comment</b>
	<div class="ErrorBox" id="_screenMessage" runat="server" style="margin-top: 10px;" visible="false" />
	<div class="SilverBox" style="margin-bottom: 10px; margin-top: 10px; height: 53px;">
		<div style="float: left;">
			<asp:Image id="_icon" runat="server" align="absmiddle" />
			<asp:HyperLink id="_mediaLink" runat="server" target="_blank" />
		</div>
		<div syle="float: right; text-align: right;" align="right">
			ID: <asp:Literal id="_id" runat="server" /> |
			Author: <img src="/_images/silk/user_green.png" align="absmiddle" /> <asp:HyperLink id="_authorLink" runat="server" /> |
			Created: <asp:Literal id="_created" runat="server" />
		</div>
		<hr class="thinsilver" style="margin-top: 10px; margin-bottom: 10px;" />
		<div style="float: left;">
			Report Status: <asp:DropDownList id="_reportStatus" runat="server" />
		</div>
		<div style="float: right;">
			<asp:ImageButton id="_deleteBtn" runat="server" OnClick="DeleteCommentHandler" ImageURL="/tools/resources/images/btn_delete.gif" tooltip="delete this comment!" />
		</div>
	</div>
	<div class="YellowBox">
		Comment:
		<asp:TextBox textmode="multiline" id="_commentText" runat="server" cssclass="TextBox" style="margin-top: 10px; width: 100%; height: 100px;" />
	</div>
	<div align="right" style="margin-top: 10px;">
		<asp:ImageButton runat="server" ImageURL="/tools/resources/images/btn_update.gif" OnClick="UpdateCommentHandler" />
	</div>
</form>
<Controls:Footer id="_footer" runat="server"/>