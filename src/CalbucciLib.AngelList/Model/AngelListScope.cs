using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.AngelList.Model
{
    [Flags]
    public enum AngelListScope
    {

        Comment = 0x01,
        Email = 0x02,
        Message = 0x04,
    }
}
