using B_DB_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMemberUploader
{
    
        public class AppDbContext : DbContext
        {
            public DbSet<MemberProfile> MemberProfile { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server =WASEEM-HCCLABS\\SQLEXPRESS; Database=DHA_Test;TrustServerCertificate=True; Integrated Security = true;");
            }
        }

}
