using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static B_Utility.Common.Global_Utility;

namespace B_Utility.Common
{
    public class ApiResponse<T>
    {
        public ResponseCode Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
