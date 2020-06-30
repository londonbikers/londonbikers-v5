<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="photo.aspx.cs" Inherits="Tetron.Galleries.Photo" %>
<%@ Register TagPrefix="Controls" TagName="FacebookLike" Src="~/_controls/FacebookLike.ascx" %>
<%@ Register TagPrefix="Controls" TagName="Comments" Src="~/_controls/Comments.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="/css/photo.css" />
    <script type="text/javascript">
        $(window).load(function () {
            var width = $("#frame img, #frame1600 img").width();
            var height = $("#frame img, #frame1600 img").height();
            if (height > width) {
                $("#frame img, #frame1600 img").addClass('tall');
            }
            if (height > width && height == 1600) {
                $("#frame1600").css("width", width + 2);
                $("#frame1600").css("left", (985 - width) / 2);
            }
            $("#normalSizeBtn").click(function () {
                window.location = "/galleries/res/1024/?ref=" + window.location;
            });
            $("#superSizeBtn").click(function () {
                window.location = "/galleries/res/1600/?ref=" + window.location;
            });
        });
    </script>
    <asp:Literal ID="_preRenderLink" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="<%= FrameId %>"><asp:Literal ID="_photo" runat="server" /></div>
    <div class="floatLeft" id="sumpLeft">
        <div id="photoTitle">
            <table>
                <tr>
                    <td>
                        <span>Title:</span> <asp:Literal ID="_title" runat="server" />
                    </td>
                    <td id="btnCell">
                        <div id="normalSizeBtn" class="sizeSelected sizeBtn rounded">
                            normal
                        </div>
                        <div id="superSizeBtn" class="sizeBtn rounded">
                            supersize
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div class="subTitled padded" id="photographer">
            <Controls:FacebookLike runat="server" />
            <div class="floatRight mt5">
                Photographer: <b><asp:Literal id="_photographer" runat="server" /></b>
            </div>
            <div class="clear"></div>
        </div>
            
        <div class="subTitled padded">
            Gallery: <asp:Hyperlink ID="_galleryLink" runat="server" /> | 
            Shown in: <asp:HyperLink ID="_category" runat="server" />
            <asp:PlaceHolder ID="_viewsHolder" runat="server" Visible="false">
                | Views: <asp:Literal ID="_views" runat="server" />
            </asp:PlaceHolder>
        </div>
        <asp:PlaceHolder ID="_commentHolder" runat="server">
            <div id="comment"><asp:Literal ID="_caption" runat="server" /></div>
        </asp:PlaceHolder>

    </div>

    <div class="floatRight">
        <asp:PlaceHolder id="_prevThumbGhost" runat="server" visible="false"><div class="floatLeft navThumb"><img src="/_images/gallery-end.gif" alt="start" /><div class="navTitle">previous</div></div></asp:PlaceHolder>
        <asp:PlaceHolder ID="_prevHolder" runat="server"><div class="floatLeft navThumb"><asp:HyperLink id="_previousThumbnail" runat="server" /><div class="navTitle">previous</div></div></asp:PlaceHolder>
        <asp:PlaceHolder ID="_nextHolder" runat="server"><div class="floatRight navThumb"><asp:HyperLink id="_nextThumbnail" runat="server" /><div class="navTitle">next</div></div></asp:PlaceHolder>
        <asp:PlaceHolder id="_nextThumbGhost" runat="server" visible="false"><div class="floatLeft navThumb"><img src="/_images/gallery-end.gif" alt="end" /><div class="navTitle">previous</div></div></asp:PlaceHolder>
    </div>
    <div class="clear"></div>

    <div class="breaker"></div>
    <a href="http://motoprofessional.com" target="_blank"><img src="/_images/content/motoprofessional-lb-banner.jpg" alt="Moto Professional - Pro photos for everyone" /></a>
    <div class="breaker" id="mpBreaker"></div>
    <div class="padded">
        <Controls:Comments id="_comments" runat="server" />
    </div>
    
</asp:Content>