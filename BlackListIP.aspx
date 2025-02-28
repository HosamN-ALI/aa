<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BlackListIP.aspx.cs" Inherits="BlackListIP" enableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel runat="server" ID="passComp">הכנס סיסמא: <asp:TextBox runat="server" ID="txtBoxPass" /></asp:Panel>

        <asp:Panel runat="server" Visible="false" ID="DelBlockClient_ip" Style="margin-top: 10px;direction: rtl;">
        <asp:Button Text="נקה את כל התוכן במערך שגיאות IP" ID="DelAllIPBlock" OnClick="DelAllIPBlock_Click" Visible="false" runat="server" /><br />
        <asp:Literal Text="המערך ריק מכתובות IP" ID="NoRes" Visible="true" runat="server" />
        <asp:Repeater runat="server" ID="DelBlockClient_ipRep">
            <ItemTemplate>
                <asp:Panel ID="Panel1" runat="server" Style='<%# "display:"+ Container.ItemIndex == "0"? "none" : "block"%>'>
                    או
        <br />
                </asp:Panel>
                <asp:Button ID="Button1" Text='<%# "Del IP - "+Container.DataItem.ToString() %>' CommandArgument='<%# Container.DataItem.ToString() %>' OnClick="Unnamed_Click" runat="server" /><br />

            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>
    </div>
    </form>
</body>
</html>
