<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeFile="rank.aspx.cs" Inherits="Rank" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
	<html xmlns="http://www.w3.org/1999/xhtml" >
	
	<head>
		<title runat="server" id="PageTitle"></title>
		<meta name="description" id="PageDesc" runat="server" content="" />
	</head>
	<body id="PageBody" runat="server" style="font-family:Arial">
	
    <form id="form1" action="Default.aspx" runat="server">
		<div style="width:1018px;margin:auto auto auto auto;height:100%; background:#ffffff; text-align:center;vertical-align:top;" style="width:100%">
			    <table style="width:1018px;height:700px;" cellspacing="0" cellpadding="0"> <%--devider table--%>
		<tr>
			<td colspan="3" runat="server" id="bannerHome" style="background-image:url(http://panel.sendmsg.co.il/images/HeaderBack.gif);width:1018px; background-repeat:no-repeat; height:110px; background-position:center top; vertical-align:bottom;" ><%--banner--%>
				<table cellpadding="0" cellspacing="0" style="width:100%;" >
					<tr>
						<td style="vertical-align:top">
							<table cellpadding="0" cellspacing="0" style="margin:0px auto auto auto;vertical-align:top; width:100%;">
								<tr>
									<td style="width:20px;height:102px;">&nbsp</td><%--spacer--%>
									<td style="width:30px; vertical-align:top">&nbsp</td>
									<td style="width:6px">&nbsp</td>
									<td  style="height:100px; text-align:right;padding-right:30px;"><asp:Label style="color:White;font-size:40px;text-decoration:none;" runat="server" ID="removeHeader"></asp:Label><a href="http://www.sendmsg.co.il">                                    
                                   
                                    </td>
									
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Table ID="bannerTable" runat="server" style="direction:rtl" Width="100%" CellPadding="0" CellSpacing="0" BorderStyle="None">
								<asp:TableRow style="background-image:url('http://panel.sendmsg.co.il/images/SearchHeaderBack.jpg');height:44px;">
									<asp:TableCell style="width:15px;background-image:url('http://panel.sendmsg.co.il/images/SearchHeaderRight.jpg');">&nbsp;</asp:TableCell>
									<asp:TableCell>&nbsp</asp:TableCell>
									<asp:TableCell style="width:15px;background-image:url('http://panel.sendmsg.co.il/images/SearchHeaderLeft.jpg');"></asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td style="background-repeat:repeat-y;width:5px;background-position:left;background-image:url('http://panel.sendmsg.co.il/images/PageRight.gif');">&nbsp</td><%--right fader--%>
			<td colspan="1" style="vertical-align:middle;background-color:#ffffff;width:100%;height:554px;text-align:center; background-repeat:no-repeat; font-size:44px; color:Gray;" id="defaultMain" runat="server"><%--main--%>
			
				<asp:Literal runat="server" ID="MsgTemplate"></asp:Literal>
				

			</td>
			<td style="background-repeat:repeat-y;width:5px;background-image:url('http://panel.sendmsg.co.il/images/PageLeft.gif');">&nbsp</td><%--left fader--%>
		</tr>
		<tr style="background-image:url('http://panel.sendmsg.co.il/images/PageBottom.gif');background-repeat:repeat-x;height:5px;">
			<td><img src="http://panel.sendmsg.co.il/images/PageBR.gif" alt="" /></td>
			<td></td>
			<td><img src="http://panel.sendmsg.co.il/images/PageBL.gif" alt="" /></td>
		</tr>
	</table>
				
		<div style="width:100%; text-align:center;color:#1071b5;font-size:13px;padding-top:10px;padding-bottom:20px;"><asp:Literal runat="server" ID="ShowPageCredentials" Text='דף זה מוצג באמצעות שלח מסר – מערכת <a href="http://www.sendmsg.co.il" style="color:#1071b5">דיוור אלקטרוני</a>'></asp:Literal></div>
				
	</div>
    </form>

	
	</body>
	</html>