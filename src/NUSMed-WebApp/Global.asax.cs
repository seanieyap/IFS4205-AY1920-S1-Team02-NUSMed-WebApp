using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace NUSMed_WebApp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Overload method for form authentication role function.
        /// </summary>
        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                try
                {
                    // Extract the username now           
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);

                    if (authTicket == null || authTicket.Expired)
                    {
                        return;
                    }

                    string username = authTicket.Name;

                    string[] roles = authTicket.UserData.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    
                    // Let us set the Pricipal with our user specific details and...
                    // Extract the roles from our own custom cookie
                    e.User = new GenericPrincipal(new GenericIdentity(authTicket.Name, "Forms"), roles);
                }
                catch
                {
                }
            }
        }

    }
}