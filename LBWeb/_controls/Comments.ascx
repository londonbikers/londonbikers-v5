<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Comments.ascx.cs" Inherits="Tetron.Controls.Comments" %>
<form runat="server">
    <a name="comments"></a>
    <img src="/_images/big-comments.png" width="28" height="24" alt="" class="floatLeft mr10" />
    <h2><asp:Literal ID="_commentsTitle" runat="server" /></h2>
    <div class="clear"></div>
    <div id="_responseBox" runat="server" class="ResponseBox mt10" visible="false" />
    <asp:Placeholder ID="_anonView" runat="server">
        <div class="standoutDiv mt10">
            <h3>Leave Your Comment!</h3>
            <div class="mt10">
                &raquo; <b><a href="/register/">Register now</a></b> to leave comments! It only takes a few seconds...
            </div>   
        </div>
    </asp:Placeholder>
    <asp:Placeholder ID="_memberView" runat="server">
        <div class="standoutDiv mt10">
            <h4>Leave Your Comment:</h4>
            <asp:TextBox TextMode="MultiLine" ID="_comment" runat="server" class="CommentBox" />
            <div class="floatLeft mt10 smallText">
                <asp:CheckBox ID="_receiveNotifications" runat="server" Checked="true" Text=" Get Reply Notifications?" />
            </div>
            <div class="floatRight mt10">
                <asp:Button Text="post comment" runat="server" OnClick="PostCommentHandler" CssClass="Tight" />
            </div>
            <div class="clear"></div>
        </div>        
    </asp:Placeholder>
    <div class="mt20">
        <asp:Repeater 
	        id="_commentsTable" 
	        runat="server" 
	        OnItemCreated="CommentItemCreatedHandler">
	        <ItemTemplate>
                <div class="mb5 commentBreak">
                    <asp:HyperLink ID="_avatarLink" runat="server"><asp:Image ID="_avatarImage" runat="server" class="floatLeft mr10 commentAv" /></asp:HyperLink>
                    <b><asp:HyperLink ID="_authorTextLink" runat="server" /></b>
                    <span class="subText">
                        | <asp:PlaceHolder ID="_reportLinkArea" runat="server">
                            (<asp:LinkButton ID="_reportLink" runat="server" class="FlatDark" Text="report" OnCommand="ReportCommentHandler" />)
                        </asp:PlaceHolder>
                        <asp:Literal ID="_posted" runat="server" />
                    </span>
                    <div class="mt10"><asp:Literal ID="_commentText" runat="server" /></div>
                    <div class="clear"></div>
                    <asp:PlaceHolder ID="_hr" runat="server" Visible="false">
                        <hr class="light" />
                    </asp:PlaceHolder>
                </div>
	        </ItemTemplate>
        </asp:Repeater>
    </div>
</form>