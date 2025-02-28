<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MinisiteMobile.aspx.cs" Inherits="MinisiteMobile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title runat="server" id="PageTitle"></title>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.23/jquery-ui.min.js"></script>
    <script src="script/animateRotate.js"></script>
    
    <link href="MobilePreview.css" rel="stylesheet" />
</head>
<body class="mobileBody">
    <form id="form1" runat="server">
        <p></p>
        <div class="MobilePageHolder">
            <img src="images/flipMobile.png" alt="Flip Mobile" title="Flip Mobile" class="FlipMobile" onclick="rotateMobile();" />
            <input type="hidden" name="" value="0" class="FlipMobileHid" />
            <br />
            <div class="MobileHorzHolder" runat="server" id="MobileHorzHolder" style="display: none;">
                <iframe src="#" runat="server" id="MobileHorzIframe" class="MobileHorzIframe styledScroller" width="481" height="320"></iframe>
            </div>
            <div class="MobileVertHolder" runat="server" id="MobileVertHolder">
                <iframe src="#" runat="server" id="MobileVertIframe" class="MobileVertIframe styledScroller" width="320" height="479"></iframe>
            </div>            
        </div>
        <br />
        <div class="MobileShadow"></div>
        <br />
        <br />
        <br />
        <br />
        <img src="#" alt="Page QR Code" runat="server" id="QrImg" />
        <script>
            //var iframe_horz = $('.MobileHorzIframe');

            //iframe_horz.load(function () {
            //    var iframe_horz_width = iframe_horz.find('body').width();
            //    var iframe_horz_height = iframe_horz.find('body').height();
            //    console.log(iframe_horz.contents().find('body'));
            //    console.log(iframe_horz_width + ' X ' + iframe_horz_height);
            //});

            $('.MobileHorzIframe').load(function () {
                //console.log(this.contentWindow.document.body);                
            });
            $('.MobileVertIframe').load(function () {
                //console.log(this.contentWindow.document.body);                
            });

            $('.MobileHorzIframe').attr('src', $('.MobileHorzIframe').attr('src'));
            $('.MobileVertIframe').attr('src', $('.MobileVertIframe').attr('src'));

            function rotateMobile() {
                var flipMobileHid = $('.FlipMobileHid').val();
                console.log(flipMobileHid);
                if (flipMobileHid == '0') {
                    $('.MobileVertHolder').animateRotate(90, 1000, '', function () {
                        $('.MobileHorzIframe').attr('src', $('.MobileHorzIframe').attr('src'));

                        $('.MobileVertIframe').animate({
                            width: '0px'
                        }, 200, function () {

                            $('.MobileVertHolder').hide();
                            $('.MobileHorzHolder').show();

                            $('.MobileHorzIframe').animate({
                                width: '481px'
                            }, 500, function () {

                                $('.FlipMobileHid').val(1);
                                $('.MobileVertHolder').animateRotate(0);
                            });
                        });                       
                    });

                    $('.MobileShadow').animate({
                        width: '645px',
                    }, 1000);
                }
                else {
                    $('.MobileHorzHolder').animateRotate(-90, 1000, '', function () {
                        $('.MobileVertIframe').attr('src', $('.MobileVertIframe').attr('src'));

                        $('.MobileHorzIframe').animate({
                            width: '0px'
                        }, 200, function () {
                            $('.MobileVertHolder').show();
                            $('.MobileHorzHolder').hide();

                            $('.MobileVertIframe').animate({
                                width: '320px'
                            }, 500, function () {
                                $('.FlipMobileHid').val(0);
                                $('.MobileHorzHolder').animateRotate(0);
                            });
                        });                        
                    });

                    $('.MobileShadow').animate({
                        width: '345px',
                        marginTop: '0px'
                    }, 1000);
                }
            }

        </script>
    </form>
</body>
</html>
