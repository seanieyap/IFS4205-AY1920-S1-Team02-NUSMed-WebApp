﻿using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace NUSMed_WebApp.API
{
    [RoutePrefix("api/record")]
    public class RecordController : ApiController
    {
        private readonly RecordBLL recordBLL = new RecordBLL();

        // POST api/record/patient/upload
        // Patient uploads a record for himself
        [Route("patient/upload")]
        [HttpPost]
        public HttpResponseMessage PatientUpload([FromBody]dynamic credentials)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            string deviceID = credentials.deviceID;
            string jwt = credentials.jwt;

            AccountBLL accountBLL = new AccountBLL();
            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        accountBLL.SetRole(retrievedNRIC, "Patient");
                        Account account = accountBLL.GetStatus(retrievedNRIC);

                        if (account.status == 1)
                        {
                            try
                            {
                                Record record = new Record();
                                record.patientNRIC = retrievedNRIC;
                                record.creatorNRIC = retrievedNRIC;
                                record.title = credentials.title;
                                record.description = credentials.description;
                                record.type = RecordType.Get(Convert.ToString(credentials.type));
                                record.content = string.Empty;

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
                                    record.content = credentials.content;

                                    if (!record.IsContentValid())
                                    {
                                        return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid record content");
                                    }
                                }
                                else
                                {
                                    record.fileName = credentials.fileName;
                                    record.fileExtension = credentials.fileExtension;
                                    byte[] fileContent = Convert.FromBase64String(Convert.ToString(credentials.fileContent));
                                    record.fileSize = fileContent.Length;

                                    if (Convert.ToInt64(record.fileSize) > Convert.ToInt64(credentials.fileSize))
                                    {
                                        return Request.CreateResponse(HttpStatusCode.Forbidden, "Record file size mismatch");
                                    }

                                    if (!record.IsFileValid())
                                    {
                                        return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid record file");
                                    }

                                    record.createTime = DateTime.Now;

                                    Directory.CreateDirectory(record.GetFileServerPath() + "\\" + record.GetFileDirectoryNameHash());

                                    File.WriteAllBytes(record.fullpath, fileContent);
                                }

                                recordBLL.AddRecord(record);

                                response = Request.CreateResponse(HttpStatusCode.OK, jwtBll.UpdateJWT(jwt));
                            }
                            catch
                            {
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                            }

                            return response;
                        }
                    }
                }
            }

            return response;
        }

        // POST api/record/therapist/getPatients
        // Therapist get all associated patients
        [Route("therapist/getPatients")]
        [HttpPost]
        public HttpResponseMessage TherapistGetPatients([FromBody]dynamic credentials)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            string deviceID = credentials.deviceID;
            string jwt = credentials.jwt;

            AccountBLL accountBLL = new AccountBLL();
            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        accountBLL.SetRole(retrievedNRIC, "Therapist");
                        Account account = accountBLL.GetStatus(retrievedNRIC);

                        if (account.status == 1)
                        {
                            try
                            {
                                TherapistBLL therapistBLL = new TherapistBLL();
                                List<Classes.Entity.Patient> patients = therapistBLL.GetCurrentPatients("");

                                string names = "";
                                for (int i = 0; i < patients.Count; i++)
                                {
                                    names += patients[i].nric + "  " + patients[i].firstName + " " + patients[i].lastName;

                                    if (i != patients.Count - 1)
                                    {
                                        names += "\r";
                                    }
                                }

                                response = Request.CreateResponse(HttpStatusCode.OK, System.Convert.ToBase64String(Encoding.ASCII.GetBytes(names)));
                            }
                            catch
                            {
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                            }

                            return response;
                        }
                    }
                }
            }

            return response;
        }

        // POST api/record/therapist/scanPatient
        // Therapist scan NFC for emergancy patient
        [Route("therapist/scanPatient")]
        [HttpPost]
        public HttpResponseMessage TherapistScanPatient([FromBody]dynamic credentials)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            string deviceID = credentials.deviceID;
            string jwt = credentials.jwt;

            string tokenID = credentials.tokenID;

            AccountBLL accountBLL = new AccountBLL();
            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        accountBLL.SetRole(retrievedNRIC, "Therapist");
                        Account account = accountBLL.GetStatus(retrievedNRIC);

                        if (account.status == 1)
                        {
                            try
                            {
                                TherapistBLL therapistBLL = new TherapistBLL();
                                bool check = therapistBLL.AcceptEmergencyPatient(tokenID);

                                if (check)
                                {
                                    response = Request.CreateResponse(HttpStatusCode.OK, jwtBll.UpdateJWT(jwt));
                                }
                                else
                                {
                                    response = Request.CreateResponse(HttpStatusCode.Forbidden);
                                }
                            }
                            catch
                            {
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                            }

                            return response;
                        }
                    }
                }
            }

            return response;
        }

        // POST api/record/therapist/upload
        // Therapist uploads a record for his patient
        [Route("therapist/upload")]
        [HttpPost]
        public HttpResponseMessage TherapistUpload([FromBody]dynamic credentials)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Unauthorized);

            string deviceID = credentials.deviceID;
            string jwt = credentials.jwt;

            AccountBLL accountBLL = new AccountBLL();
            JWTBLL jwtBll = new JWTBLL();

            if (!string.IsNullOrEmpty(jwt) && AccountBLL.IsDeviceIDValid(deviceID))
            {
                if (jwtBll.ValidateJWT(jwt))
                {
                    string retrievedNRIC = jwtBll.getNRIC(jwt);

                    if (accountBLL.IsValid(retrievedNRIC, deviceID))
                    {
                        accountBLL.SetRole(retrievedNRIC, "Therapist");
                        Account account = accountBLL.GetStatus(retrievedNRIC);

                        if (account.status == 1)
                        {
                            try
                            {
                                Classes.Entity.Patient patient = new TherapistBLL().GetPatientPermissions(Convert.ToString(credentials.patientNRIC));

                                Record record = new Record();
                                record.patientNRIC = credentials.patientNRIC;
                                record.creatorNRIC = retrievedNRIC;
                                record.title = credentials.title;
                                record.description = credentials.description;
                                record.type = RecordType.Get(Convert.ToString(credentials.type));
                                record.isEmergency = patient.isEmergency;
                                record.content = string.Empty;

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
                                    record.content = credentials.content;

                                    if (!record.IsContentValid())
                                    {
                                        return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid record content");
                                    }
                                }
                                else
                                {
                                    record.fileName = credentials.fileName;
                                    record.fileExtension = credentials.fileExtension;
                                    byte[] fileContent = Convert.FromBase64String(Convert.ToString(credentials.fileContent));
                                    record.fileSize = fileContent.Length;

                                    if (Convert.ToInt64(record.fileSize) > Convert.ToInt64(credentials.fileSize))
                                    {
                                        return Request.CreateResponse(HttpStatusCode.Forbidden, "Record file size mismatch");
                                    }

                                    if (!record.IsFileValid())
                                    {
                                        return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid record file");
                                    }

                                    record.createTime = DateTime.Now;

                                    Directory.CreateDirectory(record.GetFileServerPath() + "\\" + record.GetFileDirectoryNameHash());

                                    File.WriteAllBytes(record.fullpath, fileContent);
                                }

                                recordBLL.AddRecord(record);

                                response = Request.CreateResponse(HttpStatusCode.OK, jwtBll.UpdateJWT(jwt));
                            }
                            catch
                            {
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                            }

                            return response;
                        }
                    }
                }
            }

            return response;
        }
    }
}