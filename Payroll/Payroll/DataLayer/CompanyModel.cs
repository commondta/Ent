using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_HCC.Models
{
    public class CompanyModel
    {
        public int id { get; set; }
        public string CompanyName { get; set; }
        public string TaxNo { get; set; }
        public string ESINo { get; set; }
        public string AnnualDays { get; set; }
        public string Address { get; set; }
        public string Active { get; set; }
    }
}