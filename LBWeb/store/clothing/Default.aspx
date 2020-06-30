<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Tetron.Store.Clothing.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">
        <h1>The Shop</h1>
        <div class="subText">
            <a href="../">The Store</a> | <a href="../stickers/">Stickers & Keyfobs</a> | Clothing
        </div>
        <p>
		    If you're having trouble with your shopping basket, use <a href="http://londonbikers.spreadshirt.co.uk" target="_blank">this link</a> instead.<br />
	    </p>
	    <p>
		    Sporting the clothing or stickers helps us recognise each other, and helps show the world how united we are. No
		    profit is made on anything in the store, it's all for the community!
	    </p>
        <p>
            <b>Customisable hoodies available.</b> Have your name on the back and arms!
        </p>
        <iframe src="http://londonbikers.spreadshirt.net/" width="100%" height="1680" frameborder="0" scrolling="no"></iframe>

    </div>
</asp:Content>