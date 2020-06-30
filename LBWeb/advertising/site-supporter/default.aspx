<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Advertising.SiteSupporter.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">
        <h1>Site Supporter</h1>
	    <div class="smallText">&laquo; <a href="../">Back to Advertising</a></div>
	    <hr />
	    <img src="/_images/content/site-supporter-big.jpg" alt="Site Supporter Posting" style="float: left; margin-right: 10px;" />
	    <h3>What is it?</h3>
	    <p>
            Our Community Forums are some of the busiest motorcycle discussion forums in the UK, with thousands of users each day. 
	        The <b>Site Supporter</b> option allows you to contribute to the forum as a business and engage with the community, providing recommendations, product/service information and benefit from regular branding.
        </p>
        <h3>What's the benefit?</h3>
        <p>
            Our <b>Site Supporters</b> find this is an extremely cost-effective way to engage with the community, allowing them to be the first to present a commercial solution to members asking questions, such as where to get their 
            bike serviced, what accessory to buy, where to buy new tyres or who to get insurance with, etc.
        </p>
        <p>
            It's also a novel way to be proactive about customer-care. You can directly engage with people who need help with your service, whilst also raising the public image of your business by being proactive and approachable. 
            We think this option is an excellent accompaniment to our other advertising solutions. It keeps your business in the minds of our many members and readers.
        </p>
        <h3>What's it look like?</h3>
        <p>
            The package allows you to have a graphical banner in your "signature" (inserted under each message you post) that links through to your website, or three-lines of text, including a link again. As well as this, 
            your user-status message under your name and picture will say <b>"Site Supporter"</b>, clearly authenticating your status.
        </p>
        <hr />
        <div style="background-color: #f8f8f8; padding: 20px 10px 20px 10px;">
                
            <h2 class="mb20">Become a Site Supporter now!</h2>
            <p>
                It costs a mere <b>&pound;25 per month</b> to become a Site Supporter. We accept payment via PayPal subcription and standing order only. If you want to pay by BACS or cheque, a minimum of six months is to be paid in advance.
            </p>
            <p>
                You can setup a subscription below, allowing you to be billed for the &pound;25 each month. We need to know your member account username to upgrade your status,
                so please provide it below, or if you haven't yet signed up, just enter 'not yet' and let us know by <a href="mailto:advertising@londonbikers.com">email</a> when you're ready.
            </p>
                    
            <table style="margin: 0px auto; margin-top: 20px;">
                <tr>
                    <td>
                        <form action="https://www.paypal.com/cgi-bin/webscr" method="post">
                            <input type="hidden" name="cmd" value="_s-xclick" />
                            <input type="hidden" name="hosted_button_id" value="8429267" />
                            <table><tr><td><input type="hidden" name="on0" value="Forum Username" />Forum Username</td></tr><tr><td><input type="text" name="os0" maxlength="60" /></td></tr></table>
                            <input type="image" src="https://www.paypal.com/en_US/GB/i/btn/btn_subscribeCC_LG.gif" style="border: 0px;" name="submit" alt="PayPal - The safer, easier way to pay online." />
                            <img alt="" border="0" src="https://www.paypal.com/en_GB/i/scr/pixel.gif" width="1" height="1" />
                        </form>
                    </td>
                </tr>
            </table>

        </div>
        <hr />
                
        <h3>Specifications</h3>
        <p><b>Graphical Signature:</b> 468 x 60px banner, in Flash, Jpeg or animated Gif format. As per our regular advertising options, no excessively distracting animations will be accepted, and no pop-ups, pop-unders and in this case, no expandable 
        adverts will be permitted. The maximum file-size is 75k.</p>
        <p><b>Text Signature:</b> Three lines of regular-sized text, or two lines of larger text, with as many links in as you want. Use of formatting such as bold, italic and underline is permitted. Help with formatting is available upon request with a forum moderator.</p>

        <h3 class="mt20">Setup Notes</h3>
        <p>It is up to you to get your signature via your <a href="/forums/controlpanel">forum control-panel</a>, though we will upgrade your LB account upon payment to show you as a Site Supporter. If you require us to design your graphical banner, you are required 
        to commit to six months or more. There is no charge for banner design.</p>
                
        <hr />
        <div>
            <h5 class="lightText">Acceptable Use Policy</h5>
            <p class="lightText smallText">Becoming a Site Supporter is an excellent way to promote your business at a cost-effective rate, but it also comes with responsibility. You are still subject to the forum rules and are expected to post up in a professional and 
            courteous manner. You're also required to keep the number of posts you make of a commercial nature to a reasonable amount. If our Forum Moderation team feel you are exceeding this reasonable amount and are spamming the forums, they may ask you to 
            reduce the number of posts you're making.</p>
            <p class="lightText smallText">The <a href="mailto:moderators@londonbikers.com" class="darker">community forum moderation team's</a> decision is always final and if you repeatedly ignore their requests, your status will be revoked and no refund will be eligable.</p>
        </div>
    </div>
</asp:Content>