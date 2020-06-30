<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacebookLike.ascx.cs" Inherits="Tetron.Controls.FacebookLike" %>
<div id="fb-root"></div>
<script type="text/javascript">
    window.fbAsyncInit = function () {
        FB.init({ appId: '131542586870292', status: true, cookie: true, xfbml: true });
    };
    (function () {
        var e = document.createElement('script'); e.async = true;
        e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';
        document.getElementById('fb-root').appendChild(e);
    } ());
</script>
<fb:like></fb:like>