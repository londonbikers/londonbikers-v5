<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="LBWeb.about._default" 
 %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/about.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">
        <h1>About Us: Who, what where and how?</h1>
        <h2 class="mt20">What's it all about?</h2>
        <p>LB is a brilliant coming-together of an online-magazine and community for all bikers, publishing all the latest motorcycle news, reviews, interviews, industry-leading racing coverage 
        of the BSB, WSB, MotoGP, British MX series' as well as home to the brilliant London biking community.</p>
        <p>We started out offering a service for bikers local to London, but as the editorial aspect developed, it became clear that we were the first to bring high-resolution photography,
        advanced tagging and linking features to stories and the best high-resolution bike racing photographs on the web. Since then we've developed LB into a thriving and massively popular 
        portal that appeals to bikers all over the world.</p>

        <h2>The People</h2>
        <p>LB is owned and run independently by <a href="http://mediapanther.com">Media Panther Network</a>, a UK registered company. It's managed by a core team of highly passionate bikers that help to keep the site fresh, up to date, manage the contributors input and constantly develop the web-site features. This core team is comprised of:</p>

        <div class="mt20">
            <div class="thumbContainer floatLeft mr10">
                <img src="/_images/content/ja.jpg" width="120" height="148" alt="Jay Adair" class="imgFrame" />
            </div>
            <h3>Jay Adair</h3>
            <h4>Technical Director</h4>
            <p>Coming from a seasoned technical development background with over <%= DateTime.Now.Year - 1997 %> years of industry experience as an I.T professional, Jay brings a wealth of professional technical capability to LB, overseeing development of the technology that runs the website, whilst also helping to bring together all other operations and strategies. He’s a trackday junkie and also has a keen interest in photography.</p>
            <div class="clear"></div>
            <div class="thumbContainer floatLeft mr10">
                <img src="/_images/content/ah.jpg" width="120" height="145" alt="Andrew Harbron" class="imgFrame" />
            </div>
            <h3>Andrew Harbron</h3>
            <h4>Media Director</h4>
            <p>Andrew is responsible for LB gaining wide acclaim for pioneering our industry-leading racing photography. He's been with us since LB started in 2004 and has put in a dedication that is well beyond the call of duty. He's spent countless nights in the back of his car at race circuits in all weather, from swealtering heat to freezing snow, working with our other photographers and ensuring we publish the most unique and amazing BSB, WSB, British and World MX coverage. When not snapping, he's having a ball on his Triumph Daytona 675 or spending time with his new family.</p>
            <div class="clear"></div>
        </div>

        <h2>The Editorial Team</h2>
        <p>The magazine aspect is run by a team of journalists, contributors, presenters, photographers and editors. The people that make the brilliant news, articles and galleries service happen are:</p>
        <ul>
            <li>Garret Cashman - Editor</li>
            <li>Neil Everett - Features Editor</li>
            <li>Bertie Simmonds - Journalist</li>
            <li>Toby Stokes - Photo Journalist</li>
            <li>Mike Dodd - Photographer</li>
            <li>Ian Biggs - Photographer</li>
            <li>Ryan Smith - Photographer</li>
            <li>Clinton Thomas - Contributor</li>
            <li>Ben Curwen - Contributor</li>
            <li>Panagiotis Foufoutis - Contributor</li>
            <li>John Steward - Contributor</li>
            <li>Brian Pilcher - Contributor</li>
            <li>James Green - Contributor</li>
        </ul>


        <h2 class="mt20">The Community Team</h2>
        <p>The community forums are managed and maintained by a volunteer group of community members. These people have been selected by LB as responsible people 
        to help ensure that the forum is kept clean and safe for all. They perform the thankless task of helping to preserve the balance of having the freedom 
        to post, and the inherent conflicts that arise because of this. The moderators are:</p>
        <ul>
            <li>Patrick McConnon</li>
            <li>Eugene Odeluga</li>
            <li>Paul Johnston</li>
            <li>Shane McDonald</li>
        </ul>

        <h2 class="mt20">Can I help?</h2>
        <p>We're constantly looking for new people to join our editorial team. If you have an interest in a particular subject and have some spare time, and wish to get involved 
        then please get in <a href="/contact">contact</a> with us. We're interested in speaking to writers as well as non-writers who just have some spare time on their hands to process 
        news that comes in to us. There's so many subjects that need covering!</p>
        <p>We're also looking for experienced marketing people who can develop our advertising service and help bring greater advertiser relevancy to our readers. 
        Comission options exist. Contact <a href="mailto:advertising@londonbikers.com">advertising@londonbikers.com</a> for more information.</p>

        <h2 class="mt20">Investors</h2>
        <p>We're working hard to deliver upon an exciting long-term plan. We're looking for investors to help accelerate the business and we believe 
        there are great opportunities for investors with us. Please get in <a href="/contact">contact</a> if you're an investor to find out more.</p>

        <h2 class="mt20">Technology</h2>
        <p>We develop all of the technology and software required to run the site in-house. Everything is built be-spoke to our requirements and to maximise performance and minimise operational and future development cost.</p>
        <p>We've recently (2010) launched an automated news processing system which allows our content partners to send content to us and have it ingested into our editorial 
        systems automatically and immediately. Information on how content suppliers can benefit from this will be available shortly. For now, please <a href="/contact">contact us</a>.</p>
    </div>
</asp:Content>