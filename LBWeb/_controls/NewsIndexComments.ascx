<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsIndexComments.ascx.cs" Inherits="Tetron.Controls.NewsIndexComments" %>
<%@ OutputCache Duration="10" VaryByControl="none" %>
<div id="commentsArea">
    <div class="hDiv4"></div>
    <div id="commentsHead" class="boxHeading">latest comments</div>
    <div id="commentsTail" class="boxTail"></div>
    <div id="commentsFrame">
        <table>
            <asp:Repeater 
				id="_comments" 
				runat="server" 
				OnItemCreated="CommentsHandler">
				<ItemTemplate>
                    <tr>
                        <td class="pb10 pr5 icn"><img src="/_images/comments.png" alt="" /></td>
                        <td class="pb10"><asp:Literal ID="_link" runat="server" /></td>
                    </tr>
                </ItemTemplate>
			</asp:Repeater>
        </table>
    </div>
</div>