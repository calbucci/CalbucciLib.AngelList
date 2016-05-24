using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CalbucciLib.AngelList.Model;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList
{
    public class AngelListAuth
    {

        // ====================================================================
        //
        //  Constructor
        //
        // ====================================================================

        public AngelListAuth(string clientId, string clientSecret, string redirectUri)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
        }


        public string GetAuthorizationUrl(AngelListScope scopes = 0, string state = null)
        {
            // [ThreadStatic] public static InstagramAuthError _LastError;
            // https://angel.co/api/oauth/authorize?client_id =...&state =...&response_type = code

            StringBuilder sb = new StringBuilder(250);
            sb.Append("https://angel.co/api/oauth/authorize?response_type=code&client_id=");
            sb.Append(HttpUtility.UrlEncode(ClientId));
            if (scopes > 0 || DefaultScopes > 0)
            {
                sb.Append("&scope=");
                if (scopes == 0)
                    scopes = DefaultScopes;
                sb.Append(ScopesToString(scopes));
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                sb.Append("&state=");
                sb.Append(HttpUtility.UrlEncode(state));
            }

            return sb.ToString();
        }

        public AngelListToken ExchangeToken(Uri redirectUri)
        {
            //_LastError = null;

            /*
            POST https://angel.co/api/oauth/token?
                 client_id=...&
                 client_secret=...&
                 code=...&
                 grant_type=authorization_code
                 */

            var qss = redirectUri.Query;

            var qs = HttpUtility.ParseQueryString(qss.Substring(qss.IndexOf('?')).Split('#')[0]);

            string code = qs["code"];
            if (string.IsNullOrEmpty(code))
                return null;

            string exchangeUrl =
                $"https://angel.co/api/oath/token?client_id={ClientId}&client_secret={ClientSecret}&grant_type=authorization_code&code="
                                 + HttpUtility.UrlEncode(code);

            string data = null;
            try
            {
                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                    var resp = wc.UploadString(exchangeUrl, "");

                    var alt = JsonConvert.DeserializeObject<AngelListToken>(resp);
                    if (string.IsNullOrWhiteSpace(alt?.AccessToken))
                        return null;

                    return alt;
                }
            }
            catch (WebException wex)
            {

                return null;
            }
        }



        // ====================================================================
        //
        //  Helpers
        //
        // ====================================================================
        protected string ScopesToString(AngelListScope scope)
        {
            if (scope == 0)
                return "";

            List<string> items = new List<string>();
            if ((scope & AngelListScope.Comment) > 0)
                items.Add("comment");
            if ((scope & AngelListScope.Email) > 0)
                items.Add("email");
            if ((scope & AngelListScope.Message) > 0)
                items.Add("message");

            return string.Join("%20", items);
        }


        // ====================================================================
        //
        //  Properties
        //
        // ====================================================================

        public AngelListScope DefaultScopes { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }

    }

}