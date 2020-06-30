<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DirectorySearchBox.ascx.cs" Inherits="Tetron.Controls.DirectorySearchBox" %>
<div align="right" style="width: <% if (StackedFormat){ %>300<% } else { %>400<% } %>px;">
	<form action="/directory/results.aspx" method="get" target="_top">
		<input type="text" name="s" style="margin-top: 0px; width: 150px;" value="<%= Criteria %>" />
		<input type="submit" value="search" />&nbsp;<a href="/help/directory" class="dark smallText" title="get help using the directory">(help)</a>
		<% if (StackedFormat){ %><br /><% } %>
		<% if (ShowModeControls) { %>
		<input class="Radio" type="radio" name="m" id="sm_concept" value="m"<%= ConceptChecked %> /><label for="sm_concept" title="search by subject, i.e. 'recovery' for all breakdown/recovery firms">subject</label>&nbsp;
		<input class="Radio" type="radio" name="m" id="sm_title" value="t"<%= TitleChecked %> /><label for="sm_title" title="search for a specific item title">title</label>
		<% } %>
	</form>
</div>