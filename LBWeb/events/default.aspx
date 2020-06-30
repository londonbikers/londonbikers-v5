<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Events.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/events.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">
        <h1><span class="lightText">Events:</span> The Calendar</h1>
	    <hr />
        <div class="yellowStandout rounded2">
            We've got rideouts happening all the time, 
            <a href="/forums/biking-experiences/ride-outs,-meets-events">check the community forums!</a> &raquo;
        </div>
        <div class="blueStandout rounded2 mt10">
            Come along to our weekly meet at <a href="wednesday-meet">Borough Market</a> &raquo;
        </div>
    </div>
    <div class="pl5 mt10">
        <iframe 
            src="http://www.google.com/calendar/embed?src=mc50u40fgm9nvhhli686nmv1i4%40group.calendar.google.com&title=londonbikers.com&chrome=NAVIGATION&height=750" 
            width="100%" 
            frameborder="0"
            height="800"></iframe>
    </div>
</asp:Content>