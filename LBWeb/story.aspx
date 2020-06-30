<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="story.aspx.cs" Inherits="Tetron.Story" %>
<%@ Register TagPrefix="Controls" TagName="Comments" Src="~/_controls/Comments.ascx" %>
<%@ Register TagPrefix="Controls" TagName="FacebookLike" Src="~/_controls/FacebookLike.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/document.css" />
    <link rel="stylesheet" href="/css/prettyPhoto.css" />
    <script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script>
    <script type="text/javascript" src="https://apis.google.com/js/plusone.js">
        { lang: 'en-GB' }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Literal ID="_introImage" runat="server" />
    <div class="padded">
        <h1><asp:Literal id="_title" runat="server" /></h1>
        <div class="floatLeft">
            <Controls:FacebookLike runat="server" />
        </div>
        <div class="floatRight tright twit"><a href="http://twitter.com/share" class="twitter-share-button" data-count="horizontal" data-via="londonbikers">Tweet</a></div>
        <div id="buzzContainer" class="floatRight"><g:plusone size="medium"></g:plusone></div>
        <div class="clear"></div>
        <hr class="hr3" />
        <div class="subTitled" id="subTitle">
            By: <asp:Literal ID="_author" runat="server" /> | 
            Published <asp:Literal ID="_pubDate" runat="server" /> <asp:Literal ID="_views" runat="server" /> | tags: <asp:Literal ID="_tags" runat="server" />
        </div>
        <div id="editorial">
            <asp:PlaceHolder ID="_otherContentArea" runat="server">
                <div id="otherStories">
                    <asp:PlaceHolder ID="_popularDocsArea" runat="server">
                        <div class="relatedHead">
                            Top Stories Right Now
                        </div>
                        <div class="padded5">
                            <asp:Repeater 
			                    id="_popularStories" 
			                    runat="server" 
			                    OnItemCreated="RelatedDocumentItemCreatedHandler">
			                    <ItemTemplate>
                                    <div class="clear mb5">
                                        <div class="similarStoryImg rounded"><asp:HyperLink ID="_img" runat="server" ClientIDMode="Static" /></div>
                                        <div class="relStory"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                                        <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.gif" width="16" height="16" alt="" /> <asp:Literal ID="_comments" runat="server" /> comments</div></asp:Placeholder>
                                        <div class="clear"></div>
                                    </div>
                                </ItemTemplate>
		                    </asp:Repeater> 
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="_relatedDocsArea" runat="server">
                        <div class="relatedHead" id="relatedStoriesHead">
                            Relates Stories
                        </div>
                        <div class="padded5">
                            <asp:Repeater 
			                    id="_relatedDocs" 
			                    runat="server" 
			                    OnItemCreated="RelatedDocumentItemCreatedHandler">
			                    <ItemTemplate>
                                    <div class="clear mb5">
                                        <div class="similarStoryImg rounded"><asp:HyperLink ID="_img" runat="server" ClientIDMode="Static" /></div>
                                        <div class="relStory"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                                        <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.gif" width="16" height="16" alt="" /> <asp:Literal ID="_comments" runat="server" /> comments</div></asp:Placeholder>
                                        <div class="clear"></div>
                                    </div>
                                </ItemTemplate>
		                    </asp:Repeater> 
                        </div>
                    </asp:PlaceHolder>
                </div>
            </asp:PlaceHolder>
            <asp:Literal ID="_coverImage" runat="server" />
            <asp:PlaceHolder ID="_prPrefix" Visible="false" runat="server">
                <div class="mb20"><i>Press Release:</i></div>
            </asp:PlaceHolder>
            <div id="firstParagraph"><asp:Literal ID="_firstPara" runat="server" Visible="true" /></div>
            <asp:Literal ID="_body" runat="server" Visible="true" />
            <div class="clear"></div>
            <asp:PlaceHolder ID="_photosArea" runat="server" Visible="false">
                <div id="photosOpening" class="hDiv3 mt20"></div>
                <div class="mb5">
                    <div id="photosLabel" class="boxHeading">Photos</div>
                    <div id="photosTail" class="boxTail">click to zoom</div>
                </div>
                <div id="hdPhotos">
                    <asp:Repeater ID="_photos" runat="server" OnItemCreated="PhotoItemCreatedHandler">
                        <ItemTemplate><asp:Literal ID="_image" runat="server" /></ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:PlaceHolder>
            <hr class="hr3" />
            <Controls:Comments id="_comments" runat="server" />
        </div>
    </div>
</asp:Content>