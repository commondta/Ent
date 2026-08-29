using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_HCC.Models
{
    public class FormulaMasterChildModel
    {
        public int id { get; set; }
        public int ParentID { get; set; }
        public string PayCode { get; set; }
        public string AmtHigherLimit { get; set; }
        public string AmtLowerLimit { get; set; }
        public string Percentages { get; set; }
        public string Remarks { get; set; }
    }
}