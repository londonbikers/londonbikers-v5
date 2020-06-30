<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Events.WednesdayMeet.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/events.css" />
    <script type="text/javascript">
        var markerLat = "51.505617";
        var markerLong = "-0.091313";
    </script>
    <script type="text/javascript" src="http://www.google.com/jsapi"></script>
    <script type="text/javascript" src="/js/map.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">
        <h1><span class="lightText">Events:</span> The Borough Market Meet</h1>
		<hr />
		<h2>Every Wednesday Evening!</h2>
		<div class="standoutBox mb10 mt20">
			Every Wednesday evening we hold a bike-meet at the entrance to the Borough Market. All riders are invited, regardless of what you ride, or how you ride it! You don't even have to be an LB
			member to come along, all nice people are welcome! Find out about the meet below. We hope to see you there next week!
		</div>
		<div>
			<h3>Background</h3>
			<hr />
			We have a history of regular weekly meets going right back to the early days of LB where we used to meet at the Ace Cafe. From there we changed to The Cubana in Waterloo as a summer meet, but
			it was all out in the open so come the winter we changed to another venue where we could be inside to keep warm and dry but still meet up! This was at the Brazen Head in Paddington. Over the winter
			months of 2006, the volunteers of LB started talking about a place for the summer again, so work begun on riding around London looking for a suitable location. The Borough Market was chosen in the end
			as it ticked all the boxes; central, easy to get to, cover from the elements, area with character, bike friendly, good food & drink! The first BMM night was had on the 4th April 2007 and was a
			resounding success with over 100 people attending.
			<br />
			<br />
			<h3>Parking & Considerations</h3>
			<hr />
			Please park on the left side of the road, the one with the pubs and sausage shop. The bays on the right belong to the Market and we've been asked not to use them. We hope this is a temporary measure, but for now please park on the left, <b>nose to the kerb</b>. Nose to the kerb ensures that the shops aren't filled with exhaust fumes when bikes are left running. It's also easier to park!
			<br />
			<br />
			Please also remember that like everywhere else in this great metropolis of ours, there are residents within eat-shot of the meet, so please keep noise to a minimum, i.e. no rev'ing or silly stuff. The longevity of the meet depends on us getting along with our neighbours. Previous well-established (Chelsea Bridge, Soho, etc) meets have been lost in London before due to the careless actions of a minority, so please act responsibly when arriving and leaving the meet!
			<br />
			<br />
			<h3>Directions</h3>
			<hr />
			<b>By Road:</b> Immediately south of London Bridge, it's on a turn-off called Stoney Road from the A3/King William Street. You shouldn't be able to miss it, but you can always head to the Cubana for an escort if all else fails:
			<br />
			<br />
			We'll be running escorts all evening for the opening night from our previous summer meet, The Cubana, which is a mile down the road, so if you don't feel confident that you'll find the venue on your own, head to the Cubana and if there's not already an LB Host there to meet you, there will be one shortly, so hang-tight and you'll be escorted to the 'market.
			<br />
			<br />
			<b>By Train:</b> There's two tube-stations near-by. Borough Station which means a short walk up Borough High St before seeing the Stoney Road the meet is on. There's also London Bridge Station (mainline & tube). A short walk from there down St Thomas St to Borough High Road will get you to the meet. This should serve city-slickers stuck in town with no bike but an evening to kill.
			<br />
			<br />
			<h3>Location</h3>
			<div id="mapCanvas" class="mb20"></div>
			<h3>Time</h3>
			<hr />
			After work until late. This might mean 6.30pm onwards until mid-night, but it depends how quickly people want to get back to their other halves. In which case if they don't, then it might be most of the night and some of the morning. It's been known to happen.
			<br />
			<br />
			<h3>Photos</h3>
			<hr />
			Keep an eye on the <a href="/galleries/">photo galleries</a>, where we capture many of the meets.
			<table class="mt10" align="center">
				<tr>
					<td>
						<div class="ImageFrame"><img src="/_images/content/bmm1.jpg" /></div>
					</td>
					<td>
						<div class="ImageFrame"><img src="/_images/content/bmm2.jpg" /></div>
					</td>
				</tr>
				<tr>
					<td>
						<div class="ImageFrame"><img src="/_images/content/bmm3.jpg" /></div>
					</td>
					<td>
						<div class="ImageFrame"><img src="/_images/content/bmm4.jpg" /></div>
					</td>
				</tr>
				<tr>
					<td>
						<div class="ImageFrame"><img src="/_images/content/bmm5.jpg" /></div>
					</td>
					<td>
						<div class="ImageFrame"><img src="/_images/content/bmm6.jpg" /></div>
					</td>
				</tr>
			</table>
		</div>
    </div>
</asp:Content>