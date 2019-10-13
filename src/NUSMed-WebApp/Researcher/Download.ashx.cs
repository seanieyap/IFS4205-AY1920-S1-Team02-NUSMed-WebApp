using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Researcher
{
    /// <summary>
    /// Handle download of files from file server
    /// </summary>
    public class Download : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.ClearContent();
            response.ClearHeaders();
            response.Clear();

            if (HttpContext.Current.Request.QueryString.GetValues(null)?.Contains("record") ?? false)
            {
                response.StatusCode = 404;
                return;
            }

            long recordID = Convert.ToInt64(HttpContext.Current.Request.QueryString["record"]);

            Record record = new DataBLL().GetRecord(recordID);

            if (record == null || !record.IsFileSafe())
            {
                response.StatusCode = 404;
            }
            else
            {
                if (record.fileExtension.Equals(".jpeg"))
                {
                    response.ContentType = "image/jpeg";
                }
                else if (record.fileExtension.Equals(".jpg"))
                {
                    response.ContentType = "image/jpg";
                }
                else if (record.fileExtension.Equals(".png"))
                {
                    response.ContentType = "image/png";
                }
                else if (record.fileExtension.Equals(".txt"))
                {
                    response.ContentType = "text/plain";
                }
                else if (record.fileExtension.Equals(".mp4"))
                {
                    response.ContentType = "video/mp4";
                }
                response.AddHeader("Content-Disposition", "attachment; filename=\"" + record.id + record.fileExtension + "\"");
                response.WriteFile(record.fullpath);
            }

            response.Flush();
            response.Close();
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion
    }
}