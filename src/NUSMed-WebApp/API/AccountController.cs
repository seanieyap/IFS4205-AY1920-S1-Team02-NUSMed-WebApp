using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NUSMed_WebApp.API
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        // POST api/account/authenticate/password
        // Authenticate NRIC + password upon first login on mobile app
        // how to prevent this call from being exploited to brute force password?
        [Route("authenticate/password")]
        [HttpPost]
        public HttpResponseMessage AuthenticatePassword([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string nric = credentials.nric;
            string password = credentials.password;

            AccountBLL accountBLL = new AccountBLL();

            if (!string.IsNullOrEmpty(nric) && !string.IsNullOrEmpty(password))
            {
                Account account = accountBLL.GetStatus(nric, password);

                switch (account.status)
                {
                    // MFA is enabled
                    case 1:
                        // return JWT token?? or return something which cannot be replayed?
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

        // POST api/account/authenticate/registerdevice
        // Registers device for a user
        // How to prevent anyone from using this to register any device for a valid token?
        [Route("authenticate/registerdevice")]
        [HttpPost]
        public HttpResponseMessage RegisterDeviceID([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string deviceID = credentials.deviceID;
            string tokenID = credentials.tokenID;

            AccountBLL accountBLL = new AccountBLL();

            if (!string.IsNullOrEmpty(deviceID) && !string.IsNullOrEmpty(tokenID))
            {
                // If token is valid
                if (true)
                {
                    accountBLL.MFADeviceIDUpdateFromPhone(tokenID, deviceID);
                    // return JWT token to user
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    return response;
                }
                else
                {
                    return response;
                }

            }
            else
            {
                return response;
            }
        }

        // POST api/account/authenticate/requestchallenge
        // Returns a challenge to a user who has launched the mobile app
        [Route("authenticate/requestchallenge")]
        [HttpPost]
        public HttpResponseMessage RequestChallenge([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string nric = credentials.nric;

            AccountBLL accountBLL = new AccountBLL();

            if (!string.IsNullOrEmpty(nric))
            {
                // Retrieve hashed pass + device ID + token ID -> derive key from it
                // Generate random challenge
                // Encrypt challenge with key
                // Respond with encrypted challenge 

                // But how is the service going to know/wait for the user's challenge response? 
                // Save encrypted challenge into new field in db and another api call would validate challenge response from user?
                return response;
            }
            else
            {
                return response;
            }
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