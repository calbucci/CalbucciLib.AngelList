using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public AngelListAuth(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
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

            string data =
                $"client_id={ClientId}&client_secret={ClientSecret}&grant_type=authorization_code&code={HttpUtility.UrlEncode(code)}";
                                 

            string exchangeUrl =
                "https://angel.co/api/oauth/token";

            try
            {
                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Headers[HttpRequestHeader.UserAgent] = "ListpediaBot";
                    wc.Headers[HttpRequestHeader.Accept] = "*/*";

                    var resp = wc.UploadString(exchangeUrl, data);

                    var alt = JsonConvert.DeserializeObject<AngelListToken>(resp);
                    return string.IsNullOrWhiteSpace(alt?.AccessToken) ? null : alt;
                }
            }
            catch (WebException wex)
            {
                Debug.WriteLine(wex);

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

    }

}
