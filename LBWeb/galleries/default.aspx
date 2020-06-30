<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Galleries.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/galleries.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="galleriesHead" class="boxHeading">latest photo galleries</div>
    <div id="galleriesTail" class="boxTail"></div>
    <div id="galleriesBg">
        <div>
            <asp:Repeater 
			    id="_latestGalleries" 
			    runat="server" 
			    OnItemCreated="LatestGalleriesHandler">
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
                    <asp:Literal ID="_rowSep" runat="server" Visible="false"></div><div class="clear"></div><div class="mt10"></asp:Literal>
                </ItemTemplate>
		    </asp:Repeater>
        </div>
        <div class="clear"></div>
    </div>

    <div class="padded">

        <div id="categoriesHead" class="boxDesc subTitled">
            Photo Categories
        </div>
        <div id="categories">
            <asp:Repeater 
			    id="_categoriesList" 
			    runat="server" 
			    OnItemCreated="CategoriesHandler">
			    <ItemTemplate>
                    <asp:Literal ID="_link" runat="server" />&nbsp;
                </ItemTemplate>
		    </asp:Repeater>
        </div>

        <div id="stats" class="rounded">
            <div class="clear"></div>
            <div class="floatLeft">
                <span class="lightText subTitled">Number of galleries:</span> <br /><span class="statValue"><asp:Literal ID="_numOfGalleries" runat="server" /></span>
            </div>
            <div class="floatRight">
                <span class="lightText subTitled">Number of photos:</span><br /> <span class="statValue"><asp:Literal ID="_numOfPhotos" runat="server" /></span>
            </div>
            <div class="clear"></div>
        </div>

        <h2 class="mb10">Hot Photo Galleries</h2>
        <div class="favCol mr10">
            <div class="favHead">
                <img src="/_images/content/bsb-logo.gif" alt="British Superbikes" />
            </div>
            <div>
                <asp:Repeater 
			        id="_tag1" 
			        runat="server" 
			        OnItemCreated="FavouriteColumnHandler">
			        <ItemTemplate>
                        <div class="favGallery">
                            <asp:Literal ID="_image" runat="server" />
				            <div>
					            <h3><asp:Literal ID="_title" runat="server" /></h3>
                                <div class="subText"><asp:Literal ID="_numOfPhotos" runat="server" /> photos</div>
				            </div>
			            </div>
                    </ItemTemplate>
		        </asp:Repeater>
            </div>
        </div>
        <div class="favCol mr10">
            <div class="favHead">
                <img src="/_images/content/wsb-logo2.gif" alt="British Superbikes" />
            </div>
            <div>
                <asp:Repeater 
			        id="_tag2" 
			        runat="server" 
			        OnItemCreated="FavouriteColumnHandler">
			        <ItemTemplate>
                        <div class="favGallery">
                            <asp:Literal ID="_image" runat="server" />
				            <div>
					            <h3><asp:Literal ID="_title" runat="server" /></h3>
                                <div class="subText"><asp:Literal ID="_numOfPhotos" runat="server" /> photos</div>
				            </div>
			            </div>
                    </ItemTemplate>
		        </asp:Repeater>
            </div>
        </div>
        <div class="favCol mr10">
            <div class="favHeadText">
                bike shows
            </div>
            <div>
                <asp:Repeater 
			        id="_tag3" 
			        runat="server" 
			        OnItemCreated="FavouriteColumnHandler">
			        <ItemTemplate>
                        <div class="favGallery">
                            <asp:Literal ID="_image" runat="server" />
				            <div>
					            <h3><asp:Literal ID="_title" runat="server" /></h3>
                                <div class="subText"><asp:Literal ID="_numOfPhotos" runat="server" /> photos</div>
				            </div>
			            </div>
                    </ItemTemplate>
		        </asp:Repeater>
            </div>
        </div>
        <div class="favCol">
            <div class="favHeadText">
                motorcycles
            </div>
            <div>
                <asp:Repeater 
			        id="_tag4" 
			        runat="server" 
			        OnItemCreated="FavouriteColumnHandler">
			        <ItemTemplate>
                        <div class="favGallery">
                            <asp:Literal ID="_image" runat="server" />
				            <div>
					            <h3><asp:Literal ID="_title" runat="server" /></h3>
                                <div class="subText"><asp:Literal ID="_numOfPhotos" runat="server" /> photos</div>
				            </div>
			            </div>
                    </ItemTemplate>
		        </asp:Repeater>
            </div>
        </div>
    </div>

    <div class="clear"></div>
</asp:Content>