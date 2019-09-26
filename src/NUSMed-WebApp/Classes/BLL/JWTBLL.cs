using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace NUSMed_WebApp.Classes.BLL
{
    public class JWTBLL
    {
        private readonly string RSAFullKey = ConfigurationManager.AppSettings["JWTRsaFullKey"].ToString()
            .Replace("< ?", "<").Replace("> ?", ">");

        public JWTBLL()
        {
        }

        public string validateJWT(string jwt)
        {
            bool validated = false;

            RSA rsa = RSA.Create();
            rsa.FromXmlString(RSAFullKey);

            String[] jwtParts = jwt.Split('.');

            byte[] claimsBytes = Convert.FromBase64String(jwtParts[0]);
            byte[] signedJwtBytes = Convert.FromBase64String(jwtParts[1]);
            
            bool validSig = rsa.VerifyData(claimsBytes, signedJwtBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            if (validSig)
            {
                String claims = Encoding.UTF8.GetString(claimsBytes);
                
                try
                {
                    JWT jwtEntity = JsonConvert.DeserializeObject<JWT>(claims);
                    return jwtEntity.creationTime.ToString();
                }
                catch (Exception e)
                {
                    return e.Message + "\n" + e.ToString() + "\n" + e.StackTrace;
                }
                
                /*DateTime startTime = jwtEntity.creationTime;
                DateTime endTime = DateTime.Now;
                TimeSpan span = endTime.Subtract(startTime);

                if (span.Minutes >= 15)
                {
                    return validated;
                }*/

                // check if last password change time for an nric is after creationTime

                validated = true;
            }
            
            return "test";
        }

        public string getNRIC(string jwt)
        {
            String[] jwtParts = jwt.Split('.');

            byte[] claimsBytes = Convert.FromBase64String(jwtParts[0]);
            String claims = Encoding.UTF8.GetString(claimsBytes);
            JWT jwtEntity = (JWT)JsonConvert.DeserializeObject(claims);

            return jwtEntity.nric;
        }

        public string getJWT(string nric, string role)
        {
            JWT jwtEntity = new JWT
            {
                nric = nric,
                Roles = role,
                creationTime = DateTime.Now
            };
            String jwtSerialized = JsonConvert.SerializeObject(jwtEntity);

            RSA rsa = RSA.Create();
            rsa.FromXmlString(RSAFullKey);
            
            byte[] jwtBytes = Encoding.UTF8.GetBytes(jwtSerialized);
            byte[] signedJwtBytes = rsa.SignData(jwtBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            String claims = Convert.ToBase64String(jwtBytes);
            String signature = Convert.ToBase64String(signedJwtBytes);

            String jwt = claims + "." + signature;
            return jwt;
        }

        public string updateJWT(string jwt)
        {
            String[] jwtParts = jwt.Split('.');

            byte[] claimsBytes = Convert.FromBase64String(jwtParts[0]);
            String claims = Encoding.UTF8.GetString(claimsBytes);
            JWT jwtEntity = (JWT)JsonConvert.DeserializeObject(claims);

            DateTime startTime = jwtEntity.creationTime;
            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime.Subtract(startTime);

            if (span.Minutes > 8 && span.Minutes < 15)
            {
                jwtEntity.creationTime = DateTime.Now;

                String jwtSerialized = JsonConvert.SerializeObject(jwtEntity);

                RSA rsa = RSA.Create();
                rsa.FromXmlString(RSAFullKey);

                byte[] jwtBytes = Encoding.UTF8.GetBytes(jwtSerialized);
                byte[] signedJwtBytes = rsa.SignData(jwtBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                claims = Convert.ToBase64String(jwtBytes);
                String signature = Convert.ToBase64String(signedJwtBytes);

                jwt = claims + "." + signature;
            }

            return jwt;
        }
    }
}