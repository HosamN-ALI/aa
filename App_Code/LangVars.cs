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
using System.Web.UI.WebControls;


public class LangVars : ExpressionBuilder
{

	static string lang = "Heb";
	public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
	{
	CodeTypeReferenceExpression thisType = new CodeTypeReferenceExpression(base.GetType());

		
	CodePrimitiveExpression expression = new CodePrimitiveExpression(entry.Expression.Trim().ToString());

	string evaluationMethod = "GetVar";

	return new CodeMethodInvokeExpression(thisType, evaluationMethod, new CodeExpression[] { expression });
	}


	public static ContentDirection GetVar(string expression)
	{

		string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
		ContentDirection TheValue = ContentDirection.LeftToRight;
		System.Web.HttpContext currentContext = System.Web.HttpContext.Current;
		HttpSessionState CurSession = currentContext.Session;

		if (CurSession["Lang"] != null)
		{
			lang = CurSession["Lang"].ToString();
		}

		MySql.Data.MySqlClient.MySqlDataReader MyReader = null;
		using (MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(ConnStr))
		{
			con.Open();
			try
			{
				string sql = "SELECT `" + lang + "` FROM LangVar WHERE LangVarName='" + expression + "'";
				MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, con);
				MyReader = cmd.ExecuteReader();
				if (MyReader.Read())
				{
					if (MyReader[lang].ToString().ToLower() == "rtl")
					{
						TheValue = ContentDirection.RightToLeft;
					}
				}
			}
			catch
			{

			}
			con.Close();
		}
		return TheValue;
	}


}
