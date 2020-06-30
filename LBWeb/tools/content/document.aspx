<%@ Page language="c#" Inherits="Tetron.Tools.Content.DocumentPage" Codebehind="document.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<Controls:Header id="_header" runat="server"/>
<style type="text/css">
	.halfmoon {
		margin-bottom: 0px; 
	}
	.halfmoon ul {
		padding: 3px 9px 2px 0px;
		margin-left: 0;
		margin-top: 1px;
		margin-bottom: 0;
		font: bold 10px Verdana;
		list-style-type: none;
		text-align: left;
	} 
	.halfmoon li {
		display: inline; 
		margin: 0;
	}
	.halfmoon li a { 
		text-decoration: none;
		padding: 3px 9px 2px 5px;
		margin: 0;
		margin-right: 0; /*distance between each tab*/
		border-left: 1px dotted #DDD;
		color: #818181;
		font: bold 11px Verdana;
		background: #fff url(../resources/images/tabright.gif) top right no-repeat;
	}
	.halfmoon li a:visited {
		color: #818181; 
	}
	.halfmoon li a:hover, .halfmoon li a.current {
		background-color: #f5f5f5; 
		color: #272727;
	}
	.tabcontent {
		display:none; 
	}
	img { border: 0px; }
</style>
<script type="text/javascript">
	//Set tab to intially be selected when page loads:
	//[which tab (1=first tab), ID of tab content to display (or "" if no corresponding tab content)]:
	var initialtab = [1, ""]

	//Turn menu into single level image tabs (completely hides 2nd level)?
	var turntosingle = 0 //0 for no (default), 1 for yes

	//Disable hyperlinks in 1st level tab images?
	var disabletablinks = 0 //0 for no (default), 1 for yes

	////////Stop editting////////////////
	var previoustab = ""
	if (turntosingle == 1)
		document.write('<style type="text/css">\n#tabcontentcontainer{display: none;}\n</style>')

	function expandcontent(cid, aobject){
		if (disabletablinks==1)
			aobject.onclick=new Function("return false")
		if (document.getElementById && turntosingle==0){
			highlighttab(aobject)
			if (previoustab!="")
				document.getElementById(previoustab).style.display="none"
			if (cid!=""){
				document.getElementById(cid).style.display="block"
				previoustab=cid
			}
		}
	}

	function highlighttab(aobject){
		if (typeof tabobjlinks=="undefined")
			collectddimagetabs()
		for (i=0; i<tabobjlinks.length; i++)
			tabobjlinks[i].className=""
		aobject.className="current"
	}

	function collectddimagetabs(){
		var tabobj=document.getElementById("ddimagetabs")
		tabobjlinks=tabobj.getElementsByTagName("A")
	}

	function do_onload(){
		collectddimagetabs()
		expandcontent(initialtab[1], tabobjlinks[initialtab[0]-1])
	}

	if (window.addEventListener)
		window.addEventListener("load", do_onload, false)
	else if (window.attachEvent)
		window.attachEvent("onload", do_onload)
	else if (document.getElementById)
		window.onload=do_onload
</script>
<script language="javascript" type="text/javascript" src="/_system/tiny_mce/tiny_mce.js"></script>
<script language="javascript" type="text/javascript">
	tinyMCE.init({
		theme : "advanced",
		mode : "exact",
		elements : "_body",
		valid_child_elements : "object[param|embed]",
		editor_selector : "mceEditor",
		theme_advanced_toolbar_location : "top",
		content_css : "/css/mce.css",
		plugins : "table,media,fullscreen",
		theme_advanced_buttons1_add: "fullscreen",
        theme_advanced_buttons2_add_before : "media",
		theme_advanced_buttons3_add_before: "tablecontrols,separator",
        fullscreen_new_window : false,
        fullscreen_settings : {
                theme_advanced_path_location : "top"
        },
		apply_source_formatting: false,
		verify_html: true,
		fix_list_elements: true,
        invalid_elements : "div",
        verify_css_classes: true,
        gecko_spellcheck: true
	});
	
	function toggleLayer(layer){
		if (layer == "DocumentDiv") {
			document.getElementById("DocumentDiv").style.display = "block";
			document.getElementById("AssociateDiv").style.display = "none";
			highlighttab(document.getElementById("no1"));
		}
		else if (layer == "ImagesDiv") {
			document.getElementById("DocumentDiv").style.display = "none";
			document.getElementById("AssociateDiv").style.display = "block";
			highlighttab(document.getElementById("no2"));
			window._sidepane.location.href = "windows/image-container.aspx?container=<%= _documentContainer.Value %>";
		}
		else if (layer == "TagsDiv") {
			document.getElementById("DocumentDiv").style.display = "none";
			document.getElementById("AssociateDiv").style.display = "block";
			highlighttab(document.getElementById("no3"));
			window._sidepane.location.href = "windows/tag-container.aspx?container=<%= _documentContainer.Value %>";
		} else {
			document.getElementById("DocumentDiv").style.display = "none";
			document.getElementById("AssociateDiv").style.display = "block";
			highlighttab(document.getElementById("no4"));
			window._sidepane.location.href = "windows/related-container.aspx?container=<%= _documentContainer.Value %>";
		}
	}
	
	function TypeChange(){
		if (document.getElementById("_type").value == "Generic") {
			document.getElementById("genericDiv").style.display = "block";
			document.getElementById("_mceContainer").style.display = "none";
			document.getElementById("_bodyPlain").style.display = "block";
		} else {
			document.getElementById("genericDiv").style.display = "none";
			document.getElementById("_mceContainer").style.display = "block";
			document.getElementById("_bodyPlain").style.display = "none";
		}
	}
</script>
<form runat="server">
	<input type="hidden" id="_documentContainer" runat="server" name="_documentContainer" />
	<div id="ddimagetabs" class="halfmoon" style="float: left;">
		<ul>
			<li id="no1"><a href="javascript:toggleLayer('DocumentDiv');">Content</a></li>
			<li id="no2"><a href="javascript:toggleLayer('ImagesDiv');">Images</a></li>
			<li id="no3"><a href="javascript:toggleLayer('TagsDiv');">Tags</a></li>
			<li id="no4"><a href="javascript:toggleLayer('RelatedDiv');">Related</a></li>
		</ul>
	</div>
	<div align="right" class="general" style="margin-bottom: 4px;">
	    <img src="/tools/resources/images/t.gif" width="1" height="15" alt="" /><asp:ImageButton id="_deleteBtn" imageurl="/tools/resources/images/btn_delete.gif" onclick="DeleteDocumentHandler" runat="server" OnClientClick="return(confirm('Are you sure?'));" />
	</div>
	<div class="LightNoteBox">	
		<div id="DocumentDiv">
			<div style="background-color: #eaeaea; border-top: 1px #d2d2d2 dotted; border-bottom: 1px #CCCCCC solid; padding: 4px;">
				<div style="float: left; padding-top: 1px;" class="general">
					Created: 
					<asp:Label cssclass="lightgrey" id="_created" runat="server"/>
				</div>
				<div align="right" class="general">
					Status: <asp:DropDownList id="_status" runat="server" />&nbsp;
					Author: <asp:DropDownList id="_authorList" runat="server" /> or&nbsp;&nbsp;<i>forum-id</i>: <asp:TextBox id="_authorID" runat="server" class="box" style="width: 45px;" />&nbsp;
					Type: <asp:DropDownList id="_type" runat="server" OnChange="TypeChange()" />
				</div>
			</div>
			<div id="genericDiv" style="display: <%= GenericDivCssDisplay %>; color: #595959; background-color: #e1e1e1; border-top: 1px #d2d2d2 dotted; border-bottom: 1px #d6d6d6 solid; padding: 4px;" align="right">
				Choose a section: <asp:DropDownList id="_sectionList" runat="server" />
			</div>
			<div style="margin-top: 5px;" class="lightgrey">
				Title:
			</div>
			<asp:TextBox id="_title" runat="server" style="font-weight: bold; width:100%; margin-bottom: 5px; height: 27px; margin-top: 3px; padding: 5px;" width="307" class="box" /><br />						
			<span class="lightgrey">
				Abstract:
			</span>
			<br />
			<asp:TextBox id="_abstract" textmode="multiline" style="width:100%; margin-bottom: 5px; margin-top: 3px;" rows="3" runat="server" /><br />
			<div class="lightgrey" style="margin-bottom: 5px;">
				Body:
			</div>
			<div id="_mceContainer" runat="server">
				<asp:TextBox 
					id="_body" 
					textmode="multiline" 
					style="width: 100%; height: 470px;" 
					cssclass="mceEditor"
					runat="server" />
			</div>
			<asp:TextBox 
				id="_bodyPlain" 
				textmode="multiline" 
				style="width: 100%; height: 470px;" 
				rows="35" 
				runat="server" />
			<div class="GreyNoteBox" style="text-align: right; margin-top: 10px; padding: 5px;">
				<div align="left">
					<span class="prompt">
						<asp:Literal id="_prompt" runat="server"/>
					</span>
				</div>
				<asp:HyperLink id="_preview" title="save any changes before previewing" target="_blank" runat="server" ImageURL="../resources/images/btn_preview.gif" visible="false" enableviewstate="false" /><asp:ImageButton style="margin-left: 5px;" id="_save" AlternateText="save document" onclick="SaveDocumentEventHandler" runat="server" ImageUrl="../resources/images/btn_save.gif" /></div>
		</div>
		<div id="AssociateDiv" style="display: none;">
			<iframe id="_sidepane" name="_sidepane" runat="server" scrolling="no" marginwidth="0" marginheight="0" frameborder="0" width="100%" height="685" />
		</div>
	</div>
</form>
<Controls:Footer id="_footer" runat="server"/>