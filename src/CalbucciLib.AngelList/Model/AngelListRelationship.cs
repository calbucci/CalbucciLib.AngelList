using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListRelationship
    {
        public AngelListRelationshipItem Source { get; set; }
        public AngelListRelationshipItem Target { get; set; }


    }

    public class AngelListRelationshipItem
    {
        public int Id { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
