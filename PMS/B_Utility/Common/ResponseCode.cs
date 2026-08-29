using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Utility.Common
{
    public enum ResponseCode
    {
        Success = 100,
        Error = 0,
        BadRequest = 1,
        NotFound = 2,
        Confirmation = 3,
        Conflict = 4,
        RecordNotFound = 101,
    }
}
