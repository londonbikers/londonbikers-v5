<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Tetron.Store.Stickers.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">
        <h1>The Shop</h1>
        <div class="subText">
            <a href="../">The Store</a> | Stickers & Keyfobs | <a href="../clothing/">Clothing</a>
        </div>
        <p>
		    We've got a great range of clothing, stickers and accessories to celebrate the community and friendships that LB membership brings. 
            Stickers go great on bikes, helmets and cars, and the keyfobs are just the job for bike or house keys.
	    </p>
	    <p>
		    Sporting the clothing or stickers helps us recognise each other, and helps show the world how united we are. No
		    profit is made on anything in the store, it's all for the community!
	    </p>
        <p class="smallText">
            See <a href="/galleries/gallery/1334/stickers-showcase">stickers on your gear!</a> - Having problems? <a href="http://londonbikers.bigcartel.com/" target="_blank">Click here</a>.
        </p>
    </div>
    <iframe src="http://londonbikers.bigcartel.com/" width="100%" height="1500" frameborder="0" scrolling="no"></iframe>
</asp:Content>