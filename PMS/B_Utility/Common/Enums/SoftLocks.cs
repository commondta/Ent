using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Utility.Common.Enums
{
    public enum SoftLocks
    {
        No_Transfer = 1,
        Stop_Communication = 2,
        Stop_Correspondence = 3,
        Generate_Dues_Stop_Comm_Corr = 4,
        Stop_Dues_Generation = 5
    }
}
