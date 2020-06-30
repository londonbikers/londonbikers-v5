<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomepageMemberStats.ascx.cs" Inherits="Tetron.Controls.HomepageMemberStats" %>

   <div style="padding: 5px; background-color: #eee; border-bottom: dotted 1px #cecdc1;">
        <table class="Spacer" cellpadding="0" style="width: 398px;">
            <tr>
                <td>
                    <span class="smallText bold">Total Members: <asp:Literal ID="_totalMemberCount" runat="server" /></span>
                </td>
                <td align="right">
                    <asp:HyperLink ID="_joinUsLink" runat="server" CssClass="dark" Text="join us" NavigateUrl="/register/" />
                </td>
            </tr>
        </table>
    </div>

    <div class="padded5">

        <table cellpadding="0" cellspacing="0" width="205">
            <tr>
                <td style="width: 10px; padding-right: 10px;" class="smallText">
                    <b><asp:Literal ID="_newPosts" runat="server" /></b>
                </td>
                <td class="smallText">
                    Forum posts this month
                </td>
            </tr>
            <tr>
                <td colspan="2"><div class="dottedLine" /></td>
            </tr>
            <tr>
                <td class="smallText" style="width: 10px; padding-right: 10px;">
                    <b><asp:Literal ID="_newPMs" runat="server" /></b>
                </td>
                <td class="smallText">
                    Private messages this month
                </td>
            </tr>
            <tr>
                <td colspan="2"><div class="dottedLine" /></td>
            </tr>
            <tr>
                <td class="smallText" style="width: 10px; padding-right: 10px;">
                    <b><asp:Literal ID="_newMembers" runat="server" /></b>
                </td>
                <td class="smallText">
                    New members this month
                </td>
            </tr>
            <tr>
                <td colspan="2"><div class="dottedLine" /></td>
            </tr>
        </table>

        <div class="smallText bold arial mt5">
            Top members this month:
        </div>
        <asp:Repeater 
            OnItemCreated="ItemCreatedHandler" 
            ID="_topPosters"
            runat="server">
            <HeaderTemplate>
                <div style="height: 5px; background-color: #f5f5f5; margin-top: 5px;"></div>
                <table cellspacing="0" style="width: 205px;" class="mt5">
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="statLightGridRow">
                    <td width="10" nowrap="nowrap" class="bold smallText">
                        #<asp:Literal ID="_position" runat="server" />
                    </td>
                    <td width="16">
                        <img src="/_images/silk/user.png" alt="" /> 
                    </td>
                    <td nowrap="nowrap" width="10" class="smallText lightText">
                        (<asp:Literal ID="_postCount" runat="server" />)
                    </td>
                    <td class="smallText">
                        <asp:HyperLink ID="_userLink" runat="server" />    
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="statLightGridAltRow">
                    <td width="10" nowrap="nowrap" class="bold smallText">
                        #<asp:Literal ID="_position" runat="server" />
                    </td>
                    <td width="16">
                        <img src="/_images/silk/user.png" alt="" /> 
                    </td>
                    <td nowrap="nowrap" width="10" class="smallText lightText">
                        (<asp:Literal ID="_postCount" runat="server" />)
                    </td>
                    <td class="smallText">
                        <asp:HyperLink ID="_userLink" runat="server" />    
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>            
            </FooterTemplate>
        </asp:Repeater>
    </div>
