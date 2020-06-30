<%@ Page language="c#" Inherits="Tetron.Tools.Gallery.GalleryPage" Codebehind="gallery.aspx.cs" %>
<%@ Register TagPrefix="Controls" TagName="Header" Src="../resources/controls/header.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Footer" Src="../resources/controls/footer.ascx" %>
<%@ Register TagPrefix="Controls" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<controls:header id="_header" runat="server"/>
<script type="text/javascript">
	function SetGlobalName() {
	    var name = document.getElementById('quickName').value;
	    if (name == '')
	        return;
	    
	    var names = document.getElementsByTagName('INPUT');
	    for (i = 0; i <= names.length; i++) {
	        if (names[i].name.indexOf('i_name_') > -1)
	            names[i].value = name;
	    }
	}
	
	function SetGlobalCredit() {
	    var name = document.getElementById('quickCredit').value;
	    if (name == '')
	        return;
	    
	    var names = document.getElementsByTagName('INPUT');
	    for (i = 0; i <= names.length; i++) {
	        if (names[i].name.indexOf('i_credit_') > -1)
	            names[i].value = name;
	    }
	}
</script>
<form runat="server" enctype="multipart/form-data">
	<b><asp:Literal id="_pageMode" runat="server" /> a gallery</b> <asp:HyperLink id="_deleteGalleryLink" runat="server" />
	<br />
	<br />
	<table width="100%" cellpadding="0" cellspacing="0">
		<tr>
			<td class="normal" style="padding-right: 10px;">
				Title:<br />
				<asp:TextBox id="_title" cssclass="box" style="margin-top: 5px; width: 100%;" runat="server" />
			</td>
			<td class="normal" style="padding-right: 10px;" width="140px">
				Date: <i>(optional)</i><br />
				<asp:TextBox id="_publishDate" cssclass="box" style="margin-top: 5px; width: 140px;" runat="server" />
			</td>
			<td class="normal" width="85">
				Status: <br />
				<asp:DropDownList id="_status" runat="server" style="margin-top: 5px;" />
			</td>
		</tr>
	</table>
	<br />
	Description:<br />
	<asp:TextBox 
		id="_description" 
		textmode="multiline" 
		cssclass="box" 
		style="margin-top: 5px; width: 100%; height: 200px;" 
        runat="server" />
	<br />
	<br />
	<table cellpadding="0" cellspacing="0" width="100%">
		<tr>
			<td>
				<asp:Label id="_metaPrompt" runat="server" cssclass="prompt" />
			</td>
			<td align="right">
				<asp:Button onclick="UpdateGalleryHandler" runat="server" text="update details" cssclass="btn" />
			</td>
		</tr>
	</table>

    <asp:Placeholder id="_persistedView" runat="server">
        <hr class="thinsilver" style="margin-top: 10px; margin-bottom: 20px;" />
        <h3>Upload Photos</h3>
        <b>Browser Upload</b>
        <div style="padding: 10px 0px 10px 0px;">
            <table 
		        border="0" 
		        cellpadding="5" 
		        cellspacing="0" 
		        width="555" 
		        style="border-style:solid; border-width: 1px; border-color: #cccccc;">
		    <tr>
			    <td style="background-color: #eeeeee;" class="normal">
				    <b>Add Images</b>
				    <br />
				    <br />
				    <Controls:UploadApplet 
					    id="_uploader" 
					    runat="server" 
					    AppletLocation="../../_system/ActiveUpload.jar"
					    License="F4165B29CA61E63FEFD862FCA76A9A9EA4DF3094174678783734185AD2EC6166CC57335BCCD4F4D0">
				    </Controls:UploadApplet>
			    </td>
		    </tr>
	    </table>
        </div>

        <b>FTP Upload</b>
        <div style="padding: 10px 0px 10px 0px;">
        <div style="margin-bottom: 10px;">
            <asp:RadioButtonList id="_ftpImportLocation" runat="server" AutoPostback="true" />
        </div>
        <table cellspacing="0">
            <tr>
                <td>
                    <div style="border: 3px solid #ccc; display: inline-block; padding: 5px 10px 5px 5px;">
                        <asp:TreeView id="_ftpTree" runat="server" SelectedNodeChanged="FtpTreeNodeChangedHandler" />
                    </div>
                </td>
                <td valign="top" style="padding-left: 10px;">
                    <asp:Placeholder id="_ftpContentView" visible="false" runat="server">
                        <div class="NoteBox" style="display: inline-block;">
                            <asp:Literal id="_ftpContentSummary" runat="server" />
                        </div>
                        <asp:Button ID="_ftpImportBtn" 
				            OnClientClick="if(!confirm('Are you sure?')){return false;}"
				            runat="server" 
				            Text="okay, import these" 
				            OnClick="ImportFtpPhotosHandler" 
                            style="margin-top: 10px; display: block;"
				            CssClass="BigBtn" />
                    </asp:Placeholder>
                </td>
            </tr>
        </table>
    </div>

        <h3>Edit Image Details</h3>
        <table>
	        <tr>
	            <td>Name:</td>
	            <td><input type="text" id="quickName" /> <a href="#" onclick="SetGlobalName(); return false;">set</a></td>
	        </tr>
	        <tr>
	            <td>Credit:</td>
	            <td><input type="text" id="quickCredit" /> <a href="#" onclick="SetGlobalCredit(); return false;">set</a></td>
	        </tr>
	    </table>

	    <br />
	    <br />
        <div id="_exhibitsDiv" runat="server">
		    <asp:Literal id="_exhibitGrid" runat="server" />
		    <hr class="ThinSilver" />
		    <div align="right">
			    <asp:Button onclick="UpdateExhibitsHandler" runat="server" text="Update Exhibits" CssClass="btn" />
		    </div>
	    </div>
	    <br />
	    <br />
    </asp:Placeholder>

</form>
<controls:footer id="_footer" runat="server"/>