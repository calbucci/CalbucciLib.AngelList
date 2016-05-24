using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListTag
    {
        public class AngelListRole
        {
            public int Id { get; set; }
            [JsonProperty("tag_type")]
            public string TagType { get; set; }
            public string Name { get; set; }
            [JsonProperty("display_name")]
            public string DisplayName { get; set; }
            [JsonProperty("angellist_url")]
            public string AngelListUrl { get; set; }

        }

    }
}
