using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _404 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        favicon.Href = Domain.getParamsByDomain(siteParamType.secureSitePath)+"/" + Domain.getParamsByDomain(siteParamType.Favicon);
        var data_Err_Coo = Request.Cookies["data_Err"];
        if (data_Err_Coo!=null)
        {
            string Value = data_Err_Coo.Value;
            ClientScript.RegisterClientScriptBlock(GetType(), "console_log", "console.log(`data_Err:`,`" + Value + "`);", true);

        }

    }
}