<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Tetron.Store.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">
        <h1>The Shop</h1>
        <p>
		    We've got a great range of clothing, stickers and accessories to celebrate the community and friendships that LB membership brings. Stickers go great on bikes, helmets and cars, 
		    and the keyfobs are just the job for bike or house keys.
	    </p>
	    <p>
		    Sporting the clothing or stickers helps us recognise each other, and helps show the world how united we are. No
		    profit is made on anything in the store, it's all for the community!
	    </p>

        <div class="veryBig arial">
            <div class="halfCentered">
                <a href="stickers/" class="Flat">Stickers & Keyfobs</a>
            </div>
            <div class="halfCentered">
                <a href="clothing/" class="Flat">Clothing</a>
            </div>
        </div>

        <table width="100%" cellspacing="0">
			<tr>
				<td align="center" valign="top">
					<a href="stickers/"><img src="/_images/content/stickers-default.jpg" border="0" /></a>
				</td>
				<td align="center" valign="top">
					<a href="clothing/"><img style="border: 0px;" src="http://image.spreadshirt.net/image-server/image/product/7077286/view/2/type/png/width/280/height/280" /></a>
				</td>
			</tr>
		</table>

    </div>
</asp:Content>