using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListFollowUser
    {
        public long Id { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        public AngelListUserMin Follower { get; set; }
        public AngelListUserMin Followed { get; set; }
    }

    public class AngelListFollowStartup
    {
        public long Id { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        public AngelListUserMin Follower { get; set; }
        public AngelListStartupMin Followed { get; set; }

    }
}
