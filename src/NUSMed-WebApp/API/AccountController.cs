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
        // POST api/account/authenticate/password
        // Authenticate NRIC + password + device ID upon launching mobile app
        [Route("authenticate/password")]
        [HttpPost]
        public HttpResponseMessage AuthenticatePassword([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string nric = credentials.nric;
            string password = credentials.password;
            string deviceID = credentials.deviceID;

            AccountBLL accountBLL = new AccountBLL();

            if (!string.IsNullOrEmpty(nric) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(deviceID))
            {
                Account account = accountBLL.GetStatus(nric, password, deviceID);

                switch (account.status)
                {
                    // MFA is enabled
                    case 1:
                        // return JWT token?? with no permissions to do anything
                        string guid = accountBLL.LoginDevice(nric, "Multiple");
                        response = Request.CreateResponse(HttpStatusCode.OK, guid);
                        return response;

                    // Account/MFA is disabled
                    default:
                        return response;
                }

            }
            else
            {
                return response;
            }
        }

        // POST api/account/authenticate/register
        // Registers device for a user
        [Route("authenticate/register")]
        [HttpPost]
        public HttpResponseMessage RegisterDeviceID([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string nric = credentials.nric;
            string password = credentials.password;
            string deviceID = credentials.deviceID;
            string tokenID = credentials.tokenID;

            AccountBLL accountBLL = new AccountBLL();

            if (!string.IsNullOrEmpty(nric) && !string.IsNullOrEmpty(password) && 
                !string.IsNullOrEmpty(deviceID) && !string.IsNullOrEmpty(tokenID))
            {
                Account account = accountBLL.GetStatus(nric, password, deviceID, tokenID);

                switch (account.status)
                {
                    // MFA is enabled
                    case 1:
                        accountBLL.MFADeviceIDUpdateFromPhone(nric, tokenID, deviceID);

                        // return JWT token?? with no permissions to do anything
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        return response;

                    // Account/MFA is disabled
                    default:
                        return response;
                }

            }
            else
            {
                return response;
            }
        }

        // POST api/account/authenticate/weblogin
        [Route("authenticate/weblogin")]
        [HttpPost]
        public HttpResponseMessage WebLogin([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            response = Request.CreateResponse(HttpStatusCode.OK);

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