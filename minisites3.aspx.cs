using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.IO;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]
[ScriptService]
public partial class Minisites3 : System.Web.UI.Page
{
    private string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
    static List<pageView3> PageViewsList = new List<pageView3>();
    //MySqlDataReader MyReader = null;
    int siteID;
    int landingID;
    Int64 landingValidator;
    string sentID = "";
    //int facebookValidator;
    string msgContent = "";
    string landingSubj = "";
    string msgSubj = "הצגת הודעה";
    string ShowPageDesc = "\"msgSubj\"";
    string sendFriendBody = "";
    string ShareFacebook = "";
    string sendFriend = "";
    string langDir = "2";
    int langId = 1;
    string widthForMobile = "640";
    protected string pathAndQuery = "";

    public bool isMobile = false;
    public bool isMobileFunc = false;
    string status = "";

    protected void Page_Init(object sender, EventArgs e)
    {


        //if (isMobileBrowser() && !Request.Url.ToString().Contains("ismobile=true"))
        //{
        //    Response.Redirect(Request.Url.ToString()+"&ismobile=true");
        //}
        isMobileFunc = isMobileBrowser();
        try
        {
            if (Request.QueryString != null && Request.QueryString["isIframe"] != null && Request.QueryString["isIframe"].ToString() != "" && Request.QueryString["isIframe"].ToString() == true.ToString())
            {
                FixStyleMethode();
                DragWindow_JS.Text = "<script src='https://" + Defaults.SubDomainsitePath + "/script/dragWindow.js'></script>";

            }

            if (Request.QueryString != null && Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != "")
            {
                string P = Request.QueryString["p"];
                if (P.Contains('-'))
                {
                    string[] P_arr;
                    P_arr = P.Split('-');
                    if (P_arr != null && P_arr.Length > 0)
                    {
                        int SiteID = 0;
                        int.TryParse(P_arr[2], out SiteID);
                        if (SiteID != 0)
                        {
                            using (MySqlConnection conn = new MySqlConnection(Defaults.ConnStr))
                            {
                                string sql = "";
                                MySqlDataReader dr;
                                MySqlCommand cmd = new MySqlCommand(sql, conn);
                                conn.Open();

                                cmd.CommandText = "SELECT sitestatus FROM sendermsg.tblsites where siteid=" + SiteID;
                                dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    status = dr["sitestatus"].ToString();
									if (!Defaults.IsStatusActive(dr["sitestatus"].ToString(),true))
                                    {
                                        Response.Redirect("./");
                                    }
                                }
                                dr.Read();
                                conn.Close();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception)
        {

            Response.Redirect("./");
        }
        if (!Request.Url.Host.Contains("localhost") && !Request.IsSecureConnection && !Request.Url.Host.Contains("10.100.101"))
        {
            Response.Redirect(Request.Url.ToString().Replace("http:", "https:"));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["ismobile"]))
        {
            isMobile = true;
            FixStyleMethode();
        }
        if (isMobileFunc)
        {
            isMobile = true;
            //ClientScript.RegisterClientScriptBlock(GetType(), "asd", "alert('" + isMobile + "')", true);
        }

        int charCounter = 0;
        int maxChares = 1000000;

        string urlReferrer = "";
        if (Request.UrlReferrer != null)
        {
            urlReferrer = Request.UrlReferrer.ToString();
        }

        //if (Request.Form.Count > 0)
        try
        {
            if (Request.QueryString.Count > 0)
            {
                //if (Request.Form["landingID"] != null && int.TryParse(Request.Form["landingID"].ToString(), out landingID))
                if (Request.QueryString["landingID"] != null && int.TryParse(Request.QueryString["landingID"].ToString(), out landingID))
                {
                    Defaults.DelFromTable(landingID);

                }
                //string url = Request.Url.ToString().Replace("&check=" + Request.QueryString["check"], "").Replace("?check=" + Request.QueryString["check"], "");
                //Response.Redirect(url);

            }
        }
        catch (Exception err)
        {
            Response.Write("err: " + err.Message);
        }

        if (!string.IsNullOrEmpty(Request.QueryString["landingID"]) && int.TryParse(Request.QueryString["landingID"], out landingID))
        {

            Defaults.DelFromTable(landingID);

            //string url = Request.Url.ToString().Replace("&check=" + Request.QueryString["check"], "").Replace("?check=" + Request.QueryString["check"], "");
            //Response.Redirect(url);

        }




        pathAndQuery = Request.Url.Host + Request.Url.LocalPath;
        if (Request.QueryString["p"] != null)
        {
            pathAndQuery += "?p=" + Request.QueryString["p"];
        }
        //pathAndQuery = "https://n.sendmsg.co.il/f2822/e54tyyhtrghw";
        if (Request.QueryString["lang"] != null)
        {
            int.TryParse(Request.QueryString["lang"], out langId);
        }
        string langCode = Defaults.GetLangCodeFromLangID(langId);
        int domainID = Domain.getDomainIDFromSiteID(siteID);
        //using (MySqlConnection con = new MySqlConnection(ConnStr))
        //{
        //    string theQS = "";
        //    if (Request.QueryString != null)
        //    {
        //        theQS = Request.QueryString.ToString();
        //    }

        //    con.Open();
        //    MySqlCommand cmd = new MySqlCommand();
        //    cmd.Connection = con;
        //    //cmd.CommandText = "INSERT INTO sendingusers.tempLandingUsers (IP, VisitedAt,LandingORView,query) VALUES ('" + HttpContext.Current.Request.UserHostAddress + "',now(),1,'" + theQS.Replace("'", "''") + "') ";
        //    //cmd.ExecuteNonQuery();

        //    MyReader = cmd.ExecuteReader();
        //    if (MyReader.Read())
        //    {
        ShowPageCredentials.Text = Defaults.GetPages(60, langCode, domainID);
        msgContent = Defaults.GetPages(65, langCode, domainID);
        landingSubj = Defaults.GetPages(64, langCode, domainID);
        ShowPageDesc = Defaults.GetPages(66, langCode, domainID).Replace("msgSubj", "'msgSubj'");

        ShareFacebook = Defaults.GetPages(42, langCode, domainID);
        sendFriend = Defaults.GetPages(43, langCode, domainID);
        sendFriendBody = "";
        langDir = Defaults.GetPages(46, langCode, domainID);

        //    }
        //    MyReader.Close();
        //    con.Close();
        //}

        string[] parameters = { };

        //MsgTemplate.Text = msgBody;
        if (!string.IsNullOrEmpty(Request.QueryString["p"]))
        {
            parameters = Request.QueryString["p"].Split('-');
            if (parameters.Length == 3)
            {
                int.TryParse(parameters[0], out landingID);
                Int64.TryParse(parameters[1], out landingValidator);
                int.TryParse(parameters[2], out siteID);

                ClientScript.RegisterStartupScript(GetType(), "mobileContent", "if(screen.width <= " + widthForMobile + " || $(window).width() <= " + widthForMobile + ") { console.log('got inside'); console.log('" + String.IsNullOrEmpty(Request.QueryString["ismobile"]).ToString().ToLower() + "'); if('" + (!isMobileFunc).ToString().ToLower() + "' == 'true') { mobileContent('" + Request.QueryString["p"] + "'); } };", true);

            }
            else
            {
                return;
            }
        }

        //if (!string.IsNullOrEmpty(Request.QueryString["f"]))
        //{
        //    int.TryParse(Request.QueryString["f"], out facebookValidator);
        //}
        SendMsgMinisite MyMinisiteRow;
        if (siteID != 0 && landingID != 0)
        {

            using (MySqlConnection con = new MySqlConnection(ConnStr))
            {
                con.Open();//display the message

                MyMinisiteRow = Defaults.GetMiniste(siteID, landingID, con);
                con.Close();
            }



            if (MyMinisiteRow != null)
            {
                if (Request.QueryString["lang"] == null)
                {
                    langId = MyMinisiteRow.landingLang;
                    if (isMobile)
                    {
                        devider.Attributes["style"] = "margin:auto; width:100%;";
                    }
                    else
                    {
                        devider.Attributes["style"] = "margin:auto; max-width:" + MyMinisiteRow.landingPageWidth;
                    }

                    if (MyMinisiteRow.landingPageWidth == "500px" || isMobile)
                    {
                        viewPort1.Visible = true;
                    }
                    else
                    {
                        viewPort1.Visible = false;
                    }

                    //using (MySqlConnection con2 = new MySqlConnection(ConnStr))
                    //{
                    //    con2.Open();
                    //    MySqlCommand cmd2 = new MySqlCommand();
                    //    cmd2.Connection = con2;
                    //    MySqlDataReader MyReader2 = cmd2.ExecuteReader();
                    //    if (MyReader2.Read())
                    //    {
                            ShowPageCredentials.Text = Defaults.GetPages(60, langCode, domainID);
                            msgContent = Defaults.GetPages(65, langCode, domainID);
                            landingSubj = Defaults.GetPages(64, langCode, domainID);
                            ShowPageDesc = Defaults.GetPages(66, langCode, domainID).Replace("msgSubj", "'msgSubj'");

                            ShareFacebook = Defaults.GetPages(42, langCode, domainID);
                            sendFriend = Defaults.GetPages(43, langCode, domainID);
                            sendFriendBody = "";
                            langDir = Defaults.GetPages(46, langCode, domainID);
                            PageBody.Style.Add("direction", "ltr");
                            //  pageDiv.Style.Add("direction", langDir);
                            LandingHolder.Style.Add("direction", langDir);
                    //    }
                    //    MyReader2.Close();
                    //    con2.Close();
                    //}


                }


                if (MyMinisiteRow.landingTitle != null)
                {
                    landingSubj = Server.HtmlEncode(MyMinisiteRow.landingTitle);
                }
                msgSubj = MyMinisiteRow.landingTitle;



                PageBody.Style.Add("font-family", "Arial");

                string pageBodyStyle = PageBody.Attributes["style"];
                pageBodyStyle = pageBodyStyle.Trim();
                if (pageBodyStyle != "" && pageBodyStyle[pageBodyStyle.Length - 1] != ';')
                {
                    pageBodyStyle += ";";
                }

                string pageDivStyle = pageDiv.Attributes["style"];
                if (pageDivStyle != "" && pageDivStyle[pageDivStyle.Length - 1] != ';')
                {
                    pageDivStyle += ";";
                }

                //if (MyMinisiteRow.landingColor != "" && !isMobile)
                //{
                //    pageBodyStyle += "background-color:" + MyMinisiteRow.landingColor + ";";
                //    //pageDivStyle += "background-color:" + MyMinisiteRow.landingColor + ";";
                //}
                //else if (MyMinisiteRow.landingColorMobile != "" && isMobile)
                //{
                //    pageBodyStyle += "background-color:" + MyMinisiteRow.landingColorMobile + ";";
                //    //pageDivStyle += "background-color:" + MyMinisiteRow.landingColorMobile + ";";
                //}



                if (!isMobile) // desktop
                {
                    if (MyMinisiteRow.landingColor != "")
                    {
                        pageBodyStyle += "background-color:" + MyMinisiteRow.landingColor + ";";
                        //pageDivStyle += "background-color:" + MyMinisiteRow.landingColor + ";";
                    }
                }
                else if (isMobile) // mobile
                {
                    if (MyMinisiteRow.landingMobileContent != "")
                    {
                        if (MyMinisiteRow.landingColorMobile != "")
                        {
                            pageBodyStyle += "background-color:" + MyMinisiteRow.landingColorMobile + ";";
                            //pageDivStyle += "background-color:" + MyMinisiteRow.landingColorMobile + ";";
                        }
                        else { }
                    }
                    else if (MyMinisiteRow.landingColor != "")
                    {
                        pageBodyStyle += "background-color:" + MyMinisiteRow.landingColor + ";";
                        //pageDivStyle += "background-color:" + MyMinisiteRow.landingColor + ";";
                    }

                }



                if (status == "3")
                {
                    pageBodyStyle += "background-image:url('https://panel.sendmsg.co.il/templateContentImages/images/ThankU/BG_Sky.png');";
                }
                else
                {
                    //if (MyMinisiteRow.landingBgImg != "" && !isMobile)
                    //{
                    //    pageBodyStyle += "background-image:url('" + MyMinisiteRow.landingBgImg + "');";
                    //    //pageDivStyle +=  "background-image:url('" + MyMinisiteRow.landingBgImg + "');";
                    //}
                    //else if (MyMinisiteRow.landingBgImgMobile != "" && isMobile)
                    //{
                    //    pageBodyStyle += "background-image:url('" + MyMinisiteRow.landingBgImgMobile + "');";
                    //    //pageDivStyle += "background-image:url('" + MyMinisiteRow.landingBgImgMobile + "');";
                    //}


                    if (!isMobile) // desktop
                    {
                        if (MyMinisiteRow.landingBgImg != "")
                        {
                            pageBodyStyle += "background-image:url('" + MyMinisiteRow.landingBgImg + "');";
                            //pageDivStyle +=  "background-image:url('" + MyMinisiteRow.landingBgImg + "');"; 
                        }
                    }
                    else if (isMobile) // mobile
                    {
                        if (MyMinisiteRow.landingMobileContent != "")
                        {
                            if (MyMinisiteRow.landingBgImgMobile != "")
                            {
                                pageBodyStyle += "background-image:url('" + MyMinisiteRow.landingBgImgMobile + "');";
                                //pageDivStyle += "background-image:url('" + MyMinisiteRow.landingBgImgMobile + "');";
                            }
                            else { }
                        }
                        else if (MyMinisiteRow.landingBgImg != "")
                        {
                            pageBodyStyle += "background-image:url('" + MyMinisiteRow.landingBgImg + "');";
                            //pageDivStyle +=  "background-image:url('" + MyMinisiteRow.landingBgImg + "');"; 
                        }

                    }


                }

                //if (MyMinisiteRow.LandingBGImageOption != "" && !isMobile)
                //{
                //    pageBodyStyle += MyMinisiteRow.LandingBGImageOption;
                //    //pageDivStyle +=  "background-image:url('" + MyMinisiteRow.landingBgImg + "');";
                //}
                //else if (MyMinisiteRow.LandingBGImageOptionMobile != "" && isMobile)
                //{
                //    pageBodyStyle += MyMinisiteRow.LandingBGImageOptionMobile;
                //    //pageDivStyle += "background-image:url('" + MyMinisiteRow.landingBgImgMobile + "');";
                //}


                if (!isMobile) // desktop
                {
                    if (MyMinisiteRow.LandingBGImageOption != "")
                    {
                        pageBodyStyle += MyMinisiteRow.LandingBGImageOption;
                        //pageDivStyle +=  "background-image:url('" + MyMinisiteRow.LandingBGImageOption + "');"; 
                    }
                }
                else if (isMobile) // mobile
                {
                    if (MyMinisiteRow.landingMobileContent != "")
                    {
                        if (MyMinisiteRow.LandingBGImageOptionMobile != "")
                        {
                            pageBodyStyle += MyMinisiteRow.LandingBGImageOptionMobile;
                            //pageDivStyle += "background-image:url('" + MyMinisiteRow.LandingBGImageOptionMobile + "');";
                        }
                        else { }
                    }
                    else if (MyMinisiteRow.LandingBGImageOption != "")
                    {
                        pageBodyStyle += MyMinisiteRow.LandingBGImageOption;
                        //pageDivStyle +=  "background-image:url('" + MyMinisiteRow.LandingBGImageOption + "');"; 
                    }

                }


                PageBody.Attributes["style"] = pageBodyStyle;
                //pageDiv.Attributes["style"] = pageDivStyle;

                if (Defaults.IsStatusActive(MyMinisiteRow.SiteStatus.ToString(),true))
                {
                    if (MyMinisiteRow.tplDir == 1)
                    {
                        langDir = "ltr";
                    }
                    else
                    {
                        langDir = "rtl";
                    }

                    msgContent = "<div dir=\"" + langDir + "\" style=\"direction:" + langDir + ";text-align:center; width:100%;font-family:Arial \"><table cellpadding=\"0\" cellspacing=\"0\"; style=\"margin:auto; width:100%; font-family:Arial;\">";
                    //adds holder for "share on facebook" 

                    AnaliticsCode.Text = MyMinisiteRow.analyticsCode;

                    if (MyMinisiteRow.ShowFacebook || MyMinisiteRow.showShare)
                    {
                        msgContent += "<tr><td style=\"width:100%; text-align:left\">";
                        if (MyMinisiteRow.ShowFacebook)
                        {
                            msgContent += "<span id=\"showFacebook\"></span>";
                        }

                        if (MyMinisiteRow.showShare)
                        {
                            msgContent += "<span id=\"showForward\"></span>";
                        }
                        msgContent += "</td></tr>";
                    }

                    msgContent += "<tr><td class=\"landingContent\" style=\"padding-top:20px;direction:";
                    msgContent += langDir + (langDir == "rtl" ? "; text-align:right;" : "; text-align:left;");


                    //if (MyMinisiteRow.landingColor != "" && !isMobile)
                    //{
                    //    msgContent += "background-color:" + MyMinisiteRow.landingColor + ";";
                    //}
                    //else if (MyMinisiteRow.landingColorMobile != "" && isMobile)
                    //{
                    //    msgContent += "background-color:" + MyMinisiteRow.landingColorMobile + ";";
                    //}

                    if (!isMobile || MyMinisiteRow.landingMobileContent == "")
                    {
                        msgContent += "\">" + MyMinisiteRow.landingContent.Replace("http://www.youtube.com", "https://www.youtube.com") + "</td></tr>";
                    }
                    else
                    {
                        msgContent += "\">" + MyMinisiteRow.landingMobileContent.Replace("http://www.youtube.com", "https://www.youtube.com") + "</td></tr><script>$(window).dragWindow();</script>";
                    }

                    msgContent += "</table></div></font>";
                    if (MyMinisiteRow.ShowFBComments)
                    {
                        FBCommentsShow.Visible = true;
                    }

                    if (MyMinisiteRow.IndexInGoogle)
                    {
                        robots.Attributes["content"] = "INDEX, FOLLOW";
                        googlebot.Attributes["content"] = "INDEX, FOLLOW";
                    }
                    else
                    {
                        robots.Attributes["content"] = "NOINDEX, NOFOLLOW";
                        googlebot.Attributes["content"] = "NOINDEX, NOFOLLOW";
                    }

                    //msgContent = msgContent.Replace("<body style=\"", "<head><title>msgSubj</title><meta property=\"og:title\" content=\"msgSubj\" /><meta name=\"description\" content=\"" + ShowPageDesc + "\" /></head><body style=\"margin:0px auto 0px auto;width:800px;");
                    PageDesc.Attributes["content"] = ShowPageDesc.Replace("msgSubj", msgSubj);
                    PageOGDesc.Attributes["content"] = ShowPageDesc.Replace("msgSubj", msgSubj);
                    string HaveUserDesc = HaveUserDescription(landingID, siteID);
                    if (HaveUserDesc != "")
                    {
                        PageDesc.Attributes["content"] = HaveUserDesc;
                        PageOGDesc.Attributes["content"] = HaveUserDesc;
                    }
                    PageTitle.InnerHtml = landingSubj.Replace("&quot;", "\"");
                    PageOGTitle.Attributes["content"] = Server.HtmlDecode(landingSubj).Replace("&quot;", "'");



                    int landingValidatorFromDB = 0;
                    {
                        landingValidatorFromDB = MyMinisiteRow.siteID;
                        long ctrl = 1;
                        ctrl = (landingValidatorFromDB + 3) * ctrl * landingID * 36;
                        if (Math.Abs(ctrl) != Math.Abs(landingValidator))
                        {
                            //MyReader.Close();
                            // con.Close();
                            return;
                        }


                        ctrl = (landingValidatorFromDB + landingID) * 36;

                        if (siteID != 0 && landingID != 0)
                        {

                            //adds a link to see the message in browser ("can't see this smessage properly?" is added at the top)
                            ctrl = (siteID + 3) * landingID * 36;

                            //adds a link to share this message on facebook.
                            int facebookCtrl = (siteID + landingID) * 24;
                            msgContent = msgContent.Replace("id=\"showFacebook\">", "style=\"margin-left:10px; text-align:" + (langDir == "rtl" ? "right" : "left") + "; margin-right:10px; display:inline-block; text-align:" + (langDir == "rtl" ? "right" : " left") + "\"><a style=\"color:#919191;  text-decoration:none; font-weight:normal;font-size:14px; font-family:Arial; display:block; width:63; height:16px; text-align:" + (langDir == "rtl" ? "; right" : "; left") + "; vertical-align:middle; line-height:17px;  \" target=\"_blank\" href=\"https://www.facebook.com/sharer.php?u=" + Request.Url.Scheme + "://" + HttpUtility.UrlEncode("n.sendmsg.co.il/Minisites.aspx?p=" + landingID + "-" + ctrl.ToString() + "-" + siteID.ToString() + "&lang=" + langId) + "\"><img src=\"https://panel.sendmsg.co.il/images/facebook_Icon.png\" alt=\"\" style=\"vertical-align:middle; border:none; margin-left:3px;\" />" + ShareFacebook + "</a> ");
                            msgContent = msgContent.Replace("id=\"FacebookShare\"", "target=\"_blank\" href=\"https://www.facebook.com/sharer.php?u=" + Request.Url.Scheme + "://" + HttpUtility.UrlEncode("n.sendmsg.co.il/Minisites.aspx?p=" + landingID + "-" + ctrl.ToString() + "-" + siteID.ToString() + "&lang=" + langId) + "\"");
                            msgContent = msgContent.Replace("id=\"showForward\">", "style=\"display:inline-block;text-align:" + (langDir == "rtl" ? "right" : "left") + "margin-left:10px\"><a style=\"color:#919191; text-decoration:none; font-size:14px; font-family:Arial; display:inline-block; height:14px; vertical-align:middle; line-height:17px;  width:100px; margin-right:10px;\" href=\"mailto:?body=" + HttpUtility.UrlEncode("https://n.sendmsg.co.il/Minisites.aspx?p=" + landingID.ToString() + "-" + ctrl.ToString() + "-" + siteID.ToString() + "&lang=" + langId) + "\"><img src=\"https://panel.sendmsg.co.il/images/forward_Icon.png\" style=\"vertical-align:middle; border:none; margin-left:3px;\" />" + sendFriend + "</a>");
                            //msgContent = msgContent.Replace("id=\"showForward\">", "style=\"display:inline-block;text-align:right;margin-left:10px\"><a style=\"color:#919191; text-decoration:none; font-size:14px; font-family:Arial; display:inline-block; height:14px; vertical-align:middle; line-height:17px;  width:100px; margin-right:10px;\" href=\"" + HttpUtility.UrlEncode("mailto:?subject=FW:" + landingSubj + "&body=" + sendFriendBody + " " + "http://n.sendmsg.co.il/Minisites.aspx?p=" + landingID.ToString() + "-" + ctrl.ToString() + "-" + siteID.ToString() + "&lang=" + langId) + "\"><img src=\"https://panel.sendmsg.co.il/images/forward_Icon.png\" style=\"vertical-align:middle; border:none; margin-left:3px;\" />" + sendFriend + "</a>");

                            // replace image of facebook like to a real like
                            string strRegex = @"<img\s+src\s*=\s*[""'](.*/images/editLandingPagesNew/LikeBtn.png)[""']\s+(style\s*=\s*[""']([^""']+)[""'])*\s*/*>";
                            msgContent = Regex.Replace(msgContent, strRegex, "<div class=\"fb-like\" data-href=\"" + Request.Url.Scheme + "://" + "n.sendmsg.co.il/Minisites.aspx?p=" + landingID + "-" + ctrl.ToString() + "-" + siteID.ToString() + "&lang=" + langId + "\" data-layout=\"standard\" data-action=\"like\" data-show-faces=\"true\" data-share=\"false\"></div>");
                        }
                    }
                }
                else//if the site is not active
                {
                    string page = "";
                    if (langId == 1)
                    {
                        page = "<style id=\"creaditor-ul-responsive\">@media only screen and (max-width: 640px), only screen (-webkit-min-device-pixel-ratio: 1.5) {ul {max-width: 80%;}}</style><style id=\"creaditor-imgs-responsive\">@media only screen and (max-width: 640px), only screen (-webkit-min-device-pixel-ratio: 1.5) {img:not(.cicon) {height: initial !important; width: initial !important;max-width: 100% !important;}}</style><meta content=\"width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;\" name=\"viewport\"><style type=\"text/css\">   body {    background: url('https://panel.sendmsg.co.il/templateContentImages/images/ThankU/BG_Sky.png') center center no-repeat;    background-attachment: fixed;    background-size: cover;    background-position: 50% 50%;    background-repeat: no-repeat;   }    .baceDiv {    width: 100%;    float: right;   }    .CoverLogo {    float: right;    margin-right: 10%;    margin-left: 1%;   }    .CoverAllContent {    width: 865px;    margin: auto;    margin-bottom: 50px;   }    .Cube_Hardal {    background: #19addd;    width: 173px;    height: 58px;    margin-left: 16px;    border-radius: 15px;    box-shadow: 0px -1px #74dafa;    text-align: center;    display: table-cell;    vertical-align: middle;    color: #424242;    font-size: 26px;    text-decoration: none;    padding: 5px;    line-height: 26px;    font-weight: bold;    color: #fff;   }    .CoverCenterContent {    float: right;    width: 100%;    min-height: 131px;    position: relative;    z-index: 999;   }    .TitleTextCenter {    font-size: 64px;    color: #c21717;    padding-left: 70px;    margin-bottom: 25px;    text-align: center;    line-height: 64px;   }    .CoverCqube {    margin-left: 16px;    float: right;   }    .BottomImage {    width: 395px;    height: 268px;    background: url('https://panel.sendmsg.co.il/templateContentImages/images/ThankU/WomemImage.png')top right no-repeat;    background-size: 100% 100%;    position: absolute;    bottom: 0;    margin-bottom: 14px;    margin-right: 17px;   }    .CoverText {    margin-top: 25px;    margin-right: 5px;    float: right;    text-align: center;    margin-bottom: 15px;    margin-right: 15px;   }    .TopText {    margin-bottom: 1%;    /*background: #fff;    box-shadow: 0px 12px 37px -14px #000;*/    margin-top: 20px;   }    .TopText2 {    width: 100%;    font-size: 32px;    margin: auto;    display: table;   }    .TextCenter {    width: 100%;    float: right;    text-align: center;    color: #2f2f2f;    font-size: 64px;    padding-top: 80px;    line-height: 64px;   }    .CoverThreeQube {    float: right;    margin-top: 50px;    margin-right: 140px;    margin-bottom: 30px;   }    .CoverCqube .CoverCqube:last-child {    margin-left: 0;   }    @media only screen and (max-width : 640px) {     .CoverAllContent {     width: 100%;     margin: auto;     margin-bottom: 50px;    }     .CoverCenterContent {     float: right;     width: 100%;     padding-top: 0px;     min-height: 131px;     padding-right: 0px;     padding-bottom: 160px;     position: relative;     z-index: 999;     font-size: 0px;    }     .TitleTextCenter {     font-size: 44px;     color: #c21717;     padding-left: 0px;     margin-bottom: 25px;     text-align: center;    }     .CoverCqube {     margin: auto;     margin-bottom: 35px;     float: none;     display: table;    }     .BottomImage {     width: 72%;     height: 18%;     background: url('https://panel.sendmsg.co.il/templateContentImages/images/ThankU/WomemImage.png')top right no-repeat;     background-size: 100% 100%;     position: absolute;     bottom: 0;     margin-bottom: 14px;     margin-right: 17px;    }     .CoverText {     margin: 0;     width: 100%;    }     .TopText {     margin-bottom: 1%;     /*background: #fff;*/     margin-top: 7.5%;    }     .TopText2 {     width: 100%;     font-size: 32px;     margin: auto;     display: table;    }     .TextCenter {     width: 100%;     float: right;     text-align: center;     color: #2f2f2f;     font-size: 34px;     padding-top: 20px;    }      .CoverThreeQube {     margin: auto;     margin-top: 20px;     margin-right: 0;     margin-bottom: 20px;     width: 100%;    }     .logo {     margin-right: 0!important;     width: 100%;     text-align: center;    }   }  </style><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width: 905px; padding: 20px; width: calc(100% - 40px); height: auto; margin-right: auto; margin-left: auto;\" class=\"c-ignore cdtr-holder\" data-holderwidth=\"905px\">";
                        page += "<tbody class=\"c-ignore\"><tr class=\"cdtr-block cdtr-move c-ignore\" data-blocktext=\"בחר בלוק\">";
                        page += "<td class=\"c-ignore c-editable\">";
                        page += "<div class=\"baceDiv TopText c-ignore c-editable\">";
                        page += "<div class=\"TopText2 c-hover c-ignore c-editable\">";
                        page += "<div class=\"logo c-ignore\" style=\"float: right; margin-right: 170px;\">";
                        page += "<img src=\"https://panel.sendmsg.co.il/templateContentImages/images/TemplateGeneral/SendMsgLogoNoBg.png\" class=\"c-editable\">";
                        page += "</div>";
                        page += "<div class=\"CoverText c-editable\"><span style=\"font-size: 36px; font-weight: bold;\" class=\"c-editable\">שלח מסר</span>";
                        page += "<br>";
                        page += "<span class=\"c-hover c-editable\">מערכת דיוור אלקטרונית</span></div>";
                        page += "</div>";
                        page += "</div>";
                        page += "</td>";
                        page += "</tr>";
                        page += "<tr class=\"cdtr-block cdtr-move\" data-blocktext=\"בחר בלוק\">";
                        page += "<td class=\"c-hover c-editable\">";
                        page += "<div class=\"CoverAllContent c-hover c-editable\" id=\"CoverAllContent\" style=\"position: relative;\">";
                        page += "<div class=\"TextCenter c-editable\"><span dir=\"RTL\" class=\"c-editable\"></span><span lang=\"HE\" dir=\"RTL\" style=\"font-size: 48px; font-family: Arial, sans-serif; color: rgb(67, 67, 67);\" class=\"c-hover c-editable\">לא ניתן להציג את הדף מכוון וחשבונך טרם";
                        page += "הותר לשימוש. אנא נסה שוב מאוחר יותר.<br></span><span lang=\"HE\" dir=\"RTL\" style=\"font-size: 28px; font-family: Arial, sans-serif; color: rgb(0, 0, 0); line-height: 96px;\" class=\"c-hover c-editable\">ניתן ליצור קשר עם שירות הלקוחות בכתובת </span><span lang=\"HE\" dir=\"RTL\" style=\"font-size: 36px; font-family: 'Times New Roman', serif; color: rgb(31, 73, 125);\" class=\"c-editable\"><a href=\"mailto:send.help@comstar.co.il\" class=\"c-preventDefault c-editable\"><span lang=\"EN-US\" dir=\"LTR\" style=\"font-family: Calibri, sans-serif; line-height: 6px; font-size: 28px;\" class=\"c-hover c-editable\">send.help@comstar.co.il</span></a></span></div>";
                        page += "</div>";
                        page += "</td>";
                        page += "</tr>";
                        page += "</tbody></table><script id='cdtr-script-bootstrap'>setBootstrapCss();function setBootstrapCss(){var c,d,h=undefined;d=document;c=d.createElement('link');c.id='cdtr-bootstrap';c.rel='stylesheet';c.href='//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css';h=d.getElementsByTagName('head');h[0].appendChild(c); console.log('cdtr-bootstrap appended!')};</script>\"";

                    }
                    else
                    {

                        page = "<style id=\"creaditor-ul-responsive\">@media only screen and (max-width: 640px), only screen (-webkit-min-device-pixel-ratio: 1.5) {ul {max-width: 80%;}}</style><style id=\"creaditor-imgs-responsive\">@media only screen and (max-width: 640px), only screen (-webkit-min-device-pixel-ratio: 1.5) {img:not(.cicon) {height: initial !important; width: initial !important;max-width: 100% !important;}}</style><meta content=\"width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;\" name=\"viewport\"><style type=\"text/css\">   body { background: url('https://panel.sendmsg.co.il/templateContentImages/images/ThankU/BG_Sky.png') center center no-repeat; background-attachment: fixed; background-size: cover; background-position: 50% 50%; background-repeat: no-repeat;   } .baceDiv { width: 100%; float: right;   } .CoverLogo { float: right; margin-right: 10%; margin-left: 1%;   } .CoverAllContent { width: 865px; margin: auto; margin-bottom: 50px;   } .Cube_Hardal { background: #19addd; width: 173px; height: 58px; margin-left: 16px; border-radius: 15px; box-shadow: 0px -1px #74dafa; text-align: center; display: table-cell; vertical-align: middle; color: #424242; font-size: 26px; text-decoration: none; padding: 5px; line-height: 26px; font-weight: bold; color: #fff;   } .CoverCenterContent { float: right; width: 100%; min-height: 131px; position: relative; z-index: 999;   } .TitleTextCenter { font-size: 64px; color: #c21717; padding-left: 70px; margin-bottom: 25px; text-align: center; line-height: 64px;   } .CoverCqube { margin-left: 16px; float: right;   } .BottomImage { width: 395px; height: 268px; background: url('https://panel.sendmsg.co.il/templateContentImages/images/ThankU/WomemImage.png')top right no-repeat; background-size: 100% 100%; position: absolute; bottom: 0; margin-bottom: 14px; margin-right: 17px;   } .CoverText { margin-top: 25px; margin-right: 5px; float: right; text-align: center; margin-bottom: 15px; margin-right: 15px;   } .TopText { margin-bottom: 1%; /*background: #fff; box-shadow: 0px 12px 37px -14px #000;*/ margin-top: 20px;   } .TopText2 { width: 100%; font-size: 32px; margin: auto; display: table;   } .TextCenter { width: 100%; float: right; text-align: center; color: #2f2f2f; font-size: 64px; padding-top: 80px; line-height: 64px;   } .CoverThreeQube { float: right; margin-top: 50px; margin-right: 140px; margin-bottom: 30px;   } .CoverCqube .CoverCqube:last-child { margin-left: 0;   } @media only screen and (max-width : 640px) {  .CoverAllContent {  width: 100%;  margin: auto;  margin-bottom: 50px; }  .CoverCenterContent {  float: right;  width: 100%;  padding-top: 0px;  min-height: 131px;  padding-right: 0px;  padding-bottom: 160px;  position: relative;  z-index: 999;  font-size: 0px; }  .TitleTextCenter {  font-size: 44px;  color: #c21717;  padding-left: 0px;  margin-bottom: 25px;  text-align: center; }  .CoverCqube {  margin: auto;  margin-bottom: 35px;  float: none;  display: table; }  .BottomImage {  width: 72%;  height: 18%;  background: url('https://panel.sendmsg.co.il/templateContentImages/images/ThankU/WomemImage.png')top right no-repeat;  background-size: 100% 100%;  position: absolute;  bottom: 0;  margin-bottom: 14px;  margin-right: 17px; }  .CoverText {  margin: 0;  width: 100%; }  .TopText {  margin-bottom: 1%;  /*background: #fff;*/  margin-top: 7.5%; }  .TopText2 {  width: 100%;  font-size: 32px;  margin: auto;  display: table; }  .TextCenter {  width: 100%;  float: right;  text-align: center;  color: #2f2f2f;  font-size: 34px;  padding-top: 20px; }   .CoverThreeQube {  margin: auto;  margin-top: 20px;  margin-right: 0;  margin-bottom: 20px;  width: 100%; }  .logo {  margin-right: 0!important;  width: 100%;  text-align: center; }   }  </style><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width: 905px; padding: 20px; width: calc(100% - 40px); height: auto; margin-right: auto; margin-left: auto; top: 0px; left: 0px; min-width: initial;\" class=\"c-ignore cdtr-holder\" data-holderwidth=\"905px\">";
                        page += "<tbody class=\"c-ignore\"><tr class=\"cdtr-block cdtr-move c-ignore\" data-blocktext=\"בחר בלוק\">";
                        page += "<td class=\"c-ignore c-editable\">";
                        page += "<div class=\"baceDiv TopText c-ignore c-editable\">";
                        page += "<div class=\"TopText2 c-hover c-ignore c-editable\" style=\"background-image: none; background-attachment: scroll; background-color: rgb(255, 255, 255); background-size: auto; background-position: 0% 0%; background-repeat: repeat;\">";
                        page += "<div class=\"logo c-ignore\" style=\"float: right; margin-right: 170px;\">";
                        page += "<img src=\"https://panel.sendmsg.co.il/userfiles/site4018/images/logo_sendmsg_en_png.JPG\" class=\"c-editable\" data-fullwidth=\"166\" data-fullheight=\"116\" width=\"166\" height=\"116\" style=\"width: 166px; height: 116px; min-width: initial;\">";
                        page += "</div>";
                        page += "<div class=\"CoverText c-editable\" style=\"\"><span style=\"font-size: 36px; font-weight: bold;\" class=\"c-editable\">Sendmsg</span>";
                        page += "<br>";
                        page += "<span class=\"c-editable\" style=\"\">Email marketing system</span></div>";
                        page += "</div>";
                        page += "</div>";
                        page += "</td>";
                        page += "</tr>";
                        page += "<tr class=\"cdtr-block cdtr-move\" data-blocktext=\"בחר בלוק\">";
                        page += "<td class=\"c-editable\">";
                        page += "<div class=\"CoverAllContent c-editable\" id=\"CoverAllContent\" style=\"position: relative;\">";
                        page += "<div class=\"TextCenter c-editable\" style=\"direction: ltr;\"><span dir=\"RTL\" class=\"c-editable\"></span><span lang=\"HE\" dir=\"RTL\" style=\"font-size: 48px; font-family: Arial, sans-serif; color: rgb(67, 67, 67);\" class=\"c-editable\">Your account has not yet been approved. please try again later<br></span><span lang=\"HE\" dir=\"RTL\" style=\"font-size: 28px; font-family: Arial, sans-serif; color: rgb(0, 0, 0); line-height: 96px;\" class=\"c-editable\">You can contact our technical support team at&nbsp;</span><span lang=\"HE\" dir=\"RTL\" style=\"font-size: 36px; font-family: 'Times New Roman', serif; color: rgb(31, 73, 125);\" class=\"c-editable\"><a href=\"mailto:send.help@comstar.co.il\" class=\"c-preventDefault c-editable\"><span lang=\"EN-US\" dir=\"LTR\" style=\"font-family: Calibri, sans-serif; line-height: 6px; font-size: 28px;\" class=\"c-editable\">send.help@comstar.co.il</span></a></span></div>";
                        page += "</div>";
                        page += "</td>";
                        page += "</tr>";
                        page += "</tbody></table><script id='cdtr-script-bootstrap'>setBootstrapCss();function setBootstrapCss(){var c,d,h=undefined;d=document;c=d.createElement('link');c.id='cdtr-bootstrap';c.rel='stylesheet';c.href='//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css';h=d.getElementsByTagName('head');h[0].appendChild(c); console.log('cdtr-bootstrap appended!')};</script>";

                    }
                    msgContent = "<div style=\"direction:ltr; text-align:left\">This page was not found. <br />Please try again later or contact the person who directed you to this page";
                    msgContent = page;
                    PageDesc.Attributes["content"] = ShowPageDesc;
                    PageOGDesc.Attributes["content"] = ShowPageDesc;
                    string HaveUserDesc = HaveUserDescription(landingID, siteID);
                    if (HaveUserDesc != "")
                    {
                        PageDesc.Attributes["content"] = HaveUserDesc;
                        PageOGDesc.Attributes["content"] = HaveUserDesc;
                    }
                    PageTitle.InnerHtml = landingSubj.Replace("&quot;", "\"");
                    PageOGTitle.Attributes["content"] = landingSubj.Replace("&quot;", "'");
                }

                if (MyMinisiteRow.promoShowOnTop)
                {
                    TopAds.Visible = true;
                }
                if (MyMinisiteRow.promoShowOnSide)
                {
                    SideAds.Visible = true;
                }
                if (MyMinisiteRow.promoShowOnBottom)
                {
                    BottomAds1.Visible = true;
                }

                string contentadsSubjIDs = " 1=2 ";
                string notContentadsSubjIDs = " 1=2 ";
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    string sql = "SELECT subjID FROM sendermsg.contentSubjAdvert WHERE isDel=0 AND SiteID=" + siteID;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string subjID = dr["subjID"].ToString();
                        contentadsSubjIDs += " OR SubjIDs='" + subjID + "' OR SubjIDs LIKE '" + subjID + ",%' OR SubjIDs LIKE '%," + subjID + "' OR SubjIDs LIKE '%," + subjID + ",%'";
                    }
                    dr.Close();

                    cmd.CommandText = "SELECT subjID FROM sendermsg.contentSubjNotAdvert WHERE isDel=0 AND SiteID=" + siteID;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string subjID = dr["subjID"].ToString();
                        notContentadsSubjIDs += " OR SubjIDs='" + subjID + "' OR SubjIDs LIKE '" + subjID + ",%' OR SubjIDs LIKE '%," + subjID + "' OR SubjIDs LIKE '%," + subjID + ",%'";
                    }
                    dr.Close();

                    int maxAdType = 0;
                    sql = "SELECT MAX(AdType) AS MaxAdType FROM contentads";
                    cmd.CommandText = sql;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        int.TryParse(dr["MaxAdType"].ToString(), out maxAdType);
                    }
                    dr.Close();



                    int adsCounter = 1;
                    int maxAdsPlaces = 3; // top, side, bottom                        
                    int adType = 0;
                    int topAdType = 0;
                    int sideAdType = 0;
                    int bottomAdType = 0;
                    string selectedTop3Query = "";
                    bool showSide = false;
                    bool showTop = false;
                    if (MyMinisiteRow.contentNetworkActive)//indicates the content network is active
                    {
                        PromoSenderLogo.Visible = true;

                        for (int i = 0; i < maxAdsPlaces; i++)
                        {
                            bool showFlag = false;

                            string contentadsAdType = "";
                            switch (i)
                            {
                                case 0:
                                    if (MyMinisiteRow.promoOnTopType != 0)
                                    {
                                        contentadsAdType = " AND AdType=" + MyMinisiteRow.promoOnTopType;
                                        showTop = true;
                                    }
                                    showFlag = MyMinisiteRow.promoShowOnTop;
                                    break;

                                case 1:
                                    if (MyMinisiteRow.promoOnSideType != 0)
                                    {
                                        contentadsAdType = " AND AdType=" + MyMinisiteRow.promoOnSideType;
                                        showSide = true;
                                    }
                                    showFlag = MyMinisiteRow.promoShowOnSide;
                                    break;

                                case 2:
                                    if (MyMinisiteRow.promoOnBottomType != 0)
                                    {
                                        contentadsAdType = " AND AdType=" + MyMinisiteRow.promoOnBottomType;
                                    }
                                    showFlag = MyMinisiteRow.promoShowOnBottom;
                                    break;
                            }

                            if (showFlag)
                            {
                                int randomNum = 0;
                                int minNum = 0;
                                int maxNum = 9;
                                Random rnd = new Random();
                                int.TryParse(rnd.Next(minNum, maxNum).ToString(), out randomNum);

								sql = "SELECT contentads.siteID AS siteID,ContentAdID,ContentAdName,ContentAdImage,ContentAdAltText,ContentAdUrl,AdType,AdText,(tblsites.ContentEarned-tblsites.ContentUsed) AS UserPoints" +
									" FROM contentads LEFT JOIN tblsites ON contentads.siteID=tblsites.siteID" +
									" WHERE (" + contentadsSubjIDs + ") " + contentadsAdType + selectedTop3Query + " AND NOT (" + notContentadsSubjIDs + ") AND contentads.isDel=0 AND contentads.isActive=1 AND contentads.ContentAdUrl<>0 AND contentads.SiteID<>" + siteID + " AND (tblsites.ContentEarned-sendermsg.tblsites.ContentUsed)>1 AND tblsites.contentNetworkActive=1 AND (" + Defaults.activeStatusIDsQuery + ") ORDER BY UserPoints DESC LIMIT " + randomNum + ",1";
                                cmd.CommandText = sql;
                                dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    selectedTop3Query += " AND ContentAdID<>" + dr["ContentAdID"].ToString();

                                    int.TryParse(dr["AdType"].ToString(), out adType);
                                    int adSiteID = 0;
                                    int adLandingID = 0;
                                    int contentAdID = 0;
                                    int.TryParse(dr["siteID"].ToString(), out adSiteID);
                                    int.TryParse(dr["ContentAdUrl"].ToString(), out adLandingID);
                                    int.TryParse(dr["ContentAdID"].ToString(), out contentAdID);
                                    string hrefLink = "https://n.sendmsg.co.il/Minisites.aspx?p=" + adLandingID + "-" + ((adSiteID + 3) * adLandingID * 36) + "-" + adSiteID;
                                    string adTextDiv = CreateAdTextDiv(adType, dr["ContentAdAltText"].ToString(), dr["AdText"].ToString(), dr["ContentAdImage"].ToString(), hrefLink, adSiteID, siteID, contentAdID, landingID, i + 1);
                                    UpdateAdLiteral(i + 1, adTextDiv);

                                    if (i == 0)
                                    {
                                        TopAdsHolder.Style["background"] = "#ffffff";//"#dfdfdf";
                                        LandingHolder.Style["padding-top"] = "20px";
                                    }
                                    if (i == 1)
                                    {
                                        SideAdsHolder.Style["width"] = "150px";
                                        SideAdsHolder.Style["background"] = "#ffffff";// "#efefef";
                                        SideAdsHolder.Style["vertical-align"] = "top";
                                        SideAdsHolder.Visible = true;
                                        if (langDir.ToLower() == "lrt")
                                        {
                                            SideAdsHolder.Style["border-left"] = "solid 2px #cecece;";
                                        }
                                        else
                                        {
                                            SideAdsHolder.Style["border-right"] = "solid 2px #cecece;";
                                        }
                                    }
                                    if (i == 2)
                                    {
                                        BottomAds1.Style["border-top"] = "solid 2px #cecece;";
                                        BottomAds1.Style["border-bottom"] = "solid 2px #cecece;";
                                    }
                                }
                                dr.Close();

                                switch (i)
                                {
                                    case 0:
                                        topAdType = adType;
                                        break;

                                    case 1:
                                        sideAdType = adType;
                                        break;

                                    case 2:
                                        bottomAdType = adType;
                                        break;
                                }
                            }
                        }




                        for (int i = 3; i < 9; i++)
                        {
                            bool showFlag = false;

                            string contentadsAdType = "";
                            switch (i)
                            {
                                case 3:
                                case 4:
                                    //contentadsAdType = " AND AdType=" + topAdType;
                                    showFlag = MyMinisiteRow.promoShowOnTop;
                                    break;

                                case 5:
                                case 6:
                                    //contentadsAdType = " AND AdType=" + sideAdType;
                                    showFlag = MyMinisiteRow.promoShowOnSide;
                                    break;

                                case 7:
                                case 8:
                                    //contentadsAdType = " AND AdType=" + bottomAdType;
                                    showFlag = MyMinisiteRow.promoShowOnBottom;
                                    break;
                            }

                            if (showFlag)
                            {
                                sql = "SELECT sendermsg.contentads.siteID,ContentAdID,ContentAdName,ContentAdImage,ContentAdAltText,ContentAdUrl,AdType,AdText,(sendermsg.tblsites.ContentEarned-sendermsg.tblsites.ContentUsed) AS UserPoints" +
                                    " FROM sendermsg.contentads LEFT JOIN sendermsg.tblsites ON sendermsg.contentads.siteID=sendermsg.tblsites.siteID" +
                                    " WHERE (" + contentadsSubjIDs + ") " + contentadsAdType + selectedTop3Query + " AND NOT (" + notContentadsSubjIDs + ") AND sendermsg.contentads.isDel=0 AND sendermsg.contentads.isActive=1 AND sendermsg.contentads.ContentAdUrl<>0 AND sendermsg.contentads.SiteID<>" + siteID + selectedTop3Query + " AND tblsites.contentNetworkActive=1 AND (tblsites.ContentEarned-sendermsg.tblsites.ContentUsed)>1 AND ("+Defaults.activeStatusIDsQuery+") ORDER BY RAND() DESC;"; // LIMIT 6";
                                cmd.CommandText = sql;
                                dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    selectedTop3Query += " AND ContentAdID<>" + dr["ContentAdID"].ToString();

                                    int.TryParse(dr["AdType"].ToString(), out adType);
                                    int adSiteID = 0;
                                    int adLandingID = 0;
                                    int contentAdID = 0;
                                    int.TryParse(dr["siteID"].ToString(), out adSiteID);
                                    int.TryParse(dr["ContentAdUrl"].ToString(), out adLandingID);
                                    int.TryParse(dr["ContentAdID"].ToString(), out contentAdID);
                                    string hrefLink = "https://n.sendmsg.co.il/Minisites.aspx?p=" + adLandingID + "-" + ((adSiteID + 3) * adLandingID * 36) + "-" + adSiteID;
                                    string adTextDiv = CreateAdTextDiv(adType, dr["ContentAdAltText"].ToString(), dr["AdText"].ToString(), dr["ContentAdImage"].ToString(), hrefLink, adSiteID, siteID, contentAdID, landingID, i + 1);
                                    UpdateAdLiteral(i + 1, adTextDiv);
                                }
                                dr.Close();
                            }
                        }
                    }


                    conn.Close();
                }

            }
            else
            {
                //MyReader.Close();
                //   con.Close();
                return; ;
            }

            //MyReader.Close();

            int startAt = 0;
            int endAt = 2;


            //string currentIP = HttpContext.Current.Request.UserHostAddress;
            //if (MyReader["SentOpenIP"].ToString() != "" && MyReader["SentOpenIP"].ToString() != currentIP)
            //{
            //    sql = "UPDATE SentMsgs SET NumFwd=(NumFwd+1) Where MsgID=" + msgID;
            //    MySqlCommand cmd2 = new MySqlCommand(sql, new MySqlConnection(ConnStr));
            //    cmd2.Connection.Open();
            //    cmd2.ExecuteNonQuery();

            //    cmd2.CommandText = "UPDATE `sendingUsers`.`sendingUsers" + siteID.ToString() + "` SET NumFwd=(NumFwd+1) Where sentID=" + sentID;
            //    cmd2.ExecuteNonQuery();
            //    cmd2.Connection.Close();
            //}
            //else
            //{
            using (MySqlConnection con3 = new MySqlConnection(ConnStr))
            {
                MySqlCommand cmd3 = new MySqlCommand();
                cmd3.Connection = con3;
                cmd3.CommandText = "UPDATE LandingPages SET NumOpened=(NumOpened+1) Where landingID=" + landingID;
                if (isMobile)
                {
                    cmd3.CommandText += ";UPDATE LandingPages SET NumOpenedMobile=(NumOpenedMobile+1) Where landingID=" + landingID;
                }

                con3.Open();
                cmd3.ExecuteNonQuery();
                con3.Close();
            }
            //cmd2.CommandText = "UPDATE `sendingUsers`.`sendingUsers" + siteID.ToString() + "` SET NumOpened=(NumOpened+1), SentOpenIP ='" + currentIP + "', openTime =now() Where sentID=" + sentID;
            //cmd2.ExecuteNonQuery();
            //cmd2.Connection.Close();
            //}


            #region checking if there is ChangingDate field (Saved Tag) in the template, which has to be replaced with changingData Field (Saved Tag)
            startAt = 0;
            endAt = 2;
            while (startAt != -1 && endAt != -1 && msgContent.Length > 5 && charCounter < maxChares)//looking for all the field names in the message
            {
                charCounter++;
                string cdTitle = "";
                string cleancdTitle = "";
                startAt = msgContent.IndexOf("[[[", startAt);
                endAt = msgContent.IndexOf("]]]", startAt + 4);

                if (startAt != -1 && endAt != -1)
                {
                    startAt++;
                    cdTitle = msgContent.Substring((startAt + 2), (endAt - (startAt + 2)));
                    //if the user has html code inside the FieldAlias field, it is being destroyed.
                    if (cdTitle.Contains("<"))
                    {
                        string removeWhat = "";
                        int InnerstartAt = 0;
                        int InnerendAt = 0;

                        InnerstartAt = cdTitle.IndexOf("<");
                        InnerendAt = cdTitle.LastIndexOf(">");
                        removeWhat = cdTitle.Substring(InnerstartAt, (InnerendAt - InnerstartAt) + 1);
                        if (removeWhat.Length > 0)
                            cleancdTitle = cdTitle.Replace(removeWhat, "");
                    }
                    else
                    {
                        cleancdTitle = cdTitle;
                    }

                    string sql2 = "SELECT cdContent FROM tblChangingData WHERE cdTitle='" + cleancdTitle + "' AND SiteID=" + siteID;
                    using (MySqlConnection con2 = new MySqlConnection(ConnStr))
                    {
                        MySqlDataReader MyReader2;
                        con2.Open();
                        MySqlCommand cmd2 = new MySqlCommand(sql2, con2);

                        MyReader2 = cmd2.ExecuteReader();

                        if (MyReader2.Read())
                        {
                            msgContent = msgContent.Replace("[[[" + cdTitle + "]]]", MyReader2["cdContent"].ToString());
                        }
                        else
                        {
                            msgContent = msgContent.Replace("[[[" + cdTitle + "]]]", "");
                        }
                        MyReader2.Close();
                        con2.Close();
                    }
                }
            }
            #endregion
        }
        msgContent = msgContent.Replace("=\"/", "=\"https://panel.sendmsg.co.il/").Replace("url(/", "url(https://panel.sendmsg.co.il/").Replace("msgSubj", msgSubj);
        //msgContent = msgContent.Replace("&nbsp;", " ");

        if (Request.Url.ToString().StartsWith("http:"))
        {
            msgContent = msgContent.Replace("src=\"https://", "src=\"http://");
        }

        if (msgContent.Contains("data-cke"))
        {
            msgContent = msgContent.Replace("contenteditable=\"false\"", "").Replace("data-cke-editable=\"1\"", "").Replace("data-cke-saved-name=", "name=").Replace("data-cke-pa-onclick=", "onclick=");
        }


        string newcontact = SetNewContact(msgContent.Replace("'/AddUserFromSite.aspx'", "'https://panel.sendMsg.co.il/AddUserFromSite.aspx'"));

        // landingTemplate.Text = msgContent.Replace("'/AddUserFromSite.aspx'", "'https://panel.sendMsg.co.il/AddUserFromSite.aspx'");
        landingTemplate.Text = newcontact;
        try
        {
            string langCalender = "";
            if (newcontact.Contains("loadlang('"))
            {
                int cut = newcontact.IndexOf("loadlang('") + 10;

                langCalender = newcontact.Remove(0, cut);
                langCalender = langCalender.Remove(3, langCalender.Length - 3);
                string langCulture = "";

                using (MySqlConnection conn = new MySqlConnection(Defaults.ConnStr))
                {
                    string sql = "SELECT langCulture FROM sendermsg.languages where LangCode = '" + langCalender + "';";
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        langCulture = dr["langCulture"].ToString().Replace("-", "_");
                    }
                    dr.Close();
                    conn.Close();
                }

                if (langCalender.ToLower() != "eng")
                {
                    bool haveLangCalender = false;
                    if (langCalender.ToLower() == "ara")
                    {
                        langCulture = "ar";
                        haveLangCalender = true;
                    }
                    else
                    {
                        string path = Server.MapPath(@"~\script\pickadate\translations");
                        string[] filePaths = Directory.GetFiles(path, "*.js");

                        if (filePaths.Contains(path + "\\" + langCulture + ".js"))
                        {
                            haveLangCalender = true;
                        }
                    }

                    if (haveLangCalender)
                    {
                        LangCalender.Text = "<script src='script/pickadate/translations/" + langCulture + ".js'></script>";
                        
                    }

                }
            }
        }
        catch { }
        if (Request.QueryString["harlem"] != null)
        {
            //string harlem = "javascript: (function () { function c() { var e = document.createElement(\"link\"); e.setAttribute(\"type\", \"text/css\"); e.setAttribute(\"rel\", \"stylesheet\"); e.setAttribute(\"href\", f); e.setAttribute(\"class\", l); document.body.appendChild(e) } function h() { var e = document.getElementsByClassName(l); for (var t = 0; t < e.length; t++) { document.body.removeChild(e[t]) } } function p() { var e = document.createElement(\"div\"); e.setAttribute(\"class\", a); document.body.appendChild(e); setTimeout(function () { document.body.removeChild(e) }, 100) } function d(e) { return { height: e.offsetHeight, width: e.offsetWidth } } function v(i) { var s = d(i); return s.height > e && s.height < n && s.width > t && s.width < r } function m(e) { var t = e; var n = 0; while (!!t) { n += t.offsetTop; t = t.offsetParent } return n } function g() { var e = document.documentElement; if (!!window.innerWidth) { return window.innerHeight } else if (e && !isNaN(e.clientHeight)) { return e.clientHeight } return 0 } function y() { if (window.pageYOffset) { return window.pageYOffset } return Math.max(document.documentElement.scrollTop, document.body.scrollTop) } function E(e) { var t = m(e); return t >= w && t <= b + w } function S() { var e = document.createElement(\"audio\"); e.setAttribute(\"class\", l); e.src = i; e.loop = false; e.addEventListener(\"canplay\", function () { setTimeout(function () { x(k) }, 500); setTimeout(function () { N(); p(); for (var e = 0; e < O.length; e++) { T(O[e]) } }, 15500) }, true); e.addEventListener(\"ended\", function () { N(); h() }, true); e.innerHTML = \" <p>If you are reading this, it is because your browser does not support the audio element. We recommend that you get a new browser.</p> <p>\"; document.body.appendChild(e); e.play() } function x(e) { e.className += \" \" + s + \" \" + o } function T(e) { e.className += \" \" + s + \" \" + u[Math.floor(Math.random() * u.length)] } function N() { var e = document.getElementsByClassName(s); var t = new RegExp(\"\\b\" + s + \"\\b\"); for (var n = 0; n < e.length;) { e[n].className = e[n].className.replace(t, \"\") } } var e = 30; var t = 30; var n = 350; var r = 350; var i = \"//s3.amazonaws.com/moovweb-marketing/playground/harlem-shake.mp3\"; var s = \"mw-harlem_shake_me\"; var o = \"im_first\"; var u = [\"im_drunk\", \"im_baked\", \"im_trippin\", \"im_blown\"]; var a = \"mw-strobe_light\"; var f = \"//s3.amazonaws.com/moovweb-marketing/playground/harlem-shake-style.css\"; var l = \"mw_added_css\"; var b = g(); var w = y(); var C = document.getElementsByTagName(\"*\"); var k = null; for (var L = 0; L < C.length; L++) { var A = C[L]; if (v(A)) { if (E(A)) { k = A; break } } } if (A === null) { console.warn(\"Could not find a node of the right size. Please try a different page.\"); return } c(); S(); var O = []; for (var L = 0; L < C.length; L++) { var A = C[L]; if (v(A)) { O.push(A) } } })()";
            HarlemShake.Text = "<script src=\"https://www.sendmsg.co.il/js/harlem.js\"  type=\"text/javascript\"></script>";
        }
    }


    protected string SetNewContact(string myText)
    {

        List<string> myArray = new List<string>();

        if (Request.QueryString != null && Request.QueryString.Count > 0)
        {
            myArray = new List<string>(Request.QueryString.AllKeys.ToList());
        }

        for (int i = 0; i < myArray.Count; i++)
        {
            SetQuery(ref  myText, myArray[i].ToLower());
        }

        return myText;
    }

    protected void SetQuery(ref string myText, string query)
    {


        if (Request.QueryString[query] != null)
        {
            myText = Regex.Replace(myText, @"\[\[" + query + @"\]\]", Request.QueryString[query], RegexOptions.IgnoreCase);

            //myText = myText.Replace("[[" + query + "]]",);
        }
        else
        {
            // myText = myText.Replace("[[" + query + "]]", "");
            myText = Regex.Replace(myText, @"\[\[" + query + @"\]\]", "", RegexOptions.IgnoreCase);
        }

    }

    protected string CreateAdTextDiv(int adType, string contentAdAltText, string adText, string contentAdImage, string hrefLink, int adSiteID, int publisherSiteID, int ContentAdID, int currentLandingID, int adPosition)
    {
        string imgStyle = "max-height:72px;max-width:100px;";
        bool sidePos = false;
        string floatStyle = "float:right;";
        if (adPosition == 2 || adPosition == 6 || adPosition == 7)
        {
            sidePos = true;
            imgStyle = "max-height:72px;max-width:100px;";
            floatStyle = "float:none;";

        }
        else
        {
            if (langDir == "rtl")
            {
                if (adText == "")
                {
                    floatStyle = "float:right;";
                }
                else
                {
                    floatStyle = "float:left;";
                }
            }
            else
            {
                if (adText == "")
                {
                    floatStyle = "float:right;";
                }
                else
                {
                    floatStyle = "float:left;";
                }
            }
        }
        string divAdHolder = "<div class=\"SendMsgAd\" style=\"direction:" + langDir + ";text-align:" + (langDir == "rtl" ? "right;" : "left;") + "\">";
        divAdHolder += "<input type=\"hidden\" value=\"" + publisherSiteID + "__" + adSiteID + "__" + ContentAdID + "__" + currentLandingID + "__" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(((ContentAdID + adSiteID) * 14 - currentLandingID).ToString(), "md5") + "\" class=\"SendMsgAdHid\" />";
        divAdHolder += "<a href=\"" + hrefLink + "\" class=\"contentAdAltTextClass\" style=\"text-align:" + (langDir == "rtl" ? "right" : "left") + ";display:block;margin-bottom:5px;color:#0269B1;font-weight:bold;font-size:14px;text-decoration:none;\" target=\"_blank\">" + contentAdAltText + "</a>";
        divAdHolder += "<div style=\"" + (sidePos ? "" : "height:72px;") + "\"><a class=\"adTextClass\" style=\"display:block;direction:" + langDir + "; text-align:" + (langDir == "rtl" ? "right" : "left") + ";color:#696969;font-size:12px;text-decoration:none;\" href=\"" + hrefLink + "\" target=\"_blank\">";
        #region 2 types code - when the ads are text or image
        //if (adType == 2) // is text ad
        //{
        //    divAdHolder += adText;
        //}
        //else // is picture ad
        //{
        //    divAdHolder += "<img " + ("src=\"" + contentAdImage.ToLower() + "\")").Replace("src=\"uploads", "src=\"https://panel.sendmsg.co.il/Uploads").Replace("src=\"/uploads", "src=\"https://panel.sendmsg.co.il/Uploads").Replace("src=\"userfiles", "src=\"https://panel.sendmsg.co.il/userfiles") + " alt=\"" + contentAdAltText + "\" class=\"contentAdImageClass\" style=\"border:none;"+imgStyle+"\" />";
        //}
        #endregion

        if (!String.IsNullOrEmpty(contentAdImage))
        {
            divAdHolder += "<img " + ("src=\"" + contentAdImage.ToLower() + "\")").Replace("src=\"uploads", "src=\"https://panel.sendmsg.co.il/Uploads").Replace("src=\"/uploads", "src=\"https://panel.sendmsg.co.il/Uploads").Replace("src=\"userfiles", "src=\"https://panel.sendmsg.co.il/userfiles") + " alt=\"" + contentAdAltText + "\" class=\"contentAdImageClass\" style=\"border:none;margin:0 5px 5px 5px;" + imgStyle + floatStyle + "\" />";
        }
        divAdHolder += "<span style=\"font-size:14px\">" + adText + "</span>";

        divAdHolder += "</a></div>";
        divAdHolder += "</div>";

        return divAdHolder;
    }

    /// <summary>
    /// Pay attention: there is a friendly url on this site, so pay attention for the Global.asax file!
    /// also... DO NOT transfer parameters through this method. get it only by "Request.QueryString" since it's a "Get" method.
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json, UseHttpGet = true)]
    public static string showAd()
    {
        string AdInfo = HttpContext.Current.Request.QueryString["AdInfo"];


        string allValues = "";
        if (AdInfo != null)
        {
            string[] seperator = { "__" };
            string[] AdInfoArr = AdInfo.Split(seperator, StringSplitOptions.None);
            if (AdInfoArr.Length == 6)
            {
                int publisherSiteID = 0;
                int adSiteID = 0;
                int ContentAdID = 0;
                int currentLandingID = 0;
                if (int.TryParse(AdInfoArr[0], out publisherSiteID) && int.TryParse(AdInfoArr[1], out adSiteID) && int.TryParse(AdInfoArr[2], out ContentAdID) && int.TryParse(AdInfoArr[3], out currentLandingID))
                {
                    if (System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(((ContentAdID + adSiteID) * 14 - currentLandingID).ToString(), "md5") == AdInfoArr[4])
                    {
                        bool addCount = true;
                        bool userFound = false;
                        pageView3 tempPageView = new pageView3();
                        lock (PageViewsList)
                        {
                            for (int i = PageViewsList.Count - 1; i >= 0; i--)
                            {
                                if (PageViewsList[i].ViewerTime < DateTime.Now.AddMinutes(-30))
                                {
                                    PageViewsList.RemoveAt(i);
                                }
                                else
                                {
                                    if (PageViewsList[i].ViewerIP == HttpContext.Current.Request.UserHostAddress && PageViewsList[i].ViewdPage == currentLandingID)
                                    {
                                        userFound = true;
                                        tempPageView = PageViewsList[i];
                                        if (PageViewsList[i].MsgCount > 8)
                                        {
                                            addCount = false;
                                        }
                                    }
                                }
                            }
                        }
                        if (addCount)
                        {

                            if (!userFound)
                            {
                                tempPageView.ViewdPage = currentLandingID;
                                tempPageView.ViewerIP = HttpContext.Current.Request.UserHostAddress;
                                tempPageView.ViewerTime = DateTime.Now;
                                tempPageView.MsgCount = 0;
                                PageViewsList.Add(tempPageView);
                            }

                            tempPageView.MsgCount = tempPageView.MsgCount + 1;

                            string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
                            using (MySqlConnection con = new MySqlConnection(ConnStr))
                            {
                                string theQS = "";

                                con.Open();
                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = con;
                                if (AdInfoArr[5] == "click")
                                {
                                    cmd.CommandText = "UPDATE contentads set AdClicks = (AdClicks+1) WHERE ContentAdID=" + ContentAdID + " AND siteID=" + adSiteID;
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    cmd.CommandText = "UPDATE tblSites set ContentEarned = (ContentEarned+10) WHERE siteID=" + publisherSiteID;
                                    cmd.ExecuteNonQuery();
                                    cmd.CommandText = "UPDATE tblSites set ContentUsed = (ContentUsed+11) WHERE SiteID=" + adSiteID;
                                    cmd.ExecuteNonQuery();
                                    cmd.CommandText = "UPDATE contentads set AdViews = (AdViews+1) WHERE ContentAdID=" + ContentAdID + " AND siteID=" + adSiteID;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            return "Counted : " + tempPageView.MsgCount;
                        }
                        else
                        {
                            return "NOT Counted!!";
                        }
                        //PageViewsList

                    }
                    else
                    {
                        return "err4";
                    }
                }
                else
                {
                    return "err3";
                }
            }
            else
            {
                return "err2";
            }
        }

        return "err";

    }

    /// <summary>
    /// Pay attention: there is a friendly url on this site, so pay attention for the Global.asax file!
    /// also... DO NOT transfer parameters through this method. get it only by "Request.QueryString" since it's a "Get" method.
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json, UseHttpGet = true)]
    public static string GetMobileContent()
    {
        string query_String = HttpContext.Current.Request.QueryString["query_String"];

        string mobileContentText = "";
        string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
        string[] parameters = { };
        int siteID = 0;
        int landingID = 0;
        Int64 landingValidator;

        if (!string.IsNullOrEmpty(query_String))
        {
            parameters = query_String.Split('-');
            if (parameters.Length == 3)
            {
                int.TryParse(parameters[0], out landingID);
                Int64.TryParse(parameters[1], out landingValidator);
                int.TryParse(parameters[2], out siteID);

                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    string sql = String.Format("SELECT landingMobileContent,landingColorMobile FROM landingPages WHERE siteID={0} AND landingID={1}", siteID, landingID);
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        mobileContentText = dr["landingColorMobile"].ToString() + "sendmsgMobileColor" + dr["landingMobileContent"].ToString();
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            else
            {
                return mobileContentText;
            }
        }


        return mobileContentText;
    }


    protected void UpdateAdLiteral(int adsCounter, string adTextDiv)
    {
        switch (adsCounter)
        {
            case 1:
                Ad1.Text = adTextDiv;
                break;
            case 2:
                Ad2.Text = adTextDiv;
                break;
            case 3:
                Ad3.Text = adTextDiv;
                break;
            case 4:
                Ad4.Text = adTextDiv;
                break;
            case 5:
                Ad5.Text = adTextDiv;
                break;
            case 6:
                Ad6.Text = adTextDiv;
                break;
            case 7:
                Ad7.Text = adTextDiv;
                break;
            case 8:
                Ad8.Text = adTextDiv;
                break;
            case 9:
                Ad9.Text = adTextDiv;
                break;
        }
    }
    public void FixStyleMethode()
    {
        FixStyle.Text = "<style>iframe {max-width:100%;} </style>";
    }

    public string HaveUserDescription(int LandingID, int siteID)
    {
        string UserDesc = "";


        using (MySqlConnection con = new MySqlConnection(ConnStr))
        {
            string sql = "SELECT DescriptionAdvancement FROM landingpages WHERE landingID=" + LandingID + " AND siteID=" + siteID;
            MySqlDataReader MyReader;
            con.Open();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MyReader = cmd.ExecuteReader();

            if (MyReader.Read())
            {
                UserDesc = MyReader["DescriptionAdvancement"].ToString();
            }

            MyReader.Close();
            con.Close();
        }

        return UserDesc;
    }
    public static bool isMobileBrowser()
    {
        //GETS THE CURRENT USER CONTEXT
        HttpContext context = HttpContext.Current;

        //FIRST TRY BUILT IN ASP.NT CHECK
        if (context.Request.Browser.IsMobileDevice)
        {
            return true;
        }
        //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
        if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
        {
            return true;
        }
        //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
        if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
            context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
        {
            return true;
        }
        //AND FINALLY CHECK THE HTTP_USER_AGENT 
        //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
        if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
        {
            //Create a list of all mobile types
            string[] mobiles =
                new[]
                {
                    "midp", "j2me", "avant", "docomo", 
                    "novarra", "palmos", "palmsource", 
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/", 
                    "blackberry", "mib/", "symbian", 
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio", 
                    "SIE-", "SEC-", "samsung", "HTC", 
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx", 
                    "NEC", "philips", "mmm", "xx", 
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java", 
                    "pt", "pg", "vox", "amoi", 
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo", 
                    "sgh", "gradi", "jb", "dddi", 
                    "moto", "iphone"
                };

            //Loop through each item in the list created above 
            //and check if the header contains that text
            foreach (string s in mobiles)
            {
                if (context.Request.ServerVariables["HTTP_USER_AGENT"].
                                                    ToLower().Contains(s.ToLower()))
                {
                    return true;
                }
            }
        }

        return false;
    }

}

public class pageView3
{
    string viewerIP;
    DateTime viewerTime;
    int viewdPage;
    int msgCount = 0;

    public int ViewdPage
    {
        get { return viewdPage; }
        set { viewdPage = value; }
    }

    public DateTime ViewerTime
    {
        get { return viewerTime; }
        set { viewerTime = value; }
    }

    public string ViewerIP
    {
        get { return viewerIP; }
        set { viewerIP = value; }
    }

    public int MsgCount
    {
        get { return msgCount; }
        set { msgCount = value; }
    }
}
