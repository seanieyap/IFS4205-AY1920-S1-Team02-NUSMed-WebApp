using System;

namespace NUSMed_WebApp.Classes.Entity
{
    public class JWT
    {
        private string _nric;
        public string nric
        {
            get
            {
                if (string.IsNullOrEmpty(_nric))
                {
                    return _nric;
                }
                return _nric.ToUpper();
            }
            set
            {
                _nric = value.ToUpper();
            }
        }
        public string Roles { get; set; } = null;
        public DateTime creationTime { get; set; }
    }
}