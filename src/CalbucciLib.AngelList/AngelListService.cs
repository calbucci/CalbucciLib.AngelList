﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CalbucciLib.AngelList.Model;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList
{
    public class AngelListService
    {
        public HttpStatusCode LastStatusCode { get; private set; }
        public string LastDataJson { get; private set; }
        public object LastData { get; private set; }

        // ====================================================================
        //
        //   Constructor
        //
        // ====================================================================
        public AngelListService(string authToken)
        {
            AuthToken = authToken;
        }

        // ====================================================================
        //
        //   Users
        //
        // ====================================================================
        public AngelListUser GetMe()
        {
            return ExecuteGet<AngelListUser>("/me");
        }

        public AngelListUser GetUser(int id)
        {
            return ExecuteGet<AngelListUser>("/users/" + id);
        }

        public AngelListUser GetUser(string slug)
        {
            var qs = new Dictionary<string, object>()
            {
                {"slug", slug},
            };
            return ExecuteGet<AngelListUser>("/users/search", qs);
        }

        public AngelListUser GetUserByEmail(string email)
        {
            email = email.ToLower().Trim();
            var md5 = ComputeMD5(email);

            var qs = new Dictionary<string, object>()
            {
                {"md5", md5},
            };
            return ExecuteGet<AngelListUser>("/users/search", qs);
        }

        // ====================================================================
        //
        //  Startups
        //
        // ====================================================================
        public AngelListStartup GetStartup(int id)
        {
            return ExecuteGet<AngelListStartup>("/startups/" + id);
        }

        public AngelListSearchResult GetStartup(string slug)
        {
            return SearchSlug(slug, AngelListEntityType.Startup);
        }

        // ====================================================================
        //
        //  Follows
        //
        // ====================================================================
        public bool FollowUser(int userId)
        {
            var qs = new Dictionary<string, object>
            {
                {"type", "user"},
                {"id", userId}
            };
            return ExecutePost<string>("/follows", qs) != null;
        }

        public bool UnfollowUser(int userId)
        {
            var qs = new Dictionary<string, object>
            {
                {"type", "user"},
                {"id", userId}
            };
            return ExecuteDelete<string>("/follows", qs) != null;
        }

        public bool IsFollowingUser(int sourceUserId, int targetUserId)
        {
            var qs = new Dictionary<string, object>
            {
                {"source_id", sourceUserId},
                {"target_type", "user"},
                {"target_id", targetUserId}
            };
            var ret = ExecuteGet<AngelListRelationship>("/follows/relationship", qs);
            return ret != null && ret.Source != null;
        }

        public AngelListIdsPage ListFollowerUserIds(int userId, int page = 1)
        {
            Dictionary<string, object> qs = null;
            if (page > 1)
            {
                qs = new Dictionary<string, object> {["page"] = page};
            }
            return ExecuteGet<AngelListIdsPage>($"/users/{userId}/followers/ids", qs);
        }

        public AngelListUsersPage LisFollowerUsers(int userId, int page = 1)
        {
            Dictionary<string, object> qs = null;
            if (page > 1)
            {
                qs = new Dictionary<string, object> {["page"] = page};
            }
            return ExecuteGet<AngelListUsersPage>($"/users/{userId}/followers", qs);
        }

        public AngelListIdsPage ListFollowingUserIds(int userId, int page = 1)
        {
            Dictionary<string, object> qs = new Dictionary<string, object>()
            {
                {"type", "user"}
            };
            if (page > 1)
            {
                qs["page"] = page;
            }
            return ExecuteGet<AngelListIdsPage>($"/users/{userId}/following/ids", qs);
        }

        public AngelListUsersPage LisFollowingUsers(int userId, int page = 1)
        {
            Dictionary<string, object> qs = new Dictionary<string, object>()
            {
                {"type", "user"}
            };
            if (page > 1)
            {
                qs["page"] = page;
            }
            return ExecuteGet<AngelListUsersPage>($"/users/{userId}/following", qs);
        }


        public bool FollowStartup(int startupId)
        {
            var qs = new Dictionary<string, object>
            {
                {"type", "startup"},
                {"id", startupId}
            };
            return ExecutePost<string>("/follows", qs) != null;
        }

        public bool UnfollowStartup(int startupId)
        {
            var qs = new Dictionary<string, object>
            {
                {"type", "startup"},
                {"id", startupId}
            };
            return ExecuteDelete<string>("/follows", qs) != null;
        }

        public bool IsFollowingStartup(int userId, int startupId)
        {
            var qs = new Dictionary<string, object>
            {
                {"source_id", userId},
                {"target_type", "user"},
                {"target_id", startupId}
            };
            var ret = ExecuteGet<AngelListRelationship>("/follows/relationship", qs);
            return ret != null && ret.Source != null;
        }


        public AngelListIdsPage ListFollowingStartupIds(int userId, int page = 1)
        {
            var qs = new Dictionary<string, object>()
            {
                {"type", "startup"}
            };
            if (page > 1)
            {
                qs["page"] = page;
            }
            return ExecuteGet<AngelListIdsPage>($"/users/{userId}/following/ids", qs);
        }

        public AngelListStartupsPage LisFollowingStartups(int userId, int page = 1)
        {
            var qs = new Dictionary<string, object>()
            {
                {"type", "startup"}
            };
            if (page > 1)
            {
                qs["page"] = page;
            }
            return ExecuteGet<AngelListStartupsPage>($"/users/{userId}/following", qs);
        }

        public AngelListUsersPage ListStartupFollowers(int startupId, int page = 1)
        {
            Dictionary<string, object> qs = null;
            if (page > 1)
            {
                qs = new Dictionary<string, object> { ["page"] = page };
            }
            return ExecuteGet<AngelListUsersPage>($"/startups/{startupId}/followers", qs);
        }

        public AngelListIdsPage ListStartupFollowerIds(int startupId, int page = 1)
        {
            Dictionary<string, object> qs = null;
            if (page > 1)
            {
                qs = new Dictionary<string, object> { ["page"] = page };
            }
            return ExecuteGet<AngelListIdsPage>($"/startups/{startupId}/followers/ids", qs);
        }


        // ====================================================================
        //
        //  Search
        //
        // ====================================================================
        public List<AngelListSearchResult> Search(string query, AngelListEntityType type)
        {
            var qs = new Dictionary<string, object>
            {
                {"query", query},
                {"type", type}
            };
            return ExecuteGet<List<AngelListSearchResult>>("/search", qs);
        }

        public AngelListSearchResult SearchSlug(string slug, AngelListEntityType type)
        {
            var qs = new Dictionary<string, object>
            {
                {"query", slug},
                {"type", type}
            };
            return ExecuteGet<AngelListSearchResult>("/search/slugs", qs);
        }



        // ====================================================================
        //
        //  Internals
        //
        // ====================================================================


        protected string BuildUrl(string endpoint, Dictionary<string, object> qs)
        {
            StringBuilder sb = new StringBuilder(120);
            sb.Append("https://api.angel.co/1");
            sb.Append(endpoint);
            sb.Append("?access_token=");
            sb.Append(HttpUtility.UrlEncode(AuthToken));
            if (qs != null && qs.Count > 0)
            {
                sb.Append('&');
                sb.Append(BuildFormEncoded(qs));
            }
            return sb.ToString();
        }

        protected string BuildFormEncoded(Dictionary<string, object> qs)
        {
            if (qs == null || qs.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder(50 * qs.Count);

            foreach (var kp in qs)
            {
                if (sb.Length > 0)
                    sb.Append('&');
                sb.AppendFormat("{0}={1}", kp.Key, HttpUtility.UrlEncode(kp.Value?.ToString() ?? ""));
            }
            return sb.ToString();
        }

        protected void ClearLasts()
        {
            LastData = null;
            LastDataJson = null;
            LastStatusCode = HttpStatusCode.InternalServerError;
        }

        protected T ParseResponse<T>(string json)
        {
            LastDataJson = json;
            var r = JsonConvert.DeserializeObject<T>(json);
            LastData = r;
            return r;
        }

        protected T ExecuteGetByUrl<T>(string url) where T : class
        {
            ClearLasts();
            try
            {
                using (var wc = new WebClient())
                {
                    var resp = wc.DownloadString(url);
                    return ParseResponse<T>(resp);
                }
            }
            catch (WebException wex)
            {
                LastStatusCode = ((HttpWebResponse)wex.Response).StatusCode;
                return null;
            }
        }

        protected T ExecuteGet<T>(string endpoint, Dictionary<string, object> qs = null) where T : class
        {
            var url = BuildUrl(endpoint, qs);
            return ExecuteGetByUrl<T>(url);
        }


        protected T ExecutePost<T>(string endpoint, Dictionary<string, object> form) where T : class
        {
            ClearLasts();
            var url = BuildUrl(endpoint, null);
            try
            {
                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string postData = BuildFormEncoded(form);

                    string resp = wc.UploadString(url, postData);
                    return ParseResponse<T>(resp);
                }
            }
            catch (WebException wex)
            {
                LastStatusCode = ((HttpWebResponse)wex.Response).StatusCode;
                return null;
            }
        }


        protected T ExecuteDelete<T>(string endpoint, Dictionary<string, object> qs = null) where T : class
        {
            ClearLasts();
            var url = BuildUrl(endpoint, qs);
            try
            {
                using (var wc = new WebClient())
                {
                    string resp = wc.UploadString(url, "DELETE", "");
                    return ParseResponse<T>(resp);
                }
            }
            catch (WebException wex)
            {
                LastStatusCode = ((HttpWebResponse)wex.Response).StatusCode;
                return null;
            }
        }


        private static string ComputeMD5(string text)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                string encoded = BitConverter.ToString(hashBytes)
                        .Replace("-", string.Empty)
                        .ToLower();

                return encoded;

                //// Convert the byte array to hexadecimal string
                //StringBuilder sb = new StringBuilder();
                //for (int i = 0; i < hashBytes.Length; i++)
                //{
                //    sb.Append(hashBytes[i].ToString("x2"));
                //}
                //return sb.ToString();
            }
        }
        // ====================================================================
        //
        //  Properties
        //
        // ====================================================================
        public string AuthToken { get; set; }
    }
}
