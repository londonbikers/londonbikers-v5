<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Advertising.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/js/jquery.tools.min.js"></script>
    <script type="text/javascript">
        (function ($) {
            $("#t1").tooltip({
                position: "top right",
                offset: [0, -70],
                tip: '#tooltip1'
            });
            $("#t2").tooltip({
                position: "top right",
                offset: [0, -70],
                tip: '#tooltip2'
            });
            $("#t3").tooltip({
                position: "top right",
                offset: [0, -70],
                tip: '#tooltip3'
            });
            $("#t4").tooltip({
                position: "top right",
                offset: [0, -70],
                tip: '#tooltip4'
            });
            $("#t5").tooltip({
                position: "top right",
                offset: [0, -70],
                tip: '#tooltip5'
            });
            $("#t6").tooltip({
                position: "top right",
                offset: [0, -70],
                tip: '#tooltip6'
            });
            $("#t7").tooltip({
                position: "top right",
                offset: [0, -70],
                tip: '#tooltip7'
            });
            $("#t8").tooltip({
                position: "top right",
                offset: [0, -70],
                tip: '#tooltip8'
            });
        })(window.jQuery);
    </script>
    <link rel="stylesheet" href="/css/advertising.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% 
const int averageUniques = 300000;
const int members = 16000;
%>
<div class="padded">
    <h1>Advertise Your Business</h1>
    <h4>On londonbikers.com</h4>
    <hr />
    <p>You can advertise your business on londonbikers.com and benefit from reaching hundreds of thousands of prospective customers each month. We run campaigns for some of the biggest (and smallest) companies in the motorcycle industry, from regular run-of-site adverts to community promotion packages and newsletters.</p>
    <p>In these times, being able to spend your marketing budget more wisely by focusing in on the customers you really need has never been more important. With a clearly defined majority demographic, we’re the best choice for breaking in to, or remaining on top of in the prosperous London market.</p>
    <p>We’ve got some great packages that suit budgets of all sizes, from &pound;25 per month community services up to premium run-of-site leaderboard adverts for maximum impact. Read below for more info.</p>
    <hr />
    <h2 class="mb10">Stats &amp; Demographics</h2>
    <p>
        The website pulls in an average of <b><%= averageUniques.ToString("N0")%></b> unique visitors per month (<b>36%</b> of which are in London) and has over <b><%= members.ToString("N0") %></b> community members, 
        with 99% of these receiving our email newsletters.
    </p>
    <div class="blueStandout rounded2 mt10">
        We're currently preparing more detailed demographic information for <%= DateTime.Now.Year %>.
    </div>
    <hr />

    <h2 class="center mb20">Products &amp; Services</h2>
    
    <table cellpadding="0" cellspacing="0" class="mt10">
        <tr>
            <td style="width: 50%; border-right: solid 1px #ccc; padding-right: 10px; vertical-align: top;">
                <h3>Brand Advertising</h3>
                <div class="mt10">
                    Traditional adverts allow you to place your brand in front of every visitor to the site. When placing your brand and products in front
                    of the maximum number of motorcyclists is required, this is the most effective way.
                </div>
                            
                <div id="tooltip1" class="Tooltip">
                    A Leaderboard advert (728 x 90px in size) is our premium form brand advert. It sits at the top of every page and gains maximum exposure, ensure maximum impact for your advert.
                    <img src="/_images/content/adunit-leaderboard.jpg" class="mt10" alt="Leaderboard" />
                </div> 
                <div id="tooltip2" class="Tooltip">
                    A rectangle advert (336 x 280px in size) offers a huge amount of visual impact and is visible across all of our editorial pages, i.e. homepage, news and article pages. 
                    Ideal when you sizable visuals to convey and want significant eyeball-drawing results.
                    <img src="/_images/content/adunit-rectangle.jpg" class="mt10" alt="Rectangle" />
                </div> 
                <div id="tooltip3" class="Tooltip">
                    A half-skyscraper advert (120 x 240px in size) is a great way to get large exposure above-the-fold on every page, ensuring it gets seen every time.
                    <img src="/_images/content/adunit-hsky.jpg" class="mt10" alt="Half-Skyscraper" />
                </div> 
                <div id="tooltip4" class="Tooltip">
                    A Skyscraper advert (120 x 600px in size) features on all of our pages and offers great vertical exposure.
                    <img src="/_images/content/adunit-sky.jpg" class="mt10" alt="Skyscraper" />
                </div> 
                <div id="tooltip5" class="Tooltip">
                    A Button advert (120 x 120px in size) features on all of our pages and allows business' of all sizes to get run-of-site exposure on LB. Excellent value and good impact.
                    <img src="/_images/content/adunit-button.jpg" class="mt10" alt="Button" />
                </div> 
                            
                <div class="mt20 lightText center">
                    Ad Unit Options:
                </div>
                <div class="adUnit">
                    <img src="/_images/view.jpg" alt="view" id="t1" class="Point mr10" /> <h4 style="display: inline;">Leaderboard</h4>
                </div>
                <div class="adUnit">
                    <img src="/_images/view.jpg" alt="view" id="t2" class="Point mr10" /> <h4 style="display: inline;">Rectangle</h4>
                </div>
                <div class="adUnit">
                    <img src="/_images/view.jpg" alt="view" id="t3" class="Point mr10" /> <h4 style="display: inline;">Half-Skyscraper</h4>
                </div>
                <div class="adUnit">
                    <img src="/_images/view.jpg" alt="view" id="t4" class="Point mr10" /> <h4 style="display: inline;">Skyscraper</h4>
                </div>
                <div class="adUnit">
                    <img src="/_images/view.jpg" alt="view" id="t5" class="Point mr10" /> <h4 style="display: inline;">Button</h4>
                </div>
                          
            </td>
            <td style="vertical-align: top; padding-left: 10px;">
                <h3>Community Advertising</h3>
                <div class="mt10">
                    Community adverts allow you to reach out and engage, even interact with the <b><%= members.ToString("N0") %>+</b> members LB has. 
                    This is perfect for directly marketing to specific groups or individuals, making it ideal for retailers and service-providers.
                </div>

                <div id="tooltip6" class="Tooltip">
                    Sponsoring an email newsletter to our members allows you to get your campaign out to over <%= members.ToString("N0") %> people. We're flexible with the appearance of the advert as the design can change easily, though
                    for illustration there's a leaderboard advert shown below.
                    <img src="/_images/content/adunit-newsletter.jpg" class="mt10" alt="Newsletter" />
                </div> 
                <div id="tooltip7" class="Tooltip">
                    Forum Sponsorship allows you to get a leaderboard-sized (728 x 90px) advert at the top of a chosen forum where it'll be seen by every visitor to that forum. The community forums are tremendously busy
                    and see many return visits by members during each day, giving a great opportunity to stay in prospective customers minds.
                    <img src="/_images/content/adunit-forum-sponsorship.jpg" class="mt10" alt="Forum Sponsorship" />
                </div> 
                <div id="tooltip8" class="Tooltip">
                    Our Site-Supporter package is an extremely good value way of engaging with the community. Any business' wishing to discuss anything within the forums require this package. <br /><br />
                    For &pound;25 per month this allows you to join in discussions and recommend products or services, and to announce new ones as well. There is huge potential for developing a strong and lasting relationship with customers here.
                    <img src="/_images/content/adunit-site-supporter.jpg" class="mt10" alt="Site Supporter" />
                </div> 
                                            
                <div class="mt20 lightText center">
                    Ad Unit Options:
                </div>
                <div class="adUnit">
                    <img src="/_images/view.jpg" alt="view" id="t6" class="Point mr10" /> <h4 style="display: inline;">Newsletter Sponsorship</h4>
                </div>
                <div class="adUnit">
                    <img src="/_images/view.jpg" alt="view" id="t7" class="Point mr10" /> <h4 style="display: inline;">Forum Sponsorship</h4>
                </div>
                <div class="adUnit">
                    <img src="/_images/view.jpg" alt="view" id="t8" class="Point mr10" /> <h4 style="display: inline;">Site Supporter Package</h4>
                </div>
                            
                <div class="standoutBox mt20">
                    <img src="/_images/icn-right-arrow.gif" alt="" style="vertical-align: middle;" />
                    <a href="site-supporter/" class="arial">Find out about <b>Site Supporter</b> now!</a>
                </div>
                            
            </td>
        </tr>
    </table>

    <hr />

    <h2>Rates &amp; Contact Us</h2>

    <div class="yellowStandout mt10 rounded2">
        Our rate card is <a href="http://mediapanther.com/advertising/londonbikers/">here</a>. 
    </div>

	<div class="mt10">
		<img src="/_images/silk/email.png" alt="Email us!" /> <a href="mailto:advertising@londonbikers.com" class="arial bold">Contact us by email</a> or telephone Claire (UK: 07709 340 850, World: +44 7709 340 850) to get a copy of 
		our media-pack and rates or to discuss any potential advertising or business partnerships outside of our traditional solutions. We’re only too happy to work with you to create any custom campaigns as well.
	</div>

    <h2 class="mt20 mb10">Recent Advertisers</h2>
	<p>You'll be amongst good company. The following companies have recently benefited from advertising on londonbikers.com:</p>
	<a href="http://www.superbikeschool.com/"><img src="/_images/content/css.jpg" style="display: inline; vertical-align: middle; border: 0px;" alt="California Superbike School" /></a>
	<a href="http://www.hein-gericke.co.uk/"><img src="/_images/content/hein-gericke.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="Hein Gericke" /></a>
	<a href="http://www.infinitymotorcycles.com/"><img src="/_images/content/infinity.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="Infinity Motorcycles" /></a>
	<a href="http://www.mceinsurance.com/"><img src="/_images/content/mce.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="MCE Insurance" /></a>
	<a href="http://www.spyderclub.co.uk/"><img src="/_images/content/spyderclub.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="Spyder Club" /></a>
	<a href="http://www.raceperformanceparts.co.uk/"><img src="/_images/content/rpp.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="Race Performance Products" /></a>
	<a href="http://www.diasec.co.uk/"><img src="/_images/content/drp.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="Diamond Racing Products" /></a>
	<a href="http://www.mc-ams.co.uk/"><img src="/_images/content/mcams.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="MC-AMS Solicitors" /></a>
	<a href="http://www.motogp.com"><img src="/_images/content/motogp.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="MotOGP" /></a>
	<a href="http://www.lazermotorcycles.co.uk/"><img src="/_images/content/lazermc.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="Lazer Motorcycles" /></a>
	<a href="http://www.yanchor.com/"><img src="/_images/content/yanchor.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="Y-Anchor" /></a>
	<a href="http://www.sbkcity.co.uk/"><img src="/_images/content/sbkservicing.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="SBK Servicing" /></a>
	<a href="http://www.westlondonsuzuki.co.uk/"><img src="/_images/content/wls.jpg" style="display: inline; margin-left: 10px; vertical-align: middle; border: 0px;" alt="West London Suzuki" /></a>

</div>
</asp:Content>