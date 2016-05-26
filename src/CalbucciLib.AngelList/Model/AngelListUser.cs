using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListUserMin
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Bio { get; set; }
        [JsonProperty("follower_count")]
        public int FollowerCount { get; set; }
        [JsonProperty("angellist_url")]
        public string AngelListUrl { get; set; }
        public string Image { get; set; }

    }

    public class AngelListUser : AngelListUserMin
    {
        public string Email { get; set; }
        [JsonProperty("blog_url")]
        public string BlogUrl { get; set; }
        [JsonProperty("online_bio_url")]
        public string OnlineBioUrl { get; set; }
        [JsonProperty("facebook_url")]
        public string FacebookUrl { get; set; }
        [JsonProperty("twitter_url")]
        public string TwitterUrl { get; set; }
        [JsonProperty("aboutme_url")]
        public string AboutMeUrl { get; set; }
        [JsonProperty("github_url")]
        public string GithubUrl { get; set; }
        [JsonProperty("dribble_url")]
        public string DribbleUrl { get; set; }
        [JsonProperty("behance_url")]
        public string BehanceUrl { get; set; }
        [JsonProperty("resume_url")]
        public string ResumeUrl { get; set; }
        [JsonProperty("what_ive_built")]
        public string WhatIveBuilt { get; set; }
        [JsonProperty("what_i_do")]
        public string WhatIDo { get; set; }
        public string Criteria { get; set; }
        [JsonProperty("linkedin_url")]
        public string LinkedInUrl { get; set; }
        public bool Investor { get; set; }
        public List<AngelListLocation> Locations { get; set; }
        public List<AngelListRole> Roles { get; set; }
        public List<AngelListSkill> Skills { get; set; }
        public List<string> Scopes { get; set; }


    }

}
