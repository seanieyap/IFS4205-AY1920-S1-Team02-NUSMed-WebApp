using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Security;

namespace NUSMed_WebApp.API
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        // POST api/account/authenticate
        // Authenticate NRIC + password + device ID upon launching mobile app
        [Route("authenticate")]
        [HttpPost]
        public HttpResponseMessage Authenticate([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string nric = credentials.nric;
            string password = credentials.password;
            string deviceID = credentials.deviceID;
            string guid = credentials.guid;

            AccountBLL accountBLL = new AccountBLL();

            if (!string.IsNullOrEmpty(guid))
            {
                if (HttpContext.Current.Cache[guid] != null)
                {
                    string retrievedNRIC = HttpContext.Current.Cache[guid].ToString();

                    // check valid device id for a guid
                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        return response;
                    }
                }
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Ambiguous);
                return response;
            }

            if (AccountBLL.IsNRICValid(nric) && AccountBLL.IsPasswordValid(password) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                Account account = accountBLL.GetStatus(nric, password, deviceID);

                if (account.status == 1)
                {
                    string newGuid = accountBLL.LoginDevice(nric, "Multiple");
                    response = Request.CreateResponse(HttpStatusCode.OK, newGuid);
                    return response;
                }

            }

            return response;
        }

        // POST api/account/register
        // Registers device for a user
        [Route("register")]
        [HttpPost]
        public HttpResponseMessage RegisterDevice([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string nric = credentials.nric;
            string password = credentials.password;
            string deviceID = credentials.deviceID;
            string tokenID = credentials.tokenID;

            AccountBLL accountBLL = new AccountBLL();

            if (AccountBLL.IsNRICValid(nric) && AccountBLL.IsPasswordValid(password) && 
                AccountBLL.IsDeviceIDValid(deviceID) && AccountBLL.IsTokenIDValid(tokenID))
            {
                Account account = accountBLL.GetStatus(nric, password, deviceID, tokenID);

                if (account.status == 1)
                {
                    try
                    {
                        accountBLL.MFADeviceIDUpdateFromPhone(nric, tokenID, deviceID);

                        string test = nric + "::" + tokenID + "::" + deviceID;
                        // what should server return upon successful registration?
                        response = Request.CreateResponse(HttpStatusCode.OK, test);
                    }
                    catch
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    }
                }

            }

            return response;
        }

        // POST api/account/weblogin
        [Route("weblogin")]
        [HttpPost]
        public HttpResponseMessage WebLogin([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string deviceID = credentials.deviceID;
            string tokenID = credentials.tokenID;
            string guid = credentials.guid;
            
            if (HttpContext.Current.Cache[guid] != null)
            {             
                string nric = HttpContext.Current.Cache[guid].ToString();

                // validate deviceID and tokenID for a guid
                if (new AccountBLL().IsValid(nric, tokenID, deviceID))
                {
                    if (HttpContext.Current.Cache[nric + "_MFAAttempt"] != null &&
                        HttpContext.Current.Cache.Get(nric + "_MFAAttempt").ToString().Equals("Awaiting"))
                    {
                        // MFA login was triggered, cache inserted
                        Account account = new Account();
                        account.associatedDeviceID = deviceID;
                        account.associatedTokenID = tokenID;
                        HttpContext.Current.Cache.Insert(nric + "_MFAAttempt", account, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration);

                        response = Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        // MFA login was not trigger before this request, cache not inserted
                        response = Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
            }

            return response;
        }

        // POST api/account/selectrole
        [Route("selectrole")]
        [HttpPost]
        public HttpResponseMessage SelectRole([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            // Validate token
            // Validate requested role exists for user
            // Reissue token with new role

            return response;
        }

    }
}