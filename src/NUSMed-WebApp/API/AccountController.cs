using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace NUSMed_WebApp.API
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly AccountBLL accountBLL = new AccountBLL();        

        // POST api/account/authenticate
        // Authenticates client by JWT or basic authentication
        [Route("authenticate")]
        [HttpPost]
        public HttpResponseMessage Authenticate()
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];

            // Ensure Authorization Header exists
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                string authHeaderValue = authHeader.Substring("Bearer ".Length).Trim();
                string authHeaderValueDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue));
                string[] authHeaderParts = authHeaderValueDecoded.Split(':');
                string jwt = authHeaderParts[0];
                string deviceID = authHeaderParts[1];

                response = handleJwtAuth(jwt, deviceID);
            }
            else if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string authHeaderValue = authHeader.Substring("Basic ".Length).Trim();
                string authHeaderValueDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue));
                string[] authHeaderParts = authHeaderValueDecoded.Split(':');
                string nric = authHeaderParts[0];
                string password = authHeaderParts[1];
                string deviceID = authHeaderParts[2];

                response = handleBasicAuth(nric, password, deviceID);
            }

            return response;
        }

        private HttpResponseMessage handleJwtAuth(string jwt, string deviceID)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string retrievedNRIC;

            JWTBLL jwtBll = new JWTBLL();

            // Ensure jwt, deviceID exists
            if (!(!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID)))
            {
                return response;
            }

            // Validate jwt
            if (!jwtBll.ValidateJWT(jwt))
            {
                return response;
            }
            else
            {
                retrievedNRIC = jwtBll.getNRIC(jwt);
            }

            // Authorize if the account is valid
            if (accountBLL.IsValid(retrievedNRIC, deviceID))
            {
                string newJwt = jwtBll.UpdateJWT(jwt);
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Authorization", "Bearer " + newJwt);
                return response;
            }

            return response;
        }

        private HttpResponseMessage handleBasicAuth(string nric, string password, string deviceID)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            JWTBLL jwtBll = new JWTBLL();

            // Ensure nric, password, deviceID exists
            if (!(AccountBLL.IsNRICValid(nric) && AccountBLL.IsPasswordValid(password) && AccountBLL.IsDeviceIDValid(deviceID)))
            {
                return response;
            }

            // Authorize if the account is valid
            Account account = accountBLL.GetStatus(nric, password, deviceID);
            if (account.status == 1)
            {
                string role = account.patientStatus.ToString() + account.therapistStatus.ToString();
                string newJwt = accountBLL.LoginDevice(nric, role);
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Authorization", "Bearer " + newJwt);
                return response;
            }

            return response;
        }

        // POST api/account/register
        // Registers device for a client
        [Route("register")]
        [HttpPost]
        public HttpResponseMessage RegisterDevice()
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string nric;
            string password;
            string deviceID;
            string tokenID;

            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];

            // Ensure Authorization Header exists
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string authHeaderValue = authHeader.Substring("Basic ".Length).Trim();
                string authHeaderValueDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue));
                string[] authHeaderParts = authHeaderValueDecoded.Split(':');
                nric = authHeaderParts[0];
                password = authHeaderParts[1];
                deviceID = authHeaderParts[2];
                tokenID = authHeaderParts[3];
            }
            else
            {
                return response;
            }

            // Ensure nric, password, deviceID, tokenID exists
            if (!(AccountBLL.IsNRICValid(nric) && AccountBLL.IsPasswordValid(password) &&
                AccountBLL.IsDeviceIDValid(deviceID) && AccountBLL.IsTokenIDValid(tokenID)))
            {
                return response;
            }

            // Register deviceID for an account if the account is valid
            Account account = accountBLL.GetStatus(nric, password, deviceID, tokenID);
            if (account.status == 1)
            {
                try
                {
                    accountBLL.MFADeviceIDCheckFromPhone(nric, tokenID, deviceID);
                    string responseString = "Registration successful";
                    response = Request.CreateResponse(HttpStatusCode.OK, responseString);
                }
                catch
                {
                    string responseString = "An error occured during registration.";
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, responseString);
                }
            }

            return response;
        }

        // POST api/account/weblogin
        // Validates and authorizes client's web login request 
        [Route("weblogin")]
        [HttpPost]
        public HttpResponseMessage WebLogin()
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string jwt;
            string deviceID;
            string tokenID;
            string retrievedNRIC;

            JWTBLL jwtBll = new JWTBLL();

            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];

            // Ensure Authorization Header exists
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                string authHeaderValue = authHeader.Substring("Bearer ".Length).Trim();
                string authHeaderValueDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue));
                string[] authHeaderParts = authHeaderValueDecoded.Split(':');
                jwt = authHeaderParts[0];
                deviceID = authHeaderParts[1];
                tokenID = authHeaderParts[2];
            }
            else
            {
                return response;
            }

            // Ensure jwt, deviceID, tokenID exists
            if (!(!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID) && AccountBLL.IsTokenIDValid(tokenID)))
            {
                return response;
            }

            // Validate jwt
            if (!jwtBll.ValidateJWT(jwt))
            {
                return response;
            }
            else
            {
                retrievedNRIC = jwtBll.getNRIC(jwt);
            }

            // Validate deviceID and tokenID for retrievedNRIC
            if (accountBLL.IsValid(retrievedNRIC, tokenID, deviceID))
            {
                if (HttpContext.Current.Cache[retrievedNRIC + "_MFAAttempt"] != null &&
                    HttpContext.Current.Cache.Get(retrievedNRIC + "_MFAAttempt").ToString().Equals("Awaiting"))
                {
                    // MFA login was triggered, cache inserted
                    Account account = new Account();
                    account.associatedDeviceID = deviceID;
                    account.associatedTokenID = tokenID;
                    HttpContext.Current.Cache.Insert(retrievedNRIC + "_MFAAttempt", account, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

                    string newJwt = jwtBll.UpdateJWT(jwt);
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Authorization", "Bearer " + newJwt);
                }
                else
                {
                    // MFA login was not triggered before this request, cache not inserted
                    response = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }

            return response;
        }

        // POST api/account/selectrole
        // Returns new JWT with client's selected role
        [Route("selectrole")]
        [HttpPost]
        public HttpResponseMessage SelectRole()
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string jwt;
            string deviceID;
            string newJwtRole;
            string retrievedNRIC;

            JWTBLL jwtBll = new JWTBLL();

            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];

            // Ensure Authorization Header exists
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                string authHeaderValue = authHeader.Substring("Bearer ".Length).Trim();
                string authHeaderValueDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue));
                string[] authHeaderParts = authHeaderValueDecoded.Split(':');
                jwt = authHeaderParts[0];
                deviceID = authHeaderParts[1];
                newJwtRole = authHeaderParts[2];
            }
            else
            {
                return response;
            }

            // Ensure jwt, deviceID, newJwtRole exists
            if (!(!string.IsNullOrEmpty(newJwtRole) && !string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID)))
            {
                return response;
            }

            // Validate jwt
            if (!jwtBll.ValidateJWT(jwt))
            {
                return response;
            }
            else
            {
                retrievedNRIC = jwtBll.getNRIC(jwt);
            }

            // Validate deviceID for retrievedNRIC
            if (!(accountBLL.IsValid(retrievedNRIC, deviceID)))
            {
                return response;
            }

            // Issue new JWT with client's selected role
            Account account = accountBLL.GetStatus(retrievedNRIC);
            if (account.status == 1)
            {
                if (newJwtRole.Equals("10") && account.patientStatus == 1)
                {
                    string newJwt = jwtBll.GetJWT(retrievedNRIC, newJwtRole);
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Authorization", "Bearer " + newJwt);
                }
                else if (newJwtRole.Equals("01") && account.therapistStatus == 1)
                {
                    string newJwt = jwtBll.GetJWT(retrievedNRIC, newJwtRole);
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Authorization", "Bearer " + newJwt);
                }

                return response;
            }

            return response;
        }

        // POST api/account/getallroles
        // Returns JWT with all roles for a client
        [Route("getallroles")]
        [HttpPost]
        public HttpResponseMessage GetAllRoles()
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string jwt;
            string deviceID;
            string retrievedNRIC;

            JWTBLL jwtBll = new JWTBLL();

            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];

            // Ensure Authorization Header exists
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                string authHeaderValue = authHeader.Substring("Bearer ".Length).Trim();
                string authHeaderValueDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue));
                string[] authHeaderParts = authHeaderValueDecoded.Split(':');
                jwt = authHeaderParts[0];
                deviceID = authHeaderParts[1];
            }
            else
            {
                return response;
            }

            // Ensure jwt, deviceID exists
            if (!(!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID)))
            {
                return response;
            }

            // Validate jwt
            if (!jwtBll.ValidateJWT(jwt))
            {
                return response;
            }
            else
            {
                retrievedNRIC = jwtBll.getNRIC(jwt);
            }

            // Validate deviceID for retrievedNRIC
            if (!(accountBLL.IsValid(retrievedNRIC, deviceID)))
            {
                return response;
            }

            // Issue new JWT with all roles for the client
            Account account = accountBLL.GetStatus(retrievedNRIC);
            if (account.status == 1)
            {
                string role = account.patientStatus.ToString() + account.therapistStatus.ToString();
                string newJwt = jwtBll.GetJWT(retrievedNRIC, role);
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Authorization", "Bearer " + newJwt);
                return response;
            }

            return response;
        }

        // POST api/account/updatejwt
        // Returns updated JWT for a client
        [Route("updatejwt")]
        [HttpPost]
        public HttpResponseMessage UpdateJWT()
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string jwt;
            string deviceID;
            string retrievedNRIC;

            JWTBLL jwtBll = new JWTBLL();

            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];

            // Ensure Authorization Header exists
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                string authHeaderValue = authHeader.Substring("Bearer ".Length).Trim();
                string authHeaderValueDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue));
                string[] authHeaderParts = authHeaderValueDecoded.Split(':');
                jwt = authHeaderParts[0];
                deviceID = authHeaderParts[1];
            }
            else
            {
                return response;
            }

            // Ensure jwt, deviceID exists
            if (!(!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID)))
            {
                return response;
            }

            // Validate jwt
            if (!jwtBll.ValidateJWT(jwt))
            {
                return response;
            }
            else
            {
                retrievedNRIC = jwtBll.getNRIC(jwt);
            }

            // Validate deviceID for retrievedNRIC
            if (!(accountBLL.IsValid(retrievedNRIC, deviceID)))
            {
                return response;
            }

            // Issue updated JWT for the client
            Account account = accountBLL.GetStatus(retrievedNRIC);
            if (account.status == 1)
            {
                string newJwt = jwtBll.UpdateJWT(jwt);
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Authorization", "Bearer " + newJwt);
                return response;
            }

            return response;
        }

    }
}