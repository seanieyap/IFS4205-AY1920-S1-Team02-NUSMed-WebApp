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
        // Authenticate NRIC + password + device ID upon launching mobile app
        [Route("authenticate")]
        [HttpPost]
        public HttpResponseMessage Authenticate([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string nric = credentials.nric;
            string password = credentials.password;
            string deviceID = credentials.deviceID;
            string jwt = credentials.jwt;
            //string guid = credentials.guid;

            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        string newJwt = jwtBll.UpdateJWT(jwt);
                        response = Request.CreateResponse(HttpStatusCode.OK, newJwt);
                        return response;
                    }
                }
            }
            else if (AccountBLL.IsNRICValid(nric) && AccountBLL.IsPasswordValid(password) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                Account account = accountBLL.GetStatus(nric, password, deviceID);

                if (account.status == 1)
                {
                    string role = account.patientStatus.ToString() + account.therapistStatus.ToString();
                    string newJwt = accountBLL.LoginDevice(nric, role);
                    response = Request.CreateResponse(HttpStatusCode.OK, newJwt);
                    return response;
                }

            }

            /*if (!string.IsNullOrEmpty(guid) && AccountBLL.IsDeviceIDValid(deviceID))
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
            else if (AccountBLL.IsNRICValid(nric) && AccountBLL.IsPasswordValid(password) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                Account account = accountBLL.GetStatus(nric, password, deviceID);

                if (account.status == 1)
                {
                    string newGuid = accountBLL.LoginDevice(nric, "Multiple");
                    response = Request.CreateResponse(HttpStatusCode.OK, newGuid);
                    return response;
                }

            }*/

            return response;
        }

        [Route("authenticatetest")]
        [HttpPost]
        public HttpResponseMessage AuthenticateTest()
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];
            
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
            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        string newJwt = jwtBll.UpdateJWT(jwt);
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Headers.Add("Authorization", "Bearer " + newJwt);
                        return response;
                    }
                }
            }

            return response;
        }

        private HttpResponseMessage handleBasicAuth(string nric, string password, string deviceID)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            JWTBLL jwtBll = new JWTBLL();

            if (AccountBLL.IsNRICValid(nric) && AccountBLL.IsPasswordValid(password) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                Account account = accountBLL.GetStatus(nric, password, deviceID);

                if (account.status == 1)
                {
                    string role = account.patientStatus.ToString() + account.therapistStatus.ToString();
                    string newJwt = accountBLL.LoginDevice(nric, role);
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Authorization", "Bearer " + newJwt);
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

            if (AccountBLL.IsNRICValid(nric) && AccountBLL.IsPasswordValid(password) &&
                AccountBLL.IsDeviceIDValid(deviceID) && AccountBLL.IsTokenIDValid(tokenID))
            {
                Account account = accountBLL.GetStatus(nric, password, deviceID, tokenID);

                if (account.status == 1)
                {
                    try
                    {
                        accountBLL.MFADeviceIDUpdateFromPhone(nric, tokenID, deviceID);
                        string responseString = "Registration successful";
                        response = Request.CreateResponse(HttpStatusCode.OK, responseString);
                    }
                    catch
                    {
                        string responseString = "An error occured during registration.";
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, responseString);
                    }
                }

            }

            return response;
        }

        // POST api/account/register
        // Registers device for a user
        [Route("registertest")]
        [HttpPost]
        public HttpResponseMessage RegisterDevicetest()
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            HttpContext httpContext = HttpContext.Current;
            string authHeader = httpContext.Request.Headers["Authorization"];

            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string authHeaderValue = authHeader.Substring("Basic ".Length).Trim();
                string authHeaderValueDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue));
                string[] authHeaderParts = authHeaderValueDecoded.Split(':');
                string nric = authHeaderParts[0];
                string password = authHeaderParts[1];
                string deviceID = authHeaderParts[2];
                string tokenID = authHeaderParts[3];

                if (AccountBLL.IsNRICValid(nric) && AccountBLL.IsPasswordValid(password) &&
                AccountBLL.IsDeviceIDValid(deviceID) && AccountBLL.IsTokenIDValid(tokenID))
                {
                    Account account = accountBLL.GetStatus(nric, password, deviceID, tokenID);

                    if (account.status == 1)
                    {
                        try
                        {
                            accountBLL.MFADeviceIDUpdateFromPhone(nric, tokenID, deviceID);
                            string responseString = "Registration successful";
                            response = Request.CreateResponse(HttpStatusCode.OK, responseString);
                        }
                        catch
                        {
                            string responseString = "An error occured during registration.";
                            response = Request.CreateResponse(HttpStatusCode.InternalServerError, responseString);
                        }
                    }

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
                string authHeaderValue = authHeader.Substring("Basic ".Length).Trim();
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
                    HttpContext.Current.Cache.Insert(retrievedNRIC + "_MFAAttempt", account, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration);

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
        [Route("selectrole")]
        [HttpPost]
        public HttpResponseMessage SelectRole([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string newJwtRole = credentials.newJwtRole;
            string deviceID = credentials.deviceID;
            string jwt = credentials.jwt;

            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(newJwtRole) && !string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        Account account = accountBLL.GetStatus(retrievedNRIC);

                        if (account.status == 1)
                        {     
                            if (newJwtRole.Equals("10") && account.patientStatus == 1)
                            {
                                string newJwt = jwtBll.GetJWT(retrievedNRIC, newJwtRole);
                                response = Request.CreateResponse(HttpStatusCode.OK, newJwt);
                            }
                            else if (newJwtRole.Equals("01") && account.therapistStatus == 1)
                            {
                                string newJwt = jwtBll.GetJWT(retrievedNRIC, newJwtRole);
                                response = Request.CreateResponse(HttpStatusCode.OK, newJwt);
                            }

                            return response;
                        }
                    }
                }
            }

            return response;
        }

        // POST api/account/getallroles
        [Route("getallroles")]
        [HttpPost]
        public HttpResponseMessage GetAllRoles([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string deviceID = credentials.deviceID;
            string jwt = credentials.jwt;

            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        Account account = accountBLL.GetStatus(retrievedNRIC);

                        if (account.status == 1)
                        {
                            string role = account.patientStatus.ToString() + account.therapistStatus.ToString();
                            string newJwt = jwtBll.GetJWT(retrievedNRIC, role);
                            response = Request.CreateResponse(HttpStatusCode.OK, newJwt);
                            return response;
                        }
                    }
                }
            }

            return response;
        }

        // POST api/account/updatejwt
        [Route("updatejwt")]
        [HttpPost]
        public HttpResponseMessage UpdateJWT([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string deviceID = credentials.deviceID;
            string jwt = credentials.jwt;

            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        Account account = accountBLL.GetStatus(retrievedNRIC);

                        if (account.status == 1)
                        {
                            string newJwt = jwtBll.UpdateJWT(jwt);
                            response = Request.CreateResponse(HttpStatusCode.OK, newJwt);

                            return response;
                        }
                    }
                }
            }

            return response;
        }

    }
}