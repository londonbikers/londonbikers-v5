<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="submit.aspx.cs" Inherits="Tetron.Directory.Submit" %>

<%@ Register TagPrefix="Controls" TagName="SearchBox" Src="~/_controls/DirectorySearchBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/directory.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="searchBox">
        <div class="floatLeft">
            <h2>
                The Directory</h2>
        </div>
        <div class="floatRight">
            <Controls:SearchBox ShowModeControls="false" ID="_searchBox" runat="server" />
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="padded">
        <form runat="server">
        <h1>
            <span class="lightText">Submit an item to:</span>
            <asp:Literal ID="_categoryName" runat="server" />
        </h1>
        <asp:PlaceHolder ID="_cancelArea" runat="server">
            <img src="/_images/silk/cancel.png" width="16" height="16" alt="" />
            <asp:LinkButton ID="_cancelLink" runat="server" Text="cancel submission" OnClick="CancelItem" CssClass="smallText" />
        </asp:PlaceHolder>
        <hr />
        <div class="standoutBox mb10">
            The Directory is built upon the submissions of our members. You can help to enhance
            the directory by submitting an item below.
            <br />
            <br />
            Please fill in as many details as possible. Once submitted, one of our team of moderators
            will review it and ensure it's as good as can be, before it gets put into the directory.
            Please fill out as many details as possible below to have your item look as good
            as it can and to help people find it easily.
            <br />
            <br />
            <img src="/_images/silk/help.png" width="16" height="16" alt="" />
            <a href="../help/item-submission.aspx" target="_blank" class="Flat">submission help</a>
        </div>
        <div class="errorBox mb20" id="_prompt" runat="server" visible="false" />
        <div id="_form" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td width="120" valign="top">
                        <b>* Title</b>
                    </td>
                    <td>
                        <asp:TextBox ID="_title" runat="server" Style="width: 527px;" class="padded" />
                        <div class="smallText lightText">
                            This is what the item will be called, when listed.
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>* Description</b>
                    </td>
                    <td>
                        <asp:TextBox ID="_description" TextMode="MultiLine" runat="server" class="padded"
                            Style="width: 517px; height: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Telephone No.
                    </td>
                    <td>
                        <div class="smallText">
                            <asp:TextBox ID="_telephoneNumber" runat="server" Style="width: 200px;" class="padded" />
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
                        Postcode
                    </td>
                    <td>
                        <asp:TextBox ID="_postcode" MaxLength="9" runat="server" Style="width: 200px;" class="padded" />
                        <div class="smallText lightText">
                            This will help us show a map.
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        Keywords
                    </td>
                    <td>
                        <asp:TextBox ID="_keyword" runat="server" Style="width: 527px;" class="padded" />
                        <div class="smallText lightText">
                            Comma seperated, i.e. "<i>honda, suzuki</i>", to help people find your item.
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        Links
                    </td>
                    <td>
                        <asp:TextBox ID="_link" runat="server" Style="width: 300px;" class="padded" />
                        <asp:Button runat="server" Text="add" CssClass="ActionBtn" OnClick="AddLinkHandler" />
                        <div class="smallText lightText">
                            Enter the address of a site. i.e. http://www.google.com
                        </div>
                        <asp:Repeater ID="_linkList" runat="server" OnItemCreated="LinkItemCreatedHandler">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>(<asp:LinkButton ID="_removeLinkLink" OnCommand="RemoveObjectHandler" runat="server"
                                    Text="remove" />)
                                    <asp:HyperLink ID="_listLink" runat="server" /></li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        Images
                    </td>
                    <td>
                        <asp:TextBox ID="_imageURL" runat="server" class="padded" Style="width: 300px;" />
                        <asp:Button runat="server" Text="add" CssClass="ActionBtn" OnClick="AddImageHandler" />
                        <div class="smallText">
                            Enter the address of an image. <span class="lightText">i.e. http://google.com/ourimage.jpg</span>
                        </div>
                        <div class="mb10">
                            <asp:Repeater ID="_imageList" runat="server" OnItemCreated="ImageItemCreatedHandler">
                                <HeaderTemplate>
                                    <br />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="ThumbCell">
                                                <asp:Image ID="_listImage" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" class="pt5">
                                                (<asp:LinkButton OnCommand="RemoveObjectHandler" ID="_removeImageLink" runat="server"
                                                    Text="remove" />)
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    <br />
                                </SeparatorTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button CssClass="big mb10" runat="server" title="submit my item" Text="submit my item"
                            OnClick="SubmitItemHandler" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <div class="smallText lightText">
                            <i>- Please supply a name, description and a phone number, or link.<br />
                                - By submitting an item to our Directory, you are agreeing to our <a target="_blank"
                                    title="read the item submission guidelines" href="/help/submission-guidelines.aspx">
                                    guidelines</a>. </i>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="_successPanel" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <div align="center" style="padding: 0px 10px 0px 10px;">
                            <b>Thank-you!</b>
                            <br />
                            <br />
                            Your submission will now be reviewed by us. Once the review is complete, you will
                            receive confirmation of it being published within the Directory by email.
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </div>
</asp:Content>
