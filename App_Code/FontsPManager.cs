using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for FontsPlacer
/// </summary>
public class FontsPManager
{
    Dictionary<string,Font> FontMap = new Dictionary<string, Font>();
    public FontsPManager()
    {
        FontMap.Add("Rachel", new Font("Rachel", "//app.creaditor.com/cdtrfiles/fonts/?family=Rachel"));
        FontMap.Add("SpontanyYC", new Font("SpontanyYC", "//app.creaditor.com/cdtrfiles/fonts/?family=SpontanyYC"));
        FontMap.Add("Lobster", new Font("Lobster", "//fonts.googleapis.com/css?family=Lobster"));
        FontMap.Add("Open Sans Hebrew", new Font("Open Sans Hebrew", "//fonts.googleapis.com/earlyaccess/opensanshebrew.css"));
        FontMap.Add("Frank Ruhl Libre", new Font("Frank Ruhl Libre", "//fonts.googleapis.com/css?family=Frank+Ruhl+Libre"));
        FontMap.Add("David Libre", new Font("David Libre", "//fonts.googleapis.com/css?family=David+Libre"));
        FontMap.Add("Miriam Libre", new Font("Miriam Libre", "//fonts.googleapis.com/css?family=Miriam+Libre"));
        FontMap.Add("Varela Round", new Font("Varela Round", "//fonts.googleapis.com/css?family=Varela+Round"));
        FontMap.Add("Heebo", new Font("Heebo", "//fonts.googleapis.com/css?family=Heebo"));
        FontMap.Add("Amatica SC", new Font("Amatica SC", "//fonts.googleapis.com/css?family=Amatica+SC"));
        FontMap.Add("Suez One", new Font("Suez One", "//fonts.googleapis.com/css?family=Suez+One"));
        FontMap.Add("Rubik", new Font("Rubik", "//fonts.googleapis.com/css?family=Rubik"));
        FontMap.Add("Secular One", new Font("Secular One", "//fonts.googleapis.com/css?family=Secular+One"));
        FontMap.Add("Arimo", new Font("Arimo", "//fonts.googleapis.com/css?family=Arimo"));
        FontMap.Add("Alef Hebrew", new Font("alefhebrew", "//fonts.googleapis.com/earlyaccess/alefhebrew.css"));
    }


    public bool AddFontsToPage(string html_content, Literal page) 
    {
        FontsExtractor(html_content, page);
        return true;
    }


    private bool FontsPlacer(string fontName, Literal page, ref Dictionary<string,Font> register) 
    {
        if (!string.IsNullOrWhiteSpace(fontName) && FontMap.ContainsKey(fontName) && !register.ContainsKey(fontName))
        {
            page.Text+= FontMap[fontName].ToString();
            register.Add(fontName, FontMap[fontName]);
        }

        return true;
    }

    private bool FontsExtractor(string html_content,Literal page)
    {
        var doc = new HtmlDocument();

        doc.LoadHtml(html_content);

        Dictionary<string, Font> register = new Dictionary<string,Font>(); 
        var page_font_list = doc.DocumentNode.QuerySelectorAll("font");

        foreach (HtmlNode item in page_font_list)
        {
            var checkMap = item.GetAttributeValue("face","").Replace("&quot;", "");

            FontsPlacer(checkMap, page,ref register) ;
       
        }

        page_font_list = doc.DocumentNode.QuerySelectorAll("[style*='font-family']");

        foreach (HtmlNode item in page_font_list)
        {
            var style= item.GetAttributeValue("style", "").Replace("&quot;", "");
            var match = Regex.Match(style, @"(?<all>(?<key>font-family):(?<value>[\d\w\s%^;]+);)");

            if (match.Groups["all"].Success) 
            {
                var fontname = match.Groups["value"].Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).First().Trim();
                FontsPlacer(fontname, page, ref register);
            }
       
        }

        return true;
    }
}