<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Directory.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/directory.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="searchBox">
        <form action="results.aspx" method="get" target="_top">
		    <div class="mb10">
			    <span class="veryBig">Search the Directory</span> <a href="/help/directory" class="smallText dark" title="get help using the directory">(help)</a>
		    </div>
		    <input type="text" name="s" style="font-family: Arial; font-size: 16px; padding: 5px; width: 300px; font-weight: bold;" />
		    <div class="mt10">
			    <input type="submit" class="big" value="Search!" title="Search the Directory!" />
		    </div>
	    </form>
    </div>

    <div class="padded">

        <div class="mt10 mb20">
	        The LB directory is designed to make finding all things biking, easier. You can find what you're looking for
	        by using the search box above, or by browsing the directory by the categories below. Click into one to explore!
        </div>
        <h2 class="mb20">Categories</h2>
        
        <div class="arial">
            <asp:Literal id="_categoryList" runat="server" />
        </div>

        <div class="lightText arial mt10">
            <i><b>Get Listed!</b> Have your business listed in our directory. Find a category, and then hit the 'submit item' link!</i>
        </div>

    </div>

</asp:Content>