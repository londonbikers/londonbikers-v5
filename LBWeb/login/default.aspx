<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Tetron.Login.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/login.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="padded">
		<div id="_responseBox" runat="server" class="ResponseBox" visible="false" />
        <h1>Login</h1>
        <form runat="server">
            <div class="mt10">
                No secret-handshakes here, just hit your details in below to get access.
            </div>
			<asp:HiddenField ID="_redirectUrl" runat="server" Visible="false" />
			<table cellpadding="0" cellspacing="5" id="_formTable" runat="server" class="mt20">
				<tr>
					<td style="padding-right: 10px;">
						<h3>Username:</h3>
					</td>
					<td>
						<asp:TextBox ID="_username" runat="server" CssClass="big" />
                        <span class="error">
						    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
							    ValidationGroup="login" 
							    runat="server" 
							    Text="*" 
							    Display="Dynamic" 
							    ControlToValidate="_username" 
							    ErrorMessage="Please enter your username." />
                        </span>
					</td>
				</tr>
				<tr>
					<td class="pt5">
						<h3>Password:</h3>
					</td>
					<td class="pt5">
						<asp:TextBox TextMode="Password" ID="_password" runat="server" CssClass="big" />
                        <span class="error">
						    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
							    ValidationGroup="login" 
							    runat="server" 
							    Text="*" 
							    Display="Dynamic" 
							    ControlToValidate="_password" 
							    ErrorMessage="Please enter your password." />
                        </span>
					</td>
				</tr>
			</table>
            <div id="submitRow">
                <asp:CheckBox ID="_rememberMe" Checked="true" Text="Remember me" runat="server" />
				<div class="mt5">
					<a href="/forgottendetails/">Forgotten your details?</a>
				</div>
                <div id="buttonRow">
                    <div class="mt5">
                        <asp:Button runat="server" Text="Sign-In Now" ValidationGroup="login" OnClick="LoginHandler" CssClass="big mt10" ID="_submitBtn" />
                    </div>
                    <div class="mt10 error">
                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="login" runat="server" DisplayMode="BulletList" ShowSummary="true" />
                    </div>
                </div>
            </div>

            <hr class="hr3" />
			<h1 class="bold">Not a member?</h1>
			<h1 class="mb5">Join us now!</h1>
			&raquo; <b><a href="/register/">click to register now!</a></b>
			<div class="mt10">
				Members can:
				<ul style="margin-top: 10px; padding-top: 0px;">
					<li>Post up in the community forums</li>
					<li>Message other members</li>
					<li>View high-def photo-galleries</li>
                    <li>Leave comments</li>
					<li>Be kept up to date on the latest motorcycle events</li>
				</ul>
			</div>

        </form>
    </div>
</asp:Content>