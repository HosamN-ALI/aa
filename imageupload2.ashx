<%@ WebHandler Language="C#" Class="ImgUpload2" %>

using System;
using System.Web;
using System.IO;

public class ImgUpload2 : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        string savedpath = "";
        context.Response.ContentType = "text/plain";
        context.Response.Expires = -1;
        try
        {
            HttpPostedFile postedFile = context.Request.Files["Filedata"];
            string RawTempPath = "forumstyle/images/tempimg";
            string tempPath = context.Server.MapPath(RawTempPath);
            //giving the file a unique name, but saving it's extension
            string filename = Guid.NewGuid().ToString() + "_Large"+"--200x260--" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf("."));

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
            //removing files that exist in the upload folder for more than 2 days
            string[] olderFiles = Directory.GetFiles(tempPath);
            for (int i = 0; i < olderFiles.Length; i++)
            {
                FileInfo fileInf = new FileInfo(olderFiles[i]);
                if (fileInf.CreationTime < DateTime.Now.AddDays(-2) && fileInf.FullName.Contains("_Large"))
                {
                    File.Delete(olderFiles[i]);
                }
            }
            postedFile.SaveAs(tempPath + @"\" + filename);
            string[] splitter = { "--" };
            string[] savePathsArr = context.Request["folder"].Split('|');
            int filecount = 0;

            bool isImage = false;

            if (postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(".")).ToLower() == ".jpg" || postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(".")).ToLower() == ".png" || postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(".")).ToLower() == ".gif")
            {
                isImage = true;            
            }

            if (isImage)
            {
                #region

                foreach (string Path_size_name in savePathsArr)
                {
                    if (Path_size_name.Contains("x") && Path_size_name.Contains("--"))
                    {
                        string[] Path_size_nameArr = Path_size_name.Split(splitter, StringSplitOptions.None);
                        if (Path_size_nameArr.Length == 3)
                        {
                            filecount++;
                            string savepath = context.Server.MapPath(Path_size_nameArr[0]);
                            string[] saveSize = Path_size_nameArr[1].Split('x');
                            int SaveHeight = 0;
                            int SaveWidth = 0;
                            string saveAppendix = Path_size_nameArr[2];

                            if (int.TryParse(saveSize[0], out SaveWidth) && int.TryParse(saveSize[1], out SaveHeight))
                            {

                                if (!Directory.Exists(savepath))
                                    Directory.CreateDirectory(savepath);
                                if (filecount == 1)
                                {
                                    savedpath = Path_size_nameArr[0] + filename.Replace("_Large", saveAppendix);
                                }

                                using (System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(tempPath + @"\" + filename))
                                {

                                    System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(dummyfalse);
                                    System.Drawing.Image ThumbSizeImg;

                                    if (fullSizeImg.Height * 100 / SaveHeight > fullSizeImg.Width * 100 / SaveWidth)

                                    { ThumbSizeImg = fullSizeImg.GetThumbnailImage((fullSizeImg.Width * SaveHeight) / fullSizeImg.Height, SaveHeight, dummyCallBack, IntPtr.Zero); }
                                    else
                                    { ThumbSizeImg = fullSizeImg.GetThumbnailImage(SaveWidth, (fullSizeImg.Height * SaveWidth) / fullSizeImg.Width, dummyCallBack, IntPtr.Zero); }

                                    switch (System.IO.Path.GetExtension(savepath + @"\" + filename).ToLower())
                                    {
                                        case ".jpg":
                                            ThumbSizeImg.Save((savepath + @"\" + filename).Replace("_Large", saveAppendix), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            break;
                                        case ".gif":
                                            ThumbSizeImg.Save((savepath + @"\" + filename).Replace("_Large", saveAppendix), System.Drawing.Imaging.ImageFormat.Gif);
                                            break;
                                        case ".png":
                                            ThumbSizeImg.Save((savepath + @"\" + filename).Replace("_Large", saveAppendix), System.Drawing.Imaging.ImageFormat.Png);
                                            break;
                                    }
                                    ThumbSizeImg.Dispose();
                                }
                            }
                        }
                    }
                }
                #endregion
           
            savedpath = savedpath.Remove(0, savedpath.IndexOf("/images/")+1);
            } else
            {

                savedpath = tempPath+"\\"+ filename; 
            }
            context.Response.Write(savedpath);
            context.Response.StatusCode = 200;
        }
        catch (Exception ex)
        {
            
            
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    protected bool dummyfalse()
    {
        return false;
    }

}