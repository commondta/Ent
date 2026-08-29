using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class RenumberHistery : BaseModel
    {
        public string? CurrentPropertyRegistrationNo { get; set; }
        public string? CurrentPropertyPropertyNo { get; set; }
        public string? CurrentPropertyCNIC { get; set; }
        public string? CurrentPropertyMemberName { get; set; }
        public string? CurrentPropertyMemberAddress { get; set; }
        public string? CurrentPropertyMemberMobile { get; set; }
        public string? CurrentPropertyBlock { get; set; }
        public string? CurrentPropertyCategory { get; set; }
        public string? CurrentPropertySize { get; set; }
        public string? ProposedPropertyPropertyNo { get; set; }
        public string? ProposedPropertyBlock { get; set; }
        public string? ProposedPropertyCategory { get; set; }
        public string? ProposedPropertySize { get; set; }
    }
}
