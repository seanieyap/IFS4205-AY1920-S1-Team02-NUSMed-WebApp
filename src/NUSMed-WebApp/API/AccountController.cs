using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Net;
using System.Net.Http;
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

        // POST api/account/weblogin
        [Route("weblogin")]
        [HttpPost]
        public HttpResponseMessage WebLogin([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            string deviceID = credentials.deviceID;
            string tokenID = credentials.tokenID;
            string jwt = credentials.jwt;
            //string guid = credentials.guid;

            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID) && AccountBLL.IsTokenIDValid(tokenID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    // validate deviceID and tokenID for a nric
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
                            response = Request.CreateResponse(HttpStatusCode.OK, newJwt);
                        }
                        else
                        {
                            // MFA login was not trigger before this request, cache not inserted
                            response = Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                    }
                }
            }

            /*if (HttpContext.Current.Cache[guid] != null)
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
            }*/

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