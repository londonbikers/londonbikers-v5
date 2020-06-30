<%@ Control Language="c#" Inherits="Tetron.Tools.Resources.Controls.Header" Codebehind="header.ascx.cs" %>
<html>
	<head>
		<title id="_title" runat="server"/>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="/tools/resources/styles/tools.css" type="text/css">
	</head>
	<body bgcolor="#FFFFFF" text="#000000" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" background="<%= PageBackgroundSource %>">
		<table width="100%" border="0" cellspacing="0" cellpadding="0">
			<tr> 
				<td background="/tools/resources/images/top-most-bg.gif" height="61" valign="top" width="209"><img src="/tools/resources/images/left-top.gif" width="209" height="19"><br />
					<img src="/tools/resources/images/title-left.gif" width="9" height="28"><a href="/tools/"><img src="/tools/resources/images/title-main.gif" width="113" height="28" border="0" alt="m-extranet home"></a><img src="/tools/resources/images/title-right.gif" width="87" height="28"><br />
					<img src="/tools/resources/images/title-bottom.gif" width="209" height="14"></td>
				<td background="/tools/resources/images/top-most-bg.gif" height="61" valign="bottom" align="right"> 
					<table border="0" cellspacing="0" cellpadding="0">
						<tr><td colspan="6" class="normal" align="right" style="padding-right: 5px; color: white;">Welcome back, <b><asp:Literal id="_userName" Runat="server" /></b></td></tr>
						<tr><td colspan="6"><img src="/tools/resources/images/t.gif" width="10" height="10"></td></tr>
						<tr valign="middle">
							<td id="_blankTab" runat="server" />
							<td id="_editorialTab" runat="server" width="80" height="19" align="center" />
							<td id="_galleryTab" runat="server" width="80" height="19" align="center" />
							<td id="_usersTab" runat="server" width="80" height="19" align="center" />
							<td id="_directoryTab" runat="server" width="80" height="19" align="center" />
							<td id="_adminTab" runat="server" width="80" height="19" align="center" />
						</tr>
					</table>
				</td>
			</tr>
			<tr> 
				<td background="/tools/resources/images/row-2-bg.gif" height="9"><img src="/tools/resources/images/t.gif" width="10" height="9"></td>
				<td background="/tools/resources/images/row-2-bg.gif" height="9"><img src="/tools/resources/images/t.gif" width="10" height="9"></td>
			</tr>
			<tr>
				<td height="19">
					<table width="209" border="0" cellspacing="0" cellpadding="0" height="19">
						<tr> 
							<td background="/tools/resources/images/row-3-bg-left.gif" width="109" align="center" valign="middle"><img src="/tools/resources/images/ident-home.gif" width="33" height="11"></td>
							<td background="/tools/resources/images/row-3-bg-right.gif"><img src="/tools/resources/images/row-3-left-item.gif" width="11" height="19"></td>
						</tr>
					</table>
				</td>
				<td background="/tools/resources/images/row-3-bg-right.gif"><img src="/tools/resources/images/t.gif" width="10" height="19"></td>
			</tr>
		</table>
		<table width="100%" border="0" cellspacing="0" cellpadding="0">
			<tr>
				<td width="109">&nbsp;</td>
				<td background="/tools/resources/images/light-vert-div-bg.gif" width="1"><img src="/tools/resources/images/t.gif" width="1" height="10"></td>
				<td bgcolor="#FFFFFF"><img id="_pageLabel" runat="server" height="25"/></td>
			</tr>
			<tr>
				<td width="109" background="/tools/resources/images/divs-light.gif" height="1"><img src="/tools/resources/images/t.gif" width="10" height="1"></td>
				<td background="/tools/resources/images/light-vert-div-bg.gif" width="1" height="1"><img src="/tools/resources/images/t.gif" width="1" height="1"></td>
				<td background="/tools/resources/images/divs-light.gif" height="1"><img src="/tools/resources/images/t.gif" width="10" height="1"></td>
			</tr>
			<tr>
				<td width="109" valign="top"> 
					<table width="100%" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td background="/tools/resources/images/options-box-bg.gif">
								<table width="100%" border="0" cellspacing="0" cellpadding="4">
									<tr>
										<td class="text">
											<a id="_signinHref" runat="server" title="sign in to your account">&#187; sign-in<br/></a>
											<asp:Literal id="_sectionMenu" runat="server"/>
											<br />
											<br />
											&laquo; <a href="/">back to LB</a>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td background="/tools/resources/images/divs-light.gif" height="1"><img src="/tools/resources/images/t.gif" width="10" height="1"></td>
						</tr>
					</table>
				</td>
				<td background="/tools/resources/images/light-vert-div-bg.gif" width="1"><img src="/tools/resources/images/t.gif" width="1" height="10"></td>
				<td valign="top">
					<table width="100%" border="0" cellspacing="0" cellpadding="10">
						<tr>
							<td class="text">