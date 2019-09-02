using Ganss.XSS;

namespace NUSMed_WebApp.Classes.Common
{
    public class Sanitize
    {
        public static string Clean(string html)
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            return sanitizer.Sanitize(html).ToString();
        }
    }
}