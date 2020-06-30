<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tag.aspx.cs" Inherits="Tetron.TagPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/tag.css" />
    <link rel="stylesheet" href="/css/nivo-slider.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="titleRow">
        <h1><span class="lightText">Tag:</span> <asp:Literal ID="_title" runat="server" /></h1>
    </div>
    <asp:PlaceHolder ID="_topStoriesHolder" runat="server">
        <div id="topStories" class="boxHeading">top stories</div>
        <div id="topStoriesTail" class="boxTail">being read right now for <asp:Literal ID="_tagSubTitle" runat="server" /></div>
        <div class="rowEnd clear"></div>
        <div id="storyScroller">
            <div id="tcv" class="nivoSlider">
                <asp:Repeater 
			        id="_topDocsImageList" 
			        runat="server" 
			        OnItemCreated="TopDocsImageHandler">
			        <ItemTemplate><asp:Literal ID="_image" runat="server" />
                    </ItemTemplate>
		        </asp:Repeater>
            </div>
            <asp:Repeater 
			    id="_topDocsCaptionList"
			    runat="server" 
			    OnItemCreated="TopDocsCaptionHandler">
			    <ItemTemplate><asp:Literal ID="_caption" runat="server" /></ItemTemplate>
		    </asp:Repeater>
        </div>
        <div id="ecContent">
            <asp:Repeater 
			    id="_popularStoriesListing" 
			    runat="server" 
			    OnItemCreated="TopStoriesListingHandler">
			    <ItemTemplate>
                    <asp:HyperLink ID="_ecImg" runat="server" cssclass="ecImg floatLeft" ClientIDMode="Static" />
                    <div class="ecTitle"><asp:HyperLink ID="_ecLink" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                    <div class="clear"></div>
                    <asp:Literal ID="_hr" runat="server" Visible="false" />                
                </ItemTemplate>
		    </asp:Repeater>
        </div>
        <div class="clear"></div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="_featuresHolder" runat="server">
        <div class="hDiv3 mt10"></div>
        <div id="features" class="boxHeading">reviews &amp; features</div>
        <div id="featuresTail" class="boxTail"></div>
        <div id="featuresBg">
            <div class="clear"></div>
            <asp:Repeater 
			    id="_features" 
			    runat="server" 
			    OnItemCreated="FeaturesHandler">
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
    </asp:PlaceHolder>
    <div class="hDiv1 mt10"></div>
    <div class="floatLeft">
        <asp:PlaceHolder ID="_popularStoriesHolder" runat="server">
            <div class="latestStoriesArea">
                <div class="mb5">
                    <div class="latestStories boxHeading">popular stories</div>
                    <div class="latestStoriesTail boxTail"></div>
                </div>
                <div class="storiesArea">
                    <asp:Repeater 
			            id="_popularStoriesList" 
			            runat="server" 
			            OnItemCreated="StoriesListHandler">
			            <ItemTemplate>
                            <div class="story">
                                <asp:Literal ID="_image" runat="server" />
                                <h2 class="storyTitle"><asp:Literal ID="_title" runat="server" /></h2>
                                <div class="mb10">
                                    <span class="storyAdditions">
                                        <asp:Placeholder ID="_photosPanel" runat="server"><img src="/_images/pictures.png" width="16" height="16" alt="" /> <asp:Literal ID="_numPhotos" runat="server" /> hi-def photos&nbsp;</asp:Placeholder>
                                        <asp:Placeholder ID="_commentsPanel" runat="server"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_ecComments" runat="server" /> comments</asp:Placeholder>
                                    </span>
                                    <span class="lightText smallText"><asp:Literal ID="_published" runat="server" /></span>
                                </div>
                                <div><asp:Literal ID="_abstract" runat="server" /></div>
                            </div>
                        </ItemTemplate>
		            </asp:Repeater>
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="_latestStoriesHolder" runat="server" Visible="false">
            <asp:PlaceHolder ID="_latestStoriesTopLineHolder" runat="server"><div class="hDiv2 mt10" id="latestStoriesTopLine"></div></asp:PlaceHolder>
            <div class="latestStoriesArea">
                <div class="mb5">
                    <div class="latestStories boxHeading">latest stories</div>
                    <div class="latestStoriesTail boxTail"></div>
                </div>
                <div class="storiesArea">
                    <asp:Repeater 
			            id="_latestStoriesList" 
			            runat="server" 
			            OnItemCreated="StoriesListHandler">
			            <ItemTemplate>
                            <div class="story">
                                <asp:Literal ID="_image" runat="server" />
                                <h2 class="storyTitle"><asp:Literal ID="_title" runat="server" /></h2>
                                <div class="mb10">
                                    <span class="storyAdditions">
                                        <asp:Placeholder ID="_photosPanel" runat="server"><img src="/_images/pictures.png" width="16" height="16" alt="" /> <asp:Literal ID="_numPhotos" runat="server" /> hi-def photos&nbsp;</asp:Placeholder>
                                        <asp:Placeholder ID="_commentsPanel" runat="server"><img src="/_images/comments.png" width="16" height="16" alt="" /> <asp:Literal ID="_ecComments" runat="server" /> comments</asp:Placeholder>
                                    </span>
                                    <span class="lightText smallText"><asp:Literal ID="_published" runat="server" /></span>
                                </div>
                                <div><asp:Literal ID="_abstract" runat="server" /></div>
                            </div>
                        </ItemTemplate>
		            </asp:Repeater>
                </div>
            </div>
        </asp:Placeholder>
    </div>
    <asp:PlaceHolder ID="_galleriesPlaceholder" runat="server" Visible="false">
        <div id="photoGalleryArea">
            <div id="photoGalleries" class="boxHeading">photo galleries</div>
            <div id="photoGalleryTail" class="boxTail"></div>
            <div id="photosFrame">
                <asp:Repeater 
				    id="_latestGalleries" 
				    runat="server" 
				    OnItemCreated="LatestGalleriesItemHandler">
				    <ItemTemplate>
                        <div id="_gDiv" runat="server">
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
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="_directoryHolder" runat="server">
        <div id="directoryArea">
            <div id="directoryHead" class="boxHeading">directory items</div>
            <div id="directoryTail" class="boxTail"></div>
            <div id="directoryFrame">
                <asp:Repeater 
				    id="_directoryItems" 
				    runat="server" 
				    OnItemCreated="DirectoryItemHandler">
				    <ItemTemplate>
                        <div class="directoryItem">
                            <asp:Literal ID="_link" runat="server" />
                            <span class="directoryCat"><asp:Literal ID="_category" runat="server" /></span>
                        </div>
                    </ItemTemplate>
			    </asp:Repeater>
                <asp:PlaceHolder ID="_directoryCategories" runat="server">
                    <div><div id="directoryCats" class="rounded">Categories</div></div>
                    <asp:Repeater 
				        id="_directoryCats" 
				        runat="server" 
				        OnItemCreated="DirectoryCategoryHandler">
				        <ItemTemplate>
                            <div class="directoryItem">
                                <asp:Literal ID="_link" runat="server" />
                            </div>
                        </ItemTemplate>
			        </asp:Repeater>
                </asp:PlaceHolder>
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="_latestStoriesRhsHolder" runat="server">
        <div id="latestStoriesRhsArea">
            <div id="row3Div" class="hDiv2"></div>
            <div id="lsRhsHead" class="boxHeading">latest stories</div>
            <div id="lsRhsTail" class="boxTail"></div>
            <div id="lsRhsFrame">
                <asp:Repeater 
			        id="_latestStoriesRhsRepeater" 
			        runat="server" 
			        OnItemCreated="LatestStoriesRhsCreatedHandler">
			        <ItemTemplate>
                        <div class="mb5 padded5">
                            <asp:HyperLink ID="_img" runat="server" cssclass="lsImg floatLeft" ClientIDMode="Static" />
                            <div class="ecTitle"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                            <div class="clear"></div>
                        </div>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <div class="mb5 altColumnBand padded5">
                            <asp:HyperLink ID="_img" runat="server" cssclass="lsImg floatLeft" ClientIDMode="Static" />
                            <div class="ecTitle"><asp:HyperLink ID="_link" runat="server" CssClass="dark" ClientIDMode="Static" /></div>
                            <div class="clear"></div>
                        </div>
                    </AlternatingItemTemplate>
		        </asp:Repeater>
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="_commentsHolder" runat="server">
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
    </asp:PlaceHolder>
    <div class="clear"></div>
</asp:Content>