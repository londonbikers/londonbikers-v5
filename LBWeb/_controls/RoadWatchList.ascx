<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoadWatchList.ascx.cs" Inherits="Tetron.Controls.RoadWatchList" %>
<asp:Repeater 
	id="_latestRoadWatchItems" 
	runat="server" 
	OnItemCreated="RoadWatchItemCreatedHandler">
	<ItemTemplate>
        <div class="roadWatchItem"><asp:HyperLink id="_link" runat="server" CssClass="dark" /></div>
	</ItemTemplate>
</asp:Repeater>