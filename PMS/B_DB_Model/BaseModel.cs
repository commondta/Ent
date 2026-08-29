using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
        public class BaseModel
        {
            [Key]
            public int Id { get; set; }

            public bool IsActive { get; set; }

            public bool IsDeleted { get; set; }

            public DateTime CreatedOn { get; set; }
         
            public int? CreatedBy { get; set; }

            public DateTime LastModified { get; set; }

            public int? ModifiedBy { get; set; }

            public string? LastModifiedUserName { get;set; }
        }
}
