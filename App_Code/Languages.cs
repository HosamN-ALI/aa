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


public class Languages : ExpressionBuilder
{
	static string lang = "Heb";
	public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
	{
	CodeTypeReferenceExpression thisType = new CodeTypeReferenceExpression(base.GetType());

		
	CodePrimitiveExpression expression = new CodePrimitiveExpression(entry.Expression.Trim().ToString());

	string evaluationMethod = "GetText";

	return new CodeMethodInvokeExpression(thisType, evaluationMethod, new CodeExpression[] { expression });
	}

	//static System.Collections.Generic.Dictionary<string, string> Texts = new System.Collections.Generic.Dictionary<string, string>();

	//public static string GetText(string textName)
	//{
	//    string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;

	//    System.Web.HttpContext currentContext = System.Web.HttpContext.Current;
	//    HttpSessionState CurSession = currentContext.Session;
	//    if (Texts.ContainsKey(textName) == false)
	//    {

	//        MySql.Data.MySqlClient.MySqlDataReader MyReader = null;
	//        using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
	//        {
	//            con.Open();//display the message

	//            try
	//            {
	//                string sql = "SELECT `" + lang + "` FROM LangText WHERE textName=" + textName;
	//                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, con);
	//                MyReader = cmd.ExecuteReader();
	//                if (MyReader.Read())
	//                {
	//                    Texts.Add(textName, MyReader[lang].ToString());
	//                }
	//            }
	//            catch { }
	//        }
	//    }
	//    else
	//    {
	//        Texts[textName] = Texts[textName] + " a ";
	//    }

	//    if (Texts.ContainsKey(textName))
	//    { return Texts[textName]; }
	//    else
	//    { return ""; }
	//}

	public static string GetText(string expression)
	{
		string textName = "";
		string[] StringFormatArr = { };
		if (expression.IndexOf(',') > 0)
		{
			textName = expression.Substring(0, expression.IndexOf(','));
			StringFormatArr = expression.Substring(expression.IndexOf(',') + 1, expression.Length - 2).Split(',');
		}
		else
		{
			textName = expression;
		}
		string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
		string TheText = "";
		System.Web.HttpContext currentContext = System.Web.HttpContext.Current;
		HttpSessionState CurSession = currentContext.Session;

		if (CurSession!=null && CurSession["Lang"] != null)
		{
			lang = CurSession["Lang"].ToString();
		}

		MySql.Data.MySqlClient.MySqlDataReader MyReader = null;
		using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
		{
			con.Open();
			try
			{
				string sql = "SELECT `" + lang + "` FROM LangText WHERE textName='" + textName + "'";
				MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, con);
				MyReader = cmd.ExecuteReader();
				if (MyReader.Read())
				{
					TheText = string.Format(MyReader[lang].ToString(), StringFormatArr);
				}
				else
				{
					if (textName != "")
					{
						using (MySql.Data.MySqlClient.MySqlConnection con2 = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
						{
							con2.Open();
							MySqlCommand cmd2 = new MySqlCommand();
							cmd2.Connection = con2;
							cmd2.CommandText = "INSERT INTO LangText (TextName) VALUES ('" + textName + "')";
							cmd2.ExecuteNonQuery();
							con2.Close();
						}
					}
				}
			}
			catch
			{ 
				
			}
			con.Close();
		}

		return TheText;
	}
}
