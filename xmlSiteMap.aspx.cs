using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class xmlSiteMap : System.Web.UI.Page
{
	private string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;

	protected void Page_Load(object sender, EventArgs e)
	{
		Response.Clear();
		Response.ContentType = "text/plain";

		Response.Write("\n<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		Response.Write("\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

		using (MySqlConnection con2 = new MySqlConnection(ConnStr))
		{
			MySqlDataReader MyReader2;
			con2.Open();
			MySqlCommand cmd2 = new MySqlCommand();
			cmd2.Connection = con2;
			cmd2.CommandText = "SELECT landingTitle,LandingFriendly, tblSites.SiteID, CONCAT('https://n.sendmsg.co.il/Minisites.aspx?p=' , landingID , '-' , ((tblSites.SiteID + 3) * landingID * 36) , '-' , tblSites.SiteID) AS LandingUrl  FROM landingPages LEFT JOIN tblSites ON tblSites.SiteID = landingPages.SiteID WHERE IndexInGoogle=1 AND (" + Defaults.activeStatusIDsQuery + ")";
			MyReader2 = cmd2.ExecuteReader();

			while (MyReader2.Read())
			{


				Response.Write("\n<url>");
				if (MyReader2["LandingFriendly"].ToString().Trim() != "")
				{
					Response.Write("\n<loc>https://n.sendmsg.co.il/f" + MyReader2["siteID"].ToString() + "/" + MyReader2["LandingFriendly"].ToString().Replace("&", "") + "</loc>");
				}
				else
				{
					Response.Write("\n<loc>" + MyReader2["LandingURL"].ToString() + "</loc>");
				}
				Response.Write("\n<changefreq>weekly</changefreq>");
				Response.Write("\n<priority>0.8</priority>");
				Response.Write("\n</url>");

			}
		}
		Response.Write("\n</urlset>");

	}
}
