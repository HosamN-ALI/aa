<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeFile="Minisites.aspx.cs" Inherits="Minisites" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta id="longUrl" content="<%=Request.Url.ToString() %>"></meta>
    <asp:Literal runat="server" ID="FontsPlacement"></asp:Literal>
    <asp:Literal runat="server" ID="AnaliticsCode"></asp:Literal>
    <link rel="icon" href="" type="image/x-icon" id="favicon" runat="server" />
    <link runat="server" id="StyleSheet" href="https://n.sendmsg.co.il/Style/StyleSheet.css" rel="stylesheet" />
    <title runat="server" id="PageTitle"></title>
    <meta name="description" id="PageDesc" runat="server" content="" />
    <meta property="og:description" runat="server" id="PageOGDesc" content="" />
    <meta property="og:title" runat="server" id="PageOGTitle" content='' />
    <meta property="og:image" runat="server" id="PageOGImage" content="" />
    <meta property="og:type" content="website" />
    <meta property="og:url" runat="server" id="ogUrl" content="" />
    <%--<meta property="og:image:url" runat="server" content="" />--%>
    <link rel="canonical" href="" runat="server" id="LandingCanonical" />
    <meta property="fb:app_id" runat="server" content="162086843842357" />
    <meta name="robots" content="INDEX, FOLLOW" runat="server" id="robots" />
    <meta name="googlebot" content="INDEX, FOLLOW" runat="server" id="googlebot" />
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>--%>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.23/jquery-ui.min.js"></script>
    <script src="https://panel.sendmsg.co.il/SendmsgForm.js<%=V %>" type="text/javascript"></script>
    <script src="//panel.sendmsg.co.il/js/renderIframe.js<%=V %>" type="text/javascript"></script>
    <script src="//panel.sendmsg.co.il/js/appEngineRunTime.js" type="text/javascript"></script>
    <script type="text/javascript" src="//app.creaditor.com/formCreator/assets/fc-basics.js"></script>
    <script src="//n.sendmsg.co.il/script/pickadate/picker.js"></script>
    <script src="//n.sendmsg.co.il/script/pickadate/picker.date.js"></script>
    <link href="//n.sendmsg.co.il/script/pickadate/themes/default.css" rel="stylesheet" />
    <link href="//n.sendmsg.co.il/script/pickadate/themes/default.date.css" rel="stylesheet" />
    <link href="//n.sendmsg.co.il/script/pickadate/themes/default.time.css" rel="stylesheet" />
    <link href="//n.sendmsg.co.il/script/pickadate/themes/rtl.css" rel="stylesheet" />
        <link href="//app.creaditor.com/cdtr/css/viewMode.css" rel="stylesheet" />
    <asp:Literal ID="LangCalender" runat="server" />

    <%--<script src="script/dragWindow.js"></script>--%>
    <asp:Literal Text="" ID="DragWindow_JS" runat="server" />
    <%--	<meta name="viewport" content="initial=1.0, maximum-scale=1.0, user-scalable=yes, width=100%, height=100%" runat="server" id="viewPort1" visible="false" />--%>
    <meta name="viewport" content="user-scalable=no, width=device-width" runat="server" id="viewPort1" visible="false" />
    <asp:Literal Text="" ID="FixStyle" runat="server" />
    <style type="text/css">
        .fb-like {
            z-index: 99999;
        }
    </style>
    <asp:Literal runat="server" Text="" ID="accessPlug" />
</head>
<body id="PageBody" runat="server" style="margin: 0; padding: 0;">
    <asp:Literal Text="" ID="ExternalCodeIntoBody" runat="server" />
    <div id="fb-root">
    </div>
    <script type="text/javascript">		(function (d, s, id) {
		    var js, fjs = d.getElementsByTagName(s)[0];
		    if (d.getElementById(id)) { return; }
		    js = d.createElement(s); js.id = id;
		    js.src = "//connect.facebook.net/en_US/all.js#xfbml=1&appId=162086843842357";
		    fjs.parentNode.insertBefore(js, fjs);
		}(document, 'script', 'facebook-jssdk'));


        function mobileContent(query_String) {
            var urlPath = location.pathname;
            urlPath = urlPath.replace('/', '');
            var myData = "p=" + query_String + "&ismobile=true";
            if (urlPath.indexOf('f') == 0) {
                urlPath = urlPath.substring(urlPath.indexOf('/') + 1);
                urlPath += "_ismobile";
                myData = "";
            }
            //alert($('.devider').css('width'));
            //alert($('head').html())
            $.ajax({
                type: "GET",
                url: urlPath,
                data: myData,//Upload?DA
                dataType: "html",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    $(document).html('');
                    //document.write(msg);
                    //if (msg.d != "" && msg.d.indexOf('error') < 0) {
                    //    var landingContent = msg.d.toString().split('sendmsgMobileColor');
                    //    $('body').css('background-color', landingContent[0]);
                    //    $('.landingContent,.pageDiv').css('background-color', landingContent[0]);
                    //    $('.landingContent').html(landingContent[1]);
                    $('.devider').css('width', '100%');
                    var viewPortMeta = "<meta name=\"viewport\" content=\"initial=1.0, maximum-scale=1.0, user-scalable=yes, width=100%, height=100%\" />";
                    $('head').append(viewPortMeta);
                    //}
                    //else {
                    //    console.log(msg.d);
                    //}
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var responseText = jQuery.parseJSON(jqXHR.errorThrown);
                    console.log(responseText + 'ERROR!!!!');
                    console.log(msg);
                }
            });
        }
        var d = new Date(); d.setFullYear(1900 + d.getYear() + 5);
        $(function () {
            var ua = window.navigator.userAgent.toLowerCase().replace(/\s*/g, '');
            if (!(ua.indexOf('msie8') > -1)) {

                try {
                    $('input[fieldtype="6"]').pickadate({
                        selectYears: 1500,
                        selectMonths: true,
                        // An integer (positive/negative) sets it relative to today.
                        min: new Date(1900, 1, 1),
                        // `true` sets it to today. `false` removes any limits.
                        max: d,
                        formatSubmit: 'dd/mm/yyyy',
                        format: 'dd/mm/yyyy'
                    });

                    $('script').filter(function () { return $(this).text().trim().indexOf('loadlang') > -1; })[0].childNodes[0].nodeValue;
                } catch (e) {

                }
            }

        });

    </script>
    <div id="Div1">
    </div>
    <form id="form1" action="Minisites.aspx" runat="server">
        <div class="pageDiv" runat="server" id="pageDiv" style="width: 100%">
            <div style="margin: auto; width: 100%;" id="mainTable">
                <div colspan="2" runat="server" id="TopAdsHolder">
                    <div runat="server" id="TopAds" style="text-align: center" visible="false">
                        <div style="margin: auto;">
                            <div class="adTdClass" style="width: 250px; vertical-align: bottom; display: inline-block;">
                                <asp:Literal Text="" ID="Ad1" runat="server" />
                            </div>
                            <div class="adTdClass" style="width: 250px; vertical-align: bottom; display: inline-block;">
                                <asp:Literal Text="" ID="Ad4" runat="server" />
                            </div>
                            <div class="adTdClass" style="width: 250px; vertical-align: bottom; display: inline-block;">
                                <asp:Literal Text="" ID="Ad5" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>

                <div style="text-align: center;">
                    <div class="devider" style="margin: auto;" runat="server" id="devider">
                        <%--devider table--%>
                        <div id="defaultMain" runat="server" style="text-align: center; background-repeat: no-repeat; width: 100%;">
                            <%--main--%>
                            <div style="margin: auto; text-align: right; width: 100%">
                                <div id="LandingHolder" runat="server" style="width: 100%">
                                    <asp:Literal runat="server" ID="landingTemplate"></asp:Literal>
                                    <asp:Panel ID="LandingPageLockedHolder" CssClass="" runat="server">
                                        <style type="text/css">
                                            .LandingPageLocked {
                                                position: fixed;
                                                top: 50%;
                                                left: 50%;
                                                transform: translate(-50%,-50%);
                                                z-index: 9;
                                            }

                                            .attrTextBox {
                                                color: #5A5A5A;
                                                display: flex;
                                                font-size: 16px;
                                                margin-top: 10px;
                                            }

                                            .blueOkBtn {
                                                box-shadow: 0 1px 2px rgba(0, 0, 0, 0.4);
                                                -o-box-shadow: 0 1px 2px rgba(0, 0, 0, 0.4);
                                                -moz-box-shadow: 0 1px 2px rgba(0, 0, 0, 0.4);
                                                -webkit-box-shadow: 0 1px 2px rgba(0, 0, 0, 0.4);
                                                min-width: 120px;
                                                height: 30px;
                                                border: 1px solid #bababa;
                                                border-style: solid;
                                                background-color: #3498db;
                                                color: #FFF;
                                                font-size: 14px;
                                                font-weight: bold;
                                                border-radius: 5px;
                                                margin: 0px 5px;
                                                cursor: pointer;
                                            }

                                            input[type='text'] {
                                                border: none;
                                                border: 1px solid #8F8E8E;
                                                padding: 0px 2%;
                                                outline: none;
                                                font-size: 14px;
                                                line-height: 24px;
                                                background: #fff;
                                                margin-bottom: 4px;
                                                margin-top: 4px;
                                                height: 32px;
                                                border-radius: 2px;
                                                width: calc(96% - 2px);
                                            }

                                                input[type='text']:focus {
                                                    outline: none;
                                                    border: 1px solid #8BB603;
                                                }

                                            * {
                                                font-family: Arial;
                                                transition: 200ms;
                                            }

                                            input[type='submit'] {
                                                width: calc(100%);
                                                margin: auto;
                                                margin-top: 10px;
                                            }

                                                input[type='submit']:hover {
                                                    box-shadow: 0px 2px 2px 0px rgba(0,0,0,0.8);
                                                }

                                            .titleSecurity {
                                                background: #3598db;
                                                color: #fff;
                                                padding: 20px;
                                                text-align: center;
                                                font-weight: bold;
                                                font-size: 18px;
                                            }

                                            .SecurityHolder {
                                                text-align: center;
                                                color: #fff;
                                                font-size: 45px;
                                                font-size: 15px;
                                                position: absolute;
                                                transform: translateX(-50%);
                                                left: 50%;
                                                top: 3%;
                                                display: table;
                                            }

                                            .SecurityFooter {
                                                color: #fff;
                                                position: absolute;
                                                bottom: 2%;
                                                transform: translateX(-50%);
                                                left: 50%;
                                                /*display: -webkit-box;*/
                                                width: 100%;
                                                text-align: center;
                                            }

                                            .SecurityFooterImgHolder {
                                                line-height: 73px;
                                                margin: 0px 10px;
                                                display: inline-block;
                                            }

                                            .SecurityFooterLink {
                                                display: inline-block;
                                                color: inherit;
                                                text-decoration: none;
                                            }

                                            .SecurityIcon {
                                                margin-bottom: 5px;
                                            }

                                            .SecurityFooterImg {
                                                margin-bottom: -17px;
                                                width: 94px;
                                                height: 49px;
                                            }

                                            .fixHeight {
                                                position: absolute;
                                                left: 50%;
                                                top: 50%;
                                                transform: translate(-50%, -50%);
                                            }

                                            @media screen and (max-width: 350px) {
                                                .SecurityFooterImgHolder {
                                                    margin: 0px;
                                                }
                                            }
                                        </style>
                                        <asp:Panel runat="server" ID="LandingPageLocked" CssClass="LandingPageLocked">
                                            <div class="titleSecurity">
                                                <asp:Literal Text="" ID="titleSecurity" runat="server" />
                                            </div>
                                        </asp:Panel>
                                        <div class="SecurityHolder">
                                            <img runat="server" id="SecurityIcon" class="SecurityIcon" src="images/SecurityIcon.png" />
                                            <div>
                                                <asp:Literal Text="" ID="AccessRequired" runat="server" />
                                            </div>
                                        </div>
                                        <div class="SecurityFooter" runat="server" id="SecurityFooter">
                                            <a runat="server" id="SecurityFooterLink" target="_blank" class="SecurityFooterLink">
                                                <div class="SecurityFooterImgHolder">
                                                    <asp:Literal Text="" ID="PageCreated" runat="server" />
                                                </div>
                                                <img runat="server" id="SecurityFooterImg" class="SecurityFooterImg" />
                                            </a>
                                        </div>

                                    </asp:Panel>
                                    <script type="text/javascript">
                                        $('#<%=form1.ClientID%>').attr('action', window.location.toString());
                                        reSize();

                                        window.onresize = function () {
                                            reSize();

                                            try {
                                                fixHeight();
                                            } catch (e) {

                                            }
                                        }

                                        function reSize() {
                                            $('body').height(window.innerHeight + 'px');
                                        }
                                    </script>
                                </div>
                                <asp:Literal runat="server" ID="Pixel"></asp:Literal>
                                <div style="text-align: left; margin-top: 10px; width: 100%">
                                    <asp:Literal runat="server" ID="FooterFaceBook"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div style="text-align: center">
                            <p>
                                <%--<a class="addthis_button" href="http://addthis.com/bookmark.php?v=250&amp;username=xa-4bd984c942f61fae"><img alt="Bookmark and Share" height="16" src="http://s7.addthis.com/static/btn/v2/lg-share-en.gif" style="border: 0pt none ;" width="125" /></a><script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=xa-4bd984c942f61fae"></script><!-- AddThis Button END -->--%>
                            </p>
                        </div>
                    </div>
                    <div style="width: 100%; background: white;" id="BottomAdsHeight" runat="server">
                        <div runat="server" id="BottomAds1" visible="false">
                            <div style="margin: auto;">
                                <div class="adTdClass" style="width: 250px; display: inline-block;">
                                    <asp:Literal Text="" ID="Ad3" runat="server" />
                                </div>
                                <div class="adTdClass" style="width: 250px; display: inline-block;">
                                    <asp:Literal Text="" ID="Ad8" runat="server" />
                                </div>
                                <div class="adTdClass" style="width: 250px; display: inline-block;">
                                    <asp:Literal Text="" ID="Ad9" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div>
                            <asp:Panel runat="server" ID="FBCommentsShow" Visible="false">
                                <div class="fb-comments" id="fbCommentsDiv" data-href="<% =pathAndQuery %>" data-num-posts="10" data-mobile='Auto-detected'
                                    data-width='<%=(isMobileFunc)?"100%":"800" %>'>
                                </div>
                            </asp:Panel>
                        </div>
                        <div runat="server" id="Copyright" style="width: 100%; text-align: center; color: #1071b5; font-size: 13px; padding-top: 10px; padding-bottom: 20px;">
                            <asp:Literal runat="server" ID="ShowPageCredentials" Text='דף זה מוצג באמצעות שלח מסר – מערכת <a href="https://www.sendmsg.co.il" style="color:#1071b5">דיוור אלקטרוני</a>'></asp:Literal>
                        </div>
                    </div>
                </div>

                <div runat="server" id="SideAdsHolder" visible="false" style="vertical-align: top;">
                    <div runat="server" id="SideAdFixedDiv" style="vertical-align: top; top: 114px; left: 10px; position: absolute;">
                        <a href="https://n.sendmsg.co.il/f14/promosender" target="_blank">
                            <img runat="server" id="PromoSenderLogo" visible="false" src="https://panel.sendmsg.co.il/images/promosender1.png"
                                alt="Promo Sender" style="margin: 2px 10px; width: 100px; border: none; margin-top: -30px; -ms-filter: progid:DXImageTransform.Microsoft.Alpha(Opacity=50); filter: alpha(opacity=50); -moz-opacity: 0.5; -khtml-opacity: 0.5; opacity: 0.5;" />
                        </a>
                        <div runat="server" id="SideAds" style="vertical-align: middle;" visible="false">
                            <div style="margin: auto;">
                                <div class="adTdClass" style="width: 120px;">
                                    <asp:Literal Text="" ID="Ad2" runat="server" />
                                </div>
                                <div class="adTdClass" style="width: 120px;">
                                    <asp:Literal Text="" ID="Ad6" runat="server" />
                                </div>
                                <div class="adTdClass" style="width: 120px;">
                                    <asp:Literal Text="" ID="Ad7" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="scrResolution" />
    </form>
    <!-- Start of StatCounter Code for Default Guide -->
    <script type="text/javascript">
        var sc_project = 4655009;
        var sc_invisible = 1;
        var sc_security = "bdf89bbe";
        var sc_https = 1;
        var scJsHost = (("https:" == document.location.protocol) ?
        "https://secure." : "http://www.");
        document.write("<script type='text/javascript' src='" +
        scJsHost +
        "statcounter.com/counter/counter.js'></" + "script>");
    </script>
    <noscript>
		<div class="statcounter">
			<a title="web analytics"
				href="http://statcounter.com/" target="_blank">
				<img
					class="statcounter"
					src="http://c.statcounter.com/4655009/0/bdf89bbe/1/"
					alt="web analytics"></a>
		</div>
	</noscript>
    <!-- End of StatCounter Code for Default Guide -->
    <%--old code.. - changed for SSL porpuses - 24/11/2013--%>
    <!-- Start of StatCounter Code -->
    <%--<script type="text/javascript">
		var sc_project = 4655009;
		var sc_invisible = 1;
		var sc_partition = 56;
		var sc_click_stat = 1;
		var sc_security = "bdf89bbe"; 
	</script>
	<script type="text/javascript" src="http://www.statcounter.com/counter/counter.js"></script>
	<noscript>
		<div class="statcounter">
			<img class="statcounter" src="http://c.statcounter.com/4655009/0/bdf89bbe/1/" alt="drupal statistics"></div>
	</noscript>--%>
    <!-- End of StatCounter Code -->
    <script type="text/javascript">
        var windowHeight = getWindowHeight();
        var mainTable = document.getElementById('mainTable');
        var tableHeight = mainTable.offsetHeight;
        var topAdsHeight = document.getElementById('TopAdsHolder').offsetHeight;
        var bottomAdsHeight = document.getElementById('BottomAdsHeight').offsetHeight;
        var devider = document.getElementById('devider');

        if (tableHeight < windowHeight) {
            var newHeight = windowHeight - topAdsHeight - bottomAdsHeight;
            $(devider).height(newHeight);
        }

        function getWindowHeight() {
            var windowHeight = 0;

            if (typeof (window.innerHeight) == 'number')
                windowHeight = window.innerHeight;

            else {

                if (document.documentElement && document.documentElement.clientHeight)
                    windowHeight = document.documentElement.clientHeight;

                else {
                    if (document.body && document.body.clientHeight)
                        windowHeight = document.body.clientHeight;
                };
            };

            return windowHeight;
        };

        var imagesShown = new Array();

        function ViewAd(AdInfo) {
            $.ajax({
                type: 'GET',
                url: 'minisites.aspx/showAd',
                data: "AdInfo=" + AdInfo + "__view",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) { },
                error: function (jqXHR, textStatus, errorThrown) { }
            })
        };

        function ClickAd(AdInfo) {
            $.ajax({
                type: 'GET',
                url: 'minisites.aspx/showAd',
                data: "AdInfo=" + AdInfo + "__click",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) { },
                error: function (jqXHR, textStatus, errorThrown) { }
            })
        };



        function isScrolledIntoView(elem) {
            var docViewTop = $(window).scrollTop();
            var docViewBottom = docViewTop + $(window).height();
            var docViewLeft = $(window).scrollLeft();
            var docViewRight = docViewLeft + $(window).width();

            var elemTop = $(elem).offset().top;
            var elemBottom = elemTop + $(elem).height();
            var elemLeft = $(elem).offset().left;
            var elemRight = elemLeft + $(elem).width();


            if ((elemBottom <= docViewBottom) && (elemTop >= docViewTop) && (elemLeft >= docViewLeft) && (elemRight <= docViewRight)) {
                if ($.inArray($(elem).children('.SendMsgAdHid').val(), imagesShown) == -1) {
                    imagesShown.push($(elem).children('.SendMsgAdHid').val());
                    ViewAd($(elem).children('.SendMsgAdHid').val());
                    $('.contentAdAltTextClass').click(function () { ClickAd($(elem).children('.SendMsgAdHid').val()); setTimeout('return true;', '1000'); });
                }
            }

        }

        $('.SendMsgAd').each(function () { isScrolledIntoView($(this)) });

        $(window).scroll(function () {
            $('.SendMsgAd').each(function () { isScrolledIntoView($(this)) });
            try {
                $('#SideAdFixedDiv').stop(true, true).animate({
                    marginTop: ($(window).scrollTop()) + 'px'
                }, 800, function () {
                    // Animation complete.
                });

                //.css("margin-top",($(window).scrollTop()) +"px");
            }
            catch (ex)
            { }
        });

        //var e = document.body, t = document.documentElement, o = Math.max(e.scrollHeight, e.offsetHeight, t.clientHeight, t.scrollHeight, t.offsetHeight); parent.postMessage(o, "*")
        window.onload = function () {
            var e = document.body, t = document.documentElement, o = document.getElementById('LandingHolder') !== null ? document.getElementById('LandingHolder').offsetHeight + 20 : Math.max(e.scrollHeight, e.offsetHeight, t.clientHeight, t.scrollHeight, t.offsetHeight); parent.postMessage(o, "*");
            $('#<%=form1.ClientID%>').attr('action', window.location.toString());
        };

    </script>
    <script>var pID = <%:landingID%>;</script>
    <asp:Literal runat="server" ID="HarlemShake"></asp:Literal>
</body>
