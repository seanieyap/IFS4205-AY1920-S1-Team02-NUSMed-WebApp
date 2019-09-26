using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.BLL
{
    public class JWTBLL
    {
        private readonly string RSAFullKey;

        public JWTBLL()
        {
            RSAFullKey = ConfigurationManager.AppSettings["JWTRsaFullKey"].ToString();
        }
    }
}