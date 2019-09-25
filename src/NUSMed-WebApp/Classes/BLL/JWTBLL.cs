using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.BLL
{
    public class JWTBLL
    {
        private readonly string privateKey;
        private readonly string publicKey;

        public JWTBLL()
        {
            privateKey = ConfigurationManager.AppSettings["JWTSignaturePrivateKey"].ToString();
            publicKey = ConfigurationManager.AppSettings["JWTSignaturePublicKey"].ToString();
        }
    }
}