using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using MySql.Data.MySqlClient;
using System.Web.SessionState;
using System.Collections;
using System.Web.Compilation;
using System.CodeDom;
using System.Security.Permissions;



public class Languages2 : ExpressionBuilder
{

    public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
    {
        CodeTypeReferenceExpression thisType = new CodeTypeReferenceExpression(base.GetType());


        CodePrimitiveExpression expression = new CodePrimitiveExpression(entry.Expression.Trim().ToString());

        string evaluationMethod = "MyText";

        return new CodeMethodInvokeExpression(thisType, evaluationMethod, new CodeExpression[] { expression });
    }

    public static List<SendMagLangText> getTour(string lang, string pageName, int SiteID,int SecLevel, out bool autoTour, out bool HaveTour)
    {
        bool tempAutoTour = false;
        HaveTour = false;
        pageName = pageName.ToLower().Replace(".aspx", "").Replace("2", "");
        List<SendMagLangText> ListSendMagLangText = new List<SendMagLangText>();
        var tour = sendMagLangs.FirstOrDefault(f => f.LangName == lang).sendMsgPages.FirstOrDefault(g => g.PageName.ToLower() == pageName);

        try
        {
            if (tour != null && tour.sendMsgTexts != null && tour.sendMsgTexts.Count > 0)
            {
                ListSendMagLangText = tour.sendMsgTexts.Select(gg => gg).Where(e => e.TourContent != "" && e.TourOrder != 0).ToList();

                if ((ListSendMagLangText.Count > 0) && ((SecLevel == 1 || SecLevel == 14) || ListSendMagLangText.Count(d => d.TourStatus == true) > 0))
                {
                    using (MySqlConnection conn = new MySqlConnection(Defaults.ConnStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT IGNORE INTO tours (SiteID) VALUES (" + SiteID + ");";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT * FROM tours WHERE SiteID=" + SiteID + " LIMIT 1";
                        MySqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read() && (!(dr.HasColumn(pageName))))
                        {
                            dr.Close();
                            cmd.CommandText = "ALTER TABLE `sendermsg`.`tours` ADD COLUMN `" + pageName + "` TINYINT(1) UNSIGNED NOT NULL DEFAULT 0  AFTER `siteID`;";
                            cmd.ExecuteNonQuery();

                        }
                        if (!dr.IsClosed)
                        {
                            dr.Close();
                        }

                        cmd.CommandText = "SELECT  `" + pageName + "` FROM `sendermsg`.`tours` WHERE `siteID` = " + SiteID + ";";
                        dr = cmd.ExecuteReader();
                        if (dr.Read() && dr[pageName].ToString() == "0")
                        {
                            tempAutoTour = true;
                        }
                        dr.Close();

                        if (tempAutoTour)
                        {
                            cmd.CommandText = "UPDATE `sendermsg`.`tours` SET `" + pageName + "` = 1 WHERE `siteID` = " + SiteID + ";";
                            cmd.ExecuteNonQuery();
                        }


                        conn.Close();
                    }
                    HaveTour = true;
                }
                else
                {
                    ListSendMagLangText.Clear();
                }
            }
        }
        catch { }
        autoTour = tempAutoTour;
        return ListSendMagLangText;

    }

    static public List<string> LangNames = new List<string>();
    static public List<string> PageNames = new List<string>();
    static public List<List<Dictionary<string, string>>> TextPages = new List<List<Dictionary<string, string>>>();
    static public List<SendMsgLang> sendMagLangs = new List<SendMsgLang>();

    public static string oldMyText(string textName)
    {
        string lang = "Heb";
        string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
        string TheText = "";
        string PageName = "general";
        string HebText = "";
        int langPos = 0;
        int pagePos = 0;
        System.Web.HttpContext currentContext = System.Web.HttpContext.Current;
        HttpSessionState CurSession = currentContext.Session;

        if (textName.Contains("_"))
        {


            PageName = textName.Remove(textName.IndexOf("_"));
            textName = textName.Substring(textName.IndexOf("_") + 1);
        }

        if (textName.Contains("_"))
        {
            HebText = textName.Substring(textName.IndexOf("_") + 1);
            textName = textName.Remove(textName.IndexOf("_"));
        }



        if (CurSession != null && CurSession["Lang"] != null)
        {
            lang = CurSession["Lang"].ToString();
        }

        if (LangNames.Contains(lang))//if the language is in memory, find it's position.
        {
            langPos = LangNames.IndexOf(lang);
        }
        else//if the language is not in memory, find adds it.
        {
            LangNames.Add(lang);
            langPos = LangNames.Count - 1;
        }


        if (PageNames.Contains(PageName) && TextPages.Count > pagePos)//if the page is in memory, find it's position.
        {
            pagePos = PageNames.IndexOf(PageName);
            if (TextPages[pagePos].Count <= langPos)//if the language is not in memory, adds it and all the languages before it.
            {
                for (int i = 0; i < (langPos - TextPages[pagePos].Count) + 1; i++)
                {
                    TextPages[pagePos].Add(new Dictionary<string, string>());
                }
            }
        }
        else//if the page is not in memory, find adds it.
        {
            lock (PageNames)
            {
                PageNames.Add(PageName);
                pagePos = PageNames.Count - 1;
                List<Dictionary<string, string>> TempLangList = new List<Dictionary<string, string>>();
                TempLangList.Add(new Dictionary<string, string>());
                TextPages.Add(TempLangList);

                if (TextPages[pagePos].Count <= langPos)//if the language is not in memory, adds it and all the languages before it.
                {
                    for (int i = 0; i < (langPos - TextPages[pagePos].Count) + 1; i++)
                    {
                        TextPages[pagePos].Add(new Dictionary<string, string>());
                    }
                }
            }

        }

        string[] StringFormatArr = { };
        if (textName.IndexOf(',') > 0)
        {
            textName = textName.Substring(0, textName.IndexOf(','));
            StringFormatArr = textName.Substring(textName.IndexOf(',') + 1, textName.Length - 2).Split(',');
        }




        if (TextPages.Count > pagePos && TextPages[pagePos] != null && TextPages[pagePos].Count > langPos && TextPages[pagePos][langPos] != null && TextPages[pagePos][langPos].ContainsKey(textName))// if the text doesn't exist in the dictionary, gets it from DB
        {
            TheText = string.Format(TextPages[pagePos][langPos][textName], StringFormatArr);
        }
        else
        {
            lock (TextPages[pagePos])
            {
                if (TextPages[pagePos].Count <= langPos)//if the language is not in memory, adds it and all the languages before it.
                {
                    for (int i = 0; i < (langPos - TextPages[pagePos].Count) + 1; i++)
                    {
                        TextPages[pagePos].Add(new Dictionary<string, string>());
                    }
                }
            }


            MySql.Data.MySqlClient.MySqlDataReader MyReader = null;
            using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
            {
                con.Open();

                {
                    //looking for the translation in dictionary
                    string sql = "SELECT `" + lang + "`, `Eng` As EngText FROM LangText WHERE `TextName`='" + textName.Replace("'", "''") + "' AND `PageName`='" + PageName + "'";
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, con);
                    MyReader = cmd.ExecuteReader();
                    if (MyReader.Read())
                    {
                        if (MyReader[lang].ToString() != "")
                        {

                            TextPages[pagePos][langPos][textName] = MyReader[lang].ToString();

                        }
                        else
                        {
                            TextPages[pagePos][langPos][textName] = MyReader["EngText"].ToString();
                        }
                    }
                    else//if the text is not in the database - adds it.
                    {
                        if (textName != "")
                        {
                            using (MySql.Data.MySqlClient.MySqlConnection con2 = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
                            {
                                con2.Open();
                                MySqlCommand cmd2 = new MySqlCommand();
                                cmd2.Connection = con2;

                                if (HebText != "")
                                {
                                    if (lang.ToLower() != "eng")
                                    {
                                        cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + lang + "`,`Eng`) VALUES ('" + textName.Replace("'", "''") + "','" + PageName + "','" + HebText.Replace("'", "''") + "','" + textName.Replace("'", "''") + "')";
                                    }
                                    else
                                    {
                                        cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + lang + "`) VALUES ('" + textName.Replace("'", "''") + "','" + PageName + "','" + HebText.Replace("'", "''") + "')";
                                    }

                                    TextPages[pagePos][langPos][textName] = HebText;
                                }
                                else
                                {
                                    if (lang.ToLower() != "eng")
                                    {
                                        cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + lang + "`,`Eng`) VALUES ('" + textName.Replace("'", "''") + "','" + PageName + "','####" + textName + "####','" + textName.Replace("'", "''") + "')";
                                    }
                                    else
                                    {
                                        cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + lang + "`) VALUES ('" + textName.Replace("'", "''") + "','" + PageName + "','####" + textName + "####')";
                                    }
                                    TextPages[pagePos][langPos][textName] = "####" + textName + "####";
                                }
                                cmd2.ExecuteNonQuery();
                                con2.Close();
                            }
                        }
                    }

                    TheText = string.Format(TextPages[pagePos][langPos][textName], StringFormatArr);

                    MyReader.Close();
                    cmd.CommandText = "UPDATE LangText SET lastUsed=now() WHERE `TextName`='" + textName.Replace("'", "''") + "' AND `PageName`='" + PageName + "'"; ;
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }



        return TheText;
    }



    public static string oldMyText(string textName, string lang)
    {
        string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
        string TheText = "";
        string PageName = "general";
        string HebText = "";
        int langPos = 0;
        int pagePos = 0;


        if (textName.Contains("_"))
        {


            PageName = textName.Remove(textName.IndexOf("_"));
            textName = textName.Substring(textName.IndexOf("_") + 1);
        }

        if (textName.Contains("_"))
        {
            HebText = textName.Substring(textName.IndexOf("_") + 1);
            textName = textName.Remove(textName.IndexOf("_"));
        }

        if (LangNames.Contains(lang))//if the language is in memory, find it's position.
        {
            langPos = LangNames.IndexOf(lang);
        }
        else//if the language is not in memory, find adds it.
        {
            LangNames.Add(lang);
            langPos = LangNames.Count - 1;
        }


        if (PageNames.Contains(PageName) && TextPages.Count > pagePos)//if the page is in memory, find it's position.
        {
            pagePos = PageNames.IndexOf(PageName);
            if (TextPages[pagePos].Count <= langPos)//if the language is not in memory, adds it and all the languages before it.
            {
                for (int i = 0; i < (langPos - TextPages[pagePos].Count) + 1; i++)
                {
                    TextPages[pagePos].Add(new Dictionary<string, string>());
                }
            }
        }
        else//if the page is not in memory, find adds it.
        {
            PageNames.Add(PageName);
            pagePos = PageNames.Count - 1;
            List<Dictionary<string, string>> TempLangList = new List<Dictionary<string, string>>();
            TempLangList.Add(new Dictionary<string, string>());
            TextPages.Add(TempLangList);

            if (TextPages[pagePos].Count <= langPos)//if the language is not in memory, adds it and all the languages before it.
            {
                for (int i = 0; i < (langPos - TextPages[pagePos].Count) + 1; i++)
                {
                    TextPages[pagePos].Add(new Dictionary<string, string>());
                }
            }

        }

        string[] StringFormatArr = { };
        if (textName.IndexOf(',') > 0)
        {
            textName = textName.Substring(0, textName.IndexOf(','));
            StringFormatArr = textName.Substring(textName.IndexOf(',') + 1, textName.Length - 2).Split(',');
        }




        if (TextPages[pagePos][langPos].ContainsKey(textName))// if the text doesn't exist in the dictionary, gets it from DB
        {
            TheText = string.Format(TextPages[pagePos][langPos][textName], StringFormatArr);
        }
        else
        {
            MySql.Data.MySqlClient.MySqlDataReader MyReader = null;
            using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
            {
                con.Open();

                {
                    //looking for the translation in dictionary
                    string sql = "SELECT `" + lang + "` FROM LangText WHERE `TextName`='" + textName.Replace("'", "''") + "' AND `PageName`='" + PageName + "'";
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, con);
                    MyReader = cmd.ExecuteReader();
                    if (MyReader.Read())
                    {
                        TextPages[pagePos][langPos][textName] = MyReader[lang].ToString();
                    }
                    else//if the text is not in the database - adds it.
                    {
                        if (textName != "")
                        {
                            using (MySql.Data.MySqlClient.MySqlConnection con2 = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
                            {
                                con2.Open();
                                MySqlCommand cmd2 = new MySqlCommand();
                                cmd2.Connection = con2;

                                if (HebText != "")
                                {
                                    if (lang.ToLower() != "eng")
                                    {
                                        cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + lang + "`,`Eng`) VALUES ('" + textName.Replace("'", "''") + "','" + PageName + "','" + HebText.Replace("'", "''") + "','" + textName.Replace("'", "''") + "')";
                                    }
                                    else
                                    {
                                        cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + lang + "`) VALUES ('" + textName.Replace("'", "''") + "','" + PageName + "','" + HebText.Replace("'", "''") + "')";
                                    }

                                    TextPages[pagePos][langPos][textName] = HebText;
                                }
                                else
                                {
                                    if (lang.ToLower() != "eng")
                                    {
                                        cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + lang + "`,`Eng`) VALUES ('" + textName.Replace("'", "''") + "','" + PageName + "','####" + textName + "####','" + textName.Replace("'", "''") + "')";
                                    }
                                    else
                                    {
                                        cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + lang + "`) VALUES ('" + textName.Replace("'", "''") + "','" + PageName + "','####" + textName + "####')";
                                    }
                                    TextPages[pagePos][langPos][textName] = "####" + textName + "####";
                                }
                                cmd2.ExecuteNonQuery();
                                con2.Close();
                            }
                        }
                    }
                    TheText = string.Format(TextPages[pagePos][langPos][textName], StringFormatArr);

                    MyReader.Close();
                    cmd.CommandText = "UPDATE LangText SET lastUsed=now() WHERE `TextName`='" + textName.Replace("'", "''") + "' AND `PageName`='" + PageName + "'"; ;
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }



        return TheText;
    }


    public static string MyText(string textName)
    {
        System.Web.HttpContext currentContext = System.Web.HttpContext.Current;
        HttpSessionState CurSession = currentContext.Session;
        string langName = "Heb";

        if (CurSession != null && CurSession["Lang"] != null)
        {
            langName = CurSession["Lang"].ToString();
        }

        string pageName = "general";
        string hebText = "";
        if (textName.Contains("_"))
        {
            pageName = textName.Remove(textName.IndexOf("_"));
            textName = textName.Substring(textName.IndexOf("_") + 1);
        }

        if (textName.Contains("_"))
        {
            hebText = textName.Substring(textName.IndexOf("_") + 1);
            textName = textName.Remove(textName.IndexOf("_"));
        }

        SendMsgLang tempLang = null;
        lock (sendMagLangs)
        {
            foreach (SendMsgLang sendMsgLang in sendMagLangs)
            {
                if (sendMsgLang.LangName == langName)
                {
                    tempLang = sendMsgLang;
                }
            }
        }


        if (tempLang == null)
        {
            tempLang = new SendMsgLang(langName);
            lock (sendMagLangs)
            {
                sendMagLangs.Add(tempLang);
            }
        }

        return tempLang.getText(pageName, textName, hebText);
    }


    public static string MyText(string textName, string langName)
    {
        if (langName == "")
        {
            langName = "Heb";
        }

        string pageName = "general";
        string hebText = "";
        if (textName.Contains("_"))
        {
            pageName = textName.Remove(textName.IndexOf("_"));
            textName = textName.Substring(textName.IndexOf("_") + 1);
        }

        if (textName.Contains("_"))
        {
            hebText = textName.Substring(textName.IndexOf("_") + 1);
            textName = textName.Remove(textName.IndexOf("_"));
        }

        SendMsgLang tempLang = null;
        foreach (SendMsgLang sendMsgLang in sendMagLangs)
        {
            if (sendMsgLang.LangName == langName)
            {
                tempLang = sendMsgLang;
            }
        }
        if (tempLang == null)
        {
            tempLang = new SendMsgLang(langName);
            lock (sendMagLangs)
            {
                sendMagLangs.Add(tempLang);
            }
        }

        return tempLang.getText(pageName, textName, hebText);
    }

}




public class SendMsgLang
{
    string langName;
    public List<SendMsgPage> sendMsgPages = new List<SendMsgPage>();

    public string LangName
    {
        get { return langName; }
    }

    public SendMsgLang(string LangCode)
    {
        langName = LangCode;
    }

    public string getText(string pageName, string textName, string hebText)
    {
        SendMsgPage tempPage = null;
        lock (sendMsgPages)
        {
            foreach (SendMsgPage sendMsgPage in sendMsgPages)
            {
                if (sendMsgPage.PageName == pageName)
                {
                    tempPage = sendMsgPage;
                }
            }
        }


        if (tempPage == null)
        {
            tempPage = new SendMsgPage(pageName);
            lock (sendMsgPages)
            {
                sendMsgPages.Add(tempPage);
            }
        }

        return tempPage.getText(langName, textName, hebText);
    }

}

public class SendMsgPage
{
    string pageName;
    public List<SendMagLangText> sendMsgTexts = new List<SendMagLangText>();

    public SendMsgPage(string PageName)
    {
        pageName = PageName;
    }

    public string PageName
    {
        get { return pageName; }
    }

    public string getText(string langName, string textName, string hebText)
    {
        SendMagLangText tempText = null;
        lock (sendMsgTexts)
        {
            foreach (SendMagLangText sendMsgText in sendMsgTexts)
            {
                if (sendMsgText.TextName == textName)
                {
                    tempText = sendMsgText;
                }
            }
        }
        if (tempText == null)
        {
            tempText = new SendMagLangText(textName);
            lock (sendMsgTexts)
            {
                sendMsgTexts.Add(tempText);
                sendMsgTexts = sendMsgTexts.OrderBy(d => d.TourOrder).ToList();
            }

        }

        return tempText.getText(langName, pageName, hebText);
    }
}

public class SendMagLangText
{
    string textName = "";
    string textValue = null;
    public string TourContent { get; set; }
    public double TourOrder { get; set; }
    public int TourPosition { get; set; }
    public string TourFinishToPage { get; set; }
    public bool TourStatus { get; set; }

    public SendMagLangText(string TextName)
    {
        textName = TextName;
    }

    public string TextName
    {
        get { return textName; }
    }

    public string TextValue
    {
        get { return textValue; }
    }

    public string getText(string langName, string pageName, string hebText)
    {

        if (textValue != null)
        {
        }
        else
        {
            MySql.Data.MySqlClient.MySqlDataReader MyReader = null;
            string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;

            using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
            {
                con.Open();


                //looking for the translation in dictionary
                string sql = "SELECT `" + langName + "`, `Eng` As EngText FROM LangText WHERE `TextName`='" + textName.Replace("'", "''") + "' AND `PageName`='" + pageName.Replace("'", "''") + "'";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, con);
                MyReader = cmd.ExecuteReader();
                if (MyReader.Read())
                {
                    if (MyReader[langName].ToString() != "")
                    {

                        textValue = MyReader[langName].ToString();
                        //TourContent = MyReader["Tour"].ToString();

                        //double TourOrderTemp = 0;
                        //double.TryParse(MyReader["NumOrder"].ToString(), out TourOrderTemp);
                        //TourOrder = TourOrderTemp;

                        //int TourPositionTemp = 3;
                        //int.TryParse(MyReader["Position"].ToString(), out TourPositionTemp);
                        //TourPosition = TourPositionTemp;

                        //TourFinishToPage = MyReader["FinishToPage"].ToString();

                        //if (MyReader["Status"].ToString() == "1")
                        //{
                        //    TourStatus = true;
                        //}

                    }
                    else
                    {
                        textValue = MyReader["EngText"].ToString();
                    }
                }
                else//if the text is not in the database - adds it.
                {
                    if (textName != "")
                    {
                        using (MySql.Data.MySqlClient.MySqlConnection con2 = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
                        {
                            con2.Open();
                            MySqlCommand cmd2 = new MySqlCommand();
                            cmd2.Connection = con2;

                            if (hebText != "")
                            {
                                if (langName.ToLower() != "eng")
                                {
                                    cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + langName + "`,`Eng`) VALUES ('" + textName.Replace("'", "''") + "','" + pageName.Replace("'", "''") + "','" + hebText.Replace("'", "''") + "','" + textName.Replace("'", "''") + "')";
                                }
                                else
                                {
                                    cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + langName + "`) VALUES ('" + textName.Replace("'", "''") + "','" + pageName + "','" + hebText.Replace("'", "''") + "')";
                                }

                                textValue = hebText;
                            }
                            else
                            {
                                if (langName.ToLower() != "eng")
                                {
                                    cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + langName + "`,`Eng`) VALUES ('" + textName.Replace("'", "''") + "','" + pageName + "','####" + textName.Replace("'", "''") + "####','" + textName.Replace("'", "''") + "')";
                                }
                                else
                                {
                                    cmd2.CommandText = "INSERT INTO LangText (TextName,PageName, `" + langName + "`) VALUES ('" + textName.Replace("'", "''") + "','" + pageName.Replace("'", "''") + "','####" + textName.Replace("'", "''") + "####')";
                                }
                                textValue = "####" + textName + "####";
                            }
                            cmd2.ExecuteNonQuery();
                            con2.Close();
                        }
                    }
                }
                MyReader.Close();
                cmd.CommandText = "UPDATE LangText SET lastUsed=now() WHERE `TextName`='" + textName.Replace("'", "''") + "' AND `PageName`='" + pageName + "'"; ;
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        return textValue;
    }
}
