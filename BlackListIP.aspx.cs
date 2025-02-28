using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BlackListIP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString != null && !Request.QueryString.ToString().Contains("CountMaxError"))
        {
            HaveCountMaxError();
        }

        if ((Request.ServerVariables["REMOTE_ADDR"] != null && Request.ServerVariables["REMOTE_ADDR"] == "80.178.143.26") || Request.Url.ToString().StartsWith("http://localhost"))
        {
            DelBlockClient_ip.Visible = true;
            passComp.Visible = false;
            WriteIP_Btn();
        }
        else if (txtBoxPass.Text == Defaults.CompanyPass)
        {
            DelBlockClient_ip.Visible = true;
            passComp.Visible = false;
            WriteIP_Btn();
        }
    }
    void HaveCountMaxError()
    {

            string TempUrl = Request.Url.ToString();
            if (TempUrl.Contains("?"))
            {
                Response.Redirect(TempUrl + "&CountMaxError=48");
            }
            else
            {
                Response.Redirect(TempUrl + "?CountMaxError=48");
            }
    }
    public void WriteIP_Btn()
    {
        bool HaveCountMaxError = false;
        int CountMaxError = 48;
        if (Request.QueryString["CountMaxError"] != null && Request.QueryString["CountMaxError"].ToString() != "" && int.TryParse(Request.QueryString["CountMaxError"].ToString(), out CountMaxError))
        {
            HaveCountMaxError = true;
        }

        lock (Defaults.ErrorsGuestsOnlineDictio)
        {
            DelBlockClient_ipRep.DataSource = Defaults.ErrorsGuestsOnlineDictio.Where(SpecificOldErrorsGuests => SpecificOldErrorsGuests.Value.ErrorsCount > CountMaxError).Select(ThisObj => ThisObj.Key).ToList<string>();
            DelBlockClient_ipRep.DataBind();
            int RepeatCount = ((System.Collections.Generic.List<string>)DelBlockClient_ipRep.DataSource).Count;
            if (RepeatCount > 0)
            {
                NoRes.Visible = false;
                DelAllIPBlock.Visible = true;
            }
            else if (HaveCountMaxError && CountMaxError > 1)
            {
                NoRes.Text += " שכמות השגיאות שלהם גדולה מ -  " + CountMaxError;
            }
        }
    }

    protected void Unnamed_Click(object sender, EventArgs e)
    {
        string IP = ((Button)sender).CommandArgument;
        lock (Defaults.ErrorsGuestsOnlineDictio)
        {
            Defaults.ErrorsGuestsOnlineDictio.Remove(IP);
        }
        RefreshPage();
    }

    protected void DelAllIPBlock_Click(object sender, EventArgs e)
    {
        lock (Defaults.ErrorsGuestsOnlineDictio)
        {
            Defaults.ErrorsGuestsOnlineDictio.Clear();
        }
        RefreshPage();
    }

    private void RefreshPage()
    {
        Response.Redirect(Request.Url.ToString());
    }
}