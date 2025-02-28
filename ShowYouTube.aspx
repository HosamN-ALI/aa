<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowYouTube.aspx.cs" Inherits="ShowYouTube" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title></title>

    <meta property="og:title" runat="server" content="" id="ogTitle" />
    <meta property="og:image" runat="server" content="" id="ogImage" />
    <meta property="og:description" runat="server" content="" id="ogDescription" />
    <meta property="og:type" content="website" />
    <meta property="og:url" id="ogUrl" content="" runat="server" />
    <style type="text/css">
        #youTubeIframe {
            width: 95vw;
            height: 75%;
        }

        @media screen and (min-width: 700px) {
            #youTubeIframe {
                width: 50vw;
                height: 28.125vw;
            }
        }
    </style>
</head>
<body style="font-family: Arial; margin: 0px" id="PageBody" runat="server">
    <div id="fb-root"></div>
    <script type="text/javascript">(function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/all.js#xfbml=1&appId=306854319368100";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));</script>
    <form id="form1" runat="server">
        <div style="overflow: auto; float: none; top: 0; bottom: 0; right: 0; left: 0; height: 100%; width: 100%; background: #e8e8e8 url('http://www.sendmsg.co.il/minisite/images/MinisiteTopHeaderBG.png') top left repeat-x; /* padding-top: 30px; */">
            <div style="float: none; width: 100%; margin: auto; position: relative; background: url(http://panel.sendmsg.co.il/images/SendMsgStripsBG.jpg) top left no-repeat; background-size: cover; /*padding-top: 20px; */">
                <%--  <div id="TopTitleText" style="width: 100%; font-size: 18px; color: #c5d6d9; top: 10px; z-index: 100; text-align: center; white-space: nowrap;"
                    runat="server">
                                     <a href="http://www.sendmsg.co.il" id="TopTitleLogoLink" runat="server" target="_blank" style="height: 64px; z-index: 100; display: inline-block; vertical-align: middle;">
                        <img alt="" runat="server" style="border: none; float: left; max-width: 150px;" src="http://www.sendmsg.co.il/minisite/images/HomeImages/c4c74e05-5e29-4a13-b17b-c1611f282cdb_prod.png" id="TopTitleLogoImg" /></a>
                </div>--%>

                <div style="display: table; width: 100%;">
                    <div style="width: 100%; position: relative; height: calc(100vh - 45px); display: block;">
                        <asp:Literal runat="server" ID="YouTubeCode">
			
                        </asp:Literal>
                    </div>
                    <div runat="server" id="likeDiv" style="width: 100%; text-align: center; height: 100px; margin-top: 15px; margin-bottom: 15px"
                        visible="false">
                        <div runat="server" id="FacebookLike" class="fb-like" data-href="" data-width="450" data-show-faces="true" data-send="true"></div>
                    </div>
                </div>

            </div>
            <div style="width: 100%; bottom: 10px; text-align: center; color: #1071b5; font-size: 13px; padding-top: 10px; padding-bottom: 20px;" runat="server" id="FooterSpan"></div>
        </div>

        <script type="text/javascript">


</script>
    </form>
</body>
</html>
