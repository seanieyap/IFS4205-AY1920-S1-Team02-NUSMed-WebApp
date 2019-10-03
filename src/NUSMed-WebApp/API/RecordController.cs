using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Security;

namespace NUSMed_WebApp.API
{
    [RoutePrefix("api/record")]
    public class RecordController : ApiController
    {
        // POST api/record/patient/upload
        // Patient uploads a record for himself
        [Route("patient/upload")]
        [HttpPost]
        public HttpResponseMessage PatientUpload([FromBody]dynamic credentials)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            string deviceID = credentials.deviceID;
            string jwt = credentials.jwt;

            AccountBLL accountBLL = new AccountBLL();
            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.validateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        Account account = accountBLL.GetStatus(retrievedNRIC);

                        if (account.status == 1)
                        {
                            Record record = new Record();
                            record.patientNRIC = retrievedNRIC;
                            record.title = credentials.title;
                            record.description = credentials.description;
                            record.type = RecordType.Get(Convert.ToString(credentials.type));
                            record.content = credentials.content;

                            if (!record.IsTitleValid())
                            {
                                return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid record title");
                            }

                            if (!record.IsDescriptionValid())
                            {
                                return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid record description");
                            }

                            if (record.type.isContent)
                            {
                                if (!record.IsContentValid())
                                {
                                    return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid record content");
                                }
                            }
                            else
                            {
                                record.fileName = credentials.fileName;
                                record.fileExtension = credentials.fileExtension;
                                record.fileSize = credentials.fileSize;

                                if (!record.IsFileValid())
                                {
                                    return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid record file");
                                }
                            }

                            if (!record.type.isContent)
                            {
                                record.createTime = DateTime.Now;

                                Directory.CreateDirectory(record.GetFileServerPath() + "\\" + record.GetFileDirectoryNameHash());

                                File.WriteAllBytes(record.fullpath, Encoding.ASCII.GetBytes(Convert.ToString(credentials.fileContent)));
                            }
                            RecordBLL recordBLL = new RecordBLL();
                            recordBLL.AddRecord(record);

                            string role = account.patientStatus.ToString() + account.therapistStatus.ToString();
                            string newJwt = jwtBll.getJWT(retrievedNRIC, role);
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