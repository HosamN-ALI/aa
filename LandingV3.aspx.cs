using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LandingV3 : System.Web.UI.Page
{
    //readonly static string V = "?V=" + Guid.NewGuid();
    //readonly static string AccessPlug = "<script async defer type=\"text/javascript\" src=\"https://app.sendmsg.co.il/getAccessPlug.ashx" + V + "\"></script>";
    readonly static List<string> obj_adonsToHtml = new List<string>();
    //static Dictionary<string, Dictionary<string, List<string>>> pages = new System.Collections.Generic.Dictionary<string, Dictionary<string, List<string>>>();
    static readonly object _lock = new object();

    public class Collects
    {
        public class Link
        {
            public string href { get; set; }
            public string target { get; set; }
            public string componentId { get; set; }
        }
        public class Font
        {
            public string _id { get; set; }
            public string[] language { get; set; }
            public string name { get; set; }
            public string cssRule { get; set; }
            public string url { get; set; }
        }

        public List<Font> fonts { get; set; }

        public List<Link> links { get; set; }
    }

    public class Data
    {
        public string siteId { get; set; }
        public string mode { get; set; }
        public string domain { get; set; }
        public Collects collects { get; set; }
        public string html { get; set; }
    }

    public class Root
    {
        public int status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Request.IsLocal && !Request.IsSecureConnection)
        {
            Response.Redirect("https://" + Request.Url.Host + Request.RawUrl);
        }
    }

    private const int minutesCash = 10;
    string html = string.Empty;

    private string Copyright_footer(Defaults.CreV3_PageOption myPage)
    {
        if (myPage == null)
        {
            return "";
        }
        int domainID = Domain.getDomainIDFromSiteID(myPage.siteID);

        string BottomAdsStructure = @"<div id=""BottomAdsHeight"" style=""width: 100%; background: white;"" >
<div id=""Copyright"" style =""width: 100%; text-align: center; color: #1071b5; font-size: 13px; padding-top: 10px; padding-bottom: 20px;"" >
{0}
</div>
</div>";

        string langText = myPage.language;
        if (string.IsNullOrWhiteSpace(langText))
        {
            langText = "Eng";
        }
        var langPage = Defaults.GetLangCodeFromLangCode(langText);
        string BottomAds = string.Format(BottomAdsStructure, Defaults.GetPages(60, langPage, domainID));
        return BottomAds;

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Accept-Encoding", "gzip, compress, br");
        string userAgent = Defaults.GetUserAgentClient();
        DeviceType.TypeDeviceView dt = DeviceType.GetDeviceType();

        string myDt = dt.ToString();
        string pageID = Request.QueryString["pageID"];

        string formatKey = pageID + "_";
        string formatCournetKey = formatKey + myDt;
        //Dictionary<string, List<string>> htmlByUserAgentCollection;

        lock (_lock)
        {
            KeyValuePair<string, Defaults.CreV3_PageOption> rss = default(KeyValuePair<string, Defaults.CreV3_PageOption>);

            try
            {
                if (Request.QueryString[Defaults.actionName] == Defaults.rebuildName)
                {
                    //pages.Remove(pageID);


                    foreach (var suit in (DeviceType.TypeDeviceView[])Enum.GetValues(typeof(DeviceType.TypeDeviceView)))
                    {
                        var keyPage = Cache.Remove(formatKey + suit.ToString());
                        if (keyPage != null)
                        {
                            Cache.Remove(keyPage.ToString());
                        }
                    }

                    lock (Defaults.friendlyMapID_creaditorV3)
                    {
                        if (!string.IsNullOrWhiteSpace(Request.RawUrl))
                        {
                            rss = Defaults.friendlyMapID_creaditorV3.FirstOrDefault(d => d.Value.id == pageID);
                            if (!string.IsNullOrWhiteSpace(rss.Key))
                            {
                                var allRemove = Defaults.friendlyMapID_creaditorV3.Where(d => d.Value.id == pageID).ToList();
                                foreach (var item in allRemove)
                                {
                                    Defaults.friendlyMapID_creaditorV3.Remove(item.Key);
                                }
                            }
                        }
                    }
                }

                object htmlPage = null;
                var courentKey = Cache[formatCournetKey];
                if (courentKey != null)
                {
                    htmlPage = Cache[courentKey.ToString()];
                }
                //pages.TryGetValue(pageID, out htmlByUserAgentCollection);

                //if (htmlByUserAgentCollection == null)
                //{
                //    htmlByUserAgentCollection = new Dictionary<string, List<string>>();
                //}

                //var htmlByUserAgent = htmlByUserAgentCollection.FirstOrDefault(d => d.Value.Contains(userAgent));
                //html = htmlByUserAgent.Key;

                if (htmlPage != null)
                {
                    html = htmlPage.ToString();
                }

                if (string.IsNullOrWhiteSpace(html))
                {
                    string url = @"https://render.creaditor.ai/render/{0}";

                    if (string.IsNullOrWhiteSpace(rss.Key))
                    {
                        rss = Defaults.friendlyMapID_creaditorV3.FirstOrDefault(d => d.Value.id == pageID);
                    }

                    if (!string.IsNullOrWhiteSpace(rss.Key))
                    {
                        if (!rss.Value.isPublished)
                        {
                            Response.Cookies.Add(new HttpCookie("data_Err", "not 'isPublished'"));
                            Server.Transfer("404.aspx");
                        }
                    }


                    Uri domain = new Uri(Request.Url.Scheme + "://" + rss.Key);

                    string myDomain = domain.ToString();

                    if (!string.IsNullOrWhiteSpace(domain.Query))
                    {
                        myDomain = myDomain.Replace(domain.Query, "");
                    }
                    myDomain = myDomain.Replace((Request.Url.Scheme + "://"), "");

                    // Create a request using a URL that can receive a post. 

                    WebRequest request = WebRequest.Create(string.Format(url, pageID));
                    request.Timeout = 3000; // 3 s
                                            // Set the Method property of the request to POST.
                    request.Method = "POST";
                    // Create POST data and convert it to a byte array.
                    string postData = "{\"userAgent\":\"" + userAgent + "\", \"domain\":\"" + myDomain + "\", \"mode\":\"production\", \"rebuild\":\"true\", \"options\":{}, \"transforms\":[]}";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    // Set the ContentType property of the WebRequest.
                    request.ContentType = "application/JSON";
                    // Set the ContentLength property of the WebRequest.
                    request.ContentLength = byteArray.Length;

                    string responseFromServer = "";
                    // Get the request stream.
                    Stream dataStream;

                    using (dataStream = request.GetRequestStream())
                    {
                        // Write the data to the request stream.
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        // Close the Stream object.
                        dataStream.Close();

                        // Get the response.
                        using (WebResponse response = request.GetResponse())
                        {
                            // Display the status.
                            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                            // Get the stream containing content returned by the server.
                            dataStream = response.GetResponseStream();

                            // Open the stream using a StreamReader for easy access.
                            using (StreamReader reader = new StreamReader(dataStream))
                            {
                                // Read the content.
                                responseFromServer = reader.ReadToEnd();
                                // Display the content.

                                // Clean up the streams.
                                reader.Close();
                            }
                            // Clean up the streams.
                            response.Close();
                        }
                        // Clean up the streams.
                        dataStream.Close();
                    }

                    Root ResPage = new JavaScriptSerializer().Deserialize<Root>(responseFromServer);


                    html = ResPage.data.html;
                    obj_adonsToHtml.Add(Copyright_footer(rss.Value));
                    adonsToHtml();
                    bool insertHtml = true;
                    string uuidKey = Guid.NewGuid().ToString();

                    foreach (var suit in (DeviceType.TypeDeviceView[])Enum.GetValues(typeof(DeviceType.TypeDeviceView)))
                    {
                        if (suit != dt)
                        {
                            var otherKey = Cache[formatKey + suit.ToString()];
                            if (otherKey != null)
                            {
                                var otherPageHtml = Cache[otherKey.ToString()];

                                if (otherPageHtml.ToString() == html)
                                {
                                    uuidKey = otherKey.ToString();
                                    insertHtml = false;
                                }
                                break;
                            }
                        }
                    }

                    if (insertHtml)
                    {
                        Cache.Insert(uuidKey, html, null, DateTime.MaxValue, TimeSpan.FromMinutes(minutesCash));
                    }
                    Cache.Insert(formatCournetKey, uuidKey, null, DateTime.MaxValue, TimeSpan.FromMinutes(minutesCash));



                    //lock (htmlByUserAgentCollection)
                    //{
                    //    if (htmlByUserAgentCollection.ContainsKey(html))
                    //    {
                    //        htmlByUserAgentCollection[html].Add(userAgent);
                    //    }
                    //    else
                    //    {
                    //        htmlByUserAgentCollection.Add(html, new List<string>() { userAgent });
                    //    }
                    //}

                    //pages.Add(pageID, htmlByUserAgentCollection);
                }
            }
            catch (Exception ex)
            {
                obj_adonsToHtml.Add(Copyright_footer(new Defaults.CreV3_PageOption()));
                adonsToHtml();
                string jsonRss = ("rss:" + new JavaScriptSerializer().Serialize(rss));
                Defaults.ErrorWritingToDB(ref ex, "LandingV3.aspx.cs", jsonRss);

                html += "<script>console.log(`Message:" + ex.Message + "`,`StackTrace:" + ex.StackTrace + "` ,`" + jsonRss + "`);</script>";
            }
            finally { }
        }
        Response.Write(html);
        Response.End();
    }

    const string bodyTag = "</body>";
    void adonsToHtml()
    {
        foreach (var item in obj_adonsToHtml)
        {
            if (!html.Contains(item))
            {
                if (html.Contains(bodyTag))
                {
                    html = html.Insert(html.IndexOf(bodyTag), item);
                }
                else
                {
                    html += item;
                }
            }
        }
    }
}