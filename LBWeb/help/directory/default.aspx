<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Help.Directory.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">
        <h1 class="mb20"><span class="lightText">Help: </span>The Directory</h1>

        <ul>
            <li><a href="#what">What is the Directory?</a></li>
            <li><a href="#how">How do I use the Directory?</a></li>
            <li><a href="#searches">How do the searches work?</a></li>
            <li><a href="#submit">How can I submit an item?</a></li>
            <li><a href="#help">Can I help maintain the Directory?</a></li>
            <li><a href="#related">Related help</a></li>
        </ul>

        <hr />
        
	    <a name="what"></a>
	    <h2 class="mb10">What is the Directory?</h2>
	    The Directory is a listing of all the biking-related organisations and companies in the UK. While most items link to 
	    the respective websites of the lister, they don't have to, and can just provide contact information instead.
	    <br />
	    <br />
	    <br />
	    <a name="how"></a>
	    <h2 class="mb10">How do I use the Directory?</h2>
	    You can either browse the Directory by category, or you can search for a particular item. To browse the Directory, 
	    you should start at the Directory home, find a category that describes the thing you're looking for in a basic 
	    sense, then if there's more specific categories inside that one, you can go further until you find a specialised 
	    category listing items that match what you're looking for.
	    <br />
	    <br />
	    When searching, you can do either a tile, or concept search. The title search is the default one, where item and 
	    category titles will be inspected to see if they match what you're looking for, i.e. searching for 'honda' could 
	    return 'honda chiswick', or 'honda uk'. A concept search is different and is described below.
	    <br />
	    <br />
	    <br />
	    <a name="searches"></a>
	    <h2 class="mb10">How do the searches work?</h2>
	    When you search the directory, instead of to browse it, you'll be searching for items and categories matching a 
	    subject, i.e. if you search for 'scooters', you could find results for 'piaggio dealer', 'gilera servicing' or 
	    'aprilia uk'. They all have one thing in common, they deal with scooters. The way this works is that every item 
	    in the Directory has a set of keywords associated with it, and these are used to link together items with concepts, 
	    so you stand a better chance of finding what you're looking for by describing it in a way you know.
	    <br />
	    <br />
	    <br />
	    <a name="submit"></a>
	    <h2 class="mb10">How can I submit an item?</h2>
	    Anyone can submit an item to the Directory! Simply register with us if you're not already, then browse the Directory 
	    to find the category you think best describes your item, then click on the 'submit an item' link at the top-left of 
	    the page. Don't worry if you think your item can be described by more than one category, we can add it to multiple 
	    ones. You'll be shown a page that will allow you to enter all the information we need. Once your item has been 
	    submitted, we'll ensure it's all correct and fits our guidelines before publishing it. You'll be notified when it's 
	    been published by email.
	    <br />
	    <br />
	    For more information on submitting an item, <a href="submission">see here</a>.
	    <br />
	    <br />
	    <br />
	    <a name="help"></a>
	    <h2 class="mb10">Can I help maintain the Directory?</h2>
	    If you're an existing member and believe you can help to moderate new items, ensure they're in the right categories 
	    and match our publishing guidelines, has all the appropriate keywords, then please get in <a href="mailto:contact@londonbikers.com">contact</a> with us, we need 
	    help to build the best UK motorcycle directory.
	    <br />
	    <br />
	    <br />
	    <a name="related"></a>
	    <h3 class="mb10">Related help:</h3>
        <ul>
            <li><a href="submission">Submitting an item</a></li>
            <li><a href="submission-guidelines">Item submission guidelines</a></li>
        </ul>

    </div>
</asp:Content>