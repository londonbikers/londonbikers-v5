<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="category.aspx.cs" Inherits="Tetron.Galleries.Category" %>
<%@Register Namespace="Webdiyer.WebControls" Assembly="UrlPager" TagPrefix="webdiyer"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/gallery-category.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="head">
        <h1><span class="lightText">Category:</span> <asp:Literal ID="_title" runat="server" /></h1>
    </div>

    <div class="padded">

        <div class="smallText mb10">
            <asp:Literal ID="_description" runat="server" />
        </div>

        <span class="pager">
            <webdiyer:UrlPager runat="server" id="_topPager" PageIndexParameterName="p" PageSize="50" AutoHide="true" RouteName="gallery category"  />
        </span>

        <hr class="hr3 mb10 mt10" />
    
        <asp:Repeater 
		    id="_galleries" 
		    runat="server" 
		    OnItemCreated="GalleryHandler">
		    <ItemTemplate>
                <div class="floatLeft mr10 frame">
                    <asp:Literal ID="_image" runat="server" />
                    <div>
                        <span class="cmt"><asp:Literal ID="_title" runat="server" /></span><br />
                        <asp:Literal ID="_numOfPhotos" runat="server" /> photos
                    </div>
                </div>
                <asp:Literal ID="_rowSep" runat="server" Visible="false"><div class="clear"></div></asp:Literal>
            </ItemTemplate>
	    </asp:Repeater>

        <div class="clear"></div>

        <span class="pager">
            <webdiyer:UrlPager runat="server" id="_bottomPager" PageIndexParameterName="p" PageSize="50" AutoHide="true" RouteName="gallery category"  />
        </span>

    </div>

    
</asp:Content>