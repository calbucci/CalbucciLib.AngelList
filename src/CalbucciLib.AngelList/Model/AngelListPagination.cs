using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListPagination
    {
        public int Total { get; set; }
        [JsonProperty("per_page")]
        public int PerPage { get; set; }
        public int Page { get; set; }
        [JsonProperty("last_page")]
        public int LastPage { get; set; }
    }

    public class AngelListIdsPage : AngelListPagination
    {
        public List<int> Ids { get; set; }
    }
}
