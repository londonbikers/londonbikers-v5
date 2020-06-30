<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Articles.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/articles.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="features" class="boxHeading">latest features</div>
    <div id="featuresTail" class="boxTail"></div>
    <div id="featuresBg">
        <div class="clear"></div>
        <div>
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
                    <asp:Literal ID="_rowSep" runat="server" Visible="false"><div class="clear"></div></div><div class="mt10"></asp:Literal>
                </ItemTemplate>
		    </asp:Repeater>
        </div>
        <div class="clear"></div>
    </div> 

    <div class="hDiv1 mt10"></div>
    <div id="latestStoriesArea">
        <div id="popularArticlesHead">
            <div id="popularArticles" class="boxHeading"><asp:Literal ID="_popularFeaturesHeading" runat="server">popular</asp:Literal> features</div>
            <div id="popularArticlesTail" class="boxTail"></div>
        </div>
        <div id="popularStoriesArea">
            <asp:Repeater 
			    id="_popularArticlesList" 
			    runat="server" 
			    OnItemCreated="PopularArticlesListCreatedHandler">
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
    
    <div id="favouriteTopicsArea">
        <div id="lsRhsFrame">
            <asp:Repeater 
			    id="_favouriteTagListInstance" 
			    runat="server" 
			    OnItemCreated="FavouriteTagInstanceHandler">
			    <ItemTemplate>
                    <div class="boxDesc subTitled colTitleRow"><asp:Literal ID="_tagName" runat="server" /></div>
                    <asp:Repeater 
			            id="_docList" 
			            runat="server" 
			            OnItemCreated="FavouriteTagDocsHandler">
			            <ItemTemplate>
                            <div class="mb5 columnBand padded5">
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
                </ItemTemplate>
		    </asp:Repeater>
        </div>
    </div>

    <div class="clear"></div>
</asp:Content>