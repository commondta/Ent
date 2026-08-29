using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class PMSUser : BaseModel
    {
        public int EMP_CODE { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordKey { get; set; }
        public string NIC_NO { get; set; }
        public string EMP_FULL_NAME { get; set; }
        public string EMP_FATHER_NAM { get; set; }
        public string DESIG_DESC { get; set; }
        public string DEPARTMENT_DESC { get; set; }
        public DateTime EMP_BIRTH_DATE { get; set; }
        public string SHIFT_DESC { get; set; }
        public string JOINING_DATE { get; set; }
        public string EMP_BANK_ACC_NO { get; set; }
        public string PAY_ORG_DESC { get; set; }
        public string PAY_CC_DESC { get; set; }
        public int Manager_Id { get; set; }
    }
}
