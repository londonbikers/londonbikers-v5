<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="gallery.aspx.cs" Inherits="Tetron.Galleries.Gallery" %>
<%@ Register TagPrefix="Controls" TagName="FacebookLike" Src="~/_controls/FacebookLike.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Comments" Src="~/_controls/Comments.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/gallery.css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#showIntro").click(function () {
                $("#desc").slideToggle('slow');
                return false;
            });
        });
    </script>
    <asp:Literal ID="_preRenderLink" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="head">
        <h1><asp:Literal ID="_title" runat="server" /></h1>
    </div>

    <div class="photos">

        <div class=" mt10 mb10" id="desc">
            <div class="subTitled mb10">
                <asp:Literal ID="_photoCount" runat="server" /> photos | Gallery Shown in: <asp:HyperLink ID="_category" runat="server" />
            </div>
            <div class="smallText">
                <asp:Literal ID="_description" runat="server" />
            </div>
        </div>

        <div class="floatLeft">
            <Controls:FacebookLike runat="server" />
        </div>
        <div class="floatRight">
            <div id="showIntro" class="rollBtn rounded">show introduction</div>
        </div>
        
        <div class="clear"></div>
        <hr class="hr3 mb10" />
        
        <asp:Repeater 
		    id="_photos" 
		    runat="server" 
		    OnItemCreated="PhotosHandler">
		    <ItemTemplate>
                <div class="floatLeft mr10 frame">
                    <asp:Literal ID="_imageLink" runat="server" />
                    <div>
                        <asp:Literal ID="_comments" runat="server" />
                        <asp:Literal ID="_title" runat="server" />
                    </div>
                </div>
                <asp:Literal ID="_rowSep" runat="server" Visible="false"><div class="clear"></div></asp:Literal>
            </ItemTemplate>
	    </asp:Repeater>
        <div class="clear"></div>

        <hr class="hr3" />
        <Controls:Comments id="_comments" runat="server" />
     
     </div>
</asp:Content>