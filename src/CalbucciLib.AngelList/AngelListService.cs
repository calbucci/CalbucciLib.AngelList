using System;
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

        public List<AngelListUser> ListUsers(IEnumerable<int> userIds)
        {
            return BatchGet(userIds, GetUser);
        }

        public List<AngelListUser> ListUsers(IEnumerable<string> slugs)
        {
            return BatchGet(slugs, GetUser);
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

        public List<AngelListStartup> ListStartups(IEnumerable<int> startupIds)
        {
            return BatchGet(startupIds, GetStartup);
        }

        public List<AngelListSearchResult> ListStartups(IEnumerable<string> slugs)
        {
            return BatchGet(slugs, GetStartup);
        }
        // ====================================================================
        //
        //  Follows
        //
        // ====================================================================
        public AngelListFollowUser FollowUser(int userId)
        {
            var qs = new Dictionary<string, object>
            {
                {"type", "user"},
                {"id", userId}
            };

            // Unfortunately, sending a follow request if you are already 
            // following a user returns "BadRequest" 
            var ret = ExecutePost<AngelListFollowUser>("/follows", qs);
            return ret;

        }

        public bool UnfollowUser(int userId)
        {
            var qs = new Dictionary<string, object>
            {
                {"type", "user"},
                {"id", userId}
            };
            return ExecuteDelete<AngelListFollowUser>("/follows", qs) != null;
        }

        public bool IsFollowingUser(int sourceUserId, int targetUserId)
        {
            var qs = new Dictionary<string, object>
            {
                {"source_id", sourceUserId},
                {"target_type", "User"},
                {"target_id", targetUserId}
            };
            var ret = ExecuteGet<AngelListRelationship>("/follows/relationship", qs);
            return ret != null && ret.Source != null;
        }

        public List<int> ListFollowerUserIds(int userId)
        {
            return PagedGet<int>($"/users/{userId}/followers/ids");
        }

        public List<AngelListUserMin> ListFollowerUsers(int userId)
        {
            return PagedGet<AngelListUserMin>($"/users/{userId}/followers");
        }

        public List<int> ListFollowingUserIds(int userId)
        {
            return PagedGet<int>($"/users/{userId}/following/ids");
        }

        public List<AngelListUserMin> ListFollowingUsers(int userId)
        {
            return PagedGet<AngelListUserMin>($"/users/{userId}/following");
        }


        public AngelListFollowStartup FollowStartup(int startupId)
        {
            var qs = new Dictionary<string, object>
            {
                {"type", "startup"},
                {"id", startupId}
            };
            return ExecutePost<AngelListFollowStartup>("/follows", qs);
        }

        public bool UnfollowStartup(int startupId)
        {
            var qs = new Dictionary<string, object>
            {
                {"type", "startup"},
                {"id", startupId}
            };
            return ExecuteDelete<AngelListFollowStartup>("/follows", qs) != null;
        } 

        public bool IsFollowingStartup(int userId, int startupId)
        {
            var qs = new Dictionary<string, object>
            {
                {"source_id", userId},
                {"target_type", "Startup"},
                {"target_id", startupId}
            };
            var ret = ExecuteGet<AngelListRelationship>("/follows/relationship", qs);
            return ret != null && ret.Source != null;
        }


        public List<int> ListFollowingStartupIds(int userId)
        {
            return PagedGet<int>($"/users/{userId}/following/ids", "startup");
        }

        public List<AngelListStartupMin> ListFollowingStartups(int userId)
        {
            return PagedGet<AngelListStartupMin>($"/users/{userId}/following", "startup");
        }

        public List<AngelListUserMin> ListStartupFollowers(int startupId)
        {
            return PagedGet<AngelListUserMin>($"/startups/{startupId}/followers");
        }

        public List<int> ListStartupFollowerIds(int startupId)
        {
            return PagedGet<int>($"/startups/{startupId}/followers/ids");
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
        //  Batch & Page helpers
        //
        // ====================================================================
        public List<T> BatchGet<T, K>(IEnumerable<K> items, Func<K, T> method) where T : class
        {
            if (items == null || !items.Any())
                return new List<T>();

            var tasks = new List<Task<T>>();
            foreach (var item in items)
            {
                var item2 = item; // avoid closure issues

                // BUG: This class is not thread-safe due to LastXYZ properties, but on 
                //      this particular call we don't care.
                var task = Task.Run<T>(() =>
                {
                    try
                    {
                        var alu = method(item2);
                        return alu;
                    }
                    catch { return null; }
                });
                tasks.Add(task);
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch { }

            var results = tasks.Where(t => t.IsCompleted && !t.IsFaulted && !t.IsCanceled)
                .Select(t => t.Result).Where(r => r != null).ToList();

            return results;
        }

        public List<T> PagedGet<T>(string endPoint, string type = null)
        {
            // TODO: Since AngelList uses numbered pages instead of a cursor, we could just
            //       ask for all the pages at the same time and improve on the async calls
            List<T> results = new List<T>();
            int pageNum = 1;
            do
            {
                var qs = new Dictionary<string, object> { ["page"] = pageNum };
                if (type != null)
                    qs["type"] = type;
                var page = ExecuteGet<AngelListPagination<T>>(endPoint, qs);
                if (page?.Items == null || !page.Items.Any())
                    break;

                results.AddRange(page.Items);
                pageNum = page.Page + 1;
                if (pageNum > page.LastPage)
                    break;
            } while (true);

            return results;

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
