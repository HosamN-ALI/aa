using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Net;

public partial class Default : System.Web.UI.Page
{
    private string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;

    string viewport = "<meta name=\"viewport\" content=\"user-scalable=no, width=device-width\" runat=\"server\" id=\"viewPort1\" />" +
        "\n<link rel=\"icon\" href=\"" + Domain.getParamsByDomain(siteParamType.secureSitePath) + "/" + Domain.getParamsByDomain(siteParamType.Favicon) + "\" type=\"image/x-icon\" id=\"favicon\"/>";
    MySqlDataReader MyReader = null;
    static List<userIp> userIPs = new List<userIp>();

    int siteID;
    int msgID;
    int userID;
    int oldUserID;
    int MsgValidator;
    int userValidator;
    string sentID = "";
    //int facebookValidator;
    bool isUserValidated = false;
    bool preventValidation = false;
    string msgBody = "";//"ההודעה לא נמצאה במערכת";
    string msgSubj = "הצגת הודעה";
    string ShowPageDesc = " הכנס על מנת לצפות בהודעה '\"msgSubj\"'.";
    string sendFriendBody = "";
    string ShareFacebook = "";
    string sendFriend = "";
    string langDir = "";
    int langId = 1;
    string AddSignatureSel = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string theQS = "";
        if (Request.QueryString != null)
        {
            theQS = Request.QueryString.ToString();
        }
        if (Request.QueryString.AllKeys.Length == 0)
        {
            Response.Redirect("404.aspx");
        }

        if (!Request.IsLocal && !Request.IsSecureConnection)
        {
            Response.Redirect(Request.Url.ToString().Replace("http:", "https:").Replace("default.aspx/?", "default.aspx?"));
        }


        int TplID = 0;
        if (!string.IsNullOrEmpty(Request.QueryString["TplID"]) && int.TryParse(Request.QueryString["TplID"], out TplID))
        {
            Defaults.DelMsgFromTable(ref TplID);
            Response.End();
        }


        if (Request.QueryString["lang"] != null)
        {
            if (!int.TryParse(Request.QueryString["lang"], out langId))
            {
                langId = 2;
            }
        }

        string langCode = Defaults.GetLangCodeFromLangID(langId);

        //using (MySqlConnection con = new MySqlConnection(ConnStr))
        //{
        //    con.Open();
        //    MySqlCommand cmd = new MySqlCommand();
        //cmd.Connection = con;
        //cmd.CommandText = "INSERT INTO sendingusers.templandingusers (IP, VisitedAt,LandingORView,query) VALUES ('" + HttpContext.Current.Request.UserHostAddress + "',now(),2,'" + theQS.Replace("'", "''") + "')";
        //cmd.ExecuteNonQuery();

        //MyReader = cmd.ExecuteReader();
        //if (MyReader.Read())
        //{
        ShowPageCredentials.Text = Defaults.GetPages(60, langCode, 0);
        msgBody = Defaults.GetPages(65, langCode, 0);
        msgSubj = Defaults.GetPages(64, langCode, 0);
        ShowPageDesc = Defaults.GetPages(66, langCode, 0).Replace("msgSubj", "'msgSubj'");

        ShareFacebook = Defaults.GetPages(42, langCode, 0);
        sendFriend = Defaults.GetPages(43, langCode, 0);
        sendFriendBody = "";
        langDir = Defaults.GetPages(46, langCode, 0);
        //}



        //MyReader.Close();
        //con.Close();
        //}

        string[] parameters = { };
        int domainID = 0;
        //MsgTemplate.Text = msgBody;

        if (!string.IsNullOrEmpty(Request.QueryString["p"]))
        {
            parameters = Request.QueryString["p"].Replace("--", "-").Split('-');
            if (parameters.Length == 5)
            {
                int.TryParse(parameters[0], out msgID);
                int.TryParse(parameters[1], out MsgValidator);
                int.TryParse(parameters[2], out siteID);
                int.TryParse(parameters[3], out userID);
                int.TryParse(parameters[4], out userValidator);
            }
            else if (parameters.Length == 3)
            {
                int.TryParse(parameters[0], out msgID);
                int.TryParse(parameters[1], out MsgValidator);
                int.TryParse(parameters[2], out siteID);
            }
            else if (parameters.Length == 4)
            {
                int.TryParse(parameters[0], out msgID);
                int.TryParse(parameters[2], out MsgValidator);
                int.TryParse(parameters[3], out siteID);
            }
            else if (parameters.Length == 6)
            {
                int.TryParse(parameters[0], out msgID);
                int.TryParse(parameters[2], out MsgValidator);
                int.TryParse(parameters[3], out siteID);
                int.TryParse(parameters[4], out userID);
                int.TryParse(parameters[5], out userValidator);
            }
            else
            {
                return;
            }
            Defaults.insertStatistic(siteID, userID, msgID);
            oldUserID = userID;
            domainID = Domain.getDomainIDFromSiteID(siteID);

            if (msgID < 831324)
            {
                if (Session["msgID"] != null && Session["msgID"].ToString() == msgID.ToString())
                {
                    int userValidatorTemp;
                    int userIDTemp;
                    if (Session["userID"] != null && Session["userValidator"] != null && int.TryParse(Session["userID"].ToString(), out userIDTemp) && int.TryParse(Session["userValidator"].ToString(), out userValidatorTemp))
                    {
                        userID = userIDTemp;
                        userValidator = userValidatorTemp;
                    }

                }
                else
                {
                    Session["msgID"] = msgID.ToString();
                    Session["userID"] = userID.ToString();
                    Session["userValidator"] = userValidator.ToString();

                    if (Request.Cookies["msgID"] != null && Request.Cookies["msgID"].Value == msgID.ToString())
                    {
                        int userValidatorTemp;
                        int userIDTemp;
                        if (Request.Cookies["userID"] != null && Request.Cookies["userValidator"] != null && int.TryParse(Request.Cookies["userID"].Value, out userIDTemp) && int.TryParse(Request.Cookies["userValidator"].Value, out userValidatorTemp))
                        {
                            userID = userIDTemp;
                            userValidator = userValidatorTemp;
                        }
                    }
                    else
                    {
                        Response.Cookies["userID"].Value = userID.ToString();
                        Response.Cookies["userValidator"].Value = userValidator.ToString();
                        Response.Cookies["msgID"].Value = msgID.ToString();
                    }
                }
            }


            if (msgID == 829564)
            {
                bool found = false;
                lock (userIPs)
                {
                    for (int i = 0; i < userIPs.Count; i++)
                    {
                        if (userIPs[i].Ip == Request.UserHostAddress.ToString())
                        {
                            found = true;
                            if (userIPs[i].userIDs.Count > 100)
                            {
                                if (Request.QueryString["remIP"] == null)
                                {
                                    preventValidation = true;

                                    string err = "נסיון מעבר בין משתמשים:\n" +
                                    "\n<br />הדף בו קרתה השגיאה: " + Request.Url.ToString();
                                    if (HttpContext.Current.Session != null && HttpContext.Current.Session["siteID"] != null)
                                    {
                                        err += "\nה<br />-SiteID של המשתמש:" + HttpContext.Current.Session["siteID"].ToString();
                                    }
                                    if (Request.UserHostAddress != null)
                                    {
                                        err += "\n\nכתובת ה-IP של המשתמש הייתה:" + Request.UserHostAddress;
                                    }

                                    System.Net.Mail.MailMessage myMessage = new System.Net.Mail.MailMessage();
                                    System.Net.Mail.SmtpClient mySmtp = new System.Net.Mail.SmtpClient();
                                    //myMessage.From = new System.Net.Mail.MailAddress("omer@sendmsg.co.il", "Omer", Encoding.UTF8);
                                    myMessage.From = new System.Net.Mail.MailAddress("error@sendmsg.co.il");
                                    mySmtp.Host = "wv240.1host.co.il";
                                    mySmtp.UseDefaultCredentials = true;

                                    //mySmtp.Host = "wv240.1host.co.il";
                                    //mySmtp.UseDefaultCredentials = true;

                                    myMessage.Subject = "נסיון מעבר בין משתמשים " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                                    myMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                                    myMessage.IsBodyHtml = true;
                                    myMessage.BodyEncoding = System.Text.Encoding.UTF8;
                                    myMessage.Body = err;
                                    mySmtp.Port = 25;
                                    myMessage.To.Clear();
                                    myMessage.To.Add("omer@comstar.co.il");

                                    try
                                    {
                                        mySmtp.Send(myMessage);
                                    }
                                    catch
                                    { }

                                }
                            }
                            else
                            {
                                if (!userIPs[i].userIDs.Contains(oldUserID))
                                {
                                    userIPs[i].userIDs.Add(oldUserID);
                                }
                            }
                        }
                    }

                    if (!found)
                    {
                        userIp tempuserIP = new userIp();
                        tempuserIP.Ip = Request.UserHostAddress.ToString();
                        tempuserIP.userIDs.Add(oldUserID);
                        userIPs.Add(tempuserIP);
                    }

                    if (userValidator == 249820 && (oldUserID < 16 || oldUserID > 90153))
                    {
                        for (int i = 0; i < userIPs.Count; i++)
                        {
                            if (userIPs[i].Ip == Request.UserHostAddress.ToString())
                            {
                                for (int j = 0; j < 130; j++)
                                {
                                    userIPs[i].userIDs.Add(j);
                                }
                                if (Request.QueryString["remIP"] == null)
                                {
                                    preventValidation = true;
                                }
                            }
                        }
                    }

                    if (Request.QueryString["remIP"] != null)
                    {
                        for (int i = 0; i < userIPs.Count; i++)
                        {
                            if (userIPs[i].Ip == Request.UserHostAddress.ToString())
                            {

                                userIPs.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Response.Write("<head>" + viewport + "<meta name=\"google-site-verification\" content=\"Wr6DdV8uYvY6enM5zfAApvZvE52t7oKFt_XZunk9J2I\" /><meta name=\"google-site-verification\" content=\"GMF4Z9FZs4xRzM5gW3fOqW4Q8wEEXopPqkeJAiFctMs\" /></head>");

            string msgTemplate = "<div style=\"direction:rtl; text-align:right; font-family:Arial;\"><span style=\"color:#0269b1; font-size:22px;font-weight:bold;\">דפים מומלצים מתוך רשימת דפי הנחיתה של מערכת השיווק שלח מסר:</span><br /><br />";
            msgTemplate += "<ul style=\"vertical-align:middle;list-style-image:url('http://panel.sendmsg.co.il/images/Search.png')\">";
            using (MySqlConnection con2 = new MySqlConnection(ConnStr))
            {
                MySqlDataReader MyReader2;
                con2.Open();
                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = con2;
                cmd2.CommandText = "SELECT landingTitle,LandingFriendly, tblSites.SiteID, CONCAT('http://n.sendmsg.co.il/Minisites.aspx?p=' , landingID , '-' , ((tblSites.SiteID + 3) * landingID * 36) , '-' , tblSites.SiteID) AS LandingUrl  FROM landingPages LEFT JOIN tblSites ON tblSites.SiteID = landingPages.SiteID WHERE landingTitle<>'' and NumOpened>100 AND IndexInGoogle=1 AND (" + Defaults.activeStatusIDsQuery + ") AND langID=1 ORDER BY RAND() limit 100";
                int siteIDInt = 0;
                if (Request.QueryString["siteID"] != null && int.TryParse(Request.QueryString["siteID"], out siteIDInt))
                {
                    cmd2.CommandText = cmd2.CommandText.Replace("ORDER BY RAND() limit 100", " AND landingPages.siteID=" + siteIDInt + " ORDER BY landingID DESC").Replace("and NumOpened>100", "");
                }

                domainID = Domain.getDomainIDFromSiteID(siteIDInt);

                MyReader2 = cmd2.ExecuteReader();

                while (MyReader2.Read())
                {
                    if (MyReader2["LandingFriendly"].ToString().Trim() != "")
                    {
                        msgTemplate += ("\n<li><a href=\"http://n.sendmsg.co.il/f" + MyReader2["siteID"].ToString() + "/" + MyReader2["LandingFriendly"].ToString() + "\" style=\"color:#8a8a8a; text-decoration:none\">" + MyReader2["landingTitle"] + "</a></li>");
                    }
                    else
                    {
                        msgTemplate += ("\n<li><a href=\"" + MyReader2["LandingURL"].ToString() + "\" style=\"color:#8a8a8a; text-decoration:none\">" + MyReader2["landingTitle"] + "</a></li>");
                    }
                    msgTemplate += ("\n<br />");
                }
            }
            msgTemplate += "</ul>";
            msgTemplate += ("\n</div>");


            MsgTemplate.Text = msgTemplate;


            return;
        }

        //if (!string.IsNullOrEmpty(Request.QueryString["f"]))
        //{
        //    int.TryParse(Request.QueryString["f"], out facebookValidator);
        //}
        SendMsgNewsletter MynewsLetter;
        if (siteID != 0 && msgID != 0)
        {
            domainID = Domain.getDomainIDFromSiteID(siteID);

            using (MySqlConnection con = new MySqlConnection(ConnStr))
            {
                con.Open();//display the message
                string ogImage = "";
                string UrlPage = Request.Url.ToString().Replace("/default.aspx/?p=", "/default.aspx?p=");
                if ("/default.aspx/" == Request.Url.AbsolutePath)
                {
                    UrlPage = UrlPage.Replace("/default.aspx", "/");
                }
                string ogUrl = "<meta property=\"og:url\" id=\"ogUrl\" content=\"" + UrlPage + "\" />";
                string ogType = "<meta property=\"og:type\" content=\"website\" />";
                string ogTitle = "";
                string ogDesc = "";

                MynewsLetter = Defaults.GetNewsLetter(siteID, msgID, con);

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                //cmd.CommandText = "SELECT MsgContent,MsgSubj, siteID, MsgLangID FROM SentMsgs WHERE siteID=" + siteID + " AND MsgID=" + msgID;
                //MyReader = cmd.ExecuteReader();
                if (MynewsLetter != null && MynewsLetter.msgID != 0)
                {
                    using (MySqlConnection con2 = new MySqlConnection(ConnStr))
                    {
                        con2.Open();
                        MySqlCommand cmd2 = new MySqlCommand();
                        cmd2.Connection = con2;
                        MySqlDataReader MyReader2;
                        //if (Request.QueryString["lang"] == null)
                        //{
                        langId = MynewsLetter.MsgLangID;

                        if (MynewsLetter.SocialMsgNewsletter != null)
                        {
                            string pathImg = (Domain.getParamsByDomain(siteParamType.secureSitePath, domainID) + "/" + MynewsLetter.SocialMsgNewsletter.Images);
                            int wSize = 0;
                            int hSize = 0;

                            using (var webClient = new WebClient())
                            {
                                try
                                {
                                    byte[] imageData = webClient.DownloadData(pathImg);

                                    using (MemoryStream imgStream = new MemoryStream(imageData))
                                    {
                                        using (System.Drawing.Image img = System.Drawing.Image.FromStream(imgStream))
                                        {

                                            wSize = img.Width;
                                            hSize = img.Height;
                                        }
                                    }
                                }
                                catch { }
                                
                            }

                            string ogImage_sizes = "";
                            if (hSize > 0 && wSize >0)
                            {
                                string ogImage_sizesTemplet = "<meta property=\"{0}\" id=\"{1}\" content=\"{2}\" />";
                                string og_image_width = "og:image:width";
                                string og_image_height = "og:image:height";

                                ogImage_sizes =
                                    string.Format(ogImage_sizesTemplet, og_image_width, og_image_width.Replace(":", "_"), wSize)
                                    +
                                    string.Format(ogImage_sizesTemplet, og_image_height, og_image_height.Replace(":", "_"), hSize)
                                    ;

                            }

                            ogImage = ("<meta property=\"og:image\" runat=\"server\" id=\"PageOGImage\" content=\"" + pathImg + "\" />"+ ogImage_sizes + "<meta property=\"fb:app_id\" runat=\"server\" content=\"162086843842357\"/><meta property=\"og:type\" content=\"website\" />");

                            ogDesc = MynewsLetter.SocialMsgNewsletter.Desc;
                            ogTitle = MynewsLetter.SocialMsgNewsletter.Title;
                        }

                        //MyReader2 = cmd2.ExecuteReader();
                        //if (MyReader2.Read())
                        //{
                        ShowPageCredentials.Text = Defaults.GetPages(60, langCode, domainID);
                        msgBody = Defaults.GetPages(65, langCode, domainID);
                        msgSubj = Defaults.GetPages(64, langCode, domainID);
                        ShowPageDesc = Defaults.GetPages(66, langCode, domainID).Replace("msgSubj", "'msgSubj'");

                        ShareFacebook = Defaults.GetPages(42, langCode, domainID);
                        sendFriend = Defaults.GetPages(43, langCode, domainID);
                        sendFriendBody = "";
                        langDir = Defaults.GetPages(46, langCode, domainID);
                        //}
                        //MyReader2.Close();

                        //}

                        cmd2.CommandText = String.Format("Select tplContent,tplDir From tblnewslettertpl where siteID={0} AND tplType=3 AND isDefaults=1", siteID);

                        MyReader2 = cmd2.ExecuteReader();
                        if (MyReader2.Read())
                        {
                            AddSignatureSel = "<div style=\"margin-top:5px;";
                            switch (MyReader2["tplDir"].ToString())
                            {
                                case "2":
                                    AddSignatureSel += " text-align: right;\" dir=\"rtl\" >";
                                    break;
                                case "1":
                                    AddSignatureSel += " text-align: left;\" dir=\"ltr\" >";
                                    break;
                                default:
                                    AddSignatureSel = "";
                                    break;
                            };

                            if (AddSignatureSel != "")
                            {
                                AddSignatureSel += MyReader2["tplContent"].ToString() + "</div>";
                            }
                        }
                        MyReader2.Close();
                        con2.Close();

                    }

                    msgSubj = Server.HtmlEncode(MynewsLetter.MsgSubj);

                    string bodyTag = "<body {0}style=\"";
                    string bodyTagOld = string.Format(bodyTag, "");
                    string bodyTagNew = string.Format(bodyTag, "class=\"clean-body\" ");


                    string noIndexNoFollow = "<meta id=\"robots\" name=\"robots\" content=\"NOINDEX, NOFOLLOW\"><meta id=\"googlebot\" name=\"googlebot\" content=\"NOINDEX, NOFOLLOW\">";
                    string hadeData = "<head>" + viewport + "" + noIndexNoFollow + "<title>msgSubj</title><meta property=\"og:title\" content=\"" + (!string.IsNullOrWhiteSpace(ogTitle) ? ogTitle : "msgSubj") + "\" /><meta name=\"description\" content=\"" + ShowPageDesc + "\" /><meta property=\"og:description\" content=\"" + (!string.IsNullOrWhiteSpace(ogDesc) ? ogDesc : ShowPageDesc) + "\" />" + (!string.IsNullOrWhiteSpace(ogImage) ? ogImage : "") + ogUrl + ogType + "</head>{0}";

                    if (MynewsLetter.MsgContent.Contains(bodyTagOld))
                    {
                        msgBody = MynewsLetter.MsgContent.Replace(bodyTagOld, string.Format(hadeData + ("margin:0px auto 0px auto;max-width:800px;"), bodyTagOld));
                    }

                    if (MynewsLetter.MsgContent.Contains(bodyTagNew))
                    {
                        msgBody = MynewsLetter.MsgContent.Replace(bodyTagNew, string.Format(hadeData, bodyTagNew + ("font-family: Arial; ")));
                    }

                    if (!msgBody.Contains("<head>") && msgBody.Contains("<body"))
                    {
                        msgBody = msgBody.Insert(msgBody.IndexOf("<body"), ("<head>" + viewport + "" + noIndexNoFollow + "</head>"));
                    }

                    msgBody = msgBody.Replace("<<<#AddSignatureSel#>>>", AddSignatureSel);
                    if (langId == 1)
                    {
                        msgBody = msgBody.Replace("</title>", "</title><meta http-equiv=\"Content-Language\" content=\"he-IL\" /><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                    }

                    int MsgValidatorFromDB = 0;
                    {
                        MsgValidatorFromDB = MynewsLetter.siteID;
                        int ctrl = (MsgValidatorFromDB + 3) * msgID * 36;
                        if (Math.Abs(ctrl) != Math.Abs(MsgValidator))
                        {
                            return;
                        }

                        ctrl = (MsgValidatorFromDB + msgID) * 36;
                        //if (facebookValidator == ctrl)
                        //{
                        //    if (msgBody.Contains("id=\"showFacebook\">"))
                        //    { msgBody = msgBody.Replace("id=\"showFacebook\">", "id=\"showFacebook\" style=\"text-align:left; padding-top:10px; width:100%\"><a name=\"fb_share\" type=\"button_count\" share_url=\"" + Request.Url.ToString() + "\" href=\"http://www.facebook.com/sharer.php\">Share</a><script src=\"http://static.ak.fbcdn.net/connect.php/js/FB.Share\" type=\"text/javascript\"></script>"); }
                        //    else//older messages - where there is no "showFacebook" id...
                        //    { msgBody = msgBody.Replace("table cellpadding=\"0\" cellspacing=\"0\"; style=\"margin:auto\">", "table cellpadding=\"0\" cellspacing=\"0\"; style=\"margin:auto\"><tr><td style=\"text-align:left; padding-top:10px; width:100%\"><a name=\"fb_share\" type=\"button_count\" share_url=\"" + Request.Url.ToString() + "\" href=\"http://www.facebook.com/sharer.php\">Share</a><script src=\"http://static.ak.fbcdn.net/connect.php/js/FB.Share\" type=\"text/javascript\"></script></td></tr>"); }
                        //    FooterFaceBook.Text = "<a name=\"fb_share\" type=\"button_count\" share_url=\"" + Request.Url.ToString() + "\" href=\"http://www.facebook.com/sharer.php\">Share</a><script src=\"http://static.ak.fbcdn.net/connect.php/js/FB.Share\" type=\"text/javascript\"></script>";
                        //}

                        if (siteID != 0 && msgID != 0)
                        {
                            //adds a link to see the message in browser ("can't see this smessage properly?" is added at the top)
                            ctrl = (siteID + 3) * msgID * 36;

                            //adds a link to share this message on facebook.
                            int facebookCtrl = (siteID + msgID) * 24;
                            string img_icon_FaceBook = "http://panel.sendmsg.co.il/images/facebook_Icon.png";

                            string pageThisURL = Request.Url.Scheme + "://" + HttpUtility.UrlEncode("newsletters.sendmsg.co.il/?p=" + msgID + "-" + ctrl.ToString() + "-" + siteID.ToString() + "-" + userID + "-" + (userValidator) + "&lang=" + langId);
                            string facebook_url = "http://www.facebook.com/sharer.php?u=" + pageThisURL;


                            msgBody = msgBody.Replace("id=\"showFacebook\">", "style=\"margin-left:10px; text-align:right; margin-right:10px; display:inline-block; text-align:right\"><a style=\"color:#919191;  text-decoration:none; font-weight:normal;font-size:14px; font-family:Arial; display:block; width:63; height:16px; text-align:right; vertical-align:middle; line-height:17px;  \" href=\"" + facebook_url + "\"><img src=\"" + img_icon_FaceBook + "\" alt=\"\" style=\"vertical-align:middle; border:none; margin-left:3px;\" />" + ShareFacebook + "</a> ");
                            msgBody = msgBody.Replace("id=\"FacebookShare\"", "href=\"" + facebook_url + "\"");

                            //NEW structor showFacebook
                            msgBody = msgBody.Replace("${facebook_img_icon}", img_icon_FaceBook).Replace("${facebook_text}", ShareFacebook).Replace("${facebook_url}", facebook_url);


                            string forward_image = "http://panel.sendmsg.co.il/images/" + "forward_Icon.png";
                            string forward_url = HttpUtility.HtmlDecode("mailto:?subject=FW:" + msgSubj + "&body=" + sendFriendBody + " " + pageThisURL);

                            msgBody = msgBody.Replace("id=\"showForward\">", "style=\"display:inline-block;text-align:right;margin-left:10px\"><a style=\"color:#919191; text-decoration:none; font-size:14px; font-family:Arial; display:inline-block; height:14px; vertical-align:middle; line-height:17px;  min-width:100px; margin-right:10px;\" href=\"" + forward_url + " \"><img src=\"" + forward_image + "\" slt=\"\" style=\"vertical-align:middle; border:none; margin-left:3px;\" />" + sendFriend + "</a>");

                            //NEW structor showForward
                            msgBody = msgBody.Replace("${forward_image}", forward_image).Replace("${forward_text}", sendFriend).Replace("${forward_url}", forward_url);

                            msgBody = msgBody.Replace("class=\"sendFriend\"", "href=\"" + HttpUtility.HtmlDecode("mailto:?subject=FW:" + Server.HtmlEncode(msgSubj.Replace("\"", "''").Replace("&quot;", "''")) + "&body=" + sendFriendBody + " " + HttpUtility.UrlEncode("http://newsletters.sendmsg.co.il/?p=" + msgID.ToString() + "-" + ctrl.ToString() + "-" + siteID.ToString() + "-" + userID + "-" + (ctrl - msgID) + "&lang=" + langId)).Replace(" ", "%20") + "\"");
                            msgBody = msgBody.Replace("showPage.aspx?", "showPage.aspx?uid=" + userID + "&ctrl=" + ctrl + "&");


                            //NEW structor showMessage
                            msgBody = msgBody.Replace("${showMessage_element}", "");

                            //NEW structor showRanks
                            msgBody = msgBody.Replace("${ranks_html}", "").Replace("${ranking_text}", "");
                        }
                    }

                    if (MynewsLetter.RemoveCredits)
                    {
                        ShowPageCredentials.Visible = false;
                    }
                }
                else
                {
                    return;
                }

                //MyReader.Close();

                int startAt = 0;
                int endAt = 2;






                #region checking if there is ChangingDate field (Saved Tag) in the template, which has to be replaced with changingData Field (Saved Tag)
                msgBody = msgBody.Replace("[[[]]]", "");
                startAt = 0;
                endAt = 2;
                List<ChangingData> allChangingData = new List<ChangingData>();
                while (startAt != -1 && endAt != -1 && msgBody.Length > 4)//looking for all the saved tags (from tblChangingData) names in the message
                {
                    string cdTitle = "";
                    string cleancdTitle = "";
                    startAt = msgBody.IndexOf("[[[", startAt);
                    endAt = msgBody.IndexOf("]]]", startAt + 4);

                    if (startAt != -1 && endAt != -1)
                    {
                        startAt++;
                        cdTitle = msgBody.Substring((startAt + 2), (endAt - (startAt + 2)));
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

                        string sql2 = "SELECT cdContent,CDID FROM tblChangingData WHERE cdTitle='" + cleancdTitle + "' AND SiteID=" + siteID + " AND parentID=0";
                        MySqlDataReader MyReader2;
                        MySqlDataReader MyReader3;
                        using (MySqlConnection con2 = new MySqlConnection(ConnStr))
                        {
                            con2.Open();

                            MySqlCommand cmd2 = new MySqlCommand(sql2, con2);

                            MyReader2 = cmd2.ExecuteReader();

                            if (MyReader2.Read())
                            {
                                ChangingData tempChangingData = new ChangingData();
                                tempChangingData.CdTitle = cdTitle;
                                tempChangingData.CdContent = MyReader2["cdContent"].ToString();
                                int cdId = 0;
                                if (int.TryParse(MyReader2["cdid"].ToString(), out cdId))
                                {
                                    tempChangingData.CdId = cdId;
                                    using (MySqlConnection con3 = new MySqlConnection(ConnStr))
                                    {
                                        con3.Open();
                                        MySqlCommand cmd3 = new MySqlCommand();
                                        cmd3.Connection = con3;

                                        cmd3.CommandText = "SELECT cdContent,CDID,dynamicList FROM tblChangingData WHERE cdTitle='" + cleancdTitle + "' AND SiteID=" + siteID + " AND parentID=" + cdId;
                                        MyReader3 = cmd3.ExecuteReader();
                                        while (MyReader3.Read())
                                        {
                                            SubChangingData tempSubChangingData = new SubChangingData();
                                            tempSubChangingData.CdContent = MyReader3["cdContent"].ToString();
                                            int subCdId = 0;
                                            int dynamicList = 0;

                                            if (int.TryParse(MyReader3["cdid"].ToString(), out subCdId))
                                            {
                                                tempSubChangingData.CdId = subCdId;
                                                if (int.TryParse(MyReader3["dynamicList"].ToString(), out dynamicList))
                                                {
                                                    tempSubChangingData.DynamicList = dynamicList;
                                                    string[] tempUserIDs = Defaults.getUsersByML(dynamicList, siteID, "ID=" + userID);

                                                    foreach (string tempUserIDstr in tempUserIDs)
                                                    {
                                                        int tempUserID = 0;

                                                        if (int.TryParse(tempUserIDstr, out tempUserID) && tempUserID == userID && !tempSubChangingData.UserIDs.Contains(tempUserID))
                                                        {
                                                            tempSubChangingData.UserIDs.Add(tempUserID);
                                                        }
                                                    }
                                                    tempChangingData.SubChangingData.Add(tempSubChangingData);
                                                }
                                            }
                                        }
                                        MyReader3.Close();


                                        con3.Close();
                                    }
                                }

                                allChangingData.Add(tempChangingData);
                            }
                            MyReader2.Close();
                            con2.Close();
                        }
                    }
                }

                int testGroupID = 0;
                if (Request.QueryString["testGroupID"] != null && int.TryParse(Request.QueryString["testGroupID"], out testGroupID))
                {
                    foreach (ChangingData tempChangingData in allChangingData)
                    {
                        bool isDefaultTempDate = true;

                        foreach (SubChangingData tempSubChangingData in tempChangingData.SubChangingData)
                        {
                            if (tempSubChangingData.DynamicList == testGroupID)//checks if this is a test message for this subdata (if the dropdown was selected).
                            {
                                isDefaultTempDate = false;
                                msgBody = msgBody.Replace("[[[" + tempChangingData.CdTitle + "]]]", tempSubChangingData.CdContent);
                            }
                        }
                        if (isDefaultTempDate)
                        {
                            msgBody = msgBody.Replace("[[[" + tempChangingData.CdTitle + "]]]", tempChangingData.CdContent);
                        }
                    }

                }
                else
                {
                    foreach (ChangingData tempChangingData in allChangingData)
                    {
                        bool isDefaultTempDate = true;

                        if (userID != 0)
                        {

                            foreach (SubChangingData tempSubChangingData in tempChangingData.SubChangingData)
                            {
                                if (tempSubChangingData.UserIDs.Contains(userID))
                                {
                                    isDefaultTempDate = false;
                                    msgBody = msgBody.Replace("[[[" + tempChangingData.CdTitle + "]]]", tempSubChangingData.CdContent);
                                }
                            }
                        }

                        if (isDefaultTempDate)
                        {
                            msgBody = msgBody.Replace("[[[" + tempChangingData.CdTitle + "]]]", tempChangingData.CdContent);
                        }
                    }
                }
                #endregion






                if (userID != 0)
                {
                    #region counting the display of the message
                    string sql = "SELECT SentOpenIP, date, sentID, NumOpened,userID FROM `sendingUsers`.`sendingUsers" + siteID.ToString() + "` LEFT JOIN tblUsers" + siteID.ToString() + " on tblUsers" + siteID.ToString() + ".id=`sendingUsers`.`sendingUsers" + siteID.ToString() + "`.userID Where MsgID=" + msgID + " AND userID=" + userID;

                    cmd.CommandText = sql;
                    try
                    {
                        MyReader = cmd.ExecuteReader();
                    }
                    catch
                    {
                        return;
                    }
                    if (MyReader.Read())
                    {
                        sentID = MyReader["sentID"].ToString();
                        //checks if the validation of the user is ok - if not, show the message without personal info
                        DateTime ctrlDT = new DateTime();
                        if (DateTime.TryParse(MyReader["date"].ToString(), out ctrlDT))
                        {
                            int ctrl = ((ctrlDT.Day + ctrlDT.Month) * ctrlDT.Year * 36) - msgID;
                            int ctrl2 = ((ctrlDT.Day + ctrlDT.Month + userID) * ctrlDT.Year * 36) - msgID;
                            if (Math.Abs(ctrl) != Math.Abs(userValidator) && Math.Abs(ctrl2) != Math.Abs(userValidator))
                            {
                                isUserValidated = false;
                                msgBody = msgBody.Replace("[[email]]", "").Replace("[[Email]]", "").Replace("[[Email]]", "").Replace("[[]]", "");
                                startAt = 0;
                                endAt = 2;
                                while (startAt != -1 && endAt != -1 && msgBody.Length > 4)//looking for all the field names in the message
                                {
                                    string FieldAlias = "";
                                    string cleanFieldAlias = "";
                                    startAt = msgBody.IndexOf("[[", startAt);
                                    endAt = msgBody.IndexOf("]]", startAt + 3);

                                    if (startAt != -1 && endAt != -1)
                                    {
                                        startAt++;
                                        FieldAlias = msgBody.Substring((startAt + 1), (endAt - (startAt + 1)));
                                        //if the user has html code inside the FieldAlias field, it is being destroyed.
                                        if (FieldAlias.Contains("<"))
                                        {
                                            string removeWhat = "";
                                            int InnerstartAt = 0;
                                            int InnerendAt = 0;

                                            InnerstartAt = FieldAlias.IndexOf("<");
                                            InnerendAt = FieldAlias.LastIndexOf(">");
                                            removeWhat = FieldAlias.Substring(InnerstartAt, (InnerendAt - InnerstartAt) + 1);
                                            if (removeWhat.Length > 0)
                                                cleanFieldAlias = FieldAlias.Replace(removeWhat, "");
                                        }

                                        msgBody = msgBody.Replace("[[" + FieldAlias + "]]", "");
                                    }
                                }


                            }
                            else
                            {
                                msgBody = msgBody.Replace("class=\"removemefrom\"", "href=\"http://panel.sendmsg.co.il/DelFromNewsLetter.aspx?site=" + siteID + "&id=" + userID + "&ctrl=" + (ctrl + msgID).ToString() + "&msgID=" + msgID + "&lang=" + langId + "\" target=\"_blank\"");
                                if (!preventValidation)
                                {
                                    isUserValidated = true;
                                }
                            }
                        }


                        string currentIP = HttpContext.Current.Request.UserHostAddress;
                        if (MyReader["SentOpenIP"].ToString() != "" && MyReader["SentOpenIP"].ToString() != currentIP)
                        {
                            sql = "UPDATE SentMsgs SET NumFwd=(NumFwd+1) Where MsgID=" + msgID;
                            MySqlCommand cmd2 = new MySqlCommand(sql, new MySqlConnection(ConnStr));
                            cmd2.Connection.Open();
                            cmd2.ExecuteNonQuery();

                            cmd2.CommandText = "UPDATE `sendingUsers`.`sendingUsers" + siteID.ToString() + "` SET NumFwd=(NumFwd+1) Where sentID=" + sentID;
                            cmd2.ExecuteNonQuery();
                            cmd2.Connection.Close();
                        }
                        else
                        {
                            sql = "UPDATE `sendingUsers`.`sendingUsers" + siteID.ToString() + "` SET NumOpened=(NumOpened+1), SentOpenIP ='" + currentIP + "', openTime =now() Where sentID=" + sentID;
                            MySqlCommand cmd2 = new MySqlCommand(sql, new MySqlConnection(ConnStr));
                            cmd2.Connection.Open();
                            cmd2.ExecuteNonQuery();

                            if (MyReader["NumOpened"].ToString() == "" || MyReader["NumOpened"].ToString() == "0")
                            {
                                cmd2.CommandText = "UPDATE SentMsgs SET NumOpened=(NumOpened+1) Where MsgID=" + msgID;
                                cmd2.ExecuteNonQuery();
                            }
                            cmd2.Connection.Close();
                        }
                    }
                    else
                    {
                        userID = 0;
                    }
                    MyReader.Close();
                    #endregion



                    if (sentID != "")
                    {


                        #region checking if there is a field in the template, which has to be replaced with the user's informatiom
                        cmd.CommandText = "SELECT tblUsers" + siteID + ".*,`sendingUsers`.`sendingUsers" + siteID.ToString() + "`.sendFields,`sendingUsers`.`sendingUsers" + siteID.ToString() + "`.sentId as sentID FROM (`sendingUsers`.`sendingUsers" + siteID.ToString() + "` LEFT JOIN tblUsers" + siteID + " ON `sendingUsers`.`sendingUsers" + siteID.ToString() + "`.userID = tblUsers" + siteID + ".id) WHERE `sendingUsers`.`sendingUsers" + siteID.ToString() + "`.sentID=" + sentID;
                        MyReader = cmd.ExecuteReader();
                        Dictionary<string, string> sendFieldsDict = new Dictionary<string, string>();


                        if (MyReader.Read())
                        {
                            if (MyReader["sendFields"].ToString() != "")
                            {
                                string[] sendFieldsSplitter = { "|||" };
                                string[] keyValueArr = MyReader["sendFields"].ToString().Split(sendFieldsSplitter, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string keyValue in keyValueArr)
                                {
                                    if (keyValue.Contains("~|~"))
                                    {
                                        if (!sendFieldsDict.ContainsKey(keyValue.Remove(keyValue.IndexOf("~|~"))))
                                        {
                                            sendFieldsDict.Add(keyValue.Remove(keyValue.IndexOf("~|~")), keyValue.Remove(0, (keyValue.IndexOf("~|~") + 3)));
                                        }
                                    }
                                }
                            }

                            #region checking if there is an API UserSendField in the template, which has to be replaced with changingData Field (Saved Tag)
                            msgBody = msgBody.Replace("[|[]|]", "");
                            startAt = 0;
                            endAt = 2;
                            while (startAt != -1 && endAt != -1 && msgBody.Length > 4)//looking for all the UserSendField in the message
                            {
                                string fieldTitle = "";
                                string cleanfieldTitle = "";
                                startAt = msgBody.IndexOf("[|[", startAt);
                                endAt = msgBody.IndexOf("]|]", startAt + 4);

                                if (startAt != -1 && endAt != -1)
                                {
                                    startAt++;
                                    fieldTitle = msgBody.Substring((startAt + 2), (endAt - (startAt + 2)));
                                    cleanfieldTitle = fieldTitle;
                                    //if the user has html code inside the FieldAlias field, it is being destroyed.
                                    if (cleanfieldTitle.Contains("<") && cleanfieldTitle.Contains(">"))
                                    {
                                        string HtmlTag = @"<(.|\n)*?>";
                                        cleanfieldTitle = System.Text.RegularExpressions.Regex.Replace(cleanfieldTitle, HtmlTag, "");

                                    }


                                    if (sendFieldsDict.ContainsKey(cleanfieldTitle))
                                    {
                                        msgBody = msgBody.Replace("[|[" + fieldTitle + "]|]", sendFieldsDict[cleanfieldTitle]);
                                        msgSubj = msgSubj.Replace("[|[" + fieldTitle + "]|]", sendFieldsDict[cleanfieldTitle]);
                                    }
                                    else
                                    {
                                        msgBody = msgBody.Replace("[|[" + fieldTitle + "]|]", "");
                                        msgSubj = msgSubj.Replace("[|[" + fieldTitle + "]|]", "");
                                    }
                                }
                            }
                            #endregion





                            msgBody = msgBody.Replace("[[email]]", MyReader["eMail"].ToString()).Replace("[[Email]]", MyReader["eMail"].ToString());
                            startAt = 0;
                            endAt = 2;
                            msgBody = msgBody.Replace("[[]]", "");

                            while (startAt != -1 && endAt != -1 && msgBody.Length > 4)//looking for all the field names in the message
                            {
                                string FieldAlias = "";
                                string cleanFieldAlias = "";
                                startAt = msgBody.IndexOf("[[", startAt);
                                endAt = msgBody.IndexOf("]]", startAt + 3);

                                if (startAt != -1 && endAt != -1)
                                {
                                    startAt++;
                                    FieldAlias = msgBody.Substring((startAt + 1), (endAt - (startAt + 1)));
                                    //if the user has html code inside the FieldAlias field, it is being destroyed.
                                    if (FieldAlias.Contains("<"))
                                    {
                                        string removeWhat = "";
                                        int InnerstartAt = 0;
                                        int InnerendAt = 0;

                                        InnerstartAt = FieldAlias.IndexOf("<");
                                        InnerendAt = FieldAlias.LastIndexOf(">");
                                        removeWhat = FieldAlias.Substring(InnerstartAt, (InnerendAt - InnerstartAt) + 1);
                                        if (removeWhat.Length > 0)
                                            cleanFieldAlias = FieldAlias.Replace(removeWhat, "");
                                    }
                                    else
                                    {
                                        cleanFieldAlias = FieldAlias;
                                    }

                                    string sql2 = "SELECT FieldNum, fieldType FROM tblAliasForUsers WHERE FieldAlias='" + cleanFieldAlias.Replace("'", "''") + "' AND SiteID=" + siteID;
                                    MySqlDataReader MyReader2;
                                    using (MySqlConnection con2 = new MySqlConnection(ConnStr))
                                    {
                                        con2.Open();
                                        cmd = new MySqlCommand(sql2, con2);

                                        MyReader2 = cmd.ExecuteReader();

                                        if (MyReader2.Read())
                                        {
                                            switch (MyReader2["fieldType"].ToString())
                                            {
                                                case "1":
                                                case "2":
                                                case "3":
                                                case "7":
                                                case "8":
                                                    msgBody = msgBody.Replace("[[" + FieldAlias + "]]", MyReader[MyReader2["FieldNum"].ToString()].ToString());
                                                    break;
                                                case "6":
                                                    string dateField = MyReader[MyReader2["FieldNum"].ToString()].ToString();
                                                    if (dateField.Contains(" "))
                                                    {
                                                        dateField = dateField.Remove(dateField.IndexOf(" "));
                                                    }
                                                    msgBody = msgBody.Replace("[[" + FieldAlias + "]]", dateField);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            msgBody = msgBody.Replace("[[" + FieldAlias + "]]", "");
                                        }
                                        MyReader2.Close();
                                        con2.Close();
                                    }
                                }
                            }
                        }
                        //if the user was not found, clears the personal fields
                        else
                        {
                            msgBody = msgBody.Replace("[[email]]", "").Replace("[[Email]]", "").Replace("[[]]", "");
                            startAt = 0;
                            endAt = 2;
                            while (startAt != -1 && endAt != -1 && msgBody.Length > 4)//looking for all the field names in the message
                            {
                                string FieldAlias = "";
                                string cleanFieldAlias = "";
                                startAt = msgBody.IndexOf("[[", startAt);
                                endAt = msgBody.IndexOf("]]", startAt + 3);

                                if (startAt != -1 && endAt != -1)
                                {
                                    startAt++;
                                    FieldAlias = msgBody.Substring((startAt + 1), (endAt - (startAt + 1)));
                                    //if the user has html code inside the FieldAlias field, it is being destroyed.
                                    if (FieldAlias.Contains("<"))
                                    {
                                        string removeWhat = "";
                                        int InnerstartAt = 0;
                                        int InnerendAt = 0;

                                        InnerstartAt = FieldAlias.IndexOf("<");
                                        InnerendAt = FieldAlias.LastIndexOf(">");
                                        removeWhat = FieldAlias.Substring(InnerstartAt, (InnerendAt - InnerstartAt) + 1);
                                        if (removeWhat.Length > 0)
                                            cleanFieldAlias = FieldAlias.Replace(removeWhat, "");
                                    }

                                    msgBody = msgBody.Replace("[[" + FieldAlias + "]]", "");
                                }
                            }
                        }
                        MyReader.Close();
                        #endregion


                        #region checking if there is a field in the *Subject*, which has to be replaced with the user's informatiom
                        cmd.Connection = con;
                        cmd.CommandText = "SELECT tblUsers" + siteID + ".*,`sendingUsers`.`sendingUsers" + siteID.ToString() + "`.sentId as sentID FROM (`sendingUsers`.`sendingUsers" + siteID.ToString() + "` LEFT JOIN tblUsers" + siteID + " ON `sendingUsers`.`sendingUsers" + siteID.ToString() + "`.userID = tblUsers" + siteID + ".id) WHERE  sentID=" + sentID;
                        MyReader = cmd.ExecuteReader();

                        if (MyReader.Read() && isUserValidated)
                        {
                            msgSubj = msgSubj.Replace("[[email]]", MyReader["eMail"].ToString()).Replace("[[Email]]", MyReader["eMail"].ToString()).Replace("[[]]", "");
                            startAt = 0;
                            endAt = 2;
                            while (startAt != -1 && endAt != -1 && msgSubj.Length > 4)//looking for all the field names in the message
                            {
                                string FieldAlias = "";
                                string cleanFieldAlias = "";
                                if (msgSubj.Length > startAt)
                                {
                                    startAt = msgSubj.IndexOf("[[", startAt);
                                    if (startAt != -1)
                                        endAt = msgSubj.IndexOf("]]", startAt + 3);

                                    if (startAt != -1 && endAt != -1)
                                    {
                                        startAt++;
                                        FieldAlias = msgSubj.Substring((startAt + 1), (endAt - (startAt + 1)));
                                        //if the user has html code inside the FieldAlias field, it is being destroyed.
                                        if (FieldAlias.Contains("<"))
                                        {
                                            string removeWhat = "";
                                            int InnerstartAt = 0;
                                            int InnerendAt = 0;

                                            InnerstartAt = FieldAlias.IndexOf("<");
                                            InnerendAt = FieldAlias.LastIndexOf(">");
                                            removeWhat = FieldAlias.Substring(InnerstartAt, (InnerendAt - InnerstartAt) + 1);
                                            cleanFieldAlias = FieldAlias.Replace(removeWhat, "");
                                        }
                                        else
                                        {
                                            cleanFieldAlias = FieldAlias;
                                        }

                                        string sql2 = "SELECT FieldNum FROM tblAliasForUsers WHERE FieldAlias='" + cleanFieldAlias + "' AND SiteID=" + siteID;
                                        MySqlDataReader MyReader2;
                                        using (MySqlConnection con2 = new MySqlConnection(ConnStr))
                                        {

                                            con2.Open();
                                            cmd = new MySqlCommand(sql2, con2);
                                            MyReader2 = cmd.ExecuteReader();
                                            if (MyReader2.Read())
                                            {
                                                msgSubj = msgSubj.Replace("[[" + FieldAlias + "]]", MyReader[MyReader2["FieldNum"].ToString()].ToString());
                                            }
                                            else
                                            {
                                                msgSubj = msgSubj.Replace("[[" + FieldAlias + "]]", "");
                                            }
                                            MyReader2.Close();
                                            con2.Close();
                                        }
                                    }
                                }
                                else
                                    startAt = -1;
                            }
                        }
                        else
                        {
                            msgSubj = msgSubj.Replace("[[email]]", "").Replace("[[Email]]", "").Replace("[[]]", "");
                            startAt = 0;
                            endAt = 2;
                            while (startAt != -1 && endAt != -1 && msgSubj.Length > 4)//looking for all the field names in the message
                            {
                                string FieldAlias = "";
                                string cleanFieldAlias = "";
                                if (msgSubj.Length > startAt)
                                {
                                    startAt = msgSubj.IndexOf("[[", startAt);
                                    if (startAt != -1)
                                        endAt = msgSubj.IndexOf("]]", startAt + 3);

                                    if (startAt != -1 && endAt != -1)
                                    {
                                        startAt++;
                                        FieldAlias = msgSubj.Substring((startAt + 1), (endAt - (startAt + 1)));
                                        //if the user has html code inside the FieldAlias field, it is being destroyed.
                                        if (FieldAlias.Contains("<"))
                                        {
                                            string removeWhat = "";
                                            int InnerstartAt = 0;
                                            int InnerendAt = 0;

                                            InnerstartAt = FieldAlias.IndexOf("<");
                                            InnerendAt = FieldAlias.LastIndexOf(">");
                                            removeWhat = FieldAlias.Substring(InnerstartAt, (InnerendAt - InnerstartAt) + 1);
                                            cleanFieldAlias = FieldAlias.Replace(removeWhat, "");
                                        }
                                        msgSubj = msgSubj.Replace("[[" + FieldAlias + "]]", "");
                                    }
                                }
                                else
                                    startAt = -1;
                            }
                        }
                        #endregion
                    }


                }
                // if no userID was given, clears the personal fields

                if (userID == 0)
                {
                    msgBody = msgBody.Replace("[[email]]", "").Replace("[[Email]]", "").Replace("[[]]", "");
                    startAt = 0;
                    endAt = 2;
                    while (startAt != -1 && endAt != -1 && msgBody.Length > 4)//looking for all the field names in the message
                    {
                        string FieldAlias = "";
                        string cleanFieldAlias = "";
                        startAt = msgBody.IndexOf("[[", startAt);
                        endAt = msgBody.IndexOf("]]", startAt + 3);

                        if (startAt != -1 && endAt != -1)
                        {
                            startAt++;
                            FieldAlias = msgBody.Substring((startAt + 1), (endAt - (startAt + 1)));
                            //if the user has html code inside the FieldAlias field, it is being destroyed.
                            if (FieldAlias.Contains("<"))
                            {
                                string removeWhat = "";
                                int InnerstartAt = 0;
                                int InnerendAt = 0;

                                InnerstartAt = FieldAlias.IndexOf("<");
                                InnerendAt = FieldAlias.LastIndexOf(">");
                                removeWhat = FieldAlias.Substring(InnerstartAt, (InnerendAt - InnerstartAt) + 1);
                                if (removeWhat.Length > 0)
                                    cleanFieldAlias = FieldAlias.Replace(removeWhat, "");
                            }

                            msgBody = msgBody.Replace("[[" + FieldAlias + "]]", "");
                        }
                    }

                    msgSubj = msgSubj.Replace("[[email]]", "").Replace("[[Email]]", "").Replace("[[]]", "");
                    startAt = 0;
                    endAt = 2;
                    while (startAt != -1 && endAt != -1 && msgSubj.Length > 4)//looking for all the field names in the message
                    {
                        string FieldAlias = "";
                        string cleanFieldAlias = "";
                        if (msgSubj.Length > startAt)
                        {
                            startAt = msgSubj.IndexOf("[[", startAt);
                            if (startAt != -1)
                                endAt = msgSubj.IndexOf("]]", startAt + 3);

                            if (startAt != -1 && endAt != -1)
                            {
                                startAt++;
                                FieldAlias = msgSubj.Substring((startAt + 1), (endAt - (startAt + 1)));
                                //if the user has html code inside the FieldAlias field, it is being destroyed.
                                if (FieldAlias.Contains("<"))
                                {
                                    string removeWhat = "";
                                    int InnerstartAt = 0;
                                    int InnerendAt = 0;

                                    InnerstartAt = FieldAlias.IndexOf("<");
                                    InnerendAt = FieldAlias.LastIndexOf(">");
                                    removeWhat = FieldAlias.Substring(InnerstartAt, (InnerendAt - InnerstartAt) + 1);
                                    cleanFieldAlias = FieldAlias.Replace(removeWhat, "");
                                }
                                msgSubj = msgSubj.Replace("[[" + FieldAlias + "]]", "");
                            }
                        }
                        else
                            startAt = -1;
                    }

                }

                con.Close();
            }
        }
        msgBody = msgBody.Replace("=\"/userfiles", "=\"http://panel.sendmsg.co.il/userfiles").Replace("msgSubj", msgSubj);
        if (Request.QueryString["print"] != null)
        {
            //msgBody = "<a href=\"mailto:?subject=" + Server.UrlEncode(Server.HtmlEncode(msgSubj)) + "&body=" + Server.UrlEncode(Server.HtmlEncode(msgBody)) + "\"  id=\"sendFriend\"> send</a>";
            string newMsgBody = "mailto:?subject=" + Server.HtmlEncode(Server.UrlEncode(msgSubj)) + "&body=" + Server.HtmlEncode(Server.UrlEncode(msgBody).Remove(500));

            ClientScript.RegisterStartupScript(GetType(), "sendFriend_CLick", "window.open(\"" + newMsgBody + "\", '_blank');", true);
        }
        msgBody = msgBody.Replace("id=\"AccessibilityPlug\"", "id=\"AccessibilityPlug\" style=\"display:none;\"");


        ///https://app.clickup.com/t/6xtw18
        var showPageNoSecure = (Domain.getParamsByDomain(siteParamType.sitePath) + "/showPage.aspx?");
        var showPageSecure = (Domain.getParamsByDomain(siteParamType.secureSitePath) + "/showPage.aspx?");
        msgBody = msgBody.Replace(showPageNoSecure, showPageSecure);


        Response.Write(msgBody);
        if (langCode.ToLower() == "heb" || langId == 1)
        {
            accessPlug.Text = "<script type=\"text/javascript\" src=\"https://app.sendmsg.co.il/getAccessPlug.ashx\"></script>";
            //accessPlug.Text = "<script type=\"text/javascript\" src=\"http://localhost:63695/getAccessPlug.ashx\"></script>";
            if (Request.QueryString["AccessPlug"] == "1")
            {
                accessPlug.Text += "<script type=\"text/javascript\">window.onload = setTimeout(function(){ $('.AccessMainHeader').click(); }, 1100);</script>";
            }

        }
    }

}

class ChangingData
{
    int cdId = 0;
    string cdContent = "";
    string cdTitle = "";
    List<SubChangingData> subChangingData = new List<SubChangingData>();

    public string CdTitle
    {
        get { return cdTitle; }
        set { cdTitle = value; }
    }

    public int CdId
    {
        get { return cdId; }
        set { cdId = value; }
    }
    public string CdContent
    {
        get { return cdContent; }
        set { cdContent = value; }
    }
    public List<SubChangingData> SubChangingData
    {
        get { return subChangingData; }
        set { subChangingData = value; }
    }
}

class SubChangingData
{
    int cdId = 0;
    List<int> userIDs = new List<int>();
    string cdContent = "";
    int dynamicList = 0;

    public int DynamicList
    {
        get { return dynamicList; }
        set { dynamicList = value; }
    }

    public int CdId
    {
        get { return cdId; }
        set { cdId = value; }
    }
    public List<int> UserIDs
    {
        get { return userIDs; }
        set { userIDs = value; }
    }
    public string CdContent
    {
        get { return cdContent; }
        set { cdContent = value; }
    }
}

class userIp
{
    public string Ip = "";
    public List<int> userIDs = new List<int>();
}