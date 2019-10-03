using NUSMed_WebApp.App_Start;
using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
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

            //ASP.NET WEB API CONFIG
            // Pass a delegate to the Configure method.
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        /// <summary>
        /// Overload method for form authentication role function.
        /// </summary>
        protected void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs e)
        {
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                try
                {
                    FormsAuthenticationTicket formAuthenticationTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);

                    if (formAuthenticationTicket == null || formAuthenticationTicket.Expired)
                    {
                        return;
                    }

                    string nric = formAuthenticationTicket.Name;
                    List<string> userData = new List<string>(formAuthenticationTicket.UserData.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
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
                        || formAuthenticationTicket.IssueDate < account.lastFullLogin
                        || HttpContext.Current == null || HttpContext.Current.Cache[nric] == null)
                    {
                        FormsAuthentication.SignOut();
                        FormsAuthentication.RedirectToLoginPage("fail-auth=true");
                    }

                    // if not cached, cache user
                    if (!HttpRuntime.Cache[nric].ToString().Equals(guid))
                    {
                        // Cache does not match, hence multiple logins detected
                        FormsAuthentication.SignOut();
                        FormsAuthentication.RedirectToLoginPage("multiple-logins=true");
                        return;
                    }
                    e.User = new GenericPrincipal(new GenericIdentity(formAuthenticationTicket.Name, "Forms"), userData.ToArray());

                    // Renew
                    FormsAuthenticationTicket newFormAuthenticationTicket = FormsAuthentication.RenewTicketIfOld(formAuthenticationTicket);
                    if (formAuthenticationTicket != newFormAuthenticationTicket)
                    {
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(newFormAuthenticationTicket));
                        cookie.HttpOnly = true;
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Domain = FormsAuthentication.CookieDomain;
                        cookie.Expires = newFormAuthenticationTicket.Expiration;
                        HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                }
                catch
                {
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage("fail-auth=true");
                }
            }
        }

    }
}