<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Tetron.Default" %>
<%@ Register TagPrefix="Controls" TagName="MemberStats" Src="_controls/HomepageMemberStats.ascx" %>
<%@ Register TagPrefix="Controls" TagName="RoadWatchList" Src="_controls/RoadWatchList.ascx" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="/css/homepage.css" />
    <script type="text/javascript" src="/js/slider.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {		
	        slideShow(3000);
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="topStories" class="boxHeading">top stories</div>
    <div id="topStoriesTail" class="boxTail">being read right now</div>
    <div id="editorsChoice" class="boxHeading">editors choice</div>
    <div id="editorsChoiceTail" class="boxTail"></div>
    <div class="rowEnd clear"></div>
    <div id="storyScroller">
        <div id="tcv">
            <asp:Repeater 
			    id="_topDocsImageList" 
			    runat="server" 
			    OnItemCreated="TopDocsImageCreatedHandler">
                <HeaderTemplate>
                    <ul class="slideshow">
                </HeaderTemplate>
			    <ItemTemplate>
                    <asp:Literal ID="_image" runat="server" />
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
		    </asp:Repeater>
        </div>
    </div>
    <div id="ecContent">
        <asp:Repeater 
			id="_editorsChoice" 
			runat="server" 
			OnItemCreated="EditorsChoiceItemCreatedHandler">
			<ItemTemplate>
                <asp:HyperLink ID="_ecImg" runat="server" cssclass="ecImg floatLeft" ClientIDMode="Static" />
                <div class="ecTitle"><asp:HyperLink ID="_ecLink" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_ecComments" runat="server" /> comments</div></asp:Placeholder>
                <div class="clear"></div>
            </ItemTemplate>
		</asp:Repeater>
    </div>
    <div class="clear"></div>
    <div id="row2Div" class="hDiv1"></div>
    <div id="latestStoriesArea">
        <div id="latestStoriesHead">
            <div id="latestStories" class="boxHeading">latest stories</div>
            <div id="latestStoriesTail" class="boxTail">across the board</div>
        </div>
		<div class="mb10 pl5">
			Find <a href="http://www.bennetts.co.uk/Bike-Insurance/">bike insurance</a> that suits your bike needs.
		</div>
        <div class="latestStoriesCol">
            <div class="boxHead boxHead1 mb5 rounded"><a href="/tags/motorcycles">motorcycles</a></div>
            <asp:Repeater 
			    id="_motorcyclesDocs" 
			    runat="server" 
			    OnItemCreated="DocumentItemCreatedHandler">
			    <ItemTemplate>
                    <div class="clear mb5">
                        <div class="storyImgPlaceholder"><asp:HyperLink ID="_img" runat="server" ClientIDMode="Static" /></div>
                        <div class="storyTitle"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                        <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_comments" runat="server" /> comments</div></asp:Placeholder>
                        <div class="clear"></div>
                    </div>
                </ItemTemplate>
		    </asp:Repeater>
        </div>
        <div class="latestStoriesCol">
            <div class="boxHead boxHead2 mb5 rounded"><a href="/tags/products">products</a></div>
            <asp:Repeater 
			    id="_productsDocs" 
			    runat="server" 
			    OnItemCreated="DocumentItemCreatedHandler">
			    <ItemTemplate>
                    <div class="clear mb5">
                        <div class="storyImgPlaceholder"><asp:HyperLink ID="_img" runat="server" ClientIDMode="Static" /></div>
                        <div class="storyTitle"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                        <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_comments" runat="server" /> comments</div></asp:Placeholder>
                        <div class="clear"></div>
                    </div>
                </ItemTemplate>
		    </asp:Repeater>
        </div>
        <div class="clear h10"></div>
        <div class="latestStoriesCol">
            <div class="boxHead boxHead3 mb5 rounded"><a href="/tags/clothing">clothing</a></div>
            <asp:Repeater 
			    id="_clothingDocs" 
			    runat="server" 
			    OnItemCreated="DocumentItemCreatedHandler">
			    <ItemTemplate>
                    <div class="clear mb5">
                        <div class="storyImgPlaceholder"><asp:HyperLink ID="_img" runat="server" ClientIDMode="Static" /></div>
                        <div class="storyTitle"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                        <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_comments" runat="server" /> comments</div></asp:Placeholder>
                        <div class="clear"></div>
                    </div>
                </ItemTemplate>
		    </asp:Repeater>  
        </div>
        <div class="latestStoriesCol">
            <div class="boxHead boxHead4 mb5 rounded"><a href="/tags/london">london</a></div>
            <asp:Repeater 
			    id="_londonDocs" 
			    runat="server" 
			    OnItemCreated="DocumentItemCreatedHandler">
			    <ItemTemplate>
                    <div class="clear mb5">
                        <div class="storyImgPlaceholder"><asp:HyperLink ID="_img" runat="server" ClientIDMode="Static" /></div>
                        <div class="storyTitle"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                        <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_comments" runat="server" /> comments</div></asp:Placeholder>
                        <div class="clear"></div>
                    </div>
                </ItemTemplate>
		    </asp:Repeater>        
        </div>
        <div class="clear"></div>
    </div>
    <div id="photoGalleryArea">
        <div id="photoGalleries" class="boxHeading">photo galleries</div>
        <div id="photoGalleryTail" class="boxTail"></div>
        <div id="photosFrame">
            <asp:Repeater 
				id="_latestGalleries" 
				runat="server" 
				OnItemCreated="LatestGalleriesItemCreatedHandler">
				<ItemTemplate>
                    <div class="boxgrid caption<asp:Literal id="_marginCss" runat="server" />">
                        <asp:HyperLink ID="_imageLink" runat="server" />
				        <div class="cover boxcaption">
					        <h3><asp:Literal ID="_title" runat="server" /></h3>
                            <asp:Literal ID="_category" runat="server" />: <asp:Literal ID="_numOfPhotos" runat="server" /> photos
				        </div>
			        </div>
                </ItemTemplate>
			</asp:Repeater>
        </div>
    </div>
    <div class="clear"></div>
    <div id="row3Div" class="hDiv2"></div>
    <div id="features" class="boxHeading">reviews &amp; features</div>
    <div id="featuresTail" class="boxTail"></div>
    <div id="featuresBg">
        <div class="clear"></div>
        <asp:Repeater 
			id="_features" 
			runat="server" 
			OnItemCreated="FeaturesCreatedHandler">
			<ItemTemplate>
                <div class="floatLeft featureboxgrid caption mr5">
                    <asp:Literal ID="_imageLink" runat="server" />
                    <div class="featureCover boxcaption">
                        <div class="fecaption">
                            <asp:Literal ID="_title" runat="server" /><br />
                            <asp:Literal ID="_tag" runat="server" />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
		</asp:Repeater>
        <div class="clear"></div>
    </div> 
    <div id="row4Div" class="hDiv3 mt10"></div>
    <div id="moreStories" class="boxHeading">more stories</div>
    <div id="moreStoriesTail" class="boxTail"></div>
    <div class="boxDesc mb5">
        The most popular stories lately. Drop into a subject to see tons more.
    </div>
    <div class="moreStoriesCol">
        <div class="boxHead boxHead5 mb5 rounded"><a href="/tags/bsb">british superbikes</a></div>
        <asp:Repeater 
			id="_bsbDocs" 
			runat="server" 
			OnItemCreated="DocumentItemCreatedHandler">
			<ItemTemplate>
                <div class="clear mb5">
                    <div class="storyImgPlaceholder"><asp:HyperLink ID="_img" runat="server" ClientIDMode="Static" /></div>
                    <div class="storyTitle"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                    <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_comments" runat="server" /> comments</div></asp:Placeholder>
                    <div class="clear"></div>
                </div>
            </ItemTemplate>
		</asp:Repeater> 
    </div>
    <div class="moreStoriesCol">
        <div class="boxHead boxHead6 mb5 rounded"><a href="/tags/wsb">world superbikes</a></div>
        <asp:Repeater 
			id="_wsbDocs" 
			runat="server" 
			OnItemCreated="DocumentItemCreatedHandler">
			<ItemTemplate>
                <div class="clear mb5">
                    <div class="storyImgPlaceholder"><asp:HyperLink ID="_img" runat="server" ClientIDMode="Static" /></div>
                    <div class="storyTitle"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                    <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_comments" runat="server" /> comments</div></asp:Placeholder>
                    <div class="clear"></div>
                </div>
            </ItemTemplate>
		</asp:Repeater>        
    </div>
    <div class="moreStoriesCol">
        <div class="boxHead boxHead7 mb5 rounded"><a href="/tags/motogp">motogp</a></div>
        <asp:Repeater 
			id="_motogpDocs" 
			runat="server" 
			OnItemCreated="DocumentItemCreatedHandler">
			<ItemTemplate>
                <div class="clear mb5">
                    <div class="storyImgPlaceholder"><asp:HyperLink ID="_img" runat="server" ClientIDMode="Static" /></div>
                    <div class="storyTitle"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                    <asp:Placeholder ID="_commentsPanel" runat="server"><div class="commentStrip"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_comments" runat="server" /> comments</div></asp:Placeholder>
                    <div class="clear"></div>
                </div>
            </ItemTemplate>
		</asp:Repeater>  
    </div>
    <div class="clear"></div>
    <div id="row5Div" class="hDiv4 mt10"></div>
    <div id="social" class="boxHeading">community &amp; events</div>
    <div id="socialTail" class="boxTail"></div>
    <div id="roadWatch" class="floatLeft">
        <div id="roadWatchHead" class="boxSubHead">road watch</div>
        <Controls:RoadWatchList runat="server" />
    </div>
    <div id="community" class="floatLeft">
        <div id="communityHead" class="boxSubHead">community info</div>
        <Controls:MemberStats runat="server" />
    </div>
    <div id="events" class="floatLeft">
        <div id="eventsHead" class="boxSubHead">upcoming events</div>
        <asp:Repeater 
			id="_latestEvents" 
			runat="server" 
			OnItemCreated="EventsItemCreatedHandler">
			<ItemTemplate>
                <div class="roadWatchItem"><asp:HyperLink id="_eventLink" runat="server" class="dark" target="_blank" ClientIDMode="Static" /></div>
			</ItemTemplate>
		</asp:Repeater>
    </div>
    <div class="clear h10"></div>
</asp:Content>