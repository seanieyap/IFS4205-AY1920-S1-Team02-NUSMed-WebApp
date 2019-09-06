using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace NUSMed_WebApp
{
    public class Global : HttpApplication
    {
        protected void Application_BeginRequest()
        {
            // Remove insecure protocols (SSL3, TLS 1.0, TLS 1.1)
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
            // Add TLS 1.2
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

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
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);

                    if (authTicket == null || authTicket.Expired)
                    {
                        return;
                    }

                    string nric = authTicket.Name;
                    List<string> userData = new List<string>(authTicket.UserData.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
                    string guid = string.Empty;

                    if (userData.Any()) //prevent IndexOutOfRangeException for empty list
                    {
                        guid = userData.ElementAt(userData.Count - 1);
                        userData.RemoveAt(userData.Count - 1);
                    }

                    AccountBLL accountBLL = new AccountBLL();
                    Account account = accountBLL.GetStatus(nric);

                    if (account.status == 0 
                        || !((account.roles.Count() > 0 && userData[0] == "Multiple") || account.roles.Contains(userData[0]))
                        || authTicket.IssueDate < account.lastFullLogin)
                    {
                        FormsAuthentication.SignOut();
                        FormsAuthentication.RedirectToLoginPage("fail-auth=true");
                    }

                    // if not cached, cache user
                    if (HttpContext.Current == null || HttpContext.Current.Cache[nric] == null || HttpRuntime.Cache.Get(nric).ToString().Equals(guid))
                    {
                        HttpRuntime.Cache.Insert(nric, guid, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15));
                    }
                    else
                    {
                        // Cache does not match, hence multiple logins detected
                        FormsAuthentication.SignOut();
                        FormsAuthentication.RedirectToLoginPage("multiple-logins=true");
                        return;
                    }

                    e.User = new GenericPrincipal(new GenericIdentity(authTicket.Name, "Forms"), userData.ToArray());
                }
                catch
                {
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
        }

    }
}