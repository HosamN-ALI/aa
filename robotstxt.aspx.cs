using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class robotstxt : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.Clear();
		Response.ContentType = "text/plain";

		Response.Write("User-agent: *\n");
		Response.Write("Allow: /\n");
		Response.Write("User-agent: Googlebot\n");
		Response.Write("Allow: /\n");
	}
}
