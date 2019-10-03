using NUSMed_WebApp.Classes.Entity;
using System;
using System.Configuration;
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

        public bool ValidateJWT(string jwt)
        {
            bool validated = false;

            RSA rsa = RSA.Create();
            rsa.FromXmlString(RSAFullKey);

            string[] jwtParts = jwt.Split('.');

            byte[] claimsBytes = Convert.FromBase64String(jwtParts[0]);
            byte[] signedJwtBytes = Convert.FromBase64String(jwtParts[1]);
            
            bool validSig = rsa.VerifyData(claimsBytes, signedJwtBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            if (validSig)
            {
                string claims = Encoding.UTF8.GetString(claimsBytes);
                JWT jwtEntity = JsonConvert.DeserializeObject<JWT>(claims);

                DateTime startTime = jwtEntity.creationTime;
                DateTime endTime = DateTime.Now;
                TimeSpan span = endTime.Subtract(startTime);

                if (span.Minutes >= 15)
                {
                    return validated;
                }

                // check if last password change time for an nric is after creationTime

                validated = true;
            }
            
            return validated;
        }

        public string getNRIC(string jwt)
        {
            string[] jwtParts = jwt.Split('.');

            byte[] claimsBytes = Convert.FromBase64String(jwtParts[0]);
            string claims = Encoding.UTF8.GetString(claimsBytes);
            JWT jwtEntity = JsonConvert.DeserializeObject<JWT>(claims);

            return jwtEntity.nric;
        }

        public string GetJWT(string nric, string role)
        {
            JWT jwtEntity = new JWT
            {
                nric = nric,
                Roles = role,
                creationTime = DateTime.Now
            };
            string jwtSerialized = JsonConvert.SerializeObject(jwtEntity);

            RSA rsa = RSA.Create();
            rsa.FromXmlString(RSAFullKey);
            
            byte[] jwtBytes = Encoding.UTF8.GetBytes(jwtSerialized);
            byte[] signedJwtBytes = rsa.SignData(jwtBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            string claims = Convert.ToBase64String(jwtBytes);
            string signature = Convert.ToBase64String(signedJwtBytes);

            string jwt = claims + "." + signature;
            return jwt;
        }

        public string UpdateJWT(string jwt)
        {
            string[] jwtParts = jwt.Split('.');

            byte[] claimsBytes = Convert.FromBase64String(jwtParts[0]);
            string claims = Encoding.UTF8.GetString(claimsBytes);
            JWT jwtEntity = JsonConvert.DeserializeObject<JWT>(claims);

            DateTime startTime = jwtEntity.creationTime;
            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime.Subtract(startTime);

            if (span.Minutes > 8 && span.Minutes < 15)
            {
                jwtEntity.creationTime = DateTime.Now;

                string jwtSerialized = JsonConvert.SerializeObject(jwtEntity);

                RSA rsa = RSA.Create();
                rsa.FromXmlString(RSAFullKey);

                byte[] jwtBytes = Encoding.UTF8.GetBytes(jwtSerialized);
                byte[] signedJwtBytes = rsa.SignData(jwtBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                claims = Convert.ToBase64String(jwtBytes);
                string signature = Convert.ToBase64String(signedJwtBytes);

                jwt = claims + "." + signature;
            }

            return jwt;
        }
    }
}