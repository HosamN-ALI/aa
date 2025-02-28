using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Rank : System.Web.UI.Page
{
    private string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
    MySqlDataReader MyReader = null;
    int siteID;
    int msgID;
    int userID;
    int rankGiven = 0;
    int MsgValidator;
    int userValidator;
    string sentID = "";
    //int facebookValidator;
    bool isUserValidated = false;

    string langDir = "";
    int langId = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["lang"] != null)
        {
            int.TryParse(Request.QueryString["lang"], out langId);
        }

        string langCode = Defaults.GetLangCodeFromLangID(langId);
        int domainID = Domain.getDomainIDFromSiteID(siteID);

        //using (MySqlConnection con = new MySqlConnection(ConnStr))
        //{
        //	con.Open();
        //	MySqlCommand cmd = new MySqlCommand();
        //	cmd.Connection = con;
        //	MyReader = cmd.ExecuteReader();
        //	if (MyReader.Read())
        //	{
        ShowPageCredentials.Text = Defaults.GetPages(60, langCode, domainID);
        langDir = Defaults.GetPages(46, langCode, domainID);
        MsgTemplate.Text = Defaults.GetPages(78, langCode, domainID);

        //	}



        //	MyReader.Close();
        //	con.Close();
        //}

        string[] parameters = { };

        //MsgTemplate.Text = msgBody;

        if (!string.IsNullOrEmpty(Request.QueryString["p"]))
        {
            parameters = Request.QueryString["p"].Replace("--", "-").Split('-');
            if (parameters.Length == 6)
            {
                int.TryParse(parameters[0], out msgID);
                int.TryParse(parameters[1], out MsgValidator);
                int.TryParse(parameters[2], out siteID);
                int.TryParse(parameters[3], out userID);
                int.TryParse(parameters[4], out userValidator);
                int.TryParse(parameters[5], out rankGiven);
                if (rankGiven > 10 || rankGiven < 0)
                { rankGiven = 0; }
            }
            else if (parameters.Length == 7)
            {
                int.TryParse(parameters[0], out msgID);
                int.TryParse(parameters[2], out MsgValidator);
                MsgValidator = MsgValidator * (-1);
                int.TryParse(parameters[3], out siteID);
                int.TryParse(parameters[4], out userID);
                int.TryParse(parameters[5], out userValidator);
                int.TryParse(parameters[6], out rankGiven);
                if (rankGiven > 10 || rankGiven < 0)
                { rankGiven = 0; }
            }

            else
            {
                return;
            }
            Defaults.insertStatistic(siteID, userID, msgID);
            MsgTemplate.Text += "<div style=\"font-size:120px; color:#ff9142\">" + rankGiven.ToString() + "</div>";
        }

        if (siteID != 0 && msgID != 0)
        {
            using (MySqlConnection con = new MySqlConnection(ConnStr))
            {
                con.Open();//display the message


                if (Request.QueryString["lang"] == null)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT siteID, MsgLangID FROM SentMsgs WHERE siteID=" + siteID + " AND MsgID=" + msgID;
                    MyReader = cmd.ExecuteReader();
                    if (MyReader.Read())
                    {
                        int.TryParse(MyReader["MsgLangID"].ToString(), out langId);

                        //using (MySqlConnection con2 = new MySqlConnection(ConnStr))
                        //{
                        //	con2.Open();
                        //	MySqlCommand cmd2 = new MySqlCommand();
                        //	cmd2.Connection = con2;
                        //	MySqlDataReader MyReader2 = cmd2.ExecuteReader();
                        //	if (MyReader2.Read())
                        //	{
                        ShowPageCredentials.Text = Defaults.GetPages(60, langCode, domainID);
                        langDir = Defaults.GetPages(46, langCode, domainID);
                        //	}
                        //	MyReader2.Close();
                        //	con2.Close();
                        //}
                    }
                    else
                    {
                        return;
                    }
                }
                if (MyReader != null && !MyReader.IsClosed)
                {
                    MyReader.Close();
                }



                if (userID != 0)
                {
                    #region counting the display of the message
                    string sql = "SELECT SentOpenIP, date, sentID,NumOpened,mark FROM `sendingUsers`.`sendingUsers" + siteID.ToString() + "` LEFT JOIN tblUsers" + siteID.ToString() + " on tblUsers" + siteID.ToString() + ".id=`sendingUsers`.`sendingUsers" + siteID.ToString() + "`.userID Where MsgID=" + msgID + " AND userID=" + userID;

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
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
                            if (Math.Abs(ctrl) == Math.Abs(userValidator))
                            {
                                isUserValidated = true;
                            }

                            string currentIP = HttpContext.Current.Request.UserHostAddress;
                            if (MyReader["SentOpenIP"].ToString() == "")
                            {
                                if (isUserValidated)
                                {
                                    using (MySqlConnection con2 = new MySqlConnection(ConnStr))
                                    {
                                        con2.Open();
                                        MySqlCommand cmd2 = new MySqlCommand();
                                        cmd2.Connection = con2;


                                        cmd2.CommandText = "UPDATE `sendingUsers`.`sendingUsers" + siteID.ToString() + "` SET NumOpened=(NumOpened+1), SentOpenIP ='" + currentIP + "', openTime =now(), mark=" + rankGiven + " Where sentID=" + sentID;
                                        cmd2.ExecuteNonQuery();

                                        cmd2.CommandText = "UPDATE SentMsgs SET NumOpened=(NumOpened+1), MsgReners=(SELECT count(mark) FROM `sendingUsers`.`sendingUsers" + siteID.ToString() + "` WHERE MsgID=" + msgID + " and mark>0) ,msgRank=(SELECT Avg(mark) FROM `sendingUsers`.`sendingUsers" + siteID.ToString() + "` WHERE MsgID=" + msgID + " and mark>0) WHERE MSgID=" + msgID;
                                        cmd2.ExecuteNonQuery();

                                        con2.Close();
                                    }
                                }

                            }
                            else
                            {
                                using (MySqlConnection con2 = new MySqlConnection(ConnStr))
                                {
                                    con2.Open();
                                    MySqlCommand cmd2 = new MySqlCommand();
                                    cmd2.Connection = con2;


                                    cmd2.CommandText = "UPDATE `sendingUsers`.`sendingUsers" + siteID.ToString() + "` SET mark=" + rankGiven + " Where sentID=" + sentID;
                                    cmd2.ExecuteNonQuery();

                                    cmd2.CommandText = "UPDATE SentMsgs SET MsgReners=(SELECT count(mark) FROM `sendingUsers`.`sendingUsers" + siteID.ToString() + "` WHERE MsgID=" + msgID + " and mark>0) ,msgRank=(SELECT Avg(mark) FROM `sendingUsers`.`sendingUsers" + siteID.ToString() + "` WHERE MsgID=" + msgID + " and mark>0) WHERE MSgID=" + msgID;
                                    cmd2.ExecuteNonQuery();

                                    con2.Close();
                                }
                            }
                        }




                    }
                    if (MyReader != null && !MyReader.IsClosed)
                    {
                        MyReader.Close();
                    }
                    #endregion
                }
                con.Close();
            }
        }
    }
}
