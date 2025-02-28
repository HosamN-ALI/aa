<%@ WebHandler Language="C#" Class="getHtmlPage" %>

using System;
using System.Web;

public class getHtmlPage : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/HTML";
        context.Response.Write(context.Request["htmlName"]);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}