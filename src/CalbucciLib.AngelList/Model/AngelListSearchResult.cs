using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.AngelList.Model
{
    public class AngelListSearchResult
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public AngelListEntityType Type { get; set; }
        
    }
}
