using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Font
/// </summary>
public class Font
{
    private string name;
    private string href;

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public string Href
    {
        get
        {
            return href;
        }

        set
        {
            href = value;
        }
    }

    public Font(string name,string href)
    {
        Name = name;
        Href = href;
    }

    public HtmlGenericControl ToControl() 
    {
        var link = new HtmlGenericControl("link");
        link.Attributes.Add("href", this.href);
        link.Attributes.Add("font-name", this.name);
        link.Attributes.Add("rel", "stylesheet");
        link.Attributes.Add("async", "true");
        return link;
    }

    public override string ToString()
    {
        return string.Format(@"<link rel=""stylesheet"" href=""{0}"" font-name=""{1}"" async=""true"">", this.href,this.name);

    }
}