using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListUser
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Bio { get; set; }
        [JsonProperty("blog_url")]
        public string BlogUrl { get; set; }
        [JsonProperty("online_bio_url")]
        public string OnlineBioUrl { get; set; }
        [JsonProperty("facebook_url")]
        public string FacebookUrl { get; set; }
        [JsonProperty("linkedin_url")]
        public string LinkedInUrl { get; set; }
        [JsonProperty("follower_count")]
        public int FollowerCount { get; set; }
        public bool Investor { get; set; }
        public AngelListLocation[] Locations { get; set; }
        public AngelListRole[] Roles { get; set; }
        [JsonProperty("angellist_url")]
        public string AngelListUrl { get; set; }
        public string Image { get; set; }       
    }
}
