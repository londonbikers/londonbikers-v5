<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.News.Default" %>
<%@ Register TagPrefix="Controls" TagName="CommentsColumn" Src="~/_controls/NewsIndexComments.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/tag.css" />
    <link rel="stylesheet" href="/css/nivo-slider.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PlaceHolder ID="_topStoriesHolder" runat="server">
        <div id="topStories" class="boxHeading">top stories</div>
        <div id="topStoriesTail" class="boxTail">being read right now</div>
        <div class="rowEnd clear"></div>
        <div id="storyScroller">
            <div id="tcv" class="nivoSlider">
                <asp:Repeater 
			        id="_topDocsImageList" 
			        runat="server" 
			        OnItemCreated="TopDocsImageCreatedHandler">
			        <ItemTemplate><asp:Literal ID="_image" runat="server" />
                    </ItemTemplate>
		        </asp:Repeater>
            </div>
            <asp:Repeater 
			    id="_topDocsCaptionList"
			    runat="server" 
			    OnItemCreated="TopDocsCaptionCreatedHandler">
			    <ItemTemplate><asp:Literal ID="_caption" runat="server" /></ItemTemplate>
		    </asp:Repeater>
        </div>
        <div id="ecContent">
            <asp:Repeater 
			    id="_popularStoriesListing" 
			    runat="server" 
			    OnItemCreated="TopStoriesListingCreatedHandler">
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
    </asp:PlaceHolder>

    <div class="hDiv1 mt10"></div>
    <div class="latestStoriesArea floatLeft">
        <div class="mb5">
            <div class="latestStories boxHeading"><asp:Literal ID="_popularStoriesTitle" runat="server" Text="Popular" /> stories</div>
            <div class="latestStoriesTail boxTail"></div>
        </div>
        <div class="storiesArea">
            <asp:Repeater 
			    id="_popularStoriesList" 
			    runat="server" 
			    OnItemCreated="PopularStoriesListCreatedHandler">
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
    
    <asp:PlaceHolder ID="_galleriesPlaceholder" runat="server" Visible="false">
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

    <Controls:CommentsColumn id="_commentsColumn" runat="server" />

    <div class="clear"></div>
</asp:Content>