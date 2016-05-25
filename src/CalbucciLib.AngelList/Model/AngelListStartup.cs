using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListStartupMin
    {
        public int Id { get; set; }
        public bool Hidden { get; set; }

        public string Name { get; set; }
        [JsonProperty("angellist_url")]
        public string AngelListUrl { get; set; }
        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }
        [JsonProperty("product_desc")]
        public string ProductDescr { get; set; }
        [JsonProperty("follower_count")]
        public int FollowerCount { get; set; }
        [JsonProperty("company_url")]
        public string CompanyUrl { get; set; }
    }

    public class AngelListStartup : AngelListStartupMin
    {
        [JsonProperty("community_profile")]
        public bool CommunityProfile { get; set; }
        [JsonProperty("thumb_url")]
        public string ThumbUrl { get; set; }
        public int Quality { get; set; }
        [JsonProperty("high_concept")]
        public string HighConcept { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("twitter_url")]
        public string TwitterUrl { get; set; }
        [JsonProperty("blog_url")]
        public string BlogUrl { get; set; }
        [JsonProperty("video_url")]
        public string VideoUrl { get; set; }

        public List<AngelListMarket> Markets { get; set; }
        public List<AngelListLocation> Locations { get; set; }
        public AngelListStatus Status { get; set; }
        public List<AngelListScreenshot> Screenshots { get; set; }

        
    }

    public class AngelListStartupsPage : AngelListPagination
    {
        public List<AngelListStartupMin> Startups { get; set; }
    }
}
