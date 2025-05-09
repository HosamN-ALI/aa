﻿<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeFile="Minisites3.aspx.cs" Inherits="Minisites3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<link href="https://n.sendmsg.co.il/Style/StyleSheet.css" rel="stylesheet" />
	<title runat="server" id="PageTitle"></title>
	<meta name="description" id="PageDesc" runat="server" content="" />
	<meta property="og:description" runat="server" id="PageOGDesc" content="" />
	<meta property="og:title" runat="server" id="PageOGTitle" content='' />
	<meta property="og:image" runat="server" content="https://n.sendmsg.co.il/images/logo.png" />
	<meta property="og:type" content="website" />
	<meta property="og:image:url" runat="server" content="https://n.sendmsg.co.il/images/logo.png" />

	<meta property="fb:app_id" runat="server" content="162086843842357" />
	<meta name="robots" content="INDEX, FOLLOW" runat="server" id="robots" />
	<meta name="googlebot" content="INDEX, FOLLOW" runat="server" id="googlebot" />
	<%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>--%>
	<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
	<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.23/jquery-ui.min.js"></script>
	<script src="https://panel.sendmsg.co.il/SendmsgForm.js" type="text/javascript"></script>

	<script src="script/pickadate/picker.js"></script>
	<script src="script/pickadate/picker.date.js"></script>
	<link href="script/pickadate/themes/default.css" rel="stylesheet" />
	<link href="script/pickadate/themes/default.date.css" rel="stylesheet" />
	<link href="script/pickadate/themes/default.time.css" rel="stylesheet" />
	<link href="script/pickadate/themes/rtl.css" rel="stylesheet" />
	<asp:Literal ID="LangCalender" runat="server" />

	<%--<script src="script/dragWindow.js"></script>--%>
	<asp:Literal Text="" ID="DragWindow_JS" runat="server" />
	<asp:Literal runat="server" ID="AnaliticsCode"></asp:Literal>
	<meta name="viewport" content="initial=1.0, maximum-scale=1.0, user-scalable=yes, width=100%, height=100%" runat="server" id="viewPort1" visible="false" />
	<asp:Literal Text="" ID="FixStyle" runat="server" />
	<style type="text/css">
		.fb-like {
			z-index: 99999;
		}
	</style>
</head>
<body id="PageBody" runat="server" style="margin: 0; padding: 0;">
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
				data: myData,//להעלות?DA
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
			}

		});

	</script>
	<div id="Div1">
	</div>
	<form id="form1" action="Minisites.aspx" runat="server">
		<div class="pageDiv" runat="server" id="pageDiv" style="width: 100%">
			<table border="0" style="margin: auto; vertical-align: top; width: 100%;" cellpadding="0"
				cellspacing="0" id="mainTable">
				<tr id="TopAdsTR">
					<td colspan="2" runat="server" id="TopAdsHolder">
						<div runat="server" id="TopAds" style="text-align: center" visible="false">
							<table border="0" cellpadding="10" cellspacing="0" style="margin: auto;">
								<tr>
									<td class="adTdClass" style="width: 250px; vertical-align: bottom;">
										<asp:Literal Text="" ID="Ad1" runat="server" />
									</td>
									<td class="adTdClass" style="width: 250px; vertical-align: bottom;">
										<asp:Literal Text="" ID="Ad4" runat="server" />
									</td>
									<td class="adTdClass" style="width: 250px; vertical-align: bottom;">
										<asp:Literal Text="" ID="Ad5" runat="server" />
									</td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td style="text-align: center;">
						<div class="devider" style="margin: auto;" cellspacing="0" cellpadding="0" runat="server"
							id="devider">
							<%--devider table--%>

								<div id="defaultMain" runat="server" style="text-align: center; vertical-align: top; background-repeat: no-repeat; width: 100%;">
									<%--main--%>
									<table cellpadding="0" cellspacing="0" style="margin: auto; text-align: right; width: 100%">
										<tr>
											<td id="LandingHolder" runat="server" style="width: 100%">
												<asp:Literal runat="server" ID="landingTemplate"></asp:Literal>
											</td>
										</tr>
										<tr>
											<asp:Literal runat="server" ID="Pixel"></asp:Literal>
										</tr>
										<tr>
											<td style="text-align: left; margin-top: 10px; width: 100%">
												<asp:Literal runat="server" ID="FooterFaceBook"></asp:Literal>
											</td>
										</tr>
									</table>
								</div>

								<div style="text-align: center">
									<p>
										<%--<a class="addthis_button" href="http://addthis.com/bookmark.php?v=250&amp;username=xa-4bd984c942f61fae"><img alt="Bookmark and Share" height="16" src="http://s7.addthis.com/static/btn/v2/lg-share-en.gif" style="border: 0pt none ;" width="125" /></a><script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=xa-4bd984c942f61fae"></script><!-- AddThis Button END -->--%>
									</p>
								</div>
						</div>
						<table border="0" cellpadding="0" cellspacing="0" style="width: 100%; background: white;" id="BottomAdsHeight">
							<tr>
								<td colspan="2">
									<div runat="server" id="BottomAds1" visible="false">
										<table border="0" cellpadding="10" cellspacing="0" style="margin: auto;">
											<tr>
												<td class="adTdClass" style="width: 250px;">
													<asp:Literal Text="" ID="Ad3" runat="server" />
												</td>
												<td class="adTdClass" style="width: 250px;">
													<asp:Literal Text="" ID="Ad8" runat="server" />
												</td>
												<td class="adTdClass" style="width: 250px;">
													<asp:Literal Text="" ID="Ad9" runat="server" />
												</td>
											</tr>
										</table>
									</div>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Panel runat="server" ID="FBCommentsShow" Visible="false">
										<div class="fb-comments" id="fbCommentsDiv" data-href="<% =pathAndQuery %>" data-num-posts="10" data-mobile='Auto-detected'
											data-width='<%=(isMobileFunc)?"100%":"800" %>'>
										</div>
									</asp:Panel>
								</td>
							</tr>
							<tr>
								<td style="width: 100%; text-align: center; color: #1071b5; font-size: 13px; padding-top: 10px; padding-bottom: 20px;">
									<asp:Literal runat="server" ID="ShowPageCredentials" Text='דף זה מוצג באמצעות שלח מסר – מערכת <a href="https://www.sendmsg.co.il" style="color:#1071b5">דיוור אלקטרוני</a>'></asp:Literal>
								</td>
							</tr>
						</table>
					</td>

					<td runat="server" id="SideAdsHolder" visible="false" style="vertical-align: top;">
						<div runat="server" id="SideAdFixedDiv" style="vertical-align: top; top: 114px; left: 10px;">
							<a href="https://n.sendmsg.co.il/f14/promosender" target="_blank">
								<img runat="server" id="PromoSenderLogo" visible="false" src="https://panel.sendmsg.co.il/images/promosender1.png"
									alt="Promo Sender" style="margin: 2px 10px; width: 100px; border: none; margin-top: -30px; -ms-filter: progid:DXImageTransform.Microsoft.Alpha(Opacity=50); filter: alpha(opacity=50); -moz-opacity: 0.5; -khtml-opacity: 0.5; opacity: 0.5;" />
							</a>
							<div runat="server" id="SideAds" style="vertical-align: middle;" visible="false">
								<table border="0" cellpadding="10" cellspacing="0" style="margin: auto;">
									<tr>
										<td class="adTdClass" style="width: 120px;">
											<asp:Literal Text="" ID="Ad2" runat="server" />
										</td>
									</tr>
									<tr>
										<td class="adTdClass" style="width: 120px;">
											<asp:Literal Text="" ID="Ad6" runat="server" />
										</td>
									</tr>
									<tr>
										<td class="adTdClass" style="width: 120px;">
											<asp:Literal Text="" ID="Ad7" runat="server" />
										</td>
									</tr>
								</table>
							</div>
						</div>
					</td>
				</tr>
			</table>
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
		document.write("<sc" + "ript type='text/javascript' src='" +
        scJsHost +
        "statcounter.com/counter/counter.js'></" + "script>");
	</script>
	<noscript><div class="statcounter"><a title="web analytics"
href="http://statcounter.com/" target="_blank"><img
class="statcounter"
src="http://c.statcounter.com/4655009/0/bdf89bbe/1/"
alt="web analytics"></a></div></noscript>
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
		var topAdsHeight = document.getElementById('TopAdsTR').offsetHeight;
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

	</script>
	<asp:Literal runat="server" ID="HarlemShake"></asp:Literal>
</body>
