<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="Default" %>

    <form id="form1" action="Default.aspx" runat="server">
        <link href="Style/StyleSheet.css" rel="stylesheet" />
		<div class="pageDiv" style="width:100%">
			<table class="devider" style="margin:auto" cellspacing="0" cellpadding="0"> <%--devider table--%>
				<tr>
					<td class="Defaultmain" id="defaultMain" runat="server" style="text-align:center; background-repeat:no-repeat;width:900px;"><%--main--%>
						<table cellpadding="0" cellspacing="0" style="margin:auto; text-align:right">
							<tr>
								<td>
									<asp:Literal runat="server" ID="MsgTemplate"></asp:Literal>
								</td>
							</tr>
							<tr>
								<asp:Literal runat="server" ID="Pixel"></asp:Literal>
							</tr>
							<tr>
								<td style="text-align:left; margin-top:10px; width:100%">
									<asp:Literal runat="server" ID="FooterFaceBook"></asp:Literal>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td style="text-align:center">
						 
					</td>
				</tr>
				<tr>
					<td style="width:100%; text-align:center;color:#1071b5;font-size:13px;padding-top:10px;padding-bottom:20px;"><asp:Literal runat="server" ID="ShowPageCredentials" Text='דף זה מוצג באמצעות שלח מסר – מערכת <a href="http://www.sendmsg.co.il" style="color:#1071b5">דיוור אלקטרוני</a>'></asp:Literal></td>
				</tr>
			</table>
			
		</div>
        <asp:Literal text="" id="ErrMsg" runat="server" ></asp:Literal>
        <asp:Literal text="" id="accessPlug" runat="server" />

    </form>

	<!-- Start of StatCounter Code -->
	<script type="text/javascript">
		var sc_project = 4655009;
		var sc_invisible = 1;
		var sc_partition = 56;
		var sc_click_stat = 1;
		var sc_security = "bdf89bbe"; 
	</script>
	<link href="Style/email-style.css" rel="stylesheet" />
	<script type="text/javascript"
	src="https://www.statcounter.com/counter/counter.js"></script><noscript><div
	class="statcounter"><img class="statcounter"
	src="https://c.statcounter.com/4655009/0/bdf89bbe/1/"
	alt="drupal statistics" ></div></noscript>
	<!-- End of StatCounter Code -->
