using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListStatus
    {
        public string Message { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
    }
}
