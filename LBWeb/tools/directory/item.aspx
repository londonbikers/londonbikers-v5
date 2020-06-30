<%@ Reference Control="~/tools/resources/controls/header.ascx" %>
<%@ Page language="c#" Inherits="Tetron.Tools.Directory.ItemPage" Codebehind="item.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<controls:header id="_header" runat="server"/>
<form runat="server">
	<b>Edit A Directory Item</b><br />
	<asp:LinkButton id="_deleteLink" runat="server" text="delete item" onclick="DeleteItemHandler" tooltip="permanently delete this item..." />
	<br />
	<br />
	<span class="Prompt">
		<asp:Literal id="_prompt" runat="server" />
	</span>
	<table border="0" cellpadding="0" cellspacing="0" width="100%">
		<tr>
			<td background="/_images/dotted-div.gif"><img src="/_images/t.gif" width="1" height="1" /></td>
		</tr>
		<tr>
			<td bgcolor="#F8F9F6" style="padding-top:10px; padding-bottom:10px;">
				<table border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td width="100" style="padding-left:15px;">
							<div class="text">
								<span class="Prompt">*</span> Title
							</div>
						</td>
						<td>
							<div class="text">
								<asp:TextBox id="_title" runat="server" cssclass="MedTextBox" style="width: 250px;" />&nbsp;&nbsp;
								<img src="/_images/left-med-arrow.gif" width="14" height="5" align="absmiddle" />
								This will be how the item will be shown when listed.
							</div>
						</td>
					</tr>
					<tr>
						<td valign="top" style="padding-top: 3px;">
							<div class="text" style="padding-left:15px;">
								<span class="Prompt">*</span> Description
							</div>
						</td>
						<td>
							<asp:TextBox id="_description" textmode="MultiLine" runat="server" cssclass="FullWidth" />
						</td>
					</tr>
					<tr>
						<td width="100" style="padding-left:15px;">
							<div class="text">
								&nbsp;&nbsp;&nbsp;Telephone
							</div>
						</td>
						<td>
							<div class="SmallText">
								<asp:TextBox id="_telephoneNumber" runat="server" cssclass="MedTextBox" style="width: 250px;" />
							</div>
						</td>
					</tr>
					<tr>
						<td width="100" style="padding-left:15px;">
							<div class="text">
								&nbsp;&nbsp;&nbsp;Postcode
							</div>
						</td>
						<td>
							<div class="text">
								<asp:TextBox id="_postcode" maxlength="9" runat="server" cssclass="MedTextBox" style="width: 75px;" />
								Recognised? <b><asp:Literal id="_locationRecognised" runat="server" /></b>
							</div>
						</td>
					</tr>
					<tr>
						<td valign="top" style="padding-left:15px; padding-top: 5px;">
							<div class="text">
								&nbsp;&nbsp;&nbsp;Submitter
							</div>
						</td>
						<td class="text" style="padding-top: 5px;">
							<img src="../resources/images/ico_user_small.gif" style="vertical-align: middle;" /> <b><asp:HyperLink id="_submitterLink" runat="server" cssclass="bluelink" /></b>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td background="./_images/dotted-div.gif"><img src="/_images/trans.gif" width="1" height="1" /></td>
		</tr>
	</table>
	<br />
	<table border="0" cellpadding="0" cellspacing="0" width="100%">
		<tr>
			<td width="115" valign="top" style="padding-top: 3px;">
				<div class="text" style="padding-left:15px;">
					&nbsp;&nbsp;&nbsp;Keywords
				</div>
			</td>
			<td>
				<div class="text">
					<asp:TextBox id="_keyword" runat="server" cssclass="MedTextBox" style="width: 250px;" />
					&nbsp;&nbsp;<img src="./_images/left-med-arrow.gif" width="14" height="5" align="absmiddle" />
					Comma seperated, i.e. "<i>honda, suzuki</i>", to help with searches.
				</div>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				&nbsp;
			</td>
		</tr>
		<tr>
			<td width="115" valign="top" style="padding-top: 3px;">
				<div class="text" style="padding-left:15px;">
					<span class="Prompt">*</span> Categories
				</div>
			</td>
			<td>
				<div class="text">
					<asp:DropDownList id="_categorySelector" runat="server" style="height: 18px" />
					<asp:Button runat="server" text="add" cssclass="ActionBtn" onclick="AddCategoryHandler" style="vertical-align: bottom" ID="Button1"/>
					<br />
					<asp:Repeater id="_categoriesList" runat="server" OnItemCreated="CategoryItemCreatedHandler">
						<HeaderTemplate>
							<ul style="margin-top: 15px;">
						</HeaderTemplate>
						<ItemTemplate>
								<li>(<asp:HyperLink id="_removeCategoryLink" runat="server" CssClass="CopperLink" text="remove" />) <asp:HyperLink cssclass="BlueLink" id="_listLink" runat="server" /></li>
						</ItemTemplate>
						<FooterTemplate>
							</ul>
						</FooterTemplate>
					</asp:Repeater>
				</div>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				&nbsp;
			</td>
		</tr>
		<tr>
			<td valign="top" style="padding-top: 3px;">
				<div class="text" style="padding-left:15px;">
					&nbsp;&nbsp;&nbsp;Links
				</div>
			</td>
			<td>
				<div class="text">
					<asp:TextBox id="_link" runat="server" cssclass="MedTextBox" style="width: 250px;" />
					<asp:Button runat="server" text="add" cssclass="ActionBtn" onclick="AddLinkHandler" />
					&nbsp;&nbsp;<img src="./_images/left-med-arrow.gif" width="14" height="5" align="absmiddle" />
					Enter the URL (address) of a site.
				</div>
				<div class="text">
					<asp:Repeater id="_linkList" runat="server" OnItemCreated="LinkItemCreatedHandler">
						<HeaderTemplate>
							<ul>
						</HeaderTemplate>
						<ItemTemplate>
								<li>(<asp:HyperLink id="_removeLinkLink" runat="server" CssClass="CopperLink" text="remove" />) <asp:HyperLink cssclass="BlueLink" id="_listLink" runat="server" /></li>
						</ItemTemplate>
						<FooterTemplate>
							</ul>
						</FooterTemplate>
					</asp:Repeater>
				</div>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				&nbsp;
			</td>
		</tr>
		<tr>
			<td valign="top" style="padding-top: 3px;">
				<div class="text" style="padding-left:15px;">
					&nbsp;&nbsp;&nbsp;Images
				</div>
			</td>
			<td>
				<div class="text">
					<asp:TextBox id="_imageURL" runat="server" cssclass="MedTextBox" style="width: 250px;" />
					<asp:Button runat="server" text="add" cssclass="ActionBtn" onclick="AddImageHandler" />
					&nbsp;&nbsp;<img src="/_images/left-med-arrow.gif" width="14" height="5" align="absmiddle" />
					Enter the URL (address) of an image.
				</div>
				<div class="text">
					<asp:Repeater id="_imageList" runat="server" OnItemCreated="ImageItemCreatedHandler">
						<HeaderTemplate>
							<br />
						</HeaderTemplate>
						<ItemTemplate>
							<table cellpadding="0" cellspacing="0" width="10">
								<tr>
									<td class="ThumbCell"><asp:Image id="_listImage" runat="server" /></td>
								</tr>
								<tr>
									<td align="center" style="padding-top:3px;">
										<div class="text">
											(<asp:HyperLink id="_removeImageLink" runat="server" cssclass="CopperLink" text="remove" />)
										</div>
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
			<td colspan="2">
				&nbsp;
			</td>
		</tr>
		<tr>
			<td colspan="2" background="/_images/dotted-div.gif"><img src="/_images/t.gif" width="1" height="1" /></td>
		</tr>
		<tr>
			<td>
				&nbsp;
			</td>
			<td style="padding-top:5px;">
				<div class="text">
					* Please supply a name, description and a phone number, or link.<br />
					** Images should be reasonably sized (200px wide max).<br />
					<span class="Prompt">*</span> = required
				</div>
				<asp:Button 
					runat="server" 
					title="update item" 
					text="update item" 
					cssclass="BoxSubmit" 
					style="font-weight: bold;" 
					onclick="UpdateItemHandler" />
			</td>
		</tr>
	</table>
</form>
<controls:footer id="_footer" runat="server"/>