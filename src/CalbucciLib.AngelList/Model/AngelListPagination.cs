using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListPagination<T>
    {
        public int Total { get; set; }
        [JsonProperty("per_page")]
        public int PerPage { get; set; }
        public int Page { get; set; }
        [JsonProperty("last_page")]
        public int LastPage { get; set; }
        public List<T> Items { get; set; }


        // Hackaroos: Each type of page uses a different name for the element that holds the items,
        // so we set this "aliases" for the Items property
        public List<T> Ids { get { return Items; } set { Items = value; } }
        public List<T> Users { get { return Items; } set { Items = value; } }
        public List<T> Startups { get { return Items; } set { Items = value; } }
    }


}
