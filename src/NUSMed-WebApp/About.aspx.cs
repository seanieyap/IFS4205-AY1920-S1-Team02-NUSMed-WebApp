using System;
using System.Collections.Generic;
using System.Web.UI;
using System.IO;
using System.Configuration;

namespace NUSMed_WebApp
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAbout();

            byte[] bytes = File.ReadAllBytes("C:\\Users\\trueh\\Downloads\\high.jpg");
            String file = Convert.ToBase64String(bytes);

            string path = ConfigurationManager.AppSettings["fileServerPath"].ToString() + "\\test\\testfile01";
            File.WriteAllBytes(path, Convert.FromBase64String(file));
        }
    }
}
