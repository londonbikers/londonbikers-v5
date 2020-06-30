<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Register.Default" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">

        <h1>Registration</h1>
        <asp:Placeholder ID="_registerView" runat="server">
            <div class="ResponseBox mb10 mt10" id="_prompt" runat="server" visible="false" />
            <div class="mt10 mb10">
                <p>Join the community now. Membership is free and we promise you won't be spammed.</p>
                <p>Members can post up within our community forums, message one-another, view high-resolution gallery photos, make comments on news, articles, galleries and more...</p>
            </div>
            <hr class="light" />
            <form runat="server" class="pretty">
				<table border="0" cellpadding="0" cellspacing="0" width="100%" class="mt20 b20m">
					<tr>
						<td width="140" class="pb5">
							<div><b>Username</b></div>
						</td>
						<td class="pb5">
							<asp:TextBox id="_username" runat="server" />
                            <span class="subText"><i>&laquo; This will be how you're known to others, i.e. "R1Fan"</i></span>
						</td>
					</tr>
					<tr>
						<td class="pb5">
							<b>E-Mail</b>
						</td>
						<td class="pb5">
							<asp:TextBox id="_email" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="pb5">
							<b>Password</b>
						</td>
						<td class="pb5">
                            <asp:TextBox id="_password" textmode="Password" runat="server" />
						</td>
					</tr>
					<tr>
						<td>
							Re-type password
						</td>
						<td>
							<asp:TextBox id="_passwordConfirmation" textmode="Password" runat="server" />
						</td>
					</tr>
				</table>

		        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin-top: 15px;">
			        <tr>
				        <td width="140" class="pb5">
						   First name
				        </td>
				        <td class="pb5">
					        <div>
						        <asp:TextBox id="_firstname" runat="server" cssclass="MedTextBox" />
                                <span class="subText"><i>* optional</i></span>
					        </div>
				        </td>
			        </tr>
			        <tr>
				        <td>
						    Last name
				        </td>
				        <td>
					        <div>
						        <asp:TextBox id="_lastname" runat="server" cssclass="MedTextBox" />
						        <span class="subText"><i>* optional</i></span>
					        </div>
				        </td>
			        </tr>
			        <tr>
				        <td colspan="2">
					        &nbsp;
				        </td>
			        </tr>
			        <tr>
				        <td>
					        <div class="smallText">
						        Remember me?
					        </div>
				        </td>
				        <td>
					        <div>
						        <asp:CheckBox id="_persistantAuth" runat="server" checked="true" />
						        <span class="subText">You won't have to sign in the next time you visit from this computer.</span>
					        </div>
				        </td>
			        </tr>
			        <tr>
				        <td>
					        <div class="smallText">
						        Agree to <a href="/about/registration-conditions" class="Flat" target="_blank">terms</a>?
					        </div>
				        </td>
				        <td>
					        <div>
						        <asp:CheckBox id="_agreement" runat="server" checked="true" />
						        <span class="subText">You must agree to our terms, to become a member.</span>
					        </div>
				        </td>
			        </tr>
			        <tr>
			            <td colspan="2" style="height: 40px; vertical-align: middle;">
			                <hr class="light" />
			            </td>
			        </tr>
			        <tr>
				        <td>
					        &nbsp;
				        </td>
				        <td>
				            <recaptcha:RecaptchaControl 
				              Theme="clean"
                              ID="recaptcha"
                              runat="server"
                              PublicKey="6Le8v7oSAAAAAC4lvboVrwtwIWTqp94iMxstBTrG"            
                              PrivateKey="6Le8v7oSAAAAAOFzzPDRnqszM0j6ZfVW0xLTPNuG"
                              />

                            <asp:Button 
                                text="Register!" 
                                runat="server" 
                                cssclass="big mt10" 
                                onclick="RegisterUser" />
				        </td>
			        </tr>
		        </table>
	        </form>			

        </asp:Placeholder>

        <asp:PlaceHolder id="_memberView" runat="server">
	        Welcome to londonbikers.com, the best online motorcycle community and magazine <b><asp:Literal id="_usernameTag" runat="server" /></b>! Thank-you for registering, a confirmation mail 
	        has been sent to your e-mail address. Please explore the site and get involved in the online-magazine and community.
	        If you're local, we hope to meet you at our Newbie Meets or weekly <a href="/events/wednesday-meet/" class="Flat">Borough Market Meet</a>.
	        <br />
	        <br />
	        Some things you can now do:
	        <ul>
		        <li>Read the latest motorcycle <a href="/news">news</a></li>
		        <li>Read our <a href="/articles">articles</a></li>
		        <li>Browse the best motorcycle photo <a href="/galleries">galleries</a> on the web</li>
		        <li>Leave comments against the above</li>
		        <li>Subscribe to our RSS feeds</li>
		        <li>Introduce yourself to the community via our <a href="/forums">forums</a></li>
		        <li>Update your <a href="/forums/ControlPanel.aspx">profile</a> to let others know about you</li>
		        <li>Come on our <a href="/forums/biking-experiences/ride-outs,-meets-events">ride-outs</a>, and to our meets</li>
		        <li>Get some <a href="/store/stickers">stickers</a> to add to your bike/gear</li>
	        </ul>
	        <div class="ErrorBox" id="_messageBox" runat="server" style="margin-top: 20px;" />
        </asp:PlaceHolder>

    </div>

</asp:Content>