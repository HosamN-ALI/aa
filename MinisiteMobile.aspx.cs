using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MinisiteMobile : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Request.Url.Host.Contains("localhost") && !Request.IsSecureConnection )
        {
            Response.Redirect(Request.Url.ToString().Replace("http:", "https:"));
        }
        //PageTitle.Text = Languages2.MyText("MinisiteMobile_Preview mobile version_תצוגה מקדימה גרסת מובייל");
        PageTitle.Text = "Mobile Preview";
        if(!String.IsNullOrEmpty(Request.QueryString["p"]))
        {
            string isIframe = "&isIframe="+true.ToString();
            string ismobile = "&ismobile=" + true.ToString();

            MobileHorzIframe.Attributes["src"] = "https://n.sendmsg.co.il/Minisites.aspx?p=" + Request.QueryString["p"]+isIframe+ismobile;
            MobileVertIframe.Attributes["src"] = "https://n.sendmsg.co.il/Minisites.aspx?p=" + Request.QueryString["p"]+isIframe+ismobile;

            //MobileHorzIframe.Attributes["src"] = Request.Url.ToString().Remove(Request.Url.ToString().LastIndexOf("/")) + "/Minisites.aspx?p=" + Request.QueryString["p"] + isIframe + ismobile;
            //MobileVertIframe.Attributes["src"] = Request.Url.ToString().Remove(Request.Url.ToString().LastIndexOf("/")) + "/Minisites.aspx?p=" + Request.QueryString["p"] + isIframe + ismobile;

            string theQuery = Request.QueryString.ToString().Replace("p=", "");
            QrImg.Src = "http://chart.apis.google.com/chart?cht=qr&chs=300x300&chl=" + Server.UrlEncode("http://n.sendmsg.co.il/Minisites.aspx?p=" + theQuery) + "&chld=H|0";
        }       
    }
}